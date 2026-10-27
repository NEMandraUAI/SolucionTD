using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmVentasMostrador : Form, IObserverIdioma
    {
        private readonly MercadoInternoBLL _mercadoBLL = new MercadoInternoBLL();
        private readonly IdiomaBLL _idiomaBLL = new IdiomaBLL();
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private Dictionary<string, string> _traduccionesRecepcion = new Dictionary<string, string>();
        private List<ProductoBE> _catalogoCompleto = new List<ProductoBE>();
        private List<ProductoBE> _catalogoFiltrado = new List<ProductoBE>();
        private readonly List<DetalleVentaProductoBE> _carrito = new List<DetalleVentaProductoBE>();
        private List<VentaProductoBE> _historialCargado = new List<VentaProductoBE>();
        private SocioBE _socioActual = null;
        private string _objetivoFisicoSocio = string.Empty;
        private string _objetivoNutricionalSocio = string.Empty;
        private string _resumenSinergiaSocio = string.Empty;
        private bool _sincronizandoSeleccion = false;

        private string T(string clave, string porDefecto)
        {
            string valor = _traducciones != null && _traducciones.ContainsKey(clave)
                ? _traducciones[clave]
                : porDefecto;
            return (valor ?? string.Empty)
                .Replace("\\r\\n", Environment.NewLine)
                .Replace("\\n", Environment.NewLine);
        }

        private string TRec(string clave, string porDefecto)
        {
            return _traduccionesRecepcion != null && _traduccionesRecepcion.ContainsKey(clave) ? _traduccionesRecepcion[clave] : porDefecto;
        }

        public frmVentasMostrador()
        {
            InitializeComponent();
            DesactivarMnemonicRecursivo(this.Controls);
            dgvStockCatalogo.CellFormatting += dgvStockCatalogo_CellFormatting;
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void DesactivarMnemonicRecursivo(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (c is Label lbl) lbl.UseMnemonic = false;
                else if (c is Button btn) btn.UseMnemonic = false;
                else if (c is CheckBox chk) chk.UseMnemonic = false;
                if (c.HasChildren)
                    DesactivarMnemonicRecursivo(c.Controls);
            }
        }

        private void frmVentasMostrador_Load(object sender, EventArgs e)
        {
            dtpHistDesde.Value = DateTime.Today.AddMonths(-4);
            dtpHistHasta.Value = DateTime.Today;
            CargarCatalogoDesdeBD();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            AplicarPermisosRol();
            CargarHistorialVentas();
        }

        private void AplicarPermisosRol()
        {
            UsuarioBE u = SessionManager.Instancia.UsuarioActual;
            bool puedeVender = u != null && (u.TienePermiso("VENDER_PRODUCTO") || u.TienePermiso("GESTION_ROLES"));
            btnAgregarCarrito.Enabled = puedeVender;
            btnQuitarCarrito.Enabled = puedeVender;
            btnConfirmarVentaProd.Enabled = puedeVender;
        }

        private void CargarCatalogoDesdeBD()
        {
            _catalogoCompleto = _mercadoBLL.ListarProductosActivos();
        }

        private void PoblarComboCategorias()
        {
            _sincronizandoSeleccion = true;
            string claveAnterior = cmbCategoriaFiltro.SelectedValue != null
                ? cmbCategoriaFiltro.SelectedValue.ToString()
                : "Todas";
            Dictionary<string, string> cats = new Dictionary<string, string>
            {
                { "Todas", T("catTodas", "Todas") },
                { "Suplementos Deportivos", T("catSuplementos", "Suplementos Deportivos") },
                { "Equipamiento y Accesorios", T("catEquipamiento", "Equipamiento y Accesorios") },
                { "Hidratación y Bebidas", T("catHidratacion", "Hidratación y Bebidas") }
            };
            cmbCategoriaFiltro.DataSource = new BindingSource(cats, null);
            cmbCategoriaFiltro.DisplayMember = "Value";
            cmbCategoriaFiltro.ValueMember = "Key";
            if (cats.ContainsKey(claveAnterior))
                cmbCategoriaFiltro.SelectedValue = claveAnterior;
            _sincronizandoSeleccion = false;
        }

        private void PoblarComboMediosPago()
        {
            string seleccionActual = cmbMetodoPagoVenta.SelectedValue != null
                ? cmbMetodoPagoVenta.SelectedValue.ToString()
                : "Efectivo";
            Dictionary<string, string> metodos = new Dictionary<string, string>
            {
                { "Efectivo", TRec("metodoEfectivo", "Efectivo") },
                { "Transferencia", TRec("metodoTransferencia", "Transferencia") },
                { "Tarjeta de Débito", TRec("metodoDebito", "Tarjeta de Débito") },
                { "Tarjeta de Crédito", TRec("metodoCredito", "Tarjeta de Crédito") }
            };
            cmbMetodoPagoVenta.DataSource = new BindingSource(metodos, null);
            cmbMetodoPagoVenta.DisplayMember = "Value";
            cmbMetodoPagoVenta.ValueMember = "Key";
            if (metodos.ContainsKey(seleccionActual))
                cmbMetodoPagoVenta.SelectedValue = seleccionActual;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            if (idioma == null) return;
            _traducciones = _idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            _traduccionesRecepcion = _idiomaBLL.ObtenerTraducciones(idioma, "frmRecepcion");
            this.Text = T("titVentasMostrador", "Punto de Venta de Mostrador e Inventario Local");
            lblTituloSeleccion.Text = T("lblTituloSeleccion", "1. IDENTIFICACIÓN DEL SOCIO Y CATÁLOGO DE PRODUCTOS");
            lblBuscarSocioDNI.Text = T("lblBuscarSocioDNI", "DNI del Socio Comprador:");
            btnVerificarSocio.Text = T("btnVerificarSocio", "Verificar Perfil");
            chkFiltrarAfines.Text = T("chkFiltrarAfines", "🎯 Mostrar solo productos afines al objetivo físico y nutricional del socio");
            lblCategoriaFiltro.Text = T("lblCategoriaFiltro", "Categoría:");
            lblProductoSelect.Text = T("lblProductoSelect", "Artículo / Producto:");
            lblCantidadVenta.Text = T("lblCantidadVenta", "Cantidad:");
            btnAgregarCarrito.Text = T("btnAgregarCarrito", "➕ Agregar al Carrito");
            btnQuitarCarrito.Text = T("btnQuitarCarrito", "🗑️ Quitar Ítem");
            lblMetodoPagoVenta.Text = T("lblMetodoPagoVenta", "Medio de Pago:");
            chkEmitirComprobante.Text = T("chkEmitirComprobante", "📄 Emitir Comprobante PDF (QuestPDF)");
            btnConfirmarVentaProd.Text = T("btnConfirmarVentaProd", "💳 Confirmar Venta y Descontar Stock");
            btnFiltrarHistorial.Text = T("btnFiltrarHistorial", "Filtrar Historial");
            btnReimprimirVentaPDF.Text = T("btnReimprimirVentaPDF", "📄 Reimprimir Comprobante PDF");
            lblTituloCarrito.Text = T("lblTituloCarrito", "2. CARRITO DE COMPRA ACTUAL");
            lblTituloHistorial.Text = T("lblTituloHistorial", "3. HISTORIAL RECIENTE DE VENTAS DE MOSTRADOR");
            nudCantidadVenta.Left = Math.Max(96, lblCantidadVenta.Right + 8);
            int nuevoLeftComboPago = Math.Max(268, lblMetodoPagoVenta.Right + 6);
            int anchoDispComboPago = Math.Max(110, (lblTotalCarritoValor.Left - 8) - nuevoLeftComboPago);
            cmbMetodoPagoVenta.Left = nuevoLeftComboPago;
            cmbMetodoPagoVenta.Width = anchoDispComboPago;
            if (_socioActual != null && !string.IsNullOrWhiteSpace(_socioActual.DNI))
            {
                if (_mercadoBLL.ObtenerPerfilAfinidadSocio(_socioActual.DNI, out SocioBE soc, out string of, out string on, out string res))
                {
                    _socioActual = soc;
                    _objetivoFisicoSocio = of;
                    _objetivoNutricionalSocio = on;
                    _resumenSinergiaSocio = res;
                }
            }
            PoblarComboCategorias();
            PoblarComboMediosPago();
            RefrescarBannerSocio();
            AplicarFiltroCatalogo();
            RefrescarGrillaCarrito();
            PoblarGrillaHistorialVentas();
        }

        private void RefrescarBannerSocio()
        {
            if (_socioActual == null)
            {
                lblBannerSinergiaSocio.BackColor = Color.FromArgb(226, 232, 240);
                lblBannerSinergiaSocio.ForeColor = Color.FromArgb(31, 41, 55);
                lblBannerSinergiaSocio.Text = T(
                    "lblBannerDefault",
                    "ℹ️ Ingrese el DNI del socio comprador para consultar su objetivo deportivo/nutricional y recomendar artículos afines.");
            }
            else
            {
                lblBannerSinergiaSocio.BackColor = Color.FromArgb(220, 252, 231);
                lblBannerSinergiaSocio.ForeColor = Color.FromArgb(22, 101, 52);
                lblBannerSinergiaSocio.Text = "✅ " + _resumenSinergiaSocio;
            }
        }

        private void LimpiarFormularioVentaCompleto()
        {
            _sincronizandoSeleccion = true;
            _socioActual = null;
            _objetivoFisicoSocio = string.Empty;
            _objetivoNutricionalSocio = string.Empty;
            _resumenSinergiaSocio = string.Empty;
            txtDNISocio.Clear();
            chkFiltrarAfines.Checked = false;
            if (cmbCategoriaFiltro.Items.Count > 0)
                cmbCategoriaFiltro.SelectedValue = "Todas";
            if (cmbMetodoPagoVenta.Items.Count > 0)
                cmbMetodoPagoVenta.SelectedValue = "Efectivo";
            nudCantidadVenta.Value = 1;
            _carrito.Clear();
            _sincronizandoSeleccion = false;
            RefrescarBannerSocio();
            RefrescarGrillaCarrito();
            CargarCatalogoDesdeBD();
            AplicarFiltroCatalogo();
            txtDNISocio.Focus();
        }

        private void btnVerificarSocio_Click(object sender, EventArgs e)
        {
            try
            {
                bool encontrado = _mercadoBLL.ObtenerPerfilAfinidadSocio(
                    txtDNISocio.Text,
                    out SocioBE socio,
                    out string objFis,
                    out string objNut,
                    out string resumen);
                if (!encontrado || socio == null)
                {
                    _socioActual = null;
                    _objetivoFisicoSocio = string.Empty;
                    _objetivoNutricionalSocio = string.Empty;
                    _resumenSinergiaSocio = string.Empty;
                    RefrescarBannerSocio();
                    MessageBox.Show(
                        T("msgSocioNoEncontrado", "No se encontró ningún socio registrado con el DNI ingresado."),
                        T("titSocioNoEncontrado", "Socio no encontrado"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                if (!socio.EstadoActivo)
                {
                    _socioActual = null;
                    RefrescarBannerSocio();
                    MessageBox.Show(
                        T("msgSocioInactivo", "El socio ingresado se encuentra dado de baja / inactivo en el padrón."),
                        T("titSocioInactivo", "Socio Inactivo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                _socioActual = socio;
                _objetivoFisicoSocio = objFis;
                _objetivoNutricionalSocio = objNut;
                _resumenSinergiaSocio = resumen;
                RefrescarBannerSocio();
                AplicarFiltroCatalogo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkFiltrarAfines_CheckedChanged(object sender, EventArgs e)
        {
            if (_sincronizandoSeleccion) return;
            AplicarFiltroCatalogo();
        }

        private void cmbCategoriaFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_sincronizandoSeleccion) return;
            AplicarFiltroCatalogo();
        }

        private string TraducirCategoriaProducto(string categoriaOriginal)
        {
            if (string.IsNullOrWhiteSpace(categoriaOriginal)) return string.Empty;
            if (categoriaOriginal.Equals("Suplementos Deportivos", StringComparison.OrdinalIgnoreCase))
                return T("catSuplementos", "Suplementos Deportivos");
            if (categoriaOriginal.Equals("Equipamiento y Accesorios", StringComparison.OrdinalIgnoreCase))
                return T("catEquipamiento", "Equipamiento y Accesorios");
            if (categoriaOriginal.Equals("Hidratación y Bebidas", StringComparison.OrdinalIgnoreCase))
                return T("catHidratacion", "Hidratación y Bebidas");
            return categoriaOriginal;
        }

        private string TraducirMetodoPago(string metodoOriginal)
        {
            if (string.IsNullOrWhiteSpace(metodoOriginal)) return string.Empty;
            if (metodoOriginal.Equals("Efectivo", StringComparison.OrdinalIgnoreCase))
                return TRec("metodoEfectivo", "Efectivo");
            if (metodoOriginal.Equals("Transferencia", StringComparison.OrdinalIgnoreCase))
                return TRec("metodoTransferencia", "Transferencia");
            if (metodoOriginal.Equals("Tarjeta de Débito", StringComparison.OrdinalIgnoreCase))
                return TRec("metodoDebito", "Tarjeta de Débito");
            if (metodoOriginal.Equals("Tarjeta de Crédito", StringComparison.OrdinalIgnoreCase))
                return TRec("metodoCredito", "Tarjeta de Crédito");
            return metodoOriginal;
        }

        private void AplicarFiltroCatalogo()
        {
            _sincronizandoSeleccion = true;
            IEnumerable<ProductoBE> query = _catalogoCompleto;
            string catSel = cmbCategoriaFiltro.SelectedValue != null
                ? cmbCategoriaFiltro.SelectedValue.ToString()
                : "Todas";
            if (!string.IsNullOrWhiteSpace(catSel) && catSel != "Todas")
            {
                query = query.Where(p => string.Equals(p.Categoria, catSel, StringComparison.OrdinalIgnoreCase));
            }
            if (chkFiltrarAfines.Checked && _socioActual != null)
            {
                query = query.Where(p => _mercadoBLL.EsProductoAfinAPerfil(p, _objetivoFisicoSocio, _objetivoNutricionalSocio));
            }
            _catalogoFiltrado = query.ToList();
            int codSeleccionadoAntes = (cmbProductoSelect.SelectedItem is ProductoBE prodAnt) ? prodAnt.CodigoProducto : 0;
            cmbProductoSelect.DataSource = null;
            cmbProductoSelect.DisplayMember = "DescripcionCombo";
            cmbProductoSelect.ValueMember = "CodigoProducto";
            cmbProductoSelect.DataSource = _catalogoFiltrado;
            if (codSeleccionadoAntes > 0 && _catalogoFiltrado.Any(x => x.CodigoProducto == codSeleccionadoAntes))
            {
                cmbProductoSelect.SelectedValue = codSeleccionadoAntes;
            }
            else if (_catalogoFiltrado.Count > 0)
            {
                cmbProductoSelect.SelectedIndex = 0;
            }
            dgvStockCatalogo.DataSource = null;
            dgvStockCatalogo.DataSource = _catalogoFiltrado.Select(p => new
            {
                p.CodigoProducto,
                SKU = p.CodigoSKU,
                Nombre = p.Nombre,
                Categoria = TraducirCategoriaProducto(p.Categoria),
                Precio = p.PrecioVenta,
                Stock = p.StockActual,
                Minimo = p.StockMinimo,
                Afinidad = $"{p.ObjetivoFisicoAfin} / {p.ObjetivoNutricionalAfin}"
            }).ToList();
            if (dgvStockCatalogo.Columns["CodigoProducto"] != null)
                dgvStockCatalogo.Columns["CodigoProducto"].Visible = false;
            if (dgvStockCatalogo.Columns["SKU"] != null)
                dgvStockCatalogo.Columns["SKU"].HeaderText = T("colSKU", "SKU");
            if (dgvStockCatalogo.Columns["Nombre"] != null)
                dgvStockCatalogo.Columns["Nombre"].HeaderText = T("colNombre", "Artículo");
            if (dgvStockCatalogo.Columns["Categoria"] != null)
                dgvStockCatalogo.Columns["Categoria"].HeaderText = T("colCategoria", "Categoría");
            if (dgvStockCatalogo.Columns["Precio"] != null)
            {
                dgvStockCatalogo.Columns["Precio"].HeaderText = T("colPrecio", "Precio ($)");
                dgvStockCatalogo.Columns["Precio"].DefaultCellStyle.Format = "N2";
            }
            if (dgvStockCatalogo.Columns["Stock"] != null)
                dgvStockCatalogo.Columns["Stock"].HeaderText = T("colStock", "Stock");
            if (dgvStockCatalogo.Columns["Minimo"] != null)
                dgvStockCatalogo.Columns["Minimo"].HeaderText = T("colMinimo", "Mín.");
            if (dgvStockCatalogo.Columns["Afinidad"] != null)
                dgvStockCatalogo.Columns["Afinidad"].HeaderText = T("colAfinidad", "Afinidad");
            _sincronizandoSeleccion = false;
            ActualizarTarjetaProductoSeleccionado();
        }

        private void dgvStockCatalogo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _catalogoFiltrado.Count) return;
            ProductoBE prod = _catalogoFiltrado[e.RowIndex];
            if (prod.StockActual <= 0)
            {
                dgvStockCatalogo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                dgvStockCatalogo.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(153, 27, 27);
            }
            else if (prod.StockActual <= prod.StockMinimo)
            {
                dgvStockCatalogo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                dgvStockCatalogo.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(146, 64, 14);
            }
        }

        private void dgvStockCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (_sincronizandoSeleccion || dgvStockCatalogo.CurrentRow == null) return;
            if (dgvStockCatalogo.CurrentRow.Cells["CodigoProducto"]?.Value != null)
            {
                int codProd = Convert.ToInt32(dgvStockCatalogo.CurrentRow.Cells["CodigoProducto"].Value);
                _sincronizandoSeleccion = true;
                cmbProductoSelect.SelectedValue = codProd;
                _sincronizandoSeleccion = false;
                ActualizarTarjetaProductoSeleccionado();
            }
        }

        private void cmbProductoSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_sincronizandoSeleccion) return;
            ActualizarTarjetaProductoSeleccionado();
        }

        private void ActualizarTarjetaProductoSeleccionado()
        {
            if (!(cmbProductoSelect.SelectedItem is ProductoBE prod))
            {
                lblInfoProductoSeleccionado.Text = T("lblInfoProdVacio", "Seleccione un artículo del catálogo...");
                return;
            }
            string estadoStock = prod.StockActual <= 0
                ? T("txtSinStock", "🚨 SIN STOCK")
                : (prod.StockActual <= prod.StockMinimo
                    ? string.Format(T("txtStockCritico", "⚠️ STOCK CRÍTICO ({0} u.)"), prod.StockActual)
                    : string.Format(T("txtStockDisponible", "✅ Disponible ({0} u.)"), prod.StockActual));
            string plantillaInfo = T(
                "lblInfoProdDetalle",
                "[{0}] {1}  |  Precio: $ {2}  |  {3}\r\nAfinidad Deportiva/Nutricional: {4} / {5}");
            lblInfoProductoSeleccionado.Text = string.Format(
                plantillaInfo,
                prod.CodigoSKU,
                prod.Nombre,
                prod.PrecioVenta.ToString("N2"),
                estadoStock,
                prod.ObjetivoFisicoAfin,
                prod.ObjetivoNutricionalAfin);
            int enCarrito = _carrito.Where(c => c.CodigoProducto == prod.CodigoProducto).Sum(c => c.Cantidad);
            int maxAgregable = Math.Max(1, prod.StockActual - enCarrito);
            nudCantidadVenta.Maximum = Math.Max(1, maxAgregable);
            if (nudCantidadVenta.Value > nudCantidadVenta.Maximum)
                nudCantidadVenta.Value = nudCantidadVenta.Maximum;
        }

        private void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            if (!(cmbProductoSelect.SelectedItem is ProductoBE prod))
            {
                MessageBox.Show(
                    T("msgSeleccioneProducto", "Seleccione un producto válido del catálogo."),
                    T("titAtencion", "Atención"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            int cantSolicitada = (int)nudCantidadVenta.Value;
            int cantYaEnCarrito = _carrito.Where(x => x.CodigoProducto == prod.CodigoProducto).Sum(x => x.Cantidad);
            if (cantSolicitada + cantYaEnCarrito > prod.StockActual)
            {
                MessageBox.Show(
                    string.Format(
                        T("msgStockInsuficiente", "Stock físico insuficiente para '{0}'.\r\nDisponible en inventario: {1} u. | Ya en carrito: {2} u."),
                        prod.Nombre,
                        prod.StockActual,
                        cantYaEnCarrito),
                    T("titControlInventario", "Control de Inventario"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            var existente = _carrito.FirstOrDefault(x => x.CodigoProducto == prod.CodigoProducto);
            if (existente != null)
            {
                existente.Cantidad += cantSolicitada;
                existente.Subtotal = Math.Round(existente.Cantidad * existente.PrecioUnitario, 2);
            }
            else
            {
                _carrito.Add(new DetalleVentaProductoBE
                {
                    CodigoProducto = prod.CodigoProducto,
                    CodigoSKU = prod.CodigoSKU,
                    NombreProducto = prod.Nombre,
                    Categoria = prod.Categoria,
                    Cantidad = cantSolicitada,
                    PrecioUnitario = prod.PrecioVenta,
                    CostoUnitario = prod.CostoCompra,
                    Subtotal = Math.Round(cantSolicitada * prod.PrecioVenta, 2)
                });
            }
            nudCantidadVenta.Value = 1;
            RefrescarGrillaCarrito();
            ActualizarTarjetaProductoSeleccionado();
        }

        private void btnQuitarCarrito_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow == null || _carrito.Count == 0) return;
            int idx = dgvCarrito.CurrentRow.Index;
            if (idx >= 0 && idx < _carrito.Count)
            {
                _carrito.RemoveAt(idx);
                RefrescarGrillaCarrito();
                ActualizarTarjetaProductoSeleccionado();
            }
        }

        private void RefrescarGrillaCarrito()
        {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = _carrito.Select(c => new
            {
                SKU = c.CodigoSKU,
                Producto = c.NombreProducto,
                c.Cantidad,
                PrecioUnitario = c.PrecioUnitario,
                c.Subtotal
            }).ToList();
            if (dgvCarrito.Columns["SKU"] != null)
                dgvCarrito.Columns["SKU"].HeaderText = T("colSKU", "SKU");
            if (dgvCarrito.Columns["Producto"] != null)
                dgvCarrito.Columns["Producto"].HeaderText = T("colNombre", "Artículo");
            if (dgvCarrito.Columns["Cantidad"] != null)
                dgvCarrito.Columns["Cantidad"].HeaderText = T("colCantidad", "Cant.");
            if (dgvCarrito.Columns["PrecioUnitario"] != null)
            {
                dgvCarrito.Columns["PrecioUnitario"].HeaderText = T("colPrecioUnit", "P. Unit. ($)");
                dgvCarrito.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
            }
            if (dgvCarrito.Columns["Subtotal"] != null)
            {
                dgvCarrito.Columns["Subtotal"].HeaderText = T("colSubtotal", "Subtotal ($)");
                dgvCarrito.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
            }
            decimal total = _carrito.Sum(x => x.Subtotal);
            lblTotalCarritoValor.Text = T("lblTotalPrefijo", "TOTAL: $ ") + total.ToString("N2");
        }

        private bool confirmandoVenta;

        private void btnConfirmarVentaProd_Click(object sender, EventArgs e)
        {
            if (confirmandoVenta) return;
            confirmandoVenta = true;
            btnConfirmarVentaProd.Enabled = false;
            VentaProductoBE ventaConfirmada = null;
            try
            {
                if (_socioActual == null && !string.IsNullOrWhiteSpace(txtDNISocio.Text))
                    btnVerificarSocio_Click(sender, e);
                if (_socioActual == null) throw new Exception("Debe verificar el socio antes de confirmar la venta.");
                if (_carrito.Count == 0) throw new Exception("El carrito de compra está vacío.");
                bool emitir = chkEmitirComprobante.Checked;
                var venta = new VentaProductoBE
                {
                    Fecha = DateTime.Now, Socio = _socioActual,
                    Vendedor = SessionManager.Instancia.UsuarioActual,
                    MetodoPago = cmbMetodoPagoVenta.SelectedValue?.ToString() ?? "Efectivo",
                    Detalles = _carrito.Select(d => new DetalleVentaProductoBE
                    {
                        CodigoProducto = d.CodigoProducto, CodigoSKU = d.CodigoSKU,
                        NombreProducto = d.NombreProducto, Categoria = d.Categoria,
                        Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario,
                        CostoUnitario = d.CostoUnitario, Subtotal = d.Subtotal
                    }).ToList()
                };
                _mercadoBLL.RegistrarVentaMostrador(venta);
                ventaConfirmada = venta;
                // Finalizar primero el estado comercial. Un PDF o una consulta
                // posterior no pueden dejar el mismo carrito listo para cobrar.
                _carrito.Clear();
                _socioActual = null;
                LimpiarFormularioVentaCompleto();
                dtpHistHasta.Value = DateTime.Today;
                CargarHistorialVentas();
                MessageBox.Show($"Venta #{venta.NroVenta:D6} registrada. Total: $ {venta.MontoTotal:N2}.",
                    "Venta registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (emitir)
                {
                    try
                    {
                        using var sfd = new SaveFileDialog
                        {
                            Filter = "Documento PDF (*.pdf)|*.pdf",
                            FileName = $"Venta_UAIFIT_{venta.NroVenta:D6}.pdf"
                        };
                        if (sfd.ShowDialog(this) == DialogResult.OK)
                            PdfCorporativoHelper.GenerarComprobanteVentaProductoPDF(sfd.FileName, venta, true);
                    }
                    catch (Exception ex)
                    {
                        InstaladorBLL.RegistrarLog("Venta confirmada; fallo PDF: " + ex.Message);
                        MessageBox.Show($"La venta #{venta.NroVenta:D6} ya está guardada. No vuelva a cobrarla. Puede reimprimirla desde el historial.\r\n\r\n{ex.Message}",
                            "Comprobante pendiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ventaConfirmada != null)
                    MessageBox.Show($"La venta #{ventaConfirmada.NroVenta:D6} está guardada; falló una tarea posterior. No vuelva a cobrarla.\r\n\r\n{ex.Message}",
                        "Venta confirmada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show(ex.Message, "Venta no registrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { confirmandoVenta = false; btnConfirmarVentaProd.Enabled = true; }
        }

        private void CargarHistorialVentas()
        {
            try
            {
                _historialCargado = _mercadoBLL.ConsultarHistorialVentas(
                    dtpHistDesde.Value,
                    dtpHistHasta.Value,
                    null,
                    "Todos");
                PoblarGrillaHistorialVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PoblarGrillaHistorialVentas()
        {
            if (_historialCargado == null) return;
            dgvHistorialVentas.DataSource = null;
            dgvHistorialVentas.DataSource = _historialCargado.Select(v => new
            {
                v.NroVenta,
                v.Fecha,
                Socio = v.NombreSocio,
                DNI = v.DNISocio,
                Articulos = v.Detalles != null ? string.Join(", ", v.Detalles.Select(d => $"{d.Cantidad}x {d.CodigoSKU}")) : "-",
                Medio = TraducirMetodoPago(v.MetodoPago),
                Total = v.MontoTotal
            }).ToList();
            TraducirColumnasHistorial();
        }

        private void TraducirColumnasHistorial()
        {
            if (dgvHistorialVentas.Columns["NroVenta"] != null)
                dgvHistorialVentas.Columns["NroVenta"].HeaderText = T("colNroVenta", "Op. #");
            if (dgvHistorialVentas.Columns["Fecha"] != null)
                dgvHistorialVentas.Columns["Fecha"].HeaderText = T("colFecha", "Fecha");
            if (dgvHistorialVentas.Columns["Socio"] != null)
                dgvHistorialVentas.Columns["Socio"].HeaderText = T("colSocio", "Socio");
            if (dgvHistorialVentas.Columns["DNI"] != null)
                dgvHistorialVentas.Columns["DNI"].HeaderText = T("colDNI", "DNI");
            if (dgvHistorialVentas.Columns["Articulos"] != null)
                dgvHistorialVentas.Columns["Articulos"].HeaderText = T("colArticulos", "Artículos");
            if (dgvHistorialVentas.Columns["Medio"] != null)
                dgvHistorialVentas.Columns["Medio"].HeaderText = T("colMedio", "Medio Pago");
            if (dgvHistorialVentas.Columns["Total"] != null)
            {
                dgvHistorialVentas.Columns["Total"].HeaderText = T("colTotal", "Total ($)");
                dgvHistorialVentas.Columns["Total"].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnFiltrarHistorial_Click(object sender, EventArgs e)
        {
            CargarHistorialVentas();
        }

        private void btnReimprimirVentaPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHistorialVentas.CurrentRow == null || _historialCargado.Count == 0)
                {
                    MessageBox.Show(
                        T("msgSeleccioneVentaHist", "Seleccione una operación de la grilla de historial para emitir su comprobante en PDF."),
                        T("titAtencion", "Atención"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                int nroVenta = Convert.ToInt32(dgvHistorialVentas.CurrentRow.Cells["NroVenta"].Value);
                VentaProductoBE ventaSel = _historialCargado.FirstOrDefault(v => v.NroVenta == nroVenta);
                if (ventaSel == null) return;
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = T("filtroPDF", "Documento PDF (*.pdf)|*.pdf");
                    sfd.Title = T("titReimprimirPDF", "Reimprimir Comprobante de Venta en PDF");
                    sfd.FileName = $"Venta_UAIFIT_{ventaSel.NroVenta:D6}_DNI_{ventaSel.DNISocio}.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        PdfCorporativoHelper.GenerarComprobanteVentaProductoPDF(sfd.FileName, ventaSel, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmVentasMostrador_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}