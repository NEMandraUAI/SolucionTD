namespace GUI
{
    partial class frmSesion
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
            lblBienvenida_Base = new Label();
            btnCerrarSesion = new Button();
            dgvLogs = new DataGridView();
            panel1 = new Panel();
            lblBuscar = new Label();
            lblCriticidad = new Label();
            lblHasta = new Label();
            lblUsuarios = new Label();
            lblDesde = new Label();
            btnLimpiar = new Button();
            btnFiltrar = new Button();
            txtBuscar = new TextBox();
            cmbUsuarios = new ComboBox();
            cmbCriticidad = new ComboBox();
            dtpHasta = new DateTimePicker();
            dtpDesde = new DateTimePicker();
            btnControlCambios = new Button();
            btnGestionRoles = new Button();
            btnBackup = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblBienvenida_Base
            // 
            lblBienvenida_Base.AutoSize = true;
            lblBienvenida_Base.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblBienvenida_Base.ForeColor = Color.FromArgb(31, 41, 55);
            lblBienvenida_Base.Location = new Point(28, 40);
            lblBienvenida_Base.Name = "lblBienvenida_Base";
            lblBienvenida_Base.Size = new Size(162, 15);
            lblBienvenida_Base.TabIndex = 0;
            lblBienvenida_Base.Text = "Sesión activa - Bienvenido/a, ";
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.BackColor = Color.FromArgb(71, 85, 105);
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(28, 88);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(280, 46);
            btnCerrarSesion.TabIndex = 1;
            btnCerrarSesion.Text = "Cerrar Sesion";
            btnCerrarSesion.UseVisualStyleBackColor = false;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // dgvLogs
            // 
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLogs.BackgroundColor = Color.White;
            dgvLogs.BorderStyle = BorderStyle.None;
            dgvLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLogs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLogs.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLogs.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.GridColor = Color.FromArgb(226, 232, 240);
            dgvLogs.Location = new Point(28, 220);
            dgvLogs.MultiSelect = false;
            dgvLogs.Name = "dgvLogs";
            dgvLogs.ReadOnly = true;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.RowTemplate.Height = 36;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.Size = new Size(1244, 486);
            dgvLogs.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lblBuscar);
            panel1.Controls.Add(lblCriticidad);
            panel1.Controls.Add(lblHasta);
            panel1.Controls.Add(lblUsuarios);
            panel1.Controls.Add(lblDesde);
            panel1.Controls.Add(btnLimpiar);
            panel1.Controls.Add(btnFiltrar);
            panel1.Controls.Add(txtBuscar);
            panel1.Controls.Add(cmbUsuarios);
            panel1.Controls.Add(cmbCriticidad);
            panel1.Controls.Add(dtpHasta);
            panel1.Controls.Add(dtpDesde);
            panel1.Location = new Point(340, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(932, 168);
            panel1.TabIndex = 3;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblBuscar.ForeColor = Color.FromArgb(31, 41, 55);
            lblBuscar.Location = new Point(16, 86);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(94, 15);
            lblBuscar.TabIndex = 10;
            lblBuscar.Text = "Buscar por Texto";
            // 
            // lblCriticidad
            // 
            lblCriticidad.AutoSize = true;
            lblCriticidad.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblCriticidad.ForeColor = Color.FromArgb(31, 41, 55);
            lblCriticidad.Location = new Point(472, 12);
            lblCriticidad.Name = "lblCriticidad";
            lblCriticidad.Size = new Size(58, 15);
            lblCriticidad.TabIndex = 9;
            lblCriticidad.Text = "Criticidad";
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblHasta.ForeColor = Color.FromArgb(31, 41, 55);
            lblHasta.Location = new Point(244, 12);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(37, 15);
            lblHasta.TabIndex = 8;
            lblHasta.Text = "Hasta";
            // 
            // lblUsuarios
            // 
            lblUsuarios.AutoSize = true;
            lblUsuarios.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblUsuarios.ForeColor = Color.FromArgb(31, 41, 55);
            lblUsuarios.Location = new Point(700, 12);
            lblUsuarios.Name = "lblUsuarios";
            lblUsuarios.Size = new Size(52, 15);
            lblUsuarios.TabIndex = 7;
            lblUsuarios.Text = "Usuarios";
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblDesde.ForeColor = Color.FromArgb(31, 41, 55);
            lblDesde.Location = new Point(16, 12);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 6;
            lblDesde.Text = "Desde";
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(71, 85, 105);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(700, 104);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(205, 44);
            btnLimpiar.TabIndex = 5;
            btnLimpiar.Text = "Limpiar Filtros";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnFiltrar
            // 
            btnFiltrar.BackColor = Color.FromArgb(37, 99, 235);
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(472, 104);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(205, 44);
            btnFiltrar.TabIndex = 4;
            btnFiltrar.Text = "Aplicar Filtros";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.BackColor = Color.White;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtBuscar.ForeColor = Color.FromArgb(31, 41, 55);
            txtBuscar.Location = new Point(16, 110);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(432, 23);
            txtBuscar.TabIndex = 4;
            // 
            // cmbUsuarios
            // 
            cmbUsuarios.BackColor = Color.White;
            cmbUsuarios.FlatStyle = FlatStyle.Flat;
            cmbUsuarios.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbUsuarios.ForeColor = Color.FromArgb(31, 41, 55);
            cmbUsuarios.FormattingEnabled = true;
            cmbUsuarios.Items.AddRange(new object[] { "Todos", "INFO", "ALERTA", "CRÍTICO" });
            cmbUsuarios.Location = new Point(700, 36);
            cmbUsuarios.Name = "cmbUsuarios";
            cmbUsuarios.Size = new Size(205, 28);
            cmbUsuarios.TabIndex = 3;
            // 
            // cmbCriticidad
            // 
            cmbCriticidad.BackColor = Color.White;
            cmbCriticidad.FlatStyle = FlatStyle.Flat;
            cmbCriticidad.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbCriticidad.ForeColor = Color.FromArgb(31, 41, 55);
            cmbCriticidad.FormattingEnabled = true;
            cmbCriticidad.Location = new Point(472, 36);
            cmbCriticidad.Name = "cmbCriticidad";
            cmbCriticidad.Size = new Size(205, 28);
            cmbCriticidad.TabIndex = 2;
            // 
            // dtpHasta
            // 
            dtpHasta.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpHasta.Location = new Point(244, 36);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(205, 28);
            dtpHasta.TabIndex = 1;
            // 
            // dtpDesde
            // 
            dtpDesde.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpDesde.Location = new Point(16, 36);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(205, 28);
            dtpDesde.TabIndex = 0;
            // 
            // btnControlCambios
            // 
            btnControlCambios.BackColor = Color.FromArgb(71, 85, 105);
            btnControlCambios.FlatAppearance.BorderSize = 0;
            btnControlCambios.FlatStyle = FlatStyle.Flat;
            btnControlCambios.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnControlCambios.ForeColor = Color.White;
            btnControlCambios.Location = new Point(1072, 744);
            btnControlCambios.Name = "btnControlCambios";
            btnControlCambios.Size = new Size(200, 48);
            btnControlCambios.TabIndex = 4;
            btnControlCambios.Text = "Control de Cambios";
            btnControlCambios.UseVisualStyleBackColor = false;
            btnControlCambios.Click += btnControlCambios_Click;
            // 
            // btnGestionRoles
            // 
            btnGestionRoles.BackColor = Color.FromArgb(71, 85, 105);
            btnGestionRoles.FlatAppearance.BorderSize = 0;
            btnGestionRoles.FlatStyle = FlatStyle.Flat;
            btnGestionRoles.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnGestionRoles.ForeColor = Color.White;
            btnGestionRoles.Location = new Point(856, 744);
            btnGestionRoles.Name = "btnGestionRoles";
            btnGestionRoles.Size = new Size(200, 48);
            btnGestionRoles.TabIndex = 5;
            btnGestionRoles.Text = "Gestionar Roles";
            btnGestionRoles.UseVisualStyleBackColor = false;
            btnGestionRoles.Click += btnGestionRoles_Click;
            // 
            // btnBackup
            // 
            btnBackup.BackColor = Color.FromArgb(37, 99, 235);
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBackup.ForeColor = Color.White;
            btnBackup.Location = new Point(640, 744);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(200, 48);
            btnBackup.TabIndex = 6;
            btnBackup.Text = "Generar Respaldo";
            btnBackup.UseVisualStyleBackColor = false;
            btnBackup.Click += btnBackup_Click;
            // 
            // frmSesion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1300, 820);
            Controls.Add(btnBackup);
            Controls.Add(btnGestionRoles);
            Controls.Add(btnControlCambios);
            Controls.Add(panel1);
            Controls.Add(dgvLogs);
            Controls.Add(btnCerrarSesion);
            Controls.Add(lblBienvenida_Base);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmSesion";
            Text = "Sesion";
            FormClosing += frmSesion_FormClosing;
            Load += Sesion_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida_Base;
        private Button btnCerrarSesion;
        private DataGridView dgvLogs;
        private Panel panel1;
        private Button btnFiltrar;
        private TextBox txtBuscar;
        private ComboBox cmbUsuarios;
        private ComboBox cmbCriticidad;
        private DateTimePicker dtpHasta;
        private DateTimePicker dtpDesde;
        private Label lblBuscar;
        private Label lblCriticidad;
        private Label lblHasta;
        private Label lblUsuarios;
        private Label lblDesde;
        private Button btnLimpiar;
        private Button btnControlCambios;
        private Button btnGestionRoles;
        private Button btnBackup;
    }
}