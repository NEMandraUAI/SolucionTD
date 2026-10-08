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
            btnConsultarPlanes = new Button();
            SuspendLayout();
            // 
            // txtDNI
            // 
            txtDNI.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtDNI.BackColor = Color.White;
            txtDNI.BorderStyle = BorderStyle.FixedSingle;
            txtDNI.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDNI.ForeColor = Color.FromArgb(31, 41, 55);
            txtDNI.Location = new Point(28, 54);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(360, 23);
            txtDNI.TabIndex = 0;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnBuscar.BackColor = Color.FromArgb(71, 85, 105);
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(420, 50);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(160, 42);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtNombre.BackColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtNombre.ForeColor = Color.FromArgb(31, 41, 55);
            txtNombre.Location = new Point(28, 148);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(360, 23);
            txtNombre.TabIndex = 2;
            // 
            // txtApellido
            // 
            txtApellido.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtApellido.BackColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtApellido.ForeColor = Color.FromArgb(31, 41, 55);
            txtApellido.Location = new Point(420, 148);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(372, 23);
            txtApellido.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtEmail.BackColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtEmail.ForeColor = Color.FromArgb(31, 41, 55);
            txtEmail.Location = new Point(420, 244);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(372, 23);
            txtEmail.TabIndex = 4;
            // 
            // txtTelefono
            // 
            txtTelefono.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtTelefono.BackColor = Color.White;
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtTelefono.ForeColor = Color.FromArgb(31, 41, 55);
            txtTelefono.Location = new Point(28, 244);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(360, 23);
            txtTelefono.TabIndex = 5;
            // 
            // cmbPlanes
            // 
            cmbPlanes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbPlanes.BackColor = Color.White;
            cmbPlanes.FlatStyle = FlatStyle.Flat;
            cmbPlanes.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbPlanes.ForeColor = Color.FromArgb(31, 41, 55);
            cmbPlanes.FormattingEnabled = true;
            cmbPlanes.Location = new Point(28, 340);
            cmbPlanes.Name = "cmbPlanes";
            cmbPlanes.Size = new Size(360, 28);
            cmbPlanes.TabIndex = 6;
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbMetodoPago.BackColor = Color.White;
            cmbMetodoPago.FlatStyle = FlatStyle.Flat;
            cmbMetodoPago.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbMetodoPago.ForeColor = Color.FromArgb(31, 41, 55);
            cmbMetodoPago.FormattingEnabled = true;
            cmbMetodoPago.Location = new Point(420, 340);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(372, 28);
            cmbMetodoPago.TabIndex = 7;
            // 
            // btnConfirmarVenta
            // 
            btnConfirmarVenta.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnConfirmarVenta.BackColor = Color.FromArgb(37, 99, 235);
            btnConfirmarVenta.FlatAppearance.BorderSize = 0;
            btnConfirmarVenta.FlatStyle = FlatStyle.Flat;
            btnConfirmarVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnConfirmarVenta.ForeColor = Color.White;
            btnConfirmarVenta.Location = new Point(420, 408);
            btnConfirmarVenta.Name = "btnConfirmarVenta";
            btnConfirmarVenta.Size = new Size(372, 46);
            btnConfirmarVenta.TabIndex = 8;
            btnConfirmarVenta.Text = "Confirmar Venta";
            btnConfirmarVenta.UseVisualStyleBackColor = false;
            btnConfirmarVenta.Click += btnConfirmarVenta_Click;
            // 
            // lblDNI
            // 
            lblDNI.AutoSize = true;
            lblDNI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDNI.ForeColor = Color.FromArgb(31, 41, 55);
            lblDNI.Location = new Point(28, 28);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(29, 15);
            lblDNI.TabIndex = 9;
            lblDNI.Text = "DNI";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.ForeColor = Color.FromArgb(31, 41, 55);
            lblNombre.Location = new Point(28, 122);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(53, 15);
            lblNombre.TabIndex = 10;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApellido.ForeColor = Color.FromArgb(31, 41, 55);
            lblApellido.Location = new Point(420, 122);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(52, 15);
            lblApellido.TabIndex = 11;
            lblApellido.Text = "Apellido";
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTelefono.ForeColor = Color.FromArgb(31, 41, 55);
            lblTelefono.Location = new Point(28, 218);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(56, 15);
            lblTelefono.TabIndex = 12;
            lblTelefono.Text = "Teléfono";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.ForeColor = Color.FromArgb(31, 41, 55);
            lblEmail.Location = new Point(420, 218);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(41, 15);
            lblEmail.TabIndex = 13;
            lblEmail.Text = "E-Mail";
            // 
            // lblPlan
            // 
            lblPlan.AutoSize = true;
            lblPlan.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlan.ForeColor = Color.FromArgb(31, 41, 55);
            lblPlan.Location = new Point(28, 314);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(125, 15);
            lblPlan.TabIndex = 14;
            lblPlan.Text = "Planes de Suscripción";
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMetodoPago.ForeColor = Color.FromArgb(31, 41, 55);
            lblMetodoPago.Location = new Point(420, 314);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(103, 15);
            lblMetodoPago.TabIndex = 15;
            lblMetodoPago.Text = "Métodos de Pago";
            // 
            // btnConsultarPlanes
            // 
            btnConsultarPlanes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnConsultarPlanes.BackColor = Color.FromArgb(71, 85, 105);
            btnConsultarPlanes.FlatAppearance.BorderSize = 0;
            btnConsultarPlanes.FlatStyle = FlatStyle.Flat;
            btnConsultarPlanes.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnConsultarPlanes.ForeColor = Color.White;
            btnConsultarPlanes.Location = new Point(28, 408);
            btnConsultarPlanes.Name = "btnConsultarPlanes";
            btnConsultarPlanes.Size = new Size(360, 46);
            btnConsultarPlanes.TabIndex = 16;
            btnConsultarPlanes.Text = "Consultar Planes";
            btnConsultarPlanes.UseVisualStyleBackColor = false;
            btnConsultarPlanes.Click += btnConsultarPlanes_Click;
            // 
            // frmRecepcion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(820, 500);
            Controls.Add(btnConsultarPlanes);
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
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
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
        private Button btnConsultarPlanes;
    }
}