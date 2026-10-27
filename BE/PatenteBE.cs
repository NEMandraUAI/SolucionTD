using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PatenteBE : ComponentePermiso
    {
        public override void AgregarHijo(ComponentePermiso c)
        {
            throw new Exception("No se pueden agregar hijos a un permiso simple.");
        }
        public override void RemoverHijo(ComponentePermiso c)
        {
            throw new Exception("No se pueden remover hijos de un permiso simple.");
        }
        public override void VaciarHijos() { }
        public override IList<ComponentePermiso> ObtenerHijos()
        {
            return new List<ComponentePermiso>();
        }
    }
}