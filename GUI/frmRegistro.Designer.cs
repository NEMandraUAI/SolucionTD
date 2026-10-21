using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI
{
    partial class frmRegistro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlCardRegistro = new Panel();
            pnlAccentTop = new Panel();
            picLogoMarca = new PictureBox();
            lblBrandTitulo = new Label();
            txtUsuario = new TextBox();
            txtClave1 = new TextBox();
            txtClave2 = new TextBox();
            btnRegistrar = new Button();
            lblUsuario = new Label();
            lblClave1 = new Label();
            lblClave2 = new Label();
            btnVerClave = new Button();
            pnlCardRegistro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoMarca).BeginInit();
            SuspendLayout();
            // 
            // pnlCardRegistro
            // 
            pnlCardRegistro.BackColor = Color.White;
            pnlCardRegistro.BorderStyle = BorderStyle.FixedSingle;
            pnlCardRegistro.Controls.Add(pnlAccentTop);
            pnlCardRegistro.Controls.Add(picLogoMarca);
            pnlCardRegistro.Controls.Add(lblBrandTitulo);
            pnlCardRegistro.Controls.Add(lblUsuario);
            pnlCardRegistro.Controls.Add(txtUsuario);
            pnlCardRegistro.Controls.Add(lblClave1);
            pnlCardRegistro.Controls.Add(txtClave1);
            pnlCardRegistro.Controls.Add(lblClave2);
            pnlCardRegistro.Controls.Add(txtClave2);
            pnlCardRegistro.Controls.Add(btnVerClave);
            pnlCardRegistro.Controls.Add(btnRegistrar);
            pnlCardRegistro.Location = new Point(40, 20);
            pnlCardRegistro.Name = "pnlCardRegistro";
            pnlCardRegistro.Size = new Size(420, 560);
            pnlCardRegistro.TabIndex = 0;
            // 
            // pnlAccentTop
            // 
            pnlAccentTop.BackColor = Color.FromArgb(16, 185, 129);
            pnlAccentTop.Dock = DockStyle.Top;
            pnlAccentTop.Location = new Point(0, 0);
            pnlAccentTop.Name = "pnlAccentTop";
            pnlAccentTop.Size = new Size(418, 6);
            pnlAccentTop.TabIndex = 0;
            // 
            // picLogoMarca
            // 
            picLogoMarca.BackColor = Color.Transparent;
            picLogoMarca.Image = GenerarEmblemaRegistroUaiFit();
            picLogoMarca.Location = new Point(166, 20);
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
            lblBrandTitulo.Location = new Point(20, 112);
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
            lblUsuario.Location = new Point(50, 152);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(125, 17);
            lblUsuario.TabIndex = 4;
            lblUsuario.Text = "Nombre de Usuario";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(248, 250, 252);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            txtUsuario.Location = new Point(50, 176);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(320, 26);
            txtUsuario.TabIndex = 0;
            // 
            // lblClave1
            // 
            lblClave1.AutoSize = true;
            lblClave1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblClave1.ForeColor = Color.FromArgb(31, 41, 55);
            lblClave1.Location = new Point(50, 220);
            lblClave1.Name = "lblClave1";
            lblClave1.Size = new Size(76, 17);
            lblClave1.TabIndex = 5;
            lblClave1.Text = "Contraseña";
            // 
            // txtClave1
            // 
            txtClave1.BackColor = Color.FromArgb(248, 250, 252);
            txtClave1.BorderStyle = BorderStyle.FixedSingle;
            txtClave1.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtClave1.ForeColor = Color.FromArgb(31, 41, 55);
            txtClave1.Location = new Point(50, 244);
            txtClave1.Name = "txtClave1";
            txtClave1.PasswordChar = '*';
            txtClave1.Size = new Size(320, 26);
            txtClave1.TabIndex = 1;
            // 
            // lblClave2
            // 
            lblClave2.AutoSize = true;
            lblClave2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblClave2.ForeColor = Color.FromArgb(31, 41, 55);
            lblClave2.Location = new Point(50, 288);
            lblClave2.Name = "lblClave2";
            lblClave2.Size = new Size(125, 17);
            lblClave2.TabIndex = 6;
            lblClave2.Text = "Repetir Contraseña";
            // 
            // txtClave2
            // 
            txtClave2.BackColor = Color.FromArgb(248, 250, 252);
            txtClave2.BorderStyle = BorderStyle.FixedSingle;
            txtClave2.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtClave2.ForeColor = Color.FromArgb(31, 41, 55);
            txtClave2.Location = new Point(50, 312);
            txtClave2.Name = "txtClave2";
            txtClave2.PasswordChar = '*';
            txtClave2.Size = new Size(320, 26);
            txtClave2.TabIndex = 2;
            // 
            // btnVerClave
            // 
            btnVerClave.BackColor = Color.FromArgb(71, 85, 105);
            btnVerClave.FlatAppearance.BorderSize = 0;
            btnVerClave.FlatStyle = FlatStyle.Flat;
            btnVerClave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnVerClave.ForeColor = Color.White;
            btnVerClave.Location = new Point(50, 368);
            btnVerClave.Name = "btnVerClave";
            btnVerClave.Size = new Size(320, 44);
            btnVerClave.TabIndex = 3;
            btnVerClave.Text = "Ver Clave";
            btnVerClave.UseVisualStyleBackColor = false;
            btnVerClave.Click += btnVerClave_Click;
            // 
            // btnRegistrar
            // 
            btnRegistrar.BackColor = Color.FromArgb(37, 99, 235);
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(50, 428);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(320, 48);
            btnRegistrar.TabIndex = 4;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // frmRegistro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(500, 600);
            Controls.Add(pnlCardRegistro);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmRegistro";
            Text = "Registro";
            Load += (s, e) => CentrarTarjetaRegistro();
            Load += frmRegistro_Load;
            Resize += (s, e) => CentrarTarjetaRegistro();
            pnlCardRegistro.ResumeLayout(false);
            pnlCardRegistro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoMarca).EndInit();
            ResumeLayout(false);
        }

        private void CentrarTarjetaRegistro()
        {
            if (pnlCardRegistro == null) return;
            pnlCardRegistro.Left = Math.Max(20, (this.ClientSize.Width - pnlCardRegistro.Width) / 2);
            pnlCardRegistro.Top = Math.Max(20, (this.ClientSize.Height - pnlCardRegistro.Height) / 2);
        }

        private static Bitmap GenerarEmblemaRegistroUaiFit()
        {
            Bitmap bmp = new Bitmap(160, 160);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (LinearGradientBrush brFondo = new LinearGradientBrush(new Rectangle(8, 8, 144, 144), Color.FromArgb(22, 101, 52), Color.FromArgb(16, 185, 129), 45f))
                {
                    g.FillEllipse(brFondo, 8, 8, 144, 144);
                }
                using (Pen penAro = new Pen(Color.FromArgb(167, 243, 208), 3.5f))
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

        private Panel pnlCardRegistro;
        private Panel pnlAccentTop;
        private PictureBox picLogoMarca;
        private Label lblBrandTitulo;
        private TextBox txtUsuario;
        private TextBox txtClave1;
        private TextBox txtClave2;
        private Button btnRegistrar;
        private Label lblUsuario;
        private Label lblClave1;
        private Label lblClave2;
        private Button btnVerClave;
    }
}