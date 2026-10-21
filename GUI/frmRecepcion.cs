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
    public partial class frmRecepcion : Form, IObserverIdioma
    {
        SocioBLL socioBLL = new SocioBLL();
        PlanSuscripcionBLL planBLL = new PlanSuscripcionBLL();
        ComprobantePagoBLL comprobanteBLL = new ComprobantePagoBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();
        SocioBE socioActual = null;
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private string _estadoBannerModo = "INICIAL";
        private int _cantPagosHistoricos = 0;
        private string _fechaUltimoPagoTexto = "-";
        private string _metodoUltimoPagoClave = "-";
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }

        public frmRecepcion()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
            this.FormClosing += frmRecepcion_FormClosing;
            this.Load += frmRecepcion_Load;
        }

        private void frmRecepcion_Load(object sender, EventArgs e)
        {
            CargarPlanes();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            ActualizarSimuladorComercial();
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            btnBuscar.Text = T("btnBuscar", "Buscar / Verificar");
            btnLimpiarCampos.Text = T("btnLimpiarCampos", "Limpiar");
            btnConfirmarVenta.Text = T("btnConfirmarVenta", "Procesar Transacción y Dar Acceso");
            btnConsultarPlanes.Text = T("btnConsultarPlanes", "Ver Detalle de Planes");
            lblDNI.Text = T("lblDNI", "DNI del Cliente:");
            lblNombre.Text = T("lblNombre", "Nombre:");
            lblApellido.Text = T("lblApellido", "Apellido:");
            lblTelefono.Text = T("lblTelefono", "Teléfono:");
            lblEmail.Text = T("lblEmail", "Correo Electrónico:");
            lblPlan.Text = T("lblPlan", "Plan de Suscripción:");
            lblMetodoPago.Text = T("lblMetodoPago", "Método de Pago:");
            lblTituloDatosSocio.Text = T("lblTituloDatosSocio", "1. IDENTIFICACIÓN Y CONTACTO DEL SOCIO");
            lblTituloSeleccionPlan.Text = T("lblTituloSeleccionPlan", "2. CONFIGURACIÓN DE SUSCRIPCIÓN Y PAGO");
            lblTituloSimulador.Text = T("lblTituloSimulador", "3. SIMULADOR COMERCIAL Y RESUMEN DE COBERTURA");
            lblSimImporteTitulo.Text = T("lblSimImporteTitulo", "IMPORTE FINAL A COBRAR:");
            chkEmitirReciboPDF.Text = T("chkEmitirReciboPDF", "📄 Generar y abrir Recibo Oficial en PDF al confirmar la venta");
            this.Text = T("titRecepcion", "Recepción");
            string metodoActualKey = cmbMetodoPago.SelectedValue != null ? cmbMetodoPago.SelectedValue.ToString() : "Efectivo";
            Dictionary<string, string> metodos = new Dictionary<string, string>
            {
                { "Efectivo", T("metodoEfectivo", "Efectivo") },
                { "Tarjeta de Débito", T("metodoDebito", "Tarjeta de Débito") },
                { "Tarjeta de Crédito", T("metodoCredito", "Tarjeta de Crédito") },
                { "Transferencia", T("metodoTransferencia", "Transferencia") }
            };
            cmbMetodoPago.DataSource = new BindingSource(metodos, null);
            cmbMetodoPago.DisplayMember = "Value";
            cmbMetodoPago.ValueMember = "Key";
            if (metodos.ContainsKey(metodoActualKey))
            {
                cmbMetodoPago.SelectedValue = metodoActualKey;
            }
            if (cmbPlanes.DataSource != null)
            {
                int indicePlan = cmbPlanes.SelectedIndex;
                List<PlanSuscripcionBE> planes = (List<PlanSuscripcionBE>)cmbPlanes.DataSource;
                foreach (var plan in planes)
                {
                    if (plan.CodigoPlan == 1) plan.Nombre = T("planMensual", "Plan Mensual");
                    if (plan.CodigoPlan == 2) plan.Nombre = T("planTrimestral", "Plan Trimestral");
                    if (plan.CodigoPlan == 3) plan.Nombre = T("planAnual", "Plan Anual");
                }
                cmbPlanes.SelectedIndexChanged -= cmbPlanes_SelectedIndexChanged;
                cmbPlanes.DataSource = null;
                cmbPlanes.DataSource = planes;
                cmbPlanes.DisplayMember = "Nombre";
                cmbPlanes.ValueMember = "CodigoPlan";
                if (indicePlan >= 0 && indicePlan < cmbPlanes.Items.Count)
                    cmbPlanes.SelectedIndex = indicePlan;
                cmbPlanes.SelectedIndexChanged += cmbPlanes_SelectedIndexChanged;
            }
            RefrescarBannerEstadoSocio();
            ActualizarSimuladorComercial();
        }

        private void CargarPlanes()
        {
            cmbPlanes.SelectedIndexChanged -= cmbPlanes_SelectedIndexChanged;
            cmbPlanes.DataSource = planBLL.ListarPlanes();
            cmbPlanes.DisplayMember = "Nombre";
            cmbPlanes.ValueMember = "CodigoPlan";
            cmbPlanes.SelectedIndexChanged += cmbPlanes_SelectedIndexChanged;
        }

        private void cmbPlanes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarSimuladorComercial();
        }

        private void ActualizarSimuladorComercial()
        {
            if (!(cmbPlanes.SelectedItem is PlanSuscripcionBE planActual)) return;
            var listaPlanes = cmbPlanes.DataSource as List<PlanSuscripcionBE>;
            PlanSuscripcionBE planMensual = listaPlanes?.FirstOrDefault(p => p.CodigoPlan == 1) ?? planActual;
            PlanSuscripcionBE planTrimestral = listaPlanes?.FirstOrDefault(p => p.CodigoPlan == 2) ?? planActual;
            PlanSuscripcionBE planAnual = listaPlanes?.FirstOrDefault(p => p.CodigoPlan == 3) ?? planActual;
            int duracionActual = planActual.DuracionDias > 0 ? planActual.DuracionDias : 30;
            decimal precioActual = planActual.Precio;
            decimal costoDiarioActual = Math.Round(precioActual / duracionActual, 2);
            DateTime fechaHoy = DateTime.Today;
            DateTime fechaVenc = fechaHoy.AddDays(duracionActual);
            int durMensual = planMensual.DuracionDias > 0 ? planMensual.DuracionDias : 30;
            int durTrim = planTrimestral.DuracionDias > 0 ? planTrimestral.DuracionDias : 90;
            int durAnual = planAnual.DuracionDias > 0 ? planAnual.DuracionDias : 365;
            decimal mesesEnTrim = Math.Max(1m, Math.Round((decimal)durTrim / durMensual, 0));
            decimal costoTrimSinDto = planMensual.Precio * mesesEnTrim;
            decimal ahorroTrim = Math.Max(0m, costoTrimSinDto - planTrimestral.Precio);
            decimal pctOffTrim = costoTrimSinDto > 0 ? Math.Round(ahorroTrim * 100m / costoTrimSinDto, 0) : 0m;
            decimal mesesEnAnual = Math.Max(1m, Math.Round((decimal)durAnual / durMensual, 0));
            decimal costoAnualSinDto = planMensual.Precio * mesesEnAnual;
            decimal ahorroAnual = Math.Max(0m, costoAnualSinDto - planAnual.Precio);
            decimal pctOffAnual = costoAnualSinDto > 0 ? Math.Round(ahorroAnual * 100m / costoAnualSinDto, 0) : 0m;
            decimal costoDiaAnual = Math.Round(planAnual.Precio / durAnual, 2);
            lblMontoFinalValor.Text = $"$ {precioActual:N2}";
            try
            {
                lblSimDuracionCosto.Text = string.Format(
                    T("lblSimDuracionCosto", "Duración: {0} días corridos   |   Costo diario: $ {1} / día"),
                    duracionActual,
                    costoDiarioActual.ToString("N2"));
                lblSimVigencia.Text = string.Format(
                    T("lblSimVigencia", "📅 Cobertura garantizada: Desde HOY ({0}) hasta el {1}"),
                    fechaHoy.ToString("dd/MM/yyyy"),
                    fechaVenc.ToString("dd/MM/yyyy"));
            }
            catch
            {
                lblSimDuracionCosto.Text = $"{duracionActual} días | $ {costoDiarioActual:N2}/día";
                lblSimVigencia.Text = $"{fechaHoy:dd/MM/yyyy} -> {fechaVenc:dd/MM/yyyy}";
            }
            try
            {
                if (planActual.CodigoPlan == 1)
                {
                    lblArgumentoUpselling.BackColor = Color.FromArgb(254, 243, 199);
                    lblArgumentoUpselling.ForeColor = Color.FromArgb(146, 64, 14);
                    lblArgumentoUpselling.Text = string.Format(
                        T("msgUpsellMensual", "💡 ARGUMENTO DE VENTA (UPSELLING):\r\n• Ofrezca el Plan Trimestral por $ {0}: el socio ahorra $ {1} ({2}% OFF).\r\n• Ofrezca el Plan Anual por $ {3}: el socio ahorra $ {4} ({5}% OFF)."),
                        planTrimestral.Precio.ToString("N2"),
                        ahorroTrim.ToString("N2"),
                        pctOffTrim.ToString("0"),
                        planAnual.Precio.ToString("N2"),
                        ahorroAnual.ToString("N2"),
                        pctOffAnual.ToString("0"));
                }
                else if (planActual.CodigoPlan == 2)
                {
                    lblArgumentoUpselling.BackColor = Color.FromArgb(239, 246, 255);
                    lblArgumentoUpselling.ForeColor = Color.FromArgb(30, 58, 138);
                    lblArgumentoUpselling.Text = string.Format(
                        T("msgUpsellTrimestral", "✅ BENEFICIO APLICADO (Plan Trimestral):\r\nEl socio obtiene un ahorro directo de $ {0} ({1}% OFF) respecto de abonar meses individuales.\r\n💡 Tip: Con el Plan Anual ($ {2}) su costo diario baja a $ {3}/día (ahorra $ {4})."),
                        ahorroTrim.ToString("N2"),
                        pctOffTrim.ToString("0"),
                        planAnual.Precio.ToString("N2"),
                        costoDiaAnual.ToString("N2"),
                        ahorroAnual.ToString("N2"));
                }
                else
                {
                    lblArgumentoUpselling.BackColor = Color.FromArgb(220, 252, 231);
                    lblArgumentoUpselling.ForeColor = Color.FromArgb(22, 101, 52);
                    lblArgumentoUpselling.Text = string.Format(
                        T("msgUpsellAnual", "🏆 MÁXIMO BENEFICIO APLICADO (Plan Anual Fidelizado):\r\nEl socio accede a la tarifa más baja del gimnasio ($ {0}/día), obteniendo un ahorro total de $ {1} ({2}% OFF) y congelando el precio por {3} días."),
                        costoDiaAnual.ToString("N2"),
                        ahorroAnual.ToString("N2"),
                        pctOffAnual.ToString("0"),
                        durAnual);
                }
            }
            catch
            {
                lblArgumentoUpselling.Text = $"{planActual.Nombre} — $ {precioActual:N2} ({duracionActual} días)";
            }
        }

        private string TraducirMetodoPago(string metodoClave)
        {
            if (string.IsNullOrWhiteSpace(metodoClave) || metodoClave == "-")
                return "-";
            if (metodoClave.Equals("Efectivo", StringComparison.OrdinalIgnoreCase))
                return T("metodoEfectivo", "Efectivo");
            if (metodoClave.Equals("Tarjeta de Débito", StringComparison.OrdinalIgnoreCase))
                return T("metodoDebito", "Tarjeta de Débito");
            if (metodoClave.Equals("Tarjeta de Crédito", StringComparison.OrdinalIgnoreCase))
                return T("metodoCredito", "Tarjeta de Crédito");
            if (metodoClave.Equals("Transferencia", StringComparison.OrdinalIgnoreCase))
                return T("metodoTransferencia", "Transferencia");
            return metodoClave;
        }

        private void RefrescarBannerEstadoSocio()
        {
            if (_estadoBannerModo == "NUEVO")
            {
                lblEstadoSocioBanner.BackColor = Color.FromArgb(239, 246, 255);
                lblEstadoSocioBanner.ForeColor = Color.FromArgb(30, 58, 138);
                lblEstadoSocioBanner.Text = T("lblEstadoNuevoSocio", "🌟 NUEVO SOCIO DETECTADO — Complete nombre, apellido, teléfono y correo para darlo de alta en el padrón.");
            }
            else if (_estadoBannerModo == "RENOVACION" && socioActual != null)
            {
                lblEstadoSocioBanner.BackColor = Color.FromArgb(220, 252, 231);
                lblEstadoSocioBanner.ForeColor = Color.FromArgb(22, 101, 52);
                string metodoTraducido = TraducirMetodoPago(_metodoUltimoPagoClave);
                try
                {
                    lblEstadoSocioBanner.Text = string.Format(
                        T("lblEstadoRenovacion", "🔄 RENOVACIÓN Habilitada: {0} | Pagos históricos: {1} | Último pago: {2} ({3})"),
                        $"{socioActual.Nombre} {socioActual.Apellido}",
                        _cantPagosHistoricos,
                        _fechaUltimoPagoTexto,
                        metodoTraducido);
                }
                catch
                {
                    lblEstadoSocioBanner.Text = $"🔄 {socioActual.Nombre} {socioActual.Apellido} — {_fechaUltimoPagoTexto} ({metodoTraducido})";
                }
            }
            else
            {
                lblEstadoSocioBanner.BackColor = Color.FromArgb(226, 232, 240);
                lblEstadoSocioBanner.ForeColor = Color.FromArgb(31, 41, 55);
                lblEstadoSocioBanner.Text = T("lblEstadoSocioInicial", "ℹ️ Ingrese el DNI del cliente y presione 'Buscar / Verificar' para consultar su historial o iniciar un Alta.");
            }
        }

        private void LimpiarCamposSocio()
        {
            txtDNI.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            socioActual = null;
            _estadoBannerModo = "INICIAL";
            _cantPagosHistoricos = 0;
            _fechaUltimoPagoTexto = "-";
            _metodoUltimoPagoClave = "-";
            RefrescarBannerEstadoSocio();
            txtDNI.Focus();
        }

        private void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            LimpiarCamposSocio();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show(T("msgIngresarDNI", "Debe ingresar un DNI para buscar."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string dniBuscado = txtDNI.Text.Trim();
            socioActual = socioBLL.ConsultarSocio(dniBuscado);
            if (socioActual != null)
            {
                bool tienePlanVigente = socioBLL.ObtenerEstadoVigenciaPlan(
                    socioActual,
                    out DateTime? fechaVenc,
                    out string nombrePlan,
                    out int diasRestantes,
                    out _);
                if (tienePlanVigente)
                {
                    MessageBox.Show(
                        $"{T("msgSocioConPlanVigente", "Operación no permitida: El socio ya posee un plan vigente activo. No se puede vender un nuevo plan hasta que venza el actual.")}\r\n\r\n({socioActual.Nombre} {socioActual.Apellido} — {nombrePlan}: {fechaVenc:dd/MM/yyyy})",
                        T("titSocioConPlanVigente", "Socio con Plan Vigente"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    LimpiarCamposSocio();
                    return;
                }
                txtNombre.Text = socioActual.Nombre;
                txtApellido.Text = socioActual.Apellido;
                txtTelefono.Text = socioActual.Telefono;
                txtEmail.Text = socioActual.Email;
                txtNombre.Enabled = false;
                txtApellido.Enabled = false;
                var historialPagos = comprobanteBLL.ConsultarComprobantesFiltros(
                    new DateTime(2000, 1, 1),
                    DateTime.Now.AddYears(1),
                    socioActual.DNI);
                _cantPagosHistoricos = historialPagos != null ? historialPagos.Count : 0;
                _fechaUltimoPagoTexto = "-";
                _metodoUltimoPagoClave = "-";
                if (historialPagos != null && historialPagos.Count > 0)
                {
                    var ultimo = historialPagos.OrderByDescending(c => c.Fecha).ThenByDescending(c => c.NroComprobante).First();
                    _fechaUltimoPagoTexto = ultimo.Fecha.ToString("dd/MM/yyyy");
                    _metodoUltimoPagoClave = !string.IsNullOrWhiteSpace(ultimo.MetodoPago) ? ultimo.MetodoPago : "Efectivo";
                    cmbMetodoPago.SelectedValue = _metodoUltimoPagoClave;
                }
                if (socioActual.PlanAsociado != null && socioActual.PlanAsociado.CodigoPlan > 0)
                {
                    cmbPlanes.SelectedValue = socioActual.PlanAsociado.CodigoPlan;
                }
                _estadoBannerModo = "RENOVACION";
                RefrescarBannerEstadoSocio();
                ActualizarSimuladorComercial();
            }
            else
            {
                _estadoBannerModo = "NUEVO";
                RefrescarBannerEstadoSocio();
                MessageBox.Show(T("msgSocioNoEncontradoAlta", "Socio no encontrado. Complete los datos para procesar el Alta."), T("titNuevoSocio", "Nuevo Socio"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Enabled = true;
                txtApellido.Enabled = true;
                txtNombre.Clear();
                txtApellido.Clear();
                txtTelefono.Clear();
                txtEmail.Clear();
                socioActual = new SocioBE();
                txtNombre.Focus();
            }
        }

        private void btnConfirmarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (socioActual == null)
                    throw new Exception(T("msgBuscarSocioPrimero", "Debe buscar o ingresar un socio primero."));
                if (cmbPlanes.SelectedItem == null)
                    throw new Exception(T("msgSeleccionarPlan", "Debe seleccionar un plan de suscripción."));
                string metodoPago = string.Empty;
                if (cmbMetodoPago.SelectedValue != null)
                {
                    metodoPago = cmbMetodoPago.SelectedValue.ToString();
                }
                else if (cmbMetodoPago.SelectedItem is KeyValuePair<string, string> kvp)
                {
                    metodoPago = !string.IsNullOrWhiteSpace(kvp.Value) ? kvp.Value : kvp.Key;
                }
                else if (cmbMetodoPago.SelectedItem != null)
                {
                    metodoPago = cmbMetodoPago.SelectedItem.ToString();
                }
                else if (!string.IsNullOrWhiteSpace(cmbMetodoPago.Text))
                {
                    metodoPago = cmbMetodoPago.Text.Trim();
                }
                if (string.IsNullOrWhiteSpace(metodoPago))
                    throw new Exception(T("msgSeleccionarMetodoPago", "Debe seleccionar un método de pago."));
                if (socioActual.ID_Socio == 0)
                {
                    socioActual.DNI = txtDNI.Text.Trim();
                    socioActual.Nombre = txtNombre.Text.Trim();
                    socioActual.Apellido = txtApellido.Text.Trim();
                    socioActual.Telefono = txtTelefono.Text.Trim();
                    socioActual.Email = txtEmail.Text.Trim();
                    socioBLL.RegistrarSocio(socioActual);
                }
                else
                {
                    socioActual.Telefono = txtTelefono.Text.Trim();
                    socioActual.Email = txtEmail.Text.Trim();
                }
                PlanSuscripcionBE planSeleccionado = (PlanSuscripcionBE)cmbPlanes.SelectedItem;
                socioBLL.ActualizarPlanSocio(socioActual, planSeleccionado);
                ComprobantePagoBE comprobante = new ComprobantePagoBE();
                comprobante.Fecha = DateTime.Now;
                comprobante.MontoTotal = planSeleccionado.Precio;
                comprobante.MetodoPago = metodoPago;
                comprobante.Socio = socioActual;
                comprobante.EmpleadoCobrador = SessionManager.Instancia.UsuarioActual;
                comprobanteBLL.RegistrarVenta(comprobante);
                MessageBox.Show(T("msgVentaExito", "Venta registrada y permisos de acceso habilitados con éxito."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (chkEmitirReciboPDF.Checked)
                {
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                        sfd.Title = "Guardar Recibo Oficial de Suscripción";
                        sfd.FileName = $"Recibo_UAIFIT_{comprobante.NroComprobante:D6}_DNI_{socioActual.DNI}.pdf";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            string cobrador = SessionManager.Instancia.UsuarioActual != null
                                ? SessionManager.Instancia.UsuarioActual.Nombre
                                : "Recepcion";
                            PdfCorporativoHelper.GenerarReciboSuscripcionPDF(
                                sfd.FileName,
                                comprobante,
                                socioActual,
                                planSeleccionado,
                                cobrador,
                                true);
                        }
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmRecepcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }

        private void btnConsultarPlanes_Click(object sender, EventArgs e)
        {
            using (frmConsultarPlanes catalogo = new frmConsultarPlanes())
            {
                catalogo.ShowDialog();
            }
        }
    }
}