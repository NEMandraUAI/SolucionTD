using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using BE;
using DAL;
using Seguridad;

namespace BLL
{
    public class MercadoInternoBLL
    {
        private readonly ProductoDAL _productoDAL = new ProductoDAL();
        private readonly MercadoInternoDAL _mercadoDAL = new MercadoInternoDAL();
        private readonly SocioDAL _socioDAL = new SocioDAL();
        private readonly RutinaEntrenamientoDAL _rutinaDAL = new RutinaEntrenamientoDAL();
        private readonly PlanNutricionalDAL _planNutDAL = new PlanNutricionalDAL();
        private readonly IntegridadDAL _integridadDAL = new IntegridadDAL();
        private readonly RegistroBLL _registroBLL = new RegistroBLL();

        private Dictionary<string, string> ObtenerDiccionarioNegocio()
        {
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID > 0)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            return new IdiomaDAL().ObtenerTraduccionesPorFormulario(idIdioma, "NegocioBLL");
        }

        private string TN(Dictionary<string, string> dic, string clave, string porDefecto, params object[] args)
        {
            string plantilla = (dic != null && dic.ContainsKey(clave)) ? dic[clave] : porDefecto;
            if (args == null || args.Length == 0) return plantilla;
            try
            {
                return string.Format(plantilla, args);
            }
            catch
            {
                return plantilla;
            }
        }

        private CultureInfo ObtenerCulturaIdiomaActual()
        {
            int idIdioma = (GestorIdioma.Instancia != null && GestorIdioma.Instancia.IdiomaActual != null)
                ? GestorIdioma.Instancia.IdiomaActual.ID
                : 1;
            string codigo = idIdioma == 2 ? "en-US" : (idIdioma == 3 ? "pt-BR" : (idIdioma == 5 ? "it-IT" : "es-AR"));
            return new CultureInfo(codigo);
        }

        public List<string> ObtenerCategoriasProductos()
        {
            return new List<string>
            {
                "Suplementos Deportivos",
                "Equipamiento y Accesorios",
                "Hidratación y Bebidas"
            };
        }

        public List<string> ObtenerMetodosPrediccion()
        {
            return new List<string>
            {
                "Promedio Móvil Ponderado (WMA)",
                "Tendencia Lineal de Holt"
            };
        }

        public List<ProductoBE> ListarProductosActivos()
        {
            return _productoDAL.ListarProductosActivos();
        }

        public List<ProductoBE> ListarTodoElCatalogo()
        {
            return _productoDAL.LeerTodos();
        }

        public void GuardarProducto(ProductoBE prod, int? stockOriginal = null)
        {
            UsuarioBE actual = SessionManager.Instancia.UsuarioActual;
            if (actual == null || (!actual.TienePermiso("ANALIZAR_DEMANDA") && !actual.TienePermiso("GESTION_ROLES")))
                throw new Exception("Operación denegada: Se requiere permiso de Administración Comercial para crear o editar productos del catálogo.");
            if (prod == null)
                throw new Exception("Datos de producto inválidos.");
            if (string.IsNullOrWhiteSpace(prod.CodigoSKU))
                throw new Exception("El código SKU del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(prod.Nombre))
                throw new Exception("El nombre del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(prod.Categoria))
                throw new Exception("Debe seleccionar una categoría para el producto.");
            if (prod.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor a cero.");
            if (prod.CostoCompra < 0 || prod.CostoCompra > prod.PrecioVenta)
                throw new Exception("El costo unitario de compra debe ser mayor o igual a cero y no superar el precio de venta.");
            if (prod.StockActual < 0 || prod.StockMinimo < 0)
                throw new Exception("El stock actual y el stock mínimo no pueden ser negativos.");
            if (prod.TiempoReposicionDias < 1 || prod.TiempoReposicionDias > 90)
                throw new Exception("El tiempo de reposición del proveedor (Lead Time) debe estar entre 1 y 90 días.");
            prod.CodigoSKU = prod.CodigoSKU.Trim().ToUpperInvariant();
            prod.Nombre = prod.Nombre.Trim();
            if (string.IsNullOrWhiteSpace(prod.ObjetivoFisicoAfin)) prod.ObjetivoFisicoAfin = "Todos";
            if (string.IsNullOrWhiteSpace(prod.ObjetivoNutricionalAfin)) prod.ObjetivoNutricionalAfin = "Todos";
            var existentePorSku = _productoDAL.ObtenerPorSKU(prod.CodigoSKU);
            if (existentePorSku != null && existentePorSku.CodigoProducto != prod.CodigoProducto)
                throw new Exception($"Ya existe otro producto registrado con el código SKU '{prod.CodigoSKU}'.");
            _productoDAL.GuardarProductoTransaccional(prod, entidad => DVManager.CalcularDVH(entidad), actual, stockOriginal);
        }

        public bool ObtenerPerfilAfinidadSocio(
            string dniSocio,
            out SocioBE socio,
            out string objetivoFisico,
            out string objetivoNutricional,
            out string resumenSinergia)
        {
            socio = null;
            objetivoFisico = string.Empty;
            objetivoNutricional = string.Empty;
            resumenSinergia = string.Empty;
            if (string.IsNullOrWhiteSpace(dniSocio))
                throw new Exception("Ingrese el DNI del socio para consultar su perfil.");
            socio = _socioDAL.ObtenerPorDNI(dniSocio.Trim());
            if (socio == null)
                return false;
            var dic = ObtenerDiccionarioNegocio();
            var rutinas = _rutinaDAL.ObtenerRutinasPorDNI(socio.DNI)
                .OrderByDescending(r => r.FechaCreacion)
                .ThenByDescending(r => r.CodigoRutina)
                .ToList();
            if (rutinas.Count > 0)
            {
                objetivoFisico = rutinas.First().ObjetivoFisico ?? string.Empty;
            }
            var planesNut = _planNutDAL.ConsultarPlanesPorSocio(socio.ID_Socio)
                .OrderByDescending(p => p.FechaCreacion)
                .ThenByDescending(p => p.CodigoPlan)
                .ToList();
            if (planesNut.Count > 0 && planesNut.First().DatosBiometricos != null)
            {
                objetivoNutricional = planesNut.First().DatosBiometricos.ObjetivoNutricional ?? string.Empty;
            }
            string txtFis = !string.IsNullOrWhiteSpace(objetivoFisico)
                ? objetivoFisico
                : TN(dic, "miSinRutinaActiva", "Sin rutina activa");
            string txtNut = !string.IsNullOrWhiteSpace(objetivoNutricional)
                ? objetivoNutricional
                : TN(dic, "miSinPlanNutri", "Sin plan nutricional");
            resumenSinergia = TN(
                dic,
                "miResumenSocioPos",
                "👤 Socio: {0} (DNI {1})   |   🏋️ Entrenamiento: {2}   |   🥗 Nutrición: {3}",
                $"{socio.Nombre} {socio.Apellido}".Trim(),
                socio.DNI,
                txtFis,
                txtNut);
            return true;
        }

        public bool EsProductoAfinAPerfil(ProductoBE prod, string objetivoFisicoSocio, string objetivoNutricionalSocio)
        {
            if (prod == null) return false;
            bool coincideFisico = false;
            bool coincideNutri = false;
            if (!string.IsNullOrWhiteSpace(objetivoFisicoSocio) && !string.IsNullOrWhiteSpace(prod.ObjetivoFisicoAfin))
            {
                string af = prod.ObjetivoFisicoAfin.Trim();
                if (af.Equals("Todos", StringComparison.OrdinalIgnoreCase) ||
                    objetivoFisicoSocio.IndexOf(af, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    af.IndexOf(objetivoFisicoSocio, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    coincideFisico = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(objetivoNutricionalSocio) && !string.IsNullOrWhiteSpace(prod.ObjetivoNutricionalAfin))
            {
                string an = prod.ObjetivoNutricionalAfin.Trim();
                if (an.Equals("Todos", StringComparison.OrdinalIgnoreCase) ||
                    objetivoNutricionalSocio.IndexOf(an, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    an.IndexOf(objetivoNutricionalSocio, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    coincideNutri = true;
                }
            }
            if (string.IsNullOrWhiteSpace(objetivoFisicoSocio) && string.IsNullOrWhiteSpace(objetivoNutricionalSocio))
                return true;
            return coincideFisico || coincideNutri;
        }

        public void RegistrarVentaMostrador(VentaProductoBE venta)
        {
            UsuarioBE vendedor = SessionManager.Instancia.UsuarioActual;
            if (vendedor == null || (!vendedor.TienePermiso("VENDER_PRODUCTO") && !vendedor.TienePermiso("GESTION_ROLES")))
                throw new Exception("Operación denegada: No posee el permiso requerido (VENDER_PRODUCTO) para registrar ventas en el Mercado Interno.");
            if (venta == null || venta.Socio == null || venta.Socio.ID_Socio <= 0)
                throw new Exception("Debe verificar y asociar un socio válido antes de confirmar la venta.");
            if (!venta.Socio.EstadoActivo)
                throw new Exception("El socio seleccionado se encuentra inactivo en el padrón.");
            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new Exception("El carrito de compra está vacío. Agregue al menos un producto para procesar la venta.");
            if (string.IsNullOrWhiteSpace(venta.MetodoPago))
                throw new Exception("Debe seleccionar un medio de pago válido.");
            foreach (var item in venta.Detalles)
            {
                if (item.Cantidad <= 0)
                    throw new Exception($"La cantidad para el artículo '{item.NombreProducto}' debe ser mayor a cero.");
            }
            DateTime ahora = DateTime.Now;
            venta.Fecha = new DateTime(ahora.Year, ahora.Month, ahora.Day, ahora.Hour, ahora.Minute, ahora.Second);
            venta.ID_Socio = venta.Socio.ID_Socio;
            venta.ID_Usuario = vendedor.ID;
            venta.Vendedor = vendedor;
            venta.MontoTotal = Math.Round(venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario), 2);
            venta.CostoTotalEstimado = Math.Round(venta.Detalles.Sum(d => d.Cantidad * d.CostoUnitario), 2);
            _mercadoDAL.RegistrarVentaProductoTransaccional(venta, entidad => DVManager.CalcularDVH(entidad));
        }

        public List<VentaProductoBE> ConsultarHistorialVentas(DateTime desde, DateTime hasta, string dniSocio, string metodoPago)
        {
            if (desde.Date > hasta.Date)
                throw new Exception("La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
            return _mercadoDAL.ConsultarVentasFiltros(desde, hasta, dniSocio, metodoPago);
        }

        public List<PrediccionProductoBE> ProyectarDemandaYReabastecimiento(
            string metodoPrediccion,
            int horizonteDias,
            bool incluirSinergiaPlanes,
            out decimal facturacionPeriodo,
            out decimal costoVentasPeriodo,
            out decimal gananciaBrutaPeriodo,
            out int totalUnidadesVendidasPeriodo,
            out string resumenSinergiaGimnasio)
        {
            if (horizonteDias <= 0) horizonteDias = 30;
            bool usarHolt = !string.IsNullOrWhiteSpace(metodoPrediccion) &&
                            metodoPrediccion.IndexOf("Holt", StringComparison.OrdinalIgnoreCase) >= 0;
            var dic = ObtenerDiccionarioNegocio();
            CultureInfo culturaActual = ObtenerCulturaIdiomaActual();
            DateTime mesActualInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            int cantMesesVentana = 4;
            List<DateTime> mesesAnalizados = new List<DateTime>();
            for (int i = cantMesesVentana - 1; i >= 0; i--)
            {
                mesesAnalizados.Add(mesActualInicio.AddMonths(-i));
            }
            DateTime fechaDesdeHistoria = mesesAnalizados.First();
            DateTime fechaHastaHistoria = DateTime.Today.AddDays(1).AddSeconds(-1);
            var productos = _productoDAL.ListarProductosActivos();
            var ventasMensuales = _mercadoDAL.ObtenerVentasMensualesPorProducto(fechaDesdeHistoria, fechaHastaHistoria);
            var enTransitoPorProd = _mercadoDAL.ObtenerCantidadesEnTransitoPorProducto();
            facturacionPeriodo = Math.Round(ventasMensuales.Sum(v => v.MontoVendido), 2);
            costoVentasPeriodo = Math.Round(ventasMensuales.Sum(v => v.CostoVendido), 2);
            gananciaBrutaPeriodo = Math.Max(0m, facturacionPeriodo - costoVentasPeriodo);
            totalUnidadesVendidasPeriodo = ventasMensuales.Sum(v => v.CantidadVendida);
            var sociosActivos = _socioDAL.LeerTodos().Where(s => s.EstadoActivo).ToList();
            int totalSociosActivos = Math.Max(1, sociosActivos.Count);
            var todasLasRutinas = _rutinaDAL.LeerTodos();
            var todosLosPlanesNut = _planNutDAL.LeerTodos();
            Dictionary<int, string> objFisicoPorSocio = new Dictionary<int, string>();
            Dictionary<int, string> objNutriPorSocio = new Dictionary<int, string>();
            foreach (var soc in sociosActivos)
            {
                var ultRut = todasLasRutinas
                    .Where(r => r.Socio != null && r.Socio.ID_Socio == soc.ID_Socio)
                    .OrderByDescending(r => r.FechaCreacion)
                    .ThenByDescending(r => r.CodigoRutina)
                    .FirstOrDefault();
                if (ultRut != null && !string.IsNullOrWhiteSpace(ultRut.ObjetivoFisico))
                {
                    objFisicoPorSocio[soc.ID_Socio] = ultRut.ObjetivoFisico;
                }
                var ultNut = todosLosPlanesNut
                    .Where(p => p.ID_Socio == soc.ID_Socio)
                    .OrderByDescending(p => p.FechaCreacion)
                    .ThenByDescending(p => p.CodigoPlan)
                    .FirstOrDefault();
                if (ultNut != null && ultNut.DatosBiometricos != null && !string.IsNullOrWhiteSpace(ultNut.DatosBiometricos.ObjetivoNutricional))
                {
                    objNutriPorSocio[soc.ID_Socio] = ultNut.DatosBiometricos.ObjetivoNutricional;
                }
            }
            int cantHipertrofiaFuerza = objFisicoPorSocio.Values.Count(v =>
                v.IndexOf("Hipertrofia", StringComparison.OrdinalIgnoreCase) >= 0 ||
                v.IndexOf("Fuerza", StringComparison.OrdinalIgnoreCase) >= 0);
            int cantDescensoResistencia = objFisicoPorSocio.Values.Count(v =>
                v.IndexOf("Peso", StringComparison.OrdinalIgnoreCase) >= 0 ||
                v.IndexOf("Resistencia", StringComparison.OrdinalIgnoreCase) >= 0);
            int cantSuperavit = objNutriPorSocio.Values.Count(v =>
                v.IndexOf("Superávit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                v.IndexOf("Superavit", StringComparison.OrdinalIgnoreCase) >= 0);
            int cantDeficit = objNutriPorSocio.Values.Count(v =>
                v.IndexOf("Déficit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                v.IndexOf("Deficit", StringComparison.OrdinalIgnoreCase) >= 0);
            resumenSinergiaGimnasio = TN(
                dic,
                "miPadronSinergia",
                "Padrón Activo: {0} socio(s) | Hipertrofia/Fuerza: {1} | Descenso/Resistencia: {2} | Superávit: {3} | Déficit: {4}",
                sociosActivos.Count,
                cantHipertrofiaFuerza,
                cantDescensoResistencia,
                cantSuperavit,
                cantDeficit);
            List<PrediccionProductoBE> resultado = new List<PrediccionProductoBE>();
            foreach (var prod in productos)
            {
                List<decimal> serieMensual = new List<decimal>();
                List<string> etiquetasHistorial = new List<string>();
                List<PuntoEvolucionBE> puntosGrafico = new List<PuntoEvolucionBE>();
                foreach (DateTime mes in mesesAnalizados)
                {
                    var regMes = ventasMensuales.FirstOrDefault(v =>
                        v.CodigoProducto == prod.CodigoProducto &&
                        v.Anio == mes.Year &&
                        v.Mes == mes.Month);
                    int cantMes = regMes.CantidadVendida;
                    serieMensual.Add(cantMes);
                    string nombreMesCorto = mes.ToString("MMM", culturaActual).TrimEnd('.');
                    if (nombreMesCorto.Length > 0)
                        nombreMesCorto = char.ToUpper(nombreMesCorto[0]) + nombreMesCorto.Substring(1);
                    etiquetasHistorial.Add($"{nombreMesCorto}:{cantMes}");
                    puntosGrafico.Add(new PuntoEvolucionBE
                    {
                        Fecha = mes,
                        EtiquetaFecha = $"{nombreMesCorto} {mes:yy}",
                        ValorPrimario = cantMes,
                        ValorSecundario = prod.StockActual,
                        Detalle = $"{cantMes} u."
                    });
                }
                int totalHistoricoProd = (int)serieMensual.Sum();
                int periodosConVentas = serieMensual.Count(x => x > 0);
                decimal pronosticoBaseMensual = 0m;
                decimal pendienteTendencia = 0m;
                decimal mae = 0m;
                decimal mape = 0m;
                string textoErrorMape = TN(dic, "miColdStartMape", "N/A (Arranque en frío)");
                string textoTendencia = TN(dic, "miColdStartTend", "❄️ Sin Historial (Cold Start)");
                if (totalHistoricoProd > 0)
                {
                    if (usarHolt)
                    {
                        pronosticoBaseMensual = CalcularSuavizadoDobleHolt(serieMensual, 0.60m, 0.40m, out pendienteTendencia, out mae, out mape);
                    }
                    else
                    {
                        pronosticoBaseMensual = CalcularPromedioMovilPonderado(serieMensual, out pendienteTendencia, out mae, out mape);
                    }
                    if (periodosConVentas >= 2)
                    {
                        textoErrorMape = $"{mape:0.0}% (MAE: {mae:0.0} u.)";
                    }
                    else
                    {
                        textoErrorMape = TN(dic, "miMuestraInicial", "Muestra inicial (<2 meses)");
                    }
                    if (pendienteTendencia > 1.0m)
                        textoTendencia = TN(dic, "miTendAlza", "📈 En Alza (+{0}/mes)", pendienteTendencia.ToString("0.0"));
                    else if (pendienteTendencia < -1.0m)
                        textoTendencia = TN(dic, "miTendBaja", "📉 En Baja ({0}/mes)", pendienteTendencia.ToString("0.0"));
                    else
                        textoTendencia = TN(dic, "miTendEstable", "➡️ Estable");
                }
                int sociosAfinesActuales = 0;
                foreach (var soc in sociosActivos)
                {
                    string of = objFisicoPorSocio.ContainsKey(soc.ID_Socio) ? objFisicoPorSocio[soc.ID_Socio] : string.Empty;
                    string on = objNutriPorSocio.ContainsKey(soc.ID_Socio) ? objNutriPorSocio[soc.ID_Socio] : string.Empty;
                    if (EsProductoAfinAPerfil(prod, of, on))
                        sociosAfinesActuales++;
                }
                decimal proporcionAfin = (decimal)sociosAfinesActuales / totalSociosActivos;
                decimal ajusteSinergia = 0m;
                if (incluirSinergiaPlanes && pronosticoBaseMensual > 0m && sociosAfinesActuales > 0)
                {
                    decimal porcentajeBoost = Math.Min(0.15m, Math.Round(proporcionAfin * 0.18m, 3));
                    ajusteSinergia = Math.Round(pronosticoBaseMensual * porcentajeBoost, 1);
                }
                decimal demandaMensualFinal = Math.Round(pronosticoBaseMensual + ajusteSinergia, 1);
                decimal demandaEnHorizonte = Math.Round(demandaMensualFinal * (horizonteDias / 30.0m), 1);
                DateTime mesProyectado = mesActualInicio.AddMonths(1);
                puntosGrafico.Add(new PuntoEvolucionBE
                {
                    Fecha = mesProyectado,
                    EtiquetaFecha = $"Proy. ({horizonteDias}d)",
                    ValorPrimario = Math.Round(demandaEnHorizonte, 1),
                    ValorSecundario = prod.StockActual,
                    Detalle = $"{demandaEnHorizonte:0.0} u."
                });
                decimal demandaDiaria = Math.Round(demandaMensualFinal / 30.0m, 2);
                int demandaLeadTime = (int)Math.Ceiling(demandaDiaria * prod.TiempoReposicionDias);
                int stockSeguridad = Math.Max(prod.StockMinimo, (int)Math.Ceiling(0.5m * demandaLeadTime));
                int rop = demandaLeadTime + stockSeguridad;
                int stockEnTransito = enTransitoPorProd.ContainsKey(prod.CodigoProducto) ? enTransitoPorProd[prod.CodigoProducto] : 0;
                int diasAutonomia = demandaDiaria > 0m
                    ? (int)Math.Floor(prod.StockActual / demandaDiaria)
                    : 999;
                string textoAutonomia = demandaDiaria > 0m
                    ? TN(dic, "miAutonomiaDias", "{0} día(s)", diasAutonomia)
                    : TN(dic, "miSinRotacion", "Sin rotación");
                string nivelRiesgo;
                int codSeveridad;
                if (prod.StockActual <= prod.StockMinimo || (demandaDiaria > 0m && diasAutonomia <= prod.TiempoReposicionDias))
                {
                    nivelRiesgo = TN(dic, "miRiesgoCritico", "🚨 CRÍTICO (Riesgo de Ruptura)");
                    codSeveridad = 3;
                }
                else if (prod.StockActual <= rop)
                {
                    nivelRiesgo = TN(dic, "miRiesgoAlerta", "⚠️ ALERTA (En Punto de Reorden)");
                    codSeveridad = 2;
                }
                else
                {
                    nivelRiesgo = TN(dic, "miRiesgoOptimo", "✅ ÓPTIMO (Abastecido)");
                    codSeveridad = 1;
                }
                int cantidadSugerida;
                string calculoDesglosado;
                if (totalHistoricoProd == 0)
                {
                    cantidadSugerida = Math.Max(0, prod.StockMinimo - (prod.StockActual + stockEnTransito));
                    calculoDesglosado = TN(
                        dic,
                        "miFormColdStart",
                        "{0} (Mín) - {1} (Stock) - {2} (Tráns) = {3} u. [Cold Start]",
                        prod.StockMinimo,
                        prod.StockActual,
                        stockEnTransito,
                        cantidadSugerida);
                }
                else
                {
                    int demRedondeada = (int)Math.Ceiling(demandaEnHorizonte);
                    int necesidadTotal = demRedondeada + stockSeguridad;
                    cantidadSugerida = Math.Max(0, necesidadTotal - (prod.StockActual + stockEnTransito));
                    calculoDesglosado = TN(
                        dic,
                        "miFormNormal",
                        "{0} (Dem) + {1} (Mín) - {2} (Stock) - {3} (Tráns) = {4} u.",
                        demRedondeada,
                        stockSeguridad,
                        prod.StockActual,
                        stockEnTransito,
                        cantidadSugerida);
                }
                decimal stockPromedio = Math.Max(1m, (prod.StockActual + prod.StockMinimo) / 2.0m);
                decimal rotacion = Math.Round(totalHistoricoProd / stockPromedio, 2);
                string sufSocios = TN(dic, "miSociosAfinesSuf", "socio(s)");
                resultado.Add(new PrediccionProductoBE
                {
                    CodigoProducto = prod.CodigoProducto,
                    CodigoSKU = prod.CodigoSKU,
                    Producto = prod.Nombre,
                    Categoria = prod.Categoria,
                    HistorialMensual = string.Join(" | ", etiquetasHistorial),
                    TotalUnidadesVendidasHistoria = totalHistoricoProd,
                    PeriodosConDatos = periodosConVentas,
                    Tendencia = textoTendencia,
                    DemandaBaseSerie = pronosticoBaseMensual,
                    AjusteSinergiaPlanes = ajusteSinergia,
                    DemandaProyectada = demandaEnHorizonte,
                    ErrorPronosticoMAPE = textoErrorMape,
                    ValorMAE = mae,
                    ValorMAPE = mape,
                    StockMinimo = stockSeguridad,
                    StockActual = prod.StockActual,
                    StockEnTransito = stockEnTransito,
                    TiempoReposicionDias = prod.TiempoReposicionDias,
                    PuntoReordenROP = rop,
                    AutonomiaDias = textoAutonomia,
                    NivelRiesgoStock = nivelRiesgo,
                    CodigoSeveridadRiesgo = codSeveridad,
                    AfinidadDeportivaNutricional = $"{prod.ObjetivoFisicoAfin} / {prod.ObjetivoNutricionalAfin} ({sociosAfinesActuales} {sufSocios})",
                    CalculoSugerencia = calculoDesglosado,
                    CantidadSugerida = cantidadSugerida,
                    CantidadAComprarAdmin = cantidadSugerida,
                    CostoUnitario = prod.CostoCompra,
                    PrecioVentaUnitario = prod.PrecioVenta,
                    MargenPorcentaje = prod.MargenPorcentaje,
                    IndiceRotacion = rotacion,
                    SerieTemporalGrafico = puntosGrafico
                });
            }
            return resultado
                .OrderByDescending(r => r.CodigoSeveridadRiesgo)
                .ThenByDescending(r => r.DemandaProyectada)
                .ToList();
        }

        private decimal CalcularPromedioMovilPonderado(
            List<decimal> serie,
            out decimal pendienteTendencia,
            out decimal mae,
            out decimal mape)
        {
            pendienteTendencia = 0m;
            mae = 0m;
            mape = 0m;
            if (serie == null || serie.Count == 0) return 0m;
            int n = serie.Count;
            decimal pronostico;
            if (n >= 3)
            {
                pronostico = (0.50m * serie[n - 1]) + (0.30m * serie[n - 2]) + (0.20m * serie[n - 3]);
                pendienteTendencia = Math.Round((serie[n - 1] - serie[0]) / (n - 1), 1);
            }
            else if (n == 2)
            {
                pronostico = (0.60m * serie[1]) + (0.40m * serie[0]);
                pendienteTendencia = Math.Round(serie[1] - serie[0], 1);
            }
            else
            {
                pronostico = serie[0];
            }
            List<decimal> erroresAbs = new List<decimal>();
            decimal sumaRealesEval = 0m;
            for (int t = 1; t < n; t++)
            {
                decimal predT = t == 1
                    ? serie[0]
                    : (t == 2
                        ? (0.60m * serie[1]) + (0.40m * serie[0])
                        : (0.50m * serie[t - 1]) + (0.30m * serie[t - 2]) + (0.20m * serie[t - 3]));
                erroresAbs.Add(Math.Abs(serie[t] - predT));
                sumaRealesEval += serie[t];
            }
            if (erroresAbs.Count > 0)
            {
                mae = Math.Round(erroresAbs.Average(), 1);
                mape = sumaRealesEval > 0m ? Math.Round((erroresAbs.Sum() / sumaRealesEval) * 100m, 1) : 0m;
            }
            return Math.Round(Math.Max(0m, pronostico), 1);
        }

        private decimal CalcularSuavizadoDobleHolt(
            List<decimal> serie,
            decimal alpha,
            decimal beta,
            out decimal pendienteTendencia,
            out decimal mae,
            out decimal mape)
        {
            pendienteTendencia = 0m;
            mae = 0m;
            mape = 0m;
            if (serie == null || serie.Count == 0) return 0m;
            if (serie.Count == 1) return serie[0];
            decimal nivel = serie[0];
            decimal tendencia = serie[1] - serie[0];
            List<decimal> erroresAbs = new List<decimal>();
            decimal sumaRealesEval = 0m;
            for (int t = 1; t < serie.Count; t++)
            {
                decimal pronosticoUnPaso = Math.Max(0m, nivel + tendencia);
                decimal valorReal = serie[t];
                erroresAbs.Add(Math.Abs(valorReal - pronosticoUnPaso));
                sumaRealesEval += valorReal;
                decimal nivelAnterior = nivel;
                nivel = (alpha * valorReal) + ((1m - alpha) * (nivelAnterior + tendencia));
                tendencia = (beta * (nivel - nivelAnterior)) + ((1m - beta) * tendencia);
            }
            pendienteTendencia = Math.Round(tendencia, 1);
            if (erroresAbs.Count > 0)
            {
                mae = Math.Round(erroresAbs.Average(), 1);
                mape = sumaRealesEval > 0m ? Math.Round((erroresAbs.Sum() / sumaRealesEval) * 100m, 1) : 0m;
            }
            decimal proyeccionFutura = Math.Max(0m, nivel + tendencia);
            return Math.Round(proyeccionFutura, 1);
        }

        public OrdenReabastecimientoBE GenerarOrdenReabastecimiento(
            List<PrediccionProductoBE> itemsEvaluados,
            string metodoPrediccion,
            int horizonteDias,
            string observaciones)
        {
            UsuarioBE admin = SessionManager.Instancia.UsuarioActual;
            if (admin == null || (!admin.TienePermiso("ANALIZAR_DEMANDA") && !admin.TienePermiso("GESTION_ROLES")))
                throw new Exception("Operación denegada: Se requiere el permiso 'ANALIZAR_DEMANDA' para emitir órdenes de reabastecimiento.");
            if (itemsEvaluados == null)
                throw new Exception("No hay productos evaluados para generar la orden.");
            var itemsAPedir = itemsEvaluados.Where(x => x.CantidadAComprarAdmin > 0).ToList();
            if (itemsAPedir.Count == 0)
                throw new Exception("Debe especificar una cantidad a solicitar mayor a cero en al menos un producto para emitir la Orden de Reabastecimiento.");
            DateTime ahora = DateTime.Now;
            OrdenReabastecimientoBE orden = new OrdenReabastecimientoBE
            {
                FechaGeneracion = new DateTime(ahora.Year, ahora.Month, ahora.Day, ahora.Hour, ahora.Minute, ahora.Second),
                MetodoPrediccion = string.IsNullOrWhiteSpace(metodoPrediccion) ? "Promedio Móvil Ponderado (WMA)" : metodoPrediccion,
                HorizonteDias = horizonteDias > 0 ? horizonteDias : 30,
                Estado = "Pendiente",
                Observaciones = observaciones ?? string.Empty,
                ID_Usuario = admin.ID,
                Administrador = admin
            };
            foreach (var item in itemsAPedir)
            {
                decimal sub = Math.Round(item.CantidadAComprarAdmin * item.CostoUnitario, 2);
                orden.Detalles.Add(new DetalleOrdenReabastecimientoBE
                {
                    CodigoProducto = item.CodigoProducto,
                    CodigoSKU = item.CodigoSKU,
                    NombreProducto = item.Producto,
                    StockAlMomento = item.StockActual,
                    DemandaProyectada = item.DemandaProyectada,
                    CantidadSugeridaSistema = item.CantidadSugerida,
                    CantidadAprobadaAdmin = item.CantidadAComprarAdmin,
                    CostoUnitarioCompra = item.CostoUnitario,
                    SubtotalCosto = sub
                });
            }
            orden.CostoTotalOrden = Math.Round(orden.Detalles.Sum(d => d.SubtotalCosto), 2);
            _mercadoDAL.RegistrarOrdenReabastecimientoTransaccional(orden, entidad => DVManager.CalcularDVH(entidad));
            return orden;
        }

        public void RecibirOrdenReabastecimiento(int nroOrden)
        {
            UsuarioBE actual = SessionManager.Instancia.UsuarioActual;
            if (actual == null || (!actual.TienePermiso("ANALIZAR_DEMANDA") && !actual.TienePermiso("CONSULTAR_STOCK") && !actual.TienePermiso("GESTION_ROLES")))
                throw new Exception("Operación denegada: No posee permisos para recepcionar órdenes de reabastecimiento.");
            _mercadoDAL.RecibirOrdenReabastecimientoTransaccional(nroOrden, entidad => DVManager.CalcularDVH(entidad), actual);
        }

        public List<OrdenReabastecimientoBE> ListarOrdenesReabastecimiento()
        {
            return _mercadoDAL.ConsultarOrdenesReabastecimiento();
        }

        public PaqueteReabastecimientoSerializadoBE ConstruirPaqueteSerializado(
            List<PrediccionProductoBE> predicciones,
            string metodoPrediccion,
            int horizonteDias,
            bool incluyeSinergia,
            decimal facturacionPeriodo,
            decimal costoPeriodo,
            decimal gananciaPeriodo,
            string resumenSinergia,
            int? nroOrdenAsociada = null)
        {
            UsuarioBE admin = SessionManager.Instancia.UsuarioActual;
            decimal margenProm = facturacionPeriodo > 0m
                ? Math.Round((gananciaPeriodo / facturacionPeriodo) * 100m, 1)
                : 0m;
            var paquete = new PaqueteReabastecimientoSerializadoBE
            {
                FechaGeneracion = DateTime.Now,
                UsuarioAdministrador = admin != null ? admin.Nombre : "Administrador",
                NroOrdenReabastecimientoAsociada = nroOrdenAsociada,
                MetodoPrediccionUtilizado = metodoPrediccion ?? "Promedio Móvil Ponderado (WMA)",
                HorizontePlanificacionDias = horizonteDias,
                SinergiaEntrenamientoNutricionActiva = incluyeSinergia,
                FacturacionBrutaPeriodo = facturacionPeriodo,
                CostoMercaderiaVendidaPeriodo = costoPeriodo,
                GananciaBrutaPeriodo = gananciaPeriodo,
                MargenPromedioPorcentaje = margenProm,
                UnidadesTotalesReabastecer = predicciones != null ? predicciones.Sum(x => x.CantidadAComprarAdmin) : 0,
                InversionTotalEstimadaReabastecimiento = predicciones != null ? Math.Round(predicciones.Sum(x => x.CostoTotalReposicion), 2) : 0m,
                ResumenSinergiaPoblacion = resumenSinergia ?? string.Empty
            };
            if (predicciones != null)
            {
                foreach (var p in predicciones)
                {
                    paquete.ProductosEvaluados.Add(new ItemReabastecimientoSerializadoBE
                    {
                        CodigoProducto = p.CodigoProducto,
                        CodigoSKU = p.CodigoSKU,
                        NombreProducto = p.Producto,
                        Categoria = p.Categoria,
                        HistorialMensualVentas = p.HistorialMensual,
                        TendenciaDetectada = p.Tendencia,
                        DemandaProyectadaUnidades = p.DemandaProyectada,
                        ErrorPronosticoMAPE = p.ErrorPronosticoMAPE,
                        StockFisicoDisponible = p.StockActual,
                        StockEnTransitoPendiente = p.StockEnTransito,
                        PuntoReordenROP = p.PuntoReordenROP,
                        NivelRiesgoRuptura = p.NivelRiesgoStock,
                        AfinidadEntrenamientoNutricion = p.AfinidadDeportivaNutricional,
                        CantidadSugeridaPorSistema = p.CantidadSugerida,
                        CantidadDecididaPorAdministrador = p.CantidadAComprarAdmin,
                        CostoUnitarioReposicion = p.CostoUnitario,
                        SubtotalReposicion = p.CostoTotalReposicion
                    });
                }
            }
            paquete.HashIntegridadSHA256 = CalcularHashCanonicaPaquete(paquete);
            return paquete;
        }

        private string CalcularHashCanonicaPaquete(PaqueteReabastecimientoSerializadoBE p)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(p.MetodoPrediccionUtilizado).Append('|');
            sb.Append(p.HorizontePlanificacionDias).Append('|');
            sb.Append(p.FacturacionBrutaPeriodo.ToString("0.00", CultureInfo.InvariantCulture)).Append('|');
            sb.Append(p.UnidadesTotalesReabastecer).Append('|');
            sb.Append(p.InversionTotalEstimadaReabastecimiento.ToString("0.00", CultureInfo.InvariantCulture)).Append('|');
            if (p.ProductosEvaluados != null)
            {
                foreach (var item in p.ProductosEvaluados.OrderBy(x => x.CodigoProducto))
                {
                    sb.Append(item.CodigoProducto).Append(':');
                    sb.Append(item.CodigoSKU).Append(':');
                    sb.Append(item.DemandaProyectadaUnidades.ToString("0.00", CultureInfo.InvariantCulture)).Append(':');
                    sb.Append(item.StockFisicoDisponible).Append(':');
                    sb.Append(item.CantidadSugeridaPorSistema).Append(':');
                    sb.Append(item.CantidadDecididaPorAdministrador).Append(':');
                    sb.Append(item.CostoUnitarioReposicion.ToString("0.00", CultureInfo.InvariantCulture)).Append(';');
                }
            }
            return CryptoManager.GenerarHash(sb.ToString());
        }

        public void ExportarPlanReabastecimientoJson(string rutaArchivo, PaqueteReabastecimientoSerializadoBE paquete)
        {
            if (paquete == null)
                throw new Exception("No hay datos de planificación para serializar.");
            paquete.HashIntegridadSHA256 = CalcularHashCanonicaPaquete(paquete);
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(paquete, opciones);
            File.WriteAllText(rutaArchivo, json, Encoding.UTF8);
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                _registroBLL.RegistrarEvento(
                    $"Archivo serializado JSON de planificación comercial exportado ({Path.GetFileName(rutaArchivo)}).",
                    SessionManager.Instancia.UsuarioActual,
                    "INFO");
            }
        }

        public PaqueteReabastecimientoSerializadoBE ImportarYVerificarPlanReabastecimientoJson(
            string rutaArchivo,
            out bool estructuraValida,
            out bool integridadValida)
        {
            estructuraValida = false;
            integridadValida = false;
            if (!File.Exists(rutaArchivo))
                throw new Exception("El archivo JSON seleccionado no existe.");
            string contenido = File.ReadAllText(rutaArchivo, Encoding.UTF8);
            PaqueteReabastecimientoSerializadoBE paquete;
            try
            {
                paquete = JsonSerializer.Deserialize<PaqueteReabastecimientoSerializadoBE>(contenido);
            }
            catch
            {
                return null;
            }
            if (paquete == null || paquete.ProductosEvaluados == null || paquete.ProductosEvaluados.Count == 0)
            {
                return null;
            }
            estructuraValida = true;
            string hashCalculado = CalcularHashCanonicaPaquete(paquete);
            integridadValida = !string.IsNullOrWhiteSpace(paquete.HashIntegridadSHA256) &&
                               string.Equals(hashCalculado, paquete.HashIntegridadSHA256, StringComparison.OrdinalIgnoreCase);
            return paquete;
        }

        public PaqueteReabastecimientoSerializadoBE RepararYResellarArchivoJsonCorrupto(
            string rutaArchivo,
            PaqueteReabastecimientoSerializadoBE paqueteConHashInvalido,
            List<PrediccionProductoBE> prediccionesMotorActual,
            string metodoActual,
            int horizonteActual,
            bool incluyeSinergiaActual,
            decimal facturacionActual,
            decimal costoActual,
            decimal gananciaActual,
            string resumenSinergiaActual,
            out string detalleReparacion)
        {
            PaqueteReabastecimientoSerializadoBE paqueteReparado;
            if (paqueteConHashInvalido != null && paqueteConHashInvalido.ProductosEvaluados != null && paqueteConHashInvalido.ProductosEvaluados.Count > 0)
            {
                var catalogoOficial = _productoDAL.LeerTodos();
                foreach (var item in paqueteConHashInvalido.ProductosEvaluados)
                {
                    if (item.CantidadDecididaPorAdministrador < 0)
                        item.CantidadDecididaPorAdministrador = Math.Max(0, item.CantidadSugeridaPorSistema);
                    var prodBD = catalogoOficial.FirstOrDefault(p =>
                        p.CodigoProducto == item.CodigoProducto ||
                        string.Equals(p.CodigoSKU, item.CodigoSKU, StringComparison.OrdinalIgnoreCase));
                    if (prodBD != null)
                    {
                        item.CodigoProducto = prodBD.CodigoProducto;
                        item.CodigoSKU = prodBD.CodigoSKU;
                        item.NombreProducto = prodBD.Nombre;
                        item.CostoUnitarioReposicion = prodBD.CostoCompra;
                        item.StockFisicoDisponible = prodBD.StockActual;
                    }
                    var predActual = prediccionesMotorActual?.FirstOrDefault(x => x.CodigoProducto == item.CodigoProducto);
                    if (predActual != null)
                    {
                        item.DemandaProyectadaUnidades = predActual.DemandaProyectada;
                        item.CantidadSugeridaPorSistema = predActual.CantidadSugerida;
                    }
                    item.SubtotalReposicion = Math.Round(item.CantidadDecididaPorAdministrador * item.CostoUnitarioReposicion, 2);
                }
                paqueteConHashInvalido.FacturacionBrutaPeriodo = facturacionActual;
                paqueteConHashInvalido.CostoMercaderiaVendidaPeriodo = costoActual;
                paqueteConHashInvalido.GananciaBrutaPeriodo = gananciaActual;
                paqueteConHashInvalido.MargenPromedioPorcentaje = facturacionActual > 0m
                    ? Math.Round((gananciaActual / facturacionActual) * 100m, 1)
                    : 0m;
                paqueteConHashInvalido.UnidadesTotalesReabastecer = paqueteConHashInvalido.ProductosEvaluados.Sum(x => x.CantidadDecididaPorAdministrador);
                paqueteConHashInvalido.InversionTotalEstimadaReabastecimiento = Math.Round(paqueteConHashInvalido.ProductosEvaluados.Sum(x => x.SubtotalReposicion), 2);
                paqueteConHashInvalido.HashIntegridadSHA256 = CalcularHashCanonicaPaquete(paqueteConHashInvalido);
                paqueteReparado = paqueteConHashInvalido;
                detalleReparacion = "Se sanearon los costos unitarios, demandas proyectadas y subtotales contra la base de datos oficial y se recalculó el sello SHA-256.";
            }
            else
            {
                paqueteReparado = ConstruirPaqueteSerializado(
                    prediccionesMotorActual,
                    metodoActual,
                    horizonteActual,
                    incluyeSinergiaActual,
                    facturacionActual,
                    costoActual,
                    gananciaActual,
                    resumenSinergiaActual,
                    null);
                detalleReparacion = "La estructura sintáctica del archivo estaba dañada. El archivo fue reconstruido íntegramente a partir de la corrida actual del motor predictivo y resellado con SHA-256.";
            }
            ExportarPlanReabastecimientoJson(rutaArchivo, paqueteReparado);
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                _registroBLL.RegistrarEvento(
                    $"Recuperación y resellado criptográfico de archivo JSON corrupto ({Path.GetFileName(rutaArchivo)}).",
                    SessionManager.Instancia.UsuarioActual,
                    "ALERTA");
            }
            return paqueteReparado;
        }

        public void RecalcularDVVProducto()
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "Producto");
            unidad.Confirmar();
        }

        public void RecalcularDVVVentaProducto()
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "VentaProducto");
            unidad.Confirmar();
        }

        public void RecalcularDVVOrdenReabastecimiento()
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, "OrdenReabastecimiento");
            unidad.Confirmar();
        }
    }
}