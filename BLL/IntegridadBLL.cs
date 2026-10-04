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
    public class IntegridadBLL
    {
        private IntegridadDAL integridadDAL = new IntegridadDAL();
        private readonly string[] tablasConDV = {
            "Usuario",
            "ComprobantePago",
            "PlanNutricional",
            "PlanSuscripcion",
            "RutinaEntrenamiento",
            "Socio",
            "Turno"
        };
        public void VerificarIntegridadSistema()
        {
            foreach (string tabla in tablasConDV)
            {
                List<IEntidadVerificable> registros = ObtenerRegistrosDeTabla(tabla);
                List<string> dvhCalculados = new List<string>();
                foreach (var reg in registros)
                {
                    string dvhReal = DVManager.CalcularDVH(reg);
                    if (dvhReal != reg.DVH)
                    {
                        throw new Exception($"Error de Integridad: El registro con ID {reg.ObtenerID()} de la tabla {tabla} ha sido alterado externamente.");
                    }
                    dvhCalculados.Add(dvhReal);
                }
                string dvvCalculado = DVManager.CalcularDVV(dvhCalculados);
                string dvvBaseDatos = integridadDAL.ObtenerDVV(tabla);
                if (dvvBaseDatos != null && dvvCalculado != dvvBaseDatos)
                {
                    throw new Exception($"Error de Integridad: Se han insertado, eliminado o reordenado registros en la tabla {tabla}.");
                }
            }
        }
        public void ActualizarDVVGeneral()
        {
            foreach (string tabla in tablasConDV)
            {
                var registros = ObtenerRegistrosDeTabla(tabla);
                var dvhs = registros.Select(r => r.DVH).ToList();
                string nuevoDVV = DVManager.CalcularDVV(dvhs);
                integridadDAL.ActualizarDVV(tabla, nuevoDVV);
            }
        }

        public void ForzarRecalculoDeTodaLaBase()
        {
            foreach (string tabla in tablasConDV)
            {
                var registros = ObtenerRegistrosDeTabla(tabla);
                List<string> dvhsCalculados = new List<string>();
                foreach (var reg in registros)
                {
                    string dvhCorrecto = DVManager.CalcularDVH(reg);
                    reg.DVH = dvhCorrecto;
                    integridadDAL.ActualizarDVHRegistro(tabla, int.Parse(reg.ObtenerID()), dvhCorrecto);
                    dvhsCalculados.Add(dvhCorrecto);
                }
                string dvvCorrecto = DVManager.CalcularDVV(dvhsCalculados);
                integridadDAL.ActualizarDVV(tabla, dvvCorrecto);
            }
        }
        private List<IEntidadVerificable> ObtenerRegistrosDeTabla(string tabla)
        {
            switch (tabla)
            {
                case "Usuario": return new UsuarioDAL().ListarTodos().Cast<IEntidadVerificable>().ToList();
                case "Socio": return new SocioDAL().LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "Turno": return new TurnoDAL().LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "ComprobantePago": return new ComprobantePagoDAL().LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "PlanNutricional": return new PlanNutricionalDAL().LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "PlanSuscripcion": return new PlanSuscripcionDAL().ListarPlanes().Cast<IEntidadVerificable>().ToList();
                case "RutinaEntrenamiento": return new RutinaEntrenamientoDAL().LeerTodos().Cast<IEntidadVerificable>().ToList();
                default: return new List<IEntidadVerificable>();
            }
        }
    }
}
