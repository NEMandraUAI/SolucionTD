using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmGestionIdiomas
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
            SuspendLayout();
            // 
            // frmGestionIdiomas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1200, 760);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmGestionIdiomas";
            Text = "frmGestionIdiomas";
            Load += AplicarEsteticaUnificadaDesigner;
            Load += frmGestionIdiomas_Load;
            ResumeLayout(false);
        }

        private void AplicarEsteticaUnificadaDesigner(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.BackColor = Color.FromArgb(243, 246, 249);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.ForeColor = Color.FromArgb(31, 41, 55);
            if (lblSeleccion != null)
            {
                lblSeleccion.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                lblSeleccion.ForeColor = Color.FromArgb(31, 41, 55);
                lblSeleccion.Location = new Point(28, 30);
            }
            if (cmbIdiomasAdmin != null)
            {
                cmbIdiomasAdmin.BackColor = Color.White;
                cmbIdiomasAdmin.FlatStyle = FlatStyle.Flat;
                cmbIdiomasAdmin.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                cmbIdiomasAdmin.ForeColor = Color.FromArgb(31, 41, 55);
                cmbIdiomasAdmin.Location = new Point(176, 26);
                cmbIdiomasAdmin.Size = new Size(240, 25);
            }
            if (btnNuevo != null)
            {
                btnNuevo.BackColor = Color.FromArgb(16, 185, 129);
                btnNuevo.FlatAppearance.BorderSize = 0;
                btnNuevo.FlatStyle = FlatStyle.Flat;
                btnNuevo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                btnNuevo.ForeColor = Color.White;
                btnNuevo.Location = new Point(436, 20);
                btnNuevo.Size = new Size(150, 36);
                btnNuevo.UseVisualStyleBackColor = false;
            }
            if (btnEliminar != null)
            {
                btnEliminar.BackColor = Color.FromArgb(185, 28, 28);
                btnEliminar.FlatAppearance.BorderSize = 0;
                btnEliminar.FlatStyle = FlatStyle.Flat;
                btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                btnEliminar.ForeColor = Color.White;
                btnEliminar.Location = new Point(600, 20);
                btnEliminar.Size = new Size(150, 36);
                btnEliminar.UseVisualStyleBackColor = false;
            }
            if (lblFiltroFormulario != null)
            {
                lblFiltroFormulario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                lblFiltroFormulario.ForeColor = Color.FromArgb(31, 41, 55);
                lblFiltroFormulario.Location = new Point(28, 78);
            }
            if (cmbFiltroFormulario != null)
            {
                cmbFiltroFormulario.BackColor = Color.White;
                cmbFiltroFormulario.FlatStyle = FlatStyle.Flat;
                cmbFiltroFormulario.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                cmbFiltroFormulario.ForeColor = Color.FromArgb(31, 41, 55);
                cmbFiltroFormulario.Location = new Point(176, 74);
                cmbFiltroFormulario.Size = new Size(240, 25);
            }
            if (lblFiltroTexto != null)
            {
                lblFiltroTexto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                lblFiltroTexto.ForeColor = Color.FromArgb(31, 41, 55);
                lblFiltroTexto.Location = new Point(436, 78);
            }
            if (txtFiltroTexto != null)
            {
                txtFiltroTexto.BackColor = Color.White;
                txtFiltroTexto.BorderStyle = BorderStyle.FixedSingle;
                txtFiltroTexto.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                txtFiltroTexto.ForeColor = Color.FromArgb(31, 41, 55);
                txtFiltroTexto.Location = new Point(536, 74);
                txtFiltroTexto.Size = new Size(280, 25);
            }
            if (btnLimpiarFiltros != null)
            {
                btnLimpiarFiltros.BackColor = Color.FromArgb(71, 85, 105);
                btnLimpiarFiltros.FlatAppearance.BorderSize = 0;
                btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
                btnLimpiarFiltros.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                btnLimpiarFiltros.ForeColor = Color.White;
                btnLimpiarFiltros.Location = new Point(832, 68);
                btnLimpiarFiltros.Size = new Size(140, 36);
                btnLimpiarFiltros.UseVisualStyleBackColor = false;
            }
            if (dgvTraducciones != null)
            {
                dgvTraducciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                dgvTraducciones.Location = new Point(28, 122);
                dgvTraducciones.Size = new Size(this.ClientSize.Width - 56, this.ClientSize.Height - 206);
                dgvTraducciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTraducciones.BackgroundColor = Color.White;
                dgvTraducciones.BorderStyle = BorderStyle.None;
                dgvTraducciones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvTraducciones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvTraducciones.EnableHeadersVisualStyles = false;
                dgvTraducciones.GridColor = Color.FromArgb(226, 232, 240);
                dgvTraducciones.RowHeadersVisible = false;
                dgvTraducciones.RowTemplate.Height = 36;
                dgvTraducciones.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 250, 252)
                };
                dgvTraducciones.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    BackColor = Color.FromArgb(31, 41, 55),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(31, 41, 55),
                    SelectionForeColor = Color.White,
                    WrapMode = DataGridViewTriState.False
                };
                dgvTraducciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvTraducciones.DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    SelectionBackColor = Color.FromArgb(219, 234, 254),
                    SelectionForeColor = Color.FromArgb(31, 41, 55),
                    Padding = new Padding(8, 4, 8, 4)
                };
                if (colFormulario != null) colFormulario.FillWeight = 22;
                if (colNombreControl != null) colNombreControl.FillWeight = 23;
                if (colTexto != null) colTexto.FillWeight = 55;
            }
            if (btnGuardar != null)
            {
                btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                btnGuardar.BackColor = Color.FromArgb(37, 99, 235);
                btnGuardar.FlatAppearance.BorderSize = 0;
                btnGuardar.FlatStyle = FlatStyle.Flat;
                btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
                btnGuardar.ForeColor = Color.White;
                btnGuardar.Location = new Point(this.ClientSize.Width - 308, this.ClientSize.Height - 66);
                btnGuardar.Size = new Size(280, 46);
                btnGuardar.UseVisualStyleBackColor = false;
            }
            this.ResumeLayout(true);
        }
        #endregion
    }
}