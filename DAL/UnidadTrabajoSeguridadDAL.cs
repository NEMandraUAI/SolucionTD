using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DAL
{
    /// <summary>
    /// Conexión y transacción explícitas para una operación completa de seguridad.
    /// El bloqueo por base serializa las lecturas de validación y sus escrituras.
    /// </summary>
    public sealed class UnidadTrabajoSeguridadDAL : IDisposable
    {
        private bool confirmada;
        private bool dispuesta;
        public SqlConnection Conexion { get; }
        public SqlTransaction Transaccion { get; }

        public UnidadTrabajoSeguridadDAL(string? connectionString = null)
        {
            Conexion = string.IsNullOrWhiteSpace(connectionString)
                ? ConexionDAL.Instancia.ObtenerConexion() : new SqlConnection(connectionString);
            SqlTransaction? transaccion = null;
            try
            {
                Conexion.Open();
                transaccion = Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
                using var cmd = new SqlCommand(@"DECLARE @resultado int;
                    EXEC @resultado = sys.sp_getapplock
                        @Resource = N'UAI-FIT:Seguridad',
                        @LockMode = N'Exclusive',
                        @LockOwner = N'Transaction',
                        @DbPrincipal = N'public',
                        @LockTimeout = 10000;
                    SELECT @resultado;", Conexion, transaccion);
                cmd.CommandTimeout = 15;
                int resultado = Convert.ToInt32(cmd.ExecuteScalar());
                if (resultado < 0)
                    throw new InvalidOperationException("No se pudo obtener el bloqueo de seguridad. Reintente la operación (código " + resultado + ").");
                Transaccion = transaccion;
            }
            catch
            {
                if (transaccion != null)
                {
                    try { transaccion.Rollback(); }
                    catch (Exception ex) when (ex is SqlException || ex is InvalidOperationException) { }
                    transaccion.Dispose();
                }
                Conexion.Dispose();
                throw;
            }
        }

        public void Confirmar()
        {
            if (dispuesta) throw new ObjectDisposedException(nameof(UnidadTrabajoSeguridadDAL));
            if (confirmada) throw new InvalidOperationException("La transacción ya fue confirmada.");
            Transaccion.Commit();
            confirmada = true;
        }

        public void Dispose()
        {
            if (dispuesta) return;
            dispuesta = true;
            try
            {
                if (!confirmada)
                {
                    try { Transaccion.Rollback(); }
                    catch (Exception ex) when (ex is SqlException || ex is InvalidOperationException) { }
                }
            }
            finally
            {
                Transaccion.Dispose();
                Conexion.Dispose();
            }
        }
    }
}
