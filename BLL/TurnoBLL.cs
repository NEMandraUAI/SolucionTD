using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Seguridad;

namespace BLL
{
    public class TurnoBLL
    {
        private TurnoDAL _turnoDAL = new TurnoDAL();
        private IntegridadDAL _integridadDAL = new IntegridadDAL();
        private RegistroBLL _registroBLL = new RegistroBLL();
        private SocioDAL _socioDAL = new SocioDAL();
        private SocioBLL _socioBLL = new SocioBLL();

        public void AsentarReserva(TurnoBE turno)
        {
            DateTime f = turno.FechaHora;
            turno.FechaHora = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, 0);
            if (turno.FechaHora <= DateTime.Now)
                throw new Exception("La fecha y hora del turno deben ser futuras.");
            SocioBE socio = turno.Socio;
            if (socio == null || string.IsNullOrWhiteSpace(socio.DNI))
            {
                socio = _socioDAL.LeerTodos().FirstOrDefault(s => s.ID_Socio == turno.ID_Socio);
            }
            _socioBLL.ValidarPlanVigenteParaFecha(socio, turno.FechaHora, "reservar turno nutricional");
            turno.Estado = "Pendiente";
            turno.NroTurno = _turnoDAL.InsertarTurno(turno);
            turno.DVH = DVManager.CalcularDVH(turno);
            _turnoDAL.ActualizarDVH(turno.NroTurno, turno.DVH);
            List<string> listaDVH = _turnoDAL.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            _integridadDAL.ActualizarDVV("Turno", nuevoDVV);
            UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
            _registroBLL.RegistrarEvento($"Turno reservado (Nro: {turno.NroTurno}) para el socio ID {turno.ID_Socio}", usuarioActual, "INFO");
        }

        public void CambiarEstadoTurno(TurnoBE turno, string nuevoEstado)
        {
            if (string.Equals(nuevoEstado, "Asistió", StringComparison.OrdinalIgnoreCase))
            {
                SocioBE socio = _socioDAL.LeerTodos().FirstOrDefault(s => s.ID_Socio == turno.ID_Socio);
                _socioBLL.ValidarPlanVigenteAlDiaDeHoy(socio, "registrar asistencia a consulta nutricional");
            }
            turno.Estado = nuevoEstado;
            turno.DVH = DVManager.CalcularDVH(turno);
            _turnoDAL.ActualizarEstado(turno.NroTurno, nuevoEstado, turno.DVH);
            List<string> listaDVH = _turnoDAL.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            _integridadDAL.ActualizarDVV("Turno", nuevoDVV);
            UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
            _registroBLL.RegistrarEvento($"Estado de Turno {turno.NroTurno} modificado a: {nuevoEstado}", usuarioActual, "INFO");
        }

        public List<TurnoBE> ObtenerAgenda(DateTime fecha)
        {
            return _turnoDAL.ConsultarTurnosPorFecha(fecha);
        }

        public void ValidarMargenDeTiempo(int idNutricionista, DateTime fechaHoraSolicitada)
        {
            List<TurnoBE> turnosDelDia = _turnoDAL.ObtenerTurnosPorNutricionistaYFecha(idNutricionista, fechaHoraSolicitada);
            foreach (TurnoBE turnoExistente in turnosDelDia)
            {
                double diferenciaMinutos = Math.Abs((turnoExistente.FechaHora - fechaHoraSolicitada).TotalMinutes);
                if (diferenciaMinutos < 30)
                {
                    throw new Exception("El profesional ya tiene un turno asignado a las " + turnoExistente.FechaHora.ToString("HH:mm") + " hs. Debe dejar un margen mínimo de 30 minutos entre consultas.");
                }
            }
        }

        public List<TurnoBE> ObtenerTurnosPendientesPorSocio(int idSocio)
        {
            return _turnoDAL.ObtenerTurnosPendientesPorSocio(idSocio);
        }

