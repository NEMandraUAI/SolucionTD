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
            menuStrip1 = new MenuStrip();
            iniciarSesionToolStripMenuItem = new ToolStripMenuItem();
            iniciarSesiónToolStripMenuItem = new ToolStripMenuItem();
            registrarseToolStripMenuItem = new ToolStripMenuItem();
            recepcionToolStripMenuItem = new ToolStripMenuItem();
            venderPlanToolStripMenuItem = new ToolStripMenuItem();
            consultarComprobantesToolStripMenuItem = new ToolStripMenuItem();
            agendarTurnoNutricionalToolStripMenuItem = new ToolStripMenuItem();
            entrenamientoToolStripMenuItem = new ToolStripMenuItem();
            asignarRutinaToolStripMenuItem = new ToolStripMenuItem();
            gestionRutinasToolStripMenuItem = new ToolStripMenuItem();
            nutriciónToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            cmbIdiomas = new ComboBox();
            btnNuevoIdioma = new Button();
            panelIdioma = new Panel();
            menuStrip1.SuspendLayout();
            panelIdioma.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(31, 41, 55);
            menuStrip1.Font = new Font("Segoe UI", 10F);
            menuStrip1.ForeColor = Color.White;
            menuStrip1.Items.AddRange(new ToolStripItem[] { iniciarSesionToolStripMenuItem, recepcionToolStripMenuItem, entrenamientoToolStripMenuItem, nutriciónToolStripMenuItem, salirToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(12, 5, 12, 5);
            menuStrip1.Size = new Size(877, 43);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // iniciarSesionToolStripMenuItem
            // 
            iniciarSesionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { iniciarSesiónToolStripMenuItem, registrarseToolStripMenuItem });
            iniciarSesionToolStripMenuItem.ForeColor = Color.White;
            iniciarSesionToolStripMenuItem.Name = "iniciarSesionToolStripMenuItem";
            iniciarSesionToolStripMenuItem.Padding = new Padding(10, 5, 10, 5);
            iniciarSesionToolStripMenuItem.Size = new Size(72, 33);
            iniciarSesionToolStripMenuItem.Text = "Sesión";
            // 
            // iniciarSesiónToolStripMenuItem
            // 
            iniciarSesiónToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            iniciarSesiónToolStripMenuItem.Name = "iniciarSesiónToolStripMenuItem";
            iniciarSesiónToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            iniciarSesiónToolStripMenuItem.Size = new Size(177, 34);
            iniciarSesiónToolStripMenuItem.Text = "Iniciar Sesión";
            iniciarSesiónToolStripMenuItem.Click += iniciarSesiónToolStripMenuItem_Click;
            // 
            // registrarseToolStripMenuItem
            // 
            registrarseToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            registrarseToolStripMenuItem.Name = "registrarseToolStripMenuItem";
            registrarseToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            registrarseToolStripMenuItem.Size = new Size(177, 34);
            registrarseToolStripMenuItem.Text = "Registrarse";
            registrarseToolStripMenuItem.Click += registrarseToolStripMenuItem_Click;
            // 
            // recepcionToolStripMenuItem
            // 
            recepcionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { venderPlanToolStripMenuItem, consultarComprobantesToolStripMenuItem, agendarTurnoNutricionalToolStripMenuItem });
            recepcionToolStripMenuItem.ForeColor = Color.White;
            recepcionToolStripMenuItem.Name = "recepcionToolStripMenuItem";
            recepcionToolStripMenuItem.Padding = new Padding(10, 5, 10, 5);
            recepcionToolStripMenuItem.Size = new Size(94, 33);
            recepcionToolStripMenuItem.Text = "Recepción";
            recepcionToolStripMenuItem.Visible = false;
            // 
            // venderPlanToolStripMenuItem
            // 
            venderPlanToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            venderPlanToolStripMenuItem.Name = "venderPlanToolStripMenuItem";
            venderPlanToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            venderPlanToolStripMenuItem.Size = new Size(260, 34);
            venderPlanToolStripMenuItem.Text = "Vender Plan";
            venderPlanToolStripMenuItem.Click += venderPlanToolStripMenuItem_Click;
            // 
            // consultarComprobantesToolStripMenuItem
            // 
            consultarComprobantesToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            consultarComprobantesToolStripMenuItem.Name = "consultarComprobantesToolStripMenuItem";
            consultarComprobantesToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            consultarComprobantesToolStripMenuItem.Size = new Size(260, 34);
            consultarComprobantesToolStripMenuItem.Text = "Consultar Comprobantes";
            consultarComprobantesToolStripMenuItem.Click += consultarComprobantesToolStripMenuItem_Click;
            // 
            // agendarTurnoNutricionalToolStripMenuItem
            // 
            agendarTurnoNutricionalToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            agendarTurnoNutricionalToolStripMenuItem.Name = "agendarTurnoNutricionalToolStripMenuItem";
            agendarTurnoNutricionalToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            agendarTurnoNutricionalToolStripMenuItem.Size = new Size(260, 34);
            agendarTurnoNutricionalToolStripMenuItem.Text = "Agendar Turno Nutricional";
            agendarTurnoNutricionalToolStripMenuItem.Click += agendarTurnoNutricionalToolStripMenuItem_Click;
            // 
            // entrenamientoToolStripMenuItem
            // 
            entrenamientoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { asignarRutinaToolStripMenuItem, gestionRutinasToolStripMenuItem });
            entrenamientoToolStripMenuItem.ForeColor = Color.White;
            entrenamientoToolStripMenuItem.Name = "entrenamientoToolStripMenuItem";
            entrenamientoToolStripMenuItem.Padding = new Padding(10, 5, 10, 5);
            entrenamientoToolStripMenuItem.Size = new Size(123, 33);
            entrenamientoToolStripMenuItem.Text = "Entrenamiento";
            entrenamientoToolStripMenuItem.Visible = false;
            // 
            // asignarRutinaToolStripMenuItem
            // 
            asignarRutinaToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            asignarRutinaToolStripMenuItem.Name = "asignarRutinaToolStripMenuItem";
            asignarRutinaToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            asignarRutinaToolStripMenuItem.Size = new Size(213, 34);
            asignarRutinaToolStripMenuItem.Text = "Asignar Rutina";
            asignarRutinaToolStripMenuItem.Click += asignarRutinaToolStripMenuItem_Click;
            // 
            // gestionRutinasToolStripMenuItem
            // 
            gestionRutinasToolStripMenuItem.ForeColor = Color.FromArgb(31, 41, 55);
            gestionRutinasToolStripMenuItem.Name = "gestionRutinasToolStripMenuItem";
            gestionRutinasToolStripMenuItem.Padding = new Padding(10, 6, 10, 6);
            gestionRutinasToolStripMenuItem.Size = new Size(213, 34);
            gestionRutinasToolStripMenuItem.Text = "Gestión de Rutinas";
            gestionRutinasToolStripMenuItem.Click += gestionRutinasToolStripMenuItem_Click;
            // 
            // nutriciónToolStripMenuItem
            // 
            nutriciónToolStripMenuItem.ForeColor = Color.White;
            nutriciónToolStripMenuItem.Name = "nutriciónToolStripMenuItem";
            nutriciónToolStripMenuItem.Padding = new Padding(10, 5, 10, 5);
            nutriciónToolStripMenuItem.Size = new Size(89, 33);
            nutriciónToolStripMenuItem.Text = "Nutrición";
            nutriciónToolStripMenuItem.Visible = false;
            nutriciónToolStripMenuItem.Click += nutriciónToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.ForeColor = Color.White;
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Padding = new Padding(10, 5, 10, 5);
            salirToolStripMenuItem.Size = new Size(58, 33);
            salirToolStripMenuItem.Text = "Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // cmbIdiomas
            // 
            cmbIdiomas.BackColor = Color.White;
            cmbIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdiomas.FlatStyle = FlatStyle.Flat;
            cmbIdiomas.Font = new Font("Segoe UI", 9F);
            cmbIdiomas.ForeColor = Color.FromArgb(31, 41, 55);
            cmbIdiomas.FormattingEnabled = true;
            cmbIdiomas.Location = new Point(12, 12);
            cmbIdiomas.Name = "cmbIdiomas";
            cmbIdiomas.Size = new Size(194, 23);
            cmbIdiomas.TabIndex = 2;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
            // 
            // btnNuevoIdioma
            // 
            btnNuevoIdioma.BackColor = Color.FromArgb(37, 99, 235);
            btnNuevoIdioma.FlatAppearance.BorderSize = 0;
            btnNuevoIdioma.FlatStyle = FlatStyle.Flat;
            btnNuevoIdioma.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevoIdioma.ForeColor = Color.White;
            btnNuevoIdioma.Location = new Point(12, 52);
            btnNuevoIdioma.Name = "btnNuevoIdioma";
            btnNuevoIdioma.Size = new Size(194, 34);
            btnNuevoIdioma.TabIndex = 4;
            btnNuevoIdioma.Text = "Gestionar Idiomas";
            btnNuevoIdioma.UseVisualStyleBackColor = false;
            // 
            // panelIdioma
            // 
            panelIdioma.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelIdioma.BackColor = Color.White;
            panelIdioma.BorderStyle = BorderStyle.FixedSingle;
            panelIdioma.Controls.Add(cmbIdiomas);
            panelIdioma.Controls.Add(btnNuevoIdioma);
            panelIdioma.Location = new Point(645, 48);
            panelIdioma.Name = "panelIdioma";
            panelIdioma.Padding = new Padding(10);
            panelIdioma.Size = new Size(220, 100);
            panelIdioma.TabIndex = 6;
            // 
            // frmMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(877, 459);
            Controls.Add(panelIdioma);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(31, 41, 55);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "frmMenu";
            Text = "Menu";
            FormClosing += frmMenu_FormClosing;
            Load += frmMenu_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panelIdioma.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem iniciarSesionToolStripMenuItem;
        private ToolStripMenuItem iniciarSesiónToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ToolStripMenuItem registrarseToolStripMenuItem;
        private ComboBox cmbIdiomas;
        private Button btnNuevoIdioma;
        private Panel panelIdioma;
        private ToolStripMenuItem recepcionToolStripMenuItem;
        private ToolStripMenuItem venderPlanToolStripMenuItem;
        private ToolStripMenuItem entrenamientoToolStripMenuItem;
        private ToolStripMenuItem asignarRutinaToolStripMenuItem;
        private ToolStripMenuItem consultarComprobantesToolStripMenuItem;
        private ToolStripMenuItem gestionRutinasToolStripMenuItem;
        private ToolStripMenuItem nutriciónToolStripMenuItem;
        private ToolStripMenuItem agendarTurnoNutricionalToolStripMenuItem;
    }
}