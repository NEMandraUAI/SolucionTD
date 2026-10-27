using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class FamiliaBE : ComponentePermiso
    {
        private List<ComponentePermiso> _hijos = new List<ComponentePermiso>();
        public override void AgregarHijo(ComponentePermiso c)
        {
            if (!_hijos.Contains(c))
                _hijos.Add(c);
        }
        public override void RemoverHijo(ComponentePermiso c)
        {
            _hijos.RemoveAll(x => x.ID == c.ID);
        }
        public override void VaciarHijos()
        {
            _hijos.Clear();
        }
        public override IList<ComponentePermiso> ObtenerHijos()
        {
            return _hijos.AsReadOnly();
        }
    }
}