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

        public void AsignarRutina(RutinaEntrenamientoBE rutina)
        {
            if (string.IsNullOrWhiteSpace(rutina.ObjetivoFisico))
                throw new Exception("Debe especificar el objetivo físico del socio.");
            if (rutina.FrecuenciaSemanal <= 0 || rutina.FrecuenciaSemanal > 7)
                throw new Exception("La frecuencia semanal debe estar entre 1 y 7 días.");
            if (string.IsNullOrWhiteSpace(rutina.DetalleEjercicios))
                throw new Exception("El detalle de los ejercicios no puede estar vacío.");
            if (rutina.Socio == null || rutina.Socio.ID_Socio == 0)
                throw new Exception("La rutina debe estar asociada a un socio válido.");
            rutina.DVH = DVManager.CalcularDVH(rutina);
            dal.InsertarRutina(rutina);
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
            if (string.IsNullOrWhiteSpace(rutina.ObjetivoFisico)) throw new Exception("El objetivo físico no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(rutina.DetalleEjercicios)) throw new Exception("El detalle de ejercicios no puede estar vacío.");
            if (rutina.FrecuenciaSemanal <= 0 || rutina.FrecuenciaSemanal > 7) throw new Exception("La frecuencia debe ser de 1 a 7 días.");
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
