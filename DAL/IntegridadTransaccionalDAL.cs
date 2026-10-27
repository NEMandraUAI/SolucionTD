using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class IntegridadTransaccionalDAL
    {
        private static readonly Dictionary<string, string> Claves = new(StringComparer.Ordinal)
        {
            ["Usuario"] = "ID", ["Socio"] = "ID_Socio", ["Turno"] = "NroTurno",
            ["ComprobantePago"] = "NroComprobante", ["PlanNutricional"] = "CodigoPlan",
            ["PlanSuscripcion"] = "CodigoPlan", ["RutinaEntrenamiento"] = "CodigoRutina",
            ["Producto"] = "CodigoProducto", ["VentaProducto"] = "NroVenta",
            ["OrdenReabastecimiento"] = "NroOrden"
        };

        public static void VerificarDVV(UnidadTrabajoSeguridadDAL unidad, params string[] tablas)
        {
            ArgumentNullException.ThrowIfNull(unidad);
            foreach (string tabla in tablas)
            {
                if (!Claves.TryGetValue(tabla, out string? pk))
                    throw new ArgumentException("Tabla no admitida para verificar integridad.");
                string? almacenado;
                using (var consultar = new SqlCommand("SELECT DVV FROM dbo.DigitoVerificador WHERE Entidad=@entidad", unidad.Conexion, unidad.Transaccion))
                {
                    consultar.Parameters.AddWithValue("@entidad", tabla);
                    object? valor=consultar.ExecuteScalar();
                    almacenado=valor==null || valor==DBNull.Value ? null : Convert.ToString(valor);
                }
                if (string.IsNullOrWhiteSpace(almacenado))
                    throw new InvalidOperationException($"Falta el DVV de {tabla}. No se realizarán cambios hasta revisar la integridad.");
                var concatenado=new StringBuilder();
                using (var leer=new SqlCommand($"SELECT DVH FROM dbo.[{tabla}] ORDER BY [{pk}]",unidad.Conexion,unidad.Transaccion))
                using (var reader=leer.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        if(reader.IsDBNull(0) || string.IsNullOrWhiteSpace(reader.GetString(0)))
                            throw new InvalidOperationException($"Hay un DVH sin inicializar en {tabla}.");
                        concatenado.Append(reader.GetString(0));
                    }
                }
                string calculado=Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(concatenado.ToString()))).ToLowerInvariant();
                if(!string.Equals(calculado,almacenado,StringComparison.Ordinal))
                    throw new InvalidOperationException($"El DVV de {tabla} no coincide. Se rechaza la operación sin recalcular datos alterados.");
            }
        }

        public static void ActualizarDVV(UnidadTrabajoSeguridadDAL unidad, params string[] tablas)
        {
            ArgumentNullException.ThrowIfNull(unidad);
            foreach (string tabla in tablas)
            {
                if (!Claves.TryGetValue(tabla, out string? pk))
                    throw new ArgumentException("Tabla no admitida para actualizar integridad.");
                using (var existe = new SqlCommand("SELECT DVV FROM dbo.DigitoVerificador WHERE Entidad=@entidad", unidad.Conexion, unidad.Transaccion))
                {
                    existe.Parameters.AddWithValue("@entidad", tabla);
                    object? valor = existe.ExecuteScalar();
                    if (valor == null || valor == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(valor)))
                        throw new InvalidOperationException($"Falta el DVV de {tabla}. Se requiere revisión administrativa antes de guardar.");
                }
                var concatenado = new StringBuilder();
                using (var leer = new SqlCommand($"SELECT DVH FROM dbo.[{tabla}] ORDER BY [{pk}]", unidad.Conexion, unidad.Transaccion))
                using (var reader = leer.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) || string.IsNullOrWhiteSpace(reader.GetString(0)))
                            throw new InvalidOperationException($"Hay un DVH sin inicializar en {tabla}; se revierte la operación.");
                        concatenado.Append(reader.GetString(0));
                    }
                }
                string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(concatenado.ToString()))).ToLowerInvariant();
                using var actualizar = new SqlCommand("UPDATE dbo.DigitoVerificador SET DVV=@dvv WHERE Entidad=@entidad", unidad.Conexion, unidad.Transaccion);
                actualizar.Parameters.AddWithValue("@dvv", hash);
                actualizar.Parameters.AddWithValue("@entidad", tabla);
                if (actualizar.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("No se pudo actualizar el dígito verificador de " + tabla);
            }
        }
    }
}
