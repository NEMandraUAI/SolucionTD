using System;
using System.Collections.Generic;
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
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey("btnBuscar")) btnBuscar.Text = _traducciones["btnBuscar"];
            if (_traducciones.ContainsKey("btnConfirmarVenta")) btnConfirmarVenta.Text = _traducciones["btnConfirmarVenta"];
            if (_traducciones.ContainsKey("btnConsultarPlanes")) btnConsultarPlanes.Text = _traducciones["btnConsultarPlanes"];
            if (_traducciones.ContainsKey("lblDNI")) lblDNI.Text = _traducciones["lblDNI"];
            if (_traducciones.ContainsKey("lblNombre")) lblNombre.Text = _traducciones["lblNombre"];
            if (_traducciones.ContainsKey("lblApellido")) lblApellido.Text = _traducciones["lblApellido"];
            if (_traducciones.ContainsKey("lblTelefono")) lblTelefono.Text = _traducciones["lblTelefono"];
            if (_traducciones.ContainsKey("lblEmail")) lblEmail.Text = _traducciones["lblEmail"];
            if (_traducciones.ContainsKey("lblPlan")) lblPlan.Text = _traducciones["lblPlan"];
            if (_traducciones.ContainsKey("lblMetodoPago")) lblMetodoPago.Text = _traducciones["lblMetodoPago"];
            this.Text = T("titRecepcion", "Recepción");
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
                cmbPlanes.DataSource = null;
                cmbPlanes.DataSource = planes;
                cmbPlanes.DisplayMember = "Nombre";
                cmbPlanes.ValueMember = "CodigoPlan";
                if (indicePlan >= 0 && indicePlan < cmbPlanes.Items.Count)
                    cmbPlanes.SelectedIndex = indicePlan;
            }
        }

        private void CargarPlanes()
        {
            cmbPlanes.DataSource = planBLL.ListarPlanes();
            cmbPlanes.DisplayMember = "Nombre";
            cmbPlanes.ValueMember = "CodigoPlan";
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
            txtDNI.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show(T("msgIngresarDNI", "Debe ingresar un DNI para buscar."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            socioActual = socioBLL.ConsultarSocio(txtDNI.Text.Trim());
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
            }
            else
            {
                MessageBox.Show(T("msgSocioNoEncontradoAlta", "Socio no encontrado. Complete los datos para procesar el Alta."), T("titNuevoSocio", "Nuevo Socio"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Enabled = true;
                txtApellido.Enabled = true;
                txtNombre.Clear();
                txtApellido.Clear();
                txtTelefono.Clear();
                txtEmail.Clear();
                socioActual = new SocioBE();
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
            frmConsultarPlanes catalogo = new frmConsultarPlanes();
            catalogo.ShowDialog();
        }
    }
}