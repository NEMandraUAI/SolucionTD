namespace GUI
{
    partial class frmAgendaRecepcion
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
            dtpFechaAgenda = new DateTimePicker();
            dgvAgenda = new DataGridView();
            txtDNI = new TextBox();
            cmbNutricionista = new ComboBox();
            dtpHoraTurno = new DateTimePicker();
            lblFechaAgenda = new Label();
            lblHoraTurno = new Label();
            lblDNI = new Label();
            lblNutricionista = new Label();
            btnReservar = new Button();
            btnAsistio = new Button();
            btnAusente = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAgenda).BeginInit();
            SuspendLayout();
            // 
            // dtpFechaAgenda
            // 
            dtpFechaAgenda.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtpFechaAgenda.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpFechaAgenda.Location = new Point(28, 58);
            dtpFechaAgenda.Name = "dtpFechaAgenda";
            dtpFechaAgenda.Size = new Size(280, 23);
            dtpFechaAgenda.TabIndex = 0;
            dtpFechaAgenda.ValueChanged += dtpFechaAgenda_ValueChanged;
            // 
            // dgvAgenda
            // 
            dgvAgenda.AllowUserToAddRows = false;
            dgvAgenda.AllowUserToDeleteRows = false;
            dgvAgenda.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvAgenda.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAgenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAgenda.BackgroundColor = Color.White;
            dgvAgenda.BorderStyle = BorderStyle.None;
            dgvAgenda.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAgenda.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAgenda.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, BackColor = Color.FromArgb(31, 41, 55), Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.White, SelectionBackColor = Color.FromArgb(31, 41, 55), SelectionForeColor = Color.White, WrapMode = DataGridViewTriState.False };
            dgvAgenda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAgenda.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Color.FromArgb(31, 41, 55), SelectionBackColor = Color.FromArgb(219, 234, 254), SelectionForeColor = Color.FromArgb(31, 41, 55), Padding = new Padding(8, 4, 8, 4) };
            dgvAgenda.EnableHeadersVisualStyles = false;
            dgvAgenda.GridColor = Color.FromArgb(226, 232, 240);
            dgvAgenda.Location = new Point(340, 28);
            dgvAgenda.Name = "dgvAgenda";
            dgvAgenda.ReadOnly = true;
            dgvAgenda.RowHeadersVisible = false;
            dgvAgenda.RowTemplate.Height = 36;
            dgvAgenda.Size = new Size(832, 664);
            dgvAgenda.TabIndex = 1;
            // 
            // txtDNI
            // 
            txtDNI.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(28, 242);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(280, 23);
            txtDNI.TabIndex = 2;
            // 
            // cmbNutricionista
            // 
            cmbNutricionista.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbNutricionista.BackColor = Color.White;
            cmbNutricionista.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNutricionista.FlatStyle = FlatStyle.Flat;
            cmbNutricionista.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbNutricionista.ForeColor = Color.FromArgb(31, 41, 55);
            cmbNutricionista.FormattingEnabled = true;
            cmbNutricionista.Location = new Point(28, 334);
            cmbNutricionista.Name = "cmbNutricionista";
            cmbNutricionista.Size = new Size(280, 23);
            cmbNutricionista.TabIndex = 3;
            // 
            // dtpHoraTurno
            // 
            dtpHoraTurno.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtpHoraTurno.Format = DateTimePickerFormat.Time;
            dtpHoraTurno.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpHoraTurno.Location = new Point(28, 150);
            dtpHoraTurno.Name = "dtpHoraTurno";
            dtpHoraTurno.ShowUpDown = true;
            dtpHoraTurno.Size = new Size(280, 23);
            dtpHoraTurno.TabIndex = 4;
            // 
            // lblFechaAgenda
            // 
            lblFechaAgenda.AutoSize = true;
            lblFechaAgenda.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblFechaAgenda.ForeColor = Color.FromArgb(31, 41, 55);
            lblFechaAgenda.Location = new Point(28, 32);
            lblFechaAgenda.Name = "lblFechaAgenda";
            lblFechaAgenda.Size = new Size(41, 15);
            lblFechaAgenda.TabIndex = 5;
            lblFechaAgenda.Text = "label1";
            // 
            // lblHoraTurno
            // 
            lblHoraTurno.AutoSize = true;
            lblHoraTurno.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblHoraTurno.ForeColor = Color.FromArgb(31, 41, 55);
            lblHoraTurno.Location = new Point(28, 124);
            lblHoraTurno.Name = "lblHoraTurno";
            lblHoraTurno.Size = new Size(41, 15);
            lblHoraTurno.TabIndex = 6;
            lblHoraTurno.Text = "label1";
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(28, 216);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(41, 15);
            lblDNI.TabIndex = 7;
            lblDNI.Text = "label1";
            // 
            // lblNutricionista
            // 
            lblNutricionista.AutoSize = true;
            lblNutricionista.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblNutricionista.ForeColor = Color.FromArgb(31, 41, 55);
            lblNutricionista.Location = new Point(28, 308);
            lblNutricionista.Name = "lblNutricionista";
            lblNutricionista.Size = new Size(41, 15);
            lblNutricionista.TabIndex = 8;
            lblNutricionista.Text = "label1";
            // 
            // btnReservar
            // 
            btnReservar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnReservar.BackColor = Color.FromArgb(37, 99, 235);
            btnReservar.FlatAppearance.BorderSize = 0;
            btnReservar.FlatStyle = FlatStyle.Flat;
            btnReservar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnReservar.ForeColor = Color.White;
            btnReservar.Location = new Point(28, 410);
            btnReservar.Name = "btnReservar";
            btnReservar.Size = new Size(280, 46);
            btnReservar.TabIndex = 9;
            btnReservar.Text = "button1";
            btnReservar.UseVisualStyleBackColor = false;
            btnReservar.Click += btnReservar_Click;
            // 
            // btnAsistio
            // 
            btnAsistio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAsistio.BackColor = Color.FromArgb(21, 128, 61);
            btnAsistio.FlatAppearance.BorderSize = 0;
            btnAsistio.FlatStyle = FlatStyle.Flat;
            btnAsistio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAsistio.ForeColor = Color.White;
            btnAsistio.Location = new Point(28, 466);
            btnAsistio.Name = "btnAsistio";
            btnAsistio.Size = new Size(280, 46);
            btnAsistio.TabIndex = 10;
            btnAsistio.Text = "button2";
            btnAsistio.UseVisualStyleBackColor = false;
            btnAsistio.Click += btnAsistio_Click;
            // 
            // btnAusente
            // 
            btnAusente.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAusente.BackColor = Color.FromArgb(185, 28, 28);
            btnAusente.FlatAppearance.BorderSize = 0;
            btnAusente.FlatStyle = FlatStyle.Flat;
            btnAusente.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAusente.ForeColor = Color.White;
            btnAusente.Location = new Point(28, 522);
            btnAusente.Name = "btnAusente";
            btnAusente.Size = new Size(280, 46);
            btnAusente.TabIndex = 11;
            btnAusente.Text = "button3";
            btnAusente.UseVisualStyleBackColor = false;
            btnAusente.Click += btnAusente_Click;
            // 
            // frmAgendaRecepcion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 720);
            Controls.Add(btnAusente);
            Controls.Add(btnAsistio);
            Controls.Add(btnReservar);
            Controls.Add(lblNutricionista);
            Controls.Add(lblDNI);
            Controls.Add(lblHoraTurno);
            Controls.Add(lblFechaAgenda);
            Controls.Add(dtpHoraTurno);
            Controls.Add(cmbNutricionista);
            Controls.Add(txtDNI);
            Controls.Add(dgvAgenda);
            Controls.Add(dtpFechaAgenda);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmAgendaRecepcion";
            Text = "frmAgendaRecepcion";
            FormClosing += frmAgendaRecepcion_FormClosing;
            Load += frmAgendaRecepcion_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAgenda).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpFechaAgenda;
        private DataGridView dgvAgenda;
        private TextBox txtDNI;
        private ComboBox cmbNutricionista;
        private DateTimePicker dtpHoraTurno;
        private Label lblFechaAgenda;
        private Label lblHoraTurno;
        private Label lblDNI;
        private Label lblNutricionista;
        private Button btnReservar;
        private Button btnAsistio;
        private Button btnAusente;
    }
}