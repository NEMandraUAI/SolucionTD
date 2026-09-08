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
            entrenamientoToolStripMenuItem = new ToolStripMenuItem();
            asignarRutinaToolStripMenuItem = new ToolStripMenuItem();
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { iniciarSesionToolStripMenuItem, recepcionToolStripMenuItem, entrenamientoToolStripMenuItem, salirToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // iniciarSesionToolStripMenuItem
            // 
            iniciarSesionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { iniciarSesiónToolStripMenuItem, registrarseToolStripMenuItem });
            iniciarSesionToolStripMenuItem.Name = "iniciarSesionToolStripMenuItem";
            iniciarSesionToolStripMenuItem.Size = new Size(53, 20);
            iniciarSesionToolStripMenuItem.Text = "Sesión";
            // 
            // iniciarSesiónToolStripMenuItem
            // 
            iniciarSesiónToolStripMenuItem.Name = "iniciarSesiónToolStripMenuItem";
            iniciarSesiónToolStripMenuItem.Size = new Size(143, 22);
            iniciarSesiónToolStripMenuItem.Text = "Iniciar Sesión";
            iniciarSesiónToolStripMenuItem.Click += iniciarSesiónToolStripMenuItem_Click;
            // 
            // registrarseToolStripMenuItem
            // 
            registrarseToolStripMenuItem.Name = "registrarseToolStripMenuItem";
            registrarseToolStripMenuItem.Size = new Size(143, 22);
            registrarseToolStripMenuItem.Text = "Registrarse";
            registrarseToolStripMenuItem.Click += registrarseToolStripMenuItem_Click;
            // 
            // recepcionToolStripMenuItem
            // 
            recepcionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { venderPlanToolStripMenuItem });
            recepcionToolStripMenuItem.Name = "recepcionToolStripMenuItem";
            recepcionToolStripMenuItem.Size = new Size(74, 20);
            recepcionToolStripMenuItem.Text = "Recepción";
            recepcionToolStripMenuItem.Visible = false;
            // 
            // venderPlanToolStripMenuItem
            // 
            venderPlanToolStripMenuItem.Name = "venderPlanToolStripMenuItem";
            venderPlanToolStripMenuItem.Size = new Size(180, 22);
            venderPlanToolStripMenuItem.Text = "Vender Plan";
            venderPlanToolStripMenuItem.Click += venderPlanToolStripMenuItem_Click;
            // 
            // entrenamientoToolStripMenuItem
            // 
            entrenamientoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { asignarRutinaToolStripMenuItem });
            entrenamientoToolStripMenuItem.Name = "entrenamientoToolStripMenuItem";
            entrenamientoToolStripMenuItem.Size = new Size(97, 20);
            entrenamientoToolStripMenuItem.Text = "Entrenamiento";
            entrenamientoToolStripMenuItem.Visible = false;
            // 
            // asignarRutinaToolStripMenuItem
            // 
            asignarRutinaToolStripMenuItem.Name = "asignarRutinaToolStripMenuItem";
            asignarRutinaToolStripMenuItem.Size = new Size(180, 22);
            asignarRutinaToolStripMenuItem.Text = "Asignar Rutina";
            asignarRutinaToolStripMenuItem.Click += asignarRutinaToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(41, 20);
            salirToolStripMenuItem.Text = "Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // cmbIdiomas
            // 
            cmbIdiomas.Anchor = AnchorStyles.None;
            cmbIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIdiomas.FormattingEnabled = true;
            cmbIdiomas.Location = new Point(3, 3);
            cmbIdiomas.Name = "cmbIdiomas";
            cmbIdiomas.Size = new Size(170, 23);
            cmbIdiomas.TabIndex = 2;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
            // 
            // btnNuevoIdioma
            // 
            btnNuevoIdioma.Anchor = AnchorStyles.None;
            btnNuevoIdioma.Location = new Point(3, 32);
            btnNuevoIdioma.Name = "btnNuevoIdioma";
            btnNuevoIdioma.Size = new Size(170, 42);
            btnNuevoIdioma.TabIndex = 4;
            btnNuevoIdioma.Text = "Gestionar Idiomas";
            btnNuevoIdioma.UseVisualStyleBackColor = true;
            // 
            // panelIdioma
            // 
            panelIdioma.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelIdioma.Controls.Add(cmbIdiomas);
            panelIdioma.Controls.Add(btnNuevoIdioma);
            panelIdioma.Location = new Point(612, 27);
            panelIdioma.Name = "panelIdioma";
            panelIdioma.Size = new Size(176, 79);
            panelIdioma.TabIndex = 6;
            // 
            // frmMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelIdioma);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "frmMenu";
            Text = "Menu";
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
    }
}