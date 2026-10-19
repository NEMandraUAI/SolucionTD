using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Seguridad;

namespace BLL
{
    public class PlanNutricionalBLL
    {
        private PlanNutricionalDAL _planDAL = new PlanNutricionalDAL();
        private IntegridadDAL _integridadDAL = new IntegridadDAL();
        private RegistroBLL _registroBLL = new RegistroBLL();
        private SocioDAL _socioDAL = new SocioDAL();
        private SocioBLL _socioBLL = new SocioBLL();

        public List<string> ObtenerGenerosDisponibles()
        {
            return new List<string> { "Masculino", "Femenino" };
        }

        public List<string> ObtenerObjetivosNutricionales()
        {
            return new List<string>
            {
                "Déficit Calórico (Pérdida de Grasa)",
                "Mantenimiento (Normocalórico)",
                "Superávit Calórico (Masa Muscular)"
            };
        }

        public EvaluacionBiometricaBE CalcularBiometria(decimal pesoKg, decimal alturaCm, int edad, string genero, string objetivoNutricional)
        {
            if (pesoKg <= 0 || alturaCm <= 0 || edad <= 0)
            {
                return new EvaluacionBiometricaBE();
            }
            decimal alturaMetros = alturaCm / 100m;
            decimal imc = Math.Round(pesoKg / (alturaMetros * alturaMetros), 2);
            string gen = string.IsNullOrWhiteSpace(genero) ? "Masculino" : genero.Trim();
            decimal tmb;
            if (gen.StartsWith("Fem", StringComparison.OrdinalIgnoreCase))
            {
                tmb = 655.0955m + (9.5634m * pesoKg) + (1.8496m * alturaCm) - (4.6756m * edad);
            }
            else
            {
                tmb = 66.4730m + (13.7516m * pesoKg) + (5.0033m * alturaCm) - (6.7550m * edad);
            }
            tmb = Math.Round(tmb, 0);
            string diagnostico = ClasificarIMC(imc);
            string obj = string.IsNullOrWhiteSpace(objetivoNutricional) ? "Mantenimiento (Normocalórico)" : objetivoNutricional;
            decimal gastoEstimado = Math.Round(tmb * 1.375m, 0);
            decimal caloriasObjetivo = gastoEstimado;
            if (obj.IndexOf("Déficit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                obj.IndexOf("Deficit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                caloriasObjetivo = Math.Max(1200m, gastoEstimado - 450m);
            }
            else if (obj.IndexOf("Superávit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     obj.IndexOf("Superavit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                caloriasObjetivo = gastoEstimado + 350m;
            }
            return new EvaluacionBiometricaBE
            {
                PesoKg = Math.Round(pesoKg, 1),
                AlturaCm = Math.Round(alturaCm, 1),
                Edad = edad,
                Genero = gen,
                IndiceMasaCorporal = imc,
                TasaMetabolicaBasal = tmb,
                DiagnosticoIMC = diagnostico,
                ObjetivoNutricional = obj,
                CaloriasObjetivo = caloriasObjetivo
            };
        }

        public string ClasificarIMC(decimal imc)
        {
            if (imc <= 0) return "Sin calcular";
            if (imc < 18.5m) return "Bajo Peso";
            if (imc < 25.0m) return "Peso Normal";
            if (imc < 30.0m) return "Sobrepeso";
            if (imc < 35.0m) return "Obesidad Moderada";
            return "Obesidad Severa";
        }

        public string ObtenerAdvertenciaClinica(EvaluacionBiometricaBE bio)
        {
            if (bio == null || bio.IndiceMasaCorporal <= 0)
                return "Complete los datos biométricos para obtener el diagnóstico clínico.";
            decimal imc = bio.IndiceMasaCorporal;
            if (imc < 18.5m)
            {
                return $"⚠️ ALERTA CLÍNICA: Bajo Peso detectado (IMC {imc:0.00}). Se desaconseja déficit calórico; priorizar recuperación nutricional.";
            }
            if (imc >= 35.0m)
            {
                return $"🚨 ALERTA CRÍTICA: Obesidad Severa detectada (IMC {imc:0.00}). Requiere control metabólico estricto y actividad de bajo impacto articular.";
            }
            if (imc >= 30.0m)
            {
                return $"⚠️ ALERTA CLÍNICA: Obesidad Moderada (IMC {imc:0.00}). Se sugiere déficit calórico progresivo y monitoreo cardiovascular.";
            }
            if (imc >= 25.0m)
            {
                return $"ℹ️ AVISO CLÍNICO: Sobrepeso (IMC {imc:0.00}). Se recomienda ajuste calórico moderado y control de hábitos.";
            }
            return $"✓ ESTADO NORMAL: IMC dentro del rango saludable ({imc:0.00}). Apto para cualquier protocolo nutricional.";
        }

        public string GenerarSugerenciaHabitos(EvaluacionBiometricaBE bio)
        {
            if (bio == null || bio.PesoKg <= 0)
                return string.Empty;
            decimal litrosAgua = Math.Round(bio.PesoKg * 0.035m, 1);
            decimal gramosProteina = Math.Round(bio.PesoKg * 1.8m, 0);
            decimal gramosGrasa = Math.Round(bio.PesoKg * 0.9m, 0);
            decimal kcalRestantes = Math.Max(400m, bio.CaloriasObjetivo - (gramosProteina * 4m) - (gramosGrasa * 9m));
            decimal gramosCarbos = Math.Round(kcalRestantes / 4m, 0);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[PLAN ALIMENTARIO SUGERIDO - {bio.ObjetivoNutricional.ToUpper()}]");
            sb.AppendLine($"• Requerimiento Energético Diario: {bio.CaloriasObjetivo:0} kcal/día (TMB Base: {bio.TasaMetabolicaBasal:0} kcal).");
            sb.AppendLine($"• Distribución de Macronutrientes Diarios:");
            sb.AppendLine($"   - Proteínas: {gramosProteina} g/día (~1.8 g/kg para preservación/síntesis muscular).");
            sb.AppendLine($"   - Carbohidratos complejos: {gramosCarbos} g/día (ajustados a demanda energética).");
            sb.AppendLine($"   - Grasas saludables: {gramosGrasa} g/día (priorizar Omega-3, aceite de oliva, frutos secos).");
            sb.AppendLine($"• Hidratación Mínima: {litrosAgua} litros de agua diarios (+500 ml por hora de entrenamiento).");
            sb.AppendLine($"• Pautas de Conducta: Fraccionar en 4 comidas principales, evitar ultraprocesados y priorizar descanso de 7-8 hs.");
            return sb.ToString();
        }

        public void AsignarPlan(PlanNutricionalBE plan)
        {
            if (plan == null)
                throw new Exception("El plan nutricional no puede ser nulo.");
            SocioBE socio = _socioDAL.LeerTodos().FirstOrDefault(s => s.ID_Socio == plan.ID_Socio);
            _socioBLL.ValidarPlanVigenteAlDiaDeHoy(socio, "confeccionar y asignar un nuevo Plan Nutricional");
            if (plan.DatosBiometricos != null && plan.DatosBiometricos.PesoKg > 0 && plan.DatosBiometricos.AlturaCm > 0)
            {
                if (plan.DatosBiometricos.PesoKg < 25m || plan.DatosBiometricos.PesoKg > 300m)
                    throw new Exception("El peso ingresado debe estar entre 25 kg y 300 kg.");
                if (plan.DatosBiometricos.AlturaCm < 100m || plan.DatosBiometricos.AlturaCm > 250m)
                    throw new Exception("La altura ingresada debe estar entre 100 cm y 250 cm.");
                if (plan.DatosBiometricos.Edad < 10 || plan.DatosBiometricos.Edad > 100)
                    throw new Exception("La edad debe estar comprendida entre 10 y 100 años.");
                plan.DatosBiometricos = CalcularBiometria(
                    plan.DatosBiometricos.PesoKg,
                    plan.DatosBiometricos.AlturaCm,
                    plan.DatosBiometricos.Edad,
                    plan.DatosBiometricos.Genero,
                    plan.DatosBiometricos.ObjetivoNutricional);
                plan.SincronizarBiometriaAJson();
            }
            if (string.IsNullOrWhiteSpace(plan.EvaluacionAntropometrica) || string.IsNullOrWhiteSpace(plan.DetalleHabitos))
                throw new Exception("La evaluación biométrica y los detalles de hábitos son obligatorios.");
            DateTime f = plan.FechaCreacion == default ? DateTime.Now : plan.FechaCreacion;
            plan.FechaCreacion = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, f.Second);
            plan.CodigoPlan = _planDAL.InsertarPlan(plan);
            plan.DVH = DVManager.CalcularDVH(plan);
            _planDAL.ActualizarDVH(plan.CodigoPlan, plan.DVH);
            List<string> listaDVH = _planDAL.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            _integridadDAL.ActualizarDVV("PlanNutricional", nuevoDVV);
            UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
            _registroBLL.RegistrarEvento($"Plan Nutricional asignado (Cod: {plan.CodigoPlan}) al socio ID {plan.ID_Socio}", usuarioActual, "INFO");
        }

        public List<PlanNutricionalBE> ObtenerPlanesSocio(int idSocio)
        {
            return _planDAL.ConsultarPlanesPorSocio(idSocio);
        }

        public List<PuntoEvolucionBE> ObtenerEvolucionBiometrica(int idSocio)
        {
            List<PuntoEvolucionBE> puntos = new List<PuntoEvolucionBE>();
            if (idSocio <= 0) return puntos;
            List<PlanNutricionalBE> planes = _planDAL.ConsultarPlanesPorSocio(idSocio);
            if (planes == null || planes.Count == 0) return puntos;
            var planesValidos = planes
                .Where(p => p.DatosBiometricos != null && p.DatosBiometricos.PesoKg > 0 && p.DatosBiometricos.AlturaCm > 0)
                .OrderBy(p => p.FechaCreacion)
                .ThenBy(p => p.CodigoPlan)
                .ToList();
            foreach (var p in planesValidos)
            {
                puntos.Add(new PuntoEvolucionBE
                {
                    Fecha = p.FechaCreacion,
                    EtiquetaFecha = p.FechaCreacion.ToString("dd/MM"),
                    ValorPrimario = Math.Round(p.DatosBiometricos.PesoKg, 1),
                    ValorSecundario = Math.Round(p.DatosBiometricos.IndiceMasaCorporal, 2),
                    Detalle = $"{p.DatosBiometricos.PesoKg:0.0} kg (IMC {p.DatosBiometricos.IndiceMasaCorporal:0.0})"
                });
            }
            return puntos;
        }
    }
}