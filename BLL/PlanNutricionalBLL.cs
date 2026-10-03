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

        public void AsignarPlan(PlanNutricionalBE plan)
        {
            if (string.IsNullOrWhiteSpace(plan.EvaluacionAntropometrica) || string.IsNullOrWhiteSpace(plan.DetalleHabitos))
                throw new Exception("La evaluación y los detalles de hábitos son obligatorios.");
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
    }
}
