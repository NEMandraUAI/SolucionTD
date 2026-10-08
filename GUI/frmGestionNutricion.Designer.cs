namespace GUI
{
    partial class frmGestionNutricion
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
            dgvAgendaHoy = new DataGridView();
            txtDNISocio = new TextBox();
            btnBuscarSocio = new Button();
            lblDNISocio = new Label();
            dgvPlanesHistoricos = new DataGridView();
            txtAntropometria = new TextBox();
            txtHabitos = new TextBox();
            btnAsignarPlan = new Button();
            lblAntropometria = new Label();
            lblHabitos = new Label();
            lblAgendaHoy = new Label();
            lblPlanesHistoricos = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvAgendaHoy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPlanesHistoricos).BeginInit();
            SuspendLayout();
            // 
            // dgvAgendaHoy
            // 
            dgvAgendaHoy.AllowUserToAddRows = false;
            dgvAgendaHoy.AllowUserToDeleteRows = false;
            dgvAgendaHoy.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvAgendaHoy.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dgvAgendaHoy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAgendaHoy.BackgroundColor = Color.White;
            dgvAgendaHoy.BorderStyle = BorderStyle.None;
            dgvAgendaHoy.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAgendaHoy.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAgendaHoy.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvAgendaHoy.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAgendaHoy.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvAgendaHoy.EnableHeadersVisualStyles = false;
            dgvAgendaHoy.GridColor = Color.FromArgb(226, 232, 240);
            dgvAgendaHoy.Location = new Point(28, 54);
            dgvAgendaHoy.Name = "dgvAgendaHoy";
            dgvAgendaHoy.ReadOnly = true;
            dgvAgendaHoy.RowHeadersVisible = false;
            dgvAgendaHoy.RowTemplate.Height = 36;
            dgvAgendaHoy.Size = new Size(520, 240);
            dgvAgendaHoy.TabIndex = 0;
            // 
            // txtDNISocio
            // 
            txtDNISocio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNISocio.BackColor = Color.White;
            txtDNISocio.BorderStyle = BorderStyle.FixedSingle;
            txtDNISocio.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNISocio.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNISocio.Location = new Point(28, 354);
            txtDNISocio.Name = "txtDNISocio";
            txtDNISocio.Size = new Size(320, 23);
            txtDNISocio.TabIndex = 1;
            // 
            // btnBuscarSocio
            // 
            btnBuscarSocio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnBuscarSocio.BackColor = Color.FromArgb(71, 85, 105);
            btnBuscarSocio.FlatAppearance.BorderSize = 0;
            btnBuscarSocio.FlatStyle = FlatStyle.Flat;
            btnBuscarSocio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuscarSocio.ForeColor = Color.White;
            btnBuscarSocio.Location = new Point(366, 350);
            btnBuscarSocio.Name = "btnBuscarSocio";
            btnBuscarSocio.Size = new Size(182, 42);
            btnBuscarSocio.TabIndex = 2;
            btnBuscarSocio.Text = "button1";
            btnBuscarSocio.UseVisualStyleBackColor = false;
            btnBuscarSocio.Click += btnBuscarSocio_Click;
            // 
            // lblDNISocio
            // 
            lblDNISocio.AutoSize = true;
            lblDNISocio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblDNISocio.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNISocio.Location = new Point(28, 328);
            lblDNISocio.Name = "lblDNISocio";
            lblDNISocio.Size = new Size(38, 15);
            lblDNISocio.TabIndex = 3;
            lblDNISocio.Text = "label1";
            // 
            // dgvPlanesHistoricos
            // 
            dgvPlanesHistoricos.AllowUserToAddRows = false;
            dgvPlanesHistoricos.AllowUserToDeleteRows = false;
            dgvPlanesHistoricos.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvPlanesHistoricos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvPlanesHistoricos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanesHistoricos.BackgroundColor = Color.White;
            dgvPlanesHistoricos.BorderStyle = BorderStyle.None;
            dgvPlanesHistoricos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPlanesHistoricos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPlanesHistoricos.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvPlanesHistoricos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPlanesHistoricos.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvPlanesHistoricos.EnableHeadersVisualStyles = false;
            dgvPlanesHistoricos.GridColor = Color.FromArgb(226, 232, 240);
            dgvPlanesHistoricos.Location = new Point(28, 444);
            dgvPlanesHistoricos.Name = "dgvPlanesHistoricos";
            dgvPlanesHistoricos.ReadOnly = true;
            dgvPlanesHistoricos.RowHeadersVisible = false;
            dgvPlanesHistoricos.RowTemplate.Height = 36;
            dgvPlanesHistoricos.Size = new Size(520, 310);
            dgvPlanesHistoricos.TabIndex = 4;
            // 
            // txtAntropometria
            // 
            txtAntropometria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtAntropometria.BackColor = Color.White;
            txtAntropometria.BorderStyle = BorderStyle.FixedSingle;
            txtAntropometria.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtAntropometria.ForeColor = Color.FromArgb(31, 41, 55);
            txtAntropometria.Location = new Point(580, 54);
            txtAntropometria.Multiline = true;
            txtAntropometria.Name = "txtAntropometria";
            txtAntropometria.Size = new Size(592, 240);
            txtAntropometria.TabIndex = 5;
            // 
            // txtHabitos
            // 
            txtHabitos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtHabitos.BackColor = Color.White;
            txtHabitos.BorderStyle = BorderStyle.FixedSingle;
            txtHabitos.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtHabitos.ForeColor = Color.FromArgb(31, 41, 55);
            txtHabitos.Location = new Point(580, 356);
            txtHabitos.Multiline = true;
            txtHabitos.Name = "txtHabitos";
            txtHabitos.Size = new Size(592, 300);
            txtHabitos.TabIndex = 6;
            // 
            // btnAsignarPlan
            // 
            btnAsignarPlan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAsignarPlan.BackColor = Color.FromArgb(37, 99, 235);
            btnAsignarPlan.FlatAppearance.BorderSize = 0;
            btnAsignarPlan.FlatStyle = FlatStyle.Flat;
            btnAsignarPlan.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAsignarPlan.ForeColor = Color.White;
            btnAsignarPlan.Location = new Point(580, 680);
            btnAsignarPlan.Name = "btnAsignarPlan";
            btnAsignarPlan.Size = new Size(592, 48);
            btnAsignarPlan.TabIndex = 7;
            btnAsignarPlan.Text = "button1";
            btnAsignarPlan.UseVisualStyleBackColor = false;
            btnAsignarPlan.Click += btnAsignarPlan_Click;
            // 
            // lblAntropometria
            // 
            lblAntropometria.AutoSize = true;
            lblAntropometria.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblAntropometria.ForeColor = Color.FromArgb(31, 41, 55);
            lblAntropometria.Location = new Point(580, 28);
            lblAntropometria.Name = "lblAntropometria";
            lblAntropometria.Size = new Size(38, 15);
            lblAntropometria.TabIndex = 8;
            lblAntropometria.Text = "label2";
            // 
            // lblHabitos
            // 
            lblHabitos.AutoSize = true;
            lblHabitos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblHabitos.ForeColor = Color.FromArgb(31, 41, 55);
            lblHabitos.Location = new Point(580, 330);
            lblHabitos.Name = "lblHabitos";
            lblHabitos.Size = new Size(38, 15);
            lblHabitos.TabIndex = 9;
            lblHabitos.Text = "label3";
            // 
            // lblAgendaHoy
            // 
            lblAgendaHoy.AutoSize = true;
            lblAgendaHoy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblAgendaHoy.ForeColor = Color.FromArgb(31, 41, 55);
            lblAgendaHoy.Location = new Point(28, 28);
            lblAgendaHoy.Name = "lblAgendaHoy";
            lblAgendaHoy.Size = new Size(38, 15);
            lblAgendaHoy.TabIndex = 10;
            lblAgendaHoy.Text = "label4";
            // 
            // lblPlanesHistoricos
            // 
            lblPlanesHistoricos.AutoSize = true;
            lblPlanesHistoricos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblPlanesHistoricos.ForeColor = Color.FromArgb(31, 41, 55);
            lblPlanesHistoricos.Location = new Point(28, 418);
            lblPlanesHistoricos.Name = "lblPlanesHistoricos";
            lblPlanesHistoricos.Size = new Size(38, 15);
            lblPlanesHistoricos.TabIndex = 11;
            lblPlanesHistoricos.Text = "label1";
            // 
            // frmGestionNutricion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 800);
            Controls.Add(lblPlanesHistoricos);
            Controls.Add(lblAgendaHoy);
            Controls.Add(lblHabitos);
            Controls.Add(lblAntropometria);
            Controls.Add(btnAsignarPlan);
            Controls.Add(txtHabitos);
            Controls.Add(txtAntropometria);
            Controls.Add(dgvPlanesHistoricos);
            Controls.Add(lblDNISocio);
            Controls.Add(btnBuscarSocio);
            Controls.Add(txtDNISocio);
            Controls.Add(dgvAgendaHoy);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmGestionNutricion";
            Text = "frmGestionNutricion";
            FormClosing += frmGestionNutricion_FormClosing;
            Load += frmGestionNutricion_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAgendaHoy).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPlanesHistoricos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvAgendaHoy;
        private TextBox txtDNISocio;
        private Button btnBuscarSocio;
        private Label lblDNISocio;
        private DataGridView dgvPlanesHistoricos;
        private TextBox txtAntropometria;
        private TextBox txtHabitos;
        private Button btnAsignarPlan;
        private Label lblAntropometria;
        private Label lblHabitos;
        private Label lblAgendaHoy;
        private Label lblPlanesHistoricos;
    }
}