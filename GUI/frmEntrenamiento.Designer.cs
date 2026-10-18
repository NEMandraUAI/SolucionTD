namespace GUI
{
    partial class frmEntrenamiento
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
            btnBuscar = new Button();
            txtDNI = new TextBox();
            lblInfoSocio = new Label();
            lblAlertaNutricional = new Label();
            lblObjetivo = new Label();
            cmbObjetivo = new ComboBox();
            numFrecuencia = new NumericUpDown();
            lblFrecuencia = new Label();
            dgvEjercicios = new DataGridView();
            pnlGraficoFuerza = new Panel();
            lblDetalle = new Label();
            lblTituloMapa = new Label();
            pnlMapaCalor = new Panel();
            btnSugerirRutina = new Button();
            btnQuitarEjercicio = new Button();
            btnAsignarRutina = new Button();
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvEjercicios).BeginInit();
            SuspendLayout();
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(32, 30);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 12;
            lblDNI.Text = "DNI";
            // 
            // txtDNI
            // 
            txtDNI.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(32, 54);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(220, 23);
            txtDNI.TabIndex = 10;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnBuscar.BackColor = Color.FromArgb(71, 85, 105);
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(268, 46);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(140, 38);
            btnBuscar.TabIndex = 11;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // lblInfoSocio
            // 
            lblInfoSocio.AutoSize = true;
            lblInfoSocio.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInfoSocio.ForeColor = Color.FromArgb(31, 41, 55);
            lblInfoSocio.Location = new Point(32, 100);
            lblInfoSocio.Name = "lblInfoSocio";
            lblInfoSocio.Size = new Size(13, 17);
            lblInfoSocio.TabIndex = 13;
            lblInfoSocio.Text = "-";
            // 
            // lblAlertaNutricional
            // 
            lblAlertaNutricional.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAlertaNutricional.BackColor = Color.FromArgb(241, 245, 249);
            lblAlertaNutricional.BorderStyle = BorderStyle.FixedSingle;
            lblAlertaNutricional.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAlertaNutricional.ForeColor = Color.FromArgb(71, 85, 105);
            lblAlertaNutricional.Location = new Point(430, 34);
            lblAlertaNutricional.Name = "lblAlertaNutricional";
            lblAlertaNutricional.Padding = new Padding(10, 6, 10, 6);
            lblAlertaNutricional.Size = new Size(718, 84);
            lblAlertaNutricional.TabIndex = 25;
            lblAlertaNutricional.Text = "ℹ️ SINERGIA INTER-ÁREAS: Valide un socio para consultar su estado clínico-nutricional.";
            lblAlertaNutricional.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblObjetivo
            // 
            lblObjetivo.AutoSize = true;
            lblObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObjetivo.ForeColor = Color.FromArgb(31, 41, 55);
            lblObjetivo.Location = new Point(32, 140);
            lblObjetivo.Name = "lblObjetivo";
            lblObjetivo.Size = new Size(55, 15);
            lblObjetivo.TabIndex = 15;
            lblObjetivo.Text = "Objetivo";
            // 
            // cmbObjetivo
            // 
            cmbObjetivo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbObjetivo.BackColor = Color.White;
            cmbObjetivo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbObjetivo.ForeColor = Color.FromArgb(31, 41, 55);
            cmbObjetivo.FormattingEnabled = true;
            cmbObjetivo.Location = new Point(32, 164);
            cmbObjetivo.Name = "cmbObjetivo";
            cmbObjetivo.Size = new Size(310, 23);
            cmbObjetivo.TabIndex = 14;
            cmbObjetivo.SelectedIndexChanged += cmbObjetivo_SelectedIndexChanged;
            // 
            // lblFrecuencia
            // 
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrecuencia.ForeColor = Color.FromArgb(31, 41, 55);
            lblFrecuencia.Location = new Point(362, 140);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(67, 15);
            lblFrecuencia.TabIndex = 17;
            lblFrecuencia.Text = "Frecuencia";
            // 
            // numFrecuencia
            // 
            numFrecuencia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            numFrecuencia.BackColor = Color.White;
            numFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numFrecuencia.ForeColor = Color.FromArgb(31, 41, 55);
            numFrecuencia.Location = new Point(362, 164);
            numFrecuencia.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numFrecuencia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFrecuencia.Name = "numFrecuencia";
            numFrecuencia.Size = new Size(130, 23);
            numFrecuencia.TabIndex = 16;
            numFrecuencia.Value = new decimal(new int[] { 3, 0, 0, 0 });
            numFrecuencia.ValueChanged += numFrecuencia_ValueChanged;
            // 
            // btnSugerirRutina
            // 
            btnSugerirRutina.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnSugerirRutina.BackColor = Color.FromArgb(16, 185, 129);
            btnSugerirRutina.FlatAppearance.BorderSize = 0;
            btnSugerirRutina.FlatStyle = FlatStyle.Flat;
            btnSugerirRutina.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnSugerirRutina.ForeColor = Color.White;
            btnSugerirRutina.Location = new Point(512, 157);
            btnSugerirRutina.Name = "btnSugerirRutina";
            btnSugerirRutina.Size = new Size(150, 34);
            btnSugerirRutina.TabIndex = 21;
            btnSugerirRutina.Text = "Regenerar Sugerencia";
            btnSugerirRutina.UseVisualStyleBackColor = false;
            btnSugerirRutina.Click += btnSugerirRutina_Click;
            // 
            // btnQuitarEjercicio
            // 
            btnQuitarEjercicio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnQuitarEjercicio.BackColor = Color.FromArgb(185, 28, 28);
            btnQuitarEjercicio.FlatAppearance.BorderSize = 0;
            btnQuitarEjercicio.FlatStyle = FlatStyle.Flat;
            btnQuitarEjercicio.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnQuitarEjercicio.ForeColor = Color.White;
            btnQuitarEjercicio.Location = new Point(678, 157);
            btnQuitarEjercicio.Name = "btnQuitarEjercicio";
            btnQuitarEjercicio.Size = new Size(134, 34);
            btnQuitarEjercicio.TabIndex = 22;
            btnQuitarEjercicio.Text = "Quitar Ejercicio";
            btnQuitarEjercicio.UseVisualStyleBackColor = false;
            btnQuitarEjercicio.Click += btnQuitarEjercicio_Click;
            // 
            // lblDetalle
            // 
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDetalle.ForeColor = Color.FromArgb(31, 41, 55);
            lblDetalle.Location = new Point(32, 210);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(47, 15);
            lblDetalle.TabIndex = 19;
            lblDetalle.Text = "Detalle";
            // 
            // dgvEjercicios
            // 
            dgvEjercicios.AllowUserToAddRows = true;
            dgvEjercicios.AllowUserToDeleteRows = true;
            dgvEjercicios.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvEjercicios.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            dgvEjercicios.Location = new Point(32, 234);
            dgvEjercicios.Name = "dgvEjercicios";
            dgvEjercicios.RowHeadersVisible = true;
            dgvEjercicios.RowHeadersWidth = 28;
            dgvEjercicios.RowTemplate.Height = 32;
            dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEjercicios.Size = new Size(780, 224);
            dgvEjercicios.TabIndex = 18;
            // 
            // pnlGraficoFuerza
            // 
            pnlGraficoFuerza.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlGraficoFuerza.BackColor = Color.White;
            pnlGraficoFuerza.BorderStyle = BorderStyle.FixedSingle;
            pnlGraficoFuerza.Location = new Point(32, 468);
            pnlGraficoFuerza.Name = "pnlGraficoFuerza";
            pnlGraficoFuerza.Size = new Size(780, 164);
            pnlGraficoFuerza.TabIndex = 26;
            pnlGraficoFuerza.Paint += pnlGraficoFuerza_Paint;
            // 
            // lblTituloMapa
            // 
            lblTituloMapa.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTituloMapa.AutoSize = true;
            lblTituloMapa.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloMapa.ForeColor = Color.FromArgb(31, 41, 55);
            lblTituloMapa.Location = new Point(828, 210);
            lblTituloMapa.Name = "lblTituloMapa";
            lblTituloMapa.Size = new Size(195, 15);
            lblTituloMapa.TabIndex = 23;
            lblTituloMapa.Text = "Mapa de Calor Muscular (Volumen)";
            // 
            // pnlMapaCalor
            // 
            pnlMapaCalor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlMapaCalor.BackColor = Color.White;
            pnlMapaCalor.BorderStyle = BorderStyle.FixedSingle;
            pnlMapaCalor.Location = new Point(828, 234);
            pnlMapaCalor.Name = "pnlMapaCalor";
            pnlMapaCalor.Size = new Size(320, 398);
            pnlMapaCalor.TabIndex = 24;
            pnlMapaCalor.Paint += pnlMapaCalor_Paint;
            // 
            // btnAsignarRutina
            // 
            btnAsignarRutina.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAsignarRutina.BackColor = Color.FromArgb(37, 99, 235);
            btnAsignarRutina.FlatAppearance.BorderSize = 0;
            btnAsignarRutina.FlatStyle = FlatStyle.Flat;
            btnAsignarRutina.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAsignarRutina.ForeColor = Color.White;
            btnAsignarRutina.Location = new Point(32, 648);
            btnAsignarRutina.Name = "btnAsignarRutina";
            btnAsignarRutina.Size = new Size(1116, 44);
            btnAsignarRutina.TabIndex = 20;
            btnAsignarRutina.Text = "Asignar Rutina";
            btnAsignarRutina.UseVisualStyleBackColor = false;
            btnAsignarRutina.Click += btnAsignarRutina_Click;
            // 
            // frmEntrenamiento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1180, 712);
            Controls.Add(pnlGraficoFuerza);
            Controls.Add(lblAlertaNutricional);
            Controls.Add(pnlMapaCalor);
            Controls.Add(lblTituloMapa);
            Controls.Add(btnQuitarEjercicio);
            Controls.Add(btnSugerirRutina);
            Controls.Add(btnAsignarRutina);
            Controls.Add(lblDetalle);
            Controls.Add(dgvEjercicios);
            Controls.Add(lblFrecuencia);
            Controls.Add(numFrecuencia);
            Controls.Add(lblObjetivo);
            Controls.Add(cmbObjetivo);
            Controls.Add(lblInfoSocio);
            Controls.Add(lblDNI);
            Controls.Add(btnBuscar);
            Controls.Add(txtDNI);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmEntrenamiento";
            Text = "frmEntrenamiento";
            FormClosing += frmEntrenamiento_FormClosing;
            Load += frmEntrenamiento_Load;
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvEjercicios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDNI;
        private Button btnBuscar;
        private TextBox txtDNI;
        private Label lblInfoSocio;
        private Label lblAlertaNutricional;
        private Label lblObjetivo;
        private ComboBox cmbObjetivo;
        private NumericUpDown numFrecuencia;
        private Label lblFrecuencia;
        private DataGridView dgvEjercicios;
        private Panel pnlGraficoFuerza;
        private Label lblDetalle;
        private Label lblTituloMapa;
        private Panel pnlMapaCalor;
        private Button btnSugerirRutina;
        private Button btnQuitarEjercicio;
        private Button btnAsignarRutina;
    }
}