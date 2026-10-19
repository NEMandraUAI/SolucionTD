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
        RegistroBLL regBll = new RegistroBLL();

        public SocioBE ConsultarSocio(string dni)
        {
            return dal.ObtenerPorDNI(dni);
        }

        public void RegistrarSocio(SocioBE socio)
        {
            if (string.IsNullOrWhiteSpace(socio.DNI))
                throw new Exception("El DNI es un campo obligatorio.");
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

        public List<AlertaDashboardBE> ObtenerAlertasDashboard(UsuarioBE usuarioActual, out int alertasRecepcion, out int alertasEntrenamiento, out int alertasNutricion)
        {
            alertasRecepcion = 0;
            alertasEntrenamiento = 0;
            alertasNutricion = 0;
            List<AlertaDashboardBE> alertas = new List<AlertaDashboardBE>();
            if (usuarioActual == null) return alertas;
            bool veRecepcion = usuarioActual.TienePermiso("VENDER_PLAN") ||
                               usuarioActual.TienePermiso("ASENTAR_TURNO") ||
                               usuarioActual.TienePermiso("REVISAR_AGENDA") ||
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
                        Area = "Recepción",
                        Prioridad = vencidos > 0 ? "ALTA" : "MEDIA",
                        Titulo = $"Hay {vencimientosCriticos.Count} socio(s) con plan próximo a vencer (≤ 5 días) o vencido",
                        Detalle = $"{porVencer} vencen esta semana y {vencidos} presentan cuota vencida pendiente de renovación.",
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                    alertasRecepcion++;
                    foreach (var item in vencimientosCriticos.OrderBy(v => v.DiasRestantes))
                    {
                        string nomPlan = item.Plan != null ? item.Plan.Nombre : "Plan";
                        string estadoTexto = item.DiasRestantes < 0
                            ? $"Venció hace {Math.Abs(item.DiasRestantes)} día(s) ({item.FechaVenc:dd/MM/yyyy})"
                            : (item.DiasRestantes == 0 ? "Vence HOY" : $"Vence en {item.DiasRestantes} día(s) ({item.FechaVenc:dd/MM/yyyy})");
                        alertas.Add(new AlertaDashboardBE
                        {
                            Area = "Recepción",
                            Prioridad = item.DiasRestantes < 0 ? "ALTA" : "MEDIA",
                            Titulo = $"Vencimiento de cuota: {item.Socio.Nombre} {item.Socio.Apellido} (DNI {item.Socio.DNI})",
                            Detalle = $"{nomPlan} — {estadoTexto}. Gestionar renovación en Recepción.",
                            FechaReferencia = item.FechaVenc.ToString("dd/MM/yyyy")
                        });
                        alertasRecepcion++;
                    }
                }
                else
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = "Recepción",
                        Prioridad = "INFO",
                        Titulo = "Suscripciones al día",
                        Detalle = "No hay socios activos con planes que venzan en los próximos 5 días.",
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
                            Area = "Recepción",
                            Prioridad = t.Estado == "Pendiente" ? "MEDIA" : "INFO",
                            Titulo = $"Turno médico hoy a las {t.FechaHora:HH:mm} hs — Socio: {t.NombreSocio}",
                            Detalle = $"DNI: {t.DNISocio} | Profesional: {t.NombreNutricionista} | Estado actual: {t.Estado}.",
                            FechaReferencia = t.FechaHora.ToString("dd/MM/yyyy HH:mm")
                        });
                        alertasRecepcion++;
                    }
                }
                else
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = "Recepción",
                        Prioridad = "INFO",
                        Titulo = "Sin turnos nutricionales agendados para hoy",
                        Detalle = "La agenda de consultorios médicos del día de la fecha se encuentra libre.",
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
                        Area = "Entrenamiento",
                        Prioridad = "ALTA",
                        Titulo = $"Hay {desactualizados.Count} socio(s) que requieren actualización o asignación de rutina",
                        Detalle = $"{cantMasTresMeses} no actualizan su rutina hace más de 3 meses (>90 días) y {cantSinRutina} no poseen rutina activa.",
                        FechaReferencia = DateTime.Today.ToString("dd/MM/yyyy")
                    });
                    alertasEntrenamiento++;
                    foreach (var d in desactualizados.OrderByDescending(x => x.DiasSinActualizar))
                    {
                        string det = d.FechaUltima.HasValue
                            ? $"Última rutina creada el {d.FechaUltima.Value:dd/MM/yyyy} (hace {d.DiasSinActualizar} días). Supera el límite de 3 meses."
                            : "El socio se encuentra activo pero aún no registra ninguna rutina de entrenamiento en el sistema.";
                        alertas.Add(new AlertaDashboardBE
                        {
                            Area = "Entrenamiento",
                            Prioridad = "MEDIA",
                            Titulo = $"Revisión deportiva pendiente: {d.Socio.Nombre} {d.Socio.Apellido} (DNI {d.Socio.DNI})",
                            Detalle = det,
                            FechaReferencia = d.FechaUltima.HasValue ? d.FechaUltima.Value.ToString("dd/MM/yyyy") : "Sin rutina"
                        });
                        alertasEntrenamiento++;
                    }
                }
                else
                {
                    alertas.Add(new AlertaDashboardBE
                    {
                        Area = "Entrenamiento",
                        Prioridad = "INFO",
                        Titulo = "Planificaciones deportivas al día",
                        Detalle = $"Todos los socios activos ({sociosActivos.Count}) tienen rutinas actualizadas dentro de los últimos 3 meses.",
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
                        Area = "Nutrición",
                        Prioridad = "MEDIA",
                        Titulo = $"Tiene {pendientesHoy} consulta(s) nutricional(es) pendiente(s) para hoy",
                        Detalle = $"Próximo turno a las {turnosHoyNutri.Where(t => t.Estado == "Pendiente").Min(t => t.FechaHora):HH:mm} hs.",
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
                                Area = "Nutrición",
                                Prioridad = "ALTA",
                                Titulo = $"Seguimiento clínico prioritario: {socio.Nombre} {socio.Apellido} (DNI {socio.DNI})",
                                Detalle = $"Presenta {ultimoPlan.DatosBiometricos.DiagnosticoIMC} (IMC {imc:0.00} | {ultimoPlan.DatosBiometricos.PesoKg:0.#} kg) en su evaluación del {ultimoPlan.FechaCreacion:dd/MM/yyyy}.",
                                FechaReferencia = ultimoPlan.FechaCreacion.ToString("dd/MM/yyyy")
                            });
                            alertasNutricion++;
                        }
                    }
                }
            }
            return alertas;
        }

        public bool ObtenerEstadoVigenciaPlan(SocioBE socio, out DateTime? fechaVencimiento, out string nombrePlan, out int diasRestantes, out string mensajeEstado)
        {
            fechaVencimiento = null;
            nombrePlan = "Sin Plan";
            diasRestantes = -999;
            if (socio == null || socio.ID_Socio <= 0)
            {
                mensajeEstado = "Socio no válido.";
                return false;
            }
            if (!socio.EstadoActivo)
            {
                mensajeEstado = "El socio se encuentra dado de baja / inactivo en el padrón.";
                return false;
            }
            List<PlanSuscripcionBE> catalogoPlanes = planSuscripcionDAL.ListarPlanes();
            int codPlan = socio.PlanAsociado != null ? socio.PlanAsociado.CodigoPlan : 0;
            PlanSuscripcionBE plan = catalogoPlanes.FirstOrDefault(p => p.CodigoPlan == codPlan);
            int duracionDias = plan != null ? plan.DuracionDias : 30;
            nombrePlan = plan != null ? plan.Nombre : "Plan Estándar";
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
                mensajeEstado = "El socio no registra comprobantes de pago de suscripción.";
                return false;
            }
            ComprobantePagoBE ultimoPago = pagosSocio.First();
            DateTime venc = ultimoPago.Fecha.Date.AddDays(duracionDias);
            fechaVencimiento = venc;
            diasRestantes = (int)(venc - DateTime.Today).TotalDays;
            if (diasRestantes < 0)
            {
                mensajeEstado = $"Cuota VENCIDA el {venc:dd/MM/yyyy} (hace {Math.Abs(diasRestantes)} día(s) — {nombrePlan}).";
                return false;
            }
            mensajeEstado = $"Plan vigente ({nombrePlan}) hasta el {venc:dd/MM/yyyy} ({diasRestantes} día(s) restantes).";
            return true;
        }

        public void ValidarPlanVigenteAlDiaDeHoy(SocioBE socio, string operacion)
        {
            if (!ObtenerEstadoVigenciaPlan(socio, out DateTime? fechaVenc, out string nomPlan, out int diasRest, out string msg))
            {
                throw new Exception($"Operación denegada ({operacion}):\r\n{msg}\r\n\r\nDerive al socio a Recepción para renovar su suscripción.");
            }
        }

        public void ValidarPlanVigenteParaFecha(SocioBE socio, DateTime fechaObjetivo, string operacion)
        {
            if (!ObtenerEstadoVigenciaPlan(socio, out DateTime? fechaVenc, out string nomPlan, out int diasRest, out string msg))
            {
                throw new Exception($"Operación denegada ({operacion}):\r\n{msg}\r\n\r\nEl socio debe renovar su cuota antes de realizar esta operación.");
            }
            if (fechaVenc.HasValue && fechaObjetivo.Date > fechaVenc.Value.Date)
            {
                throw new Exception($"Cobertura de plan insuficiente para {operacion}:\r\n" +
                                    $"El socio posee '{nomPlan}' vigente hasta el {fechaVenc.Value:dd/MM/yyyy}, por lo que no cubre la fecha solicitada ({fechaObjetivo:dd/MM/yyyy}).\r\n\r\n" +
                                    $"Seleccione una fecha igual o anterior al {fechaVenc.Value:dd/MM/yyyy} o renueve la suscripción.");
            }
        }
    }
}