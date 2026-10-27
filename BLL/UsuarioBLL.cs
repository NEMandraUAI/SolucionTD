using BE;
using DAL;
using Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace BLL
{
    public class UsuarioBLL
    {
        private static readonly object accesoSesion = new object();
        private readonly UsuarioDAL usuarioDAL = new UsuarioDAL();
        private readonly IntegridadBLL integridadBLL = new IntegridadBLL();

        public UsuarioBE IniciarSesion(string usuario, string clavePlana)
        {
            lock (accesoSesion) return IniciarSesionCore(usuario, clavePlana);
        }

        private UsuarioBE IniciarSesionCore(string usuario, string clavePlana)
        {
            if (SessionManager.Instancia.UsuarioActual != null)
                throw new InvalidOperationException("Operación denegada. Ya existe una sesión activa en el sistema.");
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(clavePlana))
                throw new ArgumentException("Usuario o clave incorrectos.");
            UsuarioBE confirmado;
            string? rechazo = null;
            using (var unidad = new UnidadTrabajoSeguridadDAL())
            {
                integridadBLL.VerificarIntegridadUsuario(unidad);
                var dal = new UsuarioDAL(unidad);
                var registro = new RegistroBLL(unidad);
                confirmado = dal.ObtenerPorUsername(usuario) ?? throw new UnauthorizedAccessException("Usuario o clave incorrectos.");
                if (confirmado.Bloqueado)
                {
                    registro.RegistrarEvento("Intento de acceso a cuenta bloqueada: " + confirmado.Nombre, confirmado, "CRÍTICO");
                    rechazo = "El usuario se encuentra bloqueado por múltiples intentos fallidos. Contacte al administrador.";
                }
                else if (PasswordManager.Verificar(clavePlana, confirmado.Clave))
                {
                    bool actualizarClave = PasswordManager.RequiereActualizacion(confirmado.Clave);
                    if (confirmado.IntentosFallidos > 0 || actualizarClave)
                    {
                        confirmado.IntentosFallidos = 0;
                        if (actualizarClave) confirmado.Clave = PasswordManager.GenerarHash(clavePlana);
                        confirmado.DVH = DVManager.CalcularDVH(confirmado);
                        dal.ActualizarUsuarioCompleto(confirmado);
                        RegistrarCambioHistorico(dal, confirmado, confirmado.ID, actualizarClave
                            ? "Actualización de protección de contraseña por login exitoso"
                            : "Reinicio de intentos fallidos por login exitoso");
                        integridadBLL.ActualizarDVVUsuario(unidad);
                    }
                    new PermisoBLL(unidad).LlenarPermisosDeUsuario(confirmado);
                    registro.RegistrarEvento("Inicio de sesión del usuario: " + confirmado.Nombre, confirmado);
                }
                else
                {
                    confirmado.IntentosFallidos++;
                    confirmado.Bloqueado = confirmado.IntentosFallidos >= 3;
                    confirmado.DVH = DVManager.CalcularDVH(confirmado);
                    dal.ActualizarEstadoUsuario(confirmado);
                    RegistrarCambioHistorico(dal, confirmado, confirmado.ID, confirmado.Bloqueado
                        ? "Bloqueo de cuenta por intentos fallidos" : $"Intento fallido sumado (#{confirmado.IntentosFallidos})");
                    registro.RegistrarEvento(confirmado.Bloqueado
                        ? $"Usuario <{confirmado.Nombre}> bloqueado por superar límite de intentos (3)"
                        : $"Intento de inicio de sesión fallido. Intento #{confirmado.IntentosFallidos}. Usuario: {confirmado.Nombre}",
                        confirmado, confirmado.Bloqueado ? "CRÍTICO" : "ALERTA");
                    integridadBLL.ActualizarDVVUsuario(unidad);
                    rechazo = confirmado.Bloqueado ? "Su cuenta ha sido bloqueada tras 3 intentos fallidos."
                        : $"Usuario o clave incorrectos. Le quedan {3 - confirmado.IntentosFallidos} intento(s).";
                }
                // Los intentos rechazados también se confirman, sin activar sesión.
                unidad.Confirmar();
            }
            if (rechazo != null) throw new UnauthorizedAccessException(rechazo);
            SessionManager.Instancia.Iniciar(confirmado);
            if (confirmado.Idioma != null)
            {
                try
                {
                    GestorIdioma.Instancia.CambiarIdioma(confirmado.Idioma);
                    foreach (var error in GestorIdioma.Instancia.UltimosErroresNotificacion)
                        InstaladorBLL.RegistrarLog("Login confirmado; error auxiliar de idioma: " + error.Message);
                }
                catch (Exception ex) { InstaladorBLL.RegistrarLog("Login confirmado; no se pudo aplicar el idioma: " + ex.Message); }
            }
            return confirmado;
        }

        public void CerrarSesion()
        {
            lock (accesoSesion) CerrarSesionCore();
        }

        private void CerrarSesionCore()
        {
            var actual = SessionManager.Instancia.UsuarioActual;
            try
            {
                if (actual != null) new RegistroBLL().RegistrarEvento("Cierre de sesión del usuario: " + actual.Nombre, actual);
            }
            catch (Exception ex) { InstaladorBLL.RegistrarLog("La sesión se cerró localmente; falló su registro SQL: " + ex.Message); }
            finally { SessionManager.Instancia.Cerrar(); }
        }

        public List<UsuarioBE> ListarTodos() => usuarioDAL.ListarTodos();
        public List<UsuarioHistoricoBE> ObtenerHistorialUsuario(int idUsuario) => usuarioDAL.ListarHistorial(idUsuario);

        public void RegistrarUsuario(string nombre, string clavePlana)
        {
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length < 4 || nombre.Length > 100)
                throw new ArgumentException("El nombre de usuario debe tener entre 4 y 100 caracteres.");
            if (string.IsNullOrWhiteSpace(clavePlana) || clavePlana.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
            using var unidad = new UnidadTrabajoSeguridadDAL();
            integridadBLL.VerificarIntegridadUsuario(unidad);
            using (var inicializacion = new SqlCommand("SELECT InicializacionPendiente FROM dbo.VersionEsquema WHERE ID=1", unidad.Conexion, unidad.Transaccion))
            {
                object? pendiente = inicializacion.ExecuteScalar();
                if (pendiente is not bool estado || estado)
                    throw new InvalidOperationException("Primero debe crear el administrador inicial desde la configuración del sistema.");
            }
            var dal = new UsuarioDAL(unidad);
            if (dal.ObtenerPorUsername(nombre) != null) throw new InvalidOperationException("El nombre de usuario ya se encuentra registrado.");
            var rolSimple = new PermisoBLL(unidad).ObtenerRolInicialSeguro();
            var nuevo = new UsuarioBE { Nombre = nombre, Clave = PasswordManager.GenerarHash(clavePlana), IntentosFallidos = 0, Bloqueado = false, NivelJerarquia = 1 };
            nuevo.ID = dal.CrearUsuario(nuevo);
            nuevo.Permisos.Add(rolSimple);
            new PermisoDAL(unidad).AsignarPermisoUsuario(nuevo.ID, rolSimple.ID);
            nuevo.DVH = DVManager.CalcularDVH(nuevo);
            dal.ActualizarDVH(nuevo.ID, nuevo.DVH);
            integridadBLL.ActualizarDVVUsuario(unidad);
            RegistrarCambioHistorico(dal, nuevo, nuevo.ID, "Creación de usuario");
            new RegistroBLL(unidad).RegistrarEvento("Nuevo usuario registrado: " + nombre, nuevo, "ALTA");
            unidad.Confirmar();
        }

        public void RegistrarAdministradorInicial(string nombre, string clavePlana)
        {
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length < 4 || nombre.Length > 100)
                throw new ArgumentException("El nombre de usuario debe tener entre 4 y 100 caracteres.");
            if (string.IsNullOrWhiteSpace(clavePlana) || clavePlana.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
            using var unidad = new UnidadTrabajoSeguridadDAL();
            using (var permisoSql = new SqlCommand("SELECT CASE WHEN IS_SRVROLEMEMBER('sysadmin')=1 OR HAS_PERMS_BY_NAME(DB_NAME(),'DATABASE','CONTROL')=1 THEN 1 ELSE 0 END", unidad.Conexion, unidad.Transaccion))
                if (Convert.ToInt32(permisoSql.ExecuteScalar()) != 1)
                    throw new UnauthorizedAccessException("Crear el administrador inicial requiere CONTROL sobre la base configurada.");
            using (var estado = new SqlCommand("SELECT (SELECT COUNT(*) FROM dbo.Usuario), InicializacionPendiente FROM dbo.VersionEsquema WHERE ID=1", unidad.Conexion, unidad.Transaccion))
            using (var lector = estado.ExecuteReader())
                if (!lector.Read() || lector.GetInt32(0) != 0 || !lector.GetBoolean(1))
                    throw new InvalidOperationException("La creación del administrador inicial ya no está disponible.");
            integridadBLL.VerificarIntegridadUsuario(unidad);
            var catalogo = new PermisoBLL(unidad).ObtenerArbolCompleto();
            var roles = catalogo.OfType<FamiliaBE>().Where(p => string.Equals(p.Nombre?.Trim(), "Administrador", StringComparison.OrdinalIgnoreCase)).ToList();
            if (roles.Count != 1 || !new UsuarioBE { Permisos = new List<ComponentePermiso> { roles[0] } }.TienePermiso("GESTION_ROLES"))
                throw new InvalidOperationException("El catálogo del rol Administrador está incompleto.");
            var dal = new UsuarioDAL(unidad);
            var nuevo = new UsuarioBE { Nombre = nombre.Trim(), Clave = PasswordManager.GenerarHash(clavePlana), IntentosFallidos = 0, Bloqueado = false, NivelJerarquia = 100 };
            nuevo.ID = dal.CrearUsuario(nuevo);
            nuevo.DVH = DVManager.CalcularDVH(nuevo);
            dal.ActualizarJerarquiaYDVH(nuevo.ID, 100, nuevo.DVH);
            new PermisoDAL(unidad).AsignarPermisoUsuario(nuevo.ID, roles[0].ID);
            integridadBLL.ActualizarDVVUsuario(unidad);
            RegistrarCambioHistorico(dal, nuevo, nuevo.ID, "Creación del administrador inicial");
            new RegistroBLL(unidad).RegistrarEvento("Administrador inicial creado durante la configuración.", nuevo, "ALTA");
            using (var finalizar = new SqlCommand("UPDATE dbo.VersionEsquema SET InicializacionPendiente=0 WHERE ID=1 AND InicializacionPendiente=1", unidad.Conexion, unidad.Transaccion))
                if (finalizar.ExecuteNonQuery() != 1) throw new InvalidOperationException("No se pudo finalizar la configuración inicial.");
            unidad.Confirmar();
        }

        public void RestaurarEstadoUsuario(int idUsuario, int idCambioHistorico, UsuarioBE usuarioAutor)
        {
            UsuarioBE confirmado;
            using (var unidad = new UnidadTrabajoSeguridadDAL())
            {
                integridadBLL.VerificarIntegridadUsuario(unidad);
                var dal = new UsuarioDAL(unidad);
                var permisos = new PermisoBLL(unidad);
                var autor = ObtenerAutorControlCambios(unidad, usuarioAutor);
                confirmado = dal.ObtenerPorID(idUsuario) ?? throw new ArgumentException("El usuario especificado no existe.");
                var version = dal.ListarHistorial(idUsuario).FirstOrDefault(h => h.ID_Cambio == idCambioHistorico)
                    ?? throw new ArgumentException("No se encontró el estado histórico especificado.");
                permisos.LlenarPermisosDeUsuario(confirmado);
                var administrador = permisos.ObtenerArbolCompleto().OfType<FamiliaBE>().Single(p => string.Equals(p.Nombre, "Administrador", StringComparison.OrdinalIgnoreCase));
                bool tieneRolAdministrador = confirmado.Permisos.Any(p => p.ID == administrador.ID);
                if ((tieneRolAdministrador && version.NivelJerarquia != 100) || (!tieneRolAdministrador && version.NivelJerarquia == 100))
                    throw new InvalidOperationException("El nivel histórico no es compatible con el rol actual. Administre los roles antes de restaurar.");
                if (version.NivelJerarquia < 1 || version.NivelJerarquia > 100 || autor.NivelJerarquia < confirmado.NivelJerarquia || autor.NivelJerarquia < version.NivelJerarquia)
                    throw new UnauthorizedAccessException("Su jerarquía no permite restaurar el estado seleccionado.");
                if (confirmado.Nombre == version.Nombre && confirmado.Clave == version.Clave && confirmado.IntentosFallidos == version.IntentosFallidos &&
                    confirmado.Bloqueado == version.Bloqueado && confirmado.NivelJerarquia == version.NivelJerarquia)
                    throw new InvalidOperationException("El estado histórico seleccionado es idéntico al estado actual del usuario.");
                confirmado.RestaurarMemento(new UsuarioMemento(version.Nombre, version.Clave, version.IntentosFallidos, version.Bloqueado, version.NivelJerarquia));
                confirmado.DVH = DVManager.CalcularDVH(confirmado);
                dal.ActualizarUsuarioCompleto(confirmado);
                if (new PermisoDAL(unidad).CantidadUsuariosConRol(administrador.ID) < 1)
                    throw new InvalidOperationException("La restauración no puede dejar al sistema sin un Administrador habilitado.");
                integridadBLL.ActualizarDVVUsuario(unidad);
                RegistrarCambioHistorico(dal, confirmado, autor.ID, $"Restauración a versión de fecha {version.FechaHora}");
                new RegistroBLL(unidad).RegistrarEvento($"Restauración de estado del usuario {confirmado.Nombre}", autor, "ALTA");
                unidad.Confirmar();
            }
            // Una restauración puede cambiar clave o bloqueo: se exige login nuevo.
            if (SessionManager.Instancia.UsuarioActual?.ID == confirmado.ID) SessionManager.Instancia.Cerrar();
        }

        private static UsuarioBE ObtenerAutorControlCambios(UnidadTrabajoSeguridadDAL unidad, UsuarioBE solicitado)
        {
            var sesion = SessionManager.Instancia.UsuarioActual;
            if (sesion == null || solicitado == null || sesion.ID != solicitado.ID)
                throw new UnauthorizedAccessException("Se requiere una sesión válida para administrar usuarios.");
            var autor = new UsuarioDAL(unidad).ObtenerPorID(sesion.ID);
            if (autor == null || autor.Bloqueado) throw new UnauthorizedAccessException("La cuenta de la sesión no está habilitada.");
            new PermisoBLL(unidad).LlenarPermisosDeUsuario(autor);
            if (!autor.TienePermiso("CONTROL_CAMBIOS")) throw new UnauthorizedAccessException("No posee el permiso requerido (CONTROL_CAMBIOS).");
            return autor;
        }

        private static void RegistrarCambioHistorico(UsuarioDAL dal, UsuarioBE afectado, int idAutor, string accion)
        {
            dal.GuardarEstadoHistorico(new UsuarioHistoricoBE { ID = afectado.ID, Nombre = afectado.Nombre, Clave = afectado.Clave,
                IntentosFallidos = afectado.IntentosFallidos, Bloqueado = afectado.Bloqueado, NivelJerarquia = afectado.NivelJerarquia,
                ID_Usuario_Autor = idAutor, FechaHora = DateTime.Now, Accion = accion });
        }

        public void ActualizarIdiomaUsuario(int idUsuario, int idIdioma)
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            var actual = SessionManager.Instancia.UsuarioActual;
            if (actual == null || actual.ID != idUsuario) throw new UnauthorizedAccessException("Solo puede cambiar el idioma de su propia sesión.");
            new UsuarioDAL(unidad).ActualizarIdioma(idUsuario, idIdioma);
            unidad.Confirmar();
        }

        public void ModificarJerarquiaUsuario(int idUsuarioTarget, int nuevoNivel, UsuarioBE usuarioAutor)
        {
            UsuarioBE confirmado;
            using (var unidad = new UnidadTrabajoSeguridadDAL())
            {
                integridadBLL.VerificarIntegridadUsuario(unidad);
                var dal = new UsuarioDAL(unidad);
                var autor = ObtenerAutorControlCambios(unidad, usuarioAutor);
                confirmado = dal.ObtenerPorID(idUsuarioTarget) ?? throw new ArgumentException("El usuario especificado no existe.");
                if (confirmado.NivelJerarquia == 100) throw new InvalidOperationException("Un Administrador tiene nivel fijo (100) y no puede modificarse manualmente.");
                if (nuevoNivel < 1 || nuevoNivel > 99) throw new ArgumentException("El nivel de jerarquía debe ser un entero entre 1 y 99.");
                if (autor.NivelJerarquia < confirmado.NivelJerarquia || nuevoNivel > autor.NivelJerarquia)
                    throw new UnauthorizedAccessException("Su jerarquía no permite realizar esta modificación.");
                confirmado.NivelJerarquia = nuevoNivel;
                confirmado.DVH = DVManager.CalcularDVH(confirmado);
                dal.ActualizarJerarquiaYDVH(confirmado.ID, nuevoNivel, confirmado.DVH);
                integridadBLL.ActualizarDVVUsuario(unidad);
                RegistrarCambioHistorico(dal, confirmado, autor.ID, $"Modificación manual de jerarquía a nivel {nuevoNivel}");
                new RegistroBLL(unidad).RegistrarEvento($"Actualización de jerarquía del usuario '{confirmado.Nombre}'.", autor, "ALTA");
                new PermisoBLL(unidad).LlenarPermisosDeUsuario(confirmado);
                unidad.Confirmar();
            }
            if (SessionManager.Instancia.UsuarioActual?.ID == confirmado.ID) SessionManager.Instancia.ActualizarUsuarioEnSesion(confirmado);
        }
    }
}
