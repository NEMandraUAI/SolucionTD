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
            if (idSocio <= 0)
            {
                return new SugerenciaTurnoBE
                {
                    TieneSugerencia = false,
                    Mensaje = "Ingrese el DNI de un socio activo para analizar su historial de turnos."
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
                    Mensaje = "ℹ️ Socio sin historial previo de turnos nutricionales. Seleccione día y horario manualmente."
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
            string infoNutri = nutriElegido != null ? $" con {nutriElegido.Nombre}" : "";
            string notaDisponibilidad = fueDesplazado
                ? " (Horario habitual ocupado; se sugiere el turno libre más cercano)"
                : " (Horario libre verificado)";
            return new SugerenciaTurnoBE
            {
                TieneSugerencia = true,
                FechaHoraSugerida = fechaHoraFinal,
                ID_NutricionistaSugerido = idNutriPreferido,
                NombreNutricionistaSugerido = nutriElegido != null ? nutriElegido.Nombre : "",
                CoincidenciasDia = coincidenciasDia,
                CoincidenciasHora = coincidenciasHora,
                TotalTurnosAnalizados = turnosBase.Count,
                Mensaje = $"💡 Sugerencia Predictiva: {nombreDia} {fechaHoraFinal:dd/MM/yyyy} a las {fechaHoraFinal:HH:mm} hs{infoNutri}.\r\n" +
                          $"Coincide con su patrón histórico ({coincidenciasDia} de {turnosBase.Count} turnos en {nombreDia}){notaDisponibilidad}."
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
            switch (dia)
            {
                case DayOfWeek.Monday: return "Lunes";
                case DayOfWeek.Tuesday: return "Martes";
                case DayOfWeek.Wednesday: return "Miércoles";
                case DayOfWeek.Thursday: return "Jueves";
                case DayOfWeek.Friday: return "Viernes";
                case DayOfWeek.Saturday: return "Sábado";
                case DayOfWeek.Sunday: return "Domingo";
                default: return dia.ToString();
            }
        }
    }
}