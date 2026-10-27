using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PermisoDAL
    {
        private readonly UnidadTrabajoSeguridadDAL? unidad;

        public PermisoDAL(UnidadTrabajoSeguridadDAL? unidad = null)
        {
            this.unidad = unidad;
        }

        public List<ComponentePermiso> ObtenerComponentesBases()
        {
            List<ComponentePermiso> lista = new List<ComponentePermiso>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "SELECT ID, Nombre, PermisoCodigo, EsFamilia FROM Permiso";
                using (SqlCommand cmd = acceso.CrearComando(query))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);
                        ComponentePermiso c;
                        if (esFamilia)
                            c = new FamiliaBE();
                        else
                            c = new PatenteBE();

                        c.ID = Convert.ToInt32(reader["ID"]);
                        c.Nombre = reader["Nombre"].ToString();
                        c.PermisoCodigo = reader["PermisoCodigo"] != DBNull.Value ? reader["PermisoCodigo"].ToString() : string.Empty;
                        lista.Add(c);
                    }
                }
            }
            return lista;
        }
        public List<ComponentePermiso> ObtenerArbolCompleto()
        {
            if (unidad == null)
            {
                using var lectura = new UnidadTrabajoSeguridadDAL();
                var resultado = new PermisoDAL(lectura).ObtenerArbolCompleto();
                lectura.Confirmar();
                return resultado;
            }
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando(@"SELECT ID, Nombre, PermisoCodigo, EsFamilia FROM dbo.Permiso ORDER BY ID;
                SELECT ID_Padre, ID_Hijo FROM dbo.Permiso_Permiso ORDER BY ID_Padre, ID_Hijo;");
            using var reader = cmd.ExecuteReader();
            var componentes = new Dictionary<int, ComponentePermiso>();
            while (reader.Read())
            {
                ComponentePermiso componente = reader.GetBoolean(3) ? new FamiliaBE() : new PatenteBE();
                componente.ID = reader.GetInt32(0);
                componente.Nombre = reader.GetString(1);
                componente.PermisoCodigo = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                componentes.Add(componente.ID, componente);
            }
            var hijos = componentes.Keys.ToDictionary(id => id, _ => new List<int>());
            var entradas = componentes.Keys.ToDictionary(id => id, _ => 0);
            if (!reader.NextResult()) throw new InvalidOperationException("No se pudo leer la estructura de permisos.");
            while (reader.Read())
            {
                int padre = reader.GetInt32(0);
                int hijo = reader.GetInt32(1);
                if (!componentes.TryGetValue(padre, out var componentePadre) || !componentes.ContainsKey(hijo))
                    throw new InvalidOperationException("La estructura de permisos contiene referencias inexistentes.");
                if (componentePadre is not FamiliaBE)
                    throw new InvalidOperationException("La estructura de permisos contiene una patente con hijos.");
                if (hijos[padre].Contains(hijo))
                    throw new InvalidOperationException("La estructura de permisos contiene una relación repetida.");
                hijos[padre].Add(hijo);
                entradas[hijo]++;
            }
            // Orden topológico iterativo: una estructura corrupta no debe agotar
            // la pila ni abrir conexiones SQL recursivas.
            var pendientes = new Queue<int>(entradas.Where(p => p.Value == 0).Select(p => p.Key));
            var profundidad = componentes.Keys.ToDictionary(id => id, _ => 0);
            int procesados = 0;
            while (pendientes.Count > 0)
            {
                int padre = pendientes.Dequeue();
                procesados++;
                foreach (int hijo in hijos[padre])
                {
                    profundidad[hijo] = Math.Max(profundidad[hijo], profundidad[padre] + 1);
                    if (profundidad[hijo] > 256)
                        throw new InvalidOperationException("La estructura de permisos supera la profundidad máxima admitida (256).");
                    if (--entradas[hijo] == 0) pendientes.Enqueue(hijo);
                }
            }
            if (procesados != componentes.Count)
                throw new InvalidOperationException("Se detectó una referencia circular en los permisos. Contacte al administrador.");
            foreach (var relacion in hijos)
                foreach (int hijo in relacion.Value)
                    ((FamiliaBE)componentes[relacion.Key]).AgregarHijo(componentes[hijo]);
            return componentes.Values.OrderBy(p => p.ID).ToList();
        }

        public void LlenarFamiliaComponentes(FamiliaBE familia, List<ComponentePermiso> catalogoBases)
        {
            if (familia == null || catalogoBases == null)
                throw new ArgumentException("Debe proporcionar una familia y un catálogo válidos.");
            var completo = ObtenerArbolCompleto();
            var destino = catalogoBases.ToDictionary(p => p.ID);
            if (!destino.TryGetValue(familia.ID, out var padre) || padre is not FamiliaBE)
                throw new InvalidOperationException("La familia no pertenece al catálogo proporcionado.");
            foreach (var existente in catalogoBases.OfType<FamiliaBE>()) existente.VaciarHijos();
            foreach (var original in completo.OfType<FamiliaBE>())
            {
                if (!destino.TryGetValue(original.ID, out var nuevoPadre) || nuevoPadre is not FamiliaBE familiaDestino)
                    throw new InvalidOperationException("El catálogo cambió. Actualice la pantalla de permisos.");
                foreach (var hijo in original.ObtenerHijos())
                {
                    if (!destino.TryGetValue(hijo.ID, out var nuevoHijo))
                        throw new InvalidOperationException("El catálogo cambió. Actualice la pantalla de permisos.");
                    familiaDestino.AgregarHijo(nuevoHijo);
                }
            }
        }

        public int GuardarFamilia(FamiliaBE familia)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "INSERT INTO Permiso (Nombre, EsFamilia) VALUES (@Nombre, 1); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@Nombre", familia.Nombre);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public void EliminarFamilia(int idFamilia)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                using (SqlCommand cmd1 = acceso.CrearComando("DELETE FROM Permiso_Permiso WHERE ID_Padre = @ID OR ID_Hijo = @ID"))
                {
                    cmd1.Parameters.AddWithValue("@ID", idFamilia);
                    cmd1.ExecuteNonQuery();
                }
                using (SqlCommand cmd2 = acceso.CrearComando("DELETE FROM Permiso WHERE ID = @ID"))
                {
                    cmd2.Parameters.AddWithValue("@ID", idFamilia);
                    cmd2.ExecuteNonQuery();
                }
            }
        }
        public void GuardarRelacionFamiliaPermiso(int idPadre, int idHijo)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "INSERT INTO Permiso_Permiso (ID_Padre, ID_Hijo) VALUES (@Padre, @Hijo)";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@Padre", idPadre);
                    cmd.Parameters.AddWithValue("@Hijo", idHijo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void BorrarRelacionesFamilia(int idPadre)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "DELETE FROM Permiso_Permiso WHERE ID_Padre = @ID";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@ID", idPadre);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AsignarPermisoUsuario(int idUsuario, int idPermiso)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "INSERT INTO Usuario_Permiso (ID_Usuario, ID_Permiso) VALUES (@Usu, @Per)";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@Usu", idUsuario);
                    cmd.Parameters.AddWithValue("@Per", idPermiso);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void LimpiarPermisosUsuario(int idUsuario)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "DELETE FROM Usuario_Permiso WHERE ID_Usuario = @Usu";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@Usu", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool RolAsignadoAAlgunUsuario(int idPermiso)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "SELECT COUNT(*) FROM Usuario_Permiso WHERE ID_Permiso = @ID";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@ID", idPermiso);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
        public List<ComponentePermiso> ObtenerPermisosDeUsuario(int idUsuario, List<ComponentePermiso> catalogoBases)
        {
            List<ComponentePermiso> asignados = new List<ComponentePermiso>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "SELECT ID_Permiso FROM Usuario_Permiso WHERE ID_Usuario = @Usu";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@Usu", idUsuario);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPermiso = Convert.ToInt32(reader["ID_Permiso"]);
                            var comp = catalogoBases.FirstOrDefault(c => c.ID == idPermiso);
                            if (comp == null) throw new InvalidOperationException("El usuario tiene un permiso que no existe en el catálogo.");
                            asignados.Add(comp);
                        }
                    }
                }
            }
            return asignados;
        }
        public int CantidadUsuariosConRol(int idRol)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cn = acceso.Conexion;
                string query = "SELECT COUNT(*) FROM dbo.Usuario_Permiso up JOIN dbo.Usuario u ON u.ID=up.ID_Usuario WHERE up.ID_Permiso=@ID AND u.Bloqueado=0 AND u.NivelJerarquia=100";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@ID", idRol);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
