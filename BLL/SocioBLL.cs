using BE;
using DAL;
using Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SocioBLL
    {
        SocioDAL dal = new SocioDAL();
        PlanSuscripcionDAL planSuscripcionDAL = new PlanSuscripcionDAL();
        ComprobantePagoDAL comprobanteDAL = new ComprobantePagoDAL();
        TurnoDAL turnoDAL = new TurnoDAL();
        RutinaEntrenamientoDAL rutinaDAL = new RutinaEntrenamientoDAL();
        PlanNutricionalDAL planNutricionalDAL = new PlanNutricionalDAL();
        IntegridadDAL intDal = new IntegridadDAL();
        IdiomaDAL idiomaDAL = new IdiomaDAL();
        RegistroBLL regBll = new RegistroBLL();

        public SocioBE ConsultarSocio(string dni)
        {
            return dal.ObtenerPorDNI(dni);
        }

        public void RegistrarSocio(SocioBE socio)
        {
            if (socio == null || string.IsNullOrWhiteSpace(socio.DNI))
                throw new Exception("El DNI es un campo obligatorio.");
            string dniLimpio = socio.DNI.Trim();
            if (dniLimpio.Length < 7 || dniLimpio.Length > 8 || !dniLimpio.All(char.IsDigit))
                throw new Exception("El DNI debe contener únicamente entre 7 y 8 dígitos numéricos.");
            if (string.IsNullOrWhiteSpace(socio.Nombre) || string.IsNullOrWhiteSpace(socio.Apellido))
                throw new Exception("El nombre y el apellido del socio son obligatorios.");
            socio.DNI = dniLimpio;
            if (dal.ObtenerPorDNI(socio.DNI) != null)
                throw new Exception("Ya existe un socio registrado con este DNI.");
            socio.EstadoActivo = true;
            socio.DVH = string.Empty;
            dal.InsertarSocio(socio);
            socio.DVH = DVManager.CalcularDVH(socio);
            intDal.ActualizarDVHRegistro("Socio", socio.ID_Socio, socio.DVH);
            RecalcularDVVSocio();
            regBll.RegistrarEvento($"Alta de nuevo socio. DNI: {socio.DNI}", SessionManager.Instancia.UsuarioActual, "ALTA");
        }

        public void ActualizarPlanSocio(SocioBE socio, PlanSuscripcionBE nuevoPlan)
        {
            if (socio != null && socio.ID_Socio > 0)
            {
                if (ObtenerEstadoVigenciaPlan(socio, out DateTime? fechaVenc, out string nomPlanActual, out int diasRest, out _))
                {
                    throw new Exception($"El socio {socio.Nombre} {socio.Apellido} (DNI {socio.DNI}) ya posee un plan vigente activo ({nomPlanActual}) con cobertura hasta el {fechaVenc:dd/MM/yyyy} ({diasRest} día(s) restantes).\r\n\r\nNo se permite abonar ni superponer un nuevo plan hasta que venza el actual.");
                }
            }
            socio.PlanAsociado = nuevoPlan;
            socio.EstadoActivo = true;
            socio.DVH = DVManager.CalcularDVH(socio);
            dal.ActualizarSocio(socio);
            RecalcularDVVSocio();
            regBll.RegistrarEvento($"Se vinculó un nuevo plan al socio DNI: {socio.DNI}", SessionManager.Instancia.UsuarioActual, "INFO");
        }

        private void RecalcularDVVSocio()
        {
            List<string> listaDVH = dal.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            intDal.ActualizarDVV("Socio", nuevoDVV);
        }

        private Dictionary<string, string> ObtenerDiccionarioNegocio()
        {
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID > 0)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            return new IdiomaDAL().ObtenerTraduccionesPorFormulario(idIdioma, "NegocioBLL");
        }

        private string TN(Dictionary<string, string> dic, string clave, string porDefecto, params object[] args)
        {
            string plantilla = (dic != null && dic.ContainsKey(clave)) ? dic[clave] : porDefecto;
            if (args == null || args.Length == 0) return plantilla;
            try
            {
                return string.Format(plantilla, args);
            }
            catch
            {
                return plantilla;
            }
        }

        public bool ObtenerEstadoVigenciaPlan(
            SocioBE socio,
            out DateTime? fechaVencimiento,
            out string nombrePlan,
            out int diasRestantes,
            out string mensajeEstado)
        {
            var dic = ObtenerDiccionarioNegocio();
            fechaVencimiento = null;
            nombrePlan = TN(dic, "vigSinPlan", "Sin Plan");
            diasRestantes = -999;
            if (socio == null || socio.ID_Socio <= 0)
            {
                mensajeEstado = TN(dic, "vigSocioInvalido", "Socio no válido.");
                return false;
            }
            if (!socio.EstadoActivo)
            {
                mensajeEstado = TN(dic, "vigSocioInactivo", "El socio se encuentra dado de baja / inactivo en el padrón.");
                return false;
            }
            List<PlanSuscripcionBE> catalogoPlanes = planSuscripcionDAL.ListarPlanes();
            int codPlan = socio.PlanAsociado != null ? socio.PlanAsociado.CodigoPlan : 0;
            PlanSuscripcionBE plan = catalogoPlanes.FirstOrDefault(p => p.CodigoPlan == codPlan);
            int duracionDias = plan != null ? plan.DuracionDias : 30;
            nombrePlan = plan != null ? plan.Nombre : TN(dic, "vigPlanEstandar", "Plan Estándar");
            List<ComprobantePagoBE> pagosSocio = comprobanteDAL.ConsultarComprobantesFiltros(
                new DateTime(2000, 1, 1),
                DateTime.Now.AddYears(5),
                socio.DNI)
                .Where(c => c.Socio != null && c.Socio.ID_Socio == socio.ID_Socio)
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.NroComprobante)
                .ToList();
            if (pagosSocio.Count == 0)
            {
                mensajeEstado = TN(dic, "vigSinPagos", "El socio no registra comprobantes de pago de suscripción.");
                return false;
            }
            ComprobantePagoBE ultimoPago = pagosSocio.First();
            DateTime venc = ultimoPago.Fecha.Date.AddDays(duracionDias);
            fechaVencimiento = venc;
            diasRestantes = (int)(venc - DateTime.Today).TotalDays;
            if (diasRestantes < 0)
            {
                mensajeEstado = TN(dic, "vigCuotaVencida", "Cuota VENCIDA el {0} (hace {1} día(s) — {2}).", venc.ToString("dd/MM/yyyy"), Math.Abs(diasRestantes), nombrePlan);
                return false;
            }
            mensajeEstado = TN(dic, "vigPlanVigente", "Plan vigente ({0}) hasta el {1} ({2} día(s) restantes).", nombrePlan, venc.ToString("dd/MM/yyyy"), diasRestantes);
            return true;
        }

        public void ValidarPlanVigenteAlDiaDeHoy(SocioBE socio, string operacion)
        {
            if (!ObtenerEstadoVigenciaPlan(socio, out DateTime? fechaVenc, out string nomPlan, out int diasRest, out string msg))
            {
                throw new Exception($"{operacion}:\r\n{msg}");
            }
        }

        public void ValidarPlanVigenteParaFecha(SocioBE socio, DateTime fechaObjetivo, string operacion)
        {
            if (!ObtenerEstadoVigenciaPlan(socio, out DateTime? fechaVenc, out string nomPlan, out int diasRest, out string msg))
            {
                throw new Exception($"{operacion}:\r\n{msg}");
            }
            if (fechaVenc.HasValue && fechaObjetivo.Date > fechaVenc.Value.Date)
            {
                throw new Exception($"{operacion}:\r\n'{nomPlan}' ({fechaVenc.Value:dd/MM/yyyy}) < ({fechaObjetivo:dd/MM/yyyy}).");
            }
        }

        public List<AlertaDashboardBE> ObtenerAlertasDashboard(
            UsuarioBE usuarioActual,
            out int alertasRecepcion,
            out int alertasEntrenamiento,
            out int alertasNutricion)
        {
            alertasRecepcion = 0;
            alertasEntrenamiento = 0;
            alertasNutricion = 0;
            List<AlertaDashboardBE> alertas = new List<AlertaDashboardBE>();
            if (usuarioActual == null) return alertas;
            var dic = ObtenerDiccionarioNegocio();
            string nomAreaRec = TN(dic, "areaRecepcion", "Recepción");
            string nomAreaEnt = TN(dic, "areaEntrenamiento", "Entrenamiento");
            string nomAreaNut = TN(dic, "areaNutricion", "Nutrición");
            bool veRecepcion = usuarioActual.TienePermiso("VENDER_PLAN") ||
                               usuarioActual.TienePermiso("ASENTAR_TURNO") ||
                               usuarioActual.TienePermiso("REVISAR_AGENDA") ||
                               usuarioActual.TienePermiso("CONSULTAR_COMPROBANTES") ||
                               usuarioActual.TienePermiso("GESTION_ROLES");
            bool veEntrenamiento = usuarioActual.TienePermiso("ASIGNAR_RUTINA") ||
                                   usuarioActual.TienePermiso("CONSULTAR_SOCIO") ||
                                   usuarioActual.TienePermiso("GESTION_ROLES");
            bool veNutricion = usuarioActual.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL") ||
                               usuarioActual.TienePermiso("CONSULTAR_AGENDA") ||
                               usuarioActual.TienePermiso("CONSULTAR_PLANES_NUT") ||
                               usuarioActual.TienePermiso("GESTION_ROLES");
            List<SocioBE> sociosActivos = dal.LeerTodos().Where(s => s.EstadoActivo).ToList();
            if (veRecepcion)
            {
                List<PlanSuscripcionBE> catalogoPlanes = planSuscripcionDAL.ListarPlanes();
                List<ComprobantePagoBE> todosLosPagos = comprobanteDAL.ConsultarComprobantesFiltros(
                    new DateTime(2000, 1, 1),
                    DateTime.Now.AddYears(5),
                    null);
                var vencimientosCriticos = new List<(SocioBE Socio, PlanSuscripcionBE Plan, DateTime FechaVenc, int DiasRestantes)>();
                foreach (var socio in sociosActivos)
                {
                    int codPlan = socio.PlanAsociado != null ? socio.PlanAsociado.CodigoPlan : 0;
                    PlanSuscripcionBE plan = catalogoPlanes.FirstOrDefault(p => p.CodigoPlan == codPlan);
                    int duracionDias = plan != null ? plan.DuracionDias : 30;
                    ComprobantePagoBE ultimoPago = todosLosPagos
                        .Where(c => c.Socio != null && c.Socio.ID_Socio == socio.ID_Socio)
                        .OrderByDescending(c => c.Fecha)
                        .ThenByDescending(c => c.NroComprobante)
                        .FirstOrDefault();
                    if (ultimoPago != null)
                    {
                        DateTime fechaVencimiento = ultimoPago.Fecha.Date.AddDays(duracionDias);
                        int diasRestantes = (int)(fechaVencimiento - DateTime.Today).TotalDays;
                        if (diasRestantes <= 5)
                        {
                            vencimientosCriticos.Add((socio, plan, fechaVencimiento, diasRestantes));
                        }
                    }
                }
                if (vencimientosCriticos.Count > 0)
                {
                    int porVencer = vencimientosCriticos.Count(v => v.DiasRestantes >= 0);
                    int vencidos = vencimientosCriticos.Count(v => v.DiasRestantes < 0);
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = nomAreaRec,
                        Prioridad = vencidos > 0 ? "ALTA" : "MEDIA",
                        Titulo = TN(dic, "alertaVencResumenTit", "Hay {0} socio(s) con plan próximo a vencer (≤ 5 días) o vencido", vencimientosCriticos.Count),
                        Detalle = TN(dic, "alertaVencResumenDet", "{0} vencen esta semana y {1} presentan cuota vencida pendiente de renovación.", porVencer, vencidos),
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                    alertasRecepcion++;
                    foreach (var item in vencimientosCriticos.OrderBy(v => v.DiasRestantes))
                    {
                        string nomPlan = item.Plan != null ? item.Plan.Nombre : "Plan";
                        string detTexto = item.DiasRestantes < 0
                            ? TN(dic, "alertaVencItemDetVencido", "{0} — Venció hace {1} día(s) ({2}). Gestionar renovación en Recepción.", nomPlan, Math.Abs(item.DiasRestantes), item.FechaVenc.ToString("dd/MM/yyyy"))
                            : TN(dic, "alertaVencItemDetPorVencer", "{0} — Vence en {1} día(s) ({2}). Gestionar renovación en Recepción.", nomPlan, item.DiasRestantes, item.FechaVenc.ToString("dd/MM/yyyy"));
                        alertas.Add(new AlertaDashboardBE
                        {
                            Area = nomAreaRec,
                            Prioridad = item.DiasRestantes < 0 ? "ALTA" : "MEDIA",
                            Titulo = TN(dic, "alertaVencItemTit", "Vencimiento de cuota: {0} (DNI {1})", $"{item.Socio.Nombre} {item.Socio.Apellido}", item.Socio.DNI),
                            Detalle = detTexto,
                            FechaReferencia = item.FechaVenc.ToString("dd/MM/yyyy")
                        });
                        alertasRecepcion++;
                    }
                }
                else
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = nomAreaRec,
                        Prioridad = "INFO",
                        Titulo = TN(dic, "alertaSuscAlDiaTit", "Suscripciones al día"),
                        Detalle = TN(dic, "alertaSuscAlDiaDet", "No hay socios activos con planes que venzan en los próximos 5 días."),
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                }
                List<TurnoBE> turnosHoy = turnoDAL.ConsultarTurnosPorFecha(DateTime.Today);
                if (turnosHoy.Count > 0)
                {
                    foreach (var t in turnosHoy.OrderBy(x => x.FechaHora))
                    {
                        alertas.Add(new AlertaDashboardBE
                        {
                            Area = nomAreaRec,
                            Prioridad = t.Estado == "Pendiente" ? "MEDIA" : "INFO",
                            Titulo = TN(dic, "alertaTurnoHoyTit", "Turno médico hoy a las {0} hs — Socio: {1}", t.FechaHora.ToString("HH:mm"), t.NombreSocio),
                            Detalle = TN(dic, "alertaTurnoHoyDet", "DNI: {0} | Profesional: {1} | Estado actual: {2}.", t.DNISocio, t.NombreNutricionista, t.Estado),
                            FechaReferencia = t.FechaHora.ToString("dd/MM/yyyy HH:mm")
                        });
                        alertasRecepcion++;
                    }
                }
                else
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = nomAreaRec,
                        Prioridad = "INFO",
                        Titulo = TN(dic, "alertaSinTurnosHoyTit", "Sin turnos nutricionales agendados para hoy"),
                        Detalle = TN(dic, "alertaSinTurnosHoyDet", "La agenda de consultorios médicos del día de la fecha se encuentra libre."),
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                }
            }
            if (veEntrenamiento)
            {
                List<RutinaEntrenamientoBE> todasLasRutinas = rutinaDAL.LeerTodos();
                var desactualizados = new List<(SocioBE Socio, DateTime? FechaUltima, int DiasSinActualizar)>();
                foreach (var socio in sociosActivos)
                {
                    var rutinasSocio = todasLasRutinas
                        .Where(r => r.Socio != null && r.Socio.ID_Socio == socio.ID_Socio)
                        .OrderByDescending(r => r.FechaCreacion)
                        .ThenByDescending(r => r.CodigoRutina)
                        .ToList();
                    if (rutinasSocio.Count == 0)
                    {
                        desactualizados.Add((socio, null, 999));
                    }
                    else
                    {
                        DateTime fechaUltima = rutinasSocio.First().FechaCreacion;
                        int diasAntiguedad = (int)(DateTime.Today - fechaUltima.Date).TotalDays;
                        if (diasAntiguedad > 90)
                        {
                            desactualizados.Add((socio, fechaUltima, diasAntiguedad));
                        }
                    }
                }
                int cantMasTresMeses = desactualizados.Count(d => d.FechaUltima.HasValue);
                int cantSinRutina = desactualizados.Count(d => !d.FechaUltima.HasValue);
                if (desactualizados.Count > 0)
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = nomAreaEnt,
                        Prioridad = "ALTA",
                        Titulo = TN(dic, "alertaRutinaResumenTit", "Hay {0} socio(s) que requieren actualización o asignación de rutina", desactualizados.Count),
                        Detalle = TN(dic, "alertaRutinaResumenDet", "{0} no actualizan su rutina hace más de 3 meses (>90 días) y {1} no poseen rutina activa.", cantMasTresMeses, cantSinRutina),
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                    alertasEntrenamiento++;
                    foreach (var d in desactualizados.OrderByDescending(x => x.DiasSinActualizar))
                    {
                        string det = d.FechaUltima.HasValue
                            ? TN(dic, "alertaRutinaItemDetAntigua", "Última rutina creada el {0} (hace {1} días). Supera el límite de 3 meses.", d.FechaUltima.Value.ToString("dd/MM/yyyy"), d.DiasSinActualizar)
                            : TN(dic, "alertaRutinaItemDetSinRutina", "El socio se encuentra activo pero aún no registra ninguna rutina de entrenamiento.");
                        alertas.Add(new AlertaDashboardBE
                        {
                            Area = nomAreaEnt,
                            Prioridad = "MEDIA",
                            Titulo = TN(dic, "alertaRutinaItemTit", "Revisión deportiva pendiente: {0} (DNI {1})", $"{d.Socio.Nombre} {d.Socio.Apellido}", d.Socio.DNI),
                            Detalle = det,
                            FechaReferencia = d.FechaUltima.HasValue ? d.FechaUltima.Value.ToString("dd/MM/yyyy") : "-"
                        });
                        alertasEntrenamiento++;
                    }
                }
                else
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = nomAreaEnt,
                        Prioridad = "INFO",
                        Titulo = TN(dic, "alertaRutinasAlDiaTit", "Planificaciones deportivas al día"),
                        Detalle = TN(dic, "alertaRutinasAlDiaDet", "Todos los socios activos ({0}) tienen rutinas actualizadas dentro de los últimos 3 meses.", sociosActivos.Count),
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                }
            }
            if (veNutricion)
            {
                List<TurnoBE> turnosHoyNutri = turnoDAL.ConsultarTurnosPorFecha(DateTime.Today);
                if (!usuarioActual.TienePermiso("GESTION_ROLES"))
                {
                    turnosHoyNutri = turnosHoyNutri.Where(t => t.ID_Nutricionista == usuarioActual.ID).ToList();
                }
                int pendientesHoy = turnosHoyNutri.Count(t => t.Estado == "Pendiente");
                if (pendientesHoy > 0)
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = nomAreaNut,
                        Prioridad = "MEDIA",
                        Titulo = TN(dic, "alertaNutriTurnosPendTit", "Tiene {0} consulta(s) nutricional(es) pendiente(s) para hoy", pendientesHoy),
                        Detalle = TN(dic, "alertaNutriTurnosPendDet", "Próximo turno a las {0} hs.", turnosHoyNutri.Where(t => t.Estado == "Pendiente").Min(t => t.FechaHora).ToString("HH:mm")),
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                    alertasNutricion++;
                }
                List<PlanNutricionalBE> todosLosPlanesNut = planNutricionalDAL.LeerTodos();
                foreach (var socio in sociosActivos)
                {
                    var ultimoPlan = todosLosPlanesNut
                        .Where(p => p.ID_Socio == socio.ID_Socio)
                        .OrderByDescending(p => p.FechaCreacion)
                        .ThenByDescending(p => p.CodigoPlan)
                        .FirstOrDefault();
                    if (ultimoPlan != null && ultimoPlan.DatosBiometricos != null && ultimoPlan.DatosBiometricos.IndiceMasaCorporal > 0)
                    {
                        decimal imc = ultimoPlan.DatosBiometricos.IndiceMasaCorporal;
                        if (imc < 18.5m || imc >= 30.0m)
                        {
                            alertas.Add(new AlertaDashboardBE
                            {
                                Area = nomAreaNut,
                                Prioridad = "ALTA",
                                Titulo = TN(dic, "alertaNutriImcTit", "Seguimiento clínico prioritario: {0} (DNI {1})", $"{socio.Nombre} {socio.Apellido}", socio.DNI),
                                Detalle = TN(dic, "alertaNutriImcDet", "Presenta {0} (IMC {1} | {2} kg) en su evaluación del {3}.", ultimoPlan.DatosBiometricos.DiagnosticoIMC, imc.ToString("0.00"), ultimoPlan.DatosBiometricos.PesoKg.ToString("0.#"), ultimoPlan.FechaCreacion.ToString("dd/MM/yyyy")),
                                FechaReferencia = ultimoPlan.FechaCreacion.ToString("dd/MM/yyyy")
                            });
                            alertasNutricion++;
                        }
                    }
                }
            }
            return alertas;
        }

        public List<SocioRiesgoBE> ObtenerSociosEnRiesgoDeFuga(out decimal tasaChurnPorcentaje, out int cantidadRiesgoAlto)
        {
            tasaChurnPorcentaje = 0m;
            cantidadRiesgoAlto = 0;
            UsuarioBE usuarioSesion = SessionManager.Instancia.UsuarioActual;
            if (usuarioSesion == null || !usuarioSesion.TienePermiso("GESTION_SOCIOS_RIESGO"))
            {
                return new List<SocioRiesgoBE>();
            }
            var dic = ObtenerDiccionarioNegocio();
            List<SocioRiesgoBE> resultado = new List<SocioRiesgoBE>();
            List<SocioBE> todosLosSocios = dal.LeerTodos();
            if (todosLosSocios == null || todosLosSocios.Count == 0)
                return resultado;
            List<RutinaEntrenamientoBE> todasLasRutinas = rutinaDAL.LeerTodos();
            List<TurnoBE> todosLosTurnos = turnoDAL.LeerTodos();
            foreach (var socio in todosLosSocios)
            {
                int puntaje = 0;
                List<string> motivos = new List<string>();
                List<DateTime> fechasActividad = new List<DateTime>();
                bool planVigente = ObtenerEstadoVigenciaPlan(
                    socio,
                    out DateTime? fechaVencimiento,
                    out string nombrePlan,
                    out int diasRestantes,
                    out string msgVigencia);
                if (fechaVencimiento.HasValue)
                {
                    fechasActividad.Add(fechaVencimiento.Value.AddDays(-30));
                }
                if (!socio.EstadoActivo)
                {
                    puntaje += 85;
                    motivos.Add(TN(dic, "churnMotivoInactivo", "Socio inactivo en el padrón"));
                }
                else if (!fechaVencimiento.HasValue)
                {
                    puntaje += 75;
                    motivos.Add(TN(dic, "churnMotivoSinPagos", "No registra pagos de suscripción"));
                }
                else if (diasRestantes <= -5)
                {
                    puntaje += 70;
                    motivos.Add(TN(dic, "churnMotivoPlanVencido", "Plan '{0}' venció hace {1} día(s) ({2})", nombrePlan, Math.Abs(diasRestantes), fechaVencimiento.Value.ToString("dd/MM/yyyy")));
                }
                else if (diasRestantes < 0)
                {
                    puntaje += 50;
                    motivos.Add(TN(dic, "churnMotivoPlanVencido", "Plan '{0}' venció hace {1} día(s) ({2})", nombrePlan, Math.Abs(diasRestantes), fechaVencimiento.Value.ToString("dd/MM/yyyy")));
                }
                else if (diasRestantes <= 5)
                {
                    puntaje += 30;
                    motivos.Add(TN(dic, "churnMotivoPlanPorVencer", "Plan '{0}' vence en {1} día(s) ({2})", nombrePlan, diasRestantes, fechaVencimiento.Value.ToString("dd/MM/yyyy")));
                }
                var rutinasSocio = todasLasRutinas
                    .Where(r => r.Socio != null && r.Socio.ID_Socio == socio.ID_Socio)
                    .OrderByDescending(r => r.FechaCreacion)
                    .ThenByDescending(r => r.CodigoRutina)
                    .ToList();
                if (rutinasSocio.Count == 0)
                {
                    puntaje += 20;
                    motivos.Add(TN(dic, "churnMotivoSinRutina", "Sin rutina de entrenamiento asignada"));
                }
                else
                {
                    DateTime ultimaRutinaFecha = rutinasSocio.First().FechaCreacion;
                    fechasActividad.Add(ultimaRutinaFecha);
                    int diasSinActualizarRutina = (int)(DateTime.Today - ultimaRutinaFecha.Date).TotalDays;
                    if (diasSinActualizarRutina > 90)
                    {
                        puntaje += 35;
                        motivos.Add(TN(dic, "churnMotivoRutinaAntigua", "Sin actualización de rutina hace {0} días", diasSinActualizarRutina));
                    }
                    else if (diasSinActualizarRutina > 45)
                    {
                        puntaje += 15;
                        motivos.Add(TN(dic, "churnMotivoRutinaAntigua", "Sin actualización de rutina hace {0} días", diasSinActualizarRutina));
                    }
                }
                var turnosSocio = todosLosTurnos
                    .Where(t => t.ID_Socio == socio.ID_Socio)
                    .OrderByDescending(t => t.FechaHora)
                    .ToList();
                if (turnosSocio.Count > 0)
                {
                    fechasActividad.Add(turnosSocio.First().FechaHora);
                    int ausencias = turnosSocio.Count(t => string.Equals(t.Estado, "Ausente", StringComparison.OrdinalIgnoreCase));
                    if (ausencias > 0)
                    {
                        puntaje += Math.Min(25, ausencias * 12);
                        motivos.Add(TN(dic, "churnMotivoAusencias", "Registra {0} ausencia(s) en consultorio nutricional", ausencias));
                    }
                }
                if (fechaVencimiento.HasValue && diasRestantes <= -5 && puntaje < 60)
                {
                    puntaje = 70;
                }
                puntaje = Math.Clamp(puntaje, 5, 98);
                string nivelRiesgo;
                if (puntaje >= 60)
                {
                    nivelRiesgo = TN(dic, "churnNivelAlto", "Riesgo de Abandono Alto");
                    cantidadRiesgoAlto++;
                }
                else if (puntaje >= 30)
                {
                    nivelRiesgo = TN(dic, "churnNivelModerado", "Riesgo Moderado");
                }
                else
                {
                    nivelRiesgo = TN(dic, "churnNivelBajo", "Bajo (Fidelizado)");
                }
                if (motivos.Count == 0)
                {
                    motivos.Add(TN(dic, "churnMotivoAlDia", "Cuota al día (vence el {0}) y rutina activa.", fechaVencimiento.HasValue ? fechaVencimiento.Value.ToString("dd/MM/yyyy") : "-"));
                }
                DateTime? ultimaAct = fechasActividad.Count > 0 ? fechasActividad.Max() : (DateTime?)null;
                resultado.Add(new SocioRiesgoBE
                {
                    ID_Socio = socio.ID_Socio,
                    DNI = socio.DNI,
                    Socio = $"{socio.Nombre} {socio.Apellido}",
                    PlanActual = nombrePlan,
                    NivelRiesgo = nivelRiesgo,
                    PuntajeFuga = puntaje,
                    ProbabilidadFuga = $"{puntaje}%",
                    MotivoDetectado = string.Join(" | ", motivos),
                    UltimaActividad = ultimaAct.HasValue ? ultimaAct.Value.ToString("dd/MM/yyyy") : "-",
                    Telefono = socio.Telefono ?? string.Empty,
                    Email = socio.Email ?? string.Empty,
                    Contacto = $"{(string.IsNullOrWhiteSpace(socio.Telefono) ? "-" : socio.Telefono)} / {(string.IsNullOrWhiteSpace(socio.Email) ? "-" : socio.Email)}"
                });
            }
            tasaChurnPorcentaje = Math.Round((decimal)cantidadRiesgoAlto * 100m / todosLosSocios.Count, 1);
            return resultado
                .OrderByDescending(r => r.PuntajeFuga)
                .ThenBy(r => r.Socio)
                .ToList();
        }

        public string GenerarMensajeRetencion(SocioRiesgoBE socioRiesgo)
        {
            UsuarioBE usuarioSesion = SessionManager.Instancia.UsuarioActual;
            if (usuarioSesion == null || !usuarioSesion.TienePermiso("GESTION_SOCIOS_RIESGO"))
            {
                throw new Exception("Acceso denegado: No posee el permiso requerido (GESTION_SOCIOS_RIESGO) para contactar socios en riesgo.");
            }
            if (socioRiesgo == null) return string.Empty;
            var dic = ObtenerDiccionarioNegocio();
            return TN(
                dic,
                "churnMsgRetencion",
                "¡Hola {0}! Te contactamos del equipo de UAI-FIT. Notamos en nuestro sistema el siguiente estado en tu perfil: ({1}). Queremos acompañarte para que retomes tus objetivos deportivos: acercate a Recepción para renovar tu {2} y coordinar una actualización sin cargo de tu rutina. ¡Te esperamos!",
                socioRiesgo.Socio,
                socioRiesgo.MotivoDetectado,
                socioRiesgo.PlanActual);
        }
    }
}