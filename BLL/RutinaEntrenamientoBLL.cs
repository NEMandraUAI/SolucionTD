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
    }
}
