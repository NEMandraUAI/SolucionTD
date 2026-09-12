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
            lblDNI.Location = new Point(637, 12);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 0;
            lblDNI.Text = "DNI";
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(637, 30);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(151, 23);
            txtDNI.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(637, 59);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(151, 39);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // dgvRutinas
            // 
            dgvRutinas.AllowUserToAddRows = false;
            dgvRutinas.AllowUserToDeleteRows = false;
            dgvRutinas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRutinas.Location = new Point(12, 12);
            dgvRutinas.Name = "dgvRutinas";
            dgvRutinas.ReadOnly = true;
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.Size = new Size(619, 224);
            dgvRutinas.TabIndex = 3;
            dgvRutinas.CellClick += dgvRutinas_CellClick;
            // 
            // txtObjetivo
            // 
            txtObjetivo.Location = new Point(12, 281);
            txtObjetivo.Name = "txtObjetivo";
            txtObjetivo.Size = new Size(151, 23);
            txtObjetivo.TabIndex = 5;
            // 
            // lblObjetivo
            // 
            lblObjetivo.AutoSize = true;
            lblObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObjetivo.Location = new Point(12, 263);
            lblObjetivo.Name = "lblObjetivo";
            lblObjetivo.Size = new Size(55, 15);
            lblObjetivo.TabIndex = 4;
            lblObjetivo.Text = "Objetivo";
            // 
            // numFrecuencia
            // 
            numFrecuencia.Location = new Point(228, 282);
            numFrecuencia.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numFrecuencia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFrecuencia.Name = "numFrecuencia";
            numFrecuencia.Size = new Size(151, 23);
            numFrecuencia.TabIndex = 6;
            numFrecuencia.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFrecuencia
            // 
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrecuencia.Location = new Point(228, 264);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(67, 15);
            lblFrecuencia.TabIndex = 7;
            lblFrecuencia.Text = "Frecuencia";
            // 
            // lblDetalle
            // 
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDetalle.Location = new Point(12, 324);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(47, 15);
            lblDetalle.TabIndex = 8;
            lblDetalle.Text = "Detalle";
            // 
            // txtDetalle
            // 
            txtDetalle.Location = new Point(12, 342);
            txtDetalle.Multiline = true;
            txtDetalle.Name = "txtDetalle";
            txtDetalle.Size = new Size(367, 96);
            txtDetalle.TabIndex = 9;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(385, 342);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(151, 39);
            btnModificar.TabIndex = 10;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(385, 399);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(151, 39);
            btnEliminar.TabIndex = 11;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // frmGestionRutinas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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