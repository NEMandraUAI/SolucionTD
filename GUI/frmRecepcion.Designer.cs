namespace GUI
{
    partial class frmRecepcion
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
            lblEstadoSocioBanner = new Label();
            pnlDatosSocio = new Panel();
            lblTituloDatosSocio = new Label();
            lblDNI = new Label();
            txtDNI = new TextBox();
            btnBuscar = new Button();
            btnLimpiarCampos = new Button();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            pnlSeleccionPlan = new Panel();
            lblTituloSeleccionPlan = new Label();
            lblPlan = new Label();
            cmbPlanes = new ComboBox();
            lblMetodoPago = new Label();
            cmbMetodoPago = new ComboBox();
            btnConsultarPlanes = new Button();
            pnlSimuladorComercial = new Panel();
            lblTituloSimulador = new Label();
            pnlCardPrecio = new Panel();
            lblSimImporteTitulo = new Label();
            lblMontoFinalValor = new Label();
            lblSimDuracionCosto = new Label();
            lblSimVigencia = new Label();
            lblArgumentoUpselling = new Label();
            chkEmitirReciboPDF = new CheckBox();
            btnConfirmarVenta = new Button();
            pnlDatosSocio.SuspendLayout();
            pnlSeleccionPlan.SuspendLayout();
            pnlSimuladorComercial.SuspendLayout();
            pnlCardPrecio.SuspendLayout();
            SuspendLayout();
            // 
            // lblEstadoSocioBanner
            // 
            lblEstadoSocioBanner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblEstadoSocioBanner.BackColor = Color.FromArgb(226, 232, 240);
            lblEstadoSocioBanner.BorderStyle = BorderStyle.FixedSingle;
            lblEstadoSocioBanner.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblEstadoSocioBanner.ForeColor = Color.FromArgb(31, 41, 55);
            lblEstadoSocioBanner.Location = new Point(28, 18);
            lblEstadoSocioBanner.Name = "lblEstadoSocioBanner";
            lblEstadoSocioBanner.Padding = new Padding(12, 0, 12, 0);
            lblEstadoSocioBanner.Size = new Size(1124, 42);
            lblEstadoSocioBanner.TabIndex = 20;
            lblEstadoSocioBanner.Text = "ℹ️ Ingrese el DNI del cliente y presione 'Buscar / Verificar' para consultar su historial o iniciar un Alta.";
            lblEstadoSocioBanner.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlDatosSocio
            // 
            pnlDatosSocio.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlDatosSocio.BackColor = Color.White;
            pnlDatosSocio.BorderStyle = BorderStyle.FixedSingle;
            pnlDatosSocio.Controls.Add(lblTituloDatosSocio);
            pnlDatosSocio.Controls.Add(lblDNI);
            pnlDatosSocio.Controls.Add(txtDNI);
            pnlDatosSocio.Controls.Add(btnBuscar);
            pnlDatosSocio.Controls.Add(btnLimpiarCampos);
            pnlDatosSocio.Controls.Add(lblNombre);
            pnlDatosSocio.Controls.Add(txtNombre);
            pnlDatosSocio.Controls.Add(lblApellido);
            pnlDatosSocio.Controls.Add(txtApellido);
            pnlDatosSocio.Controls.Add(lblTelefono);
            pnlDatosSocio.Controls.Add(txtTelefono);
            pnlDatosSocio.Controls.Add(lblEmail);
            pnlDatosSocio.Controls.Add(txtEmail);
            pnlDatosSocio.Location = new Point(28, 74);
            pnlDatosSocio.Name = "pnlDatosSocio";
            pnlDatosSocio.Size = new Size(576, 336);
            pnlDatosSocio.TabIndex = 0;
            // 
            // lblTituloDatosSocio
            // 
            lblTituloDatosSocio.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloDatosSocio.Dock = DockStyle.Top;
            lblTituloDatosSocio.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTituloDatosSocio.ForeColor = Color.White;
            lblTituloDatosSocio.Location = new Point(0, 0);
            lblTituloDatosSocio.Name = "lblTituloDatosSocio";
            lblTituloDatosSocio.Padding = new Padding(12, 0, 0, 0);
            lblTituloDatosSocio.Size = new Size(574, 34);
            lblTituloDatosSocio.TabIndex = 0;
            lblTituloDatosSocio.Text = "1. IDENTIFICACIÓN Y CONTACTO DEL SOCIO";
            lblTituloDatosSocio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(22, 48);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(92, 15);
            lblDNI.TabIndex = 1;
            lblDNI.Text = "DNI del Cliente:";
            // 
            // txtDNI
            // 
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 10F);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(22, 70);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(236, 25);
            txtDNI.TabIndex = 2;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(37, 99, 235);
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(272, 64);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(150, 36);
            btnBuscar.TabIndex = 3;
            btnBuscar.Text = "Buscar / Verificar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiarCampos
            // 
            btnLimpiarCampos.BackColor = Color.FromArgb(100, 116, 139);
            btnLimpiarCampos.FlatAppearance.BorderSize = 0;
            btnLimpiarCampos.FlatStyle = FlatStyle.Flat;
            btnLimpiarCampos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLimpiarCampos.ForeColor = Color.White;
            btnLimpiarCampos.Location = new Point(432, 64);
            btnLimpiarCampos.Name = "btnLimpiarCampos";
            btnLimpiarCampos.Size = new Size(120, 36);
            btnLimpiarCampos.TabIndex = 4;
            btnLimpiarCampos.Text = "Limpiar";
            btnLimpiarCampos.UseVisualStyleBackColor = false;
            btnLimpiarCampos.Click += btnLimpiarCampos_Click;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(31, 41, 55);
            lblNombre.Location = new Point(22, 124);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(53, 15);
            lblNombre.TabIndex = 5;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 9.5F);
            txtNombre.ForeColor = Color.FromArgb(31, 41, 55);
            txtNombre.Location = new Point(22, 146);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(254, 24);
            txtNombre.TabIndex = 6;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblApellido.ForeColor = Color.FromArgb(31, 41, 55);
            lblApellido.Location = new Point(298, 124);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(55, 15);
            lblApellido.TabIndex = 7;
            lblApellido.Text = "Apellido:";
            // 
            // txtApellido
            // 
            txtApellido.BackColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Font = new Font("Segoe UI", 9.5F);
            txtApellido.ForeColor = Color.FromArgb(31, 41, 55);
            txtApellido.Location = new Point(298, 146);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(254, 24);
            txtApellido.TabIndex = 8;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTelefono.ForeColor = Color.FromArgb(31, 41, 55);
            lblTelefono.Location = new Point(22, 204);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(59, 15);
            lblTelefono.TabIndex = 9;
            lblTelefono.Text = "Teléfono:";
            // 
            // txtTelefono
            // 
            txtTelefono.BackColor = Color.White;
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Font = new Font("Segoe UI", 9.5F);
            txtTelefono.ForeColor = Color.FromArgb(31, 41, 55);
            txtTelefono.Location = new Point(22, 226);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(254, 24);
            txtTelefono.TabIndex = 10;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(31, 41, 55);
            lblEmail.Location = new Point(298, 204);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(112, 15);
            lblEmail.TabIndex = 11;
            lblEmail.Text = "Correo Electrónico:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 9.5F);
            txtEmail.ForeColor = Color.FromArgb(31, 41, 55);
            txtEmail.Location = new Point(298, 226);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(254, 24);
            txtEmail.TabIndex = 12;
            // 
            // pnlSeleccionPlan
            // 
            pnlSeleccionPlan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlSeleccionPlan.BackColor = Color.White;
            pnlSeleccionPlan.BorderStyle = BorderStyle.FixedSingle;
            pnlSeleccionPlan.Controls.Add(lblTituloSeleccionPlan);
            pnlSeleccionPlan.Controls.Add(lblPlan);
            pnlSeleccionPlan.Controls.Add(cmbPlanes);
            pnlSeleccionPlan.Controls.Add(lblMetodoPago);
            pnlSeleccionPlan.Controls.Add(cmbMetodoPago);
            pnlSeleccionPlan.Controls.Add(btnConsultarPlanes);
            pnlSeleccionPlan.Location = new Point(28, 426);
            pnlSeleccionPlan.Name = "pnlSeleccionPlan";
            pnlSeleccionPlan.Size = new Size(576, 244);
            pnlSeleccionPlan.TabIndex = 1;
            // 
            // lblTituloSeleccionPlan
            // 
            lblTituloSeleccionPlan.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloSeleccionPlan.Dock = DockStyle.Top;
            lblTituloSeleccionPlan.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTituloSeleccionPlan.ForeColor = Color.White;
            lblTituloSeleccionPlan.Location = new Point(0, 0);
            lblTituloSeleccionPlan.Name = "lblTituloSeleccionPlan";
            lblTituloSeleccionPlan.Padding = new Padding(12, 0, 0, 0);
            lblTituloSeleccionPlan.Size = new Size(574, 34);
            lblTituloSeleccionPlan.TabIndex = 0;
            lblTituloSeleccionPlan.Text = "2. CONFIGURACIÓN DE SUSCRIPCIÓN Y PAGO";
            lblTituloSeleccionPlan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPlan
            // 
            lblPlan.AutoSize = true;
            lblPlan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPlan.ForeColor = Color.FromArgb(31, 41, 55);
            lblPlan.Location = new Point(22, 54);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(120, 15);
            lblPlan.TabIndex = 1;
            lblPlan.Text = "Plan de Suscripción:";
            // 
            // cmbPlanes
            // 
            cmbPlanes.BackColor = Color.White;
            cmbPlanes.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlanes.FlatStyle = FlatStyle.Flat;
            cmbPlanes.Font = new Font("Segoe UI", 9.5F);
            cmbPlanes.ForeColor = Color.FromArgb(31, 41, 55);
            cmbPlanes.FormattingEnabled = true;
            cmbPlanes.Location = new Point(22, 76);
            cmbPlanes.Name = "cmbPlanes";
            cmbPlanes.Size = new Size(254, 25);
            cmbPlanes.TabIndex = 2;
            cmbPlanes.SelectedIndexChanged += cmbPlanes_SelectedIndexChanged;
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMetodoPago.ForeColor = Color.FromArgb(31, 41, 55);
            lblMetodoPago.Location = new Point(298, 54);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(100, 15);
            lblMetodoPago.TabIndex = 3;
            lblMetodoPago.Text = "Método de Pago:";
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.BackColor = Color.White;
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPago.FlatStyle = FlatStyle.Flat;
            cmbMetodoPago.Font = new Font("Segoe UI", 9.5F);
            cmbMetodoPago.ForeColor = Color.FromArgb(31, 41, 55);
            cmbMetodoPago.FormattingEnabled = true;
            cmbMetodoPago.Location = new Point(298, 76);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(254, 25);
            cmbMetodoPago.TabIndex = 4;
            // 
            // btnConsultarPlanes
            // 
            btnConsultarPlanes.BackColor = Color.FromArgb(71, 85, 105);
            btnConsultarPlanes.FlatAppearance.BorderSize = 0;
            btnConsultarPlanes.FlatStyle = FlatStyle.Flat;
            btnConsultarPlanes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConsultarPlanes.ForeColor = Color.White;
            btnConsultarPlanes.Location = new Point(22, 134);
            btnConsultarPlanes.Name = "btnConsultarPlanes";
            btnConsultarPlanes.Size = new Size(530, 42);
            btnConsultarPlanes.TabIndex = 5;
            btnConsultarPlanes.Text = "Ver Catálogo Detallado de Planes";
            btnConsultarPlanes.UseVisualStyleBackColor = false;
            btnConsultarPlanes.Click += btnConsultarPlanes_Click;
            // 
            // pnlSimuladorComercial
            // 
            pnlSimuladorComercial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlSimuladorComercial.BackColor = Color.White;
            pnlSimuladorComercial.BorderStyle = BorderStyle.FixedSingle;
            pnlSimuladorComercial.Controls.Add(lblTituloSimulador);
            pnlSimuladorComercial.Controls.Add(pnlCardPrecio);
            pnlSimuladorComercial.Controls.Add(lblSimVigencia);
            pnlSimuladorComercial.Controls.Add(lblArgumentoUpselling);
            pnlSimuladorComercial.Controls.Add(chkEmitirReciboPDF);
            pnlSimuladorComercial.Controls.Add(btnConfirmarVenta);
            pnlSimuladorComercial.Location = new Point(624, 74);
            pnlSimuladorComercial.Name = "pnlSimuladorComercial";
            pnlSimuladorComercial.Size = new Size(528, 596);
            pnlSimuladorComercial.TabIndex = 2;
            // 
            // lblTituloSimulador
            // 
            lblTituloSimulador.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloSimulador.Dock = DockStyle.Top;
            lblTituloSimulador.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTituloSimulador.ForeColor = Color.White;
            lblTituloSimulador.Location = new Point(0, 0);
            lblTituloSimulador.Name = "lblTituloSimulador";
            lblTituloSimulador.Padding = new Padding(12, 0, 0, 0);
            lblTituloSimulador.Size = new Size(526, 34);
            lblTituloSimulador.TabIndex = 0;
            lblTituloSimulador.Text = "3. SIMULADOR COMERCIAL Y RESUMEN DE COBERTURA";
            lblTituloSimulador.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlCardPrecio
            // 
            pnlCardPrecio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCardPrecio.BackColor = Color.FromArgb(239, 246, 255);
            pnlCardPrecio.BorderStyle = BorderStyle.FixedSingle;
            pnlCardPrecio.Controls.Add(lblSimImporteTitulo);
            pnlCardPrecio.Controls.Add(lblMontoFinalValor);
            pnlCardPrecio.Controls.Add(lblSimDuracionCosto);
            pnlCardPrecio.Location = new Point(22, 52);
            pnlCardPrecio.Name = "pnlCardPrecio";
            pnlCardPrecio.Size = new Size(482, 118);
            pnlCardPrecio.TabIndex = 1;
            // 
            // lblSimImporteTitulo
            // 
            lblSimImporteTitulo.AutoSize = true;
            lblSimImporteTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSimImporteTitulo.ForeColor = Color.FromArgb(30, 58, 138);
            lblSimImporteTitulo.Location = new Point(16, 12);
            lblSimImporteTitulo.Name = "lblSimImporteTitulo";
            lblSimImporteTitulo.Size = new Size(165, 15);
            lblSimImporteTitulo.TabIndex = 0;
            lblSimImporteTitulo.Text = "IMPORTE FINAL A COBRAR:";
            // 
            // lblMontoFinalValor
            // 
            lblMontoFinalValor.AutoSize = true;
            lblMontoFinalValor.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblMontoFinalValor.ForeColor = Color.FromArgb(37, 99, 235);
            lblMontoFinalValor.Location = new Point(14, 32);
            lblMontoFinalValor.Name = "lblMontoFinalValor";
            lblMontoFinalValor.Size = new Size(185, 41);
            lblMontoFinalValor.TabIndex = 1;
            lblMontoFinalValor.Text = "$ 15.000,00";
            // 
            // lblSimDuracionCosto
            // 
            lblSimDuracionCosto.AutoSize = true;
            lblSimDuracionCosto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSimDuracionCosto.ForeColor = Color.FromArgb(71, 85, 105);
            lblSimDuracionCosto.Location = new Point(16, 84);
            lblSimDuracionCosto.Name = "lblSimDuracionCosto";
            lblSimDuracionCosto.Size = new Size(340, 15);
            lblSimDuracionCosto.TabIndex = 2;
            lblSimDuracionCosto.Text = "Duración: 30 días corridos   |   Costo diario: $ 500,00 / día";
            // 
            // lblSimVigencia
            // 
            lblSimVigencia.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSimVigencia.BackColor = Color.FromArgb(240, 253, 244);
            lblSimVigencia.BorderStyle = BorderStyle.FixedSingle;
            lblSimVigencia.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSimVigencia.ForeColor = Color.FromArgb(22, 101, 52);
            lblSimVigencia.Location = new Point(22, 184);
            lblSimVigencia.Name = "lblSimVigencia";
            lblSimVigencia.Padding = new Padding(12, 0, 12, 0);
            lblSimVigencia.Size = new Size(482, 46);
            lblSimVigencia.TabIndex = 2;
            lblSimVigencia.Text = "📅 Cobertura garantizada: Desde HOY hasta el -";
            lblSimVigencia.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblArgumentoUpselling
            // 
            lblArgumentoUpselling.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblArgumentoUpselling.BackColor = Color.FromArgb(254, 243, 199);
            lblArgumentoUpselling.BorderStyle = BorderStyle.FixedSingle;
            lblArgumentoUpselling.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblArgumentoUpselling.ForeColor = Color.FromArgb(146, 64, 14);
            lblArgumentoUpselling.Location = new Point(22, 246);
            lblArgumentoUpselling.Name = "lblArgumentoUpselling";
            lblArgumentoUpselling.Padding = new Padding(14, 10, 14, 10);
            lblArgumentoUpselling.Size = new Size(482, 184);
            lblArgumentoUpselling.TabIndex = 3;
            lblArgumentoUpselling.Text = "💡 ARGUMENTO DE VENTA (UPSELLING):";
            lblArgumentoUpselling.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkEmitirReciboPDF
            // 
            chkEmitirReciboPDF.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkEmitirReciboPDF.AutoSize = true;
            chkEmitirReciboPDF.Checked = true;
            chkEmitirReciboPDF.CheckState = CheckState.Checked;
            chkEmitirReciboPDF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkEmitirReciboPDF.ForeColor = Color.FromArgb(31, 41, 55);
            chkEmitirReciboPDF.Location = new Point(22, 456);
            chkEmitirReciboPDF.Name = "chkEmitirReciboPDF";
            chkEmitirReciboPDF.Size = new Size(406, 19);
            chkEmitirReciboPDF.TabIndex = 4;
            chkEmitirReciboPDF.Text = "📄 Generar y abrir Recibo Oficial en PDF al confirmar la venta";
            chkEmitirReciboPDF.UseVisualStyleBackColor = true;
            // 
            // btnConfirmarVenta
            // 
            btnConfirmarVenta.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnConfirmarVenta.BackColor = Color.FromArgb(16, 185, 129);
            btnConfirmarVenta.FlatAppearance.BorderSize = 0;
            btnConfirmarVenta.FlatStyle = FlatStyle.Flat;
            btnConfirmarVenta.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnConfirmarVenta.ForeColor = Color.White;
            btnConfirmarVenta.Location = new Point(22, 496);
            btnConfirmarVenta.Name = "btnConfirmarVenta";
            btnConfirmarVenta.Size = new Size(482, 68);
            btnConfirmarVenta.TabIndex = 5;
            btnConfirmarVenta.Text = "Procesar Transacción y Dar Acceso";
            btnConfirmarVenta.UseVisualStyleBackColor = false;
            btnConfirmarVenta.Click += btnConfirmarVenta_Click;
            // 
            // frmRecepcion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1180, 696);
            Controls.Add(lblEstadoSocioBanner);
            Controls.Add(pnlDatosSocio);
            Controls.Add(pnlSeleccionPlan);
            Controls.Add(pnlSimuladorComercial);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmRecepcion";
            Text = "frmRecepcion";
            FormClosing += frmRecepcion_FormClosing;
            Load += frmRecepcion_Load;
            pnlDatosSocio.ResumeLayout(false);
            pnlDatosSocio.PerformLayout();
            pnlSeleccionPlan.ResumeLayout(false);
            pnlSeleccionPlan.PerformLayout();
            pnlSimuladorComercial.ResumeLayout(false);
            pnlSimuladorComercial.PerformLayout();
            pnlCardPrecio.ResumeLayout(false);
            pnlCardPrecio.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblEstadoSocioBanner;
        private Panel pnlDatosSocio;
        private Label lblTituloDatosSocio;
        private TextBox txtDNI;
        private Button btnBuscar;
        private Button btnLimpiarCampos;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private Panel pnlSeleccionPlan;
        private Label lblTituloSeleccionPlan;
        private ComboBox cmbPlanes;
        private ComboBox cmbMetodoPago;
        private Button btnConsultarPlanes;
        private Panel pnlSimuladorComercial;
        private Label lblTituloSimulador;
        private Panel pnlCardPrecio;
        private Label lblSimImporteTitulo;
        private Label lblMontoFinalValor;
        private Label lblSimDuracionCosto;
        private Label lblSimVigencia;
        private Label lblArgumentoUpselling;
        private CheckBox chkEmitirReciboPDF;
        private Button btnConfirmarVenta;
        private Label lblDNI;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblTelefono;
        private Label lblEmail;
        private Label lblPlan;
        private Label lblMetodoPago;
    }
}