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
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }

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
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey("lblDesde")) lblDesde.Text = _traducciones["lblDesde"];
            if (_traducciones.ContainsKey("lblHasta")) lblHasta.Text = _traducciones["lblHasta"];
            if (_traducciones.ContainsKey("lblDNI")) lblDNI.Text = _traducciones["lblDNI"];
            if (_traducciones.ContainsKey("btnFiltrar")) btnFiltrar.Text = _traducciones["btnFiltrar"];
            if (_traducciones.ContainsKey("btnLimpiar")) btnLimpiar.Text = _traducciones["btnLimpiar"];
            this.Text = T("titConsultarComprobantes", "Consulta de Comprobantes");
            if (dgvComprobantes.Columns["NroComprobante"] != null && _traducciones.ContainsKey("colNro")) dgvComprobantes.Columns["NroComprobante"].HeaderText = _traducciones["colNro"];
            if (dgvComprobantes.Columns["Fecha"] != null && _traducciones.ContainsKey("colFecha")) dgvComprobantes.Columns["Fecha"].HeaderText = _traducciones["colFecha"];
            if (dgvComprobantes.Columns["MontoTotal"] != null && _traducciones.ContainsKey("colMonto")) dgvComprobantes.Columns["MontoTotal"].HeaderText = _traducciones["colMonto"];
            if (dgvComprobantes.Columns["MetodoPago"] != null && _traducciones.ContainsKey("colMetodo")) dgvComprobantes.Columns["MetodoPago"].HeaderText = _traducciones["colMetodo"];
            if (dgvComprobantes.Columns["DNI_Socio"] != null && _traducciones.ContainsKey("colDNISocio")) dgvComprobantes.Columns["DNI_Socio"].HeaderText = _traducciones["colDNISocio"];
            if (dgvComprobantes.Columns["Nombre_Socio"] != null && _traducciones.ContainsKey("colNombreSocio")) dgvComprobantes.Columns["Nombre_Socio"].HeaderText = _traducciones["colNombreSocio"];
            if (dgvComprobantes.Columns["Cobrador"] != null && _traducciones.ContainsKey("colCobrador")) dgvComprobantes.Columns["Cobrador"].HeaderText = _traducciones["colCobrador"];
        }

        private void EjecutarFiltro()
        {
            try
            {
                if (dtpDesde.Value.Date > dtpHasta.Value.Date)
                {
                    throw new Exception(T("msgFechasInvalidas", "La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'."));
                }
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
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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