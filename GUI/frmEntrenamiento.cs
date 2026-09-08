using System;
using System.Collections.Generic;
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

        public frmEntrenamiento()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
            this.FormClosing += frmEntrenamiento_FormClosing;
            this.Load += frmEntrenamiento_Load;
        }

        private void frmEntrenamiento_Load(object sender, EventArgs e)
        {
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            BloquearControlesRutina(true);
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
            txtObjetivo.Enabled = !bloquear;
            numFrecuencia.Enabled = !bloquear;
            txtDetalle.Enabled = !bloquear;
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
                }
                else
                {
                    MessageBox.Show("Socio no encontrado. Verifique el DNI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblInfoSocio.Text = "-";
                    socioActual = null;
                    BloquearControlesRutina(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAsignarRutina_Click(object sender, EventArgs e)
        {
            try
            {
                if (socioActual == null) throw new Exception("Debe validar un socio primero.");
                RutinaEntrenamientoBE rutina = new RutinaEntrenamientoBE();
                rutina.FechaCreacion = DateTime.Now;
                rutina.ObjetivoFisico = txtObjetivo.Text;
                rutina.FrecuenciaSemanal = (int)numFrecuencia.Value;
                rutina.DetalleEjercicios = txtDetalle.Text;
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
