using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmPrediccionReabastecimiento
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
            pnlKpi1 = new Panel();
            lblKpi1Titulo = new Label();
            lblKpi1Valor = new Label();
            lblKpi1Sub = new Label();
            pnlKpi2 = new Panel();
            lblKpi2Titulo = new Label();
            lblKpi2Valor = new Label();
            lblKpi2Sub = new Label();
            pnlKpi3 = new Panel();
            lblKpi3Titulo = new Label();
            lblKpi3Valor = new Label();
            lblKpi3Sub = new Label();
            pnlKpi4 = new Panel();
            lblKpi4Titulo = new Label();
            lblKpi4Valor = new Label();
            lblKpi4Sub = new Label();
            pnlParametros = new Panel();
            lblMetodoPred = new Label();
            cmbMetodoPred = new ComboBox();
            lblHorizonteDias = new Label();
            cmbHorizonteDias = new ComboBox();
            chkIncluirSinergia = new CheckBox();
            btnEjecutarPrediccion = new Button();
            btnNuevoProducto = new Button();
            lblResumenSinergiaPoblacion = new Label();
            dgvPredicciones = new DataGridView();
            pnlGraficoContenedor = new Panel();
            lblTituloGrafico = new Label();
            pnlGraficoDemanda = new Panel();
            pnlOrdenesYAcciones = new Panel();
            lblTituloOrdenes = new Label();
            dgvOrdenesReab = new DataGridView();
            btnGenerarOrden = new Button();
            btnRecibirOrden = new Button();
            btnExportarPDF = new Button();
            btnExportarJSON = new Button();
            btnImportarJSON = new Button();
            pnlKpi1.SuspendLayout();
            pnlKpi2.SuspendLayout();
            pnlKpi3.SuspendLayout();
            pnlKpi4.SuspendLayout();
            pnlParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPredicciones).BeginInit();
            pnlGraficoContenedor.SuspendLayout();
            pnlOrdenesYAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesReab).BeginInit();
            SuspendLayout();
            // 
            // pnlKpi1
            // 
            pnlKpi1.BackColor = Color.White;
            pnlKpi1.BorderStyle = BorderStyle.FixedSingle;
            pnlKpi1.Controls.Add(lblKpi1Titulo);
            pnlKpi1.Controls.Add(lblKpi1Valor);
            pnlKpi1.Controls.Add(lblKpi1Sub);
            pnlKpi1.Location = new Point(20, 12);
            pnlKpi1.Name = "pnlKpi1";
            pnlKpi1.Size = new Size(298, 74);
            pnlKpi1.TabIndex = 0;
            // 
            // lblKpi1Titulo
            // 
            lblKpi1Titulo.AutoSize = true;
            lblKpi1Titulo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpi1Titulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpi1Titulo.Location = new Point(12, 8);
            lblKpi1Titulo.Name = "lblKpi1Titulo";
            lblKpi1Titulo.Size = new Size(188, 13);
            lblKpi1Titulo.TabIndex = 0;
            lblKpi1Titulo.Text = "FACTURACIÓN HISTÓRICA (4 MESES)";
            // 
            // lblKpi1Valor
            // 
            lblKpi1Valor.AutoSize = true;
            lblKpi1Valor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblKpi1Valor.ForeColor = Color.FromArgb(37, 99, 235);
            lblKpi1Valor.Location = new Point(12, 24);
            lblKpi1Valor.Name = "lblKpi1Valor";
            lblKpi1Valor.Size = new Size(76, 25);
            lblKpi1Valor.TabIndex = 1;
            lblKpi1Valor.Text = "$ 0,00";
            // 
            // lblKpi1Sub
            // 
            lblKpi1Sub.AutoSize = true;
            lblKpi1Sub.Font = new Font("Segoe UI", 8F);
            lblKpi1Sub.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpi1Sub.Location = new Point(12, 52);
            lblKpi1Sub.Name = "lblKpi1Sub";
            lblKpi1Sub.Size = new Size(125, 13);
            lblKpi1Sub.TabIndex = 2;
            lblKpi1Sub.Text = "Costo de ventas: $ 0,00";
            // 
            // pnlKpi2
            // 
            pnlKpi2.BackColor = Color.White;
            pnlKpi2.BorderStyle = BorderStyle.FixedSingle;
            pnlKpi2.Controls.Add(lblKpi2Titulo);
            pnlKpi2.Controls.Add(lblKpi2Valor);
            pnlKpi2.Controls.Add(lblKpi2Sub);
            pnlKpi2.Location = new Point(332, 12);
            pnlKpi2.Name = "pnlKpi2";
            pnlKpi2.Size = new Size(298, 74);
            pnlKpi2.TabIndex = 1;
            // 
            // lblKpi2Titulo
            // 
            lblKpi2Titulo.AutoSize = true;
            lblKpi2Titulo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpi2Titulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpi2Titulo.Location = new Point(12, 8);
            lblKpi2Titulo.Name = "lblKpi2Titulo";
            lblKpi2Titulo.Size = new Size(172, 13);
            lblKpi2Titulo.TabIndex = 0;
            lblKpi2Titulo.Text = "UTILIDAD BRUTA Y MARGEN (%)";
            // 
            // lblKpi2Valor
            // 
            lblKpi2Valor.AutoSize = true;
            lblKpi2Valor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblKpi2Valor.ForeColor = Color.FromArgb(21, 128, 61);
            lblKpi2Valor.Location = new Point(12, 24);
            lblKpi2Valor.Name = "lblKpi2Valor";
            lblKpi2Valor.Size = new Size(76, 25);
            lblKpi2Valor.TabIndex = 1;
            lblKpi2Valor.Text = "$ 0,00";
            // 
            // lblKpi2Sub
            // 
            lblKpi2Sub.AutoSize = true;
            lblKpi2Sub.Font = new Font("Segoe UI", 8F);
            lblKpi2Sub.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpi2Sub.Location = new Point(12, 52);
            lblKpi2Sub.Name = "lblKpi2Sub";
            lblKpi2Sub.Size = new Size(140, 13);
            lblKpi2Sub.TabIndex = 2;
            lblKpi2Sub.Text = "Margen promedio: 0,0%";
            // 
            // pnlKpi3
            // 
            pnlKpi3.BackColor = Color.White;
            pnlKpi3.BorderStyle = BorderStyle.FixedSingle;
            pnlKpi3.Controls.Add(lblKpi3Titulo);
            pnlKpi3.Controls.Add(lblKpi3Valor);
            pnlKpi3.Controls.Add(lblKpi3Sub);
            pnlKpi3.Location = new Point(644, 12);
            pnlKpi3.Name = "pnlKpi3";
            pnlKpi3.Size = new Size(298, 74);
            pnlKpi3.TabIndex = 2;
            // 
            // lblKpi3Titulo
            // 
            lblKpi3Titulo.AutoSize = true;
            lblKpi3Titulo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpi3Titulo.ForeColor = Color.FromArgb(100, 116, 139);
            lblKpi3Titulo.Location = new Point(12, 8);
            lblKpi3Titulo.Name = "lblKpi3Titulo";
            lblKpi3Titulo.Size = new Size(188, 13);
            lblKpi3Titulo.TabIndex = 0;
            lblKpi3Titulo.Text = "PRODUCTOS EN RIESGO (STOCK <= ROP)";
            // 
            // lblKpi3Valor
            // 
            lblKpi3Valor.AutoSize = true;
            lblKpi3Valor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblKpi3Valor.ForeColor = Color.FromArgb(185, 28, 28);
            lblKpi3Valor.Location = new Point(12, 24);
            lblKpi3Valor.Name = "lblKpi3Valor";
            lblKpi3Valor.Size = new Size(65, 25);
            lblKpi3Valor.TabIndex = 1;
            lblKpi3Valor.Text = "0 SKUs";
            // 
            // lblKpi3Sub
            // 
            lblKpi3Sub.AutoSize = true;
            lblKpi3Sub.Font = new Font("Segoe UI", 8F);
            lblKpi3Sub.ForeColor = Color.FromArgb(71, 85, 105);
            lblKpi3Sub.Location = new Point(12, 52);
            lblKpi3Sub.Name = "lblKpi3Sub";
            lblKpi3Sub.Size = new Size(175, 13);
            lblKpi3Sub.TabIndex = 2;
            lblKpi3Sub.Text = "Requieren reposición prioritaria";
            // 
            // pnlKpi4
            // 
            pnlKpi4.BackColor = Color.FromArgb(239, 246, 255);
            pnlKpi4.BorderStyle = BorderStyle.FixedSingle;
            pnlKpi4.Controls.Add(lblKpi4Titulo);
            pnlKpi4.Controls.Add(lblKpi4Valor);
            pnlKpi4.Controls.Add(lblKpi4Sub);
            pnlKpi4.Location = new Point(956, 12);
            pnlKpi4.Name = "pnlKpi4";
            pnlKpi4.Size = new Size(304, 74);
            pnlKpi4.TabIndex = 3;
            // 
            // lblKpi4Titulo
            // 
            lblKpi4Titulo.AutoSize = true;
            lblKpi4Titulo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpi4Titulo.ForeColor = Color.FromArgb(30, 58, 138);
            lblKpi4Titulo.Location = new Point(12, 8);
            lblKpi4Titulo.Name = "lblKpi4Titulo";
            lblKpi4Titulo.Size = new Size(192, 13);
            lblKpi4Titulo.TabIndex = 0;
            lblKpi4Titulo.Text = "INVERSIÓN ESTIMADA DE REPOSICIÓN";
            // 
            // lblKpi4Valor
            // 
            lblKpi4Valor.AutoSize = true;
            lblKpi4Valor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblKpi4Valor.ForeColor = Color.FromArgb(109, 40, 217);
            lblKpi4Valor.Location = new Point(12, 24);
            lblKpi4Valor.Name = "lblKpi4Valor";
            lblKpi4Valor.Size = new Size(76, 25);
            lblKpi4Valor.TabIndex = 1;
            lblKpi4Valor.Text = "$ 0,00";
            // 
            // lblKpi4Sub
            // 
            lblKpi4Sub.AutoSize = true;
            lblKpi4Sub.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKpi4Sub.ForeColor = Color.FromArgb(30, 58, 138);
            lblKpi4Sub.Location = new Point(12, 52);
            lblKpi4Sub.Name = "lblKpi4Sub";
            lblKpi4Sub.Size = new Size(162, 13);
            lblKpi4Sub.TabIndex = 2;
            lblKpi4Sub.Text = "0 unidades aprobadas p/ compra";
            // 
            // pnlParametros
            // 
            pnlParametros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlParametros.BackColor = Color.White;
            pnlParametros.BorderStyle = BorderStyle.FixedSingle;
            pnlParametros.Controls.Add(lblMetodoPred);
            pnlParametros.Controls.Add(cmbMetodoPred);
            pnlParametros.Controls.Add(lblHorizonteDias);
            pnlParametros.Controls.Add(cmbHorizonteDias);
            pnlParametros.Controls.Add(chkIncluirSinergia);
            pnlParametros.Controls.Add(btnEjecutarPrediccion);
            pnlParametros.Controls.Add(btnNuevoProducto);
            pnlParametros.Controls.Add(lblResumenSinergiaPoblacion);
            pnlParametros.Location = new Point(20, 94);
            pnlParametros.Name = "pnlParametros";
            pnlParametros.Size = new Size(1240, 68);
            pnlParametros.TabIndex = 4;
            // 
            // lblMetodoPred
            // 
            lblMetodoPred.AutoSize = true;
            lblMetodoPred.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
            lblMetodoPred.Location = new Point(12, 12);
            lblMetodoPred.Name = "lblMetodoPred";
            lblMetodoPred.Size = new Size(112, 15);
            lblMetodoPred.TabIndex = 0;
            lblMetodoPred.Text = "Modelo Predictivo:";
            // 
            // cmbMetodoPred
            // 
            cmbMetodoPred.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPred.FlatStyle = FlatStyle.Flat;
            cmbMetodoPred.Font = new Font("Segoe UI", 9F);
            cmbMetodoPred.Location = new Point(126, 8);
            cmbMetodoPred.Name = "cmbMetodoPred";
            cmbMetodoPred.Size = new Size(214, 23);
            cmbMetodoPred.TabIndex = 1;
            cmbMetodoPred.SelectedIndexChanged += ParametrosPrediccion_Changed;
            // 
            // lblHorizonteDias
            // 
            lblHorizonteDias.AutoSize = true;
            lblHorizonteDias.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
            lblHorizonteDias.Location = new Point(348, 12);
            lblHorizonteDias.Name = "lblHorizonteDias";
            lblHorizonteDias.Size = new Size(98, 15);
            lblHorizonteDias.TabIndex = 2;
            lblHorizonteDias.Text = "Horizonte (Días):";
            // 
            // cmbHorizonteDias
            // 
            cmbHorizonteDias.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHorizonteDias.FlatStyle = FlatStyle.Flat;
            cmbHorizonteDias.Font = new Font("Segoe UI", 9F);
            cmbHorizonteDias.Location = new Point(450, 8);
            cmbHorizonteDias.Name = "cmbHorizonteDias";
            cmbHorizonteDias.Size = new Size(64, 23);
            cmbHorizonteDias.TabIndex = 3;
            cmbHorizonteDias.SelectedIndexChanged += ParametrosPrediccion_Changed;
            // 
            // chkIncluirSinergia
            // 
            chkIncluirSinergia.AutoSize = true;
            chkIncluirSinergia.Checked = true;
            chkIncluirSinergia.CheckState = CheckState.Checked;
            chkIncluirSinergia.Font = new Font("Segoe UI", 8.3F, FontStyle.Bold);
            chkIncluirSinergia.ForeColor = Color.FromArgb(30, 58, 138);
            chkIncluirSinergia.Location = new Point(524, 10);
            chkIncluirSinergia.Name = "chkIncluirSinergia";
            chkIncluirSinergia.Size = new Size(320, 19);
            chkIncluirSinergia.TabIndex = 4;
            chkIncluirSinergia.Text = "🔗 Ponderar demanda inducida por Rutinas y Planes activos";
            chkIncluirSinergia.UseVisualStyleBackColor = true;
            chkIncluirSinergia.CheckedChanged += ParametrosPrediccion_Changed;
            // 
            // btnEjecutarPrediccion
            // 
            btnEjecutarPrediccion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEjecutarPrediccion.BackColor = Color.FromArgb(37, 99, 235);
            btnEjecutarPrediccion.FlatAppearance.BorderSize = 0;
            btnEjecutarPrediccion.FlatStyle = FlatStyle.Flat;
            btnEjecutarPrediccion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnEjecutarPrediccion.ForeColor = Color.White;
            btnEjecutarPrediccion.Location = new Point(854, 6);
            btnEjecutarPrediccion.Name = "btnEjecutarPrediccion";
            btnEjecutarPrediccion.Size = new Size(192, 28);
            btnEjecutarPrediccion.TabIndex = 5;
            btnEjecutarPrediccion.Text = "🔄 Restaurar Sugerencias";
            btnEjecutarPrediccion.UseVisualStyleBackColor = false;
            btnEjecutarPrediccion.Click += btnEjecutarPrediccion_Click;
            // 
            // btnNuevoProducto
            // 
            btnNuevoProducto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoProducto.BackColor = Color.FromArgb(71, 85, 105);
            btnNuevoProducto.FlatAppearance.BorderSize = 0;
            btnNuevoProducto.FlatStyle = FlatStyle.Flat;
            btnNuevoProducto.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnNuevoProducto.ForeColor = Color.White;
            btnNuevoProducto.Location = new Point(1054, 6);
            btnNuevoProducto.Name = "btnNuevoProducto";
            btnNuevoProducto.Size = new Size(176, 28);
            btnNuevoProducto.TabIndex = 6;
            btnNuevoProducto.Text = "➕ Nuevo / Editar Producto";
            btnNuevoProducto.UseVisualStyleBackColor = false;
            btnNuevoProducto.Click += btnNuevoProducto_Click;
            // 
            // lblResumenSinergiaPoblacion
            // 
            lblResumenSinergiaPoblacion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblResumenSinergiaPoblacion.BackColor = Color.FromArgb(241, 245, 249);
            lblResumenSinergiaPoblacion.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
            lblResumenSinergiaPoblacion.ForeColor = Color.FromArgb(51, 65, 85);
            lblResumenSinergiaPoblacion.Location = new Point(12, 39);
            lblResumenSinergiaPoblacion.Name = "lblResumenSinergiaPoblacion";
            lblResumenSinergiaPoblacion.Padding = new Padding(8, 0, 8, 0);
            lblResumenSinergiaPoblacion.Size = new Size(1218, 22);
            lblResumenSinergiaPoblacion.TabIndex = 7;
            lblResumenSinergiaPoblacion.Text = "Sinergia Inter-Áreas...";
            lblResumenSinergiaPoblacion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvPredicciones
            // 
            dgvPredicciones.AllowUserToAddRows = false;
            dgvPredicciones.AllowUserToDeleteRows = false;
            dgvPredicciones.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvPredicciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPredicciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPredicciones.BackgroundColor = Color.White;
            dgvPredicciones.BorderStyle = BorderStyle.FixedSingle;
            dgvPredicciones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPredicciones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPredicciones.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 8.2F, FontStyle.Bold),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White
            };
            dgvPredicciones.ColumnHeadersHeight = 32;
            dgvPredicciones.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55)
            };
            dgvPredicciones.EnableHeadersVisualStyles = false;
            dgvPredicciones.GridColor = Color.FromArgb(226, 232, 240);
            dgvPredicciones.Location = new Point(20, 170);
            dgvPredicciones.MultiSelect = false;
            dgvPredicciones.Name = "dgvPredicciones";
            dgvPredicciones.RowHeadersVisible = false;
            dgvPredicciones.RowTemplate.Height = 28;
            dgvPredicciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPredicciones.Size = new Size(1240, 272);
            dgvPredicciones.TabIndex = 5;
            dgvPredicciones.CellEndEdit += dgvPredicciones_CellEndEdit;
            dgvPredicciones.CellFormatting += dgvPredicciones_CellFormatting;
            dgvPredicciones.DataError += dgvPredicciones_DataError;
            dgvPredicciones.SelectionChanged += dgvPredicciones_SelectionChanged;
            // 
            // pnlGraficoContenedor
            // 
            pnlGraficoContenedor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlGraficoContenedor.BackColor = Color.White;
            pnlGraficoContenedor.BorderStyle = BorderStyle.FixedSingle;
            pnlGraficoContenedor.Controls.Add(pnlGraficoDemanda);
            pnlGraficoContenedor.Controls.Add(lblTituloGrafico);
            pnlGraficoContenedor.Location = new Point(20, 450);
            pnlGraficoContenedor.Name = "pnlGraficoContenedor";
            pnlGraficoContenedor.Size = new Size(540, 254);
            pnlGraficoContenedor.TabIndex = 6;
            // 
            // lblTituloGrafico
            // 
            lblTituloGrafico.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloGrafico.Dock = DockStyle.Top;
            lblTituloGrafico.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
            lblTituloGrafico.ForeColor = Color.White;
            lblTituloGrafico.Location = new Point(0, 0);
            lblTituloGrafico.Name = "lblTituloGrafico";
            lblTituloGrafico.Padding = new Padding(10, 0, 0, 0);
            lblTituloGrafico.Size = new Size(538, 28);
            lblTituloGrafico.TabIndex = 0;
            lblTituloGrafico.Text = "CURVA DE DEMANDA HISTÓRICA VS. PROYECTADA Y PUNTO DE REORDEN (ROP)";
            lblTituloGrafico.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlGraficoDemanda
            // 
            pnlGraficoDemanda.BackColor = Color.White;
            pnlGraficoDemanda.Dock = DockStyle.Fill;
            pnlGraficoDemanda.Location = new Point(0, 28);
            pnlGraficoDemanda.Name = "pnlGraficoDemanda";
            pnlGraficoDemanda.Size = new Size(538, 224);
            pnlGraficoDemanda.TabIndex = 1;
            pnlGraficoDemanda.Paint += pnlGraficoDemanda_Paint;
            // 
            // pnlOrdenesYAcciones
            // 
            pnlOrdenesYAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlOrdenesYAcciones.BackColor = Color.White;
            pnlOrdenesYAcciones.BorderStyle = BorderStyle.FixedSingle;
            pnlOrdenesYAcciones.Controls.Add(lblTituloOrdenes);
            pnlOrdenesYAcciones.Controls.Add(dgvOrdenesReab);
            pnlOrdenesYAcciones.Controls.Add(btnGenerarOrden);
            pnlOrdenesYAcciones.Controls.Add(btnRecibirOrden);
            pnlOrdenesYAcciones.Controls.Add(btnExportarPDF);
            pnlOrdenesYAcciones.Controls.Add(btnExportarJSON);
            pnlOrdenesYAcciones.Controls.Add(btnImportarJSON);
            pnlOrdenesYAcciones.Location = new Point(572, 450);
            pnlOrdenesYAcciones.Name = "pnlOrdenesYAcciones";
            pnlOrdenesYAcciones.Size = new Size(688, 254);
            pnlOrdenesYAcciones.TabIndex = 7;
            // 
            // lblTituloOrdenes
            // 
            lblTituloOrdenes.BackColor = Color.FromArgb(31, 41, 55);
            lblTituloOrdenes.Dock = DockStyle.Top;
            lblTituloOrdenes.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
            lblTituloOrdenes.ForeColor = Color.White;
            lblTituloOrdenes.Location = new Point(0, 0);
            lblTituloOrdenes.Name = "lblTituloOrdenes";
            lblTituloOrdenes.Padding = new Padding(10, 0, 0, 0);
            lblTituloOrdenes.Size = new Size(686, 28);
            lblTituloOrdenes.TabIndex = 0;
            lblTituloOrdenes.Text = "ÓRDENES DE REABASTECIMIENTO, REPORTE EJECUTIVO PDF Y ARCHIVO SERIALIZADO JSON";
            lblTituloOrdenes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvOrdenesReab
            // 
            dgvOrdenesReab.AllowUserToAddRows = false;
            dgvOrdenesReab.AllowUserToDeleteRows = false;
            dgvOrdenesReab.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            dgvOrdenesReab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrdenesReab.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenesReab.BackgroundColor = Color.White;
            dgvOrdenesReab.BorderStyle = BorderStyle.FixedSingle;
            dgvOrdenesReab.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvOrdenesReab.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvOrdenesReab.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(31, 41, 55),
                SelectionForeColor = Color.White
            };
            dgvOrdenesReab.ColumnHeadersHeight = 26;
            dgvOrdenesReab.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 8.2F),
                ForeColor = Color.FromArgb(31, 41, 55),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(31, 41, 55)
            };
            dgvOrdenesReab.EnableHeadersVisualStyles = false;
            dgvOrdenesReab.GridColor = Color.FromArgb(226, 232, 240);
            dgvOrdenesReab.Location = new Point(12, 36);
            dgvOrdenesReab.MultiSelect = false;
            dgvOrdenesReab.Name = "dgvOrdenesReab";
            dgvOrdenesReab.ReadOnly = true;
            dgvOrdenesReab.RowHeadersVisible = false;
            dgvOrdenesReab.RowTemplate.Height = 24;
            dgvOrdenesReab.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenesReab.Size = new Size(662, 130);
            dgvOrdenesReab.TabIndex = 1;
            // 
            // btnGenerarOrden
            // 
            btnGenerarOrden.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGenerarOrden.BackColor = Color.FromArgb(16, 185, 129);
            btnGenerarOrden.FlatAppearance.BorderSize = 0;
            btnGenerarOrden.FlatStyle = FlatStyle.Flat;
            btnGenerarOrden.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnGenerarOrden.ForeColor = Color.White;
            btnGenerarOrden.Location = new Point(12, 174);
            btnGenerarOrden.Name = "btnGenerarOrden";
            btnGenerarOrden.Size = new Size(324, 32);
            btnGenerarOrden.TabIndex = 2;
            btnGenerarOrden.Text = "📦 Confirmar Orden de Reabastecimiento";
            btnGenerarOrden.UseVisualStyleBackColor = false;
            btnGenerarOrden.Click += btnGenerarOrden_Click;
            // 
            // btnRecibirOrden
            // 
            btnRecibirOrden.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnRecibirOrden.BackColor = Color.FromArgb(37, 99, 235);
            btnRecibirOrden.FlatAppearance.BorderSize = 0;
            btnRecibirOrden.FlatStyle = FlatStyle.Flat;
            btnRecibirOrden.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnRecibirOrden.ForeColor = Color.White;
            btnRecibirOrden.Location = new Point(344, 174);
            btnRecibirOrden.Name = "btnRecibirOrden";
            btnRecibirOrden.Size = new Size(330, 32);
            btnRecibirOrden.TabIndex = 3;
            btnRecibirOrden.Text = "✅ Marcar Orden Recibida (Sumar Stock)";
            btnRecibirOrden.UseVisualStyleBackColor = false;
            btnRecibirOrden.Click += btnRecibirOrden_Click;
            // 
            // btnExportarPDF
            // 
            btnExportarPDF.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportarPDF.BackColor = Color.FromArgb(185, 28, 28);
            btnExportarPDF.FlatAppearance.BorderSize = 0;
            btnExportarPDF.FlatStyle = FlatStyle.Flat;
            btnExportarPDF.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnExportarPDF.ForeColor = Color.White;
            btnExportarPDF.Location = new Point(12, 212);
            btnExportarPDF.Name = "btnExportarPDF";
            btnExportarPDF.Size = new Size(214, 32);
            btnExportarPDF.TabIndex = 4;
            btnExportarPDF.Text = "📄 Reporte Ejecutivo PDF";
            btnExportarPDF.UseVisualStyleBackColor = false;
            btnExportarPDF.Click += btnExportarPDF_Click;
            // 
            // btnExportarJSON
            // 
            btnExportarJSON.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportarJSON.BackColor = Color.FromArgb(109, 40, 217);
            btnExportarJSON.FlatAppearance.BorderSize = 0;
            btnExportarJSON.FlatStyle = FlatStyle.Flat;
            btnExportarJSON.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnExportarJSON.ForeColor = Color.White;
            btnExportarJSON.Location = new Point(234, 212);
            btnExportarJSON.Name = "btnExportarJSON";
            btnExportarJSON.Size = new Size(218, 32);
            btnExportarJSON.TabIndex = 5;
            btnExportarJSON.Text = "💾 Exportar JSON Serializado";
            btnExportarJSON.UseVisualStyleBackColor = false;
            btnExportarJSON.Click += btnExportarJSON_Click;
            // 
            // btnImportarJSON
            // 
            btnImportarJSON.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnImportarJSON.BackColor = Color.FromArgb(31, 41, 55);
            btnImportarJSON.FlatAppearance.BorderSize = 0;
            btnImportarJSON.FlatStyle = FlatStyle.Flat;
            btnImportarJSON.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnImportarJSON.ForeColor = Color.White;
            btnImportarJSON.Location = new Point(460, 212);
            btnImportarJSON.Name = "btnImportarJSON";
            btnImportarJSON.Size = new Size(214, 32);
            btnImportarJSON.TabIndex = 6;
            btnImportarJSON.Text = "📂 Importar / Auditar JSON";
            btnImportarJSON.UseVisualStyleBackColor = false;
            btnImportarJSON.Click += btnImportarJSON_Click;
            // 
            // frmPrediccionReabastecimiento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1280, 720);
            Controls.Add(pnlKpi1);
            Controls.Add(pnlKpi2);
            Controls.Add(pnlKpi3);
            Controls.Add(pnlKpi4);
            Controls.Add(pnlParametros);
            Controls.Add(dgvPredicciones);
            Controls.Add(pnlGraficoContenedor);
            Controls.Add(pnlOrdenesYAcciones);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmPrediccionReabastecimiento";
            Text = "Inteligencia Comercial, Predicción de Demanda y Reabastecimiento";
            FormClosing += frmPrediccionReabastecimiento_FormClosing;
            Load += frmPrediccionReabastecimiento_Load;
            Resize += frmPrediccionReabastecimiento_Resize;
            pnlKpi1.ResumeLayout(false);
            pnlKpi1.PerformLayout();
            pnlKpi2.ResumeLayout(false);
            pnlKpi2.PerformLayout();
            pnlKpi3.ResumeLayout(false);
            pnlKpi3.PerformLayout();
            pnlKpi4.ResumeLayout(false);
            pnlKpi4.PerformLayout();
            pnlParametros.ResumeLayout(false);
            pnlParametros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPredicciones).EndInit();
            pnlGraficoContenedor.ResumeLayout(false);
            pnlOrdenesYAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesReab).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlKpi1;
        private Label lblKpi1Titulo;
        private Label lblKpi1Valor;
        private Label lblKpi1Sub;
        private Panel pnlKpi2;
        private Label lblKpi2Titulo;
        private Label lblKpi2Valor;
        private Label lblKpi2Sub;
        private Panel pnlKpi3;
        private Label lblKpi3Titulo;
        private Label lblKpi3Valor;
        private Label lblKpi3Sub;
        private Panel pnlKpi4;
        private Label lblKpi4Titulo;
        private Label lblKpi4Valor;
        private Label lblKpi4Sub;
        private Panel pnlParametros;
        private Label lblMetodoPred;
        private ComboBox cmbMetodoPred;
        private Label lblHorizonteDias;
        private ComboBox cmbHorizonteDias;
        private CheckBox chkIncluirSinergia;
        private Button btnEjecutarPrediccion;
        private Button btnNuevoProducto;
        private Label lblResumenSinergiaPoblacion;
        private DataGridView dgvPredicciones;
        private Panel pnlGraficoContenedor;
        private Label lblTituloGrafico;
        private Panel pnlGraficoDemanda;
        private Panel pnlOrdenesYAcciones;
        private Label lblTituloOrdenes;
        private DataGridView dgvOrdenesReab;
        private Button btnGenerarOrden;
        private Button btnRecibirOrden;
        private Button btnExportarPDF;
        private Button btnExportarJSON;
        private Button btnImportarJSON;
    }
}