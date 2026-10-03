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
    public partial class frmAgendaRecepcion : Form, IObserverIdioma
    {
        private TurnoBLL _turnoBLL;
        private SocioBLL _socioBLL;
        private UsuarioBLL _usuarioBLL;
        private Dictionary<string, string> _traducciones;

        public frmAgendaRecepcion()
        {
            InitializeComponent();
            _turnoBLL = new TurnoBLL();
            _socioBLL = new SocioBLL();
            _usuarioBLL = new UsuarioBLL();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmAgendaRecepcion_Load(object sender, EventArgs e)
        {
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            CargarNutricionistas();
            CargarAgenda(dtpFechaAgenda.Value);
        }

        private void CargarNutricionistas()
        {
            List<UsuarioBE> todosLosUsuarios = _usuarioBLL.ListarTodos();
            List<UsuarioBE> nutricionistas = new List<UsuarioBE>();
            PermisoBLL permisoBLL = new PermisoBLL();
            foreach (var usu in todosLosUsuarios)
            {
                permisoBLL.LlenarPermisosDeUsuario(usu);
                if (usu.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL"))
                {
                    nutricionistas.Add(usu);
                }
            }
            cmbNutricionista.DataSource = nutricionistas;
            cmbNutricionista.DisplayMember = "Nombre";
            cmbNutricionista.ValueMember = "ID";
        }

        private void CargarAgenda(DateTime fecha)
        {
            dgvAgenda.DataSource = null;
            dgvAgenda.DataSource = _turnoBLL.ObtenerAgenda(fecha);
            if (dgvAgenda.Columns["ID_Socio"] != null) dgvAgenda.Columns["ID_Socio"].Visible = false;
            if (dgvAgenda.Columns["ID_Nutricionista"] != null) dgvAgenda.Columns["ID_Nutricionista"].Visible = false;
            if (dgvAgenda.Columns["DVH"] != null) dgvAgenda.Columns["DVH"].Visible = false;
            if (dgvAgenda.Columns["Socio"] != null) dgvAgenda.Columns["Socio"].Visible = false;
            if (dgvAgenda.Columns["Nutricionista"] != null) dgvAgenda.Columns["Nutricionista"].Visible = false;
        }

        private void dtpFechaAgenda_ValueChanged(object sender, EventArgs e)
        {
            CargarAgenda(dtpFechaAgenda.Value);
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDNI.Text)) throw new Exception("Debe ingresar el DNI del socio.");
                if (cmbNutricionista.SelectedItem == null) throw new Exception("Debe seleccionar un nutricionista.");
                SocioBE socio = _socioBLL.ConsultarSocio(txtDNI.Text);
                if (socio == null) throw new Exception("Socio no encontrado.");
                if (!socio.EstadoActivo) throw new Exception("El socio se encuentra inactivo.");
                DateTime fechaHoraTurno = dtpFechaAgenda.Value.Date + dtpHoraTurno.Value.TimeOfDay;
                TurnoBE nuevoTurno = new TurnoBE
                {
                    FechaHora = fechaHoraTurno,
                    ID_Socio = socio.ID_Socio,
                    ID_Nutricionista = (int)cmbNutricionista.SelectedValue
                };
                _turnoBLL.AsentarReserva(nuevoTurno);
                MessageBox.Show("Turno reservado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarAgenda(dtpFechaAgenda.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CambiarEstadoSeleccionado(string nuevoEstado)
        {
            try
            {
                if (dgvAgenda.CurrentRow == null) throw new Exception("Seleccione un turno de la agenda.");
                TurnoBE turnoSeleccionado = (TurnoBE)dgvAgenda.CurrentRow.DataBoundItem;
                _turnoBLL.CambiarEstadoTurno(turnoSeleccionado, nuevoEstado);
                MessageBox.Show($"El turno ha sido marcado como: {nuevoEstado}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarAgenda(dtpFechaAgenda.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAsistio_Click(object sender, EventArgs e)
        {
            CambiarEstadoSeleccionado("Asistió");
        }

        private void btnAusente_Click(object sender, EventArgs e)
        {
            CambiarEstadoSeleccionado("Ausente");
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            if (idioma == null) return;
            _traducciones = new IdiomaBLL().ObtenerTraducciones(idioma, this.Name);
            TraducirControlesRecursivo(this.Controls);
            if (_traducciones.ContainsKey("titAgendaRecepcion")) this.Text = _traducciones["titAgendaRecepcion"];
        }

        private void TraducirControlesRecursivo(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (_traducciones.ContainsKey(c.Name)) c.Text = _traducciones[c.Name];
                if (c.HasChildren) TraducirControlesRecursivo(c.Controls);
            }
        }

        private void frmAgendaRecepcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
