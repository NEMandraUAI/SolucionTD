using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmPrediccionReabastecimiento : Form, IObserverIdioma
    {
        private readonly MercadoInternoBLL _mercadoBLL = new MercadoInternoBLL();
        private readonly IdiomaBLL _idiomaBLL = new IdiomaBLL();
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private List<PrediccionProductoBE> _prediccionesActuales = new List<PrediccionProductoBE>();
        private List<OrdenReabastecimientoBE> _ordenesActuales = new List<OrdenReabastecimientoBE>();
        private decimal _facturacionPeriodo = 0m;
        private decimal _costoVentasPeriodo = 0m;
        private decimal _gananciaBrutaPeriodo = 0m;
        private int _totalUnidadesVendidasPeriodo = 0;
        private string _resumenSinergiaGimnasio = string.Empty;
        private bool _cargandoControles = false;

        private string T(string clave, string porDefecto)
        {
            string valor = _traducciones != null && _traducciones.ContainsKey(clave)
                ? _traducciones[clave]
                : porDefecto;
            return (valor ?? string.Empty)
                .Replace("\\r\\n", Environment.NewLine)
                .Replace("\\n", Environment.NewLine);
        }

        public frmPrediccionReabastecimiento()
        {
            InitializeComponent();
            DesactivarMnemonicRecursivo(this.Controls);
            HabilitarDoubleBuffering(pnlGraficoDemanda);
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

        private void HabilitarDoubleBuffering(Panel pnl)
        {
            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                pnl,
                new object[] { true });
        }

        private void frmPrediccionReabastecimiento_Load(object sender, EventArgs e)
        {
            _cargandoControles = true;
            cmbHorizonteDias.Items.Clear();
            cmbHorizonteDias.Items.AddRange(new object[] { "15", "30", "45", "60" });
            cmbHorizonteDias.SelectedIndex = 1;
            _cargandoControles = false;
            ConfigurarColumnasGrillaPrediccion();
            DistribuirTarjetasKpi();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }

        private void frmPrediccionReabastecimiento_Resize(object sender, EventArgs e)
        {
            DistribuirTarjetasKpi();
            pnlGraficoDemanda.Invalidate();
        }

        private void DistribuirTarjetasKpi()
        {
            int margenLateral = 20;
            int espaciado = 12;
            int anchoTotalDisponible = this.ClientSize.Width - (margenLateral * 2) - (espaciado * 3);
            if (anchoTotalDisponible <= 200) return;
            int anchoTarjeta = anchoTotalDisponible / 4;
            pnlKpi1.SetBounds(margenLateral, 12, anchoTarjeta, 74);
            pnlKpi2.SetBounds(margenLateral + anchoTarjeta + espaciado, 12, anchoTarjeta, 74);
            pnlKpi3.SetBounds(margenLateral + (anchoTarjeta + espaciado) * 2, 12, anchoTarjeta, 74);
            pnlKpi4.SetBounds(margenLateral + (anchoTarjeta + espaciado) * 3, 12, anchoTotalDisponible - (anchoTarjeta * 3), 74);
        }

        private void PoblarComboModelosPredictivos()
        {
            bool prevFlag = _cargandoControles;
            _cargandoControles = true;
            string claveActual = cmbMetodoPred.SelectedValue != null
                ? cmbMetodoPred.SelectedValue.ToString()
                : "Promedio Móvil Ponderado (WMA)";
            Dictionary<string, string> modelos = new Dictionary<string, string>
            {
                { "Promedio Móvil Ponderado (WMA)", T("metWMA", "Promedio Móvil Ponderado (WMA)") },
                { "Tendencia Lineal de Holt", T("metHolt", "Tendencia Lineal de Holt") }
            };
            cmbMetodoPred.DataSource = new BindingSource(modelos, null);
            cmbMetodoPred.DisplayMember = "Value";
            cmbMetodoPred.ValueMember = "Key";
            if (modelos.ContainsKey(claveActual))
                cmbMetodoPred.SelectedValue = claveActual;
            _cargandoControles = prevFlag;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            if (idioma == null) return;
            _traducciones = _idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            this.Text = T("titPrediccion", "Inteligencia Comercial, Predicción de Demanda y Reabastecimiento");
            lblKpi1Titulo.Text = T("lblKpi1Titulo", "FACTURACIÓN HISTÓRICA (4 MESES)");
            lblKpi2Titulo.Text = T("lblKpi2Titulo", "UTILIDAD BRUTA Y MARGEN (%)");
            lblKpi3Titulo.Text = T("lblKpi3Titulo", "PRODUCTOS EN RIESGO (STOCK <= ROP)");
            lblKpi4Titulo.Text = T("lblKpi4Titulo", "INVERSIÓN ESTIMADA DE REPOSICIÓN");
            lblMetodoPred.Text = T("lblMetodoPred", "Modelo Predictivo:");
            lblHorizonteDias.Text = T("lblHorizonteDias", "Horizonte (Días):");
            chkIncluirSinergia.Text = T("chkIncluirSinergia", "🔗 Ponderar demanda inducida por Rutinas y Planes activos");
            btnEjecutarPrediccion.Text = T("btnEjecutarPrediccion", "🔄 Restaurar Sugerencias");
            btnNuevoProducto.Text = T("btnNuevoProducto", "➕ Nuevo / Editar Prod.");
            lblTituloOrdenes.Text = T("lblTituloOrdenes", "ÓRDENES DE REABASTECIMIENTO, REPORTE EJECUTIVO PDF Y ARCHIVO SERIALIZADO JSON");
            btnGenerarOrden.Text = T("btnGenerarOrden", "📦 Confirmar Orden de Reabastecimiento");
            btnRecibirOrden.Text = T("btnRecibirOrden", "✅ Marcar Orden Recibida (Sumar Stock)");
            btnExportarPDF.Text = T("btnExportarPDF", "📄 Reporte Ejecutivo PDF");
            btnExportarJSON.Text = T("btnExportarJSON", "💾 Exportar JSON Serializado");
            btnImportarJSON.Text = T("btnImportarJSON", "📂 Importar / Auditar JSON");
            PoblarComboModelosPredictivos();
            TraducirEncabezadosGrillaPrediccion();
            EjecutarProyeccionDemanda(conservarEdicionesManuales: true);
            CargarOrdenesReabastecimiento();
        }

        private void ConfigurarColumnasGrillaPrediccion()
        {
            dgvPredicciones.AutoGenerateColumns = false;
            dgvPredicciones.Columns.Clear();
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CodigoSKU",
                DataPropertyName = "CodigoSKU",
                HeaderText = "SKU",
                ReadOnly = true,
                FillWeight = 7
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Producto",
                DataPropertyName = "Producto",
                HeaderText = "Producto / Artículo",
                ReadOnly = true,
                FillWeight = 14
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HistorialMensual",
                DataPropertyName = "HistorialMensual",
                HeaderText = "Ventas Mensuales (4M)",
                ReadOnly = true,
                FillWeight = 12
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tendencia",
                DataPropertyName = "Tendencia",
                HeaderText = "Tendencia",
                ReadOnly = true,
                FillWeight = 9
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DemandaProyectada",
                DataPropertyName = "DemandaProyectada",
                HeaderText = "Demanda Proy.",
                ReadOnly = true,
                FillWeight = 7,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "0.0", Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ErrorPronosticoMAPE",
                DataPropertyName = "ErrorPronosticoMAPE",
                HeaderText = "Error (MAPE)",
                ReadOnly = true,
                FillWeight = 9
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockMinimo",
                DataPropertyName = "StockMinimo",
                HeaderText = "Stock Mín.",
                ReadOnly = true,
                FillWeight = 5,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockActual",
                DataPropertyName = "StockActual",
                HeaderText = "Stock Disp.",
                ReadOnly = true,
                FillWeight = 5,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockEnTransito",
                DataPropertyName = "StockEnTransito",
                HeaderText = "En Tránsito",
                ReadOnly = true,
                FillWeight = 5,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PuntoReordenROP",
                DataPropertyName = "PuntoReordenROP",
                HeaderText = "ROP",
                ReadOnly = true,
                FillWeight = 4,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NivelRiesgoStock",
                DataPropertyName = "NivelRiesgoStock",
                HeaderText = "Diagnóstico de Stock",
                ReadOnly = true,
                FillWeight = 11
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalculoSugerencia",
                DataPropertyName = "CalculoSugerencia",
                HeaderText = "Cálculo Sugerencia (Fórmula)",
                ReadOnly = true,
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(51, 65, 85) }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CantidadSugerida",
                DataPropertyName = "CantidadSugerida",
                HeaderText = "Sugerido Sist.",
                ReadOnly = true,
                FillWeight = 6,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CantidadAComprarAdmin",
                DataPropertyName = "CantidadAComprarAdmin",
                HeaderText = "✏️ A Comprar (Admin)",
                ReadOnly = false,
                FillWeight = 7,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(239, 246, 255),
                    ForeColor = Color.FromArgb(30, 58, 138),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });
            dgvPredicciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CostoTotalReposicion",
                DataPropertyName = "CostoTotalReposicion",
                HeaderText = "Inversión ($)",
                ReadOnly = true,
                FillWeight = 8,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) }
            });
        }

        private void TraducirEncabezadosGrillaPrediccion()
        {
            if (dgvPredicciones.Columns["CodigoSKU"] != null) dgvPredicciones.Columns["CodigoSKU"].HeaderText = T("colSKU", "SKU");
            if (dgvPredicciones.Columns["Producto"] != null) dgvPredicciones.Columns["Producto"].HeaderText = T("colProducto", "Producto / Artículo");
            if (dgvPredicciones.Columns["HistorialMensual"] != null) dgvPredicciones.Columns["HistorialMensual"].HeaderText = T("colHistorial", "Ventas Mensuales (4M)");
            if (dgvPredicciones.Columns["Tendencia"] != null) dgvPredicciones.Columns["Tendencia"].HeaderText = T("colTendencia", "Tendencia");
            if (dgvPredicciones.Columns["DemandaProyectada"] != null) dgvPredicciones.Columns["DemandaProyectada"].HeaderText = T("colDemandaProy", "Demanda Proy.");
            if (dgvPredicciones.Columns["ErrorPronosticoMAPE"] != null) dgvPredicciones.Columns["ErrorPronosticoMAPE"].HeaderText = T("colErrorMAPE", "Error (MAPE)");
            if (dgvPredicciones.Columns["StockMinimo"] != null) dgvPredicciones.Columns["StockMinimo"].HeaderText = T("colStockMinimo", "Stock Mín.");
            if (dgvPredicciones.Columns["StockActual"] != null) dgvPredicciones.Columns["StockActual"].HeaderText = T("colStockActual", "Stock Disp.");
            if (dgvPredicciones.Columns["StockEnTransito"] != null) dgvPredicciones.Columns["StockEnTransito"].HeaderText = T("colEnTransito", "En Tránsito");
            if (dgvPredicciones.Columns["PuntoReordenROP"] != null) dgvPredicciones.Columns["PuntoReordenROP"].HeaderText = T("colROP", "ROP");
            if (dgvPredicciones.Columns["NivelRiesgoStock"] != null) dgvPredicciones.Columns["NivelRiesgoStock"].HeaderText = T("colRiesgo", "Diagnóstico de Stock");
            if (dgvPredicciones.Columns["CalculoSugerencia"] != null) dgvPredicciones.Columns["CalculoSugerencia"].HeaderText = T("colCalculoSug", "Cálculo Sugerencia (Fórmula)");
            if (dgvPredicciones.Columns["CantidadSugerida"] != null) dgvPredicciones.Columns["CantidadSugerida"].HeaderText = T("colSugerido", "Sugerido Sist.");
            if (dgvPredicciones.Columns["CantidadAComprarAdmin"] != null) dgvPredicciones.Columns["CantidadAComprarAdmin"].HeaderText = T("colAComprar", "✏️ A Comprar (Admin)");
            if (dgvPredicciones.Columns["CostoTotalReposicion"] != null) dgvPredicciones.Columns["CostoTotalReposicion"].HeaderText = T("colInversion", "Inversión ($)");
        }

        private string ObtenerClaveMetodoSeleccionado()
        {
            if (cmbMetodoPred.SelectedValue != null)
                return cmbMetodoPred.SelectedValue.ToString();
            return "Promedio Móvil Ponderado (WMA)";
        }

        private void ParametrosPrediccion_Changed(object sender, EventArgs e)
        {
            if (_cargandoControles) return;
            EjecutarProyeccionDemanda(conservarEdicionesManuales: false);
        }

        private void btnEjecutarPrediccion_Click(object sender, EventArgs e)
        {
            EjecutarProyeccionDemanda(conservarEdicionesManuales: false);
        }

        private void EjecutarProyeccionDemanda(bool conservarEdicionesManuales = false)
        {
            try
            {
                Dictionary<int, int> edicionesPrevias = new Dictionary<int, int>();
                if (conservarEdicionesManuales && _prediccionesActuales != null)
                {
                    foreach (var p in _prediccionesActuales)
                    {
                        edicionesPrevias[p.CodigoProducto] = p.CantidadAComprarAdmin;
                    }
                }
                string metodo = ObtenerClaveMetodoSeleccionado();
                int horizonte = 30;
                if (cmbHorizonteDias.SelectedItem != null)
                    int.TryParse(cmbHorizonteDias.SelectedItem.ToString(), out horizonte);
                bool incluirSinergia = chkIncluirSinergia.Checked;
                _prediccionesActuales = _mercadoBLL.ProyectarDemandaYReabastecimiento(
                    metodo,
                    horizonte,
                    incluirSinergia,
                    out _facturacionPeriodo,
                    out _costoVentasPeriodo,
                    out _gananciaBrutaPeriodo,
                    out _totalUnidadesVendidasPeriodo,
                    out _resumenSinergiaGimnasio);
                if (conservarEdicionesManuales && edicionesPrevias.Count > 0)
                {
                    foreach (var item in _prediccionesActuales)
                    {
                        if (edicionesPrevias.ContainsKey(item.CodigoProducto))
                            item.CantidadAComprarAdmin = edicionesPrevias[item.CodigoProducto];
                    }
                }
                dgvPredicciones.DataSource = null;
                dgvPredicciones.DataSource = _prediccionesActuales;
                string hint = T("lblHintDobleClic", "💡 Doble clic en '✏️ A Comprar (Admin)' para ajustar manualmente la compra.");
                lblResumenSinergiaPoblacion.Text = $"🔗 {_resumenSinergiaGimnasio}   |   {hint}";
                ActualizarTarjetasKpiEnVivo();
                pnlGraficoDemanda.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titErrorMotor", "Error en Motor Predictivo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarTarjetasKpiEnVivo()
        {
            decimal margenProm = _facturacionPeriodo > 0m
                ? Math.Round((_gananciaBrutaPeriodo / _facturacionPeriodo) * 100m, 1)
                : 0m;
            int cantEnRiesgo = _prediccionesActuales.Count(p => p.CodigoSeveridadRiesgo >= 2);
            int cantCriticos = _prediccionesActuales.Count(p => p.CodigoSeveridadRiesgo == 3);
            int totalUnidadesComprar = _prediccionesActuales.Sum(p => p.CantidadAComprarAdmin);
            decimal inversionTotal = Math.Round(_prediccionesActuales.Sum(p => p.CostoTotalReposicion), 2);
            lblKpi1Valor.Text = $"$ {_facturacionPeriodo:N2}";
            lblKpi1Sub.Text = string.Format(T("lblKpi1Sub", "Costo Ventas: $ {0} ({1} u.)"), _costoVentasPeriodo.ToString("N2"), _totalUnidadesVendidasPeriodo);
            lblKpi2Valor.Text = $"$ {_gananciaBrutaPeriodo:N2}";
            lblKpi2Sub.Text = string.Format(T("lblKpi2Sub", "Margen Bruto Promedio: {0}%"), margenProm.ToString("0.0"));
            lblKpi3Valor.Text = string.Format(T("lblKpi3Valor", "{0} de {1} SKUs"), cantEnRiesgo, _prediccionesActuales.Count);
            lblKpi3Sub.Text = string.Format(T("lblKpi3Sub", "{0} en riesgo crítico de ruptura"), cantCriticos);
            lblKpi4Valor.Text = $"$ {inversionTotal:N2}";
            lblKpi4Sub.Text = string.Format(T("lblKpi4Sub", "{0} unidad(es) aprobadas p/ compra"), totalUnidadesComprar);
        }

        private void dgvPredicciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _prediccionesActuales.Count) return;
            var item = _prediccionesActuales[e.RowIndex];
            if (dgvPredicciones.Columns[e.ColumnIndex].Name == "NivelRiesgoStock")
            {
                if (item.CodigoSeveridadRiesgo == 3)
                {
                    e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
                    e.CellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                }
                else if (item.CodigoSeveridadRiesgo == 2)
                {
                    e.CellStyle.BackColor = Color.FromArgb(254, 243, 199);
                    e.CellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.BackColor = Color.FromArgb(220, 252, 231);
                    e.CellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                }
            }
        }

        private void dgvPredicciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _prediccionesActuales.Count) return;
            if (_prediccionesActuales[e.RowIndex].CantidadAComprarAdmin < 0)
            {
                _prediccionesActuales[e.RowIndex].CantidadAComprarAdmin = 0;
            }
            dgvPredicciones.InvalidateRow(e.RowIndex);
            ActualizarTarjetasKpiEnVivo();
        }

        private void dgvPredicciones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            MessageBox.Show(T("msgValorInvalidoCant", "Ingrese un número entero válido mayor o igual a cero para la cantidad a comprar."), T("titValorInvalido", "Valor Inválido"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvPredicciones_SelectionChanged(object sender, EventArgs e)
        {
            pnlGraficoDemanda.Invalidate();
        }

        private void pnlGraficoDemanda_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            PrediccionProductoBE prodSel = null;
            if (dgvPredicciones.CurrentRow != null && dgvPredicciones.CurrentRow.DataBoundItem is PrediccionProductoBE p)
            {
                prodSel = p;
            }
            else if (_prediccionesActuales.Count > 0)
            {
                prodSel = _prediccionesActuales[0];
            }
            if (prodSel == null || prodSel.SerieTemporalGrafico == null || prodSel.SerieTemporalGrafico.Count == 0)
            {
                return;
            }
            lblTituloGrafico.Text = T("lblTituloGraficoBase", "SERIE TEMPORAL Y PROYECCIÓN: ") + $"[{prodSel.CodigoSKU}] {prodSel.Producto.ToUpper()}";
            int w = pnlGraficoDemanda.ClientSize.Width;
            int h = pnlGraficoDemanda.ClientSize.Height;
            if (w < 120 || h < 100) return;
            int padLeft = 44;
            int padRight = 24;
            int padTop = 34;
            int padBottom = 38;
            int chartW = w - padLeft - padRight;
            int chartH = h - padTop - padBottom;
            var puntos = prodSel.SerieTemporalGrafico;
            decimal maxSerie = puntos.Max(x => x.ValorPrimario);
            decimal maxRef = Math.Max(maxSerie, Math.Max(prodSel.StockActual, prodSel.PuntoReordenROP));
            if (maxRef <= 0) maxRef = 10m;
            maxRef = Math.Ceiling(maxRef * 1.20m);
            using (Font fLeg = new Font("Segoe UI", 7.8F, FontStyle.Bold))
            {
                using (Brush bHist = new SolidBrush(Color.FromArgb(37, 99, 235)))
                    g.FillRectangle(bHist, padLeft, 8, 12, 10);
                g.DrawString(T("legVentasHist", "Ventas Históricas"), fLeg, Brushes.DarkSlateGray, padLeft + 15, 6);
                using (Brush bProy = new SolidBrush(Color.FromArgb(109, 40, 217)))
                    g.FillRectangle(bProy, padLeft + 135, 8, 12, 10);
                g.DrawString(T("legDemandaProy", "Demanda Proyectada"), fLeg, Brushes.DarkSlateGray, padLeft + 150, 6);
                using (Pen pRop = new Pen(Color.FromArgb(220, 38, 38), 2f) { DashStyle = DashStyle.Dash })
                    g.DrawLine(pRop, padLeft + 285, 13, padLeft + 305, 13);
                g.DrawString($"ROP ({prodSel.PuntoReordenROP} u.)", fLeg, Brushes.Firebrick, padLeft + 310, 6);
                using (Pen pStk = new Pen(Color.FromArgb(21, 128, 61), 2f) { DashStyle = DashStyle.Dot })
                    g.DrawLine(pStk, padLeft + 395, 13, padLeft + 415, 13);
                g.DrawString($"Stock ({prodSel.StockActual} u.)", fLeg, Brushes.ForestGreen, padLeft + 420, 6);
            }
            using (Pen penGrid = new Pen(Color.FromArgb(226, 232, 240), 1f))
            using (Font fAxis = new Font("Segoe UI", 7.5F))
            using (Brush bAxis = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                int pasosY = 4;
                for (int i = 0; i <= pasosY; i++)
                {
                    float y = padTop + chartH - (chartH * (i / (float)pasosY));
                    g.DrawLine(penGrid, padLeft, y, padLeft + chartW, y);
                    decimal valY = Math.Round(maxRef * (i / (decimal)pasosY), 0);
                    g.DrawString(valY.ToString("0"), fAxis, bAxis, 8, y - 7);
                }
            }
            int n = puntos.Count;
            float slotW = chartW / (float)n;
            float barW = Math.Min(48f, slotW * 0.52f);
            using (Font fVal = new Font("Segoe UI", 8F, FontStyle.Bold))
            using (Font fMes = new Font("Segoe UI", 8F))
            {
                for (int i = 0; i < n; i++)
                {
                    bool esProyeccion = (i == n - 1);
                    float ratio = (float)(puntos[i].ValorPrimario / maxRef);
                    float barH = Math.Max(3f, ratio * chartH);
                    float x = padLeft + (i * slotW) + ((slotW - barW) / 2f);
                    float y = padTop + chartH - barH;
                    Color cBarra = esProyeccion
                        ? Color.FromArgb(109, 40, 217)
                        : Color.FromArgb(37, 99, 235);
                    using (Brush bBarra = new SolidBrush(cBarra))
                    {
                        g.FillRectangle(bBarra, x, y, barW, barH);
                    }
                    string txtVal = esProyeccion ? $"{puntos[i].ValorPrimario:0.0}" : $"{puntos[i].ValorPrimario:0}";
                    SizeF szVal = g.MeasureString(txtVal, fVal);
                    g.DrawString(txtVal, fVal, Brushes.Black, x + (barW - szVal.Width) / 2f, y - 15);
                    SizeF szMes = g.MeasureString(puntos[i].EtiquetaFecha, fMes);
                    g.DrawString(puntos[i].EtiquetaFecha, fMes, Brushes.DarkSlateGray, x + (barW - szMes.Width) / 2f, padTop + chartH + 6);
                }
            }
            float yRop = padTop + chartH - (float)(prodSel.PuntoReordenROP / maxRef) * chartH;
            if (yRop >= padTop && yRop <= padTop + chartH)
            {
                using (Pen penRop = new Pen(Color.FromArgb(220, 38, 38), 1.8f) { DashStyle = DashStyle.Dash })
                {
                    g.DrawLine(penRop, padLeft, yRop, padLeft + chartW, yRop);
                }
            }
            float yStock = padTop + chartH - (float)(prodSel.StockActual / maxRef) * chartH;
            if (yStock >= padTop && yStock <= padTop + chartH)
            {
                using (Pen penStock = new Pen(Color.FromArgb(21, 128, 61), 1.8f) { DashStyle = DashStyle.Dot })
                {
                    g.DrawLine(penStock, padLeft, yStock, padLeft + chartW, yStock);
                }
            }
        }

        private string TraducirEstadoOrden(string estadoOriginal)
        {
            if (string.IsNullOrWhiteSpace(estadoOriginal)) return string.Empty;
            if (estadoOriginal.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                return T("estPendiente", "Pendiente");
            if (estadoOriginal.Equals("Recibida", StringComparison.OrdinalIgnoreCase))
                return T("estRecibida", "Recibida");
            return estadoOriginal;
        }

        private string TraducirModeloOrden(string modeloOriginal)
        {
            if (string.IsNullOrWhiteSpace(modeloOriginal)) return string.Empty;
            if (modeloOriginal.IndexOf("Holt", StringComparison.OrdinalIgnoreCase) >= 0)
                return T("metHolt", "Tendencia Lineal de Holt");
            return T("metWMA", "Promedio Móvil Ponderado (WMA)");
        }

        private void CargarOrdenesReabastecimiento()
        {
            _ordenesActuales = _mercadoBLL.ListarOrdenesReabastecimiento();
            dgvOrdenesReab.DataSource = null;
            dgvOrdenesReab.DataSource = _ordenesActuales.Select(o => new
            {
                o.NroOrden,
                Fecha = o.FechaGeneracion,
                Estado = TraducirEstadoOrden(o.Estado),
                Modelo = TraducirModeloOrden(o.MetodoPrediccion),
                Items = o.Detalles != null ? string.Join(", ", o.Detalles.Select(d => $"{d.CantidadAprobadaAdmin}x {d.CodigoSKU}")) : "-",
                CostoTotal = o.CostoTotalOrden
            }).ToList();
            if (dgvOrdenesReab.Columns["NroOrden"] != null)
                dgvOrdenesReab.Columns["NroOrden"].HeaderText = T("colNroOrden", "Orden #");
            if (dgvOrdenesReab.Columns["Fecha"] != null)
                dgvOrdenesReab.Columns["Fecha"].HeaderText = T("colFechaOrden", "Fecha");
            if (dgvOrdenesReab.Columns["Estado"] != null)
                dgvOrdenesReab.Columns["Estado"].HeaderText = T("colEstadoOrden", "Estado");
            if (dgvOrdenesReab.Columns["Modelo"] != null)
                dgvOrdenesReab.Columns["Modelo"].HeaderText = T("colModeloOrden", "Modelo");
            if (dgvOrdenesReab.Columns["Items"] != null)
                dgvOrdenesReab.Columns["Items"].HeaderText = T("colItemsOrden", "Ítems");
            if (dgvOrdenesReab.Columns["CostoTotal"] != null)
            {
                dgvOrdenesReab.Columns["CostoTotal"].HeaderText = T("colInversion", "Inversión ($)");
                dgvOrdenesReab.Columns["CostoTotal"].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnGenerarOrden_Click(object sender, EventArgs e)
        {
            try
            {
                dgvPredicciones.EndEdit();
                var aComprar = _prediccionesActuales.Where(x => x.CantidadAComprarAdmin > 0).ToList();
                if (aComprar.Count == 0)
                {
                    MessageBox.Show(
                        T("msgSinItemsAComprar", "No hay productos con cantidad a comprar mayor a cero. Ajuste la columna '✏️ A Comprar (Admin)' antes de emitir la orden."),
                        T("titAtencion", "Atención"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                decimal totalOrden = aComprar.Sum(x => x.CostoTotalReposicion);
                DialogResult dr = MessageBox.Show(
                    string.Format(
                        T("msgConfirmarEmisionOrden", "¿Confirma la emisión de una nueva Orden de Reabastecimiento con {0} producto(s) ({1} unidades en total) por una inversión estimada de $ {2}?"),
                        aComprar.Count,
                        aComprar.Sum(x => x.CantidadAComprarAdmin),
                        totalOrden.ToString("N2")),
                    T("titConfirmarOrden", "Confirmar Decisión de Reabastecimiento"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr != DialogResult.Yes) return;
                string metodo = ObtenerClaveMetodoSeleccionado();
                int horizonte = 30;
                if (cmbHorizonteDias.SelectedItem != null)
                    int.TryParse(cmbHorizonteDias.SelectedItem.ToString(), out horizonte);
                OrdenReabastecimientoBE nuevaOrden = _mercadoBLL.GenerarOrdenReabastecimiento(
                    _prediccionesActuales,
                    metodo,
                    horizonte,
                    "Orden generada desde el Tablero Predictivo de Mercado Interno.");
                MessageBox.Show(
                    string.Format(
                        T("msgOrdenEmitidaExito", "Orden de Reabastecimiento #{0} emitida correctamente en estado 'Pendiente'.\r\n\r\nLas unidades solicitadas ya figuran como 'En Tránsito' para evitar pedidos duplicados. Cuando ingrese la mercadería física, seleccione la orden y presione '✅ Marcar Orden Recibida'."),
                        nuevaOrden.NroOrden.ToString("D5")),
                    T("titOrdenEmitida", "Orden Emitida"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                EjecutarProyeccionDemanda();
                CargarOrdenesReabastecimiento();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRecibirOrden_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrdenesReab.CurrentRow == null || _ordenesActuales.Count == 0)
                {
                    MessageBox.Show(
                        T("msgSeleccioneOrdenPend", "Seleccione una Orden de Reabastecimiento en estado 'Pendiente' de la grilla inferior derecha."),
                        T("titAtencion", "Atención"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                int nroOrden = Convert.ToInt32(dgvOrdenesReab.CurrentRow.Cells["NroOrden"].Value);
                OrdenReabastecimientoBE ordSel = _ordenesActuales.FirstOrDefault(o => o.NroOrden == nroOrden);
                if (ordSel == null) return;
                if (!string.Equals(ordSel.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        string.Format(T("msgOrdenYaRecibida", "La Orden #{0} ya se encuentra en estado '{1}'."), nroOrden.ToString("D5"), TraducirEstadoOrden(ordSel.Estado)),
                        T("titAviso", "Aviso"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                DialogResult dr = MessageBox.Show(
                    string.Format(
                        T("msgConfirmarRecepcionOrd", "¿Confirma la recepción física de la Orden #{0}?\r\nSe incrementará automáticamente el Stock Disponible de los artículos recibidos y se recalcularán los Dígitos Verificadores."),
                        nroOrden.ToString("D5")),
                    T("titConfirmarRecepcion", "Confirmar Ingreso de Mercadería"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr != DialogResult.Yes) return;
                _mercadoBLL.RecibirOrdenReabastecimiento(nroOrden);
                MessageBox.Show(
                    string.Format(
                        T("msgOrdenRecibidaExito", "La Orden #{0} fue recepcionada con éxito y las unidades ingresaron al inventario físico disponible."),
                        nroOrden.ToString("D5")),
                    T("titStockActualizado", "Stock Actualizado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                EjecutarProyeccionDemanda();
                CargarOrdenesReabastecimiento();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                dgvPredicciones.EndEdit();
                if (_prediccionesActuales == null || _prediccionesActuales.Count == 0) return;
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = T("filtroPDF", "Documento PDF (*.pdf)|*.pdf");
                    sfd.Title = T("titGuardarPDFPred", "Exportar Reporte Ejecutivo de Predicción y Reabastecimiento (QuestPDF)");
                    sfd.FileName = $"Reporte_Prediccion_Reabastecimiento_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string metodo = ObtenerClaveMetodoSeleccionado();
                        int horizonte = 30;
                        if (cmbHorizonteDias.SelectedItem != null)
                            int.TryParse(cmbHorizonteDias.SelectedItem.ToString(), out horizonte);
                        OrdenReabastecimientoBE ultimaOrden = _ordenesActuales.FirstOrDefault();
                        var paquete = _mercadoBLL.ConstruirPaqueteSerializado(
                            _prediccionesActuales,
                            metodo,
                            horizonte,
                            chkIncluirSinergia.Checked,
                            _facturacionPeriodo,
                            _costoVentasPeriodo,
                            _gananciaBrutaPeriodo,
                            _resumenSinergiaGimnasio,
                            ultimaOrden?.NroOrden);
                        PdfCorporativoHelper.GenerarReportePrediccionYReabastecimientoPDF(
                            sfd.FileName,
                            _prediccionesActuales,
                            TraducirModeloOrden(metodo),
                            horizonte,
                            chkIncluirSinergia.Checked,
                            _facturacionPeriodo,
                            _costoVentasPeriodo,
                            _gananciaBrutaPeriodo,
                            _resumenSinergiaGimnasio,
                            ultimaOrden,
                            paquete.HashIntegridadSHA256,
                            true);
                        MessageBox.Show(
                            T("msgPDFPredExito", "Reporte Ejecutivo PDF generado exitosamente mediante QuestPDF."),
                            T("titPDFPredExito", "Exportación PDF Exitosa"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarJSON_Click(object sender, EventArgs e)
        {
            try
            {
                dgvPredicciones.EndEdit();
                if (_prediccionesActuales == null || _prediccionesActuales.Count == 0) return;
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = T("filtroJSON", "Archivo Serializado JSON (*.json)|*.json");
                    sfd.Title = T("titGuardarJSON", "Exportar Planificación Comercial Serializada en JSON");
                    sfd.FileName = $"PlanReabastecimiento_UAIFIT_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string metodo = ObtenerClaveMetodoSeleccionado();
                        int horizonte = 30;
                        if (cmbHorizonteDias.SelectedItem != null)
                            int.TryParse(cmbHorizonteDias.SelectedItem.ToString(), out horizonte);
                        OrdenReabastecimientoBE ultimaOrden = _ordenesActuales.FirstOrDefault();
                        var paquete = _mercadoBLL.ConstruirPaqueteSerializado(
                            _prediccionesActuales,
                            metodo,
                            horizonte,
                            chkIncluirSinergia.Checked,
                            _facturacionPeriodo,
                            _costoVentasPeriodo,
                            _gananciaBrutaPeriodo,
                            _resumenSinergiaGimnasio,
                            ultimaOrden?.NroOrden);
                        _mercadoBLL.ExportarPlanReabastecimientoJson(sfd.FileName, paquete);
                        MessageBox.Show(
                            string.Format(
                                T("msgJSONExportExito", "Archivo serializado JSON generado y firmado criptográficamente con éxito.\r\n\r\nRuta: {0}\r\nSello SHA-256: {1}"),
                                sfd.FileName,
                                paquete.HashIntegridadSHA256),
                            T("titJSONExportExito", "Serialización JSON Exitosa"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportarJSON_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = T("filtroJSON", "Archivo Serializado JSON (*.json)|*.json");
                    ofd.Title = T("titAbrirJSON", "Importar y Auditar Archivo Serializado JSON de Reabastecimiento");
                    if (ofd.ShowDialog() != DialogResult.OK) return;
                    var paquete = _mercadoBLL.ImportarYVerificarPlanReabastecimientoJson(
                        ofd.FileName,
                        out bool estructuraValida,
                        out bool integridadValida);
                    if (!estructuraValida || !integridadValida)
                    {
                        string motivoCorrupcion = !estructuraValida
                            ? T("msgCorrupcionSintaxis", "La estructura sintáctica del archivo JSON está dañada, incompleta o truncada.")
                            : T("msgCorrupcionHash", "El contenido del archivo fue alterado manualmente por fuera del sistema (el sello criptográfico SHA-256 no coincide con las cantidades, costos o proyecciones del documento).");
                        DialogResult recuperar = MessageBox.Show(
                            string.Format(
                                T("msgAsistenteRepararJSON", "🚨 CORRUPCIÓN DETECTADA EN EL ARCHIVO '{0}'\r\n\r\nDiagnóstico: {1}\r\n\r\n🛠️ SOLUCIÓN DE RECUPERACIÓN DISPONIBLE:\r\n¿Desea que el sistema SANEE los datos contra el catálogo oficial de la Base de Datos, recalcule la estructura y RESELLE criptográficamente (SHA-256) el archivo ahora mismo (SÍ)?\r\n\r\nPresione NO para cancelar y descartar la importación."),
                                Path.GetFileName(ofd.FileName),
                                motivoCorrupcion),
                            T("titAsistenteRepararJSON", "Asistente de Reparación de Archivo Serializado JSON"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                        if (recuperar != DialogResult.Yes) return;
                        string metodo = ObtenerClaveMetodoSeleccionado();
                        int horizonte = 30;
                        if (cmbHorizonteDias.SelectedItem != null)
                            int.TryParse(cmbHorizonteDias.SelectedItem.ToString(), out horizonte);
                        paquete = _mercadoBLL.RepararYResellarArchivoJsonCorrupto(
                            ofd.FileName,
                            paquete,
                            _prediccionesActuales,
                            metodo,
                            horizonte,
                            chkIncluirSinergia.Checked,
                            _facturacionPeriodo,
                            _costoVentasPeriodo,
                            _gananciaBrutaPeriodo,
                            _resumenSinergiaGimnasio,
                            out string detalleReparacion);
                        MessageBox.Show(
                            string.Format(
                                T("msgJSONReparadoExito", "✅ ARCHIVO JSON REPARADO Y RESELLADO CON ÉXITO\r\n\r\n• Acción ejecutada: {0}\r\n• Nuevo Sello SHA-256: {1}\r\n\r\nA continuación se cargarán los valores saneados en la grilla."),
                                detalleReparacion,
                                paquete.HashIntegridadSHA256),
                            T("titRecuperacionExito", "Recuperación Exitosa"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            string.Format(
                                T("msgJSONAuditoriaExito", "✅ AUDITORÍA EXITOSA — ARCHIVO SERIALIZADO ÍNTEGRO Y VERIFICADO\r\n\r\n• Fecha de Generación: {0}\r\n• Administrador Autor: {1}\r\n• Modelo Predictivo: {2} (Horizonte: {3} días)\r\n• Productos Evaluados: {4} SKUs\r\n• Unidades Aprobadas: {5} u. ($ {6})\r\n• Sello SHA-256: {7}\r\n\r\nLas cantidades decididas en el archivo han sido cargadas en la grilla actual."),
                                paquete.FechaGeneracion.ToString("dd/MM/yyyy HH:mm:ss"),
                                paquete.UsuarioAdministrador,
                                TraducirModeloOrden(paquete.MetodoPrediccionUtilizado),
                                paquete.HorizontePlanificacionDias,
                                paquete.ProductosEvaluados.Count,
                                paquete.UnidadesTotalesReabastecer,
                                paquete.InversionTotalEstimadaReabastecimiento.ToString("N2"),
                                paquete.HashIntegridadSHA256),
                            T("titAuditoriaJSON", "Verificación de Archivo Serializado JSON"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    foreach (var itemJson in paquete.ProductosEvaluados)
                    {
                        var fila = _prediccionesActuales.FirstOrDefault(x => x.CodigoProducto == itemJson.CodigoProducto || x.CodigoSKU == itemJson.CodigoSKU);
                        if (fila != null)
                        {
                            fila.CantidadAComprarAdmin = itemJson.CantidadDecididaPorAdministrador;
                        }
                    }
                    dgvPredicciones.Refresh();
                    ActualizarTarjetasKpiEnVivo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNuevoProducto_Click(object sender, EventArgs e)
        {
            ProductoBE prodBase = null;
            if (dgvPredicciones.CurrentRow != null && dgvPredicciones.CurrentRow.DataBoundItem is PrediccionProductoBE filaSel)
            {
                DialogResult modo = MessageBox.Show(
                    string.Format(
                        T("msgModoCatalogoProd", "¿Desea registrar un NUEVO producto en el catálogo (SÍ) o EDITAR el producto seleccionado '{0}' (NO)?\r\n\r\nPresione Cancelar para salir."),
                        filaSel.Producto),
                    T("titGestionCatalogo", "Gestión de Catálogo de Productos"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (modo == DialogResult.Cancel) return;
                if (modo == DialogResult.No)
                {
                    prodBase = _mercadoBLL.ListarTodoElCatalogo().FirstOrDefault(p => p.CodigoProducto == filaSel.CodigoProducto);
                }
            }
            MostrarDialogoABMProducto(prodBase);
        }

        private void MostrarDialogoABMProducto(ProductoBE existente)
        {
            bool esEdicion = existente != null && existente.CodigoProducto > 0;
            int? stockOriginal = esEdicion ? existente.StockActual : null;
            using (Form dlg = new Form())
            {
                dlg.Text = esEdicion
                    ? string.Format(T("dlgTitEditarProd", "Editar Producto [{0}]"), existente.CodigoSKU)
                    : T("dlgTitNuevoProd", "Nuevo Producto del Mercado Interno");
                dlg.Size = new Size(460, 480);
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.White;
                dlg.Font = new Font("Segoe UI", 9F);
                Label Lbl(string txt, int top) => new Label { Text = txt, Left = 20, Top = top, Width = 175, Font = new Font("Segoe UI", 8.8F, FontStyle.Bold), UseMnemonic = false };
                TextBox txtSku = new TextBox { Left = 200, Top = 18, Width = 220, Text = esEdicion ? existente.CodigoSKU : "SUP-NUEVO-01" };
                TextBox txtNom = new TextBox { Left = 200, Top = 54, Width = 220, Text = esEdicion ? existente.Nombre : "" };
                ComboBox cmbCat = new ComboBox { Left = 200, Top = 90, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbCat.Items.AddRange(_mercadoBLL.ObtenerCategoriasProductos().ToArray());
                cmbCat.SelectedIndex = esEdicion && cmbCat.Items.Contains(existente.Categoria) ? cmbCat.Items.IndexOf(existente.Categoria) : 0;
                NumericUpDown nudPrecio = new NumericUpDown { Left = 200, Top = 126, Width = 220, Maximum = 9999999, DecimalPlaces = 2, Value = esEdicion ? existente.PrecioVenta : 15000 };
                NumericUpDown nudCosto = new NumericUpDown { Left = 200, Top = 162, Width = 220, Maximum = 9999999, DecimalPlaces = 2, Value = esEdicion ? existente.CostoCompra : 9000 };
                NumericUpDown nudStock = new NumericUpDown { Left = 200, Top = 198, Width = 220, Maximum = int.MaxValue, Value = esEdicion ? existente.StockActual : 10 };
                NumericUpDown nudMin = new NumericUpDown { Left = 200, Top = 234, Width = 220, Maximum = 10000, Value = esEdicion ? existente.StockMinimo : 5 };
                NumericUpDown nudLead = new NumericUpDown { Left = 200, Top = 270, Width = 220, Minimum = 1, Maximum = 90, Value = esEdicion ? existente.TiempoReposicionDias : 5 };
                ComboBox cmbAfinFis = new ComboBox { Left = 200, Top = 306, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbAfinFis.Items.AddRange(new object[] { "Todos", "Hipertrofia", "Fuerza", "Pérdida de Peso", "Resistencia / Tonificación" });
                cmbAfinFis.SelectedIndex = esEdicion && cmbAfinFis.Items.Contains(existente.ObjetivoFisicoAfin) ? cmbAfinFis.Items.IndexOf(existente.ObjetivoFisicoAfin) : 0;
                ComboBox cmbAfinNut = new ComboBox { Left = 200, Top = 342, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbAfinNut.Items.AddRange(new object[] { "Todos", "Superávit Calórico", "Déficit Calórico", "Mantenimiento" });
                cmbAfinNut.SelectedIndex = esEdicion && cmbAfinNut.Items.Contains(existente.ObjetivoNutricionalAfin) ? cmbAfinNut.Items.IndexOf(existente.ObjetivoNutricionalAfin) : 0;
                Button btnGuardar = new Button
                {
                    Text = T("dlgBtnGuardar", "💾 Guardar Producto"),
                    Left = 200,
                    Top = 388,
                    Width = 220,
                    Height = 36,
                    BackColor = Color.FromArgb(37, 99, 235),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    UseMnemonic = false
                };
                btnGuardar.FlatAppearance.BorderSize = 0;
                btnGuardar.Click += (s, args) =>
                {
                    try
                    {
                        ProductoBE p = new ProductoBE { CodigoProducto = esEdicion ? existente.CodigoProducto : 0 };
                        p.CodigoSKU = txtSku.Text;
                        p.Nombre = txtNom.Text;
                        p.Categoria = cmbCat.SelectedItem.ToString();
                        p.PrecioVenta = nudPrecio.Value;
                        p.CostoCompra = nudCosto.Value;
                        p.StockActual = (int)nudStock.Value;
                        p.StockMinimo = (int)nudMin.Value;
                        p.TiempoReposicionDias = (int)nudLead.Value;
                        p.ObjetivoFisicoAfin = cmbAfinFis.SelectedItem.ToString();
                        p.ObjetivoNutricionalAfin = cmbAfinNut.SelectedItem.ToString();
                        p.Activo = true;
                        _mercadoBLL.GuardarProducto(p, stockOriginal);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(T(ex.Message, ex.Message), T("titValidacionProd", "Validación de Producto"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                dlg.Controls.AddRange(new Control[]
                {
                    Lbl(T("dlgLblSKU", "Código SKU:"), 20), txtSku,
                    Lbl(T("dlgLblNombre", "Nombre del Artículo:"), 56), txtNom,
                    Lbl(T("dlgLblCategoria", "Categoría:"), 92), cmbCat,
                    Lbl(T("dlgLblPrecio", "Precio de Venta ($):"), 128), nudPrecio,
                    Lbl(T("dlgLblCosto", "Costo de Compra ($):"), 164), nudCosto,
                    Lbl(T("dlgLblStock", "Stock Físico Actual:"), 200), nudStock,
                    Lbl(T("dlgLblMinimo", "Stock Mínimo (Seguridad):"), 236), nudMin,
                    Lbl(T("dlgLblLeadTime", "Lead Time Proveedor (Días):"), 272), nudLead,
                    Lbl(T("dlgLblAfinFis", "Objetivo Físico Afín:"), 308), cmbAfinFis,
                    Lbl(T("dlgLblAfinNut", "Objetivo Nutricional Afín:"), 344), cmbAfinNut,
                    btnGuardar
                });
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    EjecutarProyeccionDemanda();
                }
            }
        }

        private void frmPrediccionReabastecimiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}