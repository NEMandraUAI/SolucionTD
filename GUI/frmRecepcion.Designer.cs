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
            txtDNI = new TextBox();
            btnBuscar = new Button();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtEmail = new TextBox();
            txtTelefono = new TextBox();
            cmbPlanes = new ComboBox();
            cmbMetodoPago = new ComboBox();
            btnConfirmarVenta = new Button();
            lblDNI = new Label();
            lblNombre = new Label();
            lblApellido = new Label();
            lblTelefono = new Label();
            lblEmail = new Label();
            lblPlan = new Label();
            lblMetodoPago = new Label();
            SuspendLayout();
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(12, 37);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(158, 23);
            txtDNI.TabIndex = 0;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(12, 80);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(158, 38);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(12, 197);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(158, 23);
            txtNombre.TabIndex = 2;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(199, 197);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(158, 23);
            txtApellido.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(199, 259);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(158, 23);
            txtEmail.TabIndex = 4;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(12, 259);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(158, 23);
            txtTelefono.TabIndex = 5;
            // 
            // cmbPlanes
            // 
            cmbPlanes.FormattingEnabled = true;
            cmbPlanes.Location = new Point(12, 321);
            cmbPlanes.Name = "cmbPlanes";
            cmbPlanes.Size = new Size(158, 23);
            cmbPlanes.TabIndex = 6;
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.FormattingEnabled = true;
            cmbMetodoPago.Location = new Point(199, 321);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(158, 23);
            cmbMetodoPago.TabIndex = 7;
            // 
            // btnConfirmarVenta
            // 
            btnConfirmarVenta.Location = new Point(12, 383);
            btnConfirmarVenta.Name = "btnConfirmarVenta";
            btnConfirmarVenta.Size = new Size(158, 38);
            btnConfirmarVenta.TabIndex = 8;
            btnConfirmarVenta.Text = "Confirmar Venta";
            btnConfirmarVenta.UseVisualStyleBackColor = true;
            btnConfirmarVenta.Click += btnConfirmarVenta_Click;
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.Location = new Point(12, 19);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 9;
            lblDNI.Text = "DNI";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(12, 179);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(53, 15);
            lblNombre.TabIndex = 10;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApellido.Location = new Point(199, 179);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(52, 15);
            lblApellido.TabIndex = 11;
            lblApellido.Text = "Apellido";
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTelefono.Location = new Point(12, 241);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(56, 15);
            lblTelefono.TabIndex = 12;
            lblTelefono.Text = "Teléfono";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(199, 241);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(41, 15);
            lblEmail.TabIndex = 13;
            lblEmail.Text = "E-Mail";
            // 
            // lblPlan
            // 
            lblPlan.AutoSize = true;
            lblPlan.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlan.Location = new Point(12, 303);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(125, 15);
            lblPlan.TabIndex = 14;
            lblPlan.Text = "Planes de Suscripción";
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMetodoPago.Location = new Point(199, 303);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(103, 15);
            lblMetodoPago.TabIndex = 15;
            lblMetodoPago.Text = "Métodos de Pago";
            // 
            // frmRecepcion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 450);
            Controls.Add(lblMetodoPago);
            Controls.Add(lblPlan);
            Controls.Add(lblEmail);
            Controls.Add(lblTelefono);
            Controls.Add(lblApellido);
            Controls.Add(lblNombre);
            Controls.Add(lblDNI);
            Controls.Add(btnConfirmarVenta);
            Controls.Add(cmbMetodoPago);
            Controls.Add(cmbPlanes);
            Controls.Add(txtTelefono);
            Controls.Add(txtEmail);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(btnBuscar);
            Controls.Add(txtDNI);
            Name = "frmRecepcion";
            Text = "frmRecepcion";
            FormClosing += frmRecepcion_FormClosing;
            Load += frmRecepcion_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtDNI;
        private Button btnBuscar;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private ComboBox cmbPlanes;
        private ComboBox cmbMetodoPago;
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