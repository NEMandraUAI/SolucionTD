using BE;
using BLL;
using GUI.Eventos;
using Seguridad;

namespace GUI
{
    public partial class frmLogIn : Form, IObserverIdioma
    {
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public event EventHandler<LogInEventArgs> LoginExitoso;
        public frmLogIn()
        {
            InitializeComponent();
            txtClave.PasswordChar = '*';
            btnVerClave.Text = T("btnVerClave_Text", "Ver Clave");
            GestorIdioma.Instancia.Suscribir(this);
        }
        private void btnIniciarSesion_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show(T("msgCamposIncompletos", "Por favor, complete ambos campos para iniciar sesión."), T("titCamposIncompletos", "Campos Incompletos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                UsuarioBE usuarioLogueado = usuarioBLL.IniciarSesion(txtUsuario.Text, txtClave.Text);
                try
                {
                    LoginExitoso?.Invoke(this, new LogInEventArgs(usuarioLogueado));
                }
                catch (Exception ex)
                {
                    SessionManager.Instancia.Cerrar();
                    InstaladorBLL.RegistrarLog("La autenticacion se confirmo, pero fallo la pantalla de sesion: " + ex.Message);
                    throw new InvalidOperationException("No se pudo abrir la pantalla de sesion. La sesion se cerro; vuelva a intentarlo.", ex);
                }
            }
            catch (Exception ex)
            {
                string mensajeTraducido = T(ex.Message, ex.Message);
                if (ex.Message.StartsWith("Usuario o clave incorrectos. Le quedan "))
                {
                    string digitos = new string(ex.Message.Where(char.IsDigit).ToArray());
                    mensajeTraducido = T("msgIntentosRestantes", "Usuario o clave incorrectos. Intentos restantes: ") + digitos;
                }
                MessageBox.Show(mensajeTraducido, T("titErrorAuth", "Error de Autenticación"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnVerClave_Click(object sender, EventArgs e)
        {
            if (txtClave.PasswordChar == '*')
            {
                txtClave.PasswordChar = '\0';
                btnVerClave.Text = T("btnOcultarClave", "Ocultar Clave");
            }
            else
            {
                txtClave.PasswordChar = '*';
                btnVerClave.Text = T("btnVerClave_Text", "Ver Clave");
            }
        }
        private void frmLogIn_Load(object sender, EventArgs e)
        {
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }
        public void ActualizarIdioma(IdiomaBE idioma)
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey(this.Name))
                this.Text = _traducciones[this.Name];
            TraducirControlesRecursivo(this.Controls, _traducciones);
            btnVerClave.Text = txtClave.PasswordChar == '*'
                ? T("btnVerClave_Text", "Ver Clave")
                : T("btnOcultarClave", "Ocultar Clave");
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
        private void frmLogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
