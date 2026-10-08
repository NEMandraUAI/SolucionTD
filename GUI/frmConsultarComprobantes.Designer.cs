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
            dtpDesde.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtpDesde.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpDesde.Location = new Point(28, 150);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(280, 23);
            dtpDesde.TabIndex = 0;
            // 
            // dtpHasta
            // 
            dtpHasta.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtpHasta.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpHasta.Location = new Point(28, 242);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(280, 23);
            dtpHasta.TabIndex = 1;
            // 
            // txtDNI
            // 
            txtDNI.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(28, 58);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(280, 23);
            txtDNI.TabIndex = 2;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnFiltrar.BackColor = Color.FromArgb(37, 99, 235);
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(28, 306);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(132, 42);
            btnFiltrar.TabIndex = 3;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLimpiar.BackColor = Color.FromArgb(71, 85, 105);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(176, 306);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(132, 42);
            btnLimpiar.TabIndex = 4;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // dgvComprobantes
            // 
            dgvComprobantes.AllowUserToAddRows = false;
            dgvComprobantes.AllowUserToDeleteRows = false;
            dgvComprobantes.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvComprobantes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvComprobantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvComprobantes.BackgroundColor = Color.White;
            dgvComprobantes.BorderStyle = BorderStyle.None;
            dgvComprobantes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvComprobantes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvComprobantes.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvComprobantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvComprobantes.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvComprobantes.EnableHeadersVisualStyles = false;
            dgvComprobantes.GridColor = Color.FromArgb(226, 232, 240);
            dgvComprobantes.Location = new Point(340, 28);
            dgvComprobantes.Name = "dgvComprobantes";
            dgvComprobantes.ReadOnly = true;
            dgvComprobantes.RowHeadersVisible = false;
            dgvComprobantes.RowTemplate.Height = 36;
            dgvComprobantes.Size = new Size(832, 664);
            dgvComprobantes.TabIndex = 5;
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(28, 32);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 6;
            lblDNI.Text = "DNI";
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDesde.ForeColor = Color.FromArgb(31, 41, 55);
            lblDesde.Location = new Point(28, 124);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(42, 15);
            lblDesde.TabIndex = 7;
            lblDesde.Text = "Desde";
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHasta.ForeColor = Color.FromArgb(31, 41, 55);
            lblHasta.Location = new Point(28, 216);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(38, 15);
            lblHasta.TabIndex = 8;
            lblHasta.Text = "Hasta";
            // 
            // frmConsultarComprobantes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 720);
            Controls.Add(lblHasta);
            Controls.Add(lblDesde);
            Controls.Add(lblDNI);
            Controls.Add(dgvComprobantes);
            Controls.Add(btnLimpiar);
            Controls.Add(btnFiltrar);
            Controls.Add(txtDNI);
            Controls.Add(dtpHasta);
            Controls.Add(dtpDesde);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
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