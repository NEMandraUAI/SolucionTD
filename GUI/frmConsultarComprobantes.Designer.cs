namespace GUI
{
    partial class frmConsultarComprobantes
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
            dtpDesde = new DateTimePicker();
            dtpHasta = new DateTimePicker();
            txtDNI = new TextBox();
            btnFiltrar = new Button();
            btnLimpiar = new Button();
            dgvComprobantes = new DataGridView();
            lblDNI = new Label();
            lblDesde = new Label();
            lblHasta = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvComprobantes).BeginInit();
            SuspendLayout();
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(12, 96);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 0;
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(12, 160);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 1;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(12, 30);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(200, 23);
            txtDNI.TabIndex = 2;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Location = new Point(12, 202);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(91, 42);
            btnFiltrar.TabIndex = 3;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(121, 202);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(91, 42);
            btnLimpiar.TabIndex = 4;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // dgvComprobantes
            // 
            dgvComprobantes.AllowUserToAddRows = false;
            dgvComprobantes.AllowUserToDeleteRows = false;
            dgvComprobantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvComprobantes.Location = new Point(218, 12);
            dgvComprobantes.Name = "dgvComprobantes";
            dgvComprobantes.ReadOnly = true;
            dgvComprobantes.Size = new Size(570, 232);
            dgvComprobantes.TabIndex = 5;
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.Location = new Point(12, 12);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 6;
            lblDNI.Text = "DNI";
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDesde.Location = new Point(12, 78);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(42, 15);
            lblDesde.TabIndex = 7;
            lblDesde.Text = "Desde";
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHasta.Location = new Point(12, 142);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(38, 15);
            lblHasta.TabIndex = 8;
            lblHasta.Text = "Hasta";
            // 
            // frmConsultarComprobantes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 259);
            Controls.Add(lblHasta);
            Controls.Add(lblDesde);
            Controls.Add(lblDNI);
            Controls.Add(dgvComprobantes);
            Controls.Add(btnLimpiar);
            Controls.Add(btnFiltrar);
            Controls.Add(txtDNI);
            Controls.Add(dtpHasta);
            Controls.Add(dtpDesde);
            Name = "frmConsultarComprobantes";
            Text = "frmConsultarComprobantes";
            FormClosing += frmConsultarComprobantes_FormClosing;
            Load += frmConsultarComprobantes_Load;
            ((System.ComponentModel.ISupportInitialize)dgvComprobantes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private TextBox txtDNI;
        private Button btnFiltrar;
        private Button btnLimpiar;
        private DataGridView dgvComprobantes;
        private Label lblDNI;
        private Label lblDesde;
        private Label lblHasta;
    }
}