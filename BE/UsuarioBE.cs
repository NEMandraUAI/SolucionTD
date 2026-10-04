using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class UsuarioBE : IEntidadVerificable
    {
        [DigitoVerificador(1)] public int ID { get; set; }
        [DigitoVerificador(2)] public string Nombre { get; set; }
        [DigitoVerificador(3)] public string Clave { get; set; }
        [DigitoVerificador(4)] public int IntentosFallidos { get; set; }
        [DigitoVerificador(5)] public bool Bloqueado { get; set; }
        [DigitoVerificador(6)] public int NivelJerarquia { get; set; }
        public string DVH { get; set; }
        public List<ComponentePermiso> Permisos { get; set; } = new List<ComponentePermiso>();
        public IdiomaBE Idioma { get; set; }
        public UsuarioMemento CrearMemento()
        {
            return new UsuarioMemento(this.Nombre, this.Clave, this.IntentosFallidos, this.Bloqueado, this.NivelJerarquia);
        }

        public string ObtenerID()
        {
            return ID.ToString();
        }

        public void RestaurarMemento(UsuarioMemento memento)
        {
            this.Nombre = memento.Nombre;
            this.Clave = memento.Clave;
            this.IntentosFallidos = memento.IntentosFallidos;
            this.Bloqueado = memento.Bloqueado;
            this.NivelJerarquia = memento.NivelJerarquia;
        }
        public bool TienePermiso(string codigoPermiso)
        {
            if (Permisos == null) return false;

            foreach (var componente in Permisos)
            {
                if (EvaluarPermisoRecursivo(componente, codigoPermiso))
                {
                    return true;
                }
            }
            return false;
        }
        private bool EvaluarPermisoRecursivo(ComponentePermiso componente, string codigoPermiso)
        {
            if (!string.IsNullOrEmpty(componente.PermisoCodigo) &&
                componente.PermisoCodigo.Equals(codigoPermiso, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            foreach (var hijo in componente.ObtenerHijos())
            {
                if (EvaluarPermisoRecursivo(hijo, codigoPermiso))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
