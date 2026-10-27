using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class ComprobantePagoDAL
    {
        private readonly UnidadTrabajoSeguridadDAL? unidad;
        public ComprobantePagoDAL(UnidadTrabajoSeguridadDAL? unidad = null) { this.unidad = unidad; }

        public void InsertarComprobante(ComprobantePagoBE comprobante)
        {
            if (unidad == null)
                throw new InvalidOperationException("El comprobante debe registrarse mediante la venta transaccional de suscripción.");
            InsertarComprobanteInterno(unidad, comprobante);
        }

        public void RegistrarVentaSuscripcionTransaccional(SocioBE socio, ComprobantePagoBE comprobante,
            Func<object, string> calcularDVH, out bool fueAltaNuevoSocio)
        {
            if (unidad != null)
            {
                RegistrarVentaCore(unidad, socio, comprobante, calcularDVH, out fueAltaNuevoSocio);
                return;
            }
            int idOriginal = socio.ID_Socio;
            int nroOriginal = comprobante.NroComprobante;
            using var trabajo = new UnidadTrabajoSeguridadDAL();
            try
            {
                RegistrarVentaCore(trabajo, socio, comprobante, calcularDVH, out fueAltaNuevoSocio);
                trabajo.Confirmar();
            }
            catch
            {
                socio.ID_Socio = idOriginal;
                comprobante.NroComprobante = nroOriginal;
                throw;
            }
        }

        private static void RegistrarVentaCore(UnidadTrabajoSeguridadDAL trabajo, SocioBE solicitado,
            ComprobantePagoBE comprobante, Func<object, string> calcularDVH, out bool fueAltaNuevoSocio)
        {
            var cx = trabajo.Conexion;
            var tx = trabajo.Transaccion;
            IntegridadTransaccionalDAL.VerificarDVV(trabajo, "Socio", "ComprobantePago");
            var usuarios = new UsuarioDAL(trabajo);
            var actor = usuarios.ObtenerPorID(comprobante.EmpleadoCobrador?.ID ?? 0);
            if (actor == null || actor.Bloqueado) throw new UnauthorizedAccessException("La cuenta cobradora no está habilitada.");
            if (actor.DVH != calcularDVH(actor)) throw new InvalidOperationException("La cuenta cobradora no superó la verificación de integridad.");
            var permisos = new PermisoDAL(trabajo);
            actor.Permisos = permisos.ObtenerPermisosDeUsuario(actor.ID, permisos.ObtenerArbolCompleto());
            if (!actor.TienePermiso("VENDER_PLAN") && !actor.TienePermiso("GESTION_ROLES"))
                throw new UnauthorizedAccessException("No posee permiso para vender suscripciones.");

            var planes = new PlanSuscripcionDAL(trabajo).ListarPlanes();
            var plan = planes.SingleOrDefault(p => p.CodigoPlan == solicitado.PlanAsociado?.CodigoPlan)
                ?? throw new InvalidOperationException("El plan seleccionado ya no existe. Actualice el catálogo.");
            if (plan.Precio <= 0 || plan.DuracionDias < 1 || plan.DuracionDias > 36500)
                throw new InvalidOperationException("El plan tiene precio o duración inválidos.");
            if (plan.DVH != calcularDVH(plan)) throw new InvalidOperationException("El plan no superó la verificación de integridad.");
            if (comprobante.MontoTotal != plan.Precio)
                throw new InvalidOperationException("El precio del plan cambió desde que abrió el formulario. Actualice el catálogo y confirme nuevamente.");

            var socios = new SocioDAL(trabajo);
            var persistido = socios.ObtenerPorDNI(solicitado.DNI);
            fueAltaNuevoSocio = solicitado.ID_Socio == 0;
            if (fueAltaNuevoSocio && persistido != null)
                throw new InvalidOperationException("El socio ya fue registrado por otra operación. Búsquelo nuevamente.");
            if (!fueAltaNuevoSocio && (persistido == null || persistido.ID_Socio != solicitado.ID_Socio))
                throw new InvalidOperationException("El socio seleccionado ya no coincide con su DNI. Vuelva a buscarlo.");
            if (persistido != null)
            {
                if (persistido.DVH != calcularDVH(persistido)) throw new InvalidOperationException("El socio no superó la verificación de integridad.");
                var ultimo = new ComprobantePagoDAL(trabajo).ObtenerUltimoPorSocio(persistido.ID_Socio);
                if (ultimo != null)
                {
                    DateTime? hasta = ultimo.FechaCoberturaHasta;
                    if (!hasta.HasValue)
                    {
                        var planAnterior = planes.SingleOrDefault(p => p.CodigoPlan == persistido.PlanAsociado?.CodigoPlan);
                        if (planAnterior == null || planAnterior.DuracionDias < 1 || planAnterior.DuracionDias > 36500)
                            throw new InvalidOperationException("El último pago es histórico y no permite determinar su cobertura. Requiere revisión administrativa antes de renovar.");
                        // Compatibilidad con registros previos: conserva la regla de
                        // vigencia existente, pero no inventa un snapshot histórico.
                        hasta = ultimo.Fecha.Date.AddDays(planAnterior.DuracionDias);
                    }
                    if (hasta.Value.Date >= DateTime.Today)
                        throw new InvalidOperationException($"El socio ya posee cobertura vigente hasta el {hasta:dd/MM/yyyy}. No se permite superponer suscripciones.");
                }
            }
            var final = new SocioBE
            {
                ID_Socio = persistido?.ID_Socio ?? 0,
                DNI = persistido?.DNI ?? solicitado.DNI,
                Nombre = persistido?.Nombre ?? solicitado.Nombre,
                Apellido = persistido?.Apellido ?? solicitado.Apellido,
                Telefono = solicitado.Telefono, Email = solicitado.Email,
                EstadoActivo = true, PlanAsociado = plan
            };
            if (fueAltaNuevoSocio) socios.InsertarSocio(final);
            final.DVH = calcularDVH(final);
            socios.ActualizarSocio(final);

            comprobante.Socio = final;
            comprobante.EmpleadoCobrador = actor;
            comprobante.MontoTotal = plan.Precio;
            comprobante.CodigoPlanVendido = plan.CodigoPlan;
            comprobante.NombrePlanVendido = plan.Nombre;
            comprobante.DuracionDiasVendida = plan.DuracionDias;
            comprobante.FechaCoberturaDesde = comprobante.Fecha.Date;
            comprobante.FechaCoberturaHasta = comprobante.Fecha.Date.AddDays(plan.DuracionDias);
            InsertarComprobanteInterno(trabajo, comprobante);
            comprobante.DVH = calcularDVH(comprobante);
            new IntegridadDAL(trabajo).ActualizarDVHRegistro("ComprobantePago", comprobante.NroComprobante, comprobante.DVH);
            IntegridadTransaccionalDAL.ActualizarDVV(trabajo, "Socio", "ComprobantePago");
            new RegistroDAL(trabajo).GuardarRegistro(new RegistroBE
            {
                ID_Usuario = actor.ID, FechaHora = DateTime.Now, Criticidad = "ALTA",
                Evento = $"Venta de suscripción confirmada. Comprobante #{comprobante.NroComprobante}; socio #{final.ID_Socio}; plan #{plan.CodigoPlan}."
            }, actor);
            // Se publica la identidad confirmable, no datos de nombre/clave
            // enviados por el formulario para un socio ya existente.
            solicitado.ID_Socio = final.ID_Socio; solicitado.Nombre = final.Nombre; solicitado.Apellido = final.Apellido;
            solicitado.DNI = final.DNI; solicitado.EstadoActivo = final.EstadoActivo;
            solicitado.PlanAsociado = plan; solicitado.DVH = final.DVH;
        }

        private static void InsertarComprobanteInterno(UnidadTrabajoSeguridadDAL trabajo, ComprobantePagoBE comp)
        {
            using var cmd = new SqlCommand(@"INSERT dbo.ComprobantePago
                (Fecha,MontoTotal,MetodoPago,ID_Socio,ID_Usuario,DVH,CodigoPlanVendido,NombrePlanVendido,DuracionDiasVendida,FechaCoberturaDesde,FechaCoberturaHasta)
                VALUES
                (@fecha,@monto,@metodo,@socio,@usuario,@dvh,@plan,@nombrePlan,@duracion,@desde,@hasta); SELECT CONVERT(int,SCOPE_IDENTITY());", trabajo.Conexion, trabajo.Transaccion);
            cmd.Parameters.AddWithValue("@fecha", comp.Fecha);
            cmd.Parameters.AddWithValue("@monto", comp.MontoTotal);
            cmd.Parameters.AddWithValue("@metodo", comp.MetodoPago);
            cmd.Parameters.AddWithValue("@socio", comp.Socio.ID_Socio);
            cmd.Parameters.AddWithValue("@usuario", comp.EmpleadoCobrador.ID);
            cmd.Parameters.AddWithValue("@dvh", (object?)comp.DVH ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@plan", (object?)comp.CodigoPlanVendido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@nombrePlan", (object?)comp.NombrePlanVendido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@duracion", (object?)comp.DuracionDiasVendida ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@desde", (object?)comp.FechaCoberturaDesde ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@hasta", (object?)comp.FechaCoberturaHasta ?? DBNull.Value);
            comp.NroComprobante = Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<string> ObtenerTodosLosDVH()
        {
            var resultado = new List<string>();
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando("SELECT DVH FROM dbo.ComprobantePago ORDER BY NroComprobante");
            using var dr = cmd.ExecuteReader();
            while (dr.Read()) resultado.Add(dr.IsDBNull(0) ? "" : dr.GetString(0));
            return resultado;
        }

        private const string Columnas = @"c.NroComprobante,c.Fecha,c.MontoTotal,c.MetodoPago,c.ID_Socio,c.ID_Usuario,c.DVH,
            c.CodigoPlanVendido,c.NombrePlanVendido,c.DuracionDiasVendida,c.FechaCoberturaDesde,c.FechaCoberturaHasta";

        public ComprobantePagoBE? ObtenerUltimoPorSocio(int idSocio)
        {
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando("SELECT TOP 1 " + Columnas + " FROM dbo.ComprobantePago c WHERE c.ID_Socio=@id ORDER BY c.Fecha DESC,c.NroComprobante DESC");
            cmd.Parameters.AddWithValue("@id", idSocio);
            using var dr = cmd.ExecuteReader();
            return dr.Read() ? Mapear(dr) : null;
        }

        public List<ComprobantePagoBE> ConsultarComprobantesFiltros(DateTime desde, DateTime hasta, string dniSocio)
        {
            var resultado = new List<ComprobantePagoBE>();
            using var acceso = new AccesoSeguridadDAL(unidad);
            string sql = "SELECT " + Columnas + @",s.DNI,s.Nombre AS NombreSocio,s.Apellido AS ApellidoSocio,u.Nombre AS NombreUsuario
                FROM dbo.ComprobantePago c JOIN dbo.Socio s ON s.ID_Socio=c.ID_Socio JOIN dbo.Usuario u ON u.ID=c.ID_Usuario
                WHERE c.Fecha>=@desde AND c.Fecha<@hasta";
            if (!string.IsNullOrWhiteSpace(dniSocio)) sql += " AND s.DNI LIKE @dni";
            sql += " ORDER BY c.Fecha DESC,c.NroComprobante DESC";
            using var cmd = acceso.CrearComando(sql);
            cmd.Parameters.AddWithValue("@desde", desde.Date);
            cmd.Parameters.AddWithValue("@hasta", hasta.Date.AddDays(1));
            if (!string.IsNullOrWhiteSpace(dniSocio)) cmd.Parameters.AddWithValue("@dni", "%" + dniSocio + "%");
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var comp = Mapear(dr);
                comp.Socio.DNI = dr["DNI"].ToString(); comp.Socio.Nombre = dr["NombreSocio"].ToString(); comp.Socio.Apellido = dr["ApellidoSocio"].ToString();
                comp.EmpleadoCobrador.Nombre = dr["NombreUsuario"].ToString();
                resultado.Add(comp);
            }
            return resultado;
        }

        public List<ComprobantePagoBE> LeerTodos()
        {
            var resultado = new List<ComprobantePagoBE>();
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando("SELECT " + Columnas + " FROM dbo.ComprobantePago c ORDER BY c.NroComprobante");
            using var dr = cmd.ExecuteReader();
            while (dr.Read()) resultado.Add(Mapear(dr));
            return resultado;
        }

        private static ComprobantePagoBE Mapear(SqlDataReader dr) => new ComprobantePagoBE
        {
            NroComprobante = Convert.ToInt32(dr["NroComprobante"]), Fecha = Convert.ToDateTime(dr["Fecha"]),
            MontoTotal = Convert.ToDecimal(dr["MontoTotal"]), MetodoPago = dr["MetodoPago"].ToString(),
            Socio = new SocioBE { ID_Socio = Convert.ToInt32(dr["ID_Socio"]) },
            EmpleadoCobrador = new UsuarioBE { ID = Convert.ToInt32(dr["ID_Usuario"]) },
            DVH = dr["DVH"] == DBNull.Value ? null : dr["DVH"].ToString(),
            CodigoPlanVendido = dr["CodigoPlanVendido"] == DBNull.Value ? null : Convert.ToInt32(dr["CodigoPlanVendido"]),
            NombrePlanVendido = dr["NombrePlanVendido"] == DBNull.Value ? null : dr["NombrePlanVendido"].ToString(),
            DuracionDiasVendida = dr["DuracionDiasVendida"] == DBNull.Value ? null : Convert.ToInt32(dr["DuracionDiasVendida"]),
            FechaCoberturaDesde = dr["FechaCoberturaDesde"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaCoberturaDesde"]),
            FechaCoberturaHasta = dr["FechaCoberturaHasta"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaCoberturaHasta"])
        };
    }
}
