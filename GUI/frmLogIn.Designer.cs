using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI
{
    partial class frmLogIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlCardAuth = new Panel();
            pnlAccentTop = new Panel();
            picLogoMarca = new PictureBox();
            lblBrandTitulo = new Label();
            txtUsuario = new TextBox();
            txtClave = new TextBox();
            btnIniciarSesion = new Button();
            lblUsuario = new Label();
            lblClave = new Label();
            btnVerClave = new Button();
            pnlCardAuth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoMarca).BeginInit();
            SuspendLayout();
            // 
            // pnlCardAuth
            // 
            pnlCardAuth.BackColor = Color.White;
            pnlCardAuth.BorderStyle = BorderStyle.FixedSingle;
            pnlCardAuth.Controls.Add(pnlAccentTop);
            pnlCardAuth.Controls.Add(picLogoMarca);
            pnlCardAuth.Controls.Add(lblBrandTitulo);
            pnlCardAuth.Controls.Add(lblUsuario);
            pnlCardAuth.Controls.Add(txtUsuario);
            pnlCardAuth.Controls.Add(lblClave);
            pnlCardAuth.Controls.Add(txtClave);
            pnlCardAuth.Controls.Add(btnVerClave);
            pnlCardAuth.Controls.Add(btnIniciarSesion);
            pnlCardAuth.Location = new Point(40, 24);
            pnlCardAuth.Name = "pnlCardAuth";
            pnlCardAuth.Size = new Size(420, 496);
            pnlCardAuth.TabIndex = 0;
            // 
            // pnlAccentTop
            // 
            pnlAccentTop.BackColor = Color.FromArgb(37, 99, 235);
            pnlAccentTop.Dock = DockStyle.Top;
            pnlAccentTop.Location = new Point(0, 0);
            pnlAccentTop.Name = "pnlAccentTop";
            pnlAccentTop.Size = new Size(418, 6);
            pnlAccentTop.TabIndex = 0;
            // 
            // picLogoMarca
            // 
            picLogoMarca.BackColor = Color.Transparent;
            picLogoMarca.Image = GenerarEmblemaUaiFit();
            picLogoMarca.Location = new Point(166, 24);
            picLogoMarca.Name = "picLogoMarca";
            picLogoMarca.Size = new Size(88, 88);
            picLogoMarca.SizeMode = PictureBoxSizeMode.Zoom;
            picLogoMarca.TabIndex = 1;
            picLogoMarca.TabStop = false;
            // 
            // lblBrandTitulo
            // 
            lblBrandTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblBrandTitulo.ForeColor = Color.FromArgb(31, 41, 55);
            lblBrandTitulo.Location = new Point(20, 118);
            lblBrandTitulo.Name = "lblBrandTitulo";
            lblBrandTitulo.Size = new Size(380, 28);
            lblBrandTitulo.TabIndex = 2;
            lblBrandTitulo.Text = "UAI-FIT";
            lblBrandTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            lblUsuario.Location = new Point(50, 162);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(54, 17);
            lblUsuario.TabIndex = 3;
            lblUsuario.Text = "Usuario";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(248, 250, 252);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            txtUsuario.Location = new Point(50, 186);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(320, 26);
            txtUsuario.TabIndex = 0;
            // 
            // lblClave
            // 
            lblClave.AutoSize = true;
            lblClave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblClave.ForeColor = Color.FromArgb(31, 41, 55);
            lblClave.Location = new Point(50, 234);
            lblClave.Name = "lblClave";
            lblClave.Size = new Size(76, 17);
            lblClave.TabIndex = 4;
            lblClave.Text = "Contraseña";
            // 
            // txtClave
            // 
            txtClave.BackColor = Color.FromArgb(248, 250, 252);
            txtClave.BorderStyle = BorderStyle.FixedSingle;
            txtClave.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtClave.ForeColor = Color.FromArgb(31, 41, 55);
            txtClave.Location = new Point(50, 258);
            txtClave.Name = "txtClave";
            txtClave.PasswordChar = '*';
            txtClave.Size = new Size(320, 26);
            txtClave.TabIndex = 1;
            // 
            // btnVerClave
            // 
            btnVerClave.BackColor = Color.FromArgb(71, 85, 105);
            btnVerClave.FlatAppearance.BorderSize = 0;
            btnVerClave.FlatStyle = FlatStyle.Flat;
            btnVerClave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnVerClave.ForeColor = Color.White;
            btnVerClave.Location = new Point(50, 316);
            btnVerClave.Name = "btnVerClave";
            btnVerClave.Size = new Size(320, 44);
            btnVerClave.TabIndex = 2;
            btnVerClave.Text = "Ver Clave";
            btnVerClave.UseVisualStyleBackColor = false;
            btnVerClave.Click += btnVerClave_Click;
            // 
            // btnIniciarSesion
            // 
            btnIniciarSesion.BackColor = Color.FromArgb(37, 99, 235);
            btnIniciarSesion.FlatAppearance.BorderSize = 0;
            btnIniciarSesion.FlatStyle = FlatStyle.Flat;
            btnIniciarSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnIniciarSesion.ForeColor = Color.White;
            btnIniciarSesion.Location = new Point(50, 378);
            btnIniciarSesion.Name = "btnIniciarSesion";
            btnIniciarSesion.Size = new Size(320, 48);
            btnIniciarSesion.TabIndex = 3;
            btnIniciarSesion.Text = "Iniciar Sesión";
            btnIniciarSesion.UseVisualStyleBackColor = false;
            btnIniciarSesion.Click += btnIniciarSesion_Click_1;
            // 
            // frmLogIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(500, 544);
            Controls.Add(pnlCardAuth);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmLogIn";
            Text = "LogIn";
            FormClosing += frmLogIn_FormClosing;
            Load += (s, e) => CentrarTarjetaLogin();
            Load += frmLogIn_Load;
            Resize += (s, e) => CentrarTarjetaLogin();
            pnlCardAuth.ResumeLayout(false);
            pnlCardAuth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoMarca).EndInit();
            ResumeLayout(false);
        }

        private void CentrarTarjetaLogin()
        {
            if (pnlCardAuth == null) return;
            pnlCardAuth.Left = Math.Max(20, (this.ClientSize.Width - pnlCardAuth.Width) / 2);
            pnlCardAuth.Top = Math.Max(20, (this.ClientSize.Height - pnlCardAuth.Height) / 2);
        }

        private static Bitmap GenerarEmblemaUaiFit()
        {
            Bitmap bmp = new Bitmap(160, 160);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (LinearGradientBrush brFondo = new LinearGradientBrush(new Rectangle(8, 8, 144, 144), Color.FromArgb(30, 58, 138), Color.FromArgb(37, 99, 235), 45f))
                {
                    g.FillEllipse(brFondo, 8, 8, 144, 144);
                }
                using (Pen penAro = new Pen(Color.FromArgb(147, 197, 253), 3.5f))
                {
                    g.DrawEllipse(penAro, 16, 16, 128, 128);
                }
                using (SolidBrush brBlanco = new SolidBrush(Color.White))
                {
                    g.FillRectangle(brBlanco, 50, 75, 60, 10);
                    g.FillRectangle(brBlanco, 38, 52, 11, 56);
                    g.FillRectangle(brBlanco, 26, 62, 9, 36);
                    g.FillRectangle(brBlanco, 111, 52, 11, 56);
                    g.FillRectangle(brBlanco, 125, 62, 9, 36);
                }
            }
            return bmp;
        }

        #endregion

        private Panel pnlCardAuth;
        private Panel pnlAccentTop;
        private PictureBox picLogoMarca;
        private Label lblBrandTitulo;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private Button btnIniciarSesion;
        private Label lblUsuario;
        private Label lblClave;
        private Button btnVerClave;
    }
}