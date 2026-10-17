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
    public partial class frmEntrenamiento : Form, IObserverIdioma
    {
        SocioBLL socioBLL = new SocioBLL();
        RutinaEntrenamientoBLL rutinaBLL = new RutinaEntrenamientoBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();
        SocioBE socioActual = null;
        BindingList<EjercicioBE> _ejerciciosBinding = new BindingList<EjercicioBE>();
        bool _cargandoInicial = false;
        int _varianteSugerencia = 0;

        public frmEntrenamiento()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmEntrenamiento_Load(object sender, EventArgs e)
        {
            _cargandoInicial = true;
            CargarCombosIniciales();
            ConfigurarGrillaEjercicios();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            BloquearControlesRutina(true);
            _cargandoInicial = false;
        }

        private void CargarCombosIniciales()
        {
            cmbObjetivo.Items.Clear();
            foreach (var obj in rutinaBLL.ObtenerObjetivosDisponibles())
            {
                cmbObjetivo.Items.Add(obj);
            }
            if (cmbObjetivo.Items.Count > 0)
                cmbObjetivo.SelectedIndex = 0;
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
            dgvEjercicios.DefaultValuesNeeded += dgvEjercicios_DefaultValuesNeeded;
            dgvEjercicios.DataSource = _ejerciciosBinding;
        }

        private void dgvEjercicios_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["GrupoMuscular"].Value = "Pecho";
            e.Row.Cells["Series"].Value = 4;
            e.Row.Cells["Repeticiones"].Value = "10-12";
            e.Row.Cells["TiempoDescansoSegundos"].Value = 90;
            e.Row.Cells["CargaSugeridaKg"].Value = 20m;
        }

        private void CargarRutinaSugeridaInteligente()
        {
            if (_cargandoInicial) return;
            string objetivo = cmbObjetivo.SelectedItem != null
                ? cmbObjetivo.SelectedItem.ToString()
                : (!string.IsNullOrWhiteSpace(cmbObjetivo.Text) ? cmbObjetivo.Text : "Hipertrofia");
            int frecuencia = (int)numFrecuencia.Value;
            dgvEjercicios.CancelEdit();
            dgvEjercicios.EndEdit();
            var sugeridos = rutinaBLL.GenerarRutinaSugerida(objetivo, frecuencia, _varianteSugerencia);
            _ejerciciosBinding = new BindingList<EjercicioBE>(sugeridos);
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = _ejerciciosBinding;
            dgvEjercicios.Refresh();
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("btnBuscar")) btnBuscar.Text = traducciones["btnBuscar"];
            if (traducciones.ContainsKey("btnAsignarRutina")) btnAsignarRutina.Text = traducciones["btnAsignarRutina"];
            if (traducciones.ContainsKey("lblDNI")) lblDNI.Text = traducciones["lblDNI"];
            if (traducciones.ContainsKey("lblObjetivo")) lblObjetivo.Text = traducciones["lblObjetivo"];
            if (traducciones.ContainsKey("lblFrecuencia")) lblFrecuencia.Text = traducciones["lblFrecuencia"];
            if (traducciones.ContainsKey("lblDetalle")) lblDetalle.Text = traducciones["lblDetalle"];
            this.Text = traducciones.ContainsKey("titEntrenamiento") ? traducciones["titEntrenamiento"] : "Asignación de Rutina";
        }

        private void BloquearControlesRutina(bool bloquear)
        {
            cmbObjetivo.Enabled = !bloquear;
            numFrecuencia.Enabled = !bloquear;
            dgvEjercicios.Enabled = !bloquear;
            btnSugerirRutina.Enabled = !bloquear;
            btnQuitarEjercicio.Enabled = !bloquear;
            btnAsignarRutina.Enabled = !bloquear;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                socioActual = socioBLL.ConsultarSocio(txtDNI.Text);
                if (socioActual != null)
                {
                    if (!socioActual.EstadoActivo)
                    {
                        MessageBox.Show("El socio se encuentra inactivo. Derívelo a Recepción.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        BloquearControlesRutina(true);
                        return;
                    }
                    lblInfoSocio.Text = $"Socio Validado: {socioActual.Nombre} {socioActual.Apellido}";
                    BloquearControlesRutina(false);
                    _varianteSugerencia = 0;
                    CargarRutinaSugeridaInteligente();
                }
                else
                {
                    MessageBox.Show("Socio no encontrado. Verifique el DNI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblInfoSocio.Text = "-";
                    socioActual = null;
                    _ejerciciosBinding.Clear();
                    BloquearControlesRutina(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbObjetivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (socioActual != null && socioActual.EstadoActivo)
            {
                _varianteSugerencia = 0;
                CargarRutinaSugeridaInteligente();
            }
        }

        private void numFrecuencia_ValueChanged(object sender, EventArgs e)
        {
            if (socioActual != null && socioActual.EstadoActivo)
            {
                CargarRutinaSugeridaInteligente();
            }
        }

        private void btnSugerirRutina_Click(object sender, EventArgs e)
        {
            _varianteSugerencia = (_varianteSugerencia + 1) % 3;
            CargarRutinaSugeridaInteligente();
            char letraVariante = (char)('A' + _varianteSugerencia);
            lblInfoSocio.Text = $"Socio Validado: {socioActual.Nombre} {socioActual.Apellido} (Sugerencia: Variante {letraVariante})";
        }

        private void btnQuitarEjercicio_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow != null && !dgvEjercicios.CurrentRow.IsNewRow)
            {
                dgvEjercicios.Rows.Remove(dgvEjercicios.CurrentRow);
            }
        }

        private void btnAsignarRutina_Click(object sender, EventArgs e)
        {
            try
            {
                if (socioActual == null) throw new Exception("Debe validar un socio primero.");
                dgvEjercicios.EndEdit();
                RutinaEntrenamientoBE rutina = new RutinaEntrenamientoBE();
                rutina.FechaCreacion = DateTime.Now;
                rutina.ObjetivoFisico = cmbObjetivo.SelectedItem != null ? cmbObjetivo.SelectedItem.ToString() : cmbObjetivo.Text;
                rutina.FrecuenciaSemanal = (int)numFrecuencia.Value;
                rutina.Ejercicios = _ejerciciosBinding
                    .Where(ej => ej != null && !string.IsNullOrWhiteSpace(ej.Nombre))
                    .ToList();
                rutina.Socio = socioActual;
                rutina.Entrenador = SessionManager.Instancia.UsuarioActual;
                rutinaBLL.AsignarRutina(rutina);
                MessageBox.Show("Rutina de entrenamiento generada y asignada al socio con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmEntrenamiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}