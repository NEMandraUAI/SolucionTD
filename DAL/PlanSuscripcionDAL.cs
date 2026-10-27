using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PlanSuscripcionDAL
    {
        private readonly UnidadTrabajoSeguridadDAL? unidad;
        public PlanSuscripcionDAL(UnidadTrabajoSeguridadDAL? unidad = null) { this.unidad = unidad; }
        public List<PlanSuscripcionBE> ListarPlanes()
        {
            List<PlanSuscripcionBE> lista = new List<PlanSuscripcionBE>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                using SqlCommand cmd = acceso.CrearComando("SELECT CodigoPlan, Nombre, Precio, DuracionDias, DVH FROM PlanSuscripcion ORDER BY CodigoPlan ASC");
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        PlanSuscripcionBE plan = new PlanSuscripcionBE();
                        plan.CodigoPlan = Convert.ToInt32(dr["CodigoPlan"]);
                        plan.Nombre = dr["Nombre"].ToString();
                        plan.Precio = Convert.ToDecimal(dr["Precio"]);
                        plan.DuracionDias = Convert.ToInt32(dr["DuracionDias"]);
                        plan.DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : null;
                        lista.Add(plan);
                    }
                }
            }
            return lista;
        }
    }
}