        public SugerenciaTurnoBE ObtenerSugerenciaPredictiva(int idSocio, List<UsuarioBE> nutricionistasDisponibles)
        {
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID > 0)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            var dic = new IdiomaDAL().ObtenerTraduccionesPorFormulario(idIdioma, "NegocioBLL");
            if (idSocio <= 0)
            {
                return new SugerenciaTurnoBE
                {
                    TieneSugerencia = false,
                    Mensaje = string.Empty
                };
            }
            List<TurnoBE> historialSocio = _turnoDAL.LeerTodos()
                .Where(t => t.ID_Socio == idSocio)
                .ToList();
            if (historialSocio.Count == 0)
            {
                return new SugerenciaTurnoBE
                {
                    TieneSugerencia = false,
                    Mensaje = dic.ContainsKey("predSinHistorial")
                        ? dic["predSinHistorial"]
                        : "ℹ️ Socio sin historial previo de turnos nutricionales. Seleccione día y horario manualmente."
                };
            }
            var turnosBase = historialSocio.Where(t => string.Equals(t.Estado, "Asistió", StringComparison.OrdinalIgnoreCase)).ToList();
            if (turnosBase.Count == 0)
            {
                turnosBase = historialSocio;
            }
            var grupoDia = turnosBase
                .GroupBy(t => t.FechaHora.DayOfWeek)
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Max(x => x.FechaHora))
                .First();
            DayOfWeek diaPreferido = grupoDia.Key;
            int coincidenciasDia = grupoDia.Count();
            var turnosEnDiaPreferido = grupoDia.ToList();
            var grupoHora = turnosEnDiaPreferido
                .GroupBy(t => new TimeSpan(t.FechaHora.Hour, t.FechaHora.Minute, 0))
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Max(x => x.FechaHora))
                .First();
            TimeSpan horaPreferida = grupoHora.Key;
            int coincidenciasHora = turnosBase.Count(t => t.FechaHora.Hour == horaPreferida.Hours && t.FechaHora.Minute == horaPreferida.Minutes);
            int idNutriPreferido = turnosBase
                .GroupBy(t => t.ID_Nutricionista)
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Max(x => x.FechaHora))
                .First().Key;
            UsuarioBE nutriElegido = null;
            if (nutricionistasDisponibles != null && nutricionistasDisponibles.Count > 0)
            {
                nutriElegido = nutricionistasDisponibles.FirstOrDefault(u => u.ID == idNutriPreferido)
                               ?? nutricionistasDisponibles.First();
                idNutriPreferido = nutriElegido.ID;
            }
            DateTime fechaCandidata = CalcularProximaFechaDia(diaPreferido, horaPreferida);
            DateTime fechaHoraFinal = BuscarSlotDisponible(idNutriPreferido, fechaCandidata, horaPreferida, out bool fueDesplazado);
            string nombreDia = TraducirDiaSemana(fechaHoraFinal.DayOfWeek);
            string infoNutri = nutriElegido != null ? $" ({nutriElegido.Nombre})" : "";
            string notaDisponibilidad = fueDesplazado
                ? (dic.ContainsKey("predNotaDesplazado") ? dic["predNotaDesplazado"] : " (Horario habitual ocupado; se sugiere el turno libre más cercano)")
                : (dic.ContainsKey("predNotaLibre") ? dic["predNotaLibre"] : " (Horario libre verificado)");
            string plantillaSug = dic.ContainsKey("predSugerencia")
                ? dic["predSugerencia"]
                : "💡 Sugerencia Predictiva: {0} {1} a las {2} hs{3}.\r\nCoincide con su patrón histórico ({4} de {5} turnos en {0}){6}.";
            string mensajeFormateado;
            try
            {
                mensajeFormateado = string.Format(plantillaSug, nombreDia, fechaHoraFinal.ToString("dd/MM/yyyy"), fechaHoraFinal.ToString("HH:mm"), infoNutri, coincidenciasDia, turnosBase.Count, notaDisponibilidad);
            }
            catch
            {
                mensajeFormateado = $"{nombreDia} {fechaHoraFinal:dd/MM/yyyy HH:mm}{infoNutri}";
            }
            return new SugerenciaTurnoBE
            {
                TieneSugerencia = true,
                FechaHoraSugerida = fechaHoraFinal,
                ID_NutricionistaSugerido = idNutriPreferido,
                NombreNutricionistaSugerido = nutriElegido != null ? nutriElegido.Nombre : "",
                CoincidenciasDia = coincidenciasDia,
                CoincidenciasHora = coincidenciasHora,
                TotalTurnosAnalizados = turnosBase.Count,
                Mensaje = mensajeFormateado
            };
        }

        private DateTime CalcularProximaFechaDia(DayOfWeek diaObjetivo, TimeSpan horaObjetivo)
        {
            DateTime hoy = DateTime.Today;
            int diasFaltantes = ((int)diaObjetivo - (int)hoy.DayOfWeek + 7) % 7;
            if (diasFaltantes == 0 && DateTime.Now.TimeOfDay >= horaObjetivo)
            {
                diasFaltantes = 7;
            }
            return hoy.AddDays(diasFaltantes);
        }

        private DateTime BuscarSlotDisponible(int idNutricionista, DateTime fechaBase, TimeSpan horaPreferida, out bool fueDesplazado)
        {
            fueDesplazado = false;
            DateTime candidatoExacto = fechaBase.Date.Add(horaPreferida);
            if (EstaHorarioDisponible(idNutricionista, candidatoExacto))
            {
                return candidatoExacto;
            }
            fueDesplazado = true;
            int[] desplazamientosMinutos = { 30, -30, 60, -60, 90 };
            foreach (int offset in desplazamientosMinutos)
            {
                DateTime alternativo = candidatoExacto.AddMinutes(offset);
                if (alternativo > DateTime.Now && alternativo.Hour >= 8 && alternativo.Hour <= 21)
                {
                    if (EstaHorarioDisponible(idNutricionista, alternativo))
                    {
                        return alternativo;
                    }
                }
            }
            DateTime semanaSiguiente = candidatoExacto.AddDays(7);
            return semanaSiguiente;
        }

        private bool EstaHorarioDisponible(int idNutricionista, DateTime fechaHoraSolicitada)
        {
            if (fechaHoraSolicitada <= DateTime.Now) return false;
            List<TurnoBE> turnosDelDia = _turnoDAL.ObtenerTurnosPorNutricionistaYFecha(idNutricionista, fechaHoraSolicitada);
            foreach (TurnoBE t in turnosDelDia)
            {
                if (Math.Abs((t.FechaHora - fechaHoraSolicitada).TotalMinutes) < 30)
                {
                    return false;
                }
            }
            return true;
        }

        private string TraducirDiaSemana(DayOfWeek dia)
        {
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            string cultura = idIdioma == 2 ? "en-US" : (idIdioma == 3 ? "pt-BR" : (idIdioma == 5 ? "it-IT" : "es-AR"));
            string nombre = new CultureInfo(cultura).DateTimeFormat.GetDayName(dia);
            return char.ToUpper(nombre[0]) + nombre.Substring(1);
        }
    }
}