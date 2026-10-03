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
            dgvAgendaHoy.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAgendaHoy.Location = new Point(12, 30);
            dgvAgendaHoy.Name = "dgvAgendaHoy";
            dgvAgendaHoy.ReadOnly = true;
            dgvAgendaHoy.Size = new Size(437, 211);
            dgvAgendaHoy.TabIndex = 0;
            // 
            // txtDNISocio
            // 
            txtDNISocio.Location = new Point(12, 278);
            txtDNISocio.Name = "txtDNISocio";
            txtDNISocio.Size = new Size(181, 23);
            txtDNISocio.TabIndex = 1;
            // 
            // btnBuscarSocio
            // 
            btnBuscarSocio.Location = new Point(12, 307);
            btnBuscarSocio.Name = "btnBuscarSocio";
            btnBuscarSocio.Size = new Size(181, 45);
            btnBuscarSocio.TabIndex = 2;
            btnBuscarSocio.Text = "button1";
            btnBuscarSocio.UseVisualStyleBackColor = true;
            btnBuscarSocio.Click += btnBuscarSocio_Click;
            // 
            // lblDNISocio
            // 
            lblDNISocio.AutoSize = true;
            lblDNISocio.Location = new Point(12, 260);
            lblDNISocio.Name = "lblDNISocio";
            lblDNISocio.Size = new Size(38, 15);
            lblDNISocio.TabIndex = 3;
            lblDNISocio.Text = "label1";
            // 
            // dgvPlanesHistoricos
            // 
            dgvPlanesHistoricos.AllowUserToAddRows = false;
            dgvPlanesHistoricos.AllowUserToDeleteRows = false;
            dgvPlanesHistoricos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPlanesHistoricos.Location = new Point(12, 376);
            dgvPlanesHistoricos.Name = "dgvPlanesHistoricos";
            dgvPlanesHistoricos.ReadOnly = true;
            dgvPlanesHistoricos.Size = new Size(437, 294);
            dgvPlanesHistoricos.TabIndex = 4;
            // 
            // txtAntropometria
            // 
            txtAntropometria.Location = new Point(455, 376);
            txtAntropometria.Multiline = true;
            txtAntropometria.Name = "txtAntropometria";
            txtAntropometria.Size = new Size(404, 102);
            txtAntropometria.TabIndex = 5;
            // 
            // txtHabitos
            // 
            txtHabitos.Location = new Point(455, 517);
            txtHabitos.Multiline = true;
            txtHabitos.Name = "txtHabitos";
            txtHabitos.Size = new Size(404, 102);
            txtHabitos.TabIndex = 6;
            // 
            // btnAsignarPlan
            // 
            btnAsignarPlan.Location = new Point(455, 625);
            btnAsignarPlan.Name = "btnAsignarPlan";
            btnAsignarPlan.Size = new Size(181, 45);
            btnAsignarPlan.TabIndex = 7;
            btnAsignarPlan.Text = "button1";
            btnAsignarPlan.UseVisualStyleBackColor = true;
            btnAsignarPlan.Click += btnAsignarPlan_Click;
            // 
            // lblAntropometria
            // 
            lblAntropometria.AutoSize = true;
            lblAntropometria.Location = new Point(455, 358);
            lblAntropometria.Name = "lblAntropometria";
            lblAntropometria.Size = new Size(38, 15);
            lblAntropometria.TabIndex = 8;
            lblAntropometria.Text = "label2";
            // 
            // lblHabitos
            // 
            lblHabitos.AutoSize = true;
            lblHabitos.Location = new Point(455, 499);
            lblHabitos.Name = "lblHabitos";
            lblHabitos.Size = new Size(38, 15);
            lblHabitos.TabIndex = 9;
            lblHabitos.Text = "label3";
            // 
            // lblAgendaHoy
            // 
            lblAgendaHoy.AutoSize = true;
            lblAgendaHoy.Location = new Point(12, 12);
            lblAgendaHoy.Name = "lblAgendaHoy";
            lblAgendaHoy.Size = new Size(38, 15);
            lblAgendaHoy.TabIndex = 10;
            lblAgendaHoy.Text = "label4";
            // 
            // lblPlanesHistoricos
            // 
            lblPlanesHistoricos.AutoSize = true;
            lblPlanesHistoricos.Location = new Point(12, 358);
            lblPlanesHistoricos.Name = "lblPlanesHistoricos";
            lblPlanesHistoricos.Size = new Size(38, 15);
            lblPlanesHistoricos.TabIndex = 11;
            lblPlanesHistoricos.Text = "label1";
            // 
            // frmGestionNutricion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(872, 689);
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