using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class UsuarioDAL
    {
        public UsuarioBE ObtenerPorUsername(string nombreUsuario)
        {
            UsuarioBE usuario = null;
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_Usuario_ObtenerPorUsername", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", nombreUsuario);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new UsuarioBE();
                            usuario.ID = Convert.ToInt32(reader["ID"]);
                            usuario.Nombre = reader["Nombre"].ToString();
                            usuario.Clave = reader["Clave"].ToString();
                            usuario.IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]);
                            usuario.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                            usuario.DVH = reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null;
                            usuario.Idioma = reader["ID_Idioma"] != DBNull.Value ? new IdiomaBE { ID = Convert.ToInt32(reader["ID_Idioma"]) } : null;
                            usuario.NivelJerarquia = Convert.ToInt32(reader["NivelJerarquia"]);
                        }
                    }
                }
            }
            return usuario;
        }
        public void ActualizarEstadoUsuario(UsuarioBE usuario)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_Usuario_ActualizarEstado", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Intentos", usuario.IntentosFallidos);
                    cmd.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                    cmd.Parameters.AddWithValue("@DVH", (object)usuario.DVH ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID", usuario.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<UsuarioBE> ListarTodos()
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "SELECT ID, Nombre, Clave, IntentosFallidos, Bloqueado, DVH, ID_Idioma, NivelJerarquia FROM Usuario ORDER BY ID ASC";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UsuarioBE usuario = new UsuarioBE();
                            usuario.ID = Convert.ToInt32(reader["ID"]);
                            usuario.Nombre = reader["Nombre"].ToString();
                            usuario.Clave = reader["Clave"].ToString();
                            usuario.IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]);
                            usuario.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                            usuario.DVH = reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null;
                            usuario.Idioma = reader["ID_Idioma"] != DBNull.Value ? new IdiomaBE { ID = Convert.ToInt32(reader["ID_Idioma"]) } : null;
                            usuario.NivelJerarquia = Convert.ToInt32(reader["NivelJerarquia"]);
                            lista.Add(usuario);
                        }
                    }
                }
            }
            return lista;
        }
        public int CrearUsuario(UsuarioBE nuevoUsuario)
        {
            int nuevoID = 0;
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "INSERT INTO Usuario (Nombre, Clave, IntentosFallidos, Bloqueado, NivelJerarquia) VALUES (@Nombre, @Clave, 0, 0, 1); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nuevoUsuario.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", nuevoUsuario.Clave);
                    nuevoID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return nuevoID;
        }
        public void ActualizarDVH(int idUsuario, string dvh)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "UPDATE Usuario SET DVH = @DVH WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@DVH", dvh);
                    cmd.Parameters.AddWithValue("@ID", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public UsuarioBE ObtenerPorID(int idUsuario)
        {
            UsuarioBE usuario = null;
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "SELECT ID, Nombre, Clave, IntentosFallidos, Bloqueado, DVH, ID_Idioma, NivelJerarquia FROM Usuario WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@ID", idUsuario);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new UsuarioBE
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]),
                                Bloqueado = Convert.ToBoolean(reader["Bloqueado"]),
                                DVH = reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null,
                                Idioma = reader["ID_Idioma"] != DBNull.Value ? new IdiomaBE { ID = Convert.ToInt32(reader["ID_Idioma"]) } : null,
                                NivelJerarquia = Convert.ToInt32(reader["NivelJerarquia"])
                            };
                        }
                    }
                }
            }
            return usuario;
        }
        public void GuardarEstadoHistorico(UsuarioHistoricoBE historico)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = @"INSERT INTO Usuario_Historico 
                                    (ID_Usuario, Nombre, Clave, IntentosFallidos, Bloqueado, ID_Usuario_Autor, FechaHora, Accion, NivelJerarquia) 
                                    VALUES (@ID_Usu, @Nombre, @Clave, @Intentos, @Bloqueado, @ID_Autor, @Fecha, @Accion, @NivelJerarquia)";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@ID_Usu", historico.ID);
                    cmd.Parameters.AddWithValue("@Nombre", historico.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", historico.Clave);
                    cmd.Parameters.AddWithValue("@Intentos", historico.IntentosFallidos);
                    cmd.Parameters.AddWithValue("@Bloqueado", historico.Bloqueado);
                    cmd.Parameters.AddWithValue("@ID_Autor", historico.ID_Usuario_Autor);
                    cmd.Parameters.AddWithValue("@Fecha", historico.FechaHora);
                    cmd.Parameters.AddWithValue("@Accion", historico.Accion);
                    cmd.Parameters.AddWithValue("@NivelJerarquia", historico.NivelJerarquia);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<UsuarioHistoricoBE> ListarHistorial(int idUsuario)
        {
            List<UsuarioHistoricoBE> lista = new List<UsuarioHistoricoBE>();
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = @"SELECT h.*, u.Nombre AS NombreAutor 
                                     FROM Usuario_Historico h
                                     INNER JOIN Usuario u ON h.ID_Usuario_Autor = u.ID
                                     WHERE h.ID_Usuario = @ID_Usuario ORDER BY h.FechaHora DESC";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new UsuarioHistoricoBE
                            {
                                ID_Cambio = Convert.ToInt32(reader["ID_Cambio"]),
                                ID = Convert.ToInt32(reader["ID_Usuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]),
                                Bloqueado = Convert.ToBoolean(reader["Bloqueado"]),
                                ID_Usuario_Autor = Convert.ToInt32(reader["ID_Usuario_Autor"]),
                                NombreUsuarioAutor = reader["NombreAutor"].ToString(),
                                FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                                Accion = reader["Accion"].ToString(),
                                NivelJerarquia = Convert.ToInt32(reader["NivelJerarquia"])
                            });
                        }
                    }
                }
            }
            return lista;
        }
        public void ActualizarUsuarioCompleto(UsuarioBE usuario)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = @"UPDATE Usuario 
                                    SET Nombre = @Nombre, 
                                    Clave = @Clave, 
                                    IntentosFallidos = @Intentos, 
                                    Bloqueado = @Bloqueado,
                                    DVH = @DVH,
                                    ID_Idioma = @ID_Idioma,
                                    NivelJerarquia = @NivelJerarquia
                                    WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Intentos", usuario.IntentosFallidos);
                    cmd.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                    cmd.Parameters.AddWithValue("@DVH", (object)usuario.DVH ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID_Idioma", usuario.Idioma != null ? (object)usuario.Idioma.ID : DBNull.Value);
                    cmd.Parameters.AddWithValue("@NivelJerarquia", usuario.NivelJerarquia);
                    cmd.Parameters.AddWithValue("@ID", usuario.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ActualizarIdioma(int idUsuario, int idIdioma)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string query = "UPDATE Usuario SET ID_Idioma = @ID_Idioma WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ID_Idioma", idIdioma);
                    cmd.Parameters.AddWithValue("@ID", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ActualizarJerarquiaYDVH(int idUsuario, int nivelJerarquia, string dvh)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "UPDATE Usuario SET NivelJerarquia = @Nivel, DVH = @DVH WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@Nivel", nivelJerarquia);
                    cmd.Parameters.AddWithValue("@DVH", dvh);
                    cmd.Parameters.AddWithValue("@ID", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
