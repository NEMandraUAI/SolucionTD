using Microsoft.Data.SqlClient;
using System;

namespace DAL
{
    /// <summary>Dispone únicamente una conexión propia; nunca la transacción prestada.</summary>
    internal sealed class AccesoSeguridadDAL : IDisposable
    {
        private readonly UnidadTrabajoSeguridadDAL? unidad;
        private readonly bool esPropia;
        internal SqlConnection Conexion { get; }

        internal AccesoSeguridadDAL(UnidadTrabajoSeguridadDAL? unidad)
        {
            this.unidad = unidad;
            esPropia = unidad == null;
            Conexion = unidad?.Conexion ?? ConexionDAL.Instancia.ObtenerConexion();
            if (esPropia)
            {
                try { Conexion.Open(); }
                catch { Conexion.Dispose(); throw; }
            }
        }

        internal SqlCommand CrearComando(string texto = "") =>
            new SqlCommand(texto, Conexion, unidad?.Transaccion);

        public void Dispose()
        {
            if (esPropia) Conexion.Dispose();
        }
    }
}
