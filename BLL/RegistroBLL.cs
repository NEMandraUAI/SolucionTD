using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RegistroBLL
    {
        private readonly RegistroDAL registroDAL;

        public RegistroBLL() : this(null) { }

        internal RegistroBLL(UnidadTrabajoSeguridadDAL? unidad)
        {
            registroDAL = new RegistroDAL(unidad);
        }
        public void RegistrarEvento(string evento, UsuarioBE pUsuario, string criticidad = "INFO")
        {
            RegistroBE registro = new RegistroBE();
            registro.FechaHora = DateTime.Now;
            registro.Evento = evento;
            registro.ID_Usuario = pUsuario.ID;
            registro.Criticidad = criticidad;
            registroDAL.GuardarRegistro(registro, pUsuario);
        }
        public List<RegistroBE> ConsultaLogsFiltros(DateTime desde, DateTime hasta, string criticidad, int? idUsuario, string palabraClave) => registroDAL.ConsultaLogsFiltros(desde, hasta, criticidad, idUsuario, palabraClave);
    }
}
