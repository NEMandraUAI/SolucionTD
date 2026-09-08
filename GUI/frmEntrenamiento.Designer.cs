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
            lblObjetivo = new Label();
            txtObjetivo = new TextBox();
            numFrecuencia = new NumericUpDown();
            lblFrecuencia = new Label();
            txtDetalle = new TextBox();
            lblDetalle = new Label();
            btnAsignarRutina = new Button();
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).BeginInit();
            SuspendLayout();
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.Location = new Point(12, 19);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 12;
            lblDNI.Text = "DNI";
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(12, 80);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(158, 38);
            btnBuscar.TabIndex = 11;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(12, 37);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(158, 23);
            txtDNI.TabIndex = 10;
            // 
            // lblInfoSocio
            // 
            lblInfoSocio.AutoSize = true;
            lblInfoSocio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInfoSocio.Location = new Point(176, 40);
            lblInfoSocio.Name = "lblInfoSocio";
            lblInfoSocio.Size = new Size(12, 15);
            lblInfoSocio.TabIndex = 13;
            lblInfoSocio.Text = "-";
            // 
            // lblObjetivo
            // 
            lblObjetivo.AutoSize = true;
            lblObjetivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObjetivo.Location = new Point(12, 157);
            lblObjetivo.Name = "lblObjetivo";
            lblObjetivo.Size = new Size(55, 15);
            lblObjetivo.TabIndex = 15;
            lblObjetivo.Text = "Objetivo";
            // 
            // txtObjetivo
            // 
            txtObjetivo.Location = new Point(12, 175);
            txtObjetivo.Name = "txtObjetivo";
            txtObjetivo.Size = new Size(158, 23);
            txtObjetivo.TabIndex = 14;
            // 
            // numFrecuencia
            // 
            numFrecuencia.Location = new Point(201, 175);
            numFrecuencia.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numFrecuencia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFrecuencia.Name = "numFrecuencia";
            numFrecuencia.Size = new Size(158, 23);
            numFrecuencia.TabIndex = 16;
            numFrecuencia.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFrecuencia
            // 
            lblFrecuencia.AutoSize = true;
            lblFrecuencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrecuencia.Location = new Point(201, 157);
            lblFrecuencia.Name = "lblFrecuencia";
            lblFrecuencia.Size = new Size(67, 15);
            lblFrecuencia.TabIndex = 17;
            lblFrecuencia.Text = "Frecuencia";
            // 
            // txtDetalle
            // 
            txtDetalle.Location = new Point(12, 241);
            txtDetalle.Multiline = true;
            txtDetalle.Name = "txtDetalle";
            txtDetalle.Size = new Size(347, 110);
            txtDetalle.TabIndex = 18;
            // 
            // lblDetalle
            // 
            lblDetalle.AutoSize = true;
            lblDetalle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDetalle.Location = new Point(12, 223);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(47, 15);
            lblDetalle.TabIndex = 19;
            lblDetalle.Text = "Detalle";
            // 
            // btnAsignarRutina
            // 
            btnAsignarRutina.Location = new Point(12, 357);
            btnAsignarRutina.Name = "btnAsignarRutina";
            btnAsignarRutina.Size = new Size(158, 38);
            btnAsignarRutina.TabIndex = 20;
            btnAsignarRutina.Text = "Asignar Rutina";
            btnAsignarRutina.UseVisualStyleBackColor = true;
            btnAsignarRutina.Click += btnAsignarRutina_Click;
            // 
            // frmEntrenamiento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(374, 414);
            Controls.Add(btnAsignarRutina);
            Controls.Add(lblDetalle);
            Controls.Add(txtDetalle);
            Controls.Add(lblFrecuencia);
            Controls.Add(numFrecuencia);
            Controls.Add(lblObjetivo);
            Controls.Add(txtObjetivo);
            Controls.Add(lblInfoSocio);
            Controls.Add(lblDNI);
            Controls.Add(btnBuscar);
            Controls.Add(txtDNI);
            Name = "frmEntrenamiento";
            Text = "frmEntrenamiento";
            FormClosing += frmEntrenamiento_FormClosing;
            Load += frmEntrenamiento_Load;
            ((System.ComponentModel.ISupportInitialize)numFrecuencia).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDNI;
        private Button btnBuscar;
        private TextBox txtDNI;
        private Label lblInfoSocio;
        private Label lblObjetivo;
        private TextBox txtObjetivo;
        private NumericUpDown numFrecuencia;
        private Label lblFrecuencia;
        private TextBox txtDetalle;
        private Label lblDetalle;
        private Button btnAsignarRutina;
    }
}