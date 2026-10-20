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
    public class UsuarioBLL
    {
        private UsuarioDAL usuarioDAL = new UsuarioDAL();
        private RegistroBLL registroBLL = new RegistroBLL();
        private IntegridadBLL integridadBLL = new IntegridadBLL();
        public UsuarioBE IniciarSesion(string usuario, string clavePlana)
        {
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                throw new Exception("Operación denegada. Ya existe una sesión activa en el sistema.");
            }
            UsuarioBE usuarioBD = usuarioDAL.ObtenerPorUsername(usuario);
            if (usuarioBD == null)
            {
                throw new Exception("Usuario o clave incorrectos.");
            }
            if (usuarioBD.Nombre != usuario)
            {
                throw new Exception("Usuario o clave incorrectos.");
            }
            if (usuarioBD.Bloqueado)
            {
                registroBLL.RegistrarEvento("Intento de acceso a cuenta bloqueada: " + usuarioBD.Nombre, usuarioBD, "CRÍTICO");
                throw new Exception("El usuario se encuentra bloqueado por múltiples intentos fallidos. Contacte al administrador.");
            }
            bool hashCoincide = CryptoManager.Comparar(clavePlana, usuarioBD.Clave);
            if (hashCoincide)
            {
                if (usuarioBD.IntentosFallidos > 0)
                {
                    usuarioBD.IntentosFallidos = 0;
                    usuarioBD.DVH = DVManager.CalcularDVH(usuarioBD);
                    usuarioDAL.ActualizarEstadoUsuario(usuarioBD);
                    integridadBLL.ActualizarDVVGeneral();
                    RegistrarCambioHistorico(usuarioBD, usuarioBD.ID, "Reinicio de intentos fallidos por login exitoso");
                }
                PermisoBLL permisoBLL = new PermisoBLL();
                permisoBLL.LlenarPermisosDeUsuario(usuarioBD);
                registroBLL.RegistrarEvento("Inicio de sesión del usuario: " + usuarioBD.Nombre, usuarioBD);
                SessionManager.Instancia.Iniciar(usuarioBD);
                if (usuarioBD.Idioma != null)
                {
                    GestorIdioma.Instancia.CambiarIdioma(usuarioBD.Idioma);
                }
                return usuarioBD;
            }
            else
            {
                usuarioBD.IntentosFallidos++;
                if (usuarioBD.IntentosFallidos >= 3)
                {
                    usuarioBD.Bloqueado = true;
                    usuarioBD.DVH = DVManager.CalcularDVH(usuarioBD);
                    usuarioDAL.ActualizarEstadoUsuario(usuarioBD);
                    integridadBLL.ActualizarDVVGeneral();
                    RegistrarCambioHistorico(usuarioBD, usuarioBD.ID, "Bloqueo de cuenta por intentos fallidos");
                    registroBLL.RegistrarEvento($"Usuario <{usuarioBD.Nombre}> bloqueado por superar límite de intentos (3)", usuarioBD, "CRÍTICO");
                    throw new Exception("Su cuenta ha sido bloqueada tras 3 intentos fallidos.");
                }
                else
                {
                    usuarioBD.DVH = DVManager.CalcularDVH(usuarioBD);
                    usuarioDAL.ActualizarEstadoUsuario(usuarioBD);
                    integridadBLL.ActualizarDVVGeneral();
                    RegistrarCambioHistorico(usuarioBD, usuarioBD.ID, $"Intento fallido sumado (#{usuarioBD.IntentosFallidos})");
                    registroBLL.RegistrarEvento($"Intento de inicio de sesión fallido. Intento #{usuarioBD.IntentosFallidos}. Usuario: " + usuarioBD.Nombre, usuarioBD, "ALERTA");
                    int intentosRestantes = 3 - usuarioBD.IntentosFallidos;
                    throw new Exception($"Usuario o clave incorrectos. Le quedan {intentosRestantes} intento(s).");
                }
            }
        }
        public void CerrarSesion()
        {
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                registroBLL.RegistrarEvento("Cierre de sesión del usuario: " + SessionManager.Instancia.UsuarioActual.Nombre, SessionManager.Instancia.UsuarioActual);
            }
            SessionManager.Instancia.Cerrar();
        }
        public List<UsuarioBE> ListarTodos() => usuarioDAL.ListarTodos();
        public List<UsuarioHistoricoBE> ObtenerHistorialUsuario(int idUsuario) => usuarioDAL.ListarHistorial(idUsuario);
        public void RegistrarUsuario(string nombre, string clavePlana)
        {
            if (usuarioDAL.ObtenerPorUsername(nombre) != null)
            {
                throw new Exception("El nombre de usuario ya se encuentra registrado.");
            }
            UsuarioBE nuevoUsuario = new UsuarioBE();
            nuevoUsuario.Nombre = nombre;
            nuevoUsuario.Clave = CryptoManager.GenerarHash(clavePlana);
            nuevoUsuario.IntentosFallidos = 0;
            nuevoUsuario.Bloqueado = false;
            nuevoUsuario.NivelJerarquia = 1;
            int nuevoId = usuarioDAL.CrearUsuario(nuevoUsuario);
            nuevoUsuario.ID = nuevoId;
            PermisoBLL permisoBLL = new PermisoBLL();
            var catalogo = permisoBLL.ObtenerArbolCompleto();
            var rolSimple = catalogo.FirstOrDefault(x => x.Nombre.ToUpper() == "USUARIO SIMPLE");
            if (rolSimple != null)
            {
                nuevoUsuario.Permisos.Add(rolSimple);
                permisoBLL.ActualizarPermisosUsuario(nuevoUsuario);
            }
            nuevoUsuario.DVH = DVManager.CalcularDVH(nuevoUsuario);
            usuarioDAL.ActualizarDVH(nuevoId, nuevoUsuario.DVH);
            integridadBLL.ActualizarDVVGeneral();
            UsuarioBE usuarioCreado = usuarioDAL.ObtenerPorUsername(nombre);
            RegistrarCambioHistorico(usuarioCreado, usuarioCreado.ID, "Creación de usuario");
            registroBLL.RegistrarEvento("Nuevo usuario registrado: " + nombre, usuarioCreado, "ALTA");
        }
        public void RestaurarEstadoUsuario(int idUsuario, int idCambioHistorico, UsuarioBE usuarioAutor)
        {
            if (usuarioAutor == null || !usuarioAutor.TienePermiso("CONTROL_CAMBIOS"))
                throw new Exception("Operación de Seguridad Denegada: No posee el permiso requerido (CONTROL_CAMBIOS).");
            var historial = usuarioDAL.ListarHistorial(idUsuario);
            var versionARestaurar = historial.FirstOrDefault(h => h.ID_Cambio == idCambioHistorico);
            if (versionARestaurar == null)
                throw new Exception("No se encontró el estado histórico especificado.");
            UsuarioBE usuarioActual = usuarioDAL.ObtenerPorID(idUsuario);
            if (usuarioActual.NivelJerarquia == 100 && versionARestaurar.NivelJerarquia != 100)
            {
                throw new Exception("Operación Denegada: El usuario seleccionado actualmente es Administrador y posee un nivel fijo de jerarquía (100). No se permite restaurar un estado histórico que altere o reduzca su nivel jerárquico.");
            }
            if (usuarioAutor.NivelJerarquia < usuarioActual.NivelJerarquia)
            {
                throw new Exception("Operación de Seguridad Denegada: No tiene la jerarquía suficiente para modificar a este usuario.");
            }
            if (usuarioActual.Nombre == versionARestaurar.Nombre &&
                usuarioActual.Clave == versionARestaurar.Clave &&
                usuarioActual.IntentosFallidos == versionARestaurar.IntentosFallidos &&
                usuarioActual.Bloqueado == versionARestaurar.Bloqueado &&
                usuarioActual.NivelJerarquia == versionARestaurar.NivelJerarquia)
            {
                throw new Exception("El estado histórico seleccionado es idéntico al estado actual del usuario. No se puede realizar una restauración redundante.");
            }
            UsuarioMemento memento = new UsuarioMemento(
                versionARestaurar.Nombre,
                versionARestaurar.Clave,
                versionARestaurar.IntentosFallidos,
                versionARestaurar.Bloqueado,
                versionARestaurar.NivelJerarquia
            );
            usuarioActual.RestaurarMemento(memento);
            usuarioActual.DVH = DVManager.CalcularDVH(usuarioActual);
            usuarioDAL.ActualizarUsuarioCompleto(usuarioActual);
            integridadBLL.ActualizarDVVGeneral();
            UsuarioHistoricoBE nuevoRegistro = new UsuarioHistoricoBE
            {
                ID = usuarioActual.ID,
                Nombre = usuarioActual.Nombre,
                Clave = usuarioActual.Clave,
                IntentosFallidos = usuarioActual.IntentosFallidos,
                Bloqueado = usuarioActual.Bloqueado,
                NivelJerarquia = usuarioActual.NivelJerarquia,
                ID_Usuario_Autor = usuarioAutor.ID,
                FechaHora = DateTime.Now,
                Accion = $"Restauración a versión de fecha {versionARestaurar.FechaHora}"
            };
            usuarioDAL.GuardarEstadoHistorico(nuevoRegistro);
            registroBLL.RegistrarEvento($"Restauración de estado del usuario {usuarioActual.Nombre}", usuarioAutor, "ALTA");
        }
        private void RegistrarCambioHistorico(UsuarioBE usuarioAfectado, int idAutor, string accion)
        {
            UsuarioHistoricoBE historico = new UsuarioHistoricoBE
            {
                ID = usuarioAfectado.ID,
                Nombre = usuarioAfectado.Nombre,
                Clave = usuarioAfectado.Clave,
                IntentosFallidos = usuarioAfectado.IntentosFallidos,
                Bloqueado = usuarioAfectado.Bloqueado,
                NivelJerarquia = usuarioAfectado.NivelJerarquia,
                ID_Usuario_Autor = idAutor,
                FechaHora = DateTime.Now,
                Accion = accion
            };
            usuarioDAL.GuardarEstadoHistorico(historico);
        }
        public void ActualizarIdiomaUsuario(int idUsuario, int idIdioma)
        {
            usuarioDAL.ActualizarIdioma(idUsuario, idIdioma);
        }
        public void ModificarJerarquiaUsuario(int idUsuarioTarget, int nuevoNivel, UsuarioBE usuarioAutor)
        {
            if (usuarioAutor == null || !usuarioAutor.TienePermiso("CONTROL_CAMBIOS"))
                throw new Exception("Operación de Seguridad Denegada: No posee el permiso requerido (CONTROL_CAMBIOS).");
            UsuarioBE target = usuarioDAL.ObtenerPorID(idUsuarioTarget);
            if (target == null) throw new Exception("El usuario especificado no existe.");
            if (target.NivelJerarquia == 100)
            {
                throw new Exception("Operación Denegada: Un Administrador tiene nivel fijo (100) y no puede ser modificado manualmente.");
            }
            if (nuevoNivel < 1 || nuevoNivel > 99)
            {
                throw new Exception("Operación Denegada: El nivel de jerarquía debe ser un número entero entre 1 y 99.");
            }
            if (usuarioAutor.NivelJerarquia < target.NivelJerarquia)
            {
                throw new Exception("Operación Denegada: Su nivel de jerarquía es inferior al del usuario que intenta modificar.");
            }
            if (nuevoNivel > usuarioAutor.NivelJerarquia)
            {
                throw new Exception($"Operación Denegada: No puede asignar una jerarquía ({nuevoNivel}) superior a la suya ({usuarioAutor.NivelJerarquia}).");
            }
            target.NivelJerarquia = nuevoNivel;
            target.DVH = DVManager.CalcularDVH(target);
            usuarioDAL.ActualizarJerarquiaYDVH(target.ID, target.NivelJerarquia, target.DVH);
            integridadBLL.ActualizarDVVGeneral();
            RegistrarCambioHistorico(target, usuarioAutor.ID, $"Modificación manual de jerarquía a nivel {nuevoNivel}");
        }
    }
}
