using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class ComponentePermiso
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string PermisoCodigo { get; set; }
        public abstract void AgregarHijo(ComponentePermiso c);
        public abstract void RemoverHijo(ComponentePermiso c);
        public abstract void VaciarHijos();
        public abstract IList<ComponentePermiso> ObtenerHijos();
    }
}