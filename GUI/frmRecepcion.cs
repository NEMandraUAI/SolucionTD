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
            CargarMetodosPago();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("btnBuscar")) btnBuscar.Text = traducciones["btnBuscar"];
            if (traducciones.ContainsKey("btnConfirmarVenta")) btnConfirmarVenta.Text = traducciones["btnConfirmarVenta"];
            if (traducciones.ContainsKey("lblDNI")) lblDNI.Text = traducciones["lblDNI"];
            if (traducciones.ContainsKey("lblNombre")) lblNombre.Text = traducciones["lblNombre"];
            if (traducciones.ContainsKey("lblApellido")) lblApellido.Text = traducciones["lblApellido"];
            if (traducciones.ContainsKey("lblTelefono")) lblTelefono.Text = traducciones["lblTelefono"];
            if (traducciones.ContainsKey("lblEmail")) lblEmail.Text = traducciones["lblEmail"];
            if (traducciones.ContainsKey("lblPlan")) lblPlan.Text = traducciones["lblPlan"];
            if (traducciones.ContainsKey("lblMetodoPago")) lblMetodoPago.Text = traducciones["lblMetodoPago"];
            this.Text = traducciones.ContainsKey("titRecepcion") ? traducciones["titRecepcion"] : "Recepción";
        }

        private void CargarPlanes()
        {
            cmbPlanes.DataSource = planBLL.ListarPlanes();
            cmbPlanes.DisplayMember = "Nombre";
            cmbPlanes.ValueMember = "CodigoPlan";
        }

        private void CargarMetodosPago()
        {
            cmbMetodoPago.Items.Add("Efectivo");
            cmbMetodoPago.Items.Add("Tarjeta de Débito");
            cmbMetodoPago.Items.Add("Tarjeta de Crédito");
            cmbMetodoPago.Items.Add("Transferencia");
            cmbMetodoPago.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            socioActual = socioBLL.ConsultarSocio(txtDNI.Text);
            if (socioActual != null)
            {
                txtNombre.Text = socioActual.Nombre;
                txtApellido.Text = socioActual.Apellido;
                txtTelefono.Text = socioActual.Telefono;
                txtEmail.Text = socioActual.Email;
                txtNombre.Enabled = false;
                txtApellido.Enabled = false;
            }
            else
            {
                MessageBox.Show("Socio no encontrado. Complete los datos para procesar el Alta.");
                txtNombre.Enabled = true;
                txtApellido.Enabled = true;
                txtNombre.Clear();
                txtApellido.Clear();
                socioActual = new SocioBE();
            }
        }

        private void btnConfirmarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (socioActual == null) throw new Exception("Debe buscar o ingresar un socio primero.");
                if (socioActual.ID_Socio == 0)
                {
                    socioActual.DNI = txtDNI.Text;
                    socioActual.Nombre = txtNombre.Text;
                    socioActual.Apellido = txtApellido.Text;
                    socioActual.Telefono = txtTelefono.Text;
                    socioActual.Email = txtEmail.Text;
                    socioBLL.RegistrarSocio(socioActual);
                }
                PlanSuscripcionBE planSeleccionado = (PlanSuscripcionBE)cmbPlanes.SelectedItem;
                socioBLL.ActualizarPlanSocio(socioActual, planSeleccionado);
                ComprobantePagoBE comprobante = new ComprobantePagoBE();
                comprobante.Fecha = DateTime.Now;
                comprobante.MontoTotal = planSeleccionado.Precio;
                comprobante.MetodoPago = cmbMetodoPago.SelectedItem.ToString();
                comprobante.Socio = socioActual;
                comprobante.EmpleadoCobrador = SessionManager.Instancia.UsuarioActual;
                comprobanteBLL.RegistrarVenta(comprobante);
                MessageBox.Show("Venta registrada y permisos de acceso habilitados con éxito.");
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
    }
}
