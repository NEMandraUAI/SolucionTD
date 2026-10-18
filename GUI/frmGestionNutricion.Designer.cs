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
            pnlGraficoNutricion = new Panel();
            pnlCalculadoraBiometrica = new Panel();
            lblPeso = new Label();
            numPeso = new NumericUpDown();
            lblAltura = new Label();
            numAltura = new NumericUpDown();
            lblEdad = new Label();
            numEdad = new NumericUpDown();
            lblGenero = new Label();
            cmbGenero = new ComboBox();
            lblObjetivoNut = new Label();
            cmbObjetivoNutricional = new ComboBox();
            lblValorIMC = new Label();
            lblValorTMB = new Label();
            lblValorCalorias = new Label();
            lblAlertaIMC = new Label();
            btnSugerirHabitos = new Button();
            txtHabitos = new TextBox();
            btnAsignarPlan = new Button();
            btnExportarPDF = new Button();
            lblAntropometria = new Label();
            lblHabitos = new Label();
            lblAgendaHoy = new Label();
            lblPlanesHistoricos = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvAgendaHoy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPlanesHistoricos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPeso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numAltura).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numEdad).BeginInit();
            pnlCalculadoraBiometrica.SuspendLayout();
            SuspendLayout();
            // 
            // lblAgendaHoy
            // 
            lblAgendaHoy.AutoSize = true;
            lblAgendaHoy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblAgendaHoy.ForeColor = Color.FromArgb(31, 41, 55);
            lblAgendaHoy.Location = new Point(28, 22);
            lblAgendaHoy.Name = "lblAgendaHoy";
            lblAgendaHoy.Size = new Size(124, 15);
            lblAgendaHoy.TabIndex = 10;
            lblAgendaHoy.Text = "Mi Agenda para Hoy:";
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
            dgvAgendaHoy.Location = new Point(28, 46);
            dgvAgendaHoy.Name = "dgvAgendaHoy";
            dgvAgendaHoy.ReadOnly = true;
            dgvAgendaHoy.RowHeadersVisible = false;
            dgvAgendaHoy.RowTemplate.Height = 34;
            dgvAgendaHoy.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAgendaHoy.Size = new Size(520, 230);
            dgvAgendaHoy.TabIndex = 0;
            dgvAgendaHoy.CellClick += dgvAgendaHoy_CellClick;
            // 
            // lblDNISocio
            // 
            lblDNISocio.AutoSize = true;
            lblDNISocio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblDNISocio.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNISocio.Location = new Point(28, 294);
            lblDNISocio.Name = "lblDNISocio";
            lblDNISocio.Size = new Size(83, 15);
            lblDNISocio.TabIndex = 3;
            lblDNISocio.Text = "DNI del Socio:";
            // 
            // txtDNISocio
            // 
            txtDNISocio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNISocio.BackColor = Color.White;
            txtDNISocio.BorderStyle = BorderStyle.FixedSingle;
            txtDNISocio.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNISocio.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNISocio.Location = new Point(28, 318);
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
            btnBuscarSocio.Location = new Point(366, 310);
            btnBuscarSocio.Name = "btnBuscarSocio";
            btnBuscarSocio.Size = new Size(182, 38);
            btnBuscarSocio.TabIndex = 2;
            btnBuscarSocio.Text = "Buscar Socio";
            btnBuscarSocio.UseVisualStyleBackColor = false;
            btnBuscarSocio.Click += btnBuscarSocio_Click;
            // 
            // lblPlanesHistoricos
            // 
            lblPlanesHistoricos.AutoSize = true;
            lblPlanesHistoricos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblPlanesHistoricos.ForeColor = Color.FromArgb(31, 41, 55);
            lblPlanesHistoricos.Location = new Point(28, 366);
            lblPlanesHistoricos.Name = "lblPlanesHistoricos";
            lblPlanesHistoricos.Size = new Size(112, 15);
            lblPlanesHistoricos.TabIndex = 11;
            lblPlanesHistoricos.Text = "Historial de Planes:";
            // 
            // dgvPlanesHistoricos
            // 
            dgvPlanesHistoricos.AllowUserToAddRows = false;
            dgvPlanesHistoricos.AllowUserToDeleteRows = false;
            dgvPlanesHistoricos.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvPlanesHistoricos.Anchor = AnchorStyles.Top | AnchorStyles.Left;
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
            dgvPlanesHistoricos.Location = new Point(28, 390);
            dgvPlanesHistoricos.Name = "dgvPlanesHistoricos";
            dgvPlanesHistoricos.ReadOnly = true;
            dgvPlanesHistoricos.RowHeadersVisible = false;
            dgvPlanesHistoricos.RowTemplate.Height = 32;
            dgvPlanesHistoricos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanesHistoricos.Size = new Size(520, 150);
            dgvPlanesHistoricos.TabIndex = 4;
            dgvPlanesHistoricos.CellClick += dgvPlanesHistoricos_CellClick;
            // 
            // pnlGraficoNutricion
            // 
            pnlGraficoNutricion.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlGraficoNutricion.BackColor = Color.White;
            pnlGraficoNutricion.BorderStyle = BorderStyle.FixedSingle;
            pnlGraficoNutricion.Location = new Point(28, 550);
            pnlGraficoNutricion.Name = "pnlGraficoNutricion";
            pnlGraficoNutricion.Size = new Size(520, 190);
            pnlGraficoNutricion.TabIndex = 14;
            pnlGraficoNutricion.Paint += pnlGraficoNutricion_Paint;
            // 
            // lblAntropometria
            // 
            lblAntropometria.AutoSize = true;
            lblAntropometria.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblAntropometria.ForeColor = Color.FromArgb(31, 41, 55);
            lblAntropometria.Location = new Point(576, 22);
            lblAntropometria.Name = "lblAntropometria";
            lblAntropometria.Size = new Size(162, 15);
            lblAntropometria.TabIndex = 8;
            lblAntropometria.Text = "Evaluación Antropométrica:";
            // 
            // pnlCalculadoraBiometrica
            // 
            pnlCalculadoraBiometrica.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCalculadoraBiometrica.BackColor = Color.White;
            pnlCalculadoraBiometrica.BorderStyle = BorderStyle.FixedSingle;
            pnlCalculadoraBiometrica.Controls.Add(lblPeso);
            pnlCalculadoraBiometrica.Controls.Add(numPeso);
            pnlCalculadoraBiometrica.Controls.Add(lblAltura);
            pnlCalculadoraBiometrica.Controls.Add(numAltura);
            pnlCalculadoraBiometrica.Controls.Add(lblEdad);
            pnlCalculadoraBiometrica.Controls.Add(numEdad);
            pnlCalculadoraBiometrica.Controls.Add(lblGenero);
            pnlCalculadoraBiometrica.Controls.Add(cmbGenero);
            pnlCalculadoraBiometrica.Controls.Add(lblObjetivoNut);
            pnlCalculadoraBiometrica.Controls.Add(cmbObjetivoNutricional);
            pnlCalculadoraBiometrica.Controls.Add(lblValorIMC);
            pnlCalculadoraBiometrica.Controls.Add(lblValorTMB);
            pnlCalculadoraBiometrica.Controls.Add(lblValorCalorias);
            pnlCalculadoraBiometrica.Controls.Add(lblAlertaIMC);
            pnlCalculadoraBiometrica.Location = new Point(576, 46);
            pnlCalculadoraBiometrica.Name = "pnlCalculadoraBiometrica";
            pnlCalculadoraBiometrica.Size = new Size(596, 268);
            pnlCalculadoraBiometrica.TabIndex = 5;
            // 
            // lblPeso
            // 
            lblPeso.AutoSize = true;
            lblPeso.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblPeso.Location = new Point(16, 14);
            lblPeso.Name = "lblPeso";
            lblPeso.Size = new Size(60, 15);
            lblPeso.Text = "Peso (kg):";
            // 
            // numPeso
            // 
            numPeso.DecimalPlaces = 1;
            numPeso.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            numPeso.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numPeso.Location = new Point(16, 34);
            numPeso.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            numPeso.Minimum = new decimal(new int[] { 25, 0, 0, 0 });
            numPeso.Name = "numPeso";
            numPeso.Size = new Size(120, 24);
            numPeso.Value = new decimal(new int[] { 75, 0, 0, 0 });
            numPeso.ValueChanged += ParametrosBiometricos_Changed;
            // 
            // lblAltura
            // 
            lblAltura.AutoSize = true;
            lblAltura.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblAltura.Location = new Point(156, 14);
            lblAltura.Name = "lblAltura";
            lblAltura.Size = new Size(70, 15);
            lblAltura.Text = "Altura (cm):";
            // 
            // numAltura
            // 
            numAltura.DecimalPlaces = 0;
            numAltura.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            numAltura.Location = new Point(156, 34);
            numAltura.Maximum = new decimal(new int[] { 250, 0, 0, 0 });
            numAltura.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numAltura.Name = "numAltura";
            numAltura.Size = new Size(120, 24);
            numAltura.Value = new decimal(new int[] { 175, 0, 0, 0 });
            numAltura.ValueChanged += ParametrosBiometricos_Changed;
            // 
            // lblEdad
            // 
            lblEdad.AutoSize = true;
            lblEdad.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblEdad.Location = new Point(296, 14);
            lblEdad.Name = "lblEdad";
            lblEdad.Size = new Size(72, 15);
            lblEdad.Text = "Edad (años):";
            // 
            // numEdad
            // 
            numEdad.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            numEdad.Location = new Point(296, 34);
            numEdad.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numEdad.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numEdad.Name = "numEdad";
            numEdad.Size = new Size(105, 24);
            numEdad.Value = new decimal(new int[] { 25, 0, 0, 0 });
            numEdad.ValueChanged += ParametrosBiometricos_Changed;
            // 
            // lblGenero
            // 
            lblGenero.AutoSize = true;
            lblGenero.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblGenero.Location = new Point(420, 14);
            lblGenero.Name = "lblGenero";
            lblGenero.Size = new Size(50, 15);
            lblGenero.Text = "Género:";
            // 
            // cmbGenero
            // 
            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGenero.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            cmbGenero.Location = new Point(420, 34);
            cmbGenero.Name = "cmbGenero";
            cmbGenero.Size = new Size(156, 24);
            cmbGenero.SelectedIndexChanged += ParametrosBiometricos_Changed;
            // 
            // lblObjetivoNut
            // 
            lblObjetivoNut.AutoSize = true;
            lblObjetivoNut.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblObjetivoNut.Location = new Point(16, 72);
            lblObjetivoNut.Name = "lblObjetivoNut";
            lblObjetivoNut.Size = new Size(195, 15);
            lblObjetivoNut.Text = "Protocolo / Objetivo Nutricional:";
            // 
            // cmbObjetivoNutricional
            // 
            cmbObjetivoNutricional.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbObjetivoNutricional.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            cmbObjetivoNutricional.Location = new Point(16, 92);
            cmbObjetivoNutricional.Name = "cmbObjetivoNutricional";
            cmbObjetivoNutricional.Size = new Size(560, 24);
            cmbObjetivoNutricional.SelectedIndexChanged += ParametrosBiometricos_Changed;
            // 
            // lblValorIMC
            // 
            lblValorIMC.BackColor = Color.FromArgb(241, 245, 249);
            lblValorIMC.BorderStyle = BorderStyle.FixedSingle;
            lblValorIMC.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblValorIMC.ForeColor = Color.FromArgb(30, 41, 59);
            lblValorIMC.Location = new Point(16, 130);
            lblValorIMC.Name = "lblValorIMC";
            lblValorIMC.Size = new Size(180, 54);
            lblValorIMC.Text = "IMC Calculado:\r\n24.49 kg/m²";
            lblValorIMC.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblValorTMB
            // 
            lblValorTMB.BackColor = Color.FromArgb(241, 245, 249);
            lblValorTMB.BorderStyle = BorderStyle.FixedSingle;
            lblValorTMB.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblValorTMB.ForeColor = Color.FromArgb(30, 41, 59);
            lblValorTMB.Location = new Point(206, 130);
            lblValorTMB.Name = "lblValorTMB";
            lblValorTMB.Size = new Size(184, 54);
            lblValorTMB.Text = "TMB (Harris-Benedict):\r\n1804 kcal/día";
            lblValorTMB.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblValorCalorias
            // 
            lblValorCalorias.BackColor = Color.FromArgb(239, 246, 255);
            lblValorCalorias.BorderStyle = BorderStyle.FixedSingle;
            lblValorCalorias.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblValorCalorias.ForeColor = Color.FromArgb(29, 78, 216);
            lblValorCalorias.Location = new Point(400, 130);
            lblValorCalorias.Name = "lblValorCalorias";
            lblValorCalorias.Size = new Size(176, 54);
            lblValorCalorias.Text = "Meta Diaria Sugerida:\r\n2480 kcal/día";
            lblValorCalorias.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAlertaIMC
            // 
            lblAlertaIMC.BackColor = Color.FromArgb(220, 252, 231);
            lblAlertaIMC.BorderStyle = BorderStyle.FixedSingle;
            lblAlertaIMC.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblAlertaIMC.ForeColor = Color.FromArgb(22, 101, 52);
            lblAlertaIMC.Location = new Point(16, 196);
            lblAlertaIMC.Name = "lblAlertaIMC";
            lblAlertaIMC.Padding = new Padding(8, 4, 8, 4);
            lblAlertaIMC.Size = new Size(560, 56);
            lblAlertaIMC.Text = "✓ ESTADO NORMAL: IMC dentro del rango saludable.";
            lblAlertaIMC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHabitos
            // 
            lblHabitos.AutoSize = true;
            lblHabitos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblHabitos.ForeColor = Color.FromArgb(31, 41, 55);
            lblHabitos.Location = new Point(576, 332);
            lblHabitos.Name = "lblHabitos";
            lblHabitos.Size = new Size(110, 15);
            lblHabitos.TabIndex = 9;
            lblHabitos.Text = "Detalle de Hábitos:";
            // 
            // btnSugerirHabitos
            // 
            btnSugerirHabitos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSugerirHabitos.BackColor = Color.FromArgb(16, 185, 129);
            btnSugerirHabitos.FlatAppearance.BorderSize = 0;
            btnSugerirHabitos.FlatStyle = FlatStyle.Flat;
            btnSugerirHabitos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnSugerirHabitos.ForeColor = Color.White;
            btnSugerirHabitos.Location = new Point(982, 322);
            btnSugerirHabitos.Name = "btnSugerirHabitos";
            btnSugerirHabitos.Size = new Size(190, 32);
            btnSugerirHabitos.TabIndex = 12;
            btnSugerirHabitos.Text = "Sugerir Pauta Nutricional";
            btnSugerirHabitos.UseVisualStyleBackColor = false;
            btnSugerirHabitos.Click += btnSugerirHabitos_Click;
            // 
            // txtHabitos
            // 
            txtHabitos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtHabitos.BackColor = Color.White;
            txtHabitos.BorderStyle = BorderStyle.FixedSingle;
            txtHabitos.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtHabitos.ForeColor = Color.FromArgb(31, 41, 55);
            txtHabitos.Location = new Point(576, 360);
            txtHabitos.Multiline = true;
            txtHabitos.Name = "txtHabitos";
            txtHabitos.ScrollBars = ScrollBars.Vertical;
            txtHabitos.Size = new Size(596, 314);
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
            btnAsignarPlan.Location = new Point(576, 692);
            btnAsignarPlan.Name = "btnAsignarPlan";
            btnAsignarPlan.Size = new Size(374, 48);
            btnAsignarPlan.TabIndex = 7;
            btnAsignarPlan.Text = "Generar y Asignar Plan";
            btnAsignarPlan.UseVisualStyleBackColor = false;
            btnAsignarPlan.Click += btnAsignarPlan_Click;
            // 
            // btnExportarPDF
            // 
            btnExportarPDF.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportarPDF.BackColor = Color.FromArgb(71, 85, 105);
            btnExportarPDF.FlatAppearance.BorderSize = 0;
            btnExportarPDF.FlatStyle = FlatStyle.Flat;
            btnExportarPDF.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnExportarPDF.ForeColor = Color.White;
            btnExportarPDF.Location = new Point(962, 692);
            btnExportarPDF.Name = "btnExportarPDF";
            btnExportarPDF.Size = new Size(210, 48);
            btnExportarPDF.TabIndex = 13;
            btnExportarPDF.Text = "Exportar Informe PDF";
            btnExportarPDF.UseVisualStyleBackColor = false;
            btnExportarPDF.Click += btnExportarPDF_Click;
            // 
            // frmGestionNutricion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 770);
            Controls.Add(btnExportarPDF);
            Controls.Add(btnSugerirHabitos);
            Controls.Add(lblPlanesHistoricos);
            Controls.Add(lblAgendaHoy);
            Controls.Add(lblHabitos);
            Controls.Add(lblAntropometria);
            Controls.Add(btnAsignarPlan);
            Controls.Add(txtHabitos);
            Controls.Add(pnlCalculadoraBiometrica);
            Controls.Add(dgvPlanesHistoricos);
            Controls.Add(pnlGraficoNutricion);
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
            ((System.ComponentModel.ISupportInitialize)numPeso).EndInit();
            ((System.ComponentModel.ISupportInitialize)numAltura).EndInit();
            ((System.ComponentModel.ISupportInitialize)numEdad).EndInit();
            pnlCalculadoraBiometrica.ResumeLayout(false);
            pnlCalculadoraBiometrica.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvAgendaHoy;
        private TextBox txtDNISocio;
        private Button btnBuscarSocio;
        private Label lblDNISocio;
        private DataGridView dgvPlanesHistoricos;
        private Panel pnlGraficoNutricion;
        private Panel pnlCalculadoraBiometrica;
        private Label lblPeso;
        private NumericUpDown numPeso;
        private Label lblAltura;
        private NumericUpDown numAltura;
        private Label lblEdad;
        private NumericUpDown numEdad;
        private Label lblGenero;
        private ComboBox cmbGenero;
        private Label lblObjetivoNut;
        private ComboBox cmbObjetivoNutricional;
        private Label lblValorIMC;
        private Label lblValorTMB;
        private Label lblValorCalorias;
        private Label lblAlertaIMC;
        private Button btnSugerirHabitos;
        private TextBox txtHabitos;
        private Button btnAsignarPlan;
        private Button btnExportarPDF;
        private Label lblAntropometria;
        private Label lblHabitos;
        private Label lblAgendaHoy;
        private Label lblPlanesHistoricos;
    }
}