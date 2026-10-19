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
            pnlDashboardAlertas = new Panel();
            btnVistaAlertas = new Button();
            btnVistaChurn = new Button();
            lblKpiRecepcion = new Label();
            lblKpiEntrenamiento = new Label();
            lblKpiNutricion = new Label();
            btnContactarSocio = new Button();
            btnRefrescarAlertas = new Button();
            dgvAlertas = new DataGridView();
            panel1 = new Panel();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblCriticidad = new Label();
            cmbCriticidad = new ComboBox();
            lblUsuarios = new Label();
            cmbUsuarios = new ComboBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnFiltrar = new Button();
            btnLimpiar = new Button();
            dgvLogs = new DataGridView();
            btnControlCambios = new Button();
            btnGestionRoles = new Button();
            btnBackup = new Button();
            pnlDashboardAlertas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAlertas).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            SuspendLayout();
            // 
            // lblBienvenida_Base
            // 
            lblBienvenida_Base.AutoSize = true;
            lblBienvenida_Base.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblBienvenida_Base.ForeColor = Color.FromArgb(31, 41, 55);
            lblBienvenida_Base.Location = new Point(28, 22);
            lblBienvenida_Base.Name = "lblBienvenida_Base";
            lblBienvenida_Base.Size = new Size(254, 25);
            lblBienvenida_Base.TabIndex = 0;
            lblBienvenida_Base.Text = "Sesión activa - Bienvenido/a, ";
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarSesion.BackColor = Color.FromArgb(71, 85, 105);
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(1052, 16);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(220, 40);
            btnCerrarSesion.TabIndex = 1;
            btnCerrarSesion.Text = "Cerrar Sesion";
            btnCerrarSesion.UseVisualStyleBackColor = false;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // pnlDashboardAlertas
            // 
            pnlDashboardAlertas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDashboardAlertas.BackColor = Color.White;
            pnlDashboardAlertas.BorderStyle = BorderStyle.FixedSingle;
            pnlDashboardAlertas.Controls.Add(btnVistaAlertas);
            pnlDashboardAlertas.Controls.Add(btnVistaChurn);
            pnlDashboardAlertas.Controls.Add(lblKpiRecepcion);
            pnlDashboardAlertas.Controls.Add(lblKpiEntrenamiento);
            pnlDashboardAlertas.Controls.Add(lblKpiNutricion);
            pnlDashboardAlertas.Controls.Add(btnContactarSocio);
            pnlDashboardAlertas.Controls.Add(btnRefrescarAlertas);
            pnlDashboardAlertas.Controls.Add(dgvAlertas);
            pnlDashboardAlertas.Location = new Point(28, 68);
            pnlDashboardAlertas.Name = "pnlDashboardAlertas";
            pnlDashboardAlertas.Size = new Size(1244, 226);
            pnlDashboardAlertas.TabIndex = 7;
            // 
            // btnVistaAlertas
            // 
            btnVistaAlertas.BackColor = Color.FromArgb(31, 41, 55);
            btnVistaAlertas.FlatAppearance.BorderSize = 0;
            btnVistaAlertas.FlatStyle = FlatStyle.Flat;
            btnVistaAlertas.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            btnVistaAlertas.ForeColor = Color.White;
            btnVistaAlertas.Location = new Point(12, 6);
            btnVistaAlertas.Name = "btnVistaAlertas";
            btnVistaAlertas.Size = new Size(195, 28);
            btnVistaAlertas.TabIndex = 0;
            btnVistaAlertas.Text = "🔔 Alertas Operativas";
            btnVistaAlertas.UseVisualStyleBackColor = false;
            btnVistaAlertas.Click += btnVistaAlertas_Click;
            // 
            // btnVistaChurn
            // 
            btnVistaChurn.BackColor = Color.FromArgb(226, 232, 240);
            btnVistaChurn.FlatAppearance.BorderSize = 0;
            btnVistaChurn.FlatStyle = FlatStyle.Flat;
            btnVistaChurn.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            btnVistaChurn.ForeColor = Color.FromArgb(31, 41, 55);
            btnVistaChurn.Location = new Point(214, 6);
            btnVistaChurn.Name = "btnVistaChurn";
            btnVistaChurn.Size = new Size(225, 28);
            btnVistaChurn.TabIndex = 1;
            btnVistaChurn.Text = "📉 Socios en Riesgo (Churn)";
            btnVistaChurn.UseVisualStyleBackColor = false;
            btnVistaChurn.Click += btnVistaChurn_Click;
            // 
            // lblKpiRecepcion
            // 
            lblKpiRecepcion.BackColor = Color.FromArgb(239, 246, 255);
            lblKpiRecepcion.BorderStyle = BorderStyle.FixedSingle;
            lblKpiRecepcion.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpiRecepcion.ForeColor = Color.FromArgb(30, 58, 138);
            lblKpiRecepcion.Location = new Point(448, 7);
            lblKpiRecepcion.Name = "lblKpiRecepcion";
            lblKpiRecepcion.Size = new Size(154, 26);
            lblKpiRecepcion.TabIndex = 2;
            lblKpiRecepcion.Text = "Recepción: 0";
            lblKpiRecepcion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblKpiEntrenamiento
            // 
            lblKpiEntrenamiento.BackColor = Color.FromArgb(254, 243, 199);
            lblKpiEntrenamiento.BorderStyle = BorderStyle.FixedSingle;
            lblKpiEntrenamiento.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpiEntrenamiento.ForeColor = Color.FromArgb(146, 64, 14);
            lblKpiEntrenamiento.Location = new Point(608, 7);
            lblKpiEntrenamiento.Name = "lblKpiEntrenamiento";
            lblKpiEntrenamiento.Size = new Size(164, 26);
            lblKpiEntrenamiento.TabIndex = 3;
            lblKpiEntrenamiento.Text = "Entrenamiento: 0";
            lblKpiEntrenamiento.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblKpiNutricion
            // 
            lblKpiNutricion.BackColor = Color.FromArgb(220, 252, 231);
            lblKpiNutricion.BorderStyle = BorderStyle.FixedSingle;
            lblKpiNutricion.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpiNutricion.ForeColor = Color.FromArgb(22, 101, 52);
            lblKpiNutricion.Location = new Point(778, 7);
            lblKpiNutricion.Name = "lblKpiNutricion";
            lblKpiNutricion.Size = new Size(154, 26);
            lblKpiNutricion.TabIndex = 4;
            lblKpiNutricion.Text = "Nutrición: 0";
            lblKpiNutricion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnContactarSocio
            // 
            btnContactarSocio.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnContactarSocio.BackColor = Color.FromArgb(21, 128, 61);
            btnContactarSocio.FlatAppearance.BorderSize = 0;
            btnContactarSocio.FlatStyle = FlatStyle.Flat;
            btnContactarSocio.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            btnContactarSocio.ForeColor = Color.White;
            btnContactarSocio.Location = new Point(942, 6);
            btnContactarSocio.Name = "btnContactarSocio";
            btnContactarSocio.Size = new Size(162, 28);
            btnContactarSocio.TabIndex = 5;
            btnContactarSocio.Text = "💬 Contactar Socio";
            btnContactarSocio.UseVisualStyleBackColor = false;
            btnContactarSocio.Visible = false;
            btnContactarSocio.Click += btnContactarSocio_Click;
            // 
            // btnRefrescarAlertas
            // 
            btnRefrescarAlertas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefrescarAlertas.BackColor = Color.FromArgb(37, 99, 235);
            btnRefrescarAlertas.FlatAppearance.BorderSize = 0;
            btnRefrescarAlertas.FlatStyle = FlatStyle.Flat;
            btnRefrescarAlertas.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            btnRefrescarAlertas.ForeColor = Color.White;
            btnRefrescarAlertas.Location = new Point(1112, 6);
            btnRefrescarAlertas.Name = "btnRefrescarAlertas";
            btnRefrescarAlertas.Size = new Size(118, 28);
            btnRefrescarAlertas.TabIndex = 6;
            btnRefrescarAlertas.Text = "Actualizar";
            btnRefrescarAlertas.UseVisualStyleBackColor = false;
            btnRefrescarAlertas.Click += btnRefrescarAlertas_Click;
            // 
            // dgvAlertas
            // 
            dgvAlertas.AllowUserToAddRows = false;
            dgvAlertas.AllowUserToDeleteRows = false;
            dgvAlertas.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvAlertas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAlertas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlertas.BackgroundColor = Color.White;
            dgvAlertas.BorderStyle = BorderStyle.None;
            dgvAlertas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAlertas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAlertas.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White,
                WrapMode = DataGridViewTriState.False
            };
            dgvAlertas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAlertas.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55),
                Padding = new Padding(6, 3, 6, 3)
            };
            dgvAlertas.EnableHeadersVisualStyles = false;
            dgvAlertas.GridColor = Color.FromArgb(226, 232, 240);
            dgvAlertas.Location = new Point(12, 40);
            dgvAlertas.MultiSelect = false;
            dgvAlertas.Name = "dgvAlertas";
            dgvAlertas.ReadOnly = true;
            dgvAlertas.RowHeadersVisible = false;
            dgvAlertas.RowTemplate.Height = 32;
            dgvAlertas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlertas.Size = new Size(1218, 174);
            dgvAlertas.TabIndex = 7;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            panel1.Location = new Point(28, 304);
            panel1.Name = "panel1";
            panel1.Size = new Size(1244, 114);
            panel1.TabIndex = 3;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblDesde.ForeColor = Color.FromArgb(31, 41, 55);
            lblDesde.Location = new Point(16, 10);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 6;
            lblDesde.Text = "Desde";
            // 
            // dtpDesde
            // 
            dtpDesde.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpDesde.Location = new Point(16, 28);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(210, 23);
            dtpDesde.TabIndex = 0;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblHasta.ForeColor = Color.FromArgb(31, 41, 55);
            lblHasta.Location = new Point(242, 10);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(37, 15);
            lblHasta.TabIndex = 8;
            lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            dtpHasta.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpHasta.Location = new Point(242, 28);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(210, 23);
            dtpHasta.TabIndex = 1;
            // 
            // lblCriticidad
            // 
            lblCriticidad.AutoSize = true;
            lblCriticidad.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblCriticidad.ForeColor = Color.FromArgb(31, 41, 55);
            lblCriticidad.Location = new Point(468, 10);
            lblCriticidad.Name = "lblCriticidad";
            lblCriticidad.Size = new Size(58, 15);
            lblCriticidad.TabIndex = 9;
            lblCriticidad.Text = "Criticidad";
            // 
            // cmbCriticidad
            // 
            cmbCriticidad.BackColor = Color.White;
            cmbCriticidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCriticidad.FlatStyle = FlatStyle.Flat;
            cmbCriticidad.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbCriticidad.ForeColor = Color.FromArgb(31, 41, 55);
            cmbCriticidad.FormattingEnabled = true;
            cmbCriticidad.Location = new Point(468, 28);
            cmbCriticidad.Name = "cmbCriticidad";
            cmbCriticidad.Size = new Size(210, 23);
            cmbCriticidad.TabIndex = 2;
            // 
            // lblUsuarios
            // 
            lblUsuarios.AutoSize = true;
            lblUsuarios.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblUsuarios.ForeColor = Color.FromArgb(31, 41, 55);
            lblUsuarios.Location = new Point(694, 10);
            lblUsuarios.Name = "lblUsuarios";
            lblUsuarios.Size = new Size(52, 15);
            lblUsuarios.TabIndex = 7;
            lblUsuarios.Text = "Usuarios";
            // 
            // cmbUsuarios
            // 
            cmbUsuarios.BackColor = Color.White;
            cmbUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsuarios.FlatStyle = FlatStyle.Flat;
            cmbUsuarios.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbUsuarios.ForeColor = Color.FromArgb(31, 41, 55);
            cmbUsuarios.FormattingEnabled = true;
            cmbUsuarios.Location = new Point(694, 28);
            cmbUsuarios.Name = "cmbUsuarios";
            cmbUsuarios.Size = new Size(210, 23);
            cmbUsuarios.TabIndex = 3;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblBuscar.ForeColor = Color.FromArgb(31, 41, 55);
            lblBuscar.Location = new Point(16, 60);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(94, 15);
            lblBuscar.TabIndex = 10;
            lblBuscar.Text = "Buscar por Texto";
            // 
            // txtBuscar
            // 
            txtBuscar.BackColor = Color.White;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtBuscar.ForeColor = Color.FromArgb(31, 41, 55);
            txtBuscar.Location = new Point(16, 78);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(888, 23);
            txtBuscar.TabIndex = 4;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFiltrar.BackColor = Color.FromArgb(37, 99, 235);
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(926, 22);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(146, 79);
            btnFiltrar.TabIndex = 4;
            btnFiltrar.Text = "Aplicar Filtros";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiar.BackColor = Color.FromArgb(71, 85, 105);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(1082, 22);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(146, 79);
            btnLimpiar.TabIndex = 5;
            btnLimpiar.Text = "Limpiar Filtros";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
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
            dgvLogs.Location = new Point(28, 426);
            dgvLogs.MultiSelect = false;
            dgvLogs.Name = "dgvLogs";
            dgvLogs.ReadOnly = true;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.RowTemplate.Height = 34;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.Size = new Size(1244, 304);
            dgvLogs.TabIndex = 2;
            // 
            // btnBackup
            // 
            btnBackup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
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
            // btnGestionRoles
            // 
            btnGestionRoles.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
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
            // btnControlCambios
            // 
            btnControlCambios.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
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
            // frmSesion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1300, 820);
            Controls.Add(pnlDashboardAlertas);
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
            pnlDashboardAlertas.ResumeLayout(false);
            pnlDashboardAlertas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAlertas).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida_Base;
        private Button btnCerrarSesion;
        private Panel pnlDashboardAlertas;
        private Button btnVistaAlertas;
        private Button btnVistaChurn;
        private Label lblKpiRecepcion;
        private Label lblKpiEntrenamiento;
        private Label lblKpiNutricion;
        private Button btnContactarSocio;
        private Button btnRefrescarAlertas;
        private DataGridView dgvAlertas;
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