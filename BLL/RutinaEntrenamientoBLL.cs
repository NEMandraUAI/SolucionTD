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

        public void AsignarRutina(RutinaEntrenamientoBE rutina)
        {
            if (string.IsNullOrWhiteSpace(rutina.ObjetivoFisico))
                throw new Exception("Debe especificar el objetivo físico del socio.");
            if (rutina.FrecuenciaSemanal <= 0 || rutina.FrecuenciaSemanal > 7)
                throw new Exception("La frecuencia semanal debe estar entre 1 y 7 días.");
            ValidarYSincronizarEjercicios(rutina);
            if (string.IsNullOrWhiteSpace(rutina.DetalleEjercicios))
                throw new Exception("El detalle de los ejercicios no puede estar vacío.");
            if (rutina.Socio == null || rutina.Socio.ID_Socio == 0)
                throw new Exception("La rutina debe estar asociada a un socio válido.");
            DateTime f = rutina.FechaCreacion == default ? DateTime.Now : rutina.FechaCreacion;
            rutina.FechaCreacion = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, f.Second);
            rutina.DVH = string.Empty;
            dal.InsertarRutina(rutina);
            rutina.DVH = DVManager.CalcularDVH(rutina);
            dal.ActualizarRutina(rutina);
            List<string> listaDVH = dal.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            intDal.ActualizarDVV("RutinaEntrenamiento", nuevoDVV);
            regBll.RegistrarEvento($"Rutina asignada al socio DNI: {rutina.Socio.DNI} por el entrenador.", SessionManager.Instancia.UsuarioActual, "INFO");
        }

        public List<RutinaEntrenamientoBE> ConsultarRutinasPorDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception("Debe ingresar el DNI del socio para buscar sus rutinas.");
            return dal.ObtenerRutinasPorDNI(dni);
        }

        public void ModificarRutina(RutinaEntrenamientoBE rutina)
        {
            if (string.IsNullOrWhiteSpace(rutina.ObjetivoFisico))
                throw new Exception("El objetivo físico no puede estar vacío.");
            if (rutina.FrecuenciaSemanal <= 0 || rutina.FrecuenciaSemanal > 7)
                throw new Exception("La frecuencia debe ser de 1 a 7 días.");
            ValidarYSincronizarEjercicios(rutina);
            if (string.IsNullOrWhiteSpace(rutina.DetalleEjercicios))
                throw new Exception("El detalle de ejercicios no puede estar vacío.");
            rutina.DVH = DVManager.CalcularDVH(rutina);
            dal.ActualizarRutina(rutina);
            List<string> listaDVH = dal.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            intDal.ActualizarDVV("RutinaEntrenamiento", nuevoDVV);
            regBll.RegistrarEvento($"Rutina {rutina.CodigoRutina} modificada.", SessionManager.Instancia.UsuarioActual, "INFO");
        }

        public void EliminarRutina(RutinaEntrenamientoBE rutina)
        {
            dal.EliminarRutina(rutina.CodigoRutina);
            List<string> listaDVH = dal.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            intDal.ActualizarDVV("RutinaEntrenamiento", nuevoDVV);
            regBll.RegistrarEvento($"Rutina {rutina.CodigoRutina} eliminada del sistema.", SessionManager.Instancia.UsuarioActual, "ALTA");
        }

        public string EvaluarSinergiaNutricional(int idSocio, string objetivoEntrenamiento, int frecuenciaSemanal, out int nivelSeveridad)
        {
            nivelSeveridad = 0;
            if (idSocio <= 0)
            {
                return "ℹ️ SINERGIA INTER-ÁREAS: Valide un socio para consultar su estado clínico-nutricional.";
            }
            List<PlanNutricionalBE> planes = planNutDAL.ConsultarPlanesPorSocio(idSocio);
            if (planes == null || planes.Count == 0)
            {
                nivelSeveridad = 0;
                return "ℹ️ SINERGIA NUTRICIÓN: El socio no posee planes nutricionales registrados. Se sugiere derivarlo a consulta nutricional.";
            }
            PlanNutricionalBE ultimoPlan = planes.OrderByDescending(p => p.FechaCreacion).First();
            EvaluacionBiometricaBE bio = ultimoPlan.DatosBiometricos;
            string objEntreno = (objetivoEntrenamiento ?? string.Empty).Trim();
            bool entrenoAltaDemanda = objEntreno.IndexOf("Fuerza", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                      objEntreno.IndexOf("Hipertrofia", StringComparison.OrdinalIgnoreCase) >= 0;
            if (bio == null || bio.PesoKg <= 0 || bio.AlturaCm <= 0)
            {
                string textoLibre = (ultimoPlan.EvaluacionAntropometrica + " " + ultimoPlan.DetalleHabitos).ToLowerInvariant();
                if (textoLibre.Contains("déficit") || textoLibre.Contains("deficit") || textoLibre.Contains("hipocal"))
                {
                    nivelSeveridad = 2;
                    return $"⚠️ Socio en Déficit Calórico (Según Nutrición - Plan del {ultimoPlan.FechaCreacion:dd/MM/yyyy}). Moderar volumen de entrenamiento.";
                }
                nivelSeveridad = 0;
                return $"ℹ️ SINERGIA NUTRICIÓN: Último plan registrado el {ultimoPlan.FechaCreacion:dd/MM/yyyy} sin biometría estructurada ({ultimoPlan.EvaluacionAntropometrica}).";
            }
            bool enDeficit = (bio.ObjetivoNutricional ?? string.Empty).IndexOf("Déficit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             (bio.ObjetivoNutricional ?? string.Empty).IndexOf("Deficit", StringComparison.OrdinalIgnoreCase) >= 0;
            if (bio.IndiceMasaCorporal < 18.5m)
            {
                nivelSeveridad = 3;
                return $"🚨 RESTRICCIÓN FÍSICA (Nutrición {ultimoPlan.FechaCreacion:dd/MM/yyyy}): Socio con Bajo Peso (IMC {bio.IndiceMasaCorporal:0.00} | {bio.PesoKg:0.#} kg).\r\n" +
                       $"Evitar gasto calórico excesivo o frecuencias mayores a 3-4 días. Priorizar fuerza submáxima y recuperación.";
            }
            if (bio.IndiceMasaCorporal >= 30.0m)
            {
                nivelSeveridad = 3;
                return $"⚠️ RESTRICCIÓN FÍSICA (Nutrición {ultimoPlan.FechaCreacion:dd/MM/yyyy}): {bio.DiagnosticoIMC} (IMC {bio.IndiceMasaCorporal:0.00} | {bio.PesoKg:0.#} kg | Meta: {bio.CaloriasObjetivo:0} kcal).\r\n" +
                       $"Evitar impacto articular axial elevado. Priorizar trabajo metabólico controlado y máquinas guiadas.";
            }
            if (enDeficit)
            {
                if (entrenoAltaDemanda && frecuenciaSemanal >= 5)
                {
                    nivelSeveridad = 3;
                    return $"🚨 ALERTA DE SOBREENTRENAMIENTO: Socio en Déficit Calórico ({bio.CaloriasObjetivo:0} kcal/día | IMC {bio.IndiceMasaCorporal:0.00}).\r\n" +
                           $"Asignar '{objEntreno}' con frecuencia de {frecuenciaSemanal} días/semana excede su recuperación energética. Se sugiere bajar a 3-4 días.";
                }
                nivelSeveridad = 2;
                return $"⚠️ Socio en Déficit Calórico (Según Nutrición: {bio.CaloriasObjetivo:0} kcal/día | Peso: {bio.PesoKg:0.#} kg | IMC: {bio.IndiceMasaCorporal:0.00}).\r\n" +
                       $"Recomendación para '{objEntreno}': Controlar el volumen total de series y priorizar cargas de mantenimiento muscular.";
            }
            if (bio.IndiceMasaCorporal >= 25.0m)
            {
                nivelSeveridad = 2;
                return $"⚠️ AVISO NUTRICIONAL ({ultimoPlan.FechaCreacion:dd/MM/yyyy}): Socio con Sobrepeso (IMC {bio.IndiceMasaCorporal:0.00} | {bio.PesoKg:0.#} kg | Protocolo: {bio.ObjetivoNutricional}).\r\n" +
                       $"Se sugiere combinar la rutina de '{objEntreno}' con densidad metabólica y descansos activos (45-60 seg).";
            }
            nivelSeveridad = 1;
            return $"✓ SINERGIA NUTRICIONAL ÓPTIMA ({ultimoPlan.FechaCreacion:dd/MM/yyyy}): {bio.ObjetivoNutricional} ({bio.CaloriasObjetivo:0} kcal/día | {bio.PesoKg:0.#} kg | IMC {bio.IndiceMasaCorporal:0.00}).\r\n" +
                   $"Disponibilidad energética plena para soportar la rutina de '{objEntreno}' ({frecuenciaSemanal} días/semana).";
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