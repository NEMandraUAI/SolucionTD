namespace GUI
{
    partial class frmGestionRutinas
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
            lblDNI = new Label();
            txtDNI = new TextBox();
            btnBuscar = new Button();
            lblAlertaNutricional = new Label();
            dgvRutinas = new DataGridView();
            pnlGraficoFuerza = new Panel();
            cmbObjetivo = new ComboBox();
            lblObjetivo = new Label();
            numFrecuencia = new NumericUpDown();
            lblFrecuencia = new Label();
            btnSugerirPlantilla = new Button();
            btnQuitarEjercicio = new Button();
            lblDetalle = new Label();
            dgvEjercicios = new DataGridView();
            lblTituloMapa = new Label();
            pnlMapaCalor = new Panel();
            btnModificar = new Button();
            btnEliminar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRutinas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvEjercicios).BeginInit();
            SuspendLayout();
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(28, 22);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 0;
            lblDNI.Text = "DNI";
            // 
            // txtDNI
            // 
            txtDNI.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(28, 46);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(280, 23);
            txtDNI.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnBuscar.BackColor = Color.FromArgb(71, 85, 105);
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(324, 40);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(140, 38);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // lblAlertaNutricional
            // 
            lblAlertaNutricional.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAlertaNutricional.BackColor = Color.FromArgb(241, 245, 249);
            lblAlertaNutricional.BorderStyle = BorderStyle.FixedSingle;
            lblAlertaNutricional.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAlertaNutricional.ForeColor = Color.FromArgb(71, 85, 105);
            lblAlertaNutricional.Location = new Point(484, 16);
            lblAlertaNutricional.Name = "lblAlertaNutricional";
            lblAlertaNutricional.Padding = new Padding(10, 4, 10, 4);
            lblAlertaNutricional.Size = new Size(688, 66);
            lblAlertaNutricional.TabIndex = 16;
            lblAlertaNutricional.Text = "ℹ️ SINERGIA INTER-ÁREAS: Busque las rutinas de un socio para consultar su estado clínico-nutricional.";
            lblAlertaNutricional.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvRutinas
            // 
            dgvRutinas.AllowUserToAddRows = false;
            dgvRutinas.AllowUserToDeleteRows = false;
            dgvRutinas.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvRutinas.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dgvRutinas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRutinas.BackgroundColor = Color.White;
            dgvRutinas.BorderStyle = BorderStyle.None;
            dgvRutinas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRutinas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRutinas.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvRutinas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRutinas.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvRutinas.EnableHeadersVisualStyles = false;
            dgvRutinas.GridColor = Color.FromArgb(226, 232, 240);
            dgvRutinas.Location = new Point(28, 92);
            dgvRutinas.Name = "dgvRutinas";
            dgvRutinas.ReadOnly = true;
            dgvRutinas.RowHeadersVisible = false;
            dgvRutinas.RowTemplate.Height = 34;
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.Size = new Size(680, 210);
            dgvRutinas.TabIndex = 3;
            dgvRutinas.CellClick += dgvRutinas_CellClick;
            // 
            // pnlGraficoFuerza
            // 
            pnlGraficoFuerza.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlGraficoFuerza.BackColor = Color.White;
            pnlGraficoFuerza.BorderStyle = BorderStyle.FixedSingle;
            pnlGraficoFuerza.Location = new Point(724, 92);
            pnlGraficoFuerza.Name = "pnlGraficoFuerza";
            pnlGraficoFuerza.Size = new Size(448, 210);
            pnlGraficoFuerza.TabIndex = 17;
            pnlGraficoFuerza.Paint += pnlGraficoFuerza_Paint;
            // 
            // lblObjetivo
            // 
            lblObjetivo.AutoSize = true;
            lblObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObjetivo.ForeColor = Color.FromArgb(31, 41, 55);
            lblObjetivo.Location = new Point(28, 316);
            lblObjetivo.Name = "lblObjetivo";
            lblObjetivo.Size = new Size(55, 15);
            lblObjetivo.TabIndex = 4;
            lblObjetivo.Text = "Objetivo";
            // 
            // cmbObjetivo
            // 
            cmbObjetivo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbObjetivo.BackColor = Color.White;
            cmbObjetivo.DropDownStyle = ComboBoxStyle.DropDown;
            cmbObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbObjetivo.ForeColor = Color.FromArgb(31, 41, 55);
            cmbObjetivo.FormattingEnabled = true;
            cmbObjetivo.Location = new Point(28, 340);
            cmbObjetivo.Name = "cmbObjetivo";
            cmbObjetivo.Size = new Size(270, 23);
            cmbObjetivo.TabIndex = 5;
            cmbObjetivo.SelectedIndexChanged += cmbObjetivo_Changed;
            cmbObjetivo.TextChanged += cmbObjetivo_Changed;
            // 
            // lblFrecuencia
            // 
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrecuencia.ForeColor = Color.FromArgb(31, 41, 55);
            lblFrecuencia.Location = new Point(314, 316);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(67, 15);
            lblFrecuencia.TabIndex = 7;
            lblFrecuencia.Text = "Frecuencia";
            // 
            // numFrecuencia
            // 
            numFrecuencia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            numFrecuencia.BackColor = Color.White;
            numFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numFrecuencia.ForeColor = Color.FromArgb(31, 41, 55);
            numFrecuencia.Location = new Point(314, 340);
            numFrecuencia.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numFrecuencia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFrecuencia.Name = "numFrecuencia";
            numFrecuencia.Size = new Size(110, 23);
            numFrecuencia.TabIndex = 6;
            numFrecuencia.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numFrecuencia.ValueChanged += numFrecuencia_ValueChanged;
            // 
            // btnSugerirPlantilla
            // 
            btnSugerirPlantilla.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnSugerirPlantilla.BackColor = Color.FromArgb(16, 185, 129);
            btnSugerirPlantilla.FlatAppearance.BorderSize = 0;
            btnSugerirPlantilla.FlatStyle = FlatStyle.Flat;
            btnSugerirPlantilla.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnSugerirPlantilla.ForeColor = Color.White;
            btnSugerirPlantilla.Location = new Point(440, 334);
            btnSugerirPlantilla.Name = "btnSugerirPlantilla";
            btnSugerirPlantilla.Size = new Size(150, 34);
            btnSugerirPlantilla.TabIndex = 12;
            btnSugerirPlantilla.Text = "Sugerir según Objetivo";
            btnSugerirPlantilla.UseVisualStyleBackColor = false;
            btnSugerirPlantilla.Click += btnSugerirPlantilla_Click;
            // 
            // btnQuitarEjercicio
            // 
            btnQuitarEjercicio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnQuitarEjercicio.BackColor = Color.FromArgb(100, 116, 139);
            btnQuitarEjercicio.FlatAppearance.BorderSize = 0;
            btnQuitarEjercicio.FlatStyle = FlatStyle.Flat;
            btnQuitarEjercicio.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnQuitarEjercicio.ForeColor = Color.White;
            btnQuitarEjercicio.Location = new Point(602, 334);
            btnQuitarEjercicio.Name = "btnQuitarEjercicio";
            btnQuitarEjercicio.Size = new Size(140, 34);
            btnQuitarEjercicio.TabIndex = 13;
            btnQuitarEjercicio.Text = "Quitar Ejercicio";
            btnQuitarEjercicio.UseVisualStyleBackColor = false;
            btnQuitarEjercicio.Click += btnQuitarEjercicio_Click;
            // 
            // lblDetalle
            // 
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDetalle.ForeColor = Color.FromArgb(31, 41, 55);
            lblDetalle.Location = new Point(28, 386);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(47, 15);
            lblDetalle.TabIndex = 8;
            lblDetalle.Text = "Detalle";
            // 
            // dgvEjercicios
            // 
            dgvEjercicios.AllowUserToAddRows = true;
            dgvEjercicios.AllowUserToDeleteRows = true;
            dgvEjercicios.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvEjercicios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEjercicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEjercicios.BackgroundColor = Color.White;
            dgvEjercicios.BorderStyle = BorderStyle.FixedSingle;
            dgvEjercicios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEjercicios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvEjercicios.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White,
                WrapMode = DataGridViewTriState.False
            };
            dgvEjercicios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEjercicios.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55),
                Padding = new Padding(4, 2, 4, 2)
            };
            dgvEjercicios.EnableHeadersVisualStyles = false;
            dgvEjercicios.GridColor = Color.FromArgb(226, 232, 240);
            dgvEjercicios.Location = new Point(28, 410);
            dgvEjercicios.Name = "dgvEjercicios";
            dgvEjercicios.RowHeadersVisible = true;
            dgvEjercicios.RowHeadersWidth = 28;
            dgvEjercicios.RowTemplate.Height = 32;
            dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEjercicios.Size = new Size(562, 326);
            dgvEjercicios.TabIndex = 9;
            // 
            // lblTituloMapa
            // 
            lblTituloMapa.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTituloMapa.AutoSize = true;
            lblTituloMapa.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloMapa.ForeColor = Color.FromArgb(31, 41, 55);
            lblTituloMapa.Location = new Point(604, 386);
            lblTituloMapa.Name = "lblTituloMapa";
            lblTituloMapa.Size = new Size(195, 15);
            lblTituloMapa.TabIndex = 14;
            lblTituloMapa.Text = "Mapa de Calor Muscular (Volumen)";
            // 
            // pnlMapaCalor
            // 
            pnlMapaCalor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlMapaCalor.BackColor = Color.White;
            pnlMapaCalor.BorderStyle = BorderStyle.FixedSingle;
            pnlMapaCalor.Location = new Point(604, 410);
            pnlMapaCalor.Name = "pnlMapaCalor";
            pnlMapaCalor.Size = new Size(306, 326);
            pnlMapaCalor.TabIndex = 15;
            pnlMapaCalor.Paint += pnlMapaCalor_Paint;
            // 
            // btnModificar
            // 
            btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnModificar.BackColor = Color.FromArgb(37, 99, 235);
            btnModificar.FlatAppearance.BorderSize = 0;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(924, 556);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(248, 56);
            btnModificar.TabIndex = 10;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEliminar.BackColor = Color.FromArgb(185, 28, 28);
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(924, 628);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(248, 56);
            btnEliminar.TabIndex = 11;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // frmGestionRutinas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 760);
            Controls.Add(pnlGraficoFuerza);
            Controls.Add(lblAlertaNutricional);
            Controls.Add(pnlMapaCalor);
            Controls.Add(lblTituloMapa);
            Controls.Add(btnQuitarEjercicio);
            Controls.Add(btnSugerirPlantilla);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(dgvEjercicios);
            Controls.Add(lblDetalle);
            Controls.Add(lblFrecuencia);
            Controls.Add(numFrecuencia);
            Controls.Add(cmbObjetivo);
            Controls.Add(lblObjetivo);
            Controls.Add(dgvRutinas);
            Controls.Add(btnBuscar);
            Controls.Add(txtDNI);
            Controls.Add(lblDNI);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmGestionRutinas";
            Text = "frmGestionRutinas";
            FormClosing += frmGestionRutinas_FormClosing;
            Load += frmGestionRutinas_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRutinas).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvEjercicios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDNI;
        private TextBox txtDNI;
        private Button btnBuscar;
        private Label lblAlertaNutricional;
        private DataGridView dgvRutinas;
        private Panel pnlGraficoFuerza;
        private ComboBox cmbObjetivo;
        private Label lblObjetivo;
        private NumericUpDown numFrecuencia;
        private Label lblFrecuencia;
        private Button btnSugerirPlantilla;
        private Button btnQuitarEjercicio;
        private Label lblDetalle;
        private DataGridView dgvEjercicios;
        private Label lblTituloMapa;
        private Panel pnlMapaCalor;
        private Button btnModificar;
        private Button btnEliminar;
    }
}