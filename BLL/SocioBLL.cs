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
            socio.DVH = DVManager.CalcularDVH(socio);
            dal.InsertarSocio(socio);
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
    }
}
