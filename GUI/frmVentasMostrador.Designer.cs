using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmVentasMostrador
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblBannerSinergiaSocio = new Label();
            pnlCatalogoYSeleccion = new Panel();
            lblTituloSeleccion = new Label();
            lblBuscarSocioDNI = new Label();
            txtDNISocio = new TextBox();
            btnVerificarSocio = new Button();
            chkFiltrarAfines = new CheckBox();
            lblCategoriaFiltro = new Label();
            cmbCategoriaFiltro = new ComboBox();
            lblProductoSelect = new Label();
            cmbProductoSelect = new ComboBox();
            lblInfoProductoSeleccionado = new Label();
            lblCantidadVenta = new Label();
            nudCantidadVenta = new NumericUpDown();
            btnAgregarCarrito = new Button();
            dgvStockCatalogo = new DataGridView();
            pnlCarritoCompra = new Panel();
            lblTituloCarrito = new Label();
            dgvCarrito = new DataGridView();
            btnQuitarCarrito = new Button();
            lblMetodoPagoVenta = new Label();
            cmbMetodoPagoVenta = new ComboBox();
            lblTotalCarritoValor = new Label();
            chkEmitirComprobante = new CheckBox();
            btnConfirmarVentaProd = new Button();
            pnlHistorialVentas = new Panel();
            lblTituloHistorial = new Label();
            dtpHistDesde = new DateTimePicker();
            dtpHistHasta = new DateTimePicker();
            btnFiltrarHistorial = new Button();
            btnReimprimirVentaPDF = new Button();
            dgvHistorialVentas = new DataGridView();
            pnlCatalogoYSeleccion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidadVenta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvStockCatalogo).BeginInit();
            pnlCarritoCompra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).BeginInit();
            pnlHistorialVentas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorialVentas).BeginInit();
            SuspendLayout();
            // 
            // lblBannerSinergiaSocio
            // 
            lblBannerSinergiaSocio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblBannerSinergiaSocio.BackColor = Color.FromArgb(226, 232, 240);
            lblBannerSinergiaSocio.BorderStyle = BorderStyle.FixedSingle;
            lblBannerSinergiaSocio.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblBannerSinergiaSocio.ForeColor = Color.FromArgb(31, 41, 55);
            lblBannerSinergiaSocio.Location = new Point(20, 14);
            lblBannerSinergiaSocio.Name = "lblBannerSinergiaSocio";
            lblBannerSinergiaSocio.Padding = new Padding(12, 0, 12, 0);
            lblBannerSinergiaSocio.Size = new Size(1240, 40);
            lblBannerSinergiaSocio.TabIndex = 0;
            lblBannerSinergiaSocio.Text = "ℹ️ Ingrese el DNI del socio comprador para consultar su objetivo deportivo/nutricional y recomendar artículos afines.";
            lblBannerSinergiaSocio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlCatalogoYSeleccion
            // 
            pnlCatalogoYSeleccion.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlCatalogoYSeleccion.BackColor = Color.White;
            pnlCatalogoYSeleccion.BorderStyle = BorderStyle.FixedSingle;
            pnlCatalogoYSeleccion.Controls.Add(lblTituloSeleccion);
            pnlCatalogoYSeleccion.Controls.Add(lblBuscarSocioDNI);
            pnlCatalogoYSeleccion.Controls.Add(txtDNISocio);
            pnlCatalogoYSeleccion.Controls.Add(btnVerificarSocio);
            pnlCatalogoYSeleccion.Controls.Add(chkFiltrarAfines);
            pnlCatalogoYSeleccion.Controls.Add(lblCategoriaFiltro);
            pnlCatalogoYSeleccion.Controls.Add(cmbCategoriaFiltro);
            pnlCatalogoYSeleccion.Controls.Add(lblProductoSelect);
            pnlCatalogoYSeleccion.Controls.Add(cmbProductoSelect);
            pnlCatalogoYSeleccion.Controls.Add(lblInfoProductoSeleccionado);
            pnlCatalogoYSeleccion.Controls.Add(lblCantidadVenta);
            pnlCatalogoYSeleccion.Controls.Add(nudCantidadVenta);
            pnlCatalogoYSeleccion.Controls.Add(btnAgregarCarrito);
            pnlCatalogoYSeleccion.Controls.Add(dgvStockCatalogo);
            pnlCatalogoYSeleccion.Location = new Point(20, 64);
            pnlCatalogoYSeleccion.Name = "pnlCatalogoYSeleccion";
            pnlCatalogoYSeleccion.Size = new Size(560, 636);
            pnlCatalogoYSeleccion.TabIndex = 1;
            // 
            // lblTituloSeleccion
            // 
            lblTituloSeleccion.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloSeleccion.Dock = DockStyle.Top;
            lblTituloSeleccion.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTituloSeleccion.ForeColor = Color.White;
            lblTituloSeleccion.Location = new Point(0, 0);
            lblTituloSeleccion.Name = "lblTituloSeleccion";
            lblTituloSeleccion.Padding = new Padding(12, 0, 0, 0);
            lblTituloSeleccion.Size = new Size(558, 32);
            lblTituloSeleccion.TabIndex = 0;
            lblTituloSeleccion.Text = "1. IDENTIFICACIÓN DEL SOCIO Y CATÁLOGO DE PRODUCTOS";
            lblTituloSeleccion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBuscarSocioDNI
            // 
            lblBuscarSocioDNI.AutoSize = true;
            lblBuscarSocioDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBuscarSocioDNI.Location = new Point(16, 44);
            lblBuscarSocioDNI.Name = "lblBuscarSocioDNI";
            lblBuscarSocioDNI.Size = new Size(154, 15);
            lblBuscarSocioDNI.TabIndex = 1;
            lblBuscarSocioDNI.Text = "DNI del Socio Comprador:";
            // 
            // txtDNISocio
            // 
            txtDNISocio.BorderStyle = BorderStyle.FixedSingle;
            txtDNISocio.Font = new Font("Segoe UI", 9.5F);
            txtDNISocio.Location = new Point(16, 64);
            txtDNISocio.Name = "txtDNISocio";
            txtDNISocio.Size = new Size(340, 24);
            txtDNISocio.TabIndex = 2;
            // 
            // btnVerificarSocio
            // 
            btnVerificarSocio.BackColor = Color.FromArgb(37, 99, 235);
            btnVerificarSocio.FlatAppearance.BorderSize = 0;
            btnVerificarSocio.FlatStyle = FlatStyle.Flat;
            btnVerificarSocio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnVerificarSocio.ForeColor = Color.White;
            btnVerificarSocio.Location = new Point(368, 59);
            btnVerificarSocio.Name = "btnVerificarSocio";
            btnVerificarSocio.Size = new Size(172, 32);
            btnVerificarSocio.TabIndex = 3;
            btnVerificarSocio.Text = "Verificar Perfil";
            btnVerificarSocio.UseVisualStyleBackColor = false;
            btnVerificarSocio.Click += btnVerificarSocio_Click;
            // 
            // chkFiltrarAfines
            // 
            chkFiltrarAfines.AutoSize = true;
            chkFiltrarAfines.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
            chkFiltrarAfines.ForeColor = Color.FromArgb(30, 58, 138);
            chkFiltrarAfines.Location = new Point(16, 98);
            chkFiltrarAfines.Name = "chkFiltrarAfines";
            chkFiltrarAfines.Size = new Size(442, 19);
            chkFiltrarAfines.TabIndex = 4;
            chkFiltrarAfines.Text = "🎯 Mostrar solo productos afines al objetivo físico y nutricional del socio";
            chkFiltrarAfines.UseVisualStyleBackColor = true;
            chkFiltrarAfines.CheckedChanged += chkFiltrarAfines_CheckedChanged;
            // 
            // lblCategoriaFiltro
            // 
            lblCategoriaFiltro.AutoSize = true;
            lblCategoriaFiltro.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCategoriaFiltro.Location = new Point(16, 126);
            lblCategoriaFiltro.Name = "lblCategoriaFiltro";
            lblCategoriaFiltro.Size = new Size(64, 15);
            lblCategoriaFiltro.TabIndex = 5;
            lblCategoriaFiltro.Text = "Categoría:";
            // 
            // cmbCategoriaFiltro
            // 
            cmbCategoriaFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoriaFiltro.FlatStyle = FlatStyle.Flat;
            cmbCategoriaFiltro.Font = new Font("Segoe UI", 9F);
            cmbCategoriaFiltro.Location = new Point(16, 146);
            cmbCategoriaFiltro.Name = "cmbCategoriaFiltro";
            cmbCategoriaFiltro.Size = new Size(524, 23);
            cmbCategoriaFiltro.TabIndex = 6;
            cmbCategoriaFiltro.SelectedIndexChanged += cmbCategoriaFiltro_SelectedIndexChanged;
            // 
            // lblProductoSelect
            // 
            lblProductoSelect.AutoSize = true;
            lblProductoSelect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductoSelect.Location = new Point(16, 178);
            lblProductoSelect.Name = "lblProductoSelect";
            lblProductoSelect.Size = new Size(118, 15);
            lblProductoSelect.TabIndex = 7;
            lblProductoSelect.Text = "Artículo / Producto:";
            // 
            // cmbProductoSelect
            // 
            cmbProductoSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductoSelect.FlatStyle = FlatStyle.Flat;
            cmbProductoSelect.Font = new Font("Segoe UI", 9.5F);
            cmbProductoSelect.Location = new Point(16, 198);
            cmbProductoSelect.Name = "cmbProductoSelect";
            cmbProductoSelect.Size = new Size(524, 25);
            cmbProductoSelect.TabIndex = 8;
            cmbProductoSelect.SelectedIndexChanged += cmbProductoSelect_SelectedIndexChanged;
            // 
            // lblInfoProductoSeleccionado
            // 
            lblInfoProductoSeleccionado.BackColor = Color.FromArgb(239, 246, 255);
            lblInfoProductoSeleccionado.BorderStyle = BorderStyle.FixedSingle;
            lblInfoProductoSeleccionado.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblInfoProductoSeleccionado.ForeColor = Color.FromArgb(30, 58, 138);
            lblInfoProductoSeleccionado.Location = new Point(16, 228);
            lblInfoProductoSeleccionado.Name = "lblInfoProductoSeleccionado";
            lblInfoProductoSeleccionado.Padding = new Padding(8, 4, 8, 4);
            lblInfoProductoSeleccionado.Size = new Size(524, 66);
            lblInfoProductoSeleccionado.TabIndex = 9;
            lblInfoProductoSeleccionado.Text = "Seleccione un artículo del catálogo...";
            lblInfoProductoSeleccionado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCantidadVenta
            // 
            lblCantidadVenta.AutoSize = true;
            lblCantidadVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCantidadVenta.Location = new Point(16, 308);
            lblCantidadVenta.Name = "lblCantidadVenta";
            lblCantidadVenta.Size = new Size(60, 15);
            lblCantidadVenta.TabIndex = 10;
            lblCantidadVenta.Text = "Cantidad:";
            // 
            // nudCantidadVenta
            // 
            nudCantidadVenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            nudCantidadVenta.Location = new Point(100, 305);
            nudCantidadVenta.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            nudCantidadVenta.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidadVenta.Name = "nudCantidadVenta";
            nudCantidadVenta.Size = new Size(92, 25);
            nudCantidadVenta.TabIndex = 11;
            nudCantidadVenta.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregarCarrito
            // 
            btnAgregarCarrito.BackColor = Color.FromArgb(37, 99, 235);
            btnAgregarCarrito.FlatAppearance.BorderSize = 0;
            btnAgregarCarrito.FlatStyle = FlatStyle.Flat;
            btnAgregarCarrito.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAgregarCarrito.ForeColor = Color.White;
            btnAgregarCarrito.Location = new Point(204, 300);
            btnAgregarCarrito.Name = "btnAgregarCarrito";
            btnAgregarCarrito.Size = new Size(336, 34);
            btnAgregarCarrito.TabIndex = 12;
            btnAgregarCarrito.Text = "➕ Agregar al Carrito";
            btnAgregarCarrito.UseVisualStyleBackColor = false;
            btnAgregarCarrito.Click += btnAgregarCarrito_Click;
            // 
            // dgvStockCatalogo
            // 
            dgvStockCatalogo.AllowUserToAddRows = false;
            dgvStockCatalogo.AllowUserToDeleteRows = false;
            dgvStockCatalogo.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvStockCatalogo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvStockCatalogo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStockCatalogo.BackgroundColor = Color.White;
            dgvStockCatalogo.BorderStyle = BorderStyle.FixedSingle;
            dgvStockCatalogo.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStockCatalogo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvStockCatalogo.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White
            };
            dgvStockCatalogo.ColumnHeadersHeight = 30;
            dgvStockCatalogo.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55)
            };
            dgvStockCatalogo.EnableHeadersVisualStyles = false;
            dgvStockCatalogo.GridColor = Color.FromArgb(226, 232, 240);
            dgvStockCatalogo.Location = new Point(16, 346);
            dgvStockCatalogo.MultiSelect = false;
            dgvStockCatalogo.Name = "dgvStockCatalogo";
            dgvStockCatalogo.ReadOnly = true;
            dgvStockCatalogo.RowHeadersVisible = false;
            dgvStockCatalogo.RowTemplate.Height = 28;
            dgvStockCatalogo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockCatalogo.Size = new Size(524, 274);
            dgvStockCatalogo.TabIndex = 13;
            dgvStockCatalogo.SelectionChanged += dgvStockCatalogo_SelectionChanged;
            // 
            // pnlCarritoCompra
            // 
            pnlCarritoCompra.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCarritoCompra.BackColor = Color.White;
            pnlCarritoCompra.BorderStyle = BorderStyle.FixedSingle;
            pnlCarritoCompra.Controls.Add(lblTituloCarrito);
            pnlCarritoCompra.Controls.Add(dgvCarrito);
            pnlCarritoCompra.Controls.Add(btnQuitarCarrito);
            pnlCarritoCompra.Controls.Add(lblMetodoPagoVenta);
            pnlCarritoCompra.Controls.Add(cmbMetodoPagoVenta);
            pnlCarritoCompra.Controls.Add(lblTotalCarritoValor);
            pnlCarritoCompra.Controls.Add(chkEmitirComprobante);
            pnlCarritoCompra.Controls.Add(btnConfirmarVentaProd);
            pnlCarritoCompra.Location = new Point(596, 64);
            pnlCarritoCompra.Name = "pnlCarritoCompra";
            pnlCarritoCompra.Size = new Size(664, 344);
            pnlCarritoCompra.TabIndex = 2;
            // 
            // lblTituloCarrito
            // 
            lblTituloCarrito.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloCarrito.Dock = DockStyle.Top;
            lblTituloCarrito.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTituloCarrito.ForeColor = Color.White;
            lblTituloCarrito.Location = new Point(0, 0);
            lblTituloCarrito.Name = "lblTituloCarrito";
            lblTituloCarrito.Padding = new Padding(12, 0, 0, 0);
            lblTituloCarrito.Size = new Size(662, 32);
            lblTituloCarrito.TabIndex = 0;
            lblTituloCarrito.Text = "CARRITO DE COMPRA ACTUAL";
            lblTituloCarrito.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvCarrito
            // 
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCarrito.BackgroundColor = Color.White;
            dgvCarrito.BorderStyle = BorderStyle.FixedSingle;
            dgvCarrito.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCarrito.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCarrito.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White
            };
            dgvCarrito.ColumnHeadersHeight = 30;
            dgvCarrito.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55)
            };
            dgvCarrito.EnableHeadersVisualStyles = false;
            dgvCarrito.GridColor = Color.FromArgb(226, 232, 240);
            dgvCarrito.Location = new Point(16, 42);
            dgvCarrito.MultiSelect = false;
            dgvCarrito.Name = "dgvCarrito";
            dgvCarrito.ReadOnly = true;
            dgvCarrito.RowHeadersVisible = false;
            dgvCarrito.RowTemplate.Height = 28;
            dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrito.Size = new Size(630, 180);
            dgvCarrito.TabIndex = 1;
            // 
            // btnQuitarCarrito
            // 
            btnQuitarCarrito.BackColor = Color.FromArgb(185, 28, 28);
            btnQuitarCarrito.FlatAppearance.BorderSize = 0;
            btnQuitarCarrito.FlatStyle = FlatStyle.Flat;
            btnQuitarCarrito.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnQuitarCarrito.ForeColor = Color.White;
            btnQuitarCarrito.Location = new Point(16, 232);
            btnQuitarCarrito.Name = "btnQuitarCarrito";
            btnQuitarCarrito.Size = new Size(136, 32);
            btnQuitarCarrito.TabIndex = 2;
            btnQuitarCarrito.Text = "🗑️ Quitar Ítem";
            btnQuitarCarrito.UseVisualStyleBackColor = false;
            btnQuitarCarrito.Click += btnQuitarCarrito_Click;
            // 
            // lblMetodoPagoVenta
            // 
            lblMetodoPagoVenta.AutoSize = true;
            lblMetodoPagoVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMetodoPagoVenta.Location = new Point(160, 239);
            lblMetodoPagoVenta.Name = "lblMetodoPagoVenta";
            lblMetodoPagoVenta.Size = new Size(94, 15);
            lblMetodoPagoVenta.TabIndex = 3;
            lblMetodoPagoVenta.Text = "Medio de Pago:";
            // 
            // cmbMetodoPagoVenta
            // 
            cmbMetodoPagoVenta.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPagoVenta.FlatStyle = FlatStyle.Flat;
            cmbMetodoPagoVenta.Font = new Font("Segoe UI", 9F);
            cmbMetodoPagoVenta.Location = new Point(304, 235);
            cmbMetodoPagoVenta.Name = "cmbMetodoPagoVenta";
            cmbMetodoPagoVenta.Size = new Size(146, 23);
            cmbMetodoPagoVenta.TabIndex = 4;
            // 
            // lblTotalCarritoValor
            // 
            lblTotalCarritoValor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotalCarritoValor.BackColor = Color.FromArgb(239, 246, 255);
            lblTotalCarritoValor.BorderStyle = BorderStyle.FixedSingle;
            lblTotalCarritoValor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalCarritoValor.ForeColor = Color.FromArgb(37, 99, 235);
            lblTotalCarritoValor.Location = new Point(458, 230);
            lblTotalCarritoValor.Name = "lblTotalCarritoValor";
            lblTotalCarritoValor.Size = new Size(188, 36);
            lblTotalCarritoValor.TabIndex = 5;
            lblTotalCarritoValor.Text = "TOTAL: $ 0,00";
            lblTotalCarritoValor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chkEmitirComprobante
            // 
            chkEmitirComprobante.AutoSize = true;
            chkEmitirComprobante.Checked = true;
            chkEmitirComprobante.CheckState = CheckState.Checked;
            chkEmitirComprobante.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
            chkEmitirComprobante.Location = new Point(16, 288);
            chkEmitirComprobante.Name = "chkEmitirComprobante";
            chkEmitirComprobante.Size = new Size(258, 19);
            chkEmitirComprobante.TabIndex = 6;
            chkEmitirComprobante.Text = "📄 Emitir Comprobante PDF (QuestPDF)";
            chkEmitirComprobante.UseVisualStyleBackColor = true;
            // 
            // btnConfirmarVentaProd
            // 
            btnConfirmarVentaProd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnConfirmarVentaProd.BackColor = Color.FromArgb(16, 185, 129);
            btnConfirmarVentaProd.FlatAppearance.BorderSize = 0;
            btnConfirmarVentaProd.FlatStyle = FlatStyle.Flat;
            btnConfirmarVentaProd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConfirmarVentaProd.ForeColor = Color.White;
            btnConfirmarVentaProd.Location = new Point(296, 276);
            btnConfirmarVentaProd.Name = "btnConfirmarVentaProd";
            btnConfirmarVentaProd.Size = new Size(350, 48);
            btnConfirmarVentaProd.TabIndex = 7;
            btnConfirmarVentaProd.Text = "💳 Confirmar Venta y Descontar Stock";
            btnConfirmarVentaProd.UseVisualStyleBackColor = false;
            btnConfirmarVentaProd.Click += btnConfirmarVentaProd_Click;
            // 
            // pnlHistorialVentas
            // 
            pnlHistorialVentas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlHistorialVentas.BackColor = Color.White;
            pnlHistorialVentas.BorderStyle = BorderStyle.FixedSingle;
            pnlHistorialVentas.Controls.Add(lblTituloHistorial);
            pnlHistorialVentas.Controls.Add(dtpHistDesde);
            pnlHistorialVentas.Controls.Add(dtpHistHasta);
            pnlHistorialVentas.Controls.Add(btnFiltrarHistorial);
            pnlHistorialVentas.Controls.Add(btnReimprimirVentaPDF);
            pnlHistorialVentas.Controls.Add(dgvHistorialVentas);
            pnlHistorialVentas.Location = new Point(596, 420);
            pnlHistorialVentas.Name = "pnlHistorialVentas";
            pnlHistorialVentas.Size = new Size(664, 280);
            pnlHistorialVentas.TabIndex = 3;
            // 
            // lblTituloHistorial
            // 
            lblTituloHistorial.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloHistorial.Dock = DockStyle.Top;
            lblTituloHistorial.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTituloHistorial.ForeColor = Color.White;
            lblTituloHistorial.Location = new Point(0, 0);
            lblTituloHistorial.Name = "lblTituloHistorial";
            lblTituloHistorial.Padding = new Padding(12, 0, 0, 0);
            lblTituloHistorial.Size = new Size(662, 32);
            lblTituloHistorial.TabIndex = 0;
            lblTituloHistorial.Text = "HISTORIAL RECIENTE DE VENTAS DE MOSTRADOR";
            lblTituloHistorial.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpHistDesde
            // 
            dtpHistDesde.Format = DateTimePickerFormat.Short;
            dtpHistDesde.Location = new Point(16, 42);
            dtpHistDesde.Name = "dtpHistDesde";
            dtpHistDesde.Size = new Size(120, 23);
            dtpHistDesde.TabIndex = 1;
            // 
            // dtpHistHasta
            // 
            dtpHistHasta.Format = DateTimePickerFormat.Short;
            dtpHistHasta.Location = new Point(146, 42);
            dtpHistHasta.Name = "dtpHistHasta";
            dtpHistHasta.Size = new Size(120, 23);
            dtpHistHasta.TabIndex = 2;
            // 
            // btnFiltrarHistorial
            // 
            btnFiltrarHistorial.BackColor = Color.FromArgb(71, 85, 105);
            btnFiltrarHistorial.FlatAppearance.BorderSize = 0;
            btnFiltrarHistorial.FlatStyle = FlatStyle.Flat;
            btnFiltrarHistorial.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnFiltrarHistorial.ForeColor = Color.White;
            btnFiltrarHistorial.Location = new Point(278, 39);
            btnFiltrarHistorial.Name = "btnFiltrarHistorial";
            btnFiltrarHistorial.Size = new Size(130, 29);
            btnFiltrarHistorial.TabIndex = 3;
            btnFiltrarHistorial.Text = "Filtrar Historial";
            btnFiltrarHistorial.UseVisualStyleBackColor = false;
            btnFiltrarHistorial.Click += btnFiltrarHistorial_Click;
            // 
            // btnReimprimirVentaPDF
            // 
            btnReimprimirVentaPDF.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReimprimirVentaPDF.BackColor = Color.FromArgb(37, 99, 235);
            btnReimprimirVentaPDF.FlatAppearance.BorderSize = 0;
            btnReimprimirVentaPDF.FlatStyle = FlatStyle.Flat;
            btnReimprimirVentaPDF.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnReimprimirVentaPDF.ForeColor = Color.White;
            btnReimprimirVentaPDF.Location = new Point(426, 39);
            btnReimprimirVentaPDF.Name = "btnReimprimirVentaPDF";
            btnReimprimirVentaPDF.Size = new Size(220, 29);
            btnReimprimirVentaPDF.TabIndex = 4;
            btnReimprimirVentaPDF.Text = "📄 Reimprimir Comprobante PDF";
            btnReimprimirVentaPDF.UseVisualStyleBackColor = false;
            btnReimprimirVentaPDF.Click += btnReimprimirVentaPDF_Click;
            // 
            // dgvHistorialVentas
            // 
            dgvHistorialVentas.AllowUserToAddRows = false;
            dgvHistorialVentas.AllowUserToDeleteRows = false;
            dgvHistorialVentas.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvHistorialVentas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistorialVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorialVentas.BackgroundColor = Color.White;
            dgvHistorialVentas.BorderStyle = BorderStyle.FixedSingle;
            dgvHistorialVentas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistorialVentas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistorialVentas.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White
            };
            dgvHistorialVentas.ColumnHeadersHeight = 28;
            dgvHistorialVentas.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55)
            };
            dgvHistorialVentas.EnableHeadersVisualStyles = false;
            dgvHistorialVentas.GridColor = Color.FromArgb(226, 232, 240);
            dgvHistorialVentas.Location = new Point(16, 76);
            dgvHistorialVentas.MultiSelect = false;
            dgvHistorialVentas.Name = "dgvHistorialVentas";
            dgvHistorialVentas.ReadOnly = true;
            dgvHistorialVentas.RowHeadersVisible = false;
            dgvHistorialVentas.RowTemplate.Height = 26;
            dgvHistorialVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorialVentas.Size = new Size(630, 188);
            dgvHistorialVentas.TabIndex = 5;
            // 
            // frmVentasMostrador
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1280, 720);
            Controls.Add(lblBannerSinergiaSocio);
            Controls.Add(pnlCatalogoYSeleccion);
            Controls.Add(pnlCarritoCompra);
            Controls.Add(pnlHistorialVentas);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmVentasMostrador";
            Text = "Punto de Venta de Mostrador e Inventario Local";
            FormClosing += frmVentasMostrador_FormClosing;
            Load += frmVentasMostrador_Load;
            pnlCatalogoYSeleccion.ResumeLayout(false);
            pnlCatalogoYSeleccion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidadVenta).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvStockCatalogo).EndInit();
            pnlCarritoCompra.ResumeLayout(false);
            pnlCarritoCompra.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).EndInit();
            pnlHistorialVentas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorialVentas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblBannerSinergiaSocio;
        private Panel pnlCatalogoYSeleccion;
        private Label lblTituloSeleccion;
        private Label lblBuscarSocioDNI;
        private TextBox txtDNISocio;
        private Button btnVerificarSocio;
        private CheckBox chkFiltrarAfines;
        private Label lblCategoriaFiltro;
        private ComboBox cmbCategoriaFiltro;
        private Label lblProductoSelect;
        private ComboBox cmbProductoSelect;
        private Label lblInfoProductoSeleccionado;
        private Label lblCantidadVenta;
        private NumericUpDown nudCantidadVenta;
        private Button btnAgregarCarrito;
        private DataGridView dgvStockCatalogo;
        private Panel pnlCarritoCompra;
        private Label lblTituloCarrito;
        private DataGridView dgvCarrito;
        private Button btnQuitarCarrito;
        private Label lblMetodoPagoVenta;
        private ComboBox cmbMetodoPagoVenta;
        private Label lblTotalCarritoValor;
        private CheckBox chkEmitirComprobante;
        private Button btnConfirmarVentaProd;
        private Panel pnlHistorialVentas;
        private Label lblTituloHistorial;
        private DateTimePicker dtpHistDesde;
        private DateTimePicker dtpHistHasta;
        private Button btnFiltrarHistorial;
        private Button btnReimprimirVentaPDF;
        private DataGridView dgvHistorialVentas;
    }
}