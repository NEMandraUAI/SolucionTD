using System;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmConsultarPlanes : Form, IObserverIdioma
    {
        PlanSuscripcionBLL planBLL = new PlanSuscripcionBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();

        public frmConsultarPlanes()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmConsultarPlanes_Load(object sender, EventArgs e)
        {
            CargarGrilla();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }

        private void CargarGrilla()
        {
            dgvPlanes.DataSource = null;
            dgvPlanes.DataSource = planBLL.ListarPlanes();
            if (dgvPlanes.Columns["CodigoPlan"] != null) dgvPlanes.Columns["CodigoPlan"].Visible = false;
            if (dgvPlanes.Columns["DVH"] != null) dgvPlanes.Columns["DVH"].Visible = false;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("btnCerrar")) btnCerrar.Text = traducciones["btnCerrar"];
            this.Text = traducciones.ContainsKey("titCatologoPlanes") ? traducciones["titCatologoPlanes"] : "Catálogo de Planes";
            if (dgvPlanes.Columns["Nombre"] != null && traducciones.ContainsKey("colNombrePlan"))
                dgvPlanes.Columns["Nombre"].HeaderText = traducciones["colNombrePlan"];
            if (dgvPlanes.Columns["Precio"] != null && traducciones.ContainsKey("colPrecio"))
                dgvPlanes.Columns["Precio"].HeaderText = traducciones["colPrecio"];
            if (dgvPlanes.Columns["DuracionDias"] != null && traducciones.ContainsKey("colDuracion"))
                dgvPlanes.Columns["DuracionDias"].HeaderText = traducciones["colDuracion"];
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmConsultarPlanes_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
