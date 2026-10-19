using BE;
using BLL;
using Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        SocioBLL socioBLL = new SocioBLL();
        UsuarioBE usuarioActual;
        bool _mostrandoVistaChurn = false;
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
            dgvAlertas.CellFormatting += DgvAlertas_CellFormatting;
            ConfigurarGrilla();
            CargarComboUsuarios();
            CargarComboCriticidad();
            dtpDesde.Value = DateTime.Now.AddDays(-7);
            dtpHasta.Value = DateTime.Now;
            EjecutarFiltro();
            MostrarVistaAlertasOperativas();
            SessionManager.Instancia.PermisosActualizados += SessionManager_PermisosActualizados;
            AplicarPermisosVisuales();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }

        private void MostrarVistaAlertasOperativas()
        {
            _mostrandoVistaChurn = false;
            btnContactarSocio.Visible = false;
            btnVistaAlertas.BackColor = Color.FromArgb(31, 41, 55);
            btnVistaAlertas.ForeColor = Color.White;
            btnVistaChurn.BackColor = Color.FromArgb(226, 232, 240);
            btnVistaChurn.ForeColor = Color.FromArgb(31, 41, 55);
            dgvAlertas.DataSource = null;
            dgvAlertas.AutoGenerateColumns = false;
            dgvAlertas.Columns.Clear();
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Area",
                HeaderText = "Área Operativa",
                FillWeight = 13
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Prioridad",
                HeaderText = "Nivel",
                FillWeight = 9
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Titulo",
                HeaderText = "Alerta Proactiva",
                FillWeight = 32
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Detalle",
                HeaderText = "Detalle y Acción Sugerida",
                FillWeight = 35
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaReferencia",
                HeaderText = "Referencia",
                FillWeight = 11
            });
            CargarDatosDashboard();
        }

        private void MostrarVistaSociosEnRiesgo()
        {
            _mostrandoVistaChurn = true;
            btnContactarSocio.Visible = true;
            btnVistaChurn.BackColor = Color.FromArgb(185, 28, 28);
            btnVistaChurn.ForeColor = Color.White;
            btnVistaAlertas.BackColor = Color.FromArgb(226, 232, 240);
            btnVistaAlertas.ForeColor = Color.FromArgb(31, 41, 55);
            dgvAlertas.DataSource = null;
            dgvAlertas.AutoGenerateColumns = false;
            dgvAlertas.Columns.Clear();
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DNI",
                HeaderText = "DNI",
                FillWeight = 10
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Socio",
                HeaderText = "Socio",
                FillWeight = 16
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PlanActual",
                HeaderText = "Plan Asociado",
                FillWeight = 12
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NivelRiesgo",
                HeaderText = "Clasificación de Riesgo",
                FillWeight = 17
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProbabilidadFuga",
                HeaderText = "Score Fuga",
                FillWeight = 9
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MotivoDetectado",
                HeaderText = "Diagnóstico de Retención / Causas Detectadas",
                FillWeight = 26
            });
            dgvAlertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UltimaActividad",
                HeaderText = "Últ. Mov.",
                FillWeight = 10
            });
            CargarDatosDashboard();
        }

        private void DgvAlertas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var item = dgvAlertas.Rows[e.RowIndex].DataBoundItem;
            if (item is AlertaDashboardBE alerta)
            {
                if (alerta.Prioridad == "ALTA")
                {
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                }
                else if (alerta.Prioridad == "MEDIA")
                {
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                }
                else
                {
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                }
            }
            else if (item is SocioRiesgoBE riesgo)
            {
                if (riesgo.NivelRiesgo == "Riesgo de Abandono Alto")
                {
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 38, 38);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (riesgo.NivelRiesgo == "Riesgo Moderado")
                {
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                }
                else
                {
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                    dgvAlertas.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                }
            }
        }

        private void CargarDatosDashboard()
        {
            if (usuarioActual == null) return;
            var alertas = socioBLL.ObtenerAlertasDashboard(
                usuarioActual,
                out int cantRec,
                out int cantEnt,
                out int cantNut);
            var listaRiesgo = socioBLL.ObtenerSociosEnRiesgoDeFuga(
                out decimal tasaChurn,
                out int cantRiesgoAlto);
            btnVistaChurn.Text = $"📉 Socios en Riesgo (Churn: {tasaChurn:0.#}%)";
            if (_mostrandoVistaChurn)
            {
                dgvAlertas.DataSource = null;
                dgvAlertas.DataSource = listaRiesgo;
            }
            else
            {
                dgvAlertas.DataSource = null;
                dgvAlertas.DataSource = alertas;
            }
            lblKpiRecepcion.Text = $"Recepción: {cantRec} alerta(s)";
            lblKpiEntrenamiento.Text = $"Entrenamiento: {cantEnt} alerta(s)";
            lblKpiNutricion.Text = $"Nutrición: {cantNut} alerta(s)";
        }

        private void btnVistaAlertas_Click(object sender, EventArgs e)
        {
            MostrarVistaAlertasOperativas();
        }

        private void btnVistaChurn_Click(object sender, EventArgs e)
        {
            MostrarVistaSociosEnRiesgo();
        }

        private void btnRefrescarAlertas_Click(object sender, EventArgs e)
        {
            CargarDatosDashboard();
        }

        private void btnContactarSocio_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_mostrandoVistaChurn || dgvAlertas.CurrentRow == null || !(dgvAlertas.CurrentRow.DataBoundItem is SocioRiesgoBE socioRiesgo))
                {
                    MessageBox.Show("Seleccione un socio de la grilla 'Socios en Riesgo' para iniciar el contacto de retención.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string mensaje = socioBLL.GenerarMensajeRetencion(socioRiesgo);
                Clipboard.SetText(mensaje);
                DialogResult canal = MessageBox.Show(
                    $"Socio seleccionado: {socioRiesgo.Socio} (DNI {socioRiesgo.DNI})\r\n" +
                    $"Estado: {socioRiesgo.NivelRiesgo} ({socioRiesgo.ProbabilidadFuga})\r\n" +
                    $"Tel: {(string.IsNullOrWhiteSpace(socioRiesgo.Telefono) ? "No registrado" : socioRiesgo.Telefono)} | Email: {(string.IsNullOrWhiteSpace(socioRiesgo.Email) ? "No registrado" : socioRiesgo.Email)}\r\n\r\n" +
                    $"El mensaje de retención ya fue copiado al portapapeles.\r\n" +
                    $"¿Desea abrir WhatsApp Web (SÍ) o enviar un Correo Electrónico (NO)?\r\n" +
                    $"Presione Cancelar para abortar.",
                    "Gestión de Retención de Clientes (Churn)",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (canal == DialogResult.Yes)
                {
                    string soloDigitos = new string((socioRiesgo.Telefono ?? string.Empty).Where(char.IsDigit).ToArray());
                    if (soloDigitos.Length == 10)
                    {
                        soloDigitos = "549" + soloDigitos;
                    }
                    string urlWhatsApp = !string.IsNullOrWhiteSpace(soloDigitos)
                        ? $"https://wa.me/{soloDigitos}?text={Uri.EscapeDataString(mensaje)}"
                        : $"https://web.whatsapp.com/";
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlWhatsApp,
                        UseShellExecute = true
                    });
                    registroBLL.RegistrarEvento($"Gestión de retención por WhatsApp iniciada para socio DNI {socioRiesgo.DNI} ({socioRiesgo.NivelRiesgo}).", usuarioActual, "INFO");
                }
                else if (canal == DialogResult.No)
                {
                    string destinoEmail = !string.IsNullOrWhiteSpace(socioRiesgo.Email) ? socioRiesgo.Email.Trim() : "";
                    string asunto = Uri.EscapeDataString("UAI-FIT - Beneficio de Renovación y Seguimiento Deportivo");
                    string cuerpo = Uri.EscapeDataString(mensaje);
                    string urlMailto = $"mailto:{destinoEmail}?subject={asunto}&body={cuerpo}";
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlMailto,
                        UseShellExecute = true
                    });
                    registroBLL.RegistrarEvento($"Gestión de retención por Email iniciada para socio DNI {socioRiesgo.DNI} ({socioRiesgo.NivelRiesgo}).", usuarioActual, "INFO");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El borrador quedó copiado en su portapapeles, pero ocurrió un aviso al abrir el navegador/cliente: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
            CargarDatosDashboard();
            lblBienvenida_Base.Text = T("lblBienvenida_Base", "Sesión activa - Bienvenido, ") + usuarioActual.Nombre;
        }

        private void AplicarPermisosVisuales()
        {
            bool veBitacora = usuarioActual.TienePermiso("CONTROL_BITACORA");
            btnGestionRoles.Visible = usuarioActual.TienePermiso("GESTION_ROLES");
            btnControlCambios.Visible = veBitacora;
            panel1.Visible = veBitacora;
            dgvLogs.Visible = veBitacora;
            btnBackup.Visible = usuarioActual.TienePermiso("REALIZAR_BACKUP");
            bool veRec = usuarioActual.TienePermiso("VENDER_PLAN") || usuarioActual.TienePermiso("ASENTAR_TURNO") || usuarioActual.TienePermiso("REVISAR_AGENDA") || usuarioActual.TienePermiso("GESTION_ROLES");
            bool veEnt = usuarioActual.TienePermiso("ASIGNAR_RUTINA") || usuarioActual.TienePermiso("CONSULTAR_SOCIO") || usuarioActual.TienePermiso("GESTION_ROLES");
            bool veNut = usuarioActual.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL") || usuarioActual.TienePermiso("CONSULTAR_AGENDA") || usuarioActual.TienePermiso("CONSULTAR_PLANES_NUT") || usuarioActual.TienePermiso("GESTION_ROLES");
            lblKpiRecepcion.Visible = veRec;
            lblKpiEntrenamiento.Visible = veEnt;
            lblKpiNutricion.Visible = veNut;
            btnVistaChurn.Visible = veRec || veEnt;
            if (veBitacora)
            {
                pnlDashboardAlertas.Location = new Point(28, 68);
                pnlDashboardAlertas.Height = 226;
                dgvAlertas.Height = 174;
            }
            else
            {
                pnlDashboardAlertas.Location = new Point(28, 68);
                pnlDashboardAlertas.Height = 656;
                dgvAlertas.Height = 602;
            }
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

        private void AbrirSubFormularioEnContenedor(Form subForm)
        {
            Control contenedor = this.Parent;
            if (contenedor != null)
            {
                subForm.TopLevel = false;
                subForm.FormBorderStyle = FormBorderStyle.None;
                subForm.Dock = DockStyle.Fill;
                contenedor.Controls.Add(subForm);
                subForm.FormClosed += (s, args) =>
                {
                    if (!this.IsDisposed)
                    {
                        this.Show();
                        this.BringToFront();
                    }
                };
                this.Hide();
                subForm.BringToFront();
                subForm.Show();
            }
            else
            {
                subForm.ShowDialog();
            }
        }

        private void btnControlCambios_Click(object sender, EventArgs e)
        {
            AbrirSubFormularioEnContenedor(new frmControlCambios());
        }

        private void btnGestionRoles_Click(object sender, EventArgs e)
        {
            AbrirSubFormularioEnContenedor(new frmGestionRoles());
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