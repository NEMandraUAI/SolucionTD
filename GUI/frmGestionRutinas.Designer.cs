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
            dgvRutinas = new DataGridView();
            txtObjetivo = new TextBox();
            lblObjetivo = new Label();
            numFrecuencia = new NumericUpDown();
            lblFrecuencia = new Label();
            lblDetalle = new Label();
            txtDetalle = new TextBox();
            btnModificar = new Button();
            btnEliminar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRutinas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).BeginInit();
            SuspendLayout();
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(28, 28);
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
            txtDNI.Location = new Point(28, 54);
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
            btnBuscar.Location = new Point(324, 50);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(140, 42);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // dgvRutinas
            // 
            dgvRutinas.AllowUserToAddRows = false;
            dgvRutinas.AllowUserToDeleteRows = false;
            dgvRutinas.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvRutinas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            dgvRutinas.Location = new Point(28, 112);
            dgvRutinas.Name = "dgvRutinas";
            dgvRutinas.ReadOnly = true;
            dgvRutinas.RowHeadersVisible = false;
            dgvRutinas.RowTemplate.Height = 36;
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.Size = new Size(1144, 330);
            dgvRutinas.TabIndex = 3;
            dgvRutinas.CellClick += dgvRutinas_CellClick;
            // 
            // txtObjetivo
            // 
            txtObjetivo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtObjetivo.BackColor = Color.White;
            txtObjetivo.BorderStyle = BorderStyle.FixedSingle;
            txtObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtObjetivo.ForeColor = Color.FromArgb(31, 41, 55);
            txtObjetivo.Location = new Point(28, 478);
            txtObjetivo.Name = "txtObjetivo";
            txtObjetivo.Size = new Size(430, 23);
            txtObjetivo.TabIndex = 5;
            // 
            // lblObjetivo
            // 
            lblObjetivo.AutoSize = true;
            lblObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObjetivo.ForeColor = Color.FromArgb(31, 41, 55);
            lblObjetivo.Location = new Point(28, 452);
            lblObjetivo.Name = "lblObjetivo";
            lblObjetivo.Size = new Size(55, 15);
            lblObjetivo.TabIndex = 4;
            lblObjetivo.Text = "Objetivo";
            // 
            // numFrecuencia
            // 
            numFrecuencia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            numFrecuencia.BackColor = Color.White;
            numFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numFrecuencia.ForeColor = Color.FromArgb(31, 41, 55);
            numFrecuencia.Location = new Point(490, 478);
            numFrecuencia.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numFrecuencia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFrecuencia.Name = "numFrecuencia";
            numFrecuencia.Size = new Size(220, 23);
            numFrecuencia.TabIndex = 6;
            numFrecuencia.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFrecuencia
            // 
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrecuencia.ForeColor = Color.FromArgb(31, 41, 55);
            lblFrecuencia.Location = new Point(490, 452);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(67, 15);
            lblFrecuencia.TabIndex = 7;
            lblFrecuencia.Text = "Frecuencia";
            // 
            // lblDetalle
            // 
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDetalle.ForeColor = Color.FromArgb(31, 41, 55);
            lblDetalle.Location = new Point(28, 530);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(47, 15);
            lblDetalle.TabIndex = 8;
            lblDetalle.Text = "Detalle";
            // 
            // txtDetalle
            // 
            txtDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDetalle.BackColor = Color.White;
            txtDetalle.BorderStyle = BorderStyle.FixedSingle;
            txtDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDetalle.ForeColor = Color.FromArgb(31, 41, 55);
            txtDetalle.Location = new Point(28, 556);
            txtDetalle.Multiline = true;
            txtDetalle.Name = "txtDetalle";
            txtDetalle.Size = new Size(844, 140);
            txtDetalle.TabIndex = 9;
            // 
            // btnModificar
            // 
            btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnModificar.BackColor = Color.FromArgb(37, 99, 235);
            btnModificar.FlatAppearance.BorderSize = 0;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(900, 556);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(272, 56);
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
            btnEliminar.Location = new Point(900, 628);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(272, 56);
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
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(txtDetalle);
            Controls.Add(lblDetalle);
            Controls.Add(lblFrecuencia);
            Controls.Add(numFrecuencia);
            Controls.Add(txtObjetivo);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDNI;
        private TextBox txtDNI;
        private Button btnBuscar;
        private DataGridView dgvRutinas;
        private TextBox txtObjetivo;
        private Label lblObjetivo;
        private NumericUpDown numFrecuencia;
        private Label lblFrecuencia;
        private Label lblDetalle;
        private TextBox txtDetalle;
        private Button btnModificar;
        private Button btnEliminar;
    }
}