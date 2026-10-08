namespace GUI
{
    partial class frmControlCambios
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
            cmbUsuarios = new ComboBox();
            dgvHistorial = new DataGridView();
            btnRestaurar = new Button();
            btnVolver = new Button();
            btnModificarJerarquia = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            SuspendLayout();
            // 
            // cmbUsuarios
            // 
            cmbUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbUsuarios.BackColor = Color.White;
            cmbUsuarios.FlatStyle = FlatStyle.Flat;
            cmbUsuarios.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbUsuarios.ForeColor = Color.FromArgb(31, 41, 55);
            cmbUsuarios.FormattingEnabled = true;
            cmbUsuarios.Location = new Point(28, 48);
            cmbUsuarios.Name = "cmbUsuarios";
            cmbUsuarios.Size = new Size(280, 28);
            cmbUsuarios.TabIndex = 0;
            cmbUsuarios.SelectedIndexChanged += cmbUsuarios_SelectedIndexChanged;
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.BackgroundColor = Color.White;
            dgvHistorial.BorderStyle = BorderStyle.None;
            dgvHistorial.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistorial.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistorial.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvHistorial.EnableHeadersVisualStyles = false;
            dgvHistorial.GridColor = Color.FromArgb(226, 232, 240);
            dgvHistorial.Location = new Point(28, 100);
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.RowTemplate.Height = 36;
            dgvHistorial.Size = new Size(1144, 544);
            dgvHistorial.TabIndex = 1;
            // 
            // btnRestaurar
            // 
            btnRestaurar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnRestaurar.BackColor = Color.FromArgb(37, 99, 235);
            btnRestaurar.FlatAppearance.BorderSize = 0;
            btnRestaurar.FlatStyle = FlatStyle.Flat;
            btnRestaurar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnRestaurar.ForeColor = Color.White;
            btnRestaurar.Location = new Point(340, 44);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(170, 42);
            btnRestaurar.TabIndex = 2;
            btnRestaurar.Text = "Restaurar";
            btnRestaurar.UseVisualStyleBackColor = false;
            btnRestaurar.Click += btnRestaurar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnVolver.BackColor = Color.FromArgb(71, 85, 105);
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(1032, 660);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(140, 42);
            btnVolver.TabIndex = 5;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // btnModificarJerarquia
            // 
            btnModificarJerarquia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnModificarJerarquia.BackColor = Color.FromArgb(71, 85, 105);
            btnModificarJerarquia.FlatAppearance.BorderSize = 0;
            btnModificarJerarquia.FlatStyle = FlatStyle.Flat;
            btnModificarJerarquia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnModificarJerarquia.ForeColor = Color.White;
            btnModificarJerarquia.Location = new Point(530, 44);
            btnModificarJerarquia.Name = "btnModificarJerarquia";
            btnModificarJerarquia.Size = new Size(190, 42);
            btnModificarJerarquia.TabIndex = 6;
            btnModificarJerarquia.Text = "Modificar Jerarquía";
            btnModificarJerarquia.UseVisualStyleBackColor = false;
            btnModificarJerarquia.Click += btnModificarJerarquia_Click;
            // 
            // frmControlCambios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 720);
            Controls.Add(btnModificarJerarquia);
            Controls.Add(btnVolver);
            Controls.Add(btnRestaurar);
            Controls.Add(dgvHistorial);
            Controls.Add(cmbUsuarios);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmControlCambios";
            Text = "Control de Cambios";
            FormClosing += frmControlCambios_FormClosing;
            Load += frmControlCambios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cmbUsuarios;
        private DataGridView dgvHistorial;
        private Button btnRestaurar;
        private Button btnVolver;
        private Button btnModificarJerarquia;
    }
}