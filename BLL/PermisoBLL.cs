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
        private readonly UnidadTrabajoSeguridadDAL? unidad;
        private readonly PermisoDAL permisoDAL;
        private readonly RegistroBLL registroBLL;

        public PermisoBLL() : this(null) { }

        internal PermisoBLL(UnidadTrabajoSeguridadDAL? unidad)
        {
            this.unidad = unidad;
            permisoDAL = new PermisoDAL(unidad);
            registroBLL = new RegistroBLL(unidad);
        }

        private T Ejecutar<T>(Func<PermisoBLL, T> operacion)
        {
            if (unidad != null) return operacion(this);
            using var transaccion = new UnidadTrabajoSeguridadDAL();
            new IntegridadBLL().VerificarIntegridadUsuario(transaccion);
            T resultado = operacion(new PermisoBLL(transaccion));
            transaccion.Confirmar();
            return resultado;
        }

        public void GuardarFamiliaCompleta(FamiliaBE familia)
        {
            ArgumentNullException.ThrowIfNull(familia);
            var copia = new FamiliaBE { ID = familia.ID, Nombre = familia.Nombre };
            foreach (var hijo in familia.ObtenerHijos()) copia.AgregarHijo(hijo);
            Ejecutar(bll => { bll.GuardarFamiliaCore(copia); return true; });
            familia.ID = copia.ID;
            familia.Nombre = copia.Nombre;
        }

        public void EliminarFamilia(FamiliaBE familia)
        {
            Ejecutar(bll => { bll.EliminarFamiliaCore(familia); return true; });
        }

        public void ActualizarPermisosUsuario(UsuarioBE usuario)
        {
            var confirmado = Ejecutar(bll => bll.ActualizarPermisosCore(usuario));
            usuario.NivelJerarquia = confirmado.NivelJerarquia;
            usuario.DVH = confirmado.DVH;
            usuario.Permisos = confirmado.Permisos;
            if (SessionManager.Instancia.UsuarioActual?.ID == confirmado.ID)
                SessionManager.Instancia.ActualizarUsuarioEnSesion(confirmado);
        }

        public void CrearRolDentroDeRol(FamiliaBE padre, string nombreNuevoRol)
        {
            Ejecutar(bll => { bll.CrearSubrolCore(padre, nombreNuevoRol); return true; });
        }
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

        private UsuarioBE ValidarPermisoGestionRoles()
        {
            UsuarioBE actual = SessionManager.Instancia.UsuarioActual;
            if (actual == null || !actual.TienePermiso("GESTION_ROLES"))
            {
                throw new UnauthorizedAccessException("Operación denegada: Se requiere una sesión con permiso de Gestión de Roles.");
            }
            // La autorización no depende de los datos enviados por el formulario
            // ni de privilegios que hayan sido revocados desde otra sesión.
            UsuarioBE persistido = new UsuarioDAL(unidad).ObtenerPorID(actual.ID);
            if (persistido == null || persistido.Bloqueado)
                throw new UnauthorizedAccessException("La cuenta de la sesión no está disponible para administrar permisos.");
            LlenarPermisosDeUsuario(persistido);
            if (!persistido.TienePermiso("GESTION_ROLES"))
                throw new UnauthorizedAccessException("El permiso de Gestión de Roles ya no está asignado a su cuenta.");
            return persistido;
        }

        private FamiliaBE ResolverRolSistema(List<ComponentePermiso> catalogo, string nombre)
        {
            var coincidencias = catalogo.OfType<FamiliaBE>()
                .Where(p => string.Equals(p.Nombre?.Trim(), nombre, StringComparison.OrdinalIgnoreCase)).ToList();
            if (coincidencias.Count != 1)
                throw new InvalidOperationException($"El catálogo debe contener un único rol '{nombre}'. Contacte al administrador.");
            return coincidencias[0];
        }

        private List<ComponentePermiso> ResolverPermisos(IEnumerable<ComponentePermiso> solicitados, List<ComponentePermiso> catalogo)
        {
            var resultado = new List<ComponentePermiso>();
            foreach (var solicitado in solicitados)
            {
                if (solicitado == null)
                    throw new ArgumentException("La selección contiene un permiso inválido.");
                var persistido = catalogo.SingleOrDefault(p => p.ID == solicitado.ID);
                if (persistido == null)
                    throw new ArgumentException("Uno de los permisos seleccionados ya no existe. Actualice la pantalla.");
                if (resultado.All(p => p.ID != persistido.ID)) resultado.Add(persistido);
            }
            return resultado;
        }

        private void ValidarDescendientes(ComponentePermiso permiso, int idAdministrador, bool permiteExclusivos, HashSet<int> camino)
        {
            if (!camino.Add(permiso.ID))
                throw new InvalidOperationException("La selección contiene una referencia circular de permisos.");
            if (permiso.ID == idAdministrador)
                throw new UnauthorizedAccessException("El rol Administrador no puede anidarse dentro de otro rol.");
            if (!permiteExclusivos && permisosExclusivosAdmin.Contains(permiso.PermisoCodigo ?? string.Empty, StringComparer.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException($"El permiso '{permiso.Nombre}' es exclusivo del rol Administrador y no puede delegarse directa ni indirectamente.");
            foreach (var hijo in permiso.ObtenerHijos())
                ValidarDescendientes(hijo, idAdministrador, permiteExclusivos, camino);
            camino.Remove(permiso.ID);
        }

        internal FamiliaBE ObtenerRolInicialSeguro()
        {
            var catalogo = ObtenerArbolCompleto();
            var simple = ResolverRolSistema(catalogo, "USUARIO SIMPLE");
            var administrador = ResolverRolSistema(catalogo, "ADMINISTRADOR");
            ValidarDescendientes(simple, administrador.ID, false, new HashSet<int>());
            return simple;
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
            return permisoDAL.ObtenerArbolCompleto();
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

        private void GuardarFamiliaCore(FamiliaBE familia)
        {
            var autor = ValidarPermisoGestionRoles();
            if (familia == null || string.IsNullOrWhiteSpace(familia.Nombre))
                throw new Exception("Debe especificar un nombre válido para el rol.");
            if (familia.Nombre.Length > 100)
                throw new ArgumentException("El nombre del rol no puede superar 100 caracteres.");
            var catalogo = ObtenerArbolCompleto();
            var administrador = ResolverRolSistema(catalogo, "ADMINISTRADOR");
            var existente = familia.ID == 0 ? null : catalogo.SingleOrDefault(p => p.ID == familia.ID) as FamiliaBE;
            if (familia.ID != 0 && existente == null)
                throw new ArgumentException("El rol seleccionado no existe.");
            string nombreLimpio = existente?.Nombre ?? familia.Nombre.Trim();
            bool esRolAdmin = existente?.ID == administrador.ID;
            if (familia.ID == 0)
            {
                if (catalogo.Any(c => c.Nombre.Trim().Equals(nombreLimpio, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception($"Ya existe un rol o permiso en el sistema con el nombre '{nombreLimpio}'.");
                }
            }
            var hijosUnicos = ResolverPermisos(familia.ObtenerHijos(), catalogo);
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
                ValidarDescendientes(hijo, administrador.ID, esRolAdmin, new HashSet<int>());
                if (familia.ID > 0 && ExisteReferenciaCircular(hijo, familia.ID))
                {
                    throw new Exception($"Error de Referencia Circular detectado al intentar agregar {hijo.Nombre} a {familia.Nombre}.");
                }
            }
            if (familia.ID == 0)
            {
                familia.Nombre = nombreLimpio;
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
            registroBLL.RegistrarEvento($"Actualización de estructura del rol '{nombreLimpio}'.", autor, "ALTA");
        }

        private void EliminarFamiliaCore(FamiliaBE familia)
        {
            ValidarPermisoGestionRoles();
            if (familia == null) throw new ArgumentException("Debe seleccionar un rol.");
            familia = ObtenerArbolCompleto().SingleOrDefault(p => p.ID == familia.ID) as FamiliaBE
                ?? throw new ArgumentException("El rol seleccionado no existe.");
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

        private UsuarioBE ActualizarPermisosCore(UsuarioBE usuario)
        {
            UsuarioBE autor = ValidarPermisoGestionRoles();
            if (usuario == null || usuario.Permisos == null || usuario.Permisos.Count == 0)
                throw new Exception("Un usuario debe tener asignado al menos un rol.");
            UsuarioBE solicitado = usuario;
            usuario = new UsuarioDAL(unidad).ObtenerPorID(solicitado.ID)
                ?? throw new ArgumentException("El usuario seleccionado no existe.");
            if (autor.NivelJerarquia < usuario.NivelJerarquia)
                throw new UnauthorizedAccessException("Su nivel de jerarquía es inferior al del usuario que intenta modificar.");
            var catalogo = ObtenerArbolCompleto();
            var rolAdmin = ResolverRolSistema(catalogo, "ADMINISTRADOR");
            int idRolAdmin = rolAdmin.ID;
            usuario.Permisos = ResolverPermisos(solicitado.Permisos, catalogo);
            bool tieneAdminAhora = usuario.Permisos.Exists(p => p.ID == idRolAdmin);
            if (tieneAdminAhora && autor.NivelJerarquia < 100)
                throw new UnauthorizedAccessException("Solo un administrador puede asignar el rol Administrador.");
            if (!tieneAdminAhora)
            {
                var permisosAnteriores = permisoDAL.ObtenerPermisosDeUsuario(usuario.ID, catalogo);
                bool eraAdmin = permisosAnteriores.Exists(p => p.ID == idRolAdmin);
                if (eraAdmin && !usuario.Bloqueado && usuario.NivelJerarquia == 100)
                {
                    int adminsRestantes = permisoDAL.CantidadUsuariosConRol(idRolAdmin);
                    if (adminsRestantes <= 1)
                        throw new Exception("Operación denegada. El sistema debe mantener al menos un usuario con el rol de Administrador.");
                }
            }
            foreach (var permiso in usuario.Permisos)
            {
                if (permiso.ID != idRolAdmin)
                    ValidarDescendientes(permiso, idRolAdmin, false, new HashSet<int>());
            }
            UsuarioDAL usuarioDAL = new UsuarioDAL(unidad);
            if (tieneAdminAhora && usuario.NivelJerarquia != 100)
            {
                usuario.NivelJerarquia = 100;
                usuario.DVH = DVManager.CalcularDVH(usuario);
                usuarioDAL.ActualizarJerarquiaYDVH(usuario.ID, usuario.NivelJerarquia, usuario.DVH);
                new IntegridadBLL().ActualizarDVVUsuario(unidad!);
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
                new IntegridadBLL().ActualizarDVVUsuario(unidad!);
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
            if (permisoDAL.CantidadUsuariosConRol(idRolAdmin) < 1)
                throw new InvalidOperationException("El sistema debe conservar al menos un Administrador habilitado con jerarquía 100.");
            registroBLL.RegistrarEvento($"Actualización de roles/permisos del usuario '{usuario.Nombre}'.", autor, "ALTA");
            return usuario;
        }

        public void LlenarPermisosDeUsuario(UsuarioBE usuario)
        {
            if (unidad == null)
            {
                using var lectura = new UnidadTrabajoSeguridadDAL();
                new PermisoBLL(lectura).LlenarPermisosDeUsuario(usuario);
                lectura.Confirmar();
                return;
            }
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

        private void CrearSubrolCore(FamiliaBE padre, string nombreNuevoRol)
        {
            ValidarPermisoGestionRoles();
            if (padre == null)
                throw new Exception("Debe seleccionar un rol padre válido.");
            if (string.IsNullOrWhiteSpace(nombreNuevoRol))
                throw new ArgumentException("Debe especificar un nombre válido para el rol.");
            padre = ObtenerArbolCompleto().SingleOrDefault(p => p.ID == padre.ID) as FamiliaBE
                ?? throw new ArgumentException("El rol padre ya no existe.");
            FamiliaBE nuevoHijo = new FamiliaBE { Nombre = nombreNuevoRol.Trim() };
            GuardarFamiliaCompleta(nuevoHijo);
            padre.AgregarHijo(nuevoHijo);
            GuardarFamiliaCompleta(padre);
        }
    }
}
