using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmConsultarComprobantes : Form, IObserverIdioma
    {
        ComprobantePagoBLL comprobanteBLL = new ComprobantePagoBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();

        public frmConsultarComprobantes()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmConsultarComprobantes_Load(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            EjecutarFiltro();
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("lblDesde")) lblDesde.Text = traducciones["lblDesde"];
            if (traducciones.ContainsKey("lblHasta")) lblHasta.Text = traducciones["lblHasta"];
            if (traducciones.ContainsKey("lblDNI")) lblDNI.Text = traducciones["lblDNI"];
            if (traducciones.ContainsKey("btnFiltrar")) btnFiltrar.Text = traducciones["btnFiltrar"];
            if (traducciones.ContainsKey("btnLimpiar")) btnLimpiar.Text = traducciones["btnLimpiar"];
            this.Text = traducciones.ContainsKey("titConsultarComprobantes") ? traducciones["titConsultarComprobantes"] : "Consulta de Comprobantes";
            if (dgvComprobantes.Columns["NroComprobante"] != null && traducciones.ContainsKey("colNro")) dgvComprobantes.Columns["NroComprobante"].HeaderText = traducciones["colNro"];
            if (dgvComprobantes.Columns["Fecha"] != null && traducciones.ContainsKey("colFecha")) dgvComprobantes.Columns["Fecha"].HeaderText = traducciones["colFecha"];
            if (dgvComprobantes.Columns["MontoTotal"] != null && traducciones.ContainsKey("colMonto")) dgvComprobantes.Columns["MontoTotal"].HeaderText = traducciones["colMonto"];
            if (dgvComprobantes.Columns["MetodoPago"] != null && traducciones.ContainsKey("colMetodo")) dgvComprobantes.Columns["MetodoPago"].HeaderText = traducciones["colMetodo"];
            if (dgvComprobantes.Columns["DNI_Socio"] != null && traducciones.ContainsKey("colDNISocio")) dgvComprobantes.Columns["DNI_Socio"].HeaderText = traducciones["colDNISocio"];
            if (dgvComprobantes.Columns["Nombre_Socio"] != null && traducciones.ContainsKey("colNombreSocio")) dgvComprobantes.Columns["Nombre_Socio"].HeaderText = traducciones["colNombreSocio"];
            if (dgvComprobantes.Columns["Cobrador"] != null && traducciones.ContainsKey("colCobrador")) dgvComprobantes.Columns["Cobrador"].HeaderText = traducciones["colCobrador"];
        }

        private void EjecutarFiltro()
        {
            try
            {
                var lista = comprobanteBLL.ConsultarComprobantesFiltros(dtpDesde.Value, dtpHasta.Value, txtDNI.Text);
                dgvComprobantes.DataSource = null;
                dgvComprobantes.DataSource = lista.Select(c => new
                {
                    c.NroComprobante,
                    c.Fecha,
                    c.MontoTotal,
                    c.MetodoPago,
                    DNI_Socio = c.Socio.DNI,
                    Nombre_Socio = c.Socio.Nombre + " " + c.Socio.Apellido,
                    Cobrador = c.EmpleadoCobrador.Nombre
                }).ToList();
                ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            EjecutarFiltro();
        }

        private void frmConsultarComprobantes_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
