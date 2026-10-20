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
    public partial class frmRegistro : Form, IObserverIdioma
    {
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        public frmRegistro()
        {
            InitializeComponent();
            txtClave1.PasswordChar = '*';
            txtClave2.PasswordChar = '*';
            btnVerClave.Text = T("btnVerClave_Text", "Ver Clave");
            GestorIdioma.Instancia.Suscribir(this);
            this.FormClosing += (s, e) => GestorIdioma.Instancia.Desuscribir(this);
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave1.Text) || string.IsNullOrWhiteSpace(txtClave2.Text))
            {
                MessageBox.Show(T("msgCamposIncompletos", "Debe completar todos los campos."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtClave1.Text != txtClave2.Text)
            {
                MessageBox.Show(T("msgClavesNoCoinciden", "Las contraseñas no coinciden."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                usuarioBLL.RegistrarUsuario(txtUsuario.Text, txtClave1.Text);
                MessageBox.Show(T("msgRegistroExito", "Usuario registrado con éxito."), T("titRegistro", "Registro"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnVerClave_Click(object sender, EventArgs e)
        {
            if (txtClave1.PasswordChar == '*' && txtClave2.PasswordChar == '*')
            {
                txtClave1.PasswordChar = '\0';
                txtClave2.PasswordChar = '\0';
                btnVerClave.Text = T("btnOcultarClave", "Ocultar Clave");
            }
            else
            {
                txtClave1.PasswordChar = '*';
                txtClave2.PasswordChar = '*';
                btnVerClave.Text = T("btnVerClave_Text", "Ver Clave");
            }
        }
        private void frmRegistro_Load(object sender, EventArgs e)
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
            btnVerClave.Text = (txtClave1.PasswordChar == '*' && txtClave2.PasswordChar == '*')
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
    }
}
