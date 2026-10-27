using System;
using System.Collections.Generic;
using System.Text;
using BE;

namespace Seguridad
{
    public class SessionManager
    {
        public event EventHandler PermisosActualizados;
        private static SessionManager _instancia;
        private static readonly object candado = new object();
        public UsuarioBE UsuarioActual { get; private set; }
        public static SessionManager Instancia
        {
            get
            {
                lock (candado)
                {
                    if (_instancia == null)
                    {
                        _instancia = new SessionManager();
                    }
                    return _instancia;
                }
            }
        }

        private SessionManager() { }

        public void Iniciar(UsuarioBE usuario)
        {
            UsuarioActual = usuario;
        }

        public void Cerrar()
        {
            lock (candado) UsuarioActual = null;
            NotificarPermisosActualizados();
        }

        public void ActualizarUsuarioEnSesion(UsuarioBE usuarioActualizado)
        {
            lock (candado)
            {
                if (UsuarioActual == null || UsuarioActual.ID != usuarioActualizado.ID) return;
                UsuarioActual = usuarioActualizado;
            }
            NotificarPermisosActualizados();
        }

        private void NotificarPermisosActualizados()
        {
            var observadores = PermisosActualizados;
            if (observadores == null) return;
            foreach (EventHandler handler in observadores.GetInvocationList())
            {
                try { handler(this, EventArgs.Empty); }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Error al notificar cambios de sesión: {0}", ex);
                }
            }
        }
    }
}