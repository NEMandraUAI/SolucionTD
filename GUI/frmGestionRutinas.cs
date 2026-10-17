using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmGestionRutinas : Form, IObserverIdioma
    {
        RutinaEntrenamientoBLL rutinaBLL = new RutinaEntrenamientoBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();
        RutinaEntrenamientoBE rutinaSeleccionada = null;
        BindingList<EjercicioBE> _ejerciciosBinding = new BindingList<EjercicioBE>();
        int _varianteSugerencia = 0;

        public frmGestionRutinas()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmGestionRutinas_Load(object sender, EventArgs e)
        {
            cmbObjetivo.Items.Clear();
            foreach (var obj in rutinaBLL.ObtenerObjetivosDisponibles())
            {
                cmbObjetivo.Items.Add(obj);
            }
            ConfigurarGrillaEjercicios();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            ActivarControlesEdicion(false);
        }

        private void ConfigurarGrillaEjercicios()
        {
            dgvEjercicios.AutoGenerateColumns = false;
            dgvEjercicios.Columns.Clear();
            dgvEjercicios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                DataPropertyName = "Nombre",
                HeaderText = "Ejercicio",
                FillWeight = 30
            });
            var colGrupo = new DataGridViewComboBoxColumn
            {
                Name = "GrupoMuscular",
                DataPropertyName = "GrupoMuscular",
                HeaderText = "Grupo Muscular",
                DataSource = rutinaBLL.ObtenerGruposMusculares(),
                FlatStyle = FlatStyle.Flat,
                FillWeight = 20
            };
            dgvEjercicios.Columns.Add(colGrupo);
            dgvEjercicios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Series",
                DataPropertyName = "Series",
                HeaderText = "Series",
                FillWeight = 10
            });
            dgvEjercicios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Repeticiones",
                DataPropertyName = "Repeticiones",
                HeaderText = "Repeticiones",
                FillWeight = 14
            });
            dgvEjercicios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TiempoDescansoSegundos",
                DataPropertyName = "TiempoDescansoSegundos",
                HeaderText = "Descanso (seg)",
                FillWeight = 13
            });
            dgvEjercicios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CargaSugeridaKg",
                DataPropertyName = "CargaSugeridaKg",
                HeaderText = "Carga Sug. (Kg)",
                FillWeight = 13
            });
            dgvEjercicios.DataError += (s, ev) => { ev.ThrowException = false; };
            dgvEjercicios.DefaultValuesNeeded += (s, ev) =>
            {
                ev.Row.Cells["GrupoMuscular"].Value = "Pecho";
                ev.Row.Cells["Series"].Value = 4;
                ev.Row.Cells["Repeticiones"].Value = "10-12";
                ev.Row.Cells["TiempoDescansoSegundos"].Value = 90;
                ev.Row.Cells["CargaSugeridaKg"].Value = 20m;
            };
            dgvEjercicios.DataSource = _ejerciciosBinding;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("lblDNI")) lblDNI.Text = traducciones["lblDNI"];
            if (traducciones.ContainsKey("btnBuscar")) btnBuscar.Text = traducciones["btnBuscar"];
            if (traducciones.ContainsKey("lblObjetivo")) lblObjetivo.Text = traducciones["lblObjetivo"];
            if (traducciones.ContainsKey("lblFrecuencia")) lblFrecuencia.Text = traducciones["lblFrecuencia"];
            if (traducciones.ContainsKey("lblDetalle")) lblDetalle.Text = traducciones["lblDetalle"];
            if (traducciones.ContainsKey("btnModificar")) btnModificar.Text = traducciones["btnModificar"];
            if (traducciones.ContainsKey("btnEliminar")) btnEliminar.Text = traducciones["btnEliminar"];
            this.Text = traducciones.ContainsKey("titGestionRutinas") ? traducciones["titGestionRutinas"] : "Gestión de Rutinas";
            if (dgvRutinas.Columns["CodigoRutina"] != null && traducciones.ContainsKey("colCod")) dgvRutinas.Columns["CodigoRutina"].HeaderText = traducciones["colCod"];
            if (dgvRutinas.Columns["FechaCreacion"] != null && traducciones.ContainsKey("colFecha")) dgvRutinas.Columns["FechaCreacion"].HeaderText = traducciones["colFecha"];
            if (dgvRutinas.Columns["ObjetivoFisico"] != null && traducciones.ContainsKey("colObj")) dgvRutinas.Columns["ObjetivoFisico"].HeaderText = traducciones["colObj"];
            if (dgvRutinas.Columns["FrecuenciaSemanal"] != null && traducciones.ContainsKey("colFrec")) dgvRutinas.Columns["FrecuenciaSemanal"].HeaderText = traducciones["colFrec"];
        }

        private void ActivarControlesEdicion(bool activo)
        {
            cmbObjetivo.Enabled = activo;
            numFrecuencia.Enabled = activo;
            dgvEjercicios.Enabled = activo;
            btnSugerirPlantilla.Enabled = activo;
            btnQuitarEjercicio.Enabled = activo;
            btnModificar.Enabled = activo;
            btnEliminar.Enabled = activo;
            if (!activo)
            {
                cmbObjetivo.Text = string.Empty;
                _ejerciciosBinding.Clear();
                numFrecuencia.Value = 1;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarRutinas();
        }

        private void BuscarRutinas()
        {
            try
            {
                var lista = rutinaBLL.ConsultarRutinasPorDNI(txtDNI.Text);
                dgvRutinas.DataSource = null;
                dgvRutinas.DataSource = lista;
                if (dgvRutinas.Columns["DVH"] != null) dgvRutinas.Columns["DVH"].Visible = false;
                if (dgvRutinas.Columns["DetalleEjercicios"] != null) dgvRutinas.Columns["DetalleEjercicios"].Visible = false;
                if (dgvRutinas.Columns["Socio"] != null) dgvRutinas.Columns["Socio"].Visible = false;
                if (dgvRutinas.Columns["Entrenador"] != null) dgvRutinas.Columns["Entrenador"].Visible = false;
                if (dgvRutinas.Columns["ID_SocioParaDVH"] != null) dgvRutinas.Columns["ID_SocioParaDVH"].Visible = false;
                if (dgvRutinas.Columns["ID_UsuarioParaDVH"] != null) dgvRutinas.Columns["ID_UsuarioParaDVH"].Visible = false;
                ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
                rutinaSeleccionada = null;
                ActivarControlesEdicion(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvRutinas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rutinaSeleccionada = (RutinaEntrenamientoBE)dgvRutinas.Rows[e.RowIndex].DataBoundItem;
                cmbObjetivo.Text = rutinaSeleccionada.ObjetivoFisico;
                numFrecuencia.Value = rutinaSeleccionada.FrecuenciaSemanal;
                var copiaEjercicios = rutinaSeleccionada.Ejercicios
                    .Select(ej => new EjercicioBE
                    {
                        Nombre = ej.Nombre,
                        GrupoMuscular = ej.GrupoMuscular,
                        Series = ej.Series,
                        Repeticiones = ej.Repeticiones,
                        TiempoDescansoSegundos = ej.TiempoDescansoSegundos,
                        CargaSugeridaKg = ej.CargaSugeridaKg
                    }).ToList();
                _ejerciciosBinding = new BindingList<EjercicioBE>(copiaEjercicios);
                dgvEjercicios.DataSource = _ejerciciosBinding;
                ActivarControlesEdicion(true);
            }
        }

        private void btnSugerirPlantilla_Click(object sender, EventArgs e)
        {
            _varianteSugerencia = (_varianteSugerencia + 1) % 3;
            string objetivo = !string.IsNullOrWhiteSpace(cmbObjetivo.Text) ? cmbObjetivo.Text : "Hipertrofia";
            int frecuencia = (int)numFrecuencia.Value;
            dgvEjercicios.CancelEdit();
            dgvEjercicios.EndEdit();
            var sugeridos = rutinaBLL.GenerarRutinaSugerida(objetivo, frecuencia, _varianteSugerencia);
            _ejerciciosBinding = new BindingList<EjercicioBE>(sugeridos);
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = _ejerciciosBinding;
            dgvEjercicios.Refresh();
        }

        private void btnQuitarEjercicio_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow != null && !dgvEjercicios.CurrentRow.IsNewRow)
            {
                dgvEjercicios.Rows.Remove(dgvEjercicios.CurrentRow);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rutinaSeleccionada == null) return;
                dgvEjercicios.EndEdit();
                rutinaSeleccionada.ObjetivoFisico = cmbObjetivo.Text;
                rutinaSeleccionada.FrecuenciaSemanal = (int)numFrecuencia.Value;
                rutinaSeleccionada.Ejercicios = _ejerciciosBinding
                    .Where(ej => ej != null && !string.IsNullOrWhiteSpace(ej.Nombre))
                    .ToList();
                rutinaBLL.ModificarRutina(rutinaSeleccionada);
                MessageBox.Show("Rutina modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BuscarRutinas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rutinaSeleccionada == null) return;
                var result = MessageBox.Show($"¿Seguro que desea eliminar la rutina {rutinaSeleccionada.CodigoRutina}?", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    rutinaBLL.EliminarRutina(rutinaSeleccionada);
                    MessageBox.Show("Rutina eliminada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BuscarRutinas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmGestionRutinas_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}