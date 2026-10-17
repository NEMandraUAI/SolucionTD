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
    }
}