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

        internal static UsuarioBE ValidarActor(UnidadTrabajoSeguridadDAL unidad, params string[] permisos)
        {
            var sesion = SessionManager.Instancia.UsuarioActual;
            if (sesion == null) throw new UnauthorizedAccessException("Debe iniciar sesión para realizar esta operación.");
            new IntegridadBLL().VerificarIntegridadUsuario(unidad);
            var autor = new UsuarioDAL(unidad).ObtenerPorID(sesion.ID);
            if (autor == null || autor.Bloqueado) throw new UnauthorizedAccessException("La cuenta no se encuentra habilitada.");
            new PermisoBLL(unidad).LlenarPermisosDeUsuario(autor);
            if (!autor.TienePermiso("GESTION_ROLES") && !permisos.Any(autor.TienePermiso))
                throw new UnauthorizedAccessException("No posee el permiso requerido para esta operación.");
            return autor;
        }

        internal static void ValidarDVHSocio(SocioBE? socio)
        {
            if (socio == null) throw new ArgumentException("El socio seleccionado no existe.");
            if (socio.DVH != DVManager.CalcularDVH(socio))
                throw new InvalidOperationException("El socio fue alterado externamente. Se requiere revisión de integridad.");
        }

        public void AsentarReserva(TurnoBE turno)
        {
            ArgumentNullException.ThrowIfNull(turno);
            using var unidad = new UnidadTrabajoSeguridadDAL();
            var autor = ValidarActor(unidad, "ASENTAR_TURNO");
            IntegridadTransaccionalDAL.VerificarDVV(unidad, "Turno");
            var dal = new TurnoDAL(unidad);
            var fecha = new DateTime(turno.FechaHora.Year, turno.FechaHora.Month, turno.FechaHora.Day, turno.FechaHora.Hour, turno.FechaHora.Minute, 0);
            if (fecha <= DateTime.Now) throw new ArgumentException("La fecha y hora del turno deben ser futuras.");
            var socio = new SocioDAL(unidad).LeerTodos().SingleOrDefault(s => s.ID_Socio == turno.ID_Socio)
                ?? throw new ArgumentException("El socio no existe.");
            ValidarDVHSocio(socio);
            new SocioBLL(unidad).ValidarPlanVigenteParaFecha(socio, fecha, "reservar turno nutricional");
            var profesional = new UsuarioDAL(unidad).ObtenerPorID(turno.ID_Nutricionista);
            if (profesional == null || profesional.Bloqueado) throw new ArgumentException("El nutricionista seleccionado no está habilitado.");
            new PermisoBLL(unidad).LlenarPermisosDeUsuario(profesional);
            if (!profesional.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL")) throw new ArgumentException("El usuario seleccionado no es un nutricionista habilitado.");
            if (dal.ObtenerTurnosPendientesPorSocio(socio.ID_Socio).Count > 0)
                throw new InvalidOperationException("El socio ya tiene un turno pendiente. Resuelva ese turno antes de reservar otro.");
            if (dal.ExisteConflicto(profesional.ID, fecha))
                throw new InvalidOperationException("El profesional ya tiene un turno a menos de 30 minutos del horario solicitado.");
            var confirmado = new TurnoBE { FechaHora=fecha, Estado="Pendiente", ID_Socio=socio.ID_Socio, ID_Nutricionista=profesional.ID };
            confirmado.NroTurno = dal.InsertarTurno(confirmado);
            confirmado.DVH = DVManager.CalcularDVH(confirmado);
            dal.ActualizarDVH(confirmado.NroTurno, confirmado.DVH);
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "Turno");
            new RegistroBLL(unidad).RegistrarEvento($"Turno reservado (Nro: {confirmado.NroTurno}) para el socio ID {socio.ID_Socio}", autor);
            unidad.Confirmar();
            turno.NroTurno=confirmado.NroTurno; turno.FechaHora=fecha; turno.Estado=confirmado.Estado; turno.DVH=confirmado.DVH;
            turno.Socio=socio; turno.Nutricionista=profesional;
        }

        public void CambiarEstadoTurno(TurnoBE turno, string nuevoEstado)
        {
            ArgumentNullException.ThrowIfNull(turno);
            using var unidad = new UnidadTrabajoSeguridadDAL();
            var autor = ValidarActor(unidad, "ASENTAR_TURNO", "REVISAR_AGENDA", "ASIGNAR_PLAN_NUTRICIONAL");
            var confirmado = CambiarEstadoEnUnidad(unidad, turno.NroTurno, nuevoEstado, autor);
            unidad.Confirmar();
            turno.Estado=confirmado.Estado; turno.DVH=confirmado.DVH;
        }

        internal static TurnoBE CambiarEstadoEnUnidad(UnidadTrabajoSeguridadDAL unidad, int nroTurno, string nuevoEstado, UsuarioBE autor)
        {
            IntegridadTransaccionalDAL.VerificarDVV(unidad, "Turno");
            if (nuevoEstado != "Asistió" && nuevoEstado != "Ausente")
                throw new ArgumentException("El estado del turno debe ser Asistió o Ausente.");
            var dal = new TurnoDAL(unidad);
            var actual = dal.ObtenerPorID(nroTurno) ?? throw new ArgumentException("El turno seleccionado no existe.");
            if (actual.DVH != DVManager.CalcularDVH(actual)) throw new InvalidOperationException("El turno fue alterado externamente. Se requiere revisión de integridad.");
            if (actual.Estado != "Pendiente") throw new InvalidOperationException("Solo puede cambiarse un turno pendiente. Actualice la agenda.");
            if (actual.FechaHora > DateTime.Now) throw new InvalidOperationException("No puede registrar asistencia o ausencia antes de la fecha y hora del turno.");
            bool recepcion = autor.TienePermiso("GESTION_ROLES") || autor.TienePermiso("ASENTAR_TURNO") || autor.TienePermiso("REVISAR_AGENDA");
            if (!recepcion && actual.ID_Nutricionista != autor.ID)
                throw new UnauthorizedAccessException("Solo puede modificar los turnos que tiene asignados.");
            if (nuevoEstado == "Asistió")
            {
                var socio = new SocioDAL(unidad).LeerTodos().SingleOrDefault(s => s.ID_Socio == actual.ID_Socio);
                ValidarDVHSocio(socio);
                new SocioBLL(unidad).ValidarPlanVigenteParaFecha(socio, actual.FechaHora, "registrar asistencia nutricional");
            }
            actual.Estado=nuevoEstado; actual.DVH=DVManager.CalcularDVH(actual);
            dal.ActualizarEstado(actual.NroTurno, nuevoEstado, actual.DVH);
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "Turno");
            new RegistroBLL(unidad).RegistrarEvento($"Estado de Turno {actual.NroTurno} modificado a: {nuevoEstado}", autor);
            return actual;
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
