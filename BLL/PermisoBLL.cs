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
    public class PermisoBLL
    {
        private PermisoDAL permisoDAL = new PermisoDAL();
        private RegistroBLL registroBLL = new RegistroBLL();
        private readonly List<string> permisosExclusivosAdmin = new List<string>
        {
            "REALIZAR_BACKUP",
            "GESTION_ROLES",
            "GESTION_IDIOMAS"
        };
        private readonly HashSet<string> rolesEstaticosSistema = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ADMINISTRADOR",
            "USUARIO SIMPLE",
            "GESTION",
            "RECEPCIONISTA",
            "ENTRENADOR",
            "NUTRICIONISTA",
            "VENDEDOR"
        };

        private void ValidarPermisoGestionRoles()
        {
            UsuarioBE actual = SessionManager.Instancia.UsuarioActual;
            if (actual != null && !actual.TienePermiso("GESTION_ROLES"))
            {
                throw new Exception("Operación denegada: No posee permisos de Gestión de Roles.");
            }
        }

        public bool ExisteReferenciaCircular(ComponentePermiso hijoAAgregar, int idPadreDestino)
        {
            if (hijoAAgregar.ID == idPadreDestino) return true;
            foreach (var nieto in hijoAAgregar.ObtenerHijos())
            {
                if (ExisteReferenciaCircular(nieto, idPadreDestino)) return true;
            }
            return false;
        }

        public List<ComponentePermiso> ObtenerArbolCompleto()
        {
            var catalogo = permisoDAL.ObtenerComponentesBases();
            foreach (var comp in catalogo)
            {
                if (comp is FamiliaBE familia)
                {
                    permisoDAL.LlenarFamiliaComponentes(familia, catalogo);
                }
            }
            return catalogo;
        }

        public List<ComponentePermiso> ObtenerArbolJerarquicoVisual()
        {
            var catalogo = ObtenerArbolCompleto();
            var todosLosHijos = new HashSet<int>();
            foreach (var comp in catalogo)
            {
                if (comp is FamiliaBE familia)
                {
                    foreach (var hijo in familia.ObtenerHijos())
                    {
                        todosLosHijos.Add(hijo.ID);
                    }
                }
            }
            return catalogo.Where(c => !todosLosHijos.Contains(c.ID)).ToList();
        }

        public void GuardarFamiliaCompleta(FamiliaBE familia)
        {
            ValidarPermisoGestionRoles();
            if (familia == null || string.IsNullOrWhiteSpace(familia.Nombre))
                throw new Exception("Debe especificar un nombre válido para el rol.");
            string nombreLimpio = familia.Nombre.Trim();
            bool esRolAdmin = nombreLimpio.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase);
            if (familia.ID == 0)
            {
                var existentes = permisoDAL.ObtenerComponentesBases();
                if (existentes.Any(c => c.Nombre.Trim().Equals(nombreLimpio, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception($"Ya existe un rol o permiso en el sistema con el nombre '{nombreLimpio}'.");
                }
            }
            var hijosUnicos = familia.ObtenerHijos()
                .Where(h => h != null)
                .GroupBy(h => h.ID)
                .Select(g => g.First())
                .ToList();
            if (esRolAdmin)
            {
                foreach (string codExclusivo in permisosExclusivosAdmin)
                {
                    bool conservaPatente = hijosUnicos.Any(h => !string.IsNullOrEmpty(h.PermisoCodigo) && h.PermisoCodigo.Equals(codExclusivo, StringComparison.OrdinalIgnoreCase));
                    if (!conservaPatente)
                    {
                        throw new Exception($"Operación protegida: No se puede quitar el permiso crítico '{codExclusivo}' del rol Administrador.");
                    }
                }
            }
            foreach (var hijo in hijosUnicos)
            {
                if (!esRolAdmin && !string.IsNullOrEmpty(hijo.PermisoCodigo) && permisosExclusivosAdmin.Contains(hijo.PermisoCodigo.ToUpper()))
                {
                    throw new Exception($"Violación de Privilegios: La patente '{hijo.Nombre}' es exclusiva del Administrador. No puede ser delegada ni incluida en el rol '{familia.Nombre}'.");
                }
                if (familia.ID > 0 && ExisteReferenciaCircular(hijo, familia.ID))
                {
                    throw new Exception($"Error de Referencia Circular detectado al intentar agregar {hijo.Nombre} a {familia.Nombre}.");
                }
            }
            if (familia.ID == 0)
            {
                familia.ID = permisoDAL.GuardarFamilia(familia);
            }
            else
            {
                permisoDAL.BorrarRelacionesFamilia(familia.ID);
            }
            foreach (var hijo in hijosUnicos)
            {
                permisoDAL.GuardarRelacionFamiliaPermiso(familia.ID, hijo.ID);
            }
        }

        public void EliminarFamilia(FamiliaBE familia)
        {
            ValidarPermisoGestionRoles();
            if (familia == null) return;
            if (rolesEstaticosSistema.Contains(familia.Nombre.Trim()))
            {
                throw new Exception("Los roles base y operativos del gimnasio son estáticos y no pueden ser eliminados.");
            }
            if (permisoDAL.RolAsignadoAAlgunUsuario(familia.ID))
            {
                throw new Exception("No se puede eliminar el rol porque se encuentra asignado a uno o más usuarios activos.");
            }
            permisoDAL.EliminarFamilia(familia.ID);
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                registroBLL.RegistrarEvento($"Rol '{familia.Nombre}' eliminado del sistema.", SessionManager.Instancia.UsuarioActual, "ALTA");
            }
        }

        public void ActualizarPermisosUsuario(UsuarioBE usuario)
        {
            if (usuario.Permisos.Count == 0)
                throw new Exception("Un usuario debe tener asignado al menos un rol.");
            UsuarioBE autor = SessionManager.Instancia.UsuarioActual;
            if (autor != null && autor.ID != usuario.ID)
            {
                if (!autor.TienePermiso("GESTION_ROLES"))
                    throw new Exception("Operación denegada: No posee permisos para modificar los roles de otros usuarios.");
                if (autor.NivelJerarquia < usuario.NivelJerarquia)
                    throw new Exception($"Operación de Seguridad Denegada: Su nivel de jerarquía ({autor.NivelJerarquia}) es inferior al del usuario '{usuario.Nombre}' ({usuario.NivelJerarquia}).");
            }
            var catalogo = ObtenerArbolCompleto();
            var rolAdmin = catalogo.Find(x => x.Nombre.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase));
            int idRolAdmin = rolAdmin != null ? rolAdmin.ID : 2;
            bool tieneAdminAhora = usuario.Permisos.Exists(p => p.ID == idRolAdmin);
            if (!tieneAdminAhora)
            {
                var permisosAnteriores = permisoDAL.ObtenerPermisosDeUsuario(usuario.ID, catalogo);
                bool eraAdmin = permisosAnteriores.Exists(p => p.ID == idRolAdmin);
                if (eraAdmin)
                {
                    int adminsRestantes = permisoDAL.CantidadUsuariosConRol(idRolAdmin);
                    if (adminsRestantes <= 1)
                        throw new Exception("Operación denegada. El sistema debe mantener al menos un usuario con el rol de Administrador.");
                }
            }
            foreach (var permiso in usuario.Permisos)
            {
                if (!string.IsNullOrEmpty(permiso.PermisoCodigo) && permisosExclusivosAdmin.Contains(permiso.PermisoCodigo.ToUpper()))
                {
                    throw new Exception($"Violación de Privilegios: La patente '{permiso.Nombre}' se hereda automáticamente al poseer el rol Administrador. Está estrictamente prohibido asignarla como un permiso suelto a un usuario.");
                }
            }
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            if (tieneAdminAhora && usuario.NivelJerarquia != 100)
            {
                usuario.NivelJerarquia = 100;
                usuario.DVH = DVManager.CalcularDVH(usuario);
                usuarioDAL.ActualizarJerarquiaYDVH(usuario.ID, usuario.NivelJerarquia, usuario.DVH);
                new IntegridadBLL().ActualizarDVVGeneral();
                usuarioDAL.GuardarEstadoHistorico(new UsuarioHistoricoBE
                {
                    ID = usuario.ID,
                    Nombre = usuario.Nombre,
                    Clave = usuario.Clave,
                    IntentosFallidos = usuario.IntentosFallidos,
                    Bloqueado = usuario.Bloqueado,
                    NivelJerarquia = usuario.NivelJerarquia,
                    ID_Usuario_Autor = autor != null ? autor.ID : usuario.ID,
                    FechaHora = DateTime.Now,
                    Accion = "Promoción a rol Administrador (Jerarquía 100)"
                });
            }
            else if (!tieneAdminAhora && usuario.NivelJerarquia == 100)
            {
                usuario.NivelJerarquia = 99;
                usuario.DVH = DVManager.CalcularDVH(usuario);
                usuarioDAL.ActualizarJerarquiaYDVH(usuario.ID, usuario.NivelJerarquia, usuario.DVH);
                new IntegridadBLL().ActualizarDVVGeneral();
                usuarioDAL.GuardarEstadoHistorico(new UsuarioHistoricoBE
                {
                    ID = usuario.ID,
                    Nombre = usuario.Nombre,
                    Clave = usuario.Clave,
                    IntentosFallidos = usuario.IntentosFallidos,
                    Bloqueado = usuario.Bloqueado,
                    NivelJerarquia = usuario.NivelJerarquia,
                    ID_Usuario_Autor = autor != null ? autor.ID : usuario.ID,
                    FechaHora = DateTime.Now,
                    Accion = "Revocación de rol Administrador (Jerarquía 99)"
                });
            }
            permisoDAL.LimpiarPermisosUsuario(usuario.ID);
            foreach (var permiso in usuario.Permisos.GroupBy(p => p.ID).Select(g => g.First()))
            {
                permisoDAL.AsignarPermisoUsuario(usuario.ID, permiso.ID);
            }
            if (autor != null)
            {
                registroBLL.RegistrarEvento($"Actualización de roles/permisos del usuario '{usuario.Nombre}'.", autor, "ALTA");
            }
        }

        public void LlenarPermisosDeUsuario(UsuarioBE usuario)
        {
            var catalogo = ObtenerArbolCompleto();
            var permisosBD = permisoDAL.ObtenerPermisosDeUsuario(usuario.ID, catalogo);
            usuario.Permisos = permisosBD;
        }

        public List<PatenteBE> ObtenerTodasLasPatentes()
        {
            var componentes = ObtenerArbolCompleto();
            var patentes = new List<PatenteBE>();
            foreach (var c in componentes)
            {
                if (c is PatenteBE patente)
                {
                    patentes.Add(patente);
                }
            }
            return patentes;
        }

        public void CrearRolDentroDeRol(FamiliaBE padre, string nombreNuevoRol)
        {
            ValidarPermisoGestionRoles();
            if (padre == null)
                throw new Exception("Debe seleccionar un rol padre válido.");
            FamiliaBE nuevoHijo = new FamiliaBE { Nombre = nombreNuevoRol.Trim() };
            GuardarFamiliaCompleta(nuevoHijo);
            try
            {
                padre.AgregarHijo(nuevoHijo);
                GuardarFamiliaCompleta(padre);
            }
            catch
            {
                padre.RemoverHijo(nuevoHijo);
                if (nuevoHijo.ID > 0)
                {
                    permisoDAL.EliminarFamilia(nuevoHijo.ID);
                }
                throw;
            }
        }
    }
}