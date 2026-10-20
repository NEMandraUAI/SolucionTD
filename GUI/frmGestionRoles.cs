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
    public partial class frmGestionRoles : Form, IObserverIdioma
    {
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }
        PermisoBLL permisoBLL = new PermisoBLL();
        UsuarioBLL usuarioBLL = new UsuarioBLL();
        List<ComponentePermiso> listaArbolCompleto;
        FamiliaBE familiaSeleccionada;

        public frmGestionRoles()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
            this.FormClosing += (s, e) => GestorIdioma.Instancia.Desuscribir(this);
        }

        private void frmGestionRoles_Load(object sender, EventArgs e)
        {
            CargarArbolRoles();
            CargarUsuarios();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            IdiomaBLL idiomaBLL = new IdiomaBLL();
            _traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (_traducciones.ContainsKey(this.Name))
                this.Text = _traducciones[this.Name];
            TraducirControlesRecursivo(this.Controls, _traducciones);
            if (tvPermisosDisponibles.Nodes.Count >= 2)
            {
                tvPermisosDisponibles.Nodes[0].Text = T("nodoRolesBase", "ROLES (Familias)") + "   ";
                tvPermisosDisponibles.Nodes[1].Text = T("nodoPatentesBase", "PERMISOS SIMPLES (Patentes)") + "   ";
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
                if (control is MenuStrip menuStrip)
                {
                    foreach (ToolStripItem item in menuStrip.Items)
                    {
                        TraducirItemMenu(item, traducciones);
                    }
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

        private void TraducirItemMenu(ToolStripItem item, Dictionary<string, string> traducciones)
        {
            if (traducciones.ContainsKey(item.Name))
            {
                item.Text = traducciones[item.Name];
            }
            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    TraducirItemMenu(subItem, traducciones);
                }
            }
        }

        private void CargarArbolRoles()
        {
            var arbolEstructural = permisoBLL.ObtenerArbolJerarquicoVisual();
            LlenarTreeView(tvRoles, arbolEstructural);
            listaArbolCompleto = permisoBLL.ObtenerArbolCompleto();
            CargarCatalogoPermisosDisponibles();
        }

        private void CargarCatalogoPermisosDisponibles()
        {
            tvPermisosDisponibles.Nodes.Clear();
            TreeNode nodoRoles = new TreeNode(T("nodoRolesBase", "ROLES (Familias)") + "   ") { ForeColor = Color.Black, NodeFont = new Font(tvPermisosDisponibles.Font, FontStyle.Bold) };
            TreeNode nodoPatentes = new TreeNode(T("nodoPatentesBase", "PERMISOS SIMPLES (Patentes)") + "   ") { ForeColor = Color.Black, NodeFont = new Font(tvPermisosDisponibles.Font, FontStyle.Bold) };
            foreach (var comp in listaArbolCompleto)
            {
                TreeNode nodo = new TreeNode(comp.Nombre);
                nodo.Tag = comp;
                if (comp is FamiliaBE)
                {
                    nodo.ForeColor = Color.Blue;
                    nodoRoles.Nodes.Add(nodo);
                }
                else
                {
                    nodo.ForeColor = Color.DarkGreen;
                    nodoPatentes.Nodes.Add(nodo);
                }
            }
            tvPermisosDisponibles.Nodes.Add(nodoRoles);
            tvPermisosDisponibles.Nodes.Add(nodoPatentes);
            tvPermisosDisponibles.ExpandAll();
        }

        private void LlenarTreeView(TreeView tv, List<ComponentePermiso> catalogo)
        {
            tv.Nodes.Clear();
            foreach (var comp in catalogo)
            {
                TreeNode nodo = new TreeNode(comp.Nombre);
                nodo.Tag = comp;
                tv.Nodes.Add(nodo);
                AsignarColorNodo(nodo, comp);
                MostrarRecursivo(nodo, comp);
            }
            tv.ExpandAll();
        }

        private void MostrarRecursivo(TreeNode nodoPadre, ComponentePermiso componente)
        {
            if (componente.ObtenerHijos() == null) return;
            foreach (var hijo in componente.ObtenerHijos())
            {
                TreeNode nodoHijo = new TreeNode(hijo.Nombre);
                nodoHijo.Tag = hijo;
                AsignarColorNodo(nodoHijo, hijo);
                nodoPadre.Nodes.Add(nodoHijo);
                MostrarRecursivo(nodoHijo, hijo);
            }
        }

        private void CargarUsuarios()
        {
            cmbUsuarios.DataSource = usuarioBLL.ListarTodos();
            cmbUsuarios.DisplayMember = "Nombre";
            cmbUsuarios.ValueMember = "ID";
        }

        private void AsignarColorNodo(TreeNode nodo, ComponentePermiso componente)
        {
            if (componente is FamiliaBE)
            {
                nodo.ForeColor = Color.Blue;
                nodo.NodeFont = new Font(tvRoles.Font, FontStyle.Bold);
            }
            else if (componente is PatenteBE)
            {
                nodo.ForeColor = Color.DarkGreen;
            }
        }

        private void btnCrearFamilia_Click(object sender, EventArgs e)
        {
            string nombreRol = Microsoft.VisualBasic.Interaction.InputBox(T("msgInputNuevoRol", "Nombre de la nueva familia (Rol):"), T("titInputNuevoRol", "Nuevo Rol"), "");
            if (!string.IsNullOrWhiteSpace(nombreRol))
            {
                FamiliaBE nuevaFamilia = new FamiliaBE { Nombre = nombreRol.Trim() };
                try
                {
                    permisoBLL.GuardarFamiliaCompleta(nuevaFamilia);
                    MessageBox.Show(T("msgRolCreado", "Rol creado. Ahora selecciónelo y añada permisos."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarArbolRoles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            if (tvRoles.SelectedNode == null) return;
            if (tvRoles.SelectedNode.Parent != null &&
                tvRoles.SelectedNode.Parent.Tag is FamiliaBE familiaPadre &&
                tvRoles.SelectedNode.Tag is ComponentePermiso componenteHijo)
            {
                DialogResult resp = MessageBox.Show(
                    $"{T("msgConfirmarQuitarHijoRol", "¿Desea quitar el permiso o sub-rol seleccionado del rol padre?")}\r\n\r\n({componenteHijo.Nombre} -> {familiaPadre.Nombre})",
                    T("titConfirmarQuitarHijo", "Quitar Permiso de Rol"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    try
                    {
                        familiaPadre.RemoverHijo(componenteHijo);
                        permisoBLL.GuardarFamiliaCompleta(familiaPadre);
                        CargarArbolRoles();
                        cmbUsuarios_SelectedIndexChanged(null, null);
                        NotificarPosibleCambioEnUsuarioActual();
                        MessageBox.Show(T("msgHijoQuitadoExito", "El permiso fue removido correctamente del rol."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        CargarArbolRoles();
                        MessageBox.Show(T(ex.Message, ex.Message), T("titEliminacionDenegada", "Eliminación denegada"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                return;
            }
            if (tvRoles.SelectedNode.Tag is FamiliaBE fam)
            {
                try
                {
                    permisoBLL.EliminarFamilia(fam);
                    CargarArbolRoles();
                    MessageBox.Show(T("msgRolEliminado", "Rol eliminado del sistema."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(T(ex.Message, ex.Message), T("titEliminacionDenegada", "Eliminación denegada"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void cmbUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsuarios.SelectedItem is UsuarioBE user)
            {
                permisoBLL.LlenarPermisosDeUsuario(user);
                tvPermisosUsuario.Nodes.Clear();
                foreach (var p in user.Permisos)
                {
                    TreeNode nodo = new TreeNode(p.Nombre);
                    nodo.Tag = p;
                    AsignarColorNodo(nodo, p);
                    tvPermisosUsuario.Nodes.Add(nodo);
                    if (p is FamiliaBE) MostrarRecursivo(nodo, p);
                }
                tvPermisosUsuario.ExpandAll();
            }
        }

        private void btnAsignarRolUsuario_Click(object sender, EventArgs e)
        {
            if (cmbUsuarios.SelectedItem is UsuarioBE user && tvRoles.SelectedNode != null)
            {
                ComponentePermiso rolAsignar = (ComponentePermiso)tvRoles.SelectedNode.Tag;
                if (!user.Permisos.Exists(x => x.ID == rolAsignar.ID))
                {
                    user.Permisos.Add(rolAsignar);
                    try
                    {
                        permisoBLL.ActualizarPermisosUsuario(user);
                        cmbUsuarios_SelectedIndexChanged(null, null);
                        NotificarPosibleCambioEnUsuarioActual();
                    }
                    catch (Exception ex)
                    {
                        user.Permisos.Remove(rolAsignar);
                        MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cmbUsuarios_SelectedIndexChanged(null, null);
                    }
                }
            }
        }

        private void btnQuitarRolUsuario_Click(object sender, EventArgs e)
        {
            if (cmbUsuarios.SelectedItem is UsuarioBE user && tvPermisosUsuario.SelectedNode != null)
            {
                if (tvPermisosUsuario.SelectedNode.Parent != null)
                {
                    MessageBox.Show(
                        T("msgPermisoHeredadoNoQuitar", "El permiso seleccionado es heredado de un Rol. Para quitarlo del usuario debe seleccionar el nodo del Rol raíz (en azul), o bien modificar la estructura del Rol en el árbol izquierdo."),
                        T("titAtencion", "Atención"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                string nombreQuitar = tvPermisosUsuario.SelectedNode.Text;
                var permisoAQuitar = user.Permisos.FirstOrDefault(x => x.Nombre == nombreQuitar);
                if (permisoAQuitar != null)
                {
                    user.Permisos.Remove(permisoAQuitar);
                    try
                    {
                        permisoBLL.ActualizarPermisosUsuario(user);
                        cmbUsuarios_SelectedIndexChanged(null, null);
                        NotificarPosibleCambioEnUsuarioActual();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(T(ex.Message, ex.Message), T("titErrorValidacion", "Error de validación"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cmbUsuarios_SelectedIndexChanged(null, null);
                    }
                }
            }
        }

        private void btnAsignarPatenteARol_Click(object sender, EventArgs e)
        {
            if (tvRoles.SelectedNode == null || tvPermisosDisponibles.SelectedNode == null)
            {
                MessageBox.Show(T("msgSeleccionarDestino", "Debe seleccionar un rol destino en el panel izquierdo y un permiso/rol origen en el panel derecho."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tvPermisosDisponibles.SelectedNode.Tag == null)
            {
                MessageBox.Show(T("msgSeleccionarPermiso", "Por favor, expanda las carpetas y seleccione un permiso individual."), T("titAccionDenegada", "Acción Denegada"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ComponentePermiso destino = (ComponentePermiso)tvRoles.SelectedNode.Tag;
            ComponentePermiso origen = (ComponentePermiso)tvPermisosDisponibles.SelectedNode.Tag;
            if (destino is PatenteBE)
            {
                MessageBox.Show(T("msgProhibidoPatente", "Prohibido: El nodo destino es un Permiso Simple (Patente). Las patentes no pueden contener otros permisos. Debe seleccionar un Rol (Familia) en color azul."), T("titAccionDenegada", "Acción Denegada"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FamiliaBE familiaDestino = (FamiliaBE)destino;
            if (familiaDestino.ObtenerHijos().Any(h => h.ID == origen.ID))
            {
                MessageBox.Show(T("msgPermisoYaExisteEnRol", "El rol seleccionado ya posee asignado este permiso o sub-rol."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                familiaDestino.AgregarHijo(origen);
                permisoBLL.GuardarFamiliaCompleta(familiaDestino);
                CargarArbolRoles();
                cmbUsuarios_SelectedIndexChanged(null, null);
                NotificarPosibleCambioEnUsuarioActual();
                MessageBox.Show(T("msgPermisoAsignadoExito1", "El permiso/rol '") + origen.Nombre + T("msgPermisoAsignadoExito2", "' se asignó correctamente a '") + familiaDestino.Nombre + "'.", T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                familiaDestino.RemoverHijo(origen);
                MessageBox.Show(T(ex.Message, ex.Message), T("titErrorValidacion", "Error de validación"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCrearRolAnidado_Click(object sender, EventArgs e)
        {
            if (tvRoles.SelectedNode == null)
            {
                MessageBox.Show(T("msgSeleccionarPadre", "Debe seleccionar un rol padre en el árbol."), T("titAtencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ComponentePermiso nodoSeleccionado = (ComponentePermiso)tvRoles.SelectedNode.Tag;
            if (nodoSeleccionado is PatenteBE)
            {
                MessageBox.Show(T("msgAnidarPatente", "No puede anidar un rol dentro de un permiso simple."), T("titAccionDenegada", "Acción Denegada"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FamiliaBE familiaPadre = (FamiliaBE)nodoSeleccionado;
            string nombreNuevoRol = Microsoft.VisualBasic.Interaction.InputBox(T("msgInputRolAnidado", "Nombre del nuevo sub-rol para '") + familiaPadre.Nombre + "':", T("titInputRolAnidado", "Nuevo Rol Anidado"), "");
            if (!string.IsNullOrWhiteSpace(nombreNuevoRol))
            {
                try
                {
                    permisoBLL.CrearRolDentroDeRol(familiaPadre, nombreNuevoRol);
                    CargarArbolRoles();
                    cmbUsuarios_SelectedIndexChanged(null, null);
                    NotificarPosibleCambioEnUsuarioActual();
                    MessageBox.Show(T("msgSubRolExito", "Sub-rol creado y asignado con éxito."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void NotificarPosibleCambioEnUsuarioActual()
        {
            if (SessionManager.Instancia.UsuarioActual != null)
            {
                UsuarioBE usuarioLogueado = SessionManager.Instancia.UsuarioActual;
                permisoBLL.LlenarPermisosDeUsuario(usuarioLogueado);
                SessionManager.Instancia.ActualizarUsuarioEnSesion(usuarioLogueado);
            }
        }
    }
}