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
            pnlFiltrosYAcciones = new Panel();
            lblDNI = new Label();
            txtDNI = new TextBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblMetodoFiltro = new Label();
            cmbMetodoFiltro = new ComboBox();
            btnFiltrar = new Button();
            btnLimpiar = new Button();
            btnCajaHoy = new Button();
            pnlGraficoMedios = new Panel();
            btnReimprimirRecibo = new Button();
            btnExportarArqueo = new Button();
            pnlKpiTotal = new Panel();
            pnlAccentTotal = new Panel();
            lblKpiTotalTitulo = new Label();
            lblKpiTotalValor = new Label();
            lblKpiTotalSub = new Label();
            pnlKpiEfectivo = new Panel();
            pnlAccentEfectivo = new Panel();
            lblKpiEfectivoTitulo = new Label();
            lblKpiEfectivoValor = new Label();
            lblKpiEfectivoSub = new Label();
            pnlKpiDigital = new Panel();
            pnlAccentDigital = new Panel();
            lblKpiDigitalTitulo = new Label();
            lblKpiDigitalValor = new Label();
            lblKpiDigitalSub = new Label();
            pnlKpiPromedio = new Panel();
            pnlAccentPromedio = new Panel();
            lblKpiPromedioTitulo = new Label();
            lblKpiPromedioValor = new Label();
            lblKpiPromedioSub = new Label();
            dgvComprobantes = new DataGridView();
            pnlFiltrosYAcciones.SuspendLayout();
            pnlKpiTotal.SuspendLayout();
            pnlKpiEfectivo.SuspendLayout();
            pnlKpiDigital.SuspendLayout();
            pnlKpiPromedio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvComprobantes).BeginInit();
            SuspendLayout();
            // 
            // pnlFiltrosYAcciones
            // 
            pnlFiltrosYAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlFiltrosYAcciones.BackColor = Color.White;
            pnlFiltrosYAcciones.BorderStyle = BorderStyle.FixedSingle;
            pnlFiltrosYAcciones.Controls.Add(lblDNI);
            pnlFiltrosYAcciones.Controls.Add(txtDNI);
            pnlFiltrosYAcciones.Controls.Add(lblDesde);
            pnlFiltrosYAcciones.Controls.Add(dtpDesde);
            pnlFiltrosYAcciones.Controls.Add(lblHasta);
            pnlFiltrosYAcciones.Controls.Add(dtpHasta);
            pnlFiltrosYAcciones.Controls.Add(lblMetodoFiltro);
            pnlFiltrosYAcciones.Controls.Add(cmbMetodoFiltro);
            pnlFiltrosYAcciones.Controls.Add(btnFiltrar);
            pnlFiltrosYAcciones.Controls.Add(btnLimpiar);
            pnlFiltrosYAcciones.Controls.Add(btnCajaHoy);
            pnlFiltrosYAcciones.Controls.Add(pnlGraficoMedios);
            pnlFiltrosYAcciones.Controls.Add(btnReimprimirRecibo);
            pnlFiltrosYAcciones.Controls.Add(btnExportarArqueo);
            pnlFiltrosYAcciones.Location = new Point(20, 20);
            pnlFiltrosYAcciones.Name = "pnlFiltrosYAcciones";
            pnlFiltrosYAcciones.Size = new Size(316, 680);
            pnlFiltrosYAcciones.TabIndex = 0;
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(18, 16);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(125, 15);
            lblDNI.TabIndex = 0;
            lblDNI.Text = "DNI Socio (Opcional):";
            // 
            // txtDNI
            // 
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 9.5F);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(18, 36);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(278, 24);
            txtDNI.TabIndex = 1;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDesde.ForeColor = Color.FromArgb(31, 41, 55);
            lblDesde.Location = new Point(18, 70);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(43, 15);
            lblDesde.TabIndex = 2;
            lblDesde.Text = "Desde:";
            // 
            // dtpDesde
            // 
            dtpDesde.Font = new Font("Segoe UI", 9F);
            dtpDesde.Location = new Point(18, 90);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(278, 23);
            dtpDesde.TabIndex = 3;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHasta.ForeColor = Color.FromArgb(31, 41, 55);
            lblHasta.Location = new Point(18, 124);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(41, 15);
            lblHasta.TabIndex = 4;
            lblHasta.Text = "Hasta:";
            // 
            // dtpHasta
            // 
            dtpHasta.Font = new Font("Segoe UI", 9F);
            dtpHasta.Location = new Point(18, 144);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(278, 23);
            dtpHasta.TabIndex = 5;
            // 
            // lblMetodoFiltro
            // 
            lblMetodoFiltro.AutoSize = true;
            lblMetodoFiltro.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMetodoFiltro.ForeColor = Color.FromArgb(31, 41, 55);
            lblMetodoFiltro.Location = new Point(18, 178);
            lblMetodoFiltro.Name = "lblMetodoFiltro";
            lblMetodoFiltro.Size = new Size(100, 15);
            lblMetodoFiltro.TabIndex = 6;
            lblMetodoFiltro.Text = "Método de Pago:";
            // 
            // cmbMetodoFiltro
            // 
            cmbMetodoFiltro.BackColor = Color.White;
            cmbMetodoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoFiltro.FlatStyle = FlatStyle.Flat;
            cmbMetodoFiltro.Font = new Font("Segoe UI", 9F);
            cmbMetodoFiltro.ForeColor = Color.FromArgb(31, 41, 55);
            cmbMetodoFiltro.Location = new Point(18, 198);
            cmbMetodoFiltro.Name = "cmbMetodoFiltro";
            cmbMetodoFiltro.Size = new Size(278, 23);
            cmbMetodoFiltro.TabIndex = 7;
            cmbMetodoFiltro.SelectedIndexChanged += cmbMetodoFiltro_SelectedIndexChanged;
            // 
            // btnFiltrar
            // 
            btnFiltrar.BackColor = Color.FromArgb(37, 99, 235);
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(18, 234);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(134, 36);
            btnFiltrar.TabIndex = 8;
            btnFiltrar.Text = "Aplicar Filtros";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(71, 85, 105);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(162, 234);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(134, 36);
            btnLimpiar.TabIndex = 9;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnCajaHoy
            // 
            btnCajaHoy.BackColor = Color.FromArgb(31, 41, 55);
            btnCajaHoy.FlatAppearance.BorderSize = 0;
            btnCajaHoy.FlatStyle = FlatStyle.Flat;
            btnCajaHoy.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCajaHoy.ForeColor = Color.White;
            btnCajaHoy.Location = new Point(18, 278);
            btnCajaHoy.Name = "btnCajaHoy";
            btnCajaHoy.Size = new Size(278, 34);
            btnCajaHoy.TabIndex = 10;
            btnCajaHoy.Text = "📅 Caja de Hoy";
            btnCajaHoy.UseVisualStyleBackColor = false;
            btnCajaHoy.Click += btnCajaHoy_Click;
            // 
            // pnlGraficoMedios
            // 
            pnlGraficoMedios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlGraficoMedios.BackColor = Color.FromArgb(248, 250, 252);
            pnlGraficoMedios.BorderStyle = BorderStyle.FixedSingle;
            pnlGraficoMedios.Location = new Point(18, 324);
            pnlGraficoMedios.Name = "pnlGraficoMedios";
            pnlGraficoMedios.Size = new Size(278, 240);
            pnlGraficoMedios.TabIndex = 11;
            pnlGraficoMedios.Paint += pnlGraficoMedios_Paint;
            // 
            // btnReimprimirRecibo
            // 
            btnReimprimirRecibo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnReimprimirRecibo.BackColor = Color.FromArgb(16, 185, 129);
            btnReimprimirRecibo.FlatAppearance.BorderSize = 0;
            btnReimprimirRecibo.FlatStyle = FlatStyle.Flat;
            btnReimprimirRecibo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReimprimirRecibo.ForeColor = Color.White;
            btnReimprimirRecibo.Location = new Point(18, 576);
            btnReimprimirRecibo.Name = "btnReimprimirRecibo";
            btnReimprimirRecibo.Size = new Size(278, 40);
            btnReimprimirRecibo.TabIndex = 12;
            btnReimprimirRecibo.Text = "📄 Emitir Recibo PDF";
            btnReimprimirRecibo.UseVisualStyleBackColor = false;
            btnReimprimirRecibo.Click += btnReimprimirRecibo_Click;
            // 
            // btnExportarArqueo
            // 
            btnExportarArqueo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportarArqueo.BackColor = Color.FromArgb(124, 58, 237);
            btnExportarArqueo.FlatAppearance.BorderSize = 0;
            btnExportarArqueo.FlatStyle = FlatStyle.Flat;
            btnExportarArqueo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportarArqueo.ForeColor = Color.White;
            btnExportarArqueo.Location = new Point(18, 624);
            btnExportarArqueo.Name = "btnExportarArqueo";
            btnExportarArqueo.Size = new Size(278, 40);
            btnExportarArqueo.TabIndex = 13;
            btnExportarArqueo.Text = "📊 Exportar Arqueo PDF";
            btnExportarArqueo.UseVisualStyleBackColor = false;
            btnExportarArqueo.Click += btnExportarArqueo_Click;
            // 
            // pnlKpiTotal
            // 
            pnlKpiTotal.BackColor = Color.White;
            pnlKpiTotal.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiTotal.Controls.Add(pnlAccentTotal);
            pnlKpiTotal.Controls.Add(lblKpiTotalTitulo);
            pnlKpiTotal.Controls.Add(lblKpiTotalValor);
            pnlKpiTotal.Controls.Add(lblKpiTotalSub);
            pnlKpiTotal.Location = new Point(352, 20);
            pnlKpiTotal.Name = "pnlKpiTotal";
            pnlKpiTotal.Size = new Size(240, 96);
            pnlKpiTotal.TabIndex = 1;
            // 
            // pnlAccentTotal
            // 
            pnlAccentTotal.BackColor = Color.FromArgb(37, 99, 235);
            pnlAccentTotal.Dock = DockStyle.Top;
            pnlAccentTotal.Location = new Point(0, 0);
            pnlAccentTotal.Name = "pnlAccentTotal";
            pnlAccentTotal.Size = new Size(238, 5);
            pnlAccentTotal.TabIndex = 0;
            // 
            // lblKpiTotalTitulo
            // 
            lblKpiTotalTitulo.AutoSize = true;
            lblKpiTotalTitulo.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
            lblKpiTotalTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpiTotalTitulo.Location = new Point(14, 12);
            lblKpiTotalTitulo.Name = "lblKpiTotalTitulo";
            lblKpiTotalTitulo.Size = new Size(165, 13);
            lblKpiTotalTitulo.TabIndex = 1;
            lblKpiTotalTitulo.Text = "RECAUDACIÓN TOTAL FILTRADA";
            // 
            // lblKpiTotalValor
            // 
            lblKpiTotalValor.AutoSize = true;
            lblKpiTotalValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblKpiTotalValor.ForeColor = Color.FromArgb(37, 99, 235);
            lblKpiTotalValor.Location = new Point(12, 32);
            lblKpiTotalValor.Name = "lblKpiTotalValor";
            lblKpiTotalValor.Size = new Size(116, 30);
            lblKpiTotalValor.TabIndex = 2;
            lblKpiTotalValor.Text = "$ 0,00";
            // 
            // lblKpiTotalSub
            // 
            lblKpiTotalSub.AutoSize = true;
            lblKpiTotalSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblKpiTotalSub.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpiTotalSub.Location = new Point(14, 68);
            lblKpiTotalSub.Name = "lblKpiTotalSub";
            lblKpiTotalSub.Size = new Size(88, 15);
            lblKpiTotalSub.TabIndex = 3;
            lblKpiTotalSub.Text = "0 operación(es)";
            // 
            // pnlKpiEfectivo
            // 
            pnlKpiEfectivo.BackColor = Color.White;
            pnlKpiEfectivo.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiEfectivo.Controls.Add(pnlAccentEfectivo);
            pnlKpiEfectivo.Controls.Add(lblKpiEfectivoTitulo);
            pnlKpiEfectivo.Controls.Add(lblKpiEfectivoValor);
            pnlKpiEfectivo.Controls.Add(lblKpiEfectivoSub);
            pnlKpiEfectivo.Location = new Point(606, 20);
            pnlKpiEfectivo.Name = "pnlKpiEfectivo";
            pnlKpiEfectivo.Size = new Size(240, 96);
            pnlKpiEfectivo.TabIndex = 2;
            // 
            // pnlAccentEfectivo
            // 
            pnlAccentEfectivo.BackColor = Color.FromArgb(16, 185, 129);
            pnlAccentEfectivo.Dock = DockStyle.Top;
            pnlAccentEfectivo.Location = new Point(0, 0);
            pnlAccentEfectivo.Name = "pnlAccentEfectivo";
            pnlAccentEfectivo.Size = new Size(238, 5);
            pnlAccentEfectivo.TabIndex = 0;
            // 
            // lblKpiEfectivoTitulo
            // 
            lblKpiEfectivoTitulo.AutoSize = true;
            lblKpiEfectivoTitulo.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
            lblKpiEfectivoTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpiEfectivoTitulo.Location = new Point(14, 12);
            lblKpiEfectivoTitulo.Name = "lblKpiEfectivoTitulo";
            lblKpiEfectivoTitulo.Size = new Size(176, 13);
            lblKpiEfectivoTitulo.TabIndex = 1;
            lblKpiEfectivoTitulo.Text = "ARQUEO EN EFECTIVO (CAJA)";
            // 
            // lblKpiEfectivoValor
            // 
            lblKpiEfectivoValor.AutoSize = true;
            lblKpiEfectivoValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblKpiEfectivoValor.ForeColor = Color.FromArgb(22, 101, 52);
            lblKpiEfectivoValor.Location = new Point(12, 32);
            lblKpiEfectivoValor.Name = "lblKpiEfectivoValor";
            lblKpiEfectivoValor.Size = new Size(116, 30);
            lblKpiEfectivoValor.TabIndex = 2;
            lblKpiEfectivoValor.Text = "$ 0,00";
            // 
            // lblKpiEfectivoSub
            // 
            lblKpiEfectivoSub.AutoSize = true;
            lblKpiEfectivoSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblKpiEfectivoSub.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpiEfectivoSub.Location = new Point(14, 68);
            lblKpiEfectivoSub.Name = "lblKpiEfectivoSub";
            lblKpiEfectivoSub.Size = new Size(76, 15);
            lblKpiEfectivoSub.TabIndex = 3;
            lblKpiEfectivoSub.Text = "0% del total";
            // 
            // pnlKpiDigital
            // 
            pnlKpiDigital.BackColor = Color.White;
            pnlKpiDigital.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiDigital.Controls.Add(pnlAccentDigital);
            pnlKpiDigital.Controls.Add(lblKpiDigitalTitulo);
            pnlKpiDigital.Controls.Add(lblKpiDigitalValor);
            pnlKpiDigital.Controls.Add(lblKpiDigitalSub);
            pnlKpiDigital.Location = new Point(860, 20);
            pnlKpiDigital.Name = "pnlKpiDigital";
            pnlKpiDigital.Size = new Size(240, 96);
            pnlKpiDigital.TabIndex = 3;
            // 
            // pnlAccentDigital
            // 
            pnlAccentDigital.BackColor = Color.FromArgb(124, 58, 237);
            pnlAccentDigital.Dock = DockStyle.Top;
            pnlAccentDigital.Location = new Point(0, 0);
            pnlAccentDigital.Name = "pnlAccentDigital";
            pnlAccentDigital.Size = new Size(238, 5);
            pnlAccentDigital.TabIndex = 0;
            // 
            // lblKpiDigitalTitulo
            // 
            lblKpiDigitalTitulo.AutoSize = true;
            lblKpiDigitalTitulo.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
            lblKpiDigitalTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpiDigitalTitulo.Location = new Point(14, 12);
            lblKpiDigitalTitulo.Name = "lblKpiDigitalTitulo";
            lblKpiDigitalTitulo.Size = new Size(168, 13);
            lblKpiDigitalTitulo.TabIndex = 1;
            lblKpiDigitalTitulo.Text = "INGRESOS DIGITALES / TARJETAS";
            // 
            // lblKpiDigitalValor
            // 
            lblKpiDigitalValor.AutoSize = true;
            lblKpiDigitalValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblKpiDigitalValor.ForeColor = Color.FromArgb(109, 40, 217);
            lblKpiDigitalValor.Location = new Point(12, 32);
            lblKpiDigitalValor.Name = "lblKpiDigitalValor";
            lblKpiDigitalValor.Size = new Size(116, 30);
            lblKpiDigitalValor.TabIndex = 2;
            lblKpiDigitalValor.Text = "$ 0,00";
            // 
            // lblKpiDigitalSub
            // 
            lblKpiDigitalSub.AutoSize = true;
            lblKpiDigitalSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblKpiDigitalSub.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpiDigitalSub.Location = new Point(14, 68);
            lblKpiDigitalSub.Name = "lblKpiDigitalSub";
            lblKpiDigitalSub.Size = new Size(76, 15);
            lblKpiDigitalSub.TabIndex = 3;
            lblKpiDigitalSub.Text = "0% del total";
            // 
            // pnlKpiPromedio
            // 
            pnlKpiPromedio.BackColor = Color.White;
            pnlKpiPromedio.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiPromedio.Controls.Add(pnlAccentPromedio);
            pnlKpiPromedio.Controls.Add(lblKpiPromedioTitulo);
            pnlKpiPromedio.Controls.Add(lblKpiPromedioValor);
            pnlKpiPromedio.Controls.Add(lblKpiPromedioSub);
            pnlKpiPromedio.Location = new Point(1114, 20);
            pnlKpiPromedio.Name = "pnlKpiPromedio";
            pnlKpiPromedio.Size = new Size(246, 96);
            pnlKpiPromedio.TabIndex = 4;
            // 
            // pnlAccentPromedio
            // 
            pnlAccentPromedio.BackColor = Color.FromArgb(245, 158, 11);
            pnlAccentPromedio.Dock = DockStyle.Top;
            pnlAccentPromedio.Location = new Point(0, 0);
            pnlAccentPromedio.Name = "pnlAccentPromedio";
            pnlAccentPromedio.Size = new Size(244, 5);
            pnlAccentPromedio.TabIndex = 0;
            // 
            // lblKpiPromedioTitulo
            // 
            lblKpiPromedioTitulo.AutoSize = true;
            lblKpiPromedioTitulo.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
            lblKpiPromedioTitulo.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpiPromedioTitulo.Location = new Point(14, 12);
            lblKpiPromedioTitulo.Name = "lblKpiPromedioTitulo";
            lblKpiPromedioTitulo.Size = new Size(158, 13);
            lblKpiPromedioTitulo.TabIndex = 1;
            lblKpiPromedioTitulo.Text = "TICKET PROMEDIO POR VENTA";
            // 
            // lblKpiPromedioValor
            // 
            lblKpiPromedioValor.AutoSize = true;
            lblKpiPromedioValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblKpiPromedioValor.ForeColor = Color.FromArgb(180, 83, 9);
            lblKpiPromedioValor.Location = new Point(12, 32);
            lblKpiPromedioValor.Name = "lblKpiPromedioValor";
            lblKpiPromedioValor.Size = new Size(116, 30);
            lblKpiPromedioValor.TabIndex = 2;
            lblKpiPromedioValor.Text = "$ 0,00";
            // 
            // lblKpiPromedioSub
            // 
            lblKpiPromedioSub.AutoSize = true;
            lblKpiPromedioSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblKpiPromedioSub.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpiPromedioSub.Location = new Point(14, 68);
            lblKpiPromedioSub.Name = "lblKpiPromedioSub";
            lblKpiPromedioSub.Size = new Size(88, 15);
            lblKpiPromedioSub.TabIndex = 3;
            lblKpiPromedioSub.Text = "0 operación(es)";
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
            dgvComprobantes.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White,
                WrapMode = DataGridViewTriState.False
            };
            dgvComprobantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvComprobantes.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55),
                Padding = new Padding(8, 4, 8, 4)
            };
            dgvComprobantes.EnableHeadersVisualStyles = false;
            dgvComprobantes.GridColor = Color.FromArgb(226, 232, 240);
            dgvComprobantes.Location = new Point(352, 128);
            dgvComprobantes.MultiSelect = false;
            dgvComprobantes.Name = "dgvComprobantes";
            dgvComprobantes.ReadOnly = true;
            dgvComprobantes.RowHeadersVisible = false;
            dgvComprobantes.RowTemplate.Height = 36;
            dgvComprobantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvComprobantes.Size = new Size(1008, 572);
            dgvComprobantes.TabIndex = 5;
            // 
            // frmConsultarComprobantes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1380, 720);
            Controls.Add(pnlFiltrosYAcciones);
            Controls.Add(pnlKpiTotal);
            Controls.Add(pnlKpiEfectivo);
            Controls.Add(pnlKpiDigital);
            Controls.Add(pnlKpiPromedio);
            Controls.Add(dgvComprobantes);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmConsultarComprobantes";
            Text = "frmConsultarComprobantes";
            FormClosing += frmConsultarComprobantes_FormClosing;
            Load += (s, e) => DistribuirTarjetasKpiResponsivo();
            Load += frmConsultarComprobantes_Load;
            Resize += (s, e) => DistribuirTarjetasKpiResponsivo();
            pnlFiltrosYAcciones.ResumeLayout(false);
            pnlFiltrosYAcciones.PerformLayout();
            pnlKpiTotal.ResumeLayout(false);
            pnlKpiTotal.PerformLayout();
            pnlKpiEfectivo.ResumeLayout(false);
            pnlKpiEfectivo.PerformLayout();
            pnlKpiDigital.ResumeLayout(false);
            pnlKpiDigital.PerformLayout();
            pnlKpiPromedio.ResumeLayout(false);
            pnlKpiPromedio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvComprobantes).EndInit();
            ResumeLayout(false);
        }

        private void DistribuirTarjetasKpiResponsivo()
        {
            if (dgvComprobantes == null || pnlKpiTotal == null || pnlKpiEfectivo == null || pnlKpiDigital == null || pnlKpiPromedio == null)
                return;

            int xInicio = dgvComprobantes.Left; // 352
            int xFin = this.ClientSize.Width - 20;
            int anchoTotalDisponible = Math.Max(400, xFin - xInicio);
            int separacion = 14;
            int anchoCadaTarjeta = (anchoTotalDisponible - (separacion * 3)) / 4;

            pnlKpiTotal.Location = new Point(xInicio, 20);
            pnlKpiTotal.Width = anchoCadaTarjeta;

            pnlKpiEfectivo.Location = new Point(pnlKpiTotal.Right + separacion, 20);
            pnlKpiEfectivo.Width = anchoCadaTarjeta;

            pnlKpiDigital.Location = new Point(pnlKpiEfectivo.Right + separacion, 20);
            pnlKpiDigital.Width = anchoCadaTarjeta;

            pnlKpiPromedio.Location = new Point(pnlKpiDigital.Right + separacion, 20);
            // La última tarjeta absorbe cualquier píxel de redondeo para terminar exactamente al final del formulario
            pnlKpiPromedio.Width = Math.Max(100, xFin - pnlKpiPromedio.Left);
        }

        #endregion

        private Panel pnlFiltrosYAcciones;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private TextBox txtDNI;
        private Label lblMetodoFiltro;
        private ComboBox cmbMetodoFiltro;
        private Button btnFiltrar;
        private Button btnLimpiar;
        private Button btnCajaHoy;
        private Panel pnlGraficoMedios;
        private Button btnReimprimirRecibo;
        private Button btnExportarArqueo;
        private Panel pnlKpiTotal;
        private Panel pnlAccentTotal;
        private Label lblKpiTotalTitulo;
        private Label lblKpiTotalValor;
        private Label lblKpiTotalSub;
        private Panel pnlKpiEfectivo;
        private Panel pnlAccentEfectivo;
        private Label lblKpiEfectivoTitulo;
        private Label lblKpiEfectivoValor;
        private Label lblKpiEfectivoSub;
        private Panel pnlKpiDigital;
        private Panel pnlAccentDigital;
        private Label lblKpiDigitalTitulo;
        private Label lblKpiDigitalValor;
        private Label lblKpiDigitalSub;
        private Panel pnlKpiPromedio;
        private Panel pnlAccentPromedio;
        private Label lblKpiPromedioTitulo;
        private Label lblKpiPromedioValor;
        private Label lblKpiPromedioSub;
        private DataGridView dgvComprobantes;
        private Label lblDNI;
        private Label lblDesde;
        private Label lblHasta;
    }
}