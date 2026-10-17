using BE;
using BLL;
using GUI.Eventos;
using Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmMenu : Form, IObserverIdioma
    {
        private MdiClient mdiClient;
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private Form formularioActivo = null;

        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }

        public frmMenu()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += FrmMenu_KeyDown;
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            SessionManager.Instancia.PermisosActualizados += SessionManager_PermisosActualizados;
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void AbrirFormularioHijo(Form formHijo)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }
            formularioActivo = formHijo;
            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(formHijo);
            pnlContenedor.Tag = formHijo;
            formHijo.BringToFront();
            formHijo.Show();
        }

        private void FrmMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.R)
            {
                try
                {
                    IntegridadBLL integridadBLL = new IntegridadBLL();
                    integridadBLL.VerificarIntegridadSistema();
                    return;
                }
                catch { }
                string claveIngresada = SolicitarClaveEmergencia();
                if (!string.IsNullOrEmpty(claveIngresada))
                {
                    string hashClaveMaestra = "3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2";
                    if (CryptoManager.Comparar(claveIngresada, hashClaveMaestra))
                    {
                        DialogResult opcion = MessageBox.Show(
                        T("msgDialogoRecuperacion", "Autenticación Maestra correcta.\n\n¿Desea RECALCULAR toda la estructura de dígitos (SÍ) o RESTAURAR un archivo de Backup (NO)?\n\nPresione Cancelar para abortar la operación."),
                        T("titDialogoRecuperacion", "Opciones de Recuperación de Emergencia"),
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
                        if (opcion == DialogResult.Yes)
                        {
                            try
                            {
                                IntegridadBLL integridadBLL = new IntegridadBLL();
                                integridadBLL.ForzarRecalculoDeTodaLaBase();
                                MessageBox.Show(T("msgRecuperacionExito", "Integridad restaurada exitosamente. Los dígitos verificadores han sido recalculados."), T("titRecuperacionExito", "Recuperación Exitosa"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (SessionManager.Instancia.UsuarioActual == null)
                                {
                                    AbrirLogin();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(T("msgRecuperacionError", "Ocurrió un error al intentar recalcular: ") + ex.Message, T("titErrorCritico", "Error Crítico"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (opcion == DialogResult.No)
                        {
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Filter = T("FiltroRestore", "Archivos de Backup SQL (*.bak)|*.bak");
                            ofd.Title = T("titRestore", "Seleccione el archivo de respaldo para restaurar");
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    BackupBLL backupBLL = new BackupBLL();
                                    backupBLL.RestaurarBackup(ofd.FileName);
                                    MessageBox.Show(T("msgRestauracionExito", "El sistema se ha restaurado correctamente al punto en el tiempo seleccionado. Por favor, inicie sesión nuevamente."), T("titRestauracionExito", "Restauración Exitosa"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    if (SessionManager.Instancia.UsuarioActual == null)
                                    {
                                        AbrirLogin();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(T("msgRestauracionError", "Ocurrió un error al intentar restaurar la base de datos: ") + ex.Message, T("titErrorCritico", "Error Crítico"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(T("msgClaveIncorrecta", "Clave maestra incorrecta. Acceso denegado."), T("titSeguridad", "Seguridad"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void AbrirLogin()
        {
            IntegridadBLL integridadBLL = new IntegridadBLL();
            try
            {
                integridadBLL.VerificarIntegridadSistema();
                frmLogIn login = new frmLogIn();
                login.LoginExitoso += OnLoginExitoso;
                AbrirFormularioHijo(login);
            }
            catch (Exception ex)
            {
                CerrarTodosLosFormularios();
                MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLoginExitoso(object sender, LogInEventArgs e)
        {
            CerrarTodosLosFormularios();
            frmSesion bienvenida = new frmSesion(e.Usuario);
            bienvenida.CerrarSesion += (s, args) => CerrarSesionGlobal();
            AbrirFormularioHijo(bienvenida);
            EvaluarPermisosIdioma();
            AplicarPermisosUI();
        }

        private string SolicitarClaveEmergencia()
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = T("titPromptRecuperacion", "Recuperación de Emergencia del Sistema"),
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label lblTexto = new Label() { Left = 20, Top = 20, Width = 300, Text = T("lblPromptRecuperacion", "Ingrese la Clave Maestra de Administrador:") };
            TextBox txtClave = new TextBox() { Left = 20, Top = 50, Width = 290, PasswordChar = '*' };
            Button btnAceptar = new Button() { Text = T("btnPromptAceptar", "Aceptar"), Left = 210, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            prompt.Controls.Add(lblTexto);
            prompt.Controls.Add(txtClave);
            prompt.Controls.Add(btnAceptar);
            prompt.AcceptButton = btnAceptar;
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                return txtClave.Text;
            }
            return "";
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            cmbIdiomas.SelectedIndexChanged -= cmbIdiomas_SelectedIndexChanged;
            cmbIdiomas.DataSource = idiomaBLL.ObtenerIdiomas();
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "ID";
            cmbIdiomas.SelectedValue = GestorIdioma.Instancia.IdiomaActual.ID;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            btnNuevoIdioma.Visible = false;
            btnNuevoIdioma.Text = T("btnNuevoIdioma", "Gestionar Idiomas");
            OcultarSubMenus();
            AplicarPermisosUI();
            if (SessionManager.Instancia.UsuarioActual == null)
            {
                AbrirLogin();
            }
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey(this.Name))
                this.Text = _traducciones[this.Name];
            TraducirControlesRecursivo(this.Controls, _traducciones);
            cmbIdiomas.SelectedIndexChanged -= cmbIdiomas_SelectedIndexChanged;
            cmbIdiomas.SelectedValue = idioma.ID;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
        }

        private void TraducirControlesRecursivo(Control.ControlCollection controles, Dictionary<string, string> traducciones)
        {
            foreach (Control control in controles)
            {
                if (traducciones.ContainsKey(control.Name))
                {
                    control.Text = traducciones[control.Name];
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

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdiomas.SelectedItem is IdiomaBE idiomaSeleccionado)
            {
                if (GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID == idiomaSeleccionado.ID)
                {
                    return;
                }
                GestorIdioma.Instancia.CambiarIdioma(idiomaSeleccionado);
                if (SessionManager.Instancia.UsuarioActual != null)
                {
                    UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
                    usuarioActual.Idioma = idiomaSeleccionado;
                    UsuarioBLL usuarioBLL = new UsuarioBLL();
                    usuarioBLL.ActualizarIdiomaUsuario(usuarioActual.ID, idiomaSeleccionado.ID);
                }
            }
        }

        public void EvaluarPermisosIdioma()
        {
            var u = SessionManager.Instancia.UsuarioActual;
            btnNuevoIdioma.Visible = (u != null && u.TienePermiso("GESTION_IDIOMAS"));
        }

        private void CerrarTodosLosFormularios()
        {
            foreach (Control c in pnlContenedor.Controls.OfType<Form>().ToList())
            {
                ((Form)c).Close();
            }
            formularioActivo = null;
            pnlContenedor.Controls.Clear();
        }

        public void AplicarPermisosUI()
        {
            bool haySesion = SessionManager.Instancia.UsuarioActual != null;
            btnMenuIniciarSesion.Visible = !haySesion;
            btnMenuRegistrarse.Visible = !haySesion;
            btnMenuCerrarSesion.Visible = haySesion;
            btnMenuInicio.Visible = haySesion;
            if (haySesion)
            {
                btnMenuRecepcion.Visible = SessionManager.Instancia.UsuarioActual.TienePermiso("VENDER_PLAN");
                btnMenuEntrenamiento.Visible = SessionManager.Instancia.UsuarioActual.TienePermiso("ASIGNAR_RUTINA");
                btnMenuNutricion.Visible = SessionManager.Instancia.UsuarioActual.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL");
            }
            else
            {
                btnMenuRecepcion.Visible = false;
                btnMenuEntrenamiento.Visible = false;
                btnMenuNutricion.Visible = false;
            }
            OcultarSubMenus();
        }

        private void frmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
            SessionManager.Instancia.PermisosActualizados -= SessionManager_PermisosActualizados;
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.CerrarSesion();
                MessageBox.Show(T("msgSesionCerrada", "La sesión se cerró correctamente."), T("titSesionCerrada", "Sesión Cerrada"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SessionManager_PermisosActualizados(object sender, EventArgs e)
        {
            AplicarPermisosUI();
            EvaluarPermisosIdioma();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            IntegridadBLL integridadBLL = new IntegridadBLL();
            try
            {
                integridadBLL.VerificarIntegridadSistema();
                if (SessionManager.Instancia.UsuarioActual != null)
                {
                    MessageBox.Show(T("msgSesionActiva", "Ya existe una sesión activa. Debe cerrarla antes de ingresar con otra cuenta."), T("titSesionActiva", "Sesión Activa"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                OcultarSubMenus();
                AbrirLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                CerrarTodosLosFormularios();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            IntegridadBLL integridadBLL = new IntegridadBLL();
            try
            {
                integridadBLL.VerificarIntegridadSistema();
                OcultarSubMenus();
                frmRegistro registro = new frmRegistro();
                AbrirFormularioHijo(registro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                CerrarTodosLosFormularios();
            }
        }

        private void btnNuevoIdioma_Click(object sender, EventArgs e)
        {
            frmGestionIdiomas frm = new frmGestionIdiomas();
            frm.FormClosed += (s, args) =>
            {
                IdiomaBLL idiomaBLL = new IdiomaBLL();
                cmbIdiomas.SelectedIndexChanged -= cmbIdiomas_SelectedIndexChanged;
                cmbIdiomas.DataSource = idiomaBLL.ObtenerIdiomas();
                cmbIdiomas.SelectedValue = GestorIdioma.Instancia.IdiomaActual.ID;
                cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
            };
            OcultarSubMenus();
            AbrirFormularioHijo(frm);
        }

        private void OcultarSubMenus()
        {
            pnlSubRecepcion.Visible = false;
            pnlSubEntrenamiento.Visible = false;
            pnlSubNutricion.Visible = false;
        }

        private void btnMenuRecepcion_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            pnlSubRecepcion.Visible = true;
            pnlSubRecepcion.BringToFront();
        }

        private void btnMenuEntrenamiento_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            pnlSubEntrenamiento.Visible = true;
            pnlSubEntrenamiento.BringToFront();
        }

        private void btnMenuNutricion_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            frmGestionNutricion frm = new frmGestionNutricion();
            AbrirFormularioHijo(frm);
        }

        private void btnMenuCerrarSesion_Click(object sender, EventArgs e)
        {
            CerrarSesionGlobal();
        }

        private void CerrarSesionGlobal()
        {
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.CerrarSesion();
                CerrarTodosLosFormularios();
                AplicarPermisosUI();
                EvaluarPermisosIdioma();
                BLL.IntegridadBLL integridadBLL = new BLL.IntegridadBLL();
                try
                {
                    integridadBLL.VerificarIntegridadSistema();
                    MessageBox.Show(T("msgSesionCerrada", "La sesión se cerró correctamente."), T("titSesionCerrada", "Sesión Cerrada"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AbrirLogin();
                }
                catch (Exception ex)
                {
                    CerrarTodosLosFormularios();
                    MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSubAsignarRutina_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmEntrenamiento());
        }

        private void btnSubGestionRutinas_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmGestionRutinas());
        }

        private void btnSubVenderPlan_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmRecepcion());
        }

        private void btnSubConsultarComprobantes_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmConsultarComprobantes());
        }

        private void btnSubAgendarTurno_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmAgendaRecepcion());
        }

        private void btnMenuInicio_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                frmSesion inicio = new frmSesion(SessionManager.Instancia.UsuarioActual);
                inicio.CerrarSesion += (s, args) => CerrarSesionGlobal();
                AbrirFormularioHijo(inicio);
            }
        }
    }
}