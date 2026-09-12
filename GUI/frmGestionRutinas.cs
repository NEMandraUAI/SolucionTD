using System;
using System.Collections.Generic;
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

        public frmGestionRutinas()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmGestionRutinas_Load(object sender, EventArgs e)
        {
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            ActivarControlesEdicion(false);
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
            txtObjetivo.Enabled = activo;
            numFrecuencia.Enabled = activo;
            txtDetalle.Enabled = activo;
            btnModificar.Enabled = activo;
            btnEliminar.Enabled = activo;
            if (!activo)
            {
                txtObjetivo.Clear();
                txtDetalle.Clear();
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
                txtObjetivo.Text = rutinaSeleccionada.ObjetivoFisico;
                numFrecuencia.Value = rutinaSeleccionada.FrecuenciaSemanal;
                txtDetalle.Text = rutinaSeleccionada.DetalleEjercicios;
                ActivarControlesEdicion(true);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rutinaSeleccionada == null) return;
                rutinaSeleccionada.ObjetivoFisico = txtObjetivo.Text;
                rutinaSeleccionada.FrecuenciaSemanal = (int)numFrecuencia.Value;
                rutinaSeleccionada.DetalleEjercicios = txtDetalle.Text;
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
