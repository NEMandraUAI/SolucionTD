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
        private List<UsuarioBE> _listaNutricionistas = new List<UsuarioBE>();
        private SugerenciaTurnoBE _sugerenciaActual = null;

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
            dtpHoraTurno.Value = DateTime.Today.AddHours(12);
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            CargarNutricionistas();
            CargarAgenda(dtpFechaAgenda.Value);
        }

        private void CargarNutricionistas()
        {
            List<UsuarioBE> todosLosUsuarios = _usuarioBLL.ListarTodos();
            _listaNutricionistas = new List<UsuarioBE>();
            PermisoBLL permisoBLL = new PermisoBLL();
            foreach (var usu in todosLosUsuarios)
            {
                permisoBLL.LlenarPermisosDeUsuario(usu);
                if (usu.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL"))
                {
                    _listaNutricionistas.Add(usu);
                }
            }
            cmbNutricionista.DataSource = _listaNutricionistas;
            cmbNutricionista.DisplayMember = "Nombre";
            cmbNutricionista.ValueMember = "ID";
        }

        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }

        private void VincularDatosGrillaAgenda(List<TurnoBE> listaTurnos)
        {
            dgvAgenda.DataSource = null;
            dgvAgenda.DataSource = listaTurnos;
            if (dgvAgenda.Columns["ID_Socio"] != null) dgvAgenda.Columns["ID_Socio"].Visible = false;
            if (dgvAgenda.Columns["ID_Nutricionista"] != null) dgvAgenda.Columns["ID_Nutricionista"].Visible = false;
            if (dgvAgenda.Columns["DVH"] != null) dgvAgenda.Columns["DVH"].Visible = false;
            if (dgvAgenda.Columns["Socio"] != null) dgvAgenda.Columns["Socio"].Visible = false;
            if (dgvAgenda.Columns["Nutricionista"] != null) dgvAgenda.Columns["Nutricionista"].Visible = false;
            if (dgvAgenda.Columns["NroTurno"] != null) dgvAgenda.Columns["NroTurno"].HeaderText = T("NroTurno", "Nro. Turno");
            if (dgvAgenda.Columns["FechaHora"] != null) dgvAgenda.Columns["FechaHora"].HeaderText = T("FechaHora", "Fecha y Hora");
            if (dgvAgenda.Columns["Estado"] != null) dgvAgenda.Columns["Estado"].HeaderText = T("Estado", "Estado");
            if (dgvAgenda.Columns["DNISocio"] != null) dgvAgenda.Columns["DNISocio"].HeaderText = T("DNISocio", "DNI Socio");
            if (dgvAgenda.Columns["NombreSocio"] != null) dgvAgenda.Columns["NombreSocio"].HeaderText = T("NombreSocio", "Socio");
            if (dgvAgenda.Columns["NombreNutricionista"] != null) dgvAgenda.Columns["NombreNutricionista"].HeaderText = T("NombreNutricionista", "Nutricionista");
        }

        private void CargarAgenda(DateTime fecha)
        {
            VincularDatosGrillaAgenda(_turnoBLL.ObtenerAgenda(fecha));
        }

        private void dtpFechaAgenda_ValueChanged(object sender, EventArgs e)
        {
            CargarAgenda(dtpFechaAgenda.Value);
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {
            EvaluarSugerenciaSocio();
        }

        private void EvaluarSugerenciaSocio()
        {
            _sugerenciaActual = null;
            btnAplicarSugerencia.Enabled = false;
            string dni = txtDNI.Text.Trim();
            if (dni.Length < 7)
            {
                pnlSugerencia.BackColor = Color.FromArgb(241, 245, 249);
                lblSugerencia.ForeColor = Color.FromArgb(71, 85, 105);
                lblSugerencia.Text = T("msgPredictivoInicial", "💡 AGENDAMIENTO PREDICTIVO:\r\nIngrese el DNI del socio para analizar su historial y sugerir el turno óptimo.");
                return;
            }
            try
            {
                SocioBE socio = _socioBLL.ConsultarSocio(dni);
                if (socio == null)
                {
                    pnlSugerencia.BackColor = Color.FromArgb(254, 242, 242);
                    lblSugerencia.ForeColor = Color.FromArgb(153, 27, 27);
                    lblSugerencia.Text = $"❌ DNI {dni}: Socio no encontrado.";
                    return;
                }
                bool planVigenteHoy = _socioBLL.ObtenerEstadoVigenciaPlan(
                    socio,
                    out DateTime? fechaVencimiento,
                    out string nombrePlan,
                    out int diasRestantes,
                    out string msgVigencia);
                if (!planVigenteHoy)
                {
                    pnlSugerencia.BackColor = Color.FromArgb(254, 226, 226);
                    lblSugerencia.ForeColor = Color.FromArgb(153, 27, 27);
                    lblSugerencia.Text = $"🚨 {socio.Nombre} {socio.Apellido}\r\n{msgVigencia}";
                    return;
                }
                List<TurnoBE> pendientes = _turnoBLL.ObtenerTurnosPendientesPorSocio(socio.ID_Socio);
                if (pendientes.Count > 0)
                {
                    TurnoBE tPend = pendientes.First();
                    pnlSugerencia.BackColor = Color.FromArgb(254, 243, 199);
                    lblSugerencia.ForeColor = Color.FromArgb(146, 64, 14);
                    lblSugerencia.Text = $"⚠️ {socio.Nombre} {socio.Apellido}\r\nTurno PENDIENTE: {tPend.FechaHora:dd/MM/yyyy HH:mm} hs (#{tPend.NroTurno}).";
                    return;
                }
                _sugerenciaActual = _turnoBLL.ObtenerSugerenciaPredictiva(socio.ID_Socio, _listaNutricionistas);
                if (_sugerenciaActual != null && _sugerenciaActual.TieneSugerencia)
                {
                    if (fechaVencimiento.HasValue && _sugerenciaActual.FechaHoraSugerida.Date > fechaVencimiento.Value.Date)
                    {
                        pnlSugerencia.BackColor = Color.FromArgb(254, 243, 199);
                        lblSugerencia.ForeColor = Color.FromArgb(146, 64, 14);
                        lblSugerencia.Text = $"⚠️ {socio.Nombre} {socio.Apellido}\r\n{msgVigencia}\r\nPróximo turno ({_sugerenciaActual.FechaHoraSugerida:dd/MM/yyyy}) supera vencimiento ({fechaVencimiento.Value:dd/MM/yyyy}).";
                        btnAplicarSugerencia.Enabled = false;
                    }
                    else
                    {
                        pnlSugerencia.BackColor = Color.FromArgb(239, 246, 255);
                        lblSugerencia.ForeColor = Color.FromArgb(30, 58, 138);
                        lblSugerencia.Text = $"✓ {socio.Nombre} {socio.Apellido} (Vence: {fechaVencimiento:dd/MM/yyyy})\r\n{_sugerenciaActual.Mensaje}";
                        btnAplicarSugerencia.Enabled = true;
                    }
                }
                else
                {
                    pnlSugerencia.BackColor = Color.FromArgb(241, 245, 249);
                    lblSugerencia.ForeColor = Color.FromArgb(71, 85, 105);
                    lblSugerencia.Text = $"✓ {socio.Nombre} {socio.Apellido}\r\n{(_sugerenciaActual != null ? _sugerenciaActual.Mensaje : "-")}";
                }
            }
            catch
            {
                pnlSugerencia.BackColor = Color.FromArgb(241, 245, 249);
            }
        }

        private void btnAplicarSugerencia_Click(object sender, EventArgs e)
        {
            if (_sugerenciaActual == null || !_sugerenciaActual.TieneSugerencia) return;
            DateTime fh = _sugerenciaActual.FechaHoraSugerida;
            dtpFechaAgenda.Value = fh.Date;
            dtpHoraTurno.Value = DateTime.Today.Add(new TimeSpan(fh.Hour, fh.Minute, 0));
            if (_sugerenciaActual.ID_NutricionistaSugerido > 0)
            {
                cmbNutricionista.SelectedValue = _sugerenciaActual.ID_NutricionistaSugerido;
            }
            CargarAgenda(dtpFechaAgenda.Value);
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            try
            {
                string dniIngresado = txtDNI.Text.Trim();
                if (string.IsNullOrWhiteSpace(dniIngresado)) throw new Exception("Debe ingresar el DNI del socio.");
                if (cmbNutricionista.SelectedItem == null) throw new Exception("Debe seleccionar un nutricionista.");
                int idNutricionistaElegido = Convert.ToInt32(cmbNutricionista.SelectedValue);
                DateTime soloFecha = dtpFechaAgenda.Value.Date;
                TimeSpan soloHora = new TimeSpan(dtpHoraTurno.Value.Hour, dtpHoraTurno.Value.Minute, 0);
                DateTime fechaHoraSolicitada = soloFecha.Add(soloHora);
                SocioBE socio = _socioBLL.ConsultarSocio(dniIngresado);
                if (socio == null) throw new Exception("Socio no encontrado.");
                if (!socio.EstadoActivo) throw new Exception("El socio se encuentra inactivo.");
                List<TurnoBE> pendientes = _turnoBLL.ObtenerTurnosPendientesPorSocio(socio.ID_Socio);
                if (pendientes.Count > 0)
                {
                    MessageBox.Show(T("msgReservaDenegadaPendiente", "Este socio ya posee un turno en estado 'Pendiente'. No puede agendar uno nuevo hasta que asista o se cancele el actual."), T("titReservaDenegada", "Reserva Denegada"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    VincularDatosGrillaAgenda(pendientes);
                    return;
                }
                _turnoBLL.ValidarMargenDeTiempo(idNutricionistaElegido, fechaHoraSolicitada);
                TurnoBE nuevoTurno = new TurnoBE
                {
                    FechaHora = fechaHoraSolicitada,
                    ID_Socio = socio.ID_Socio,
                    ID_Nutricionista = idNutricionistaElegido,
                    Socio = socio
                };
                _turnoBLL.AsentarReserva(nuevoTurno);
                MessageBox.Show(T("msgTurnoReservadoExito", "Turno reservado con éxito."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarAgenda(dtpFechaAgenda.Value);
                EvaluarSugerenciaSocio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CambiarEstadoSeleccionado(string nuevoEstado)
        {
            try
            {
                if (dgvAgenda.CurrentRow == null) throw new Exception(T("msgSeleccioneTurno", "Seleccione un turno de la agenda."));
                TurnoBE turnoSeleccionado = (TurnoBE)dgvAgenda.CurrentRow.DataBoundItem;
                _turnoBLL.CambiarEstadoTurno(turnoSeleccionado, nuevoEstado);
                MessageBox.Show($"{T("msgEstadoTurnoModificado", "El turno ha sido marcado como: ")}{nuevoEstado}", T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarAgenda(dtpFechaAgenda.Value);
                EvaluarSugerenciaSocio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            this.Text = T("titAgendaRecepcion", "Gestión de Agenda - Recepción");
            CargarAgenda(dtpFechaAgenda.Value);
            EvaluarSugerenciaSocio();
        }

        private void TraducirControlesRecursivo(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (_traducciones != null && _traducciones.ContainsKey(c.Name)) c.Text = _traducciones[c.Name];
                if (c.HasChildren) TraducirControlesRecursivo(c.Controls);
            }
        }

        private void frmAgendaRecepcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}