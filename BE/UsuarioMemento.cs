using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class UsuarioMemento
    {
        public string Nombre { get; private set; }
        public string Clave { get; private set; }
        public int IntentosFallidos { get; private set; }
        public bool Bloqueado { get; private set; }
        public int NivelJerarquia { get; private set; }
        public UsuarioMemento(string nombre, string clave, int intentos, bool bloqueado, int nivelJerarquia)
        {
            Nombre = nombre;
            Clave = clave;
            IntentosFallidos = intentos;
            Bloqueado = bloqueado;
            NivelJerarquia = nivelJerarquia;
        }
    }
}