namespace GUI
{
    partial class frmMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenu));
            pnlLateral = new Panel();
            btnMenuCerrarSesion = new Button();
            btnMenuSalir = new Button();
            btnMenuNutricion = new Button();
            btnMenuEntrenamiento = new Button();
            btnMenuRecepcion = new Button();
            btnMenuRegistrarse = new Button();
            btnMenuIniciarSesion = new Button();
            pnlCabecera = new Panel();
            pnlSubNutricion = new Panel();
            pnlSubEntrenamiento = new Panel();
            btnSubGestionRutinas = new Button();
            btnSubAsignarRutina = new Button();
            pnlSubRecepcion = new Panel();
            btnSubAgendarTurno = new Button();
            btnSubConsultarComprobantes = new Button();
            btnSubVenderPlan = new Button();
            panelIdioma = new Panel();
            cmbIdiomas = new ComboBox();
            btnNuevoIdioma = new Button();
            pnlContenedor = new Panel();
            btnMenuInicio = new Button();
            pnlLateral.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlSubEntrenamiento.SuspendLayout();
            pnlSubRecepcion.SuspendLayout();
            panelIdioma.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLateral
            // 
            pnlLateral.Controls.Add(btnMenuCerrarSesion);
            pnlLateral.Controls.Add(btnMenuSalir);
            pnlLateral.Controls.Add(btnMenuNutricion);
            pnlLateral.Controls.Add(btnMenuEntrenamiento);
            pnlLateral.Controls.Add(btnMenuRecepcion);
            pnlLateral.Controls.Add(btnMenuInicio);
            pnlLateral.Controls.Add(btnMenuRegistrarse);
            pnlLateral.Controls.Add(btnMenuIniciarSesion);
            pnlLateral.Dock = DockStyle.Left;
            pnlLateral.Location = new Point(0, 0);
            pnlLateral.Name = "pnlLateral";
            pnlLateral.Size = new Size(250, 459);
            pnlLateral.TabIndex = 7;
            // 
            // btnMenuCerrarSesion
            // 
            btnMenuCerrarSesion.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuCerrarSesion.Dock = DockStyle.Bottom;
            btnMenuCerrarSesion.FlatAppearance.BorderSize = 0;
            btnMenuCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnMenuCerrarSesion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuCerrarSesion.ForeColor = Color.White;
            btnMenuCerrarSesion.Location = new Point(0, 369);
            btnMenuCerrarSesion.Name = "btnMenuCerrarSesion";
            btnMenuCerrarSesion.Size = new Size(250, 45);
            btnMenuCerrarSesion.TabIndex = 7;
            btnMenuCerrarSesion.Text = "Cerrar Sesión";
            btnMenuCerrarSesion.UseVisualStyleBackColor = false;
            btnMenuCerrarSesion.Click += btnMenuCerrarSesion_Click;
            // 
            // btnMenuSalir
            // 
            btnMenuSalir.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuSalir.Dock = DockStyle.Bottom;
            btnMenuSalir.FlatAppearance.BorderSize = 0;
            btnMenuSalir.FlatStyle = FlatStyle.Flat;
            btnMenuSalir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuSalir.ForeColor = Color.White;
            btnMenuSalir.Location = new Point(0, 414);
            btnMenuSalir.Name = "btnMenuSalir";
            btnMenuSalir.Size = new Size(250, 45);
            btnMenuSalir.TabIndex = 6;
            btnMenuSalir.Text = "Salir";
            btnMenuSalir.UseVisualStyleBackColor = false;
            btnMenuSalir.Click += btnSalir_Click;
            // 
            // btnMenuNutricion
            // 
            btnMenuNutricion.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuNutricion.Dock = DockStyle.Top;
            btnMenuNutricion.FlatAppearance.BorderSize = 0;
            btnMenuNutricion.FlatStyle = FlatStyle.Flat;
            btnMenuNutricion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuNutricion.ForeColor = Color.White;
            btnMenuNutricion.Location = new Point(0, 225);
            btnMenuNutricion.Name = "btnMenuNutricion";
            btnMenuNutricion.Size = new Size(250, 45);
            btnMenuNutricion.TabIndex = 5;
            btnMenuNutricion.Text = "Nutrición";
            btnMenuNutricion.UseVisualStyleBackColor = false;
            btnMenuNutricion.Click += btnMenuNutricion_Click;
            // 
            // btnMenuEntrenamiento
            // 
            btnMenuEntrenamiento.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuEntrenamiento.Dock = DockStyle.Top;
            btnMenuEntrenamiento.FlatAppearance.BorderSize = 0;
            btnMenuEntrenamiento.FlatStyle = FlatStyle.Flat;
            btnMenuEntrenamiento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuEntrenamiento.ForeColor = Color.White;
            btnMenuEntrenamiento.Location = new Point(0, 180);
            btnMenuEntrenamiento.Name = "btnMenuEntrenamiento";
            btnMenuEntrenamiento.Size = new Size(250, 45);
            btnMenuEntrenamiento.TabIndex = 4;
            btnMenuEntrenamiento.Text = "Entrenamiento";
            btnMenuEntrenamiento.UseVisualStyleBackColor = false;
            btnMenuEntrenamiento.Click += btnMenuEntrenamiento_Click;
            // 
            // btnMenuRecepcion
            // 
            btnMenuRecepcion.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuRecepcion.Dock = DockStyle.Top;
            btnMenuRecepcion.FlatAppearance.BorderSize = 0;
            btnMenuRecepcion.FlatStyle = FlatStyle.Flat;
            btnMenuRecepcion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuRecepcion.ForeColor = Color.White;
            btnMenuRecepcion.Location = new Point(0, 135);
            btnMenuRecepcion.Name = "btnMenuRecepcion";
            btnMenuRecepcion.Size = new Size(250, 45);
            btnMenuRecepcion.TabIndex = 3;
            btnMenuRecepcion.Text = "Recepción";
            btnMenuRecepcion.UseVisualStyleBackColor = false;
            btnMenuRecepcion.Click += btnMenuRecepcion_Click;
            // 
            // btnMenuRegistrarse
            // 
            btnMenuRegistrarse.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuRegistrarse.Dock = DockStyle.Top;
            btnMenuRegistrarse.FlatAppearance.BorderSize = 0;
            btnMenuRegistrarse.FlatStyle = FlatStyle.Flat;
            btnMenuRegistrarse.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuRegistrarse.ForeColor = Color.White;
            btnMenuRegistrarse.Location = new Point(0, 45);
            btnMenuRegistrarse.Name = "btnMenuRegistrarse";
            btnMenuRegistrarse.Size = new Size(250, 45);
            btnMenuRegistrarse.TabIndex = 2;
            btnMenuRegistrarse.Text = "Registrarse";
            btnMenuRegistrarse.UseVisualStyleBackColor = false;
            btnMenuRegistrarse.Click += btnRegistrarse_Click;
            // 
            // btnMenuIniciarSesion
            // 
            btnMenuIniciarSesion.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuIniciarSesion.Dock = DockStyle.Top;
            btnMenuIniciarSesion.FlatAppearance.BorderSize = 0;
            btnMenuIniciarSesion.FlatStyle = FlatStyle.Flat;
            btnMenuIniciarSesion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuIniciarSesion.ForeColor = Color.White;
            btnMenuIniciarSesion.Location = new Point(0, 0);
            btnMenuIniciarSesion.Name = "btnMenuIniciarSesion";
            btnMenuIniciarSesion.Size = new Size(250, 45);
            btnMenuIniciarSesion.TabIndex = 1;
            btnMenuIniciarSesion.Text = "Iniciar Sesión";
            btnMenuIniciarSesion.UseVisualStyleBackColor = false;
            btnMenuIniciarSesion.Click += btnIniciarSesion_Click;
            // 
            // pnlCabecera
            // 
            pnlCabecera.Controls.Add(pnlSubNutricion);
            pnlCabecera.Controls.Add(pnlSubEntrenamiento);
            pnlCabecera.Controls.Add(pnlSubRecepcion);
            pnlCabecera.Controls.Add(panelIdioma);
            pnlCabecera.Dock = DockStyle.Top;
            pnlCabecera.Location = new Point(250, 0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(627, 60);
            pnlCabecera.TabIndex = 8;
            // 
            // pnlSubNutricion
            // 
            pnlSubNutricion.Dock = DockStyle.Fill;
            pnlSubNutricion.Location = new Point(0, 0);
            pnlSubNutricion.Name = "pnlSubNutricion";
            pnlSubNutricion.Size = new Size(277, 60);
            pnlSubNutricion.TabIndex = 10;
            // 
            // pnlSubEntrenamiento
            // 
            pnlSubEntrenamiento.Controls.Add(btnSubGestionRutinas);
            pnlSubEntrenamiento.Controls.Add(btnSubAsignarRutina);
            pnlSubEntrenamiento.Dock = DockStyle.Fill;
            pnlSubEntrenamiento.Location = new Point(0, 0);
            pnlSubEntrenamiento.Name = "pnlSubEntrenamiento";
            pnlSubEntrenamiento.Size = new Size(277, 60);
            pnlSubEntrenamiento.TabIndex = 9;
            // 
            // btnSubGestionRutinas
            // 
            btnSubGestionRutinas.BackColor = Color.FromArgb(37, 99, 235);
            btnSubGestionRutinas.Dock = DockStyle.Left;
            btnSubGestionRutinas.FlatAppearance.BorderSize = 0;
            btnSubGestionRutinas.FlatStyle = FlatStyle.Flat;
            btnSubGestionRutinas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSubGestionRutinas.ForeColor = Color.White;
            btnSubGestionRutinas.Location = new Point(180, 0);
            btnSubGestionRutinas.Name = "btnSubGestionRutinas";
            btnSubGestionRutinas.Size = new Size(180, 60);
            btnSubGestionRutinas.TabIndex = 1;
            btnSubGestionRutinas.Text = "button2";
            btnSubGestionRutinas.UseVisualStyleBackColor = false;
            btnSubGestionRutinas.Click += btnSubGestionRutinas_Click;
            // 
            // btnSubAsignarRutina
            // 
            btnSubAsignarRutina.BackColor = Color.FromArgb(37, 99, 235);
            btnSubAsignarRutina.Dock = DockStyle.Left;
            btnSubAsignarRutina.FlatAppearance.BorderSize = 0;
            btnSubAsignarRutina.FlatStyle = FlatStyle.Flat;
            btnSubAsignarRutina.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSubAsignarRutina.ForeColor = Color.White;
            btnSubAsignarRutina.Location = new Point(0, 0);
            btnSubAsignarRutina.Name = "btnSubAsignarRutina";
            btnSubAsignarRutina.Size = new Size(180, 60);
            btnSubAsignarRutina.TabIndex = 0;
            btnSubAsignarRutina.Text = "button1";
            btnSubAsignarRutina.UseVisualStyleBackColor = false;
            btnSubAsignarRutina.Click += btnSubAsignarRutina_Click;
            // 
            // pnlSubRecepcion
            // 
            pnlSubRecepcion.Controls.Add(btnSubAgendarTurno);
            pnlSubRecepcion.Controls.Add(btnSubConsultarComprobantes);
            pnlSubRecepcion.Controls.Add(btnSubVenderPlan);
            pnlSubRecepcion.Dock = DockStyle.Fill;
            pnlSubRecepcion.Location = new Point(0, 0);
            pnlSubRecepcion.Name = "pnlSubRecepcion";
            pnlSubRecepcion.Size = new Size(277, 60);
            pnlSubRecepcion.TabIndex = 8;
            // 
            // btnSubAgendarTurno
            // 
            btnSubAgendarTurno.BackColor = Color.FromArgb(37, 99, 235);
            btnSubAgendarTurno.Dock = DockStyle.Left;
            btnSubAgendarTurno.FlatAppearance.BorderSize = 0;
            btnSubAgendarTurno.FlatStyle = FlatStyle.Flat;
            btnSubAgendarTurno.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSubAgendarTurno.ForeColor = Color.White;
            btnSubAgendarTurno.Location = new Point(360, 0);
            btnSubAgendarTurno.Name = "btnSubAgendarTurno";
            btnSubAgendarTurno.Size = new Size(180, 60);
            btnSubAgendarTurno.TabIndex = 2;
            btnSubAgendarTurno.Text = "button3";
            btnSubAgendarTurno.UseVisualStyleBackColor = false;
            btnSubAgendarTurno.Click += btnSubAgendarTurno_Click;
            // 
            // btnSubConsultarComprobantes
            // 
            btnSubConsultarComprobantes.BackColor = Color.FromArgb(37, 99, 235);
            btnSubConsultarComprobantes.Dock = DockStyle.Left;
            btnSubConsultarComprobantes.FlatAppearance.BorderSize = 0;
            btnSubConsultarComprobantes.FlatStyle = FlatStyle.Flat;
            btnSubConsultarComprobantes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSubConsultarComprobantes.ForeColor = Color.White;
            btnSubConsultarComprobantes.Location = new Point(180, 0);
            btnSubConsultarComprobantes.Name = "btnSubConsultarComprobantes";
            btnSubConsultarComprobantes.Size = new Size(180, 60);
            btnSubConsultarComprobantes.TabIndex = 1;
            btnSubConsultarComprobantes.Text = "button2";
            btnSubConsultarComprobantes.UseVisualStyleBackColor = false;
            btnSubConsultarComprobantes.Click += btnSubConsultarComprobantes_Click;
            // 
            // btnSubVenderPlan
            // 
            btnSubVenderPlan.BackColor = Color.FromArgb(37, 99, 235);
            btnSubVenderPlan.Dock = DockStyle.Left;
            btnSubVenderPlan.FlatAppearance.BorderSize = 0;
            btnSubVenderPlan.FlatStyle = FlatStyle.Flat;
            btnSubVenderPlan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSubVenderPlan.ForeColor = Color.White;
            btnSubVenderPlan.Location = new Point(0, 0);
            btnSubVenderPlan.Name = "btnSubVenderPlan";
            btnSubVenderPlan.Size = new Size(180, 60);
            btnSubVenderPlan.TabIndex = 0;
            btnSubVenderPlan.Text = "button1";
            btnSubVenderPlan.UseVisualStyleBackColor = false;
            btnSubVenderPlan.Click += btnSubVenderPlan_Click;
            // 
            // panelIdioma
            // 
            panelIdioma.BackColor = Color.Transparent;
            panelIdioma.BorderStyle = BorderStyle.FixedSingle;
            panelIdioma.Controls.Add(cmbIdiomas);
            panelIdioma.Controls.Add(btnNuevoIdioma);
            panelIdioma.Dock = DockStyle.Right;
            panelIdioma.Location = new Point(277, 0);
            panelIdioma.Name = "panelIdioma";
            panelIdioma.Padding = new Padding(10);
            panelIdioma.Size = new Size(350, 60);
            panelIdioma.TabIndex = 7;
            // 
            // cmbIdiomas
            // 
            cmbIdiomas.BackColor = Color.White;
            cmbIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdiomas.FlatStyle = FlatStyle.Flat;
            cmbIdiomas.Font = new Font("Segoe UI", 9F);
            cmbIdiomas.ForeColor = Color.FromArgb(31, 41, 55);
            cmbIdiomas.FormattingEnabled = true;
            cmbIdiomas.Location = new Point(143, 16);
            cmbIdiomas.Name = "cmbIdiomas";
            cmbIdiomas.Size = new Size(194, 23);
            cmbIdiomas.TabIndex = 2;
            // 
            // btnNuevoIdioma
            // 
            btnNuevoIdioma.BackColor = Color.FromArgb(37, 99, 235);
            btnNuevoIdioma.FlatAppearance.BorderSize = 0;
            btnNuevoIdioma.FlatStyle = FlatStyle.Flat;
            btnNuevoIdioma.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevoIdioma.ForeColor = Color.White;
            btnNuevoIdioma.Location = new Point(11, 10);
            btnNuevoIdioma.Name = "btnNuevoIdioma";
            btnNuevoIdioma.Size = new Size(124, 34);
            btnNuevoIdioma.TabIndex = 4;
            btnNuevoIdioma.Text = "Gestionar Idiomas";
            btnNuevoIdioma.UseVisualStyleBackColor = false;
            btnNuevoIdioma.Click += btnNuevoIdioma_Click;
            // 
            // pnlContenedor
            // 
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.Location = new Point(250, 60);
            pnlContenedor.Name = "pnlContenedor";
            pnlContenedor.Size = new Size(627, 399);
            pnlContenedor.TabIndex = 8;
            // 
            // btnMenuInicio
            // 
            btnMenuInicio.BackColor = Color.FromArgb(37, 99, 235);
            btnMenuInicio.Dock = DockStyle.Top;
            btnMenuInicio.FlatAppearance.BorderSize = 0;
            btnMenuInicio.FlatStyle = FlatStyle.Flat;
            btnMenuInicio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMenuInicio.ForeColor = Color.White;
            btnMenuInicio.Location = new Point(0, 90);
            btnMenuInicio.Name = "btnMenuInicio";
            btnMenuInicio.Size = new Size(250, 45);
            btnMenuInicio.TabIndex = 8;
            btnMenuInicio.Text = "Menú";
            btnMenuInicio.UseVisualStyleBackColor = false;
            btnMenuInicio.Click += btnMenuInicio_Click;
            // 
            // frmMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            ClientSize = new Size(877, 459);
            Controls.Add(pnlContenedor);
            Controls.Add(pnlCabecera);
            Controls.Add(pnlLateral);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(31, 41, 55);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmMenu";
            Text = "Menu";
            FormClosing += frmMenu_FormClosing;
            Load += frmMenu_Load;
            pnlLateral.ResumeLayout(false);
            pnlCabecera.ResumeLayout(false);
            pnlSubEntrenamiento.ResumeLayout(false);
            pnlSubRecepcion.ResumeLayout(false);
            panelIdioma.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlLateral;
        private Panel pnlCabecera;
        private Panel pnlContenedor;
        private Panel panelIdioma;
        private ComboBox cmbIdiomas;
        private Button btnNuevoIdioma;
        private Button btnMenuIniciarSesion;
        private Button btnMenuSalir;
        private Button btnMenuNutricion;
        private Button btnMenuEntrenamiento;
        private Button btnMenuRecepcion;
        private Button btnMenuRegistrarse;
        private Panel pnlSubRecepcion;
        private Button btnSubAgendarTurno;
        private Button btnSubConsultarComprobantes;
        private Button btnSubVenderPlan;
        private Panel pnlSubEntrenamiento;
        private Button btnSubGestionRutinas;
        private Button btnSubAsignarRutina;
        private Panel pnlSubNutricion;
        private Button btnMenuCerrarSesion;
        private Button btnMenuInicio;
    }
}