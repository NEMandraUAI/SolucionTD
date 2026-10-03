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
            dtpFechaAgenda.Location = new Point(12, 27);
            dtpFechaAgenda.Name = "dtpFechaAgenda";
            dtpFechaAgenda.Size = new Size(240, 23);
            dtpFechaAgenda.TabIndex = 0;
            dtpFechaAgenda.ValueChanged += dtpFechaAgenda_ValueChanged;
            // 
            // dgvAgenda
            // 
            dgvAgenda.AllowUserToAddRows = false;
            dgvAgenda.AllowUserToDeleteRows = false;
            dgvAgenda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAgenda.Location = new Point(258, 27);
            dgvAgenda.Name = "dgvAgenda";
            dgvAgenda.ReadOnly = true;
            dgvAgenda.Size = new Size(530, 311);
            dgvAgenda.TabIndex = 1;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(12, 115);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(240, 23);
            txtDNI.TabIndex = 2;
            // 
            // cmbNutricionista
            // 
            cmbNutricionista.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNutricionista.FormattingEnabled = true;
            cmbNutricionista.Location = new Point(12, 159);
            cmbNutricionista.Name = "cmbNutricionista";
            cmbNutricionista.Size = new Size(240, 23);
            cmbNutricionista.TabIndex = 3;
            // 
            // dtpHoraTurno
            // 
            dtpHoraTurno.Format = DateTimePickerFormat.Time;
            dtpHoraTurno.Location = new Point(12, 71);
            dtpHoraTurno.Name = "dtpHoraTurno";
            dtpHoraTurno.ShowUpDown = true;
            dtpHoraTurno.Size = new Size(240, 23);
            dtpHoraTurno.TabIndex = 4;
            // 
            // lblFechaAgenda
            // 
            lblFechaAgenda.AutoSize = true;
            lblFechaAgenda.Location = new Point(12, 9);
            lblFechaAgenda.Name = "lblFechaAgenda";
            lblFechaAgenda.Size = new Size(38, 15);
            lblFechaAgenda.TabIndex = 5;
            lblFechaAgenda.Text = "label1";
            // 
            // lblHoraTurno
            // 
            lblHoraTurno.AutoSize = true;
            lblHoraTurno.Location = new Point(12, 53);
            lblHoraTurno.Name = "lblHoraTurno";
            lblHoraTurno.Size = new Size(38, 15);
            lblHoraTurno.TabIndex = 6;
            lblHoraTurno.Text = "label1";
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Location = new Point(12, 97);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(38, 15);
            lblDNI.TabIndex = 7;
            lblDNI.Text = "label1";
            // 
            // lblNutricionista
            // 
            lblNutricionista.AutoSize = true;
            lblNutricionista.Location = new Point(12, 141);
            lblNutricionista.Name = "lblNutricionista";
            lblNutricionista.Size = new Size(38, 15);
            lblNutricionista.TabIndex = 8;
            lblNutricionista.Text = "label1";
            // 
            // btnReservar
            // 
            btnReservar.Location = new Point(12, 188);
            btnReservar.Name = "btnReservar";
            btnReservar.Size = new Size(240, 46);
            btnReservar.TabIndex = 9;
            btnReservar.Text = "button1";
            btnReservar.UseVisualStyleBackColor = true;
            btnReservar.Click += btnReservar_Click;
            // 
            // btnAsistio
            // 
            btnAsistio.Location = new Point(12, 240);
            btnAsistio.Name = "btnAsistio";
            btnAsistio.Size = new Size(240, 46);
            btnAsistio.TabIndex = 10;
            btnAsistio.Text = "button2";
            btnAsistio.UseVisualStyleBackColor = true;
            btnAsistio.Click += btnAsistio_Click;
            // 
            // btnAusente
            // 
            btnAusente.Location = new Point(12, 292);
            btnAusente.Name = "btnAusente";
            btnAusente.Size = new Size(240, 46);
            btnAusente.TabIndex = 11;
            btnAusente.Text = "button3";
            btnAusente.UseVisualStyleBackColor = true;
            btnAusente.Click += btnAusente_Click;
            // 
            // frmAgendaRecepcion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 356);
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