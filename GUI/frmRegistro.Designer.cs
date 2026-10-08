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
            txtUsuario = new TextBox();
            txtClave1 = new TextBox();
            txtClave2 = new TextBox();
            btnRegistrar = new Button();
            lblUsuario = new Label();
            lblClave1 = new Label();
            lblClave2 = new Label();
            btnVerClave = new Button();
            SuspendLayout();
            // 
            // txtUsuario
            // 
            txtUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUsuario.BackColor = Color.White;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            txtUsuario.Location = new Point(36, 62);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(288, 23);
            txtUsuario.TabIndex = 0;
            // 
            // txtClave1
            // 
            txtClave1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtClave1.BackColor = Color.White;
            txtClave1.BorderStyle = BorderStyle.FixedSingle;
            txtClave1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtClave1.ForeColor = Color.FromArgb(31, 41, 55);
            txtClave1.Location = new Point(36, 144);
            txtClave1.Name = "txtClave1";
            txtClave1.PasswordChar = '*';
            txtClave1.Size = new Size(288, 23);
            txtClave1.TabIndex = 1;
            // 
            // txtClave2
            // 
            txtClave2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtClave2.BackColor = Color.White;
            txtClave2.BorderStyle = BorderStyle.FixedSingle;
            txtClave2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtClave2.ForeColor = Color.FromArgb(31, 41, 55);
            txtClave2.Location = new Point(36, 226);
            txtClave2.Name = "txtClave2";
            txtClave2.PasswordChar = '*';
            txtClave2.Size = new Size(288, 23);
            txtClave2.TabIndex = 2;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnRegistrar.BackColor = Color.FromArgb(37, 99, 235);
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(36, 338);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(288, 46);
            btnRegistrar.TabIndex = 3;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            lblUsuario.Location = new Point(36, 36);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(110, 15);
            lblUsuario.TabIndex = 4;
            lblUsuario.Text = "Nombre de Usuario";
            // 
            // lblClave1
            // 
            lblClave1.AutoSize = true;
            lblClave1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblClave1.ForeColor = Color.FromArgb(31, 41, 55);
            lblClave1.Location = new Point(36, 118);
            lblClave1.Name = "lblClave1";
            lblClave1.Size = new Size(67, 15);
            lblClave1.TabIndex = 5;
            lblClave1.Text = "Contraseña";
            // 
            // lblClave2
            // 
            lblClave2.AutoSize = true;
            lblClave2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblClave2.ForeColor = Color.FromArgb(31, 41, 55);
            lblClave2.Location = new Point(36, 200);
            lblClave2.Name = "lblClave2";
            lblClave2.Size = new Size(107, 15);
            lblClave2.TabIndex = 6;
            lblClave2.Text = "Repetir Contraseña";
            // 
            // btnVerClave
            // 
            btnVerClave.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnVerClave.BackColor = Color.FromArgb(71, 85, 105);
            btnVerClave.FlatAppearance.BorderSize = 0;
            btnVerClave.FlatStyle = FlatStyle.Flat;
            btnVerClave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnVerClave.ForeColor = Color.White;
            btnVerClave.Location = new Point(36, 274);
            btnVerClave.Name = "btnVerClave";
            btnVerClave.Size = new Size(288, 46);
            btnVerClave.TabIndex = 7;
            btnVerClave.Text = "Ver Clave";
            btnVerClave.UseVisualStyleBackColor = false;
            btnVerClave.Click += btnVerClave_Click;
            // 
            // frmRegistro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(360, 400);
            Controls.Add(btnVerClave);
            Controls.Add(lblClave2);
            Controls.Add(lblClave1);
            Controls.Add(lblUsuario);
            Controls.Add(btnRegistrar);
            Controls.Add(txtClave2);
            Controls.Add(txtClave1);
            Controls.Add(txtUsuario);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmRegistro";
            Text = "Registro";
            Load += frmRegistro_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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