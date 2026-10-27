using BLL;

namespace GUI
{
    internal sealed class frmAdministradorInicial : Form
    {
        public frmAdministradorInicial()
        {
            Text = "Crear administrador inicial — UAI-FIT";
            ClientSize = new Size(490, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            var info = new Label { Left = 20, Top = 15, Width = 450, Height = 55, Text = "La base está preparada. Cree su cuenta de administrador. No existe una contraseña predeterminada." };
            var nombre = new TextBox { Left = 200, Top = 90, Width = 260, MaxLength = 100 };
            var clave = new TextBox { Left = 200, Top = 130, Width = 260, UseSystemPasswordChar = true };
            var repetir = new TextBox { Left = 200, Top = 170, Width = 260, UseSystemPasswordChar = true };
            var crear = new Button { Left = 200, Top = 225, Width = 260, Height = 35, Text = "Crear administrador" };
            Controls.AddRange(new Control[] { info,
                new Label { Left = 20, Top = 93, Width = 170, Text = "Usuario (4–100 caracteres)" }, nombre,
                new Label { Left = 20, Top = 133, Width = 175, Text = "Contraseña (mínimo 8)" }, clave,
                new Label { Left = 20, Top = 173, Width = 170, Text = "Repetir contraseña" }, repetir, crear });
            AcceptButton = crear;
            crear.Click += (_, _) =>
            {
                if (clave.Text != repetir.Text) { MessageBox.Show(this, "Las contraseñas no coinciden."); return; }
                crear.Enabled = false;
                try
                {
                    new UsuarioBLL().RegistrarAdministradorInicial(nombre.Text.Trim(), clave.Text);
                    clave.Clear(); repetir.Clear();
                    MessageBox.Show(this, "Administrador creado. Inicie sesión con su cuenta.", "UAI-FIT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    InstaladorBLL.RegistrarLog("Administrador inicial no creado: " + ex.Message);
                    MessageBox.Show(this, ex.Message, "No se creó la cuenta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally { crear.Enabled = true; }
            };
        }
    }
}
