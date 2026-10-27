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
    public class RutinaEntrenamientoBLL
    {
        RutinaEntrenamientoDAL dal = new RutinaEntrenamientoDAL();
        IntegridadDAL intDal = new IntegridadDAL();
        PlanNutricionalDAL planNutDAL = new PlanNutricionalDAL();
        RegistroBLL regBll = new RegistroBLL();
        SocioDAL socioDAL = new SocioDAL();
        SocioBLL socioBLL = new SocioBLL();

        public List<string> ObtenerObjetivosDisponibles()
        {
            return new List<string>
            {
                "Hipertrofia",
                "Fuerza",
                "Pérdida de Peso",
                "Resistencia / Tonificación"
            };
        }

        public List<string> ObtenerGruposMusculares()
        {
            return new List<string>
            {
                "Pecho",
                "Espalda",
                "Piernas",
                "Hombros",
                "Bíceps",
                "Tríceps",
                "Core"
            };
        }

        public string NormalizarGrupoMuscular(string grupo)
        {
            if (string.IsNullOrWhiteSpace(grupo)) return string.Empty;
            string g = grupo.Trim().ToLowerInvariant();
            if (g.Contains("pecho") || g.Contains("pectoral")) return "Pecho";
            if (g.Contains("espalda") || g.Contains("dorsal") || g.Contains("trapecio")) return "Espalda";
            if (g.Contains("pierna") || g.Contains("cuadriceps") || g.Contains("cuádriceps") || g.Contains("isquio") || g.Contains("gluteo") || g.Contains("glúteo")) return "Piernas";
            if (g.Contains("hombro") || g.Contains("deltoide")) return "Hombros";
            if (g.Contains("bicep") || g.Contains("bícep")) return "Bíceps";
            if (g.Contains("tricep") || g.Contains("trícep")) return "Tríceps";
            if (g.Contains("core") || g.Contains("abdom") || g.Contains("lumbar")) return "Core";
            return string.Empty;
        }

        public Dictionary<string, int> CalcularSeriesPorGrupoMuscular(IEnumerable<EjercicioBE> ejercicios)
        {
            Dictionary<string, int> conteo = new Dictionary<string, int>
            {
                { "Pecho", 0 },
                { "Espalda", 0 },
                { "Piernas", 0 },
                { "Hombros", 0 },
                { "Bíceps", 0 },
                { "Tríceps", 0 },
                { "Core", 0 }
            };
            if (ejercicios == null) return conteo;
            foreach (var ej in ejercicios)
            {
                if (ej == null || string.IsNullOrWhiteSpace(ej.Nombre) || ej.Series <= 0)
                    continue;
                string clave = NormalizarGrupoMuscular(ej.GrupoMuscular);
                if (!string.IsNullOrEmpty(clave) && conteo.ContainsKey(clave))
                {
                    conteo[clave] += ej.Series;
                }
            }
            return conteo;
        }

        public Dictionary<string, int> CalcularNivelCalorMuscular(IEnumerable<EjercicioBE> ejercicios)
        {
            var seriesPorGrupo = CalcularSeriesPorGrupoMuscular(ejercicios);
            Dictionary<string, int> niveles = new Dictionary<string, int>();
            int maxSeries = seriesPorGrupo.Values.Count > 0 ? seriesPorGrupo.Values.Max() : 0;
            foreach (var kvp in seriesPorGrupo)
            {
                int series = kvp.Value;
                if (series <= 0)
                {
                    niveles[kvp.Key] = 0;
                }
                else if (series >= 5 || (maxSeries >= 4 && series == maxSeries))
                {
                    niveles[kvp.Key] = 2;
                }
                else
                {
                    niveles[kvp.Key] = 1;
                }
            }
            return niveles;
        }

        public List<EjercicioBE> GenerarRutinaSugerida(string objetivoFisico, int frecuenciaSemanal, int variante = 0)
        {
            if (string.IsNullOrWhiteSpace(objetivoFisico))
                objetivoFisico = "Hipertrofia";
            if (frecuenciaSemanal < 1) frecuenciaSemanal = 1;
            if (frecuenciaSemanal > 7) frecuenciaSemanal = 7;
            int indiceVariante = Math.Abs(variante) % 3;
            int seriesBase;
            string repsBase;
            int descansoBase;
            decimal factorCarga;
            string objNormalizado = objetivoFisico.Trim().ToLowerInvariant();
            if (objNormalizado.Contains("fuerza"))
            {
                seriesBase = 5;
                repsBase = "3-5";
                descansoBase = 180;
                factorCarga = 1.25m;
            }
            else if (objNormalizado.Contains("pérdida") || objNormalizado.Contains("perdida") || objNormalizado.Contains("peso"))
            {
                seriesBase = 4;
                repsBase = "15-20";
                descansoBase = 45;
                factorCarga = 0.75m;
            }
            else if (objNormalizado.Contains("resistencia") || objNormalizado.Contains("tonificaci"))
            {
                seriesBase = 3;
                repsBase = "12-15";
                descansoBase = 60;
                factorCarga = 0.85m;
            }
            else
            {
                seriesBase = 4;
                repsBase = "10-12";
                descansoBase = 90;
                factorCarga = 1.00m;
            }
            int seriesAccesorios = frecuenciaSemanal >= 5 ? Math.Max(3, seriesBase - 1) : seriesBase;
            string[] ejPiernas1 = { "Sentadilla Libre con Barra", "Prensa de Piernas 45°", "Sentadilla Hack" };
            decimal[] cargaPiernas1 = { 60m, 100m, 70m };
            string[] ejPecho1 = { "Press de Banca Plano con Barra", "Press Inclinado con Mancuernas", "Fondos en Paralelas Lastre" };
            decimal[] cargaPecho1 = { 50m, 22.5m, 15m };
            string[] ejEspalda1 = { "Remo con Barra Prono", "Jalón al Pecho Agarre Neutro", "Remo en Polea Baja con Triángulo" };
            decimal[] cargaEspalda1 = { 45m, 50m, 45m };
            string[] ejHombros1 = { "Press Militar con Mancuernas", "Press Arnold con Mancuernas", "Press Militar con Barra de Pie" };
            decimal[] cargaHombros1 = { 16m, 14m, 35m };
            string[] ejCore1 = { "Plancha Abdominal Isométrica", "Crunch Abdominal en Polea Alta", "Elevación de Piernas Colgado" };
            string[] repsCore1 = { "45 seg", "15-20", "12-15" };
            decimal[] cargaCore1 = { 0m, 25m, 0m };
            List<EjercicioBE> sugeridos = new List<EjercicioBE>
            {
                new EjercicioBE
                {
                    Nombre = ejPiernas1[indiceVariante],
                    GrupoMuscular = "Piernas",
                    Series = seriesBase,
                    Repeticiones = repsBase,
                    TiempoDescansoSegundos = descansoBase,
                    CargaSugeridaKg = Math.Round(cargaPiernas1[indiceVariante] * factorCarga, 1)
                },
                new EjercicioBE
                {
                    Nombre = ejPecho1[indiceVariante],
                    GrupoMuscular = "Pecho",
                    Series = seriesBase,
                    Repeticiones = repsBase,
                    TiempoDescansoSegundos = descansoBase,
                    CargaSugeridaKg = Math.Round(cargaPecho1[indiceVariante] * factorCarga, 1)
                },
                new EjercicioBE
                {
                    Nombre = ejEspalda1[indiceVariante],
                    GrupoMuscular = "Espalda",
                    Series = seriesBase,
                    Repeticiones = repsBase,
                    TiempoDescansoSegundos = descansoBase,
                    CargaSugeridaKg = Math.Round(cargaEspalda1[indiceVariante] * factorCarga, 1)
                },
                new EjercicioBE
                {
                    Nombre = ejHombros1[indiceVariante],
                    GrupoMuscular = "Hombros",
                    Series = seriesAccesorios,
                    Repeticiones = repsBase,
                    TiempoDescansoSegundos = Math.Max(45, descansoBase - 15),
                    CargaSugeridaKg = Math.Round(cargaHombros1[indiceVariante] * factorCarga, 1)
                },
                new EjercicioBE
                {
                    Nombre = ejCore1[indiceVariante],
                    GrupoMuscular = "Core",
                    Series = 3,
                    Repeticiones = repsCore1[indiceVariante],
                    TiempoDescansoSegundos = 45,
                    CargaSugeridaKg = Math.Round(cargaCore1[indiceVariante] * factorCarga, 1)
                }
            };
            if (frecuenciaSemanal >= 3)
            {
                string[] ejPiernas2 = { "Peso Muerto Rumano", "Camilla de Isquiotibiales", "Zancadas Búlgaras con Mancuernas" };
                decimal[] cargaPiernas2 = { 55m, 40m, 18m };
                string[] ejBiceps = { "Curl de Bíceps con Barra EZ", "Curl Alterno con Mancuernas Supino", "Curl Martillo en Polea con Soga" };
                decimal[] cargaBiceps = { 25m, 12.5m, 20m };
                string[] ejTriceps = { "Extensión de Tríceps en Polea Alta", "Press Francés con Barra EZ", "Extensión Trasnuca con Mancuerna" };
                decimal[] cargaTriceps = { 20m, 25m, 16m };
                sugeridos.Add(new EjercicioBE
                {
                    Nombre = ejPiernas2[indiceVariante],
                    GrupoMuscular = "Piernas",
                    Series = seriesBase,
                    Repeticiones = repsBase,
                    TiempoDescansoSegundos = descansoBase,
                    CargaSugeridaKg = Math.Round(cargaPiernas2[indiceVariante] * factorCarga, 1)
                });
                sugeridos.Add(new EjercicioBE
                {
                    Nombre = ejBiceps[indiceVariante],
                    GrupoMuscular = "Bíceps",
                    Series = seriesAccesorios,
                    Repeticiones = objNormalizado.Contains("fuerza") ? "6-8" : "10-12",
                    TiempoDescansoSegundos = 60,
                    CargaSugeridaKg = Math.Round(cargaBiceps[indiceVariante] * factorCarga, 1)
                });
                sugeridos.Add(new EjercicioBE
                {
                    Nombre = ejTriceps[indiceVariante],
                    GrupoMuscular = "Tríceps",
                    Series = seriesAccesorios,
                    Repeticiones = objNormalizado.Contains("fuerza") ? "6-8" : "10-12",
                    TiempoDescansoSegundos = 60,
                    CargaSugeridaKg = Math.Round(cargaTriceps[indiceVariante] * factorCarga, 1)
                });
            }
            if (frecuenciaSemanal >= 5)
            {
                string[] ejEspalda2 = { "Dominadas / Jalón Abierto Prono", "Remo Unilateral con Mancuerna", "Pull-Over en Polea Alta" };
                decimal[] cargaEspalda2 = { 45m, 24m, 25m };
                string[] ejHombros2 = { "Vuelos Laterales con Mancuernas", "Pájaros Posteriores en Polea", "Face Pull con Soga" };
                decimal[] cargaHombros2 = { 10m, 15m, 20m };
                sugeridos.Add(new EjercicioBE
                {
                    Nombre = ejEspalda2[indiceVariante],
                    GrupoMuscular = "Espalda",
                    Series = seriesAccesorios,
                    Repeticiones = repsBase,
                    TiempoDescansoSegundos = Math.Max(60, descansoBase - 30),
                    CargaSugeridaKg = Math.Round(cargaEspalda2[indiceVariante] * factorCarga, 1)
                });
                sugeridos.Add(new EjercicioBE
                {
                    Nombre = ejHombros2[indiceVariante],
                    GrupoMuscular = "Hombros",
                    Series = 4,
                    Repeticiones = "12-15",
                    TiempoDescansoSegundos = 60,
                    CargaSugeridaKg = Math.Round(cargaHombros2[indiceVariante] * factorCarga, 1)
                });
            }
            return sugeridos;
        }

        private void ValidarYSincronizarEjercicios(RutinaEntrenamientoBE rutina)
        {
            if (rutina.Ejercicios != null && rutina.Ejercicios.Count > 0)
            {
                rutina.Ejercicios = rutina.Ejercicios
                    .Where(e => e != null && !string.IsNullOrWhiteSpace(e.Nombre))
                    .ToList();
                if (rutina.Ejercicios.Count == 0)
                    throw new Exception("Debe incluir al menos un ejercicio con nombre válido en la rutina.");
                foreach (var ej in rutina.Ejercicios)
                {
                    if (string.IsNullOrWhiteSpace(ej.GrupoMuscular))
                        throw new Exception($"El ejercicio '{ej.Nombre}' debe tener un grupo muscular asignado.");
                    if (ej.Series <= 0 || ej.Series > 20)
                        throw new Exception($"La cantidad de series para '{ej.Nombre}' debe estar entre 1 y 20.");
                    if (string.IsNullOrWhiteSpace(ej.Repeticiones))
                        throw new Exception($"Debe especificar las repeticiones para el ejercicio '{ej.Nombre}'.");
                    if (ej.TiempoDescansoSegundos < 0 || ej.TiempoDescansoSegundos > 600)
                        throw new Exception($"El tiempo de descanso para '{ej.Nombre}' debe estar entre 0 y 600 segundos.");
                    if (ej.CargaSugeridaKg < 0 || ej.CargaSugeridaKg > 500)
                        throw new Exception($"La carga sugerida para '{ej.Nombre}' debe estar entre 0 y 500 kg.");
                }
                rutina.SincronizarEjerciciosAJson();
            }
        }

        private void ValidarPermisoEntrenador()
        {
            UsuarioBE actual = SessionManager.Instancia.UsuarioActual;
            if (actual == null || (!actual.TienePermiso("ASIGNAR_RUTINA") && !actual.TienePermiso("GESTION_ROLES")))
            {
                throw new Exception("Operación denegada: No posee el permiso requerido (ASIGNAR_RUTINA) para gestionar rutinas de entrenamiento.");
            }
        }

        public void AsignarRutina(RutinaEntrenamientoBE rutina)
        {
            ArgumentNullException.ThrowIfNull(rutina);
            using var unidad = new UnidadTrabajoSeguridadDAL();
            var autor = TurnoBLL.ValidarActor(unidad, "ASIGNAR_RUTINA");
            IntegridadTransaccionalDAL.VerificarDVV(unidad, "RutinaEntrenamiento");
            var socio = new SocioDAL(unidad).LeerTodos().SingleOrDefault(s => s.ID_Socio == rutina.ID_SocioParaDVH)
                ?? throw new ArgumentException("El socio seleccionado no existe.");
            TurnoBLL.ValidarDVHSocio(socio);
            new SocioBLL(unidad).ValidarPlanVigenteAlDiaDeHoy(socio, "asignar una nueva rutina de entrenamiento");
            var f = rutina.FechaCreacion == default ? DateTime.Now : rutina.FechaCreacion;
            var confirmado = new RutinaEntrenamientoBE { FechaCreacion=new DateTime(f.Year,f.Month,f.Day,f.Hour,f.Minute,f.Second),
                ObjetivoFisico=rutina.ObjetivoFisico, FrecuenciaSemanal=rutina.FrecuenciaSemanal,
                DetalleEjercicios=rutina.DetalleEjercicios, Ejercicios=rutina.Ejercicios, Socio=socio, Entrenador=autor };
            ValidarContenidoRutina(confirmado);
            var dal = new RutinaEntrenamientoDAL(unidad);
            dal.InsertarRutina(confirmado);
            confirmado.DVH=DVManager.CalcularDVH(confirmado);
            dal.ActualizarRutina(confirmado);
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "RutinaEntrenamiento");
            new RegistroBLL(unidad).RegistrarEvento($"Rutina asignada al socio ID {socio.ID_Socio} por el entrenador.", autor);
            unidad.Confirmar();
            rutina.CodigoRutina=confirmado.CodigoRutina; rutina.DVH=confirmado.DVH;
            rutina.FechaCreacion=confirmado.FechaCreacion; rutina.Socio=socio; rutina.Entrenador=autor;
        }

        private void ValidarContenidoRutina(RutinaEntrenamientoBE rutina)
        {
            if (string.IsNullOrWhiteSpace(rutina.ObjetivoFisico)) throw new ArgumentException("Debe especificar el objetivo físico.");
            if (rutina.FrecuenciaSemanal < 1 || rutina.FrecuenciaSemanal > 7) throw new ArgumentException("La frecuencia debe estar entre 1 y 7 días.");
            ValidarYSincronizarEjercicios(rutina);
            if (string.IsNullOrWhiteSpace(rutina.DetalleEjercicios)) throw new ArgumentException("El detalle de ejercicios no puede estar vacío.");
        }
        public List<RutinaEntrenamientoBE> ConsultarRutinasPorDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception("Debe ingresar el DNI del socio para buscar sus rutinas.");
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID > 0)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            var dic = new IdiomaDAL().ObtenerTraduccionesPorFormulario(idIdioma, "NegocioBLL");
            string txtVig = dic.ContainsKey("rutEstadoVigente") ? dic["rutEstadoVigente"] : "VIGENTE (Actual)";
            string txtHist = dic.ContainsKey("rutEstadoHistorica") ? dic["rutEstadoHistorica"] : "HISTÓRICA (Solo Lectura)";
            var lista = dal.ObtenerRutinasPorDNI(dni)
                .OrderByDescending(r => r.FechaCreacion)
                .ThenByDescending(r => r.CodigoRutina)
                .ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].EsVigente = (i == 0);
                lista[i].TextoEstadoVigencia = (i == 0) ? txtVig : txtHist;
            }
            return lista;
        }

        private RutinaEntrenamientoBE ObtenerRutinaVigente(UnidadTrabajoSeguridadDAL unidad, int codigo, string operacion)
        {
            var todas = new RutinaEntrenamientoDAL(unidad).LeerTodos();
            var actual = todas.SingleOrDefault(r => r.CodigoRutina == codigo)
                ?? throw new ArgumentException("La rutina seleccionada no existe.");
            if (actual.DVH != DVManager.CalcularDVH(actual)) throw new InvalidOperationException("La rutina fue alterada externamente. Se requiere revisión de integridad.");
            var socio = new SocioDAL(unidad).LeerTodos().SingleOrDefault(s => s.ID_Socio == actual.ID_SocioParaDVH);
            TurnoBLL.ValidarDVHSocio(socio);
            new SocioBLL(unidad).ValidarPlanVigenteAlDiaDeHoy(socio, operacion + " una rutina de entrenamiento");
            var vigente = todas.Where(r => r.ID_SocioParaDVH == actual.ID_SocioParaDVH)
                .OrderByDescending(r => r.FechaCreacion).ThenByDescending(r => r.CodigoRutina).First();
            if (vigente.CodigoRutina != codigo) throw new InvalidOperationException("Solo puede modificar o eliminar la rutina vigente del socio.");
            return actual;
        }

        public void ModificarRutina(RutinaEntrenamientoBE rutina)
        {
            ArgumentNullException.ThrowIfNull(rutina);
            using var unidad = new UnidadTrabajoSeguridadDAL();
            var autor = TurnoBLL.ValidarActor(unidad, "ASIGNAR_RUTINA");
            IntegridadTransaccionalDAL.VerificarDVV(unidad, "RutinaEntrenamiento");
            var confirmado = ObtenerRutinaVigente(unidad, rutina.CodigoRutina, "modificar");
            confirmado.ObjetivoFisico=rutina.ObjetivoFisico; confirmado.FrecuenciaSemanal=rutina.FrecuenciaSemanal;
            confirmado.DetalleEjercicios=rutina.DetalleEjercicios; confirmado.Ejercicios=rutina.Ejercicios;
            ValidarContenidoRutina(confirmado);
            confirmado.DVH=DVManager.CalcularDVH(confirmado);
            new RutinaEntrenamientoDAL(unidad).ActualizarRutina(confirmado);
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "RutinaEntrenamiento");
            new RegistroBLL(unidad).RegistrarEvento($"Rutina {confirmado.CodigoRutina} modificada.", autor);
            unidad.Confirmar();
            rutina.DVH=confirmado.DVH; rutina.FechaCreacion=confirmado.FechaCreacion;
            rutina.Socio=confirmado.Socio; rutina.Entrenador=confirmado.Entrenador;
        }

        public void EliminarRutina(RutinaEntrenamientoBE rutina)
        {
            ArgumentNullException.ThrowIfNull(rutina);
            using var unidad = new UnidadTrabajoSeguridadDAL();
            var autor = TurnoBLL.ValidarActor(unidad, "ASIGNAR_RUTINA");
            IntegridadTransaccionalDAL.VerificarDVV(unidad, "RutinaEntrenamiento");
            var confirmado = ObtenerRutinaVigente(unidad, rutina.CodigoRutina, "eliminar");
            new RutinaEntrenamientoDAL(unidad).EliminarRutina(confirmado.CodigoRutina);
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "RutinaEntrenamiento");
            new RegistroBLL(unidad).RegistrarEvento($"Rutina {confirmado.CodigoRutina} eliminada del sistema.", autor, "ALTA");
            unidad.Confirmar();
        }
        public string EvaluarSinergiaNutricional(int idSocio, string objetivoFisico, int frecuenciaSemanal, out int nivelSeveridad)
        {
            nivelSeveridad = 0;
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID > 0)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            var dic = new IdiomaDAL().ObtenerTraduccionesPorFormulario(idIdioma, "NegocioBLL");
            if (idSocio <= 0)
            {
                return dic.ContainsKey("sinergiaSinPlan")
                    ? dic["sinergiaSinPlan"]
                    : "ℹ️ SINERGIA INTER-ÁREAS: El socio aún no registra un Plan Nutricional en el consultorio médico.";
            }
            List<PlanNutricionalBE> planes = planNutDAL.ConsultarPlanesPorSocio(idSocio);
            if (planes == null || planes.Count == 0)
            {
                return dic.ContainsKey("sinergiaSinPlan")
                    ? dic["sinergiaSinPlan"]
                    : "ℹ️ SINERGIA INTER-ÁREAS: El socio aún no registra un Plan Nutricional en el consultorio médico.";
            }
            PlanNutricionalBE ultimoPlan = planes
                .OrderByDescending(p => p.FechaCreacion)
                .ThenByDescending(p => p.CodigoPlan)
                .First();
            var bio = ultimoPlan.DatosBiometricos;
            if (bio == null || bio.PesoKg <= 0)
            {
                nivelSeveridad = 1;
                return $"ℹ️ Plan Nutricional (#{ultimoPlan.CodigoPlan} - {ultimoPlan.FechaCreacion:dd/MM/yyyy}): {ultimoPlan.ResumenBiometrico}";
            }
            string objNut = bio.ObjetivoNutricional ?? string.Empty;
            string objEnt = objetivoFisico ?? string.Empty;
            bool enDeficit = objNut.IndexOf("Déficit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             objNut.IndexOf("Pérdida", StringComparison.OrdinalIgnoreCase) >= 0;
            bool altaExigencia = objEnt.IndexOf("Fuerza", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 objEnt.IndexOf("Hipertrofia", StringComparison.OrdinalIgnoreCase) >= 0;
            if (enDeficit && frecuenciaSemanal >= 5 && altaExigencia)
            {
                nivelSeveridad = 3;
                string plantilla = dic.ContainsKey("sinergiaRiesgoAlto")
                    ? dic["sinergiaRiesgoAlto"]
                    : "🚨 ALERTA DE SOBREENTRENAMIENTO: El socio está en '{0}' ({1} kcal | IMC {2}). Entrenar {3} días/sem con objetivo '{4}' excede su recuperación. Se recomienda ≤ 4 días o moderar volumen.";
                return string.Format(plantilla, bio.ObjetivoNutricional, bio.CaloriasObjetivo, bio.IndiceMasaCorporal.ToString("0.00"), frecuenciaSemanal, objEnt);
            }
            if (bio.IndiceMasaCorporal < 18.5m || bio.IndiceMasaCorporal >= 30.0m)
            {
                nivelSeveridad = 2;
                string plantilla = dic.ContainsKey("sinergiaPrecaucion")
                    ? dic["sinergiaPrecaucion"]
                    : "⚠️ PRECAUCIÓN CLÍNICA-DEPORTIVA: Socio con diagnóstico '{0}' (IMC {1} | Plan: {2}). Priorizar ejercicios de bajo impacto articular y control de carga.";
                return string.Format(plantilla, bio.DiagnosticoIMC, bio.IndiceMasaCorporal.ToString("0.00"), bio.ObjetivoNutricional);
            }
            nivelSeveridad = 1;
            string plantillaOpt = dic.ContainsKey("sinergiaOptima")
                ? dic["sinergiaOptima"]
                : "✅ SINERGIA ÓPTIMA: Plan nutricional '{0}' ({1} kcal | Peso: {2} kg | IMC: {3}) alineado con entrenamiento de '{4}' ({5} días/sem).";
            return string.Format(plantillaOpt, bio.ObjetivoNutricional, bio.CaloriasObjetivo, bio.PesoKg.ToString("0.#"), bio.IndiceMasaCorporal.ToString("0.00"), objEnt, frecuenciaSemanal);
        }

        public List<PuntoEvolucionBE> ObtenerEvolucionFuerza(List<RutinaEntrenamientoBE> rutinas)
        {
            List<PuntoEvolucionBE> puntos = new List<PuntoEvolucionBE>();
            if (rutinas == null || rutinas.Count == 0) return puntos;
            var ordenadas = rutinas
                .Where(r => r != null && r.Ejercicios != null && r.Ejercicios.Count > 0)
                .OrderBy(r => r.FechaCreacion)
                .ThenBy(r => r.CodigoRutina)
                .ToList();
            foreach (var r in ordenadas)
            {
                var ejerciciosConCarga = r.Ejercicios.Where(e => e.CargaSugeridaKg > 0).ToList();
                decimal cargaPromedio = ejerciciosConCarga.Count > 0
                    ? Math.Round(ejerciciosConCarga.Average(e => e.CargaSugeridaKg), 1)
                    : 0m;
                decimal volumenTotal = Math.Round(r.Ejercicios.Sum(e => e.Series * e.CargaSugeridaKg), 0);
                puntos.Add(new PuntoEvolucionBE
                {
                    Fecha = r.FechaCreacion,
                    EtiquetaFecha = $"#{r.CodigoRutina} ({r.FechaCreacion:dd/MM})",
                    ValorPrimario = cargaPromedio,
                    ValorSecundario = volumenTotal,
                    Detalle = $"{cargaPromedio:0.0} kg prom. | Vol: {volumenTotal:0} kg"
                });
            }
            return puntos;
        }
    }
}