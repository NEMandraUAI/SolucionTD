namespace GUI
{
    partial class frmGestionRoles
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
            tvRoles = new TreeView();
            cmbUsuarios = new ComboBox();
            tvPermisosUsuario = new TreeView();
            btnCrearFamilia = new Button();
            btnEliminarFamilia = new Button();
            btnAsignarRolUsuario = new Button();
            btnQuitarRolUsuario = new Button();
            btnAsignarPermisoARol = new Button();
            btnCrearRolAnidado = new Button();
            tvPermisosDisponibles = new TreeView();
            lblJerarquia = new Label();
            lblUsuario = new Label();
            lblPermisosUsuario = new Label();
            lblCatalogo = new Label();
            SuspendLayout();
            // 
            // tvRoles
            // 
            tvRoles.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            tvRoles.BackColor = Color.White;
            tvRoles.BorderStyle = BorderStyle.FixedSingle;
            tvRoles.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tvRoles.ForeColor = Color.FromArgb(31, 41, 55);
            tvRoles.HideSelection = false;
            tvRoles.Location = new Point(28, 54);
            tvRoles.Name = "tvRoles";
            tvRoles.Size = new Size(370, 300);
            tvRoles.TabIndex = 0;
            // 
            // cmbUsuarios
            // 
            cmbUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cmbUsuarios.BackColor = Color.White;
            cmbUsuarios.FlatStyle = FlatStyle.Flat;
            cmbUsuarios.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbUsuarios.ForeColor = Color.FromArgb(31, 41, 55);
            cmbUsuarios.FormattingEnabled = true;
            cmbUsuarios.Location = new Point(426, 54);
            cmbUsuarios.Name = "cmbUsuarios";
            cmbUsuarios.Size = new Size(300, 28);
            cmbUsuarios.TabIndex = 1;
            cmbUsuarios.SelectedIndexChanged += cmbUsuarios_SelectedIndexChanged;
            // 
            // tvPermisosUsuario
            // 
            tvPermisosUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvPermisosUsuario.BackColor = Color.White;
            tvPermisosUsuario.BorderStyle = BorderStyle.FixedSingle;
            tvPermisosUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tvPermisosUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            tvPermisosUsuario.Location = new Point(754, 54);
            tvPermisosUsuario.Name = "tvPermisosUsuario";
            tvPermisosUsuario.Size = new Size(618, 810);
            tvPermisosUsuario.TabIndex = 2;
            // 
            // btnCrearFamilia
            // 
            btnCrearFamilia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnCrearFamilia.BackColor = Color.FromArgb(37, 99, 235);
            btnCrearFamilia.FlatAppearance.BorderSize = 0;
            btnCrearFamilia.FlatStyle = FlatStyle.Flat;
            btnCrearFamilia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCrearFamilia.ForeColor = Color.White;
            btnCrearFamilia.Location = new Point(426, 480);
            btnCrearFamilia.Name = "btnCrearFamilia";
            btnCrearFamilia.Size = new Size(300, 52);
            btnCrearFamilia.TabIndex = 3;
            btnCrearFamilia.Text = "Crear Rol";
            btnCrearFamilia.UseVisualStyleBackColor = false;
            btnCrearFamilia.Click += btnCrearFamilia_Click;
            // 
            // btnEliminarFamilia
            // 
            btnEliminarFamilia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnEliminarFamilia.BackColor = Color.FromArgb(185, 28, 28);
            btnEliminarFamilia.FlatAppearance.BorderSize = 0;
            btnEliminarFamilia.FlatStyle = FlatStyle.Flat;
            btnEliminarFamilia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnEliminarFamilia.ForeColor = Color.White;
            btnEliminarFamilia.Location = new Point(426, 612);
            btnEliminarFamilia.Name = "btnEliminarFamilia";
            btnEliminarFamilia.Size = new Size(300, 52);
            btnEliminarFamilia.TabIndex = 5;
            btnEliminarFamilia.Text = "Eliminar Rol";
            btnEliminarFamilia.UseVisualStyleBackColor = false;
            btnEliminarFamilia.Click += btnEliminarFamilia_Click;
            // 
            // btnAsignarRolUsuario
            // 
            btnAsignarRolUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAsignarRolUsuario.BackColor = Color.FromArgb(37, 99, 235);
            btnAsignarRolUsuario.FlatAppearance.BorderSize = 0;
            btnAsignarRolUsuario.FlatStyle = FlatStyle.Flat;
            btnAsignarRolUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAsignarRolUsuario.ForeColor = Color.White;
            btnAsignarRolUsuario.Location = new Point(426, 100);
            btnAsignarRolUsuario.Name = "btnAsignarRolUsuario";
            btnAsignarRolUsuario.Size = new Size(300, 52);
            btnAsignarRolUsuario.TabIndex = 6;
            btnAsignarRolUsuario.Text = "Asignar Rol/Permiso a Usuario";
            btnAsignarRolUsuario.UseVisualStyleBackColor = false;
            btnAsignarRolUsuario.Click += btnAsignarRolUsuario_Click;
            // 
            // btnQuitarRolUsuario
            // 
            btnQuitarRolUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnQuitarRolUsuario.BackColor = Color.FromArgb(71, 85, 105);
            btnQuitarRolUsuario.FlatAppearance.BorderSize = 0;
            btnQuitarRolUsuario.FlatStyle = FlatStyle.Flat;
            btnQuitarRolUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnQuitarRolUsuario.ForeColor = Color.White;
            btnQuitarRolUsuario.Location = new Point(426, 160);
            btnQuitarRolUsuario.Name = "btnQuitarRolUsuario";
            btnQuitarRolUsuario.Size = new Size(300, 52);
            btnQuitarRolUsuario.TabIndex = 7;
            btnQuitarRolUsuario.Text = "Quitar Rol/Permiso a Usuario";
            btnQuitarRolUsuario.UseVisualStyleBackColor = false;
            btnQuitarRolUsuario.Click += btnQuitarRolUsuario_Click;
            // 
            // btnAsignarPermisoARol
            // 
            btnAsignarPermisoARol.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAsignarPermisoARol.BackColor = Color.FromArgb(37, 99, 235);
            btnAsignarPermisoARol.FlatAppearance.BorderSize = 0;
            btnAsignarPermisoARol.FlatStyle = FlatStyle.Flat;
            btnAsignarPermisoARol.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAsignarPermisoARol.ForeColor = Color.White;
            btnAsignarPermisoARol.Location = new Point(426, 414);
            btnAsignarPermisoARol.Name = "btnAsignarPermisoARol";
            btnAsignarPermisoARol.Size = new Size(300, 52);
            btnAsignarPermisoARol.TabIndex = 9;
            btnAsignarPermisoARol.Text = "Asignar Rol/Permiso a Rol";
            btnAsignarPermisoARol.UseVisualStyleBackColor = false;
            btnAsignarPermisoARol.Click += btnAsignarPatenteARol_Click;
            // 
            // btnCrearRolAnidado
            // 
            btnCrearRolAnidado.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnCrearRolAnidado.BackColor = Color.FromArgb(37, 99, 235);
            btnCrearRolAnidado.FlatAppearance.BorderSize = 0;
            btnCrearRolAnidado.FlatStyle = FlatStyle.Flat;
            btnCrearRolAnidado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCrearRolAnidado.ForeColor = Color.White;
            btnCrearRolAnidado.Location = new Point(426, 546);
            btnCrearRolAnidado.Name = "btnCrearRolAnidado";
            btnCrearRolAnidado.Size = new Size(300, 52);
            btnCrearRolAnidado.TabIndex = 10;
            btnCrearRolAnidado.Text = "Crear Rol Anidado";
            btnCrearRolAnidado.UseVisualStyleBackColor = false;
            btnCrearRolAnidado.Click += btnCrearRolAnidado_Click;
            // 
            // tvPermisosDisponibles
            // 
            tvPermisosDisponibles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tvPermisosDisponibles.BackColor = Color.White;
            tvPermisosDisponibles.BorderStyle = BorderStyle.FixedSingle;
            tvPermisosDisponibles.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tvPermisosDisponibles.ForeColor = Color.FromArgb(31, 41, 55);
            tvPermisosDisponibles.HideSelection = false;
            tvPermisosDisponibles.Location = new Point(28, 414);
            tvPermisosDisponibles.Name = "tvPermisosDisponibles";
            tvPermisosDisponibles.Size = new Size(370, 450);
            tvPermisosDisponibles.TabIndex = 11;
            // 
            // lblJerarquia
            // 
            lblJerarquia.AutoSize = true;
            lblJerarquia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblJerarquia.ForeColor = Color.FromArgb(31, 41, 55);
            lblJerarquia.Location = new Point(28, 28);
            lblJerarquia.Name = "lblJerarquia";
            lblJerarquia.Size = new Size(278, 15);
            lblJerarquia.TabIndex = 12;
            lblJerarquia.Text = "Estructura Jerárquica de Roles (Árbol de Familias)";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            lblUsuario.Location = new Point(426, 28);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(116, 15);
            lblUsuario.TabIndex = 13;
            lblUsuario.Text = "Seleccionar Usuario";
            // 
            // lblPermisosUsuario
            // 
            lblPermisosUsuario.AutoSize = true;
            lblPermisosUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPermisosUsuario.ForeColor = Color.FromArgb(31, 41, 55);
            lblPermisosUsuario.Location = new Point(754, 28);
            lblPermisosUsuario.Name = "lblPermisosUsuario";
            lblPermisosUsuario.Size = new Size(252, 15);
            lblPermisosUsuario.TabIndex = 14;
            lblPermisosUsuario.Text = "Permisos Efectivos del Usuario Seleccionado";
            // 
            // lblCatalogo
            // 
            lblCatalogo.AutoSize = true;
            lblCatalogo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCatalogo.ForeColor = Color.FromArgb(31, 41, 55);
            lblCatalogo.Location = new Point(28, 388);
            lblCatalogo.Name = "lblCatalogo";
            lblCatalogo.Size = new Size(280, 15);
            lblCatalogo.TabIndex = 15;
            lblCatalogo.Text = "Catálogo General de Permisos y Roles Disponibles";
            // 
            // frmGestionRoles
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 249);
            ClientSize = new Size(1400, 900);
            Controls.Add(lblCatalogo);
            Controls.Add(lblPermisosUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(lblJerarquia);
            Controls.Add(tvPermisosDisponibles);
            Controls.Add(btnCrearRolAnidado);
            Controls.Add(btnAsignarPermisoARol);
            Controls.Add(btnQuitarRolUsuario);
            Controls.Add(btnAsignarRolUsuario);
            Controls.Add(btnEliminarFamilia);
            Controls.Add(btnCrearFamilia);
            Controls.Add(tvPermisosUsuario);
            Controls.Add(cmbUsuarios);
            Controls.Add(tvRoles);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(31, 41, 55);
            Name = "frmGestionRoles";
            Text = "Gestion de Roles";
            Load += frmGestionRoles_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView tvRoles;
        private ComboBox cmbUsuarios;
        private TreeView tvPermisosUsuario;
        private Button btnCrearFamilia;
        private Button btnEliminarFamilia;
        private Button btnAsignarRolUsuario;
        private Button btnQuitarRolUsuario;
        private ListBox lstPatentes;
        private Button btnAsignarPermisoARol;
        private Button btnCrearRolAnidado;
        private TreeView tvPermisosDisponibles;
        private Label lblJerarquia;
        private Label lblUsuario;
        private Label lblPermisosUsuario;
        private Label lblCatalogo;
    }
}