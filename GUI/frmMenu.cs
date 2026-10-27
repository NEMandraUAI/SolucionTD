using BE;
using BLL;
using GUI.Eventos;
using Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace GUI
{
    public partial class frmMenu : Form, IObserverIdioma
    {
        private bool recuperacionEnCurso;
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private Form formularioActivo = null;

        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }

        public frmMenu()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += FrmMenu_KeyDown;
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            SessionManager.Instancia.PermisosActualizados += SessionManager_PermisosActualizados;
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void AbrirFormularioHijo(Form formHijo)
        {
            CerrarTodosLosFormularios();
            formularioActivo = formHijo;
            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(formHijo);
            pnlContenedor.Tag = formHijo;
            formHijo.BringToFront();
            formHijo.Show();
        }

        private void FrmMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control || !e.Alt || e.KeyCode != Keys.R) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            if (recuperacionEnCurso) return;
            recuperacionEnCurso = true;
            try
            {
                var backup = new BackupBLL();
                backup.AutorizarRecuperacionAdministrativa();
                try
                {
                    new IntegridadBLL().VerificarIntegridadSistema();
                    InstaladorBLL.RegistrarLog("Recuperación solicitada con integridad actualmente válida.");
                }
                catch (SqlException) { throw; }
                catch (Exception ex)
                {
                    InstaladorBLL.RegistrarLog("Diagnóstico previo a recuperación administrativa: " + ex.Message);
                }
                var opcion = MessageBox.Show(
                    "Recuperación administrativa. Antes de continuar debe conservar un respaldo de la base actual.\r\n\r\n" +
                    "SÍ: recalcular los dígitos después de una intervención técnica legítima. Esta acción acepta los datos actuales como válidos.\r\n" +
                    "NO: seleccionar un backup para restaurar; reemplazará los datos actuales.\r\n\r\n" +
                    "Cancelar: salir sin cambios.",
                    "Recuperación administrativa", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (opcion == DialogResult.Cancel) return;
                string? rutaBackup = null;
                if (opcion == DialogResult.No)
                {
                    using var ofd = new OpenFileDialog
                    {
                        Filter = "Archivos de Backup SQL (*.bak)|*.bak",
                        Title = "Seleccione el respaldo que reemplazará la base configurada",
                        CheckFileExists = true
                    };
                    if (ofd.ShowDialog(this) != DialogResult.OK) return;
                    rutaBackup = ofd.FileName;
                }
                string confirmacion = opcion == DialogResult.Yes
                    ? "¿Confirma que los datos actuales fueron revisados y autoriza recalcular su integridad?"
                    : "¿Confirma la sustitución de la base configurada por el backup seleccionado? Los cambios posteriores al backup se perderán.";
                if (MessageBox.Show(confirmacion, "Confirmar recuperación", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                SessionManager.Instancia.Cerrar();
                CerrarTodosLosFormularios();
                AplicarPermisosUI();
                EvaluarPermisosIdioma();
                if (opcion == DialogResult.Yes) backup.RecalcularIntegridadAdministrativamente();
                else backup.RestaurarBackup(rutaBackup!);
                MessageBox.Show("Recuperación completada y verificada. Inicie sesión nuevamente.",
                    "Recuperación administrativa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AbrirLogin();
            }
            catch (Exception ex)
            {
                InstaladorBLL.RegistrarLog("Recuperación administrativa no completada: " + ex.Message);
                MessageBox.Show(ex.Message, "Recuperación administrativa no completada",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                recuperacionEnCurso = false;
            }
        }

        private void ManejarErrorConexionSQL(SqlException sqlEx)
        {
            CerrarTodosLosFormularios();
            InstaladorBLL.RegistrarLog($"SqlException capturada en frmMenu (Number={sqlEx.Number}): {sqlEx.Message}");
            if (sqlEx.Number == 207 || sqlEx.Number == 208 || sqlEx.Number == 2812)
            {
                if (InstaladorBLL.RepararEsquemaBaseDeDatosInteractivo())
                {
                    AbrirLogin();
                }
                return;
            }
            DialogResult reintentar = MessageBox.Show(
                "No se pudo establecer comunicación con el motor de base de datos SQL Server.\r\n\r\n" +
                $"Detalle técnico (Código {sqlEx.Number}): {sqlEx.Message}\r\n\r\n" +
                "¿Desea ejecutar el Asistente de Diagnóstico de Servicio e Instancias SQL Server para restablecer la conexión?",
                "Error de Conexión al Motor SQL Server",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (reintentar == DialogResult.Yes && InstaladorBLL.AsegurarConexionYBaseDisponibleEnArranque())
            {
                AbrirLogin();
            }
        }

        private void AbrirLogin()
        {
            IntegridadBLL integridadBLL = new IntegridadBLL();
            try
            {
                integridadBLL.VerificarIntegridadSistema();
                frmLogIn login = new frmLogIn();
                login.LoginExitoso += OnLoginExitoso;
                AbrirFormularioHijo(login);
            }
            catch (SqlException sqlEx)
            {
                ManejarErrorConexionSQL(sqlEx);
            }
            catch (Exception ex)
            {
                CerrarTodosLosFormularios();
                MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLoginExitoso(object sender, LogInEventArgs e)
        {
            CerrarTodosLosFormularios();
            frmSesion bienvenida = new frmSesion(e.Usuario);
            bienvenida.CerrarSesion += (s, args) => CerrarSesionGlobal();
            AbrirFormularioHijo(bienvenida);
            EvaluarPermisosIdioma();
            AplicarPermisosUI();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            if (!InstaladorBLL.AsegurarConexionYBaseDisponibleEnArranque())
            {
                Application.Exit();
                return;
            }
            if (SchemaMigradorBLL.InicializacionPendiente(DAL.ConexionDAL.ObtenerConnectionStringConfigurada()))
            {
                using var inicial = new frmAdministradorInicial();
                if (inicial.ShowDialog(this) != DialogResult.OK) { Application.Exit(); return; }
            }
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            cmbIdiomas.SelectedIndexChanged -= cmbIdiomas_SelectedIndexChanged;
            cmbIdiomas.DataSource = idiomaBLL.ObtenerIdiomas();
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "ID";
            cmbIdiomas.SelectedValue = GestorIdioma.Instancia.IdiomaActual.ID;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            btnNuevoIdioma.Visible = false;
            btnNuevoIdioma.Text = T("btnNuevoIdioma", "Gestionar Idiomas");
            OcultarSubMenus();
            AplicarPermisosUI();
            if (SessionManager.Instancia.UsuarioActual == null)
            {
                AbrirLogin();
            }
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            this.Text = "UAI-FIT";
            TraducirControlesRecursivo(this.Controls, _traducciones);
            cmbIdiomas.SelectedIndexChanged -= cmbIdiomas_SelectedIndexChanged;
            cmbIdiomas.SelectedValue = idioma.ID;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
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
                        if (traducciones.ContainsKey(columna.DataPropertyName))
                        {
                            columna.HeaderText = traducciones[columna.DataPropertyName];
                        }
                    }
                }
            }
        }

        private void RecargarComboIdiomas()
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            var idiomas = idiomaBLL.ObtenerIdiomas();
            int idActual = GestorIdioma.Instancia.IdiomaActual?.ID ?? 1;
            if (!idiomas.Any(i => i.ID == idActual))
            {
                var espanol = idiomas.FirstOrDefault(i => i.ID == 1) ?? idiomas.FirstOrDefault();
                if (espanol != null)
                {
                    idActual = espanol.ID;
                    GestorIdioma.Instancia.CambiarIdioma(espanol);
                }
            }
            cmbIdiomas.SelectedIndexChanged -= cmbIdiomas_SelectedIndexChanged;
            cmbIdiomas.DataSource = null;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "ID";
            cmbIdiomas.DataSource = idiomas;
            cmbIdiomas.SelectedValue = idActual;
            cmbIdiomas.SelectedIndexChanged += cmbIdiomas_SelectedIndexChanged;
        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdiomas.SelectedItem is IdiomaBE idiomaSeleccionado)
            {
                if (GestorIdioma.Instancia.IdiomaActual != null && GestorIdioma.Instancia.IdiomaActual.ID == idiomaSeleccionado.ID)
                {
                    return;
                }
                try
                {
                    IdiomaBLL idiomaBLL = new IdiomaBLL();
                    if (!idiomaBLL.ObtenerIdiomas().Any(i => i.ID == idiomaSeleccionado.ID))
                    {
                        RecargarComboIdiomas();
                        return;
                    }
                    GestorIdioma.Instancia.CambiarIdioma(idiomaSeleccionado);
                    if (SessionManager.Instancia.UsuarioActual != null)
                    {
                        UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
                        usuarioActual.Idioma = idiomaSeleccionado;
                        UsuarioBLL usuarioBLL = new UsuarioBLL();
                        usuarioBLL.ActualizarIdiomaUsuario(usuarioActual.ID, idiomaSeleccionado.ID);
                    }
                }
                catch (Exception ex)
                {
                    InstaladorBLL.RegistrarLog("Aviso al cambiar idioma: " + ex.Message);
                    RecargarComboIdiomas();
                }
            }
        }

        public void EvaluarPermisosIdioma()
        {
            var u = SessionManager.Instancia.UsuarioActual;
            btnNuevoIdioma.Visible = (u != null && u.TienePermiso("GESTION_IDIOMAS"));
        }

        private void CerrarTodosLosFormularios()
        {
            foreach (Control c in pnlContenedor.Controls.OfType<Form>().ToList())
            {
                ((Form)c).Close();
            }
            formularioActivo = null;
            pnlContenedor.Controls.Clear();
        }

        public void AplicarPermisosUI()
        {
            UsuarioBE usuario = SessionManager.Instancia.UsuarioActual;
            bool haySesion = usuario != null;
            btnMenuIniciarSesion.Visible = !haySesion;
            btnMenuRegistrarse.Visible = !haySesion;
            btnMenuCerrarSesion.Visible = haySesion;
            btnMenuInicio.Visible = haySesion;
            if (haySesion)
            {
                bool puedeVenderPlan = usuario.TienePermiso("VENDER_PLAN");
                bool puedeConsultarComprobantes = usuario.TienePermiso("CONSULTAR_COMPROBANTES") || puedeVenderPlan;
                bool puedeAgendaRec = usuario.TienePermiso("ASENTAR_TURNO") || usuario.TienePermiso("REVISAR_AGENDA");
                btnSubVenderPlan.Visible = puedeVenderPlan;
                btnSubConsultarComprobantes.Visible = puedeConsultarComprobantes;
                btnSubAgendarTurno.Visible = puedeAgendaRec;
                btnMenuRecepcion.Visible = puedeVenderPlan || puedeConsultarComprobantes || puedeAgendaRec;
                bool puedeAsignarRutina = usuario.TienePermiso("ASIGNAR_RUTINA");
                bool puedeGestionarRutinas = usuario.TienePermiso("CONSULTAR_SOCIO") || puedeAsignarRutina;
                btnSubAsignarRutina.Visible = puedeAsignarRutina;
                btnSubGestionRutinas.Visible = puedeGestionarRutinas;
                btnMenuEntrenamiento.Visible = puedeAsignarRutina || puedeGestionarRutinas;
                bool puedeNutricion = usuario.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL") ||
                                      usuario.TienePermiso("CONSULTAR_AGENDA") ||
                                      usuario.TienePermiso("CONSULTAR_PLANES_NUT");
                btnMenuNutricion.Visible = puedeNutricion;
                bool puedeVentasMostrador = usuario.TienePermiso("VENDER_PRODUCTO");
                bool puedePrediccionDemanda = usuario.TienePermiso("ANALIZAR_DEMANDA");
                btnSubVentasMostrador.Visible = puedeVentasMostrador;
                btnSubPrediccionDemanda.Visible = puedePrediccionDemanda;
                btnMenuComercial.Visible = puedeVentasMostrador || puedePrediccionDemanda;
            }
            else
            {
                btnMenuRecepcion.Visible = false;
                btnMenuEntrenamiento.Visible = false;
                btnMenuNutricion.Visible = false;
                btnMenuComercial.Visible = false;
            }
            OcultarSubMenus();
        }

        private void frmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
            SessionManager.Instancia.PermisosActualizados -= SessionManager_PermisosActualizados;
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.CerrarSesion();
                MessageBox.Show(T("msgSesionCerrada", "La sesión se cerró correctamente."), T("titSesionCerrada", "Sesión Cerrada"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SessionManager_PermisosActualizados(object sender, EventArgs e)
        {
            AplicarPermisosUI();
            EvaluarPermisosIdioma();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            IntegridadBLL integridadBLL = new IntegridadBLL();
            try
            {
                integridadBLL.VerificarIntegridadSistema();
                if (SessionManager.Instancia.UsuarioActual != null)
                {
                    MessageBox.Show(T("msgSesionActiva", "Ya existe una sesión activa. Debe cerrarla antes de ingresar con otra cuenta."), T("titSesionActiva", "Sesión Activa"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                OcultarSubMenus();
                AbrirLogin();
            }
            catch (SqlException sqlEx)
            {
                ManejarErrorConexionSQL(sqlEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                CerrarTodosLosFormularios();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            IntegridadBLL integridadBLL = new IntegridadBLL();
            try
            {
                integridadBLL.VerificarIntegridadSistema();
                OcultarSubMenus();
                frmRegistro registro = new frmRegistro();
                AbrirFormularioHijo(registro);
            }
            catch (SqlException sqlEx)
            {
                ManejarErrorConexionSQL(sqlEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                CerrarTodosLosFormularios();
            }
        }

        private void btnNuevoIdioma_Click(object sender, EventArgs e)
        {
            frmGestionIdiomas frm = new frmGestionIdiomas();
            frm.IdiomasActualizados += (s, args) => RecargarComboIdiomas();
            frm.FormClosed += (s, args) => RecargarComboIdiomas();
            OcultarSubMenus();
            AbrirFormularioHijo(frm);
        }

        private void OcultarSubMenus()
        {
            pnlSubRecepcion.Visible = false;
            pnlSubEntrenamiento.Visible = false;
            pnlSubNutricion.Visible = false;
            pnlSubComercial.Visible = false;
        }

        private void btnMenuRecepcion_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            pnlSubRecepcion.Visible = true;
            pnlSubRecepcion.BringToFront();
        }

        private void btnMenuEntrenamiento_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            pnlSubEntrenamiento.Visible = true;
            pnlSubEntrenamiento.BringToFront();
        }

        private void btnMenuNutricion_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            frmGestionNutricion frm = new frmGestionNutricion();
            AbrirFormularioHijo(frm);
        }

        private void btnMenuComercial_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            pnlSubComercial.Visible = true;
            pnlSubComercial.BringToFront();
        }

        private void btnSubVentasMostrador_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmVentasMostrador());
        }

        private void btnSubPrediccionDemanda_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmPrediccionReabastecimiento());
        }

        private void btnMenuCerrarSesion_Click(object sender, EventArgs e)
        {
            CerrarSesionGlobal();
        }

        private void CerrarSesionGlobal()
        {
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.CerrarSesion();
                CerrarTodosLosFormularios();
                AplicarPermisosUI();
                EvaluarPermisosIdioma();
                BLL.IntegridadBLL integridadBLL = new BLL.IntegridadBLL();
                try
                {
                    integridadBLL.VerificarIntegridadSistema();
                    MessageBox.Show(T("msgSesionCerrada", "La sesión se cerró correctamente."), T("titSesionCerrada", "Sesión Cerrada"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AbrirLogin();
                }
                catch (SqlException sqlEx)
                {
                    ManejarErrorConexionSQL(sqlEx);
                }
                catch (Exception ex)
                {
                    CerrarTodosLosFormularios();
                    MessageBox.Show(T(ex.Message, ex.Message) + T("msgCorrupcion", "\nEl sistema se bloqueará por seguridad. Contacte al administrador."), T("titCorrupcion", "Corrupción de Datos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSubAsignarRutina_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmEntrenamiento());
        }

        private void btnSubGestionRutinas_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmGestionRutinas());
        }

        private void btnSubVenderPlan_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmRecepcion());
        }

        private void btnSubConsultarComprobantes_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmConsultarComprobantes());
        }

        private void btnSubAgendarTurno_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new frmAgendaRecepcion());
        }

        private void btnMenuInicio_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                frmSesion inicio = new frmSesion(SessionManager.Instancia.UsuarioActual);
                inicio.CerrarSesion += (s, args) => CerrarSesionGlobal();
                AbrirFormularioHijo(inicio);
            }
        }
    }
}