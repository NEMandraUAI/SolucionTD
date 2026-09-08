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
        private readonly List<string> permisosExclusivosAdmin = new List<string>
        {
            "REALIZAR_BACKUP",
            "GESTION_ROLES",
            "GESTION_IDIOMAS"
        };
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
            if (familia.Nombre.ToUpper() == "ADMINISTRADOR" || familia.Nombre.ToUpper() == "RECEPCIONISTA" || familia.Nombre.ToUpper() == "ENTRENADOR")
            {
                throw new Exception("Los roles operativos del gimnasio son estáticos y no pueden ser eliminados.");
            }
            if (familia.ID == 0)
            {
                familia.ID = permisoDAL.GuardarFamilia(familia);
            }
            else
            {
                permisoDAL.BorrarRelacionesFamilia(familia.ID);
            }
            foreach (var hijo in familia.ObtenerHijos())
            {
                if (!string.IsNullOrEmpty(hijo.PermisoCodigo) && permisosExclusivosAdmin.Contains(hijo.PermisoCodigo.ToUpper()))
                {
                    throw new Exception($"Violación de Privilegios: La patente '{hijo.Nombre}' es exclusiva del Administrador. No puede ser delegada ni incluida en el rol personalizado '{familia.Nombre}'.");
                }
                if (ExisteReferenciaCircular(hijo, familia.ID))
                    throw new Exception($"Error de Referencia Circular detectado al intentar agregar {hijo.Nombre} a {familia.Nombre}.");
                permisoDAL.GuardarRelacionFamiliaPermiso(familia.ID, hijo.ID);
            }
        }
        public void EliminarFamilia(FamiliaBE familia)
        {
            if (familia.Nombre.ToUpper() == "ADMINISTRADOR" || familia.Nombre.ToUpper() == "RECEPCIONISTA" || familia.Nombre.ToUpper() == "ENTRENADOR")
            {
                throw new Exception("Los roles operativos del gimnasio son estáticos y no pueden ser eliminados.");
            }
            if (permisoDAL.RolAsignadoAAlgunUsuario(familia.ID))
                throw new Exception("No se puede eliminar el rol porque se encuentra asignado a uno o más usuarios activos.");
            permisoDAL.EliminarFamilia(familia.ID);
        }
        public void ActualizarPermisosUsuario(UsuarioBE usuario)
        {
            if (usuario.Permisos.Count == 0)
                throw new Exception("Un usuario debe tener asignado al menos un rol.");
            int idRolAdmin = ObtenerArbolCompleto().Find(x => x.Nombre.ToUpper() == "ADMINISTRADOR").ID;
            bool tieneAdminAhora = usuario.Permisos.Exists(p => p.ID == idRolAdmin);
            if (!tieneAdminAhora)
            {
                var catalog = ObtenerArbolCompleto();
                var permisosAnteriores = permisoDAL.ObtenerPermisosDeUsuario(usuario.ID, catalog);
                bool eraAdmin = permisosAnteriores.Exists(p => p.ID == idRolAdmin);
                if (eraAdmin)
                {
                    int adminsRestantes = permisoDAL.CantidadUsuariosConRol(idRolAdmin);
                    if (adminsRestantes <= 1)
                        throw new Exception("Operación denegada. El sistema debe mantener al menos un usuario con el rol de Administrador.");
                }
            }
            if (tieneAdminAhora && usuario.NivelJerarquia != 100)
            {
                usuario.NivelJerarquia = 100;
                usuario.DVH = DVManager.CalcularDVH(usuario);
                UsuarioDAL usuarioDAL = new UsuarioDAL();
                usuarioDAL.ActualizarJerarquiaYDVH(usuario.ID, usuario.NivelJerarquia, usuario.DVH);
                IntegridadBLL intBLL = new IntegridadBLL();
                intBLL.ActualizarDVVGeneral();
            }
            else if (!tieneAdminAhora && usuario.NivelJerarquia == 100)
            {
                usuario.NivelJerarquia = 99;
                usuario.DVH = DVManager.CalcularDVH(usuario);
                UsuarioDAL usuarioDAL = new UsuarioDAL();
                usuarioDAL.ActualizarJerarquiaYDVH(usuario.ID, usuario.NivelJerarquia, usuario.DVH);
                IntegridadBLL intBLL = new IntegridadBLL();
                intBLL.ActualizarDVVGeneral();
            }
            permisoDAL.LimpiarPermisosUsuario(usuario.ID);
            foreach (var permiso in usuario.Permisos)
            {
                if (!string.IsNullOrEmpty(permiso.PermisoCodigo) && permisosExclusivosAdmin.Contains(permiso.PermisoCodigo.ToUpper()))
                {
                    throw new Exception($"Violación de Privilegios: La patente '{permiso.Nombre}' se hereda automáticamente al poseer el rol Administrador. Está estrictamente prohibido asignarla como un permiso suelto a un usuario.");
                }
                permisoDAL.AsignarPermisoUsuario(usuario.ID, permiso.ID);
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
            FamiliaBE nuevoHijo = new FamiliaBE { Nombre = nombreNuevoRol };
            GuardarFamiliaCompleta(nuevoHijo);
            padre.AgregarHijo(nuevoHijo);
            GuardarFamiliaCompleta(padre);
        }
    }
}
