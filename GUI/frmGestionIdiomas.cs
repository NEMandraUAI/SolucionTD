using BE;
using BLL;
using Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmGestionIdiomas : Form, IObserverIdioma
    {
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private Label lblSeleccion;
        private ComboBox cmbIdiomasAdmin;
        private Button btnNuevo;
        private Button btnEliminar;
        private Button btnGuardar;
        private DataGridView dgvTraducciones;
        private DataGridViewTextBoxColumn colFormulario;
        private DataGridViewTextBoxColumn colNombreControl;
        private DataGridViewTextBoxColumn colTexto;
        private Label lblFiltroFormulario;
        private ComboBox cmbFiltroFormulario;
        private Label lblFiltroTexto;
        private TextBox txtFiltroTexto;
        private Button btnLimpiarFiltros;
        private List<TraduccionBE> _traduccionesActuales = new List<TraduccionBE>();
        private IdiomaBLL idiomaBLL = new IdiomaBLL();
        public event EventHandler? IdiomasActualizados;

        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }

        public frmGestionIdiomas()
        {
            InitializeComponent();
            ConfigurarUI();
            GestorIdioma.Instancia.Suscribir(this);
            this.Load += frmGestionIdiomas_Load;
            this.FormClosing += frmGestionIdiomas_FormClosing;
        }

        private void ConfigurarUI()
        {
            this.Name = "frmGestionIdiomas";
            this.Size = new Size(720, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            lblSeleccion = new Label { Location = new Point(20, 25), AutoSize = true, Name = "lblSeleccion", Text = T("lblSeleccion", "Seleccionar Idioma:") };
            cmbIdiomasAdmin = new ComboBox { Location = new Point(150, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Name = "cmbIdiomasAdmin" };
            cmbIdiomasAdmin.SelectedIndexChanged += CmbIdiomasAdmin_SelectedIndexChanged;
            btnNuevo = new Button { Location = new Point(370, 18), Width = 110, Name = "btnNuevo", Text = T("btnNuevo", "Nuevo Idioma") };
            btnNuevo.Click += BtnNuevo_Click;
            btnEliminar = new Button { Location = new Point(490, 18), Width = 110, Name = "btnEliminar", Text = T("btnEliminar", "Eliminar Idioma") };
            btnEliminar.Click += BtnEliminar_Click;
            lblFiltroFormulario = new Label { Location = new Point(20, 65), AutoSize = true, Name = "lblFiltroFormulario", Text = T("lblFiltroFormulario", "Filtrar por Formulario:") };
            cmbFiltroFormulario = new ComboBox { Location = new Point(160, 60), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Name = "cmbFiltroFormulario" };
            lblFiltroTexto = new Label { Location = new Point(330, 65), AutoSize = true, Name = "lblFiltroTexto", Text = T("lblFiltroTexto", "Buscar Texto:") };
            txtFiltroTexto = new TextBox { Location = new Point(420, 60), Width = 150, Name = "txtFiltroTexto" };
            btnLimpiarFiltros = new Button { Location = new Point(590, 58), Width = 90, Name = "btnLimpiarFiltros", Text = T("btnLimpiarFiltros", "Limpiar") };
            cmbFiltroFormulario.SelectedIndexChanged += CmbFiltroFormulario_SelectedIndexChanged;
            txtFiltroTexto.TextChanged += TxtFiltroTexto_TextChanged;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            dgvTraducciones = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(660, 360),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Name = "dgvTraducciones"
            };
            colFormulario = new DataGridViewTextBoxColumn { DataPropertyName = "Formulario", ReadOnly = true, Width = 150, Name = "colFormulario", HeaderText = T("colFormulario", "Formulario") };
            colNombreControl = new DataGridViewTextBoxColumn { DataPropertyName = "NombreControl", ReadOnly = true, Width = 150, Name = "colNombreControl", HeaderText = T("colNombreControl", "Control") };
            colTexto = new DataGridViewTextBoxColumn { DataPropertyName = "Texto", Width = 310, Name = "colTexto", HeaderText = T("colTexto", "Texto Traducido") };
            dgvTraducciones.Columns.AddRange(new DataGridViewColumn[] { colFormulario, colNombreControl, colTexto });
            btnGuardar = new Button { Location = new Point(440, 480), Width = 240, Height = 40, Name = "btnGuardar", Text = T("btnGuardar", "Guardar Cambios") };
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(lblSeleccion);
            this.Controls.Add(cmbIdiomasAdmin);
            this.Controls.Add(btnNuevo);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(lblFiltroFormulario);
            this.Controls.Add(cmbFiltroFormulario);
            this.Controls.Add(lblFiltroTexto);
            this.Controls.Add(txtFiltroTexto);
            this.Controls.Add(btnLimpiarFiltros);
            this.Controls.Add(dgvTraducciones);
            this.Controls.Add(btnGuardar);
        }

        private void frmGestionIdiomas_Load(object sender, EventArgs e)
        {
            CargarIdiomas();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            if (idioma == null) return;
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey(this.Name))
                this.Text = _traducciones[this.Name];
            TraducirControlesRecursivo(this.Controls, _traducciones);
            if (cmbFiltroFormulario != null && cmbFiltroFormulario.Items.Count > 0 && _traduccionesActuales != null && _traduccionesActuales.Count > 0)
            {
                int idx = cmbFiltroFormulario.SelectedIndex;
                var formularios = _traduccionesActuales
                    .Where(t => !string.IsNullOrEmpty(t.Formulario))
                    .Select(t => t.Formulario)
                    .Distinct()
                    .OrderBy(f => f)
                    .ToList();
                formularios.Insert(0, T("cmbFiltroTodos", "Todos"));
                cmbFiltroFormulario.SelectedIndexChanged -= CmbFiltroFormulario_SelectedIndexChanged;
                cmbFiltroFormulario.DataSource = formularios;
                if (idx >= 0 && idx < cmbFiltroFormulario.Items.Count)
                    cmbFiltroFormulario.SelectedIndex = idx;
                cmbFiltroFormulario.SelectedIndexChanged += CmbFiltroFormulario_SelectedIndexChanged;
            }
        }

        private void TraducirControlesRecursivo(Control.ControlCollection controles, Dictionary<string, string> traducciones)
        {
            foreach (Control control in controles)
            {
                if (traducciones.ContainsKey(control.Name))
                {
                    control.Text = traducciones[control.Name];
                }
                if (control.HasChildren)
                {
                    TraducirControlesRecursivo(control.Controls, traducciones);
                }
                if (control is DataGridView dgv)
                {
                    foreach (DataGridViewColumn columna in dgv.Columns)
                    {
                        if (traducciones.ContainsKey(columna.Name))
                        {
                            columna.HeaderText = traducciones[columna.Name];
                        }
                    }
                }
            }
        }

        private void CargarIdiomas()
        {
            int? idPrevio = (cmbIdiomasAdmin.SelectedItem is IdiomaBE prev) ? prev.ID : null;
            var listaIdiomas = idiomaBLL.ObtenerIdiomas();
            cmbIdiomasAdmin.SelectedIndexChanged -= CmbIdiomasAdmin_SelectedIndexChanged;
            cmbIdiomasAdmin.DataSource = null;
            cmbIdiomasAdmin.DisplayMember = "Nombre";
            cmbIdiomasAdmin.ValueMember = "ID";
            cmbIdiomasAdmin.DataSource = listaIdiomas;
            if (idPrevio.HasValue && listaIdiomas.Any(i => i.ID == idPrevio.Value))
            {
                cmbIdiomasAdmin.SelectedValue = idPrevio.Value;
            }
            else if (listaIdiomas.Count > 0)
            {
                cmbIdiomasAdmin.SelectedIndex = 0;
            }
            cmbIdiomasAdmin.SelectedIndexChanged += CmbIdiomasAdmin_SelectedIndexChanged;
            if (cmbIdiomasAdmin.SelectedItem is IdiomaBE idiomaSel)
            {
                CargarTraducciones(idiomaSel.ID);
            }
        }

        private void CmbIdiomasAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdiomasAdmin.SelectedItem is IdiomaBE idiomaSel)
            {
                CargarTraducciones(idiomaSel.ID);
            }
        }

        private void CargarTraducciones(int idIdioma)
        {
            _traduccionesActuales = idiomaBLL.ObtenerTodasLasTraducciones(idIdioma);
            var formularios = _traduccionesActuales
                .Where(t => !string.IsNullOrEmpty(t.Formulario))
                .Select(t => t.Formulario)
                .Distinct()
                .OrderBy(f => f)
                .ToList();
            formularios.Insert(0, T("cmbFiltroTodos", "Todos"));
            cmbFiltroFormulario.SelectedIndexChanged -= CmbFiltroFormulario_SelectedIndexChanged;
            cmbFiltroFormulario.DataSource = formularios;
            cmbFiltroFormulario.SelectedIndexChanged += CmbFiltroFormulario_SelectedIndexChanged;
            txtFiltroTexto.TextChanged -= TxtFiltroTexto_TextChanged;
            txtFiltroTexto.Text = string.Empty;
            txtFiltroTexto.TextChanged += TxtFiltroTexto_TextChanged;
            AplicarFiltros();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            string nuevoNombre = Microsoft.VisualBasic.Interaction.InputBox(T("msgInputNuevo", "Ingrese el nombre del nuevo idioma:"), T("titInputNuevo", "Nuevo Idioma"));
            string sufijo = Microsoft.VisualBasic.Interaction.InputBox(T("msgInputSufijo", "Ingrese un sufijo identificador temporal (Ej: ENG):"), T("titInputSufijo", "Sufijo"));
            if (!string.IsNullOrEmpty(nuevoNombre) && !string.IsNullOrEmpty(sufijo))
            {
                try
                {
                    idiomaBLL.AgregarNuevoIdioma(nuevoNombre, sufijo);
                    CargarIdiomas();
                    IdiomasActualizados?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show(T("msgNuevoExito", "Idioma generado con las etiquetas base. Ahora puede editarlas en la grilla."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (cmbIdiomasAdmin.SelectedItem is IdiomaBE idiomaSeleccionado)
            {
                int idIdioma = idiomaSeleccionado.ID;
                if (MessageBox.Show(T("msgConfirmarEliminar", "¿Eliminar este idioma? Los usuarios que lo tengan volverán a Español."), T("titConfirmarEliminar", "Advertencia"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        idiomaBLL.EliminarIdioma(idIdioma);
                        if (GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID == idIdioma)
                        {
                            var espanol = idiomaBLL.ObtenerIdiomas().FirstOrDefault(i => i.ID == 1);
                            if (espanol != null) GestorIdioma.Instancia.CambiarIdioma(espanol);
                        }
                        CargarIdiomas();
                        IdiomasActualizados?.Invoke(this, EventArgs.Empty);
                        MessageBox.Show(T("msgEliminarExito", "Idioma eliminado correctamente."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            dgvTraducciones.EndEdit();
            if (_traduccionesActuales != null && _traduccionesActuales.Count > 0)
            {
                try
                {
                    idiomaBLL.ActualizarTraducciones(_traduccionesActuales);
                    MessageBox.Show(T("msgGuardarExito", "Las traducciones se han guardado exitosamente."), T("titGuardarExito", "Guardado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (cmbIdiomasAdmin.SelectedItem is IdiomaBE idiomaSel && idiomaSel.ID == GestorIdioma.Instancia.IdiomaActual.ID)
                    {
                        GestorIdioma.Instancia.Notificar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmGestionIdiomas_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }

        private void AplicarFiltros()
        {
            if (_traduccionesActuales == null) return;
            var traduccionesFiltradas = _traduccionesActuales.AsEnumerable();
            if (cmbFiltroFormulario.SelectedItem != null)
            {
                string formSeleccionado = cmbFiltroFormulario.SelectedItem.ToString();
                if (formSeleccionado != T("cmbFiltroTodos", "Todos"))
                {
                    traduccionesFiltradas = traduccionesFiltradas.Where(t => t.Formulario == formSeleccionado);
                }
            }
            string busqueda = txtFiltroTexto.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(busqueda))
            {
                traduccionesFiltradas = traduccionesFiltradas.Where(t =>
                    (t.NombreControl != null && t.NombreControl.ToLower().Contains(busqueda)) ||
                    (t.Texto != null && t.Texto.ToLower().Contains(busqueda))
                );
            }
            dgvTraducciones.DataSource = new BindingList<TraduccionBE>(traduccionesFiltradas.ToList());
        }

        private void CmbFiltroFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void TxtFiltroTexto_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            cmbFiltroFormulario.SelectedIndex = 0;
            txtFiltroTexto.Text = "";
        }
    }
}