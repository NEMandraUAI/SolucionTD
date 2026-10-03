using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmGestionNutricion : Form, IObserverIdioma
    {
        private TurnoBLL _turnoBLL;
        private SocioBLL _socioBLL;
        private PlanNutricionalBLL _planBLL;
        private Dictionary<string, string> _traducciones;
        private SocioBE _socioActual;

        public frmGestionNutricion()
        {
            InitializeComponent();
            _turnoBLL = new TurnoBLL();
            _socioBLL = new SocioBLL();
            _planBLL = new PlanNutricionalBLL();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmGestionNutricion_Load(object sender, EventArgs e)
        {
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            CargarAgendaDelDia();
            BloquearCamposPlan(true);
        }

        private void CargarAgendaDelDia()
        {
            List<TurnoBE> turnosHoy = _turnoBLL.ObtenerAgenda(DateTime.Today);
            List<TurnoBE> misTurnos = new List<TurnoBE>();
            int miID = SessionManager.Instancia.UsuarioActual.ID;
            foreach (var t in turnosHoy)
            {
                if (t.ID_Nutricionista == miID)
                {
                    misTurnos.Add(t);
                }
            }
            dgvAgendaHoy.DataSource = misTurnos;
        }

        private void btnBuscarSocio_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDNISocio.Text)) throw new Exception("Ingrese el DNI.");
                _socioActual = _socioBLL.ConsultarSocio(txtDNISocio.Text);
                if (_socioActual == null) throw new Exception("Socio no encontrado.");
                dgvPlanesHistoricos.DataSource = _planBLL.ObtenerPlanesSocio(_socioActual.ID_Socio);
                MessageBox.Show($"Socio encontrado: {_socioActual.Nombre} {_socioActual.Apellido}", "Socio Seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BloquearCamposPlan(false);
            }
            catch (Exception ex)
            {
                BloquearCamposPlan(true);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAsignarPlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (_socioActual == null) throw new Exception("Primero debe buscar y cargar un socio.");
                PlanNutricionalBE nuevoPlan = new PlanNutricionalBE
                {
                    FechaCreacion = DateTime.Now,
                    EvaluacionAntropometrica = txtAntropometria.Text,
                    DetalleHabitos = txtHabitos.Text,
                    ID_Socio = _socioActual.ID_Socio,
                    ID_Nutricionista = SessionManager.Instancia.UsuarioActual.ID
                };
                _planBLL.AsignarPlan(nuevoPlan);
                MessageBox.Show("Plan nutricional confeccionado y guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAntropometria.Clear();
                txtHabitos.Clear();
                dgvPlanesHistoricos.DataSource = _planBLL.ObtenerPlanesSocio(_socioActual.ID_Socio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BloquearCamposPlan(bool bloquear)
        {
            txtAntropometria.Enabled = !bloquear;
            txtHabitos.Enabled = !bloquear;
            btnAsignarPlan.Enabled = !bloquear;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            if (idioma == null) return;
            _traducciones = new IdiomaBLL().ObtenerTraducciones(idioma, this.Name);
            TraducirControlesRecursivo(this.Controls);
            if (_traducciones.ContainsKey("titGestionNutricion")) this.Text = _traducciones["titGestionNutricion"];
        }

        private void TraducirControlesRecursivo(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (_traducciones.ContainsKey(c.Name)) c.Text = _traducciones[c.Name];
                if (c.HasChildren) TraducirControlesRecursivo(c.Controls);
            }
        }

        private void frmGestionNutricion_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
