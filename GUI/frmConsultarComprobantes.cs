using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmConsultarComprobantes : Form, IObserverIdioma
    {
        ComprobantePagoBLL comprobanteBLL = new ComprobantePagoBLL();
        SocioBLL socioBLL = new SocioBLL();
        PlanSuscripcionBLL planBLL = new PlanSuscripcionBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private Dictionary<string, string> _traduccionesRecepcion = new Dictionary<string, string>();
        private List<ComprobantePagoBE> _comprobantesFiltrados = new List<ComprobantePagoBE>();
        private bool _cargandoComboMetodos = false;
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }
        private string TRec(string clave, string textoPorDefecto)
        {
            return _traduccionesRecepcion != null && _traduccionesRecepcion.ContainsKey(clave) ? _traduccionesRecepcion[clave] : textoPorDefecto;
        }

        public frmConsultarComprobantes()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                pnlGraficoMedios,
                new object[] { true });
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmConsultarComprobantes_Load(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            EjecutarFiltro();
        }

        private void PoblarComboMetodoFiltro()
        {
            _cargandoComboMetodos = true;
            string seleccionActual = cmbMetodoFiltro.SelectedValue != null ? cmbMetodoFiltro.SelectedValue.ToString() : "Todos";
            Dictionary<string, string> metodos = new Dictionary<string, string>
            {
                { "Todos", T("cmbTodosMetodos", "Todos") },
                { "Efectivo", TRec("metodoEfectivo", "Efectivo") },
                { "Transferencia", TRec("metodoTransferencia", "Transferencia") },
                { "Tarjeta de Débito", TRec("metodoDebito", "Tarjeta de Débito") },
                { "Tarjeta de Crédito", TRec("metodoCredito", "Tarjeta de Crédito") }
            };
            cmbMetodoFiltro.DataSource = new BindingSource(metodos, null);
            cmbMetodoFiltro.DisplayMember = "Value";
            cmbMetodoFiltro.ValueMember = "Key";
            if (metodos.ContainsKey(seleccionActual))
            {
                cmbMetodoFiltro.SelectedValue = seleccionActual;
            }
            _cargandoComboMetodos = false;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            _traduccionesRecepcion = idiomaBLL.ObtenerTraducciones(idioma, "frmRecepcion");
            lblDesde.Text = T("lblDesde", "Desde:");
            lblHasta.Text = T("lblHasta", "Hasta:");
            lblDNI.Text = T("lblDNI", "DNI Socio (Opcional):");
            lblMetodoFiltro.Text = T("lblMetodoFiltro", "Método de Pago:");
            btnFiltrar.Text = T("btnFiltrar", "Aplicar Filtros");
            btnLimpiar.Text = T("btnLimpiar", "Limpiar");
            btnCajaHoy.Text = T("btnCajaHoy", "📅 Caja de Hoy");
            btnReimprimirRecibo.Text = T("btnReimprimirRecibo", "📄 Emitir Recibo PDF");
            btnExportarArqueo.Text = T("btnExportarArqueo", "📊 Exportar Arqueo PDF");
            lblKpiTotalTitulo.Text = T("kpiTotalTit", "RECAUDACIÓN TOTAL FILTRADA");
            lblKpiEfectivoTitulo.Text = T("kpiEfectivoTit", "ARQUEO EN EFECTIVO (CAJA FÍSICA)");
            lblKpiDigitalTitulo.Text = T("kpiDigitalTit", "INGRESOS DIGITALES / TARJETAS");
            lblKpiPromedioTitulo.Text = T("kpiPromedioTit", "TICKET PROMEDIO POR VENTA");
            this.Text = T("titConsultarComprobantes", "Consulta de Comprobantes");
            PoblarComboMetodoFiltro();
            AplicarEncabezadosGrilla();
            ActualizarKpisYVisualizaciones();
        }

        private void AplicarEncabezadosGrilla()
        {
            if (dgvComprobantes.Columns["NroComprobante"] != null) dgvComprobantes.Columns["NroComprobante"].HeaderText = T("colNro", "Nro.");
            if (dgvComprobantes.Columns["Fecha"] != null) dgvComprobantes.Columns["Fecha"].HeaderText = T("colFecha", "Fecha");
            if (dgvComprobantes.Columns["MontoTotal"] != null)
            {
                dgvComprobantes.Columns["MontoTotal"].HeaderText = T("colMonto", "Total ($)");
                dgvComprobantes.Columns["MontoTotal"].DefaultCellStyle.Format = "N2";
            }
            if (dgvComprobantes.Columns["MetodoPago"] != null) dgvComprobantes.Columns["MetodoPago"].HeaderText = T("colMetodo", "Método");
            if (dgvComprobantes.Columns["DNI_Socio"] != null) dgvComprobantes.Columns["DNI_Socio"].HeaderText = T("colDNISocio", "DNI Socio");
            if (dgvComprobantes.Columns["Nombre_Socio"] != null) dgvComprobantes.Columns["Nombre_Socio"].HeaderText = T("colNombreSocio", "Socio");
            if (dgvComprobantes.Columns["Cobrador"] != null) dgvComprobantes.Columns["Cobrador"].HeaderText = T("colCobrador", "Cobrador");
        }

        private void EjecutarFiltro()
        {
            try
            {
                if (dtpDesde.Value.Date > dtpHasta.Value.Date)
                {
                    throw new Exception(T("msgFechasInvalidas", "La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'."));
                }
                var listaBase = comprobanteBLL.ConsultarComprobantesFiltros(dtpDesde.Value, dtpHasta.Value, txtDNI.Text.Trim());
                string metodoClave = cmbMetodoFiltro.SelectedValue != null ? cmbMetodoFiltro.SelectedValue.ToString() : "Todos";
                if (!string.IsNullOrWhiteSpace(metodoClave) && metodoClave != "Todos")
                {
                    listaBase = listaBase
                        .Where(c => !string.IsNullOrWhiteSpace(c.MetodoPago) &&
                                    c.MetodoPago.Equals(metodoClave, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                _comprobantesFiltrados = listaBase;
                dgvComprobantes.DataSource = null;
                dgvComprobantes.DataSource = _comprobantesFiltrados.Select(c => new
                {
                    c.NroComprobante,
                    c.Fecha,
                    c.MontoTotal,
                    c.MetodoPago,
                    DNI_Socio = c.Socio != null ? c.Socio.DNI : "-",
                    Nombre_Socio = c.Socio != null ? $"{c.Socio.Nombre} {c.Socio.Apellido}" : "-",
                    Cobrador = c.EmpleadoCobrador != null ? c.EmpleadoCobrador.Nombre : "-"
                }).ToList();
                AplicarEncabezadosGrilla();
                ActualizarKpisYVisualizaciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarKpisYVisualizaciones()
        {
            if (_comprobantesFiltrados == null) _comprobantesFiltrados = new List<ComprobantePagoBE>();
            int cant = _comprobantesFiltrados.Count;
            decimal total = _comprobantesFiltrados.Sum(c => c.MontoTotal);
            decimal efectivo = _comprobantesFiltrados
                .Where(c => !string.IsNullOrWhiteSpace(c.MetodoPago) &&
                            c.MetodoPago.IndexOf("Efectivo", StringComparison.OrdinalIgnoreCase) >= 0)
                .Sum(c => c.MontoTotal);
            decimal digital = total - efectivo;
            decimal promedio = cant > 0 ? Math.Round(total / cant, 2) : 0m;
            decimal pctEfec = total > 0 ? Math.Round(efectivo * 100m / total, 1) : 0m;
            decimal pctDig = total > 0 ? Math.Round(digital * 100m / total, 1) : 0m;
            string sufijoOps = T("kpiOperacionesSufijo", " operación(es)");
            string sufijoPct = T("kpiDelTotalSufijo", "% del total");
            lblKpiTotalValor.Text = $"$ {total:N2}";
            lblKpiTotalSub.Text = $"{cant}{sufijoOps}";
            lblKpiEfectivoValor.Text = $"$ {efectivo:N2}";
            lblKpiEfectivoSub.Text = $"{pctEfec:0.#}{sufijoPct}";
            lblKpiDigitalValor.Text = $"$ {digital:N2}";
            lblKpiDigitalSub.Text = $"{pctDig:0.#}{sufijoPct}";
            lblKpiPromedioValor.Text = $"$ {promedio:N2}";
            lblKpiPromedioSub.Text = $"{cant}{sufijoOps}";
            pnlGraficoMedios.Invalidate();
        }

        private void pnlGraficoMedios_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(248, 250, 252));
            using (Font fontTitulo = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (Font fontItem = new Font("Segoe UI", 8F, FontStyle.Bold))
            using (Font fontDetalle = new Font("Segoe UI", 7.8F, FontStyle.Regular))
            using (SolidBrush brushTexto = new SolidBrush(Color.FromArgb(31, 41, 55)))
            using (SolidBrush brushGris = new SolidBrush(Color.FromArgb(100, 116, 139)))
            using (SolidBrush brushFondoBarra = new SolidBrush(Color.FromArgb(226, 232, 240)))
            {
                g.DrawString(T("titGraficoMedios", "Distribución por Medio de Pago"), fontTitulo, brushTexto, 10, 8);
                if (_comprobantesFiltrados == null || _comprobantesFiltrados.Count == 0)
                {
                    RectangleF rectVacio = new RectangleF(10, 60, pnlGraficoMedios.Width - 20, 80);
                    g.DrawString(T("msgSinComprobantesGrafico", "No se registran comprobantes en el rango seleccionado."), fontDetalle, brushGris, rectVacio);
                    return;
                }
                decimal totalGeneral = Math.Max(1m, _comprobantesFiltrados.Sum(x => x.MontoTotal));
                var grupos = _comprobantesFiltrados
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.MetodoPago) ? "Otro" : x.MetodoPago)
                    .Select(gr => new
                    {
                        Metodo = gr.Key,
                        Monto = gr.Sum(x => x.MontoTotal),
                        Cantidad = gr.Count()
                    })
                    .OrderByDescending(gr => gr.Monto)
                    .ToList();
                Color[] paleta = new Color[]
                {
                    Color.FromArgb(37, 99, 235),
                    Color.FromArgb(16, 185, 129),
                    Color.FromArgb(124, 58, 237),
                    Color.FromArgb(245, 158, 11)
                };
                int y = 36;
                int anchoMaxBarra = pnlGraficoMedios.Width - 24;
                for (int i = 0; i < grupos.Count && i < 4; i++)
                {
                    var item = grupos[i];
                    decimal pct = Math.Round(item.Monto * 100m / totalGeneral, 1);
                    int anchoBarra = Math.Max(6, (int)Math.Round((double)(item.Monto / totalGeneral) * anchoMaxBarra));
                    g.DrawString($"{item.Metodo} ({item.Cantidad})", fontItem, brushTexto, 12, y);
                    g.DrawString($"$ {item.Monto:N0} ({pct:0.#}%)", fontDetalle, brushGris, 12, y + 15);
                    g.FillRectangle(brushFondoBarra, 12, y + 32, anchoMaxBarra, 10);
                    using (SolidBrush brushBarra = new SolidBrush(paleta[i % paleta.Length]))
                    {
                        g.FillRectangle(brushBarra, 12, y + 32, anchoBarra, 10);
                    }
                    y += 48;
                }
            }
        }

        private void cmbMetodoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoComboMetodos) return;
            EjecutarFiltro();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            EjecutarFiltro();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;
            txtDNI.Clear();
            _cargandoComboMetodos = true;
            if (cmbMetodoFiltro.Items.Count > 0) cmbMetodoFiltro.SelectedIndex = 0;
            _cargandoComboMetodos = false;
            EjecutarFiltro();
        }

        private void btnCajaHoy_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today;
            dtpHasta.Value = DateTime.Today;
            EjecutarFiltro();
        }

        private void btnReimprimirRecibo_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvComprobantes.CurrentRow == null || _comprobantesFiltrados == null || _comprobantesFiltrados.Count == 0)
                {
                    MessageBox.Show(T("msgSeleccioneComprobante", "Seleccione un comprobante de la grilla para emitir su recibo oficial en PDF."), T("titError", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int nroComp = Convert.ToInt32(dgvComprobantes.CurrentRow.Cells["NroComprobante"].Value);
                ComprobantePagoBE comp = _comprobantesFiltrados.FirstOrDefault(c => c.NroComprobante == nroComp);
                if (comp == null) return;
                SocioBE socioCompleto = comp.Socio != null && !string.IsNullOrWhiteSpace(comp.Socio.DNI)
                    ? (socioBLL.ConsultarSocio(comp.Socio.DNI) ?? comp.Socio)
                    : comp.Socio;
                PlanSuscripcionBE planAsociado = new PlanSuscripcionBE
                {
                    CodigoPlan = comp.CodigoPlanVendido ?? 0,
                    Nombre = comp.NombrePlanVendido ?? "Plan histórico no disponible",
                    Precio = comp.MontoTotal,
                    DuracionDias = comp.DuracionDiasVendida ?? 0
                };
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                    sfd.Title = "Emitir Recibo Oficial en PDF";
                    sfd.FileName = $"Recibo_UAIFIT_{comp.NroComprobante:D6}_DNI_{(socioCompleto != null ? socioCompleto.DNI : "Socio")}.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string cobrador = comp.EmpleadoCobrador != null && !string.IsNullOrWhiteSpace(comp.EmpleadoCobrador.Nombre)
                            ? comp.EmpleadoCobrador.Nombre
                            : "Recepcion";
                        PdfCorporativoHelper.GenerarReciboSuscripcionPDF(
                            sfd.FileName,
                            comp,
                            socioCompleto,
                            planAsociado,
                            cobrador,
                            true);
                        MessageBox.Show(T("msgReciboPDFExito", "Recibo oficial en PDF generado exitosamente."), T("titExito", "Exportación Exitosa"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarArqueo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_comprobantesFiltrados == null || _comprobantesFiltrados.Count == 0)
                {
                    MessageBox.Show(T("msgSinDatosArqueo", "No hay comprobantes en la vista actual para exportar el arqueo de caja."), T("titError", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                    sfd.Title = "Exportar Reporte de Arqueo de Caja en PDF";
                    sfd.FileName = $"Arqueo_Caja_UAIFIT_{dtpDesde.Value:yyyyMMdd}_al_{dtpHasta.Value:yyyyMMdd}.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string auditor = SessionManager.Instancia.UsuarioActual != null
                            ? SessionManager.Instancia.UsuarioActual.Nombre
                            : "Auditor";
                        string metodoSel = cmbMetodoFiltro.SelectedValue != null ? cmbMetodoFiltro.SelectedValue.ToString() : "Todos";
                        PdfCorporativoHelper.GenerarReporteArqueoCajaPDF(
                            sfd.FileName,
                            dtpDesde.Value.Date,
                            dtpHasta.Value.Date,
                            txtDNI.Text.Trim(),
                            metodoSel,
                            _comprobantesFiltrados,
                            auditor,
                            true);
                        MessageBox.Show(T("msgArqueoPDFExito", "Reporte de Arqueo de Caja en PDF generado exitosamente."), T("titExito", "Exportación Exitosa"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmConsultarComprobantes_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}