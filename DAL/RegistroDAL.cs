using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class RegistroDAL
    {
        private readonly UnidadTrabajoSeguridadDAL? unidad;

        public RegistroDAL(UnidadTrabajoSeguridadDAL? unidad = null)
        {
            this.unidad = unidad;
        }

        public void GuardarRegistro(RegistroBE pRegistro, UsuarioBE pUsuario)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string consulta = "INSERT INTO Bitacora (ID_Usuario, FechaHora, Evento, Criticidad) VALUES (@ID_Usuario, @FechaHora, @Evento, @Criticidad)";
                using (SqlCommand cmd = acceso.CrearComando(consulta))
                {
                    cmd.Parameters.AddWithValue("@ID_Usuario", pUsuario.ID);
                    cmd.Parameters.AddWithValue("@FechaHora", pRegistro.FechaHora);
                    cmd.Parameters.AddWithValue("@Evento", pRegistro.Evento);
                    cmd.Parameters.AddWithValue("@Criticidad", pRegistro.Criticidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<RegistroBE> ConsultaLogsFiltros(DateTime desde, DateTime hasta, string criticidad, int? idUsuario, string palabraClave)
        {
            List<RegistroBE> lr = new List<RegistroBE>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                StringBuilder consulta = new StringBuilder(@"
                SELECT b.ID_Reg, b.ID_Usuario, b.FechaHora, b.Evento, b.Criticidad, u.Nombre as NombreUsuario 
                FROM Bitacora b 
                INNER JOIN Usuario u ON b.ID_Usuario = u.ID 
                WHERE 1=1");
                using (SqlCommand cmd = acceso.CrearComando())
                {
                    consulta.Append(" AND b.FechaHora >= @Desde AND b.FechaHora <= @Hasta");
                    cmd.Parameters.AddWithValue("@Desde", desde.Date);
                    cmd.Parameters.AddWithValue("@Hasta", hasta.Date.AddDays(1).AddSeconds(-1));
                    if (!string.IsNullOrEmpty(criticidad) && criticidad != "Todos")
                    {
                        consulta.Append(" AND b.Criticidad = @Criticidad");
                        cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                    }
                    if (idUsuario.HasValue && idUsuario.Value > 0)
                    {
                        consulta.Append(" AND b.ID_Usuario = @IdUsuario");
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(palabraClave))
                    {
                        consulta.Append(" AND b.Evento LIKE @Palabra");
                        cmd.Parameters.AddWithValue("@Palabra", "%" + palabraClave + "%");
                    }
                    consulta.Append(" ORDER BY b.FechaHora DESC");
                    cmd.CommandText = consulta.ToString();
                    cmd.Connection = cn;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RegistroBE registro = new RegistroBE();
                            registro.ID_Registro = Convert.ToInt32(reader["ID_Reg"]);
                            registro.ID_Usuario = Convert.ToInt32(reader["ID_Usuario"]);
                            registro.FechaHora = Convert.ToDateTime(reader["FechaHora"]);
                            registro.Evento = reader["Evento"].ToString();
                            registro.Criticidad = reader["Criticidad"].ToString();
                            registro.NombreUsuario = reader["NombreUsuario"].ToString();
                            lr.Add(registro);
                        }
                    }
                }
            }
            return lr;
        }
    }
}
