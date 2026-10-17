using BE;
using BLL;
using Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmSesion : Form, IObserverIdioma
    {
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }
        RegistroBLL registroBLL = new RegistroBLL();
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        UsuarioBE usuarioActual;
        public event EventHandler CerrarSesion;
        public frmSesion(UsuarioBE usuarioP)
        {
            InitializeComponent();
            usuarioActual = usuarioP;
            GestorIdioma.Instancia.Suscribir(this);
        }
        private void Sesion_Load(object sender, EventArgs e)
        {
            lblBienvenida_Base.Text = T("lblBienvenida_Base", "Sesión activa - Bienvenido, ") + usuarioActual.Nombre;
            dgvLogs.AutoGenerateColumns = false;
            ConfigurarGrilla();
            CargarComboUsuarios();
            CargarComboCriticidad();
            dtpDesde.Value = DateTime.Now.AddDays(-7);
            dtpHasta.Value = DateTime.Now;
            EjecutarFiltro();
            SessionManager.Instancia.PermisosActualizados += SessionManager_PermisosActualizados;
            AplicarPermisosVisuales();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }
        public void ActualizarIdioma(IdiomaBE idioma)
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey(this.Name))
                this.Text = _traducciones[this.Name];
            TraducirControlesRecursivo(this.Controls, _traducciones);
            if (usuarioActual != null)
            {
                lblBienvenida_Base.Text = T("lblBienvenida_Base", "Sesión activa - Bienvenido, ") + usuarioActual.Nombre;
            }
            if (cmbUsuarios.Items.Count > 0)
            {
                object valorSeleccionado = cmbUsuarios.SelectedValue;
                CargarComboUsuarios();
                if (valorSeleccionado != null)
                {
                    cmbUsuarios.SelectedValue = valorSeleccionado;
                }
            }
            if (cmbCriticidad.Items.Count > 0)
            {
                object valorSeleccionadoCrit = cmbCriticidad.SelectedValue;
                CargarComboCriticidad();
                if (valorSeleccionadoCrit != null)
                {
                    cmbCriticidad.SelectedValue = valorSeleccionadoCrit;
                }
            }
        }
        private void TraducirControlesRecursivo(Control.ControlCollection controles, Dictionary<string, string> traducciones)
        {
            foreach (Control control in controles)
            {
                if (traducciones.ContainsKey(control.Name))
                {
                    control.Text = traducciones[control.Name];
                }
                if (control is MenuStrip menuStrip)
                {
                    foreach (ToolStripItem item in menuStrip.Items)
                    {
                        TraducirItemMenu(item, traducciones);
                    }
                }
                if (control.HasChildren)
                {
                    TraducirControlesRecursivo(control.Controls, traducciones);
                }
                if (control is DataGridView dgv)
                {
                    foreach (DataGridViewColumn columna in dgv.Columns)
                    {
                        if (traducciones.ContainsKey(columna.DataPropertyName))
                        {
                            columna.HeaderText = traducciones[columna.DataPropertyName];
                        }
                    }
                }
            }
        }
        private void TraducirItemMenu(ToolStripItem item, Dictionary<string, string> traducciones)
        {
            if (traducciones.ContainsKey(item.Name))
            {
                item.Text = traducciones[item.Name];
            }
            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    TraducirItemMenu(subItem, traducciones);
                }
            }
        }
        private void SessionManager_PermisosActualizados(object sender, EventArgs e)
        {
            usuarioActual = SessionManager.Instancia.UsuarioActual;
            if (usuarioActual == null) return;
            AplicarPermisosVisuales();
            lblBienvenida_Base.Text = T("lblBienvenida_Base", "Sesión activa - Bienvenido, ") + usuarioActual.Nombre;
        }
        private void AplicarPermisosVisuales()
        {
            btnGestionRoles.Visible = usuarioActual.TienePermiso("GESTION_ROLES");
            btnControlCambios.Visible = usuarioActual.TienePermiso("CONTROL_BITACORA");
            panel1.Visible = usuarioActual.TienePermiso("CONTROL_BITACORA");
            dgvLogs.Visible = usuarioActual.TienePermiso("CONTROL_BITACORA");
            btnBackup.Visible = usuarioActual.TienePermiso("REALIZAR_BACKUP");
        }
        private void ConfigurarGrilla()
        {
            dgvLogs.Columns.Clear();
            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaHora", HeaderText = T("FechaHora", "Fecha"), Width = 130 });
            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreUsuario", HeaderText = T("NombreUsuario", "Usuario"), Width = 100 });
            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Criticidad", HeaderText = T("Criticidad", "Criticidad"), Width = 90 });
            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Evento", HeaderText = T("Evento", "Detalle del Evento"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvLogs.CellFormatting += DgvLogs_CellFormatting;
        }
        private void CargarComboUsuarios()
        {
            List<UsuarioBE> listaUsuarios = usuarioBLL.ListarTodos();
            listaUsuarios.Insert(0, new UsuarioBE { ID = 0, Nombre = T("cmbTodos", "Todos") });
            cmbUsuarios.DataSource = listaUsuarios;
            cmbUsuarios.DisplayMember = "Nombre";
            cmbUsuarios.ValueMember = "ID";
        }
        private void CargarComboCriticidad()
        {
            List<KeyValuePair<string, string>> itemsCriticidad = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(T("cmbTodos", "Todos"), "Todos"),
                new KeyValuePair<string, string>(T("criticidadInfo", "INFORMACIÓN"), "INFO"),
                new KeyValuePair<string, string>(T("criticidadAlerta", "ALERTA"), "ALERTA"),
                new KeyValuePair<string, string>(T("criticidadCritico", "CRÍTICO"), "CRÍTICO"),
                new KeyValuePair<string, string>(T("criticidadAlta", "ALTA"), "ALTA")
            };
            cmbCriticidad.DataSource = itemsCriticidad;
            cmbCriticidad.DisplayMember = "Key";
            cmbCriticidad.ValueMember = "Value";
        }
        private void EjecutarFiltro()
        {
            int? idUsu = null;
            if (cmbUsuarios.SelectedValue != null && (int)cmbUsuarios.SelectedValue > 0)
            {
                idUsu = (int)cmbUsuarios.SelectedValue;
            }
            string valorCriticidad = cmbCriticidad.SelectedValue != null ? cmbCriticidad.SelectedValue.ToString() : "Todos";
            dgvLogs.DataSource = null;
            dgvLogs.DataSource = registroBLL.ConsultaLogsFiltros(
                dtpDesde.Value,
                dtpHasta.Value,
                valorCriticidad,
                idUsu,
                txtBuscar.Text);
        }
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (CerrarSesion != null)
            {
                CerrarSesion.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.Close();
            }
        }
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            EjecutarFiltro();
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-7);
            dtpHasta.Value = DateTime.Now;
            cmbCriticidad.SelectedIndex = 0;
            cmbUsuarios.SelectedIndex = 0;
            txtBuscar.Text = "";
            EjecutarFiltro();
        }
        private void DgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLogs.Rows[e.RowIndex].DataBoundItem is RegistroBE fila)
            {
                if (dgvLogs.Columns[e.ColumnIndex].DataPropertyName == "Criticidad" && e.Value != null)
                {
                    string crit = e.Value.ToString();
                    if (crit == "INFO") e.Value = T("criticidadInfo", "INFORMACIÓN");
                    else if (crit == "ALERTA") e.Value = T("criticidadAlerta", "ALERTA");
                    else if (crit == "CRÍTICO") e.Value = T("criticidadCritico", "CRÍTICO");
                    else if (crit == "ALTA") e.Value = T("criticidadAlta", "ALTA");
                }
                if (fila.Criticidad == "ALERTA")
                {
                    dgvLogs.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else if (fila.Criticidad == "CRÍTICO")
                {
                    dgvLogs.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                    dgvLogs.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
                else if (fila.Criticidad == "ALTA")
                {
                    dgvLogs.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }
        private void btnControlCambios_Click(object sender, EventArgs e)
        {
            frmControlCambios frmCambios = new frmControlCambios();
            frmCambios.MdiParent = this.MdiParent;
            frmCambios.FormClosed += (s, args) =>
            {
                this.Show();
            };
            this.Hide();
            frmCambios.Show();
        }
        private void btnGestionRoles_Click(object sender, EventArgs e)
        {
            frmGestionRoles frmGestion = new frmGestionRoles();
            frmGestion.MdiParent = this.MdiParent;
            frmGestion.FormClosed += (s, args) =>
            {
                this.Show();
            };
            this.Hide();
            frmGestion.Show();
        }
        private void btnBackup_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = T("FiltroBackup", "Archivos de Backup SQL (*.bak)|*.bak");
            sfd.Title = T("TitGuadarBackup", "Guardar Copia de Seguridad");
            sfd.FileName = $"ProyectoCampo_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BackupBLL backupBLL = new BackupBLL();
                    backupBLL.RealizarBackup(sfd.FileName);
                    MessageBox.Show(T("msgBackupExito", "Copia de seguridad generada y almacenada con éxito."), T("titBackupExito", "Backup Exitoso"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(T("msgBackupError", "Error al generar el backup: ") + ex.Message, T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void frmSesion_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
            SessionManager.Instancia.PermisosActualizados -= SessionManager_PermisosActualizados;
        }
    }
}
