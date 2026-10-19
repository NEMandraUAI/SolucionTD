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
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("btnBuscar")) btnBuscar.Text = traducciones["btnBuscar"];
            if (traducciones.ContainsKey("btnConfirmarVenta")) btnConfirmarVenta.Text = traducciones["btnConfirmarVenta"];
            if (traducciones.ContainsKey("btnConsultarPlanes")) btnConsultarPlanes.Text = traducciones["btnConsultarPlanes"];
            if (traducciones.ContainsKey("lblDNI")) lblDNI.Text = traducciones["lblDNI"];
            if (traducciones.ContainsKey("lblNombre")) lblNombre.Text = traducciones["lblNombre"];
            if (traducciones.ContainsKey("lblApellido")) lblApellido.Text = traducciones["lblApellido"];
            if (traducciones.ContainsKey("lblTelefono")) lblTelefono.Text = traducciones["lblTelefono"];
            if (traducciones.ContainsKey("lblEmail")) lblEmail.Text = traducciones["lblEmail"];
            if (traducciones.ContainsKey("lblPlan")) lblPlan.Text = traducciones["lblPlan"];
            if (traducciones.ContainsKey("lblMetodoPago")) lblMetodoPago.Text = traducciones["lblMetodoPago"];
            this.Text = traducciones.ContainsKey("titRecepcion") ? traducciones["titRecepcion"] : "Recepción";
            Dictionary<string, string> metodos = new Dictionary<string, string>();
            metodos.Add("Efectivo", traducciones.ContainsKey("metodoEfectivo") ? traducciones["metodoEfectivo"] : "Efectivo");
            metodos.Add("Tarjeta de Débito", traducciones.ContainsKey("metodoDebito") ? traducciones["metodoDebito"] : "Tarjeta de Débito");
            metodos.Add("Tarjeta de Crédito", traducciones.ContainsKey("metodoCredito") ? traducciones["metodoCredito"] : "Tarjeta de Crédito");
            metodos.Add("Transferencia", traducciones.ContainsKey("metodoTransferencia") ? traducciones["metodoTransferencia"] : "Transferencia");
            cmbMetodoPago.DataSource = new BindingSource(metodos, null);
            cmbMetodoPago.DisplayMember = "Value";
            cmbMetodoPago.ValueMember = "Key";
            if (cmbPlanes.DataSource != null)
            {
                int indicePlan = cmbPlanes.SelectedIndex;
                List<PlanSuscripcionBE> planes = (List<PlanSuscripcionBE>)cmbPlanes.DataSource;
                foreach (var plan in planes)
                {
                    if (plan.CodigoPlan == 1 && traducciones.ContainsKey("planMensual")) plan.Nombre = traducciones["planMensual"];
                    if (plan.CodigoPlan == 2 && traducciones.ContainsKey("planTrimestral")) plan.Nombre = traducciones["planTrimestral"];
                    if (plan.CodigoPlan == 3 && traducciones.ContainsKey("planAnual")) plan.Nombre = traducciones["planAnual"];
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
                MessageBox.Show("Debe ingresar un DNI para buscar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        $"Operación no permitida:\r\nEl socio {socioActual.Nombre} {socioActual.Apellido} (DNI {socioActual.DNI}) ya posee un plan vigente activo ({nombrePlan}) con cobertura hasta el {fechaVenc:dd/MM/yyyy} ({diasRestantes} día(s) restantes).\r\n\r\nNo se puede vender un nuevo plan hasta que venza el actual.",
                        "Socio con Plan Vigente",
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
                MessageBox.Show("Socio no encontrado. Complete los datos para procesar el Alta.", "Nuevo Socio", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    throw new Exception("Debe buscar o ingresar un socio primero.");
                if (cmbPlanes.SelectedItem == null)
                    throw new Exception("Debe seleccionar un plan de suscripción.");
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
                    throw new Exception("Debe seleccionar un método de pago.");
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
                MessageBox.Show("Venta registrada y permisos de acceso habilitados con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
