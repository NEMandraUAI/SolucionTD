using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class SchemaMigradorBLL
    {
        public const int VersionActual = 2;

        public static bool RequiereActualizacion(string connectionString)
        {
            using var cn = new SqlConnection(connectionString);
            cn.Open();
            using var existe = new SqlCommand("SELECT OBJECT_ID(N'dbo.VersionEsquema', N'U');", cn);
            if (existe.ExecuteScalar() is DBNull) return true;
            using var version = new SqlCommand("SELECT Version FROM dbo.VersionEsquema WHERE ID=1;", cn);
            object valor = version.ExecuteScalar();
            if (valor == null || valor == DBNull.Value) return true;
            int numero = Convert.ToInt32(valor);
            if (numero > VersionActual) throw new InvalidOperationException("La base pertenece a una versión más nueva de UAI-FIT. Instale esa versión antes de abrirla.");
            return numero < VersionActual;
        }

        public static bool InicializacionPendiente(string connectionString)
        {
            using var cn = new SqlConnection(connectionString);
            cn.Open();
            using var cmd = new SqlCommand("SELECT InicializacionPendiente FROM dbo.VersionEsquema WHERE ID=1;", cn);
            return Convert.ToBoolean(cmd.ExecuteScalar());
        }

        public static void Aplicar(string appConnectionString, bool nueva = false)
        {
            using var unidad = new UnidadTrabajoSeguridadDAL(appConnectionString);
            Aplicar(unidad, nueva);
            unidad.Confirmar();
        }

        internal static void Aplicar(UnidadTrabajoSeguridadDAL unidad, bool nueva)
        {
            using var existe = new SqlCommand("SELECT OBJECT_ID(N'dbo.VersionEsquema',N'U');", unidad.Conexion, unidad.Transaccion);
            bool tieneVersion = existe.ExecuteScalar() is not DBNull;
            if (tieneVersion)
            {
                using var leer = new SqlCommand("SELECT Version FROM dbo.VersionEsquema WHERE ID=1;", unidad.Conexion, unidad.Transaccion);
                object numero = leer.ExecuteScalar();
                if (numero != null && numero != DBNull.Value && Convert.ToInt32(numero) > VersionActual)
                    throw new InvalidOperationException("No se puede actualizar una base de una versión posterior.");
            }
            using var cmd = new SqlCommand(@"
IF OBJECT_ID(N'dbo.ComprobantePago',N'U') IS NULL THROW 51010,'La base está incompleta: falta ComprobantePago.',1;
IF COL_LENGTH(N'dbo.ComprobantePago',N'CodigoPlanVendido') IS NULL ALTER TABLE dbo.ComprobantePago ADD CodigoPlanVendido int NULL;
IF COL_LENGTH(N'dbo.ComprobantePago',N'NombrePlanVendido') IS NULL ALTER TABLE dbo.ComprobantePago ADD NombrePlanVendido nvarchar(100) NULL;
IF COL_LENGTH(N'dbo.ComprobantePago',N'DuracionDiasVendida') IS NULL ALTER TABLE dbo.ComprobantePago ADD DuracionDiasVendida int NULL;
IF COL_LENGTH(N'dbo.ComprobantePago',N'FechaCoberturaDesde') IS NULL ALTER TABLE dbo.ComprobantePago ADD FechaCoberturaDesde date NULL;
IF COL_LENGTH(N'dbo.ComprobantePago',N'FechaCoberturaHasta') IS NULL ALTER TABLE dbo.ComprobantePago ADD FechaCoberturaHasta date NULL;
IF OBJECT_ID(N'dbo.VersionEsquema',N'U') IS NULL
    CREATE TABLE dbo.VersionEsquema(ID int NOT NULL CONSTRAINT PK_VersionEsquema PRIMARY KEY,
        Version int NOT NULL, InicializacionPendiente bit NOT NULL,
        CONSTRAINT CK_VersionEsquema_ID CHECK(ID=1));
DECLARE @pendienteReal bit = CASE WHEN EXISTS(SELECT 1 FROM dbo.Usuario) THEN CAST(0 AS bit) ELSE CAST(1 AS bit) END;
IF NOT EXISTS(SELECT 1 FROM dbo.VersionEsquema WHERE ID=1)
    INSERT dbo.VersionEsquema(ID,Version,InicializacionPendiente) VALUES(1,@version,@pendienteReal);
ELSE UPDATE dbo.VersionEsquema SET Version=@version, InicializacionPendiente=@pendienteReal WHERE ID=1;", unidad.Conexion, unidad.Transaccion);
            cmd.Parameters.AddWithValue("@version", VersionActual);
            cmd.ExecuteNonQuery();
            ValidarEnConexion(unidad.Conexion, unidad.Transaccion);
        }

        public static void Validar(string connectionString)
        {
            using var unidad = new UnidadTrabajoSeguridadDAL(connectionString);
            ValidarEnConexion(unidad.Conexion, unidad.Transaccion);
            unidad.Confirmar();
        }

        private static void ValidarEnConexion(SqlConnection cn, SqlTransaction transaccion)
        {
            using var cmd = new SqlCommand(@"SELECT t.name,c.name,ty.name,c.max_length
                FROM sys.tables t JOIN sys.schemas s ON t.schema_id=s.schema_id
                JOIN sys.columns c ON c.object_id=t.object_id JOIN sys.types ty ON ty.user_type_id=c.user_type_id
                WHERE s.name=N'dbo';", cn, transaccion);
            var columnas = new Dictionary<string, (string Tipo, int Longitud)>(StringComparer.OrdinalIgnoreCase);
            using (var reader = cmd.ExecuteReader())
                while (reader.Read()) columnas[reader.GetString(0) + "." + reader.GetString(1)] = (reader.GetString(2), reader.GetInt16(3));
            foreach (var linea in ColumnasEsperadas.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var p = linea.Trim().Split('|');
                string clave = p[0] + "." + p[1];
                if (!columnas.TryGetValue(clave, out var actual)) throw new InvalidOperationException("Esquema incompleto: falta dbo." + clave + ".");
                if (!string.Equals(actual.Tipo, p[2], StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Tipo incompatible en dbo." + clave + ". Se requiere revisión administrativa.");
                int largo = int.Parse(p[3]);
                if (largo > 0 && actual.Longitud != -1 && actual.Longitud < largo)
                    throw new InvalidOperationException("La columna dbo." + clave + " no admite la longitud requerida.");
            }
            using var sp = new SqlCommand("SELECT name FROM sys.procedures WHERE schema_id=SCHEMA_ID(N'dbo');", cn, transaccion);
            var procedimientos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var r = sp.ExecuteReader()) while (r.Read()) procedimientos.Add(r.GetString(0));
            foreach (string requerido in new[] { "sp_Usuario_ActualizarEstado", "sp_Usuario_ObtenerPorUsername" })
                if (!procedimientos.Contains(requerido)) throw new InvalidOperationException("Falta el procedimiento dbo." + requerido + ".");
            using var permisos = new SqlCommand("SELECT PermisoCodigo FROM dbo.Permiso WHERE EsFamilia=0 AND PermisoCodigo IS NOT NULL;", cn, transaccion);
            var codigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var r = permisos.ExecuteReader()) while (r.Read()) codigos.Add(r.GetString(0));
            foreach (string codigo in PermisosEsperados.Split(','))
                if (!codigos.Contains(codigo)) throw new InvalidOperationException("Falta el permiso de sistema " + codigo + ".");
            using var estado = new SqlCommand(@"SELECT Version,InicializacionPendiente,
                (SELECT COUNT(*) FROM dbo.Usuario),
                (SELECT COUNT(*) FROM dbo.Idioma WHERE ID=1),
                (SELECT COUNT(*) FROM dbo.Etiqueta),
                (SELECT COUNT(*) FROM dbo.Traduccion WHERE ID_Idioma=1)
                FROM dbo.VersionEsquema WHERE ID=1;", cn, transaccion);
            using var fila = estado.ExecuteReader();
            if (!fila.Read() || fila.GetInt32(0) != VersionActual) throw new InvalidOperationException("La versión del esquema no corresponde a esta aplicación.");
            bool pendiente = fila.GetBoolean(1);
            int usuarios = fila.GetInt32(2);
            if ((pendiente && usuarios != 0) || (!pendiente && usuarios == 0))
                throw new InvalidOperationException("El estado de inicialización de usuarios es inconsistente. No se creará un administrador automáticamente.");
            if (fila.GetInt32(3) == 0 || fila.GetInt32(4) == 0 || fila.GetInt32(5) == 0)
                throw new InvalidOperationException("Faltan el idioma inicial, etiquetas o traducciones necesarias.");
        }

        private const string PermisosEsperados = "GESTION_ROLES,CONTROL_BITACORA,CONTROL_CAMBIOS,REALIZAR_BACKUP,GESTION_IDIOMAS,VENDER_PLAN,CONSULTAR_SOCIO,ASIGNAR_RUTINA,ASENTAR_TURNO,REVISAR_AGENDA,CONSULTAR_AGENDA,ASIGNAR_PLAN_NUTRICIONAL,CONSULTAR_PLANES_NUT,GESTION_SOCIOS_RIESGO,CONSULTAR_COMPROBANTES,VENDER_PRODUCTO,CONSULTAR_STOCK,ANALIZAR_DEMANDA";
        private const string ColumnasEsperadas = @"Bitacora|ID_Reg|int|0
Bitacora|ID_Usuario|int|0
Bitacora|FechaHora|datetime|0
Bitacora|Evento|nvarchar|510
Bitacora|Criticidad|nvarchar|40
ComprobantePago|NroComprobante|int|0
ComprobantePago|Fecha|datetime|0
ComprobantePago|MontoTotal|decimal|0
ComprobantePago|MetodoPago|nvarchar|100
ComprobantePago|ID_Socio|int|0
ComprobantePago|ID_Usuario|int|0
ComprobantePago|DVH|nvarchar|512
DetalleOrdenReabastecimiento|ID_DetalleOrden|int|0
DetalleOrdenReabastecimiento|NroOrden|int|0
DetalleOrdenReabastecimiento|CodigoProducto|int|0
DetalleOrdenReabastecimiento|StockAlMomento|int|0
DetalleOrdenReabastecimiento|DemandaProyectada|decimal|0
DetalleOrdenReabastecimiento|CantidadSugeridaSistema|int|0
DetalleOrdenReabastecimiento|CantidadAprobadaAdmin|int|0
DetalleOrdenReabastecimiento|CostoUnitarioCompra|decimal|0
DetalleOrdenReabastecimiento|SubtotalCosto|decimal|0
DetalleVentaProducto|ID_DetalleVenta|int|0
DetalleVentaProducto|NroVenta|int|0
DetalleVentaProducto|CodigoProducto|int|0
DetalleVentaProducto|Cantidad|int|0
DetalleVentaProducto|PrecioUnitario|decimal|0
DetalleVentaProducto|CostoUnitario|decimal|0
DetalleVentaProducto|Subtotal|decimal|0
DigitoVerificador|Entidad|varchar|50
DigitoVerificador|DVV|nvarchar|512
Etiqueta|ID|int|0
Etiqueta|NombreControl|varchar|100
Etiqueta|Formulario|varchar|100
Idioma|ID|int|0
Idioma|Nombre|varchar|50
OrdenReabastecimiento|NroOrden|int|0
OrdenReabastecimiento|FechaGeneracion|datetime|0
OrdenReabastecimiento|FechaRecepcion|datetime|0
OrdenReabastecimiento|MetodoPrediccion|nvarchar|160
OrdenReabastecimiento|HorizonteDias|int|0
OrdenReabastecimiento|Estado|nvarchar|60
OrdenReabastecimiento|CostoTotalOrden|decimal|0
OrdenReabastecimiento|Observaciones|nvarchar|600
OrdenReabastecimiento|ID_Usuario|int|0
OrdenReabastecimiento|DVH|nvarchar|512
Permiso|ID|int|0
Permiso|Nombre|varchar|100
Permiso|PermisoCodigo|varchar|50
Permiso|EsFamilia|bit|0
Permiso_Permiso|ID_Padre|int|0
Permiso_Permiso|ID_Hijo|int|0
PlanNutricional|CodigoPlan|int|0
PlanNutricional|FechaCreacion|datetime|0
PlanNutricional|EvaluacionAntropometrica|nvarchar|0
PlanNutricional|DetalleHabitos|nvarchar|0
PlanNutricional|ID_Socio|int|0
PlanNutricional|ID_Nutricionista|int|0
PlanNutricional|DVH|nvarchar|512
PlanSuscripcion|CodigoPlan|int|0
PlanSuscripcion|Nombre|nvarchar|200
PlanSuscripcion|Precio|decimal|0
PlanSuscripcion|DuracionDias|int|0
PlanSuscripcion|DVH|nvarchar|512
Producto|CodigoProducto|int|0
Producto|CodigoSKU|nvarchar|60
Producto|Nombre|nvarchar|240
Producto|Categoria|nvarchar|120
Producto|PrecioVenta|decimal|0
Producto|CostoCompra|decimal|0
Producto|StockActual|int|0
Producto|StockMinimo|int|0
Producto|TiempoReposicionDias|int|0
Producto|ObjetivoFisicoAfin|nvarchar|200
Producto|ObjetivoNutricionalAfin|nvarchar|200
Producto|Activo|bit|0
Producto|DVH|nvarchar|512
RutinaEntrenamiento|CodigoRutina|int|0
RutinaEntrenamiento|FechaCreacion|datetime|0
RutinaEntrenamiento|ObjetivoFisico|nvarchar|510
RutinaEntrenamiento|FrecuenciaSemanal|int|0
RutinaEntrenamiento|DetalleEjercicios|nvarchar|0
RutinaEntrenamiento|ID_Socio|int|0
RutinaEntrenamiento|ID_Usuario|int|0
RutinaEntrenamiento|DVH|nvarchar|512
Socio|ID_Socio|int|0
Socio|DNI|nvarchar|40
Socio|Nombre|nvarchar|200
Socio|Apellido|nvarchar|200
Socio|Telefono|nvarchar|100
Socio|Email|nvarchar|200
Socio|EstadoActivo|bit|0
Socio|CodigoPlan|int|0
Socio|DVH|nvarchar|512
Traduccion|ID|int|0
Traduccion|ID_Idioma|int|0
Traduccion|ID_Etiqueta|int|0
Traduccion|Texto|nvarchar|0
Turno|NroTurno|int|0
Turno|FechaHora|datetime|0
Turno|Estado|nvarchar|100
Turno|ID_Socio|int|0
Turno|ID_Nutricionista|int|0
Turno|DVH|nvarchar|512
Usuario|ID|int|0
Usuario|Nombre|nvarchar|200
Usuario|Clave|nvarchar|510
Usuario|IntentosFallidos|int|0
Usuario|Bloqueado|bit|0
Usuario|DVH|nvarchar|512
Usuario|ID_Idioma|int|0
Usuario|NivelJerarquia|int|0
Usuario_Historico|ID_Cambio|int|0
Usuario_Historico|ID_Usuario|int|0
Usuario_Historico|Nombre|nvarchar|200
Usuario_Historico|Clave|nvarchar|510
Usuario_Historico|IntentosFallidos|int|0
Usuario_Historico|Bloqueado|bit|0
Usuario_Historico|ID_Usuario_Autor|int|0
Usuario_Historico|FechaHora|datetime|0
Usuario_Historico|Accion|nvarchar|510
Usuario_Historico|NivelJerarquia|int|0
Usuario_Permiso|ID_Usuario|int|0
Usuario_Permiso|ID_Permiso|int|0
VentaProducto|NroVenta|int|0
VentaProducto|Fecha|datetime|0
VentaProducto|MontoTotal|decimal|0
VentaProducto|CostoTotalEstimado|decimal|0
VentaProducto|MetodoPago|nvarchar|100
VentaProducto|ID_Socio|int|0
VentaProducto|ID_Usuario|int|0
VentaProducto|DVH|nvarchar|512
ComprobantePago|CodigoPlanVendido|int|0
ComprobantePago|NombrePlanVendido|nvarchar|200
ComprobantePago|DuracionDiasVendida|int|0
ComprobantePago|FechaCoberturaDesde|date|0
ComprobantePago|FechaCoberturaHasta|date|0
VersionEsquema|ID|int|0
VersionEsquema|Version|int|0
VersionEsquema|InicializacionPendiente|bit|0";
    }
}