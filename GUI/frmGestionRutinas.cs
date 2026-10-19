using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmGestionRutinas : Form, IObserverIdioma
    {
        RutinaEntrenamientoBLL rutinaBLL = new RutinaEntrenamientoBLL();
        SocioBLL socioBLL = new SocioBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();
        RutinaEntrenamientoBE rutinaSeleccionada = null;
        SocioBE socioConsultado = null;
        BindingList<EjercicioBE> _ejerciciosBinding = new BindingList<EjercicioBE>();
        List<PuntoEvolucionBE> _serieEvolucionFuerza = new List<PuntoEvolucionBE>();
        int _varianteSugerencia = 0;
        string _textoBaseDetalle = "Detalle";

        public frmGestionRutinas()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmGestionRutinas_Load(object sender, EventArgs e)
        {
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnlMapaCalor, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnlGraficoFuerza, new object[] { true });
            dgvRutinas.CellFormatting += DgvRutinas_CellFormatting;
            cmbObjetivo.Items.Clear();
            foreach (var obj in rutinaBLL.ObtenerObjetivosDisponibles())
            {
                cmbObjetivo.Items.Add(obj);
            }
            ConfigurarGrillaEjercicios();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            LimpiarYDesactivarEdicion();
            ActualizarTagSinergiaNutricional();
            pnlMapaCalor.Invalidate();
        }

        private void DgvRutinas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRutinas.Rows[e.RowIndex].DataBoundItem is RutinaEntrenamientoBE rut)
            {
                if (rut.EsVigente)
                {
                    dgvRutinas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                    dgvRutinas.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else
                {
                    dgvRutinas.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
                    dgvRutinas.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                }
            }
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
            dgvEjercicios.CurrentCellDirtyStateChanged += (s, ev) =>
            {
                if (dgvEjercicios.IsCurrentCellDirty && dgvEjercicios.CurrentCell is DataGridViewComboBoxCell)
                {
                    dgvEjercicios.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
            dgvEjercicios.CellValueChanged += (s, ev) => pnlMapaCalor.Invalidate();
            dgvEjercicios.RowsRemoved += (s, ev) => pnlMapaCalor.Invalidate();
            dgvEjercicios.UserAddedRow += (s, ev) => pnlMapaCalor.Invalidate();
            dgvEjercicios.DataSource = _ejerciciosBinding;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            var traducciones = idiomaBLL.ObtenerTraducciones(idioma, this.Name);
            if (traducciones.ContainsKey("lblDNI")) lblDNI.Text = traducciones["lblDNI"];
            if (traducciones.ContainsKey("btnBuscar")) btnBuscar.Text = traducciones["btnBuscar"];
            if (traducciones.ContainsKey("lblObjetivo")) lblObjetivo.Text = traducciones["lblObjetivo"];
            if (traducciones.ContainsKey("lblFrecuencia")) lblFrecuencia.Text = traducciones["lblFrecuencia"];
            if (traducciones.ContainsKey("lblDetalle"))
            {
                _textoBaseDetalle = traducciones["lblDetalle"];
                lblDetalle.Text = _textoBaseDetalle;
            }
            if (traducciones.ContainsKey("btnModificar")) btnModificar.Text = traducciones["btnModificar"];
            if (traducciones.ContainsKey("btnEliminar")) btnEliminar.Text = traducciones["btnEliminar"];
            this.Text = traducciones.ContainsKey("titGestionRutinas") ? traducciones["titGestionRutinas"] : "Gestión de Rutinas";
            if (dgvRutinas.Columns["CodigoRutina"] != null && traducciones.ContainsKey("colCod")) dgvRutinas.Columns["CodigoRutina"].HeaderText = traducciones["colCod"];
            if (dgvRutinas.Columns["FechaCreacion"] != null && traducciones.ContainsKey("colFecha")) dgvRutinas.Columns["FechaCreacion"].HeaderText = traducciones["colFecha"];
            if (dgvRutinas.Columns["ObjetivoFisico"] != null && traducciones.ContainsKey("colObj")) dgvRutinas.Columns["ObjetivoFisico"].HeaderText = traducciones["colObj"];
            if (dgvRutinas.Columns["FrecuenciaSemanal"] != null && traducciones.ContainsKey("colFrec")) dgvRutinas.Columns["FrecuenciaSemanal"].HeaderText = traducciones["colFrec"];
            if (dgvRutinas.Columns["EstadoVigencia"] != null) dgvRutinas.Columns["EstadoVigencia"].HeaderText = "Estado";
        }

        private void ActualizarTagSinergiaNutricional()
        {
            int idSocio = 0;
            if (rutinaSeleccionada != null && rutinaSeleccionada.Socio != null && rutinaSeleccionada.Socio.ID_Socio > 0)
            {
                idSocio = rutinaSeleccionada.Socio.ID_Socio;
            }
            else if (socioConsultado != null && socioConsultado.ID_Socio > 0)
            {
                idSocio = socioConsultado.ID_Socio;
            }
            if (idSocio <= 0)
            {
                lblAlertaNutricional.Text = "ℹ️ SINERGIA INTER-ÁREAS: Busque las rutinas de un socio para consultar su estado clínico-nutricional.";
                lblAlertaNutricional.BackColor = Color.FromArgb(241, 245, 249);
                lblAlertaNutricional.ForeColor = Color.FromArgb(71, 85, 105);
                return;
            }
            string objetivo = !string.IsNullOrWhiteSpace(cmbObjetivo.Text) ? cmbObjetivo.Text : "Hipertrofia";
            int frecuencia = (int)numFrecuencia.Value;
            string mensaje = rutinaBLL.EvaluarSinergiaNutricional(idSocio, objetivo, frecuencia, out int severidad);
            lblAlertaNutricional.Text = mensaje;
            if (severidad == 3)
            {
                lblAlertaNutricional.BackColor = Color.FromArgb(254, 226, 226);
                lblAlertaNutricional.ForeColor = Color.FromArgb(153, 27, 27);
            }
            else if (severidad == 2)
            {
                lblAlertaNutricional.BackColor = Color.FromArgb(254, 243, 199);
                lblAlertaNutricional.ForeColor = Color.FromArgb(146, 64, 14);
            }
            else if (severidad == 1)
            {
                lblAlertaNutricional.BackColor = Color.FromArgb(220, 252, 231);
                lblAlertaNutricional.ForeColor = Color.FromArgb(22, 101, 52);
            }
            else
            {
                lblAlertaNutricional.BackColor = Color.FromArgb(241, 245, 249);
                lblAlertaNutricional.ForeColor = Color.FromArgb(71, 85, 105);
            }
        }

        private void LimpiarYDesactivarEdicion()
        {
            cmbObjetivo.Enabled = false;
            numFrecuencia.Enabled = false;
            dgvEjercicios.Enabled = false;
            btnSugerirPlantilla.Enabled = false;
            btnQuitarEjercicio.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            cmbObjetivo.Text = string.Empty;
            _ejerciciosBinding.Clear();
            numFrecuencia.Value = 1;
            lblDetalle.Text = _textoBaseDetalle;
            lblDetalle.ForeColor = Color.FromArgb(31, 41, 55);
            pnlMapaCalor.Invalidate();
        }

        private void ConfigurarModoSegunVigencia(bool esRutinaVigente)
        {
            bool cuotaAlDia = false;
            string msgVigencia = string.Empty;
            if (socioConsultado != null)
            {
                cuotaAlDia = socioBLL.ObtenerEstadoVigenciaPlan(
                    socioConsultado,
                    out _,
                    out _,
                    out _,
                    out msgVigencia);
            }
            bool permitirEdicion = esRutinaVigente && cuotaAlDia;
            dgvEjercicios.Enabled = true;
            dgvEjercicios.ReadOnly = !permitirEdicion;
            dgvEjercicios.AllowUserToAddRows = permitirEdicion;
            dgvEjercicios.AllowUserToDeleteRows = permitirEdicion;
            cmbObjetivo.Enabled = permitirEdicion;
            numFrecuencia.Enabled = permitirEdicion;
            btnSugerirPlantilla.Enabled = permitirEdicion;
            btnQuitarEjercicio.Enabled = permitirEdicion;
            btnModificar.Enabled = permitirEdicion;
            btnEliminar.Enabled = permitirEdicion;
            if (!cuotaAlDia && socioConsultado != null)
            {
                lblDetalle.Text = $"{_textoBaseDetalle} — [EDICIÓN BLOQUEADA: {msgVigencia}]";
                lblDetalle.ForeColor = Color.FromArgb(185, 28, 28);
            }
            else if (esRutinaVigente)
            {
                lblDetalle.Text = $"{_textoBaseDetalle} — [RUTINA VIGENTE: Edición Habilitada]";
                lblDetalle.ForeColor = Color.FromArgb(22, 101, 52);
            }
            else
            {
                lblDetalle.Text = $"{_textoBaseDetalle} — [RUTINA HISTÓRICA INMUTABLE: Solo Lectura para Trazabilidad]";
                lblDetalle.ForeColor = Color.FromArgb(180, 83, 9);
            }
        }

        private void CargarRutinaEnControles(RutinaEntrenamientoBE rutina)
        {
            if (rutina == null)
            {
                LimpiarYDesactivarEdicion();
                return;
            }
            rutinaSeleccionada = rutina;
            cmbObjetivo.Text = rutina.ObjetivoFisico;
            numFrecuencia.Value = Math.Clamp(rutina.FrecuenciaSemanal, (int)numFrecuencia.Minimum, (int)numFrecuencia.Maximum);
            var copiaEjercicios = rutina.Ejercicios
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
            _ejerciciosBinding.ListChanged += (s, ev) => pnlMapaCalor.Invalidate();
            dgvEjercicios.DataSource = _ejerciciosBinding;
            ConfigurarModoSegunVigencia(rutina.EsVigente);
            ActualizarTagSinergiaNutricional();
            pnlMapaCalor.Invalidate();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarRutinas();
        }

        private void BuscarRutinas()
        {
            try
            {
                socioConsultado = socioBLL.ConsultarSocio(txtDNI.Text.Trim());
                var lista = rutinaBLL.ConsultarRutinasPorDNI(txtDNI.Text.Trim());
                dgvRutinas.DataSource = null;
                dgvRutinas.DataSource = lista;
                if (dgvRutinas.Columns["DVH"] != null) dgvRutinas.Columns["DVH"].Visible = false;
                if (dgvRutinas.Columns["DetalleEjercicios"] != null) dgvRutinas.Columns["DetalleEjercicios"].Visible = false;
                if (dgvRutinas.Columns["Socio"] != null) dgvRutinas.Columns["Socio"].Visible = false;
                if (dgvRutinas.Columns["Entrenador"] != null) dgvRutinas.Columns["Entrenador"].Visible = false;
                if (dgvRutinas.Columns["ID_SocioParaDVH"] != null) dgvRutinas.Columns["ID_SocioParaDVH"].Visible = false;
                if (dgvRutinas.Columns["ID_UsuarioParaDVH"] != null) dgvRutinas.Columns["ID_UsuarioParaDVH"].Visible = false;
                ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
                _serieEvolucionFuerza = rutinaBLL.ObtenerEvolucionFuerza(lista);
                pnlGraficoFuerza.Invalidate();
                if (lista.Count > 0 && dgvRutinas.Rows.Count > 0)
                {
                    dgvRutinas.ClearSelection();
                    dgvRutinas.Rows[0].Selected = true;
                    DataGridViewColumn primeraColVisible = dgvRutinas.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                    if (primeraColVisible != null)
                    {
                        dgvRutinas.CurrentCell = dgvRutinas.Rows[0].Cells[primeraColVisible.Index];
                    }
                    CargarRutinaEnControles(lista[0]);
                }
                else
                {
                    rutinaSeleccionada = null;
                    LimpiarYDesactivarEdicion();
                    ActualizarTagSinergiaNutricional();
                }
            }
            catch (Exception ex)
            {
                socioConsultado = null;
                rutinaSeleccionada = null;
                _serieEvolucionFuerza.Clear();
                pnlGraficoFuerza.Invalidate();
                LimpiarYDesactivarEdicion();
                ActualizarTagSinergiaNutricional();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvRutinas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRutinas.Rows[e.RowIndex].DataBoundItem is RutinaEntrenamientoBE rut)
            {
                CargarRutinaEnControles(rut);
            }
        }

        private void cmbObjetivo_Changed(object sender, EventArgs e)
        {
            if (rutinaSeleccionada != null)
            {
                ActualizarTagSinergiaNutricional();
            }
        }

        private void numFrecuencia_ValueChanged(object sender, EventArgs e)
        {
            if (rutinaSeleccionada != null)
            {
                ActualizarTagSinergiaNutricional();
            }
        }

        private void btnSugerirPlantilla_Click(object sender, EventArgs e)
        {
            if (rutinaSeleccionada == null || !rutinaSeleccionada.EsVigente) return;
            _varianteSugerencia = (_varianteSugerencia + 1) % 3;
            string objetivo = !string.IsNullOrWhiteSpace(cmbObjetivo.Text) ? cmbObjetivo.Text : "Hipertrofia";
            int frecuencia = (int)numFrecuencia.Value;
            dgvEjercicios.CancelEdit();
            dgvEjercicios.EndEdit();
            var sugeridos = rutinaBLL.GenerarRutinaSugerida(objetivo, frecuencia, _varianteSugerencia);
            _ejerciciosBinding = new BindingList<EjercicioBE>(sugeridos);
            _ejerciciosBinding.ListChanged += (s, ev) => pnlMapaCalor.Invalidate();
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = _ejerciciosBinding;
            dgvEjercicios.Refresh();
            ActualizarTagSinergiaNutricional();
            pnlMapaCalor.Invalidate();
        }

        private void btnQuitarEjercicio_Click(object sender, EventArgs e)
        {
            if (rutinaSeleccionada == null || !rutinaSeleccionada.EsVigente) return;
            if (dgvEjercicios.CurrentRow != null && !dgvEjercicios.CurrentRow.IsNewRow)
            {
                dgvEjercicios.Rows.Remove(dgvEjercicios.CurrentRow);
                pnlMapaCalor.Invalidate();
            }
        }

        private void pnlGraficoFuerza_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            using (Font fontTitulo = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (Font fontEje = new Font("Segoe UI", 7.5F, FontStyle.Regular))
            using (Font fontPunto = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            using (Brush brushTexto = new SolidBrush(Color.FromArgb(31, 41, 55)))
            using (Brush brushGris = new SolidBrush(Color.FromArgb(100, 116, 139)))
            using (Pen penGrilla = new Pen(Color.FromArgb(226, 232, 240), 1f) { DashStyle = DashStyle.Dash })
            using (Pen penEje = new Pen(Color.FromArgb(148, 163, 184), 1.2f))
            using (Pen penCurva = new Pen(Color.FromArgb(16, 185, 129), 2.5f))
            {
                g.DrawString("Evolución de Fuerza (Carga Promedio en kg)", fontTitulo, brushTexto, 12, 8);
                if (_serieEvolucionFuerza == null || _serieEvolucionFuerza.Count == 0)
                {
                    g.DrawString("Busque un socio con rutinas para ver su progreso de fuerza.", fontEje, brushGris, 14, 95);
                    return;
                }
                decimal cargaIni = _serieEvolucionFuerza.First().ValorPrimario;
                decimal cargaFin = _serieEvolucionFuerza.Last().ValorPrimario;
                decimal delta = cargaFin - cargaIni;
                string signo = delta > 0 ? "+" : "";
                string textoDelta = $"Δ Carga: {signo}{delta:0.0} kg";
                SizeF tamDelta = g.MeasureString(textoDelta, fontPunto);
                g.DrawString(textoDelta, fontPunto, brushTexto, pnlGraficoFuerza.Width - tamDelta.Width - 12, 8);
                int izq = 44, der = pnlGraficoFuerza.Width - 22, top = 36, bot = pnlGraficoFuerza.Height - 28;
                int ancho = Math.Max(10, der - izq);
                int alto = Math.Max(10, bot - top);
                decimal minVal = _serieEvolucionFuerza.Min(p => p.ValorPrimario);
                decimal maxVal = _serieEvolucionFuerza.Max(p => p.ValorPrimario);
                if (minVal == maxVal)
                {
                    minVal = Math.Max(0m, minVal - 5m);
                    maxVal += 5m;
                }
                else
                {
                    decimal margen = Math.Max(1m, (maxVal - minVal) * 0.2m);
                    minVal = Math.Max(0m, minVal - margen);
                    maxVal += margen;
                }

                for (int i = 0; i <= 3; i++)
                {
                    float y = top + (alto * i / 3f);
                    decimal valRef = maxVal - ((maxVal - minVal) * i / 3m);
                    g.DrawLine(penGrilla, izq, y, der, y);
                    g.DrawString($"{valRef:0.#}", fontEje, brushGris, 4, y - 7);
                }
                g.DrawLine(penEje, izq, top, izq, bot);
                g.DrawLine(penEje, izq, bot, der, bot);
                int n = _serieEvolucionFuerza.Count;
                PointF[] pts = new PointF[n];
                for (int i = 0; i < n; i++)
                {
                    float x = n == 1 ? izq + (ancho / 2f) : izq + (ancho * i / (float)(n - 1));
                    float ratio = (float)((_serieEvolucionFuerza[i].ValorPrimario - minVal) / (maxVal - minVal));
                    float y = bot - (ratio * alto);
                    pts[i] = new PointF(x, y);
                }
                if (n > 1)
                {
                    using (GraphicsPath pathArea = new GraphicsPath())
                    {
                        pathArea.AddLines(pts);
                        pathArea.AddLine(pts[n - 1].X, bot, pts[0].X, bot);
                        pathArea.CloseFigure();
                        using (LinearGradientBrush brushArea = new LinearGradientBrush(
                            new Rectangle(izq, top, ancho, alto),
                            Color.FromArgb(90, 16, 185, 129),
                            Color.FromArgb(10, 16, 185, 129),
                            LinearGradientMode.Vertical))
                        {
                            g.FillPath(brushArea, pathArea);
                        }
                    }
                    g.DrawLines(penCurva, pts);
                }
                StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center };
                using (Brush brushPunto = new SolidBrush(Color.FromArgb(16, 185, 129)))
                using (Brush brushBlanco = new SolidBrush(Color.White))
                {
                    for (int i = 0; i < n; i++)
                    {
                        g.FillEllipse(brushBlanco, pts[i].X - 5, pts[i].Y - 5, 10, 10);
                        g.FillEllipse(brushPunto, pts[i].X - 4, pts[i].Y - 4, 8, 8);
                        g.DrawString($"{_serieEvolucionFuerza[i].ValorPrimario:0.0}kg", fontPunto, brushTexto, pts[i].X, pts[i].Y - 17, sfCenter);
                        g.DrawString(_serieEvolucionFuerza[i].EtiquetaFecha, fontEje, brushGris, pts[i].X, bot + 4, sfCenter);
                    }
                }
            }
        }

        private void pnlMapaCalor_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            var niveles = rutinaBLL.CalcularNivelCalorMuscular(_ejerciciosBinding);
            var series = rutinaBLL.CalcularSeriesPorGrupoMuscular(_ejerciciosBinding);
            Color ObtenerColorMusculo(string grupo)
            {
                int nivel = niveles.ContainsKey(grupo) ? niveles[grupo] : 0;
                if (nivel == 2) return Color.FromArgb(220, 38, 38);
                if (nivel == 1) return Color.FromArgb(249, 115, 22);
                return Color.FromArgb(203, 213, 225);
            }
            using (Font fontSub = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (Font fontLeyenda = new Font("Segoe UI", 7.2F, FontStyle.Regular))
            using (Font fontBadge = new Font("Segoe UI", 7.2F, FontStyle.Bold))
            using (Pen penBorde = new Pen(Color.FromArgb(71, 85, 105), 1.4f))
            using (Brush brushTexto = new SolidBrush(Color.FromArgb(31, 41, 55)))
            using (Brush brushCabeza = new SolidBrush(Color.FromArgb(226, 232, 240)))
            {
                int cxFrente = 78;
                int cxDorso = 224;
                int yBase = 24;
                StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString("FRENTE", fontSub, brushTexto, cxFrente, 5, sfCenter);
                g.DrawString("DORSO", fontSub, brushTexto, cxDorso, 5, sfCenter);
                g.FillEllipse(brushCabeza, cxFrente - 12, yBase, 24, 24);
                g.DrawEllipse(penBorde, cxFrente - 12, yBase, 24, 24);
                g.FillRectangle(brushCabeza, cxFrente - 5, yBase + 23, 10, 7);
                using (Brush bHombros = new SolidBrush(ObtenerColorMusculo("Hombros")))
                {
                    g.FillEllipse(bHombros, cxFrente - 38, yBase + 28, 18, 18);
                    g.DrawEllipse(penBorde, cxFrente - 38, yBase + 28, 18, 18);
                    g.FillEllipse(bHombros, cxFrente + 20, yBase + 28, 18, 18);
                    g.DrawEllipse(penBorde, cxFrente + 20, yBase + 28, 18, 18);
                }
                using (Brush bPecho = new SolidBrush(ObtenerColorMusculo("Pecho")))
                {
                    Rectangle rectPechoIzq = new Rectangle(cxFrente - 22, yBase + 30, 21, 23);
                    Rectangle rectPechoDer = new Rectangle(cxFrente + 1, yBase + 30, 21, 23);
                    g.FillRectangle(bPecho, rectPechoIzq);
                    g.DrawRectangle(penBorde, rectPechoIzq);
                    g.FillRectangle(bPecho, rectPechoDer);
                    g.DrawRectangle(penBorde, rectPechoDer);
                }
                using (Brush bCore = new SolidBrush(ObtenerColorMusculo("Core")))
                {
                    Rectangle rectCore = new Rectangle(cxFrente - 18, yBase + 55, 36, 40);
                    g.FillRectangle(bCore, rectCore);
                    g.DrawRectangle(penBorde, rectCore);
                    g.DrawLine(penBorde, cxFrente, yBase + 55, cxFrente, yBase + 95);
                    g.DrawLine(penBorde, cxFrente - 18, yBase + 68, cxFrente + 18, yBase + 68);
                    g.DrawLine(penBorde, cxFrente - 18, yBase + 81, cxFrente + 18, yBase + 81);
                }
                using (Brush bBiceps = new SolidBrush(ObtenerColorMusculo("Bíceps")))
                {
                    Rectangle bicIzq = new Rectangle(cxFrente - 39, yBase + 47, 14, 30);
                    Rectangle bicDer = new Rectangle(cxFrente + 25, yBase + 47, 14, 30);
                    g.FillRectangle(bBiceps, bicIzq);
                    g.DrawRectangle(penBorde, bicIzq);
                    g.FillRectangle(bBiceps, bicDer);
                    g.DrawRectangle(penBorde, bicDer);
                }
                g.FillRectangle(brushCabeza, cxFrente - 40, yBase + 79, 12, 24);
                g.DrawRectangle(penBorde, cxFrente - 40, yBase + 79, 12, 24);
                g.FillRectangle(brushCabeza, cxFrente + 28, yBase + 79, 12, 24);
                g.DrawRectangle(penBorde, cxFrente + 28, yBase + 79, 12, 24);
                using (Brush bPiernas = new SolidBrush(ObtenerColorMusculo("Piernas")))
                {
                    Rectangle cuadIzq = new Rectangle(cxFrente - 20, yBase + 97, 18, 46);
                    Rectangle cuadDer = new Rectangle(cxFrente + 2, yBase + 97, 18, 46);
                    g.FillRectangle(bPiernas, cuadIzq);
                    g.DrawRectangle(penBorde, cuadIzq);
                    g.FillRectangle(bPiernas, cuadDer);
                    g.DrawRectangle(penBorde, cuadDer);
                    Rectangle gemIzq = new Rectangle(cxFrente - 18, yBase + 146, 14, 38);
                    Rectangle gemDer = new Rectangle(cxFrente + 4, yBase + 146, 14, 38);
                    g.FillRectangle(bPiernas, gemIzq);
                    g.DrawRectangle(penBorde, gemIzq);
                    g.FillRectangle(bPiernas, gemDer);
                    g.DrawRectangle(penBorde, gemDer);
                }
                g.FillEllipse(brushCabeza, cxDorso - 12, yBase, 24, 24);
                g.DrawEllipse(penBorde, cxDorso - 12, yBase, 24, 24);
                g.FillRectangle(brushCabeza, cxDorso - 5, yBase + 23, 10, 7);
                using (Brush bHombros = new SolidBrush(ObtenerColorMusculo("Hombros")))
                {
                    g.FillEllipse(bHombros, cxDorso - 38, yBase + 28, 18, 18);
                    g.DrawEllipse(penBorde, cxDorso - 38, yBase + 28, 18, 18);
                    g.FillEllipse(bHombros, cxDorso + 20, yBase + 28, 18, 18);
                    g.DrawEllipse(penBorde, cxDorso + 20, yBase + 28, 18, 18);
                }
                using (Brush bEspalda = new SolidBrush(ObtenerColorMusculo("Espalda")))
                {
                    Point[] ptsEspalda = new Point[]
                    {
                        new Point(cxDorso - 22, yBase + 30),
                        new Point(cxDorso + 22, yBase + 30),
                        new Point(cxDorso + 16, yBase + 78),
                        new Point(cxDorso - 16, yBase + 78)
                    };
                    g.FillPolygon(bEspalda, ptsEspalda);
                    g.DrawPolygon(penBorde, ptsEspalda);
                    g.DrawLine(penBorde, cxDorso, yBase + 30, cxDorso, yBase + 78);
                }
                using (Brush bCore = new SolidBrush(ObtenerColorMusculo("Core")))
                {
                    Rectangle rectLumbar = new Rectangle(cxDorso - 16, yBase + 79, 32, 16);
                    g.FillRectangle(bCore, rectLumbar);
                    g.DrawRectangle(penBorde, rectLumbar);
                }
                using (Brush bTriceps = new SolidBrush(ObtenerColorMusculo("Tríceps")))
                {
                    Rectangle triIzq = new Rectangle(cxDorso - 39, yBase + 47, 14, 30);
                    Rectangle triDer = new Rectangle(cxDorso + 25, yBase + 47, 14, 30);
                    g.FillRectangle(bTriceps, triIzq);
                    g.DrawRectangle(penBorde, triIzq);
                    g.FillRectangle(bTriceps, triDer);
                    g.DrawRectangle(penBorde, triDer);
                }
                g.FillRectangle(brushCabeza, cxDorso - 40, yBase + 79, 12, 24);
                g.DrawRectangle(penBorde, cxDorso - 40, yBase + 79, 12, 24);
                g.FillRectangle(brushCabeza, cxDorso + 28, yBase + 79, 12, 24);
                g.DrawRectangle(penBorde, cxDorso + 28, yBase + 79, 12, 24);
                using (Brush bPiernas = new SolidBrush(ObtenerColorMusculo("Piernas")))
                {
                    Rectangle isqIzq = new Rectangle(cxDorso - 20, yBase + 97, 18, 46);
                    Rectangle isqDer = new Rectangle(cxDorso + 2, yBase + 97, 18, 46);
                    g.FillRectangle(bPiernas, isqIzq);
                    g.DrawRectangle(penBorde, isqIzq);
                    g.FillRectangle(bPiernas, isqDer);
                    g.DrawRectangle(penBorde, isqDer);
                    Rectangle gemIzq = new Rectangle(cxDorso - 18, yBase + 146, 14, 38);
                    Rectangle gemDer = new Rectangle(cxDorso + 4, yBase + 146, 14, 38);
                    g.FillRectangle(bPiernas, gemIzq);
                    g.DrawRectangle(penBorde, gemIzq);
                    g.FillRectangle(bPiernas, gemDer);
                    g.DrawRectangle(penBorde, gemDer);
                }
                int yLeyenda = 220;
                using (Brush bRojo = new SolidBrush(Color.FromArgb(220, 38, 38)))
                using (Brush bNaranja = new SolidBrush(Color.FromArgb(249, 115, 22)))
                using (Brush bGris = new SolidBrush(Color.FromArgb(203, 213, 225)))
                {
                    g.FillRectangle(bRojo, 10, yLeyenda, 10, 10);
                    g.DrawRectangle(penBorde, 10, yLeyenda, 10, 10);
                    g.DrawString("Primario (≥5s)", fontLeyenda, brushTexto, 23, yLeyenda - 2);
                    g.FillRectangle(bNaranja, 106, yLeyenda, 10, 10);
                    g.DrawRectangle(penBorde, 106, yLeyenda, 10, 10);
                    g.DrawString("Secundario (1-4s)", fontLeyenda, brushTexto, 119, yLeyenda - 2);
                    g.FillRectangle(bGris, 216, yLeyenda, 10, 10);
                    g.DrawRectangle(penBorde, 216, yLeyenda, 10, 10);
                    g.DrawString("Inactivo", fontLeyenda, brushTexto, 229, yLeyenda - 2);
                }
                string[] ordenGrupos = { "Pecho", "Espalda", "Piernas", "Hombros", "Bíceps", "Tríceps", "Core" };
                int col = 0;
                int fila = 0;
                int yInicioBadges = 242;
                foreach (string grp in ordenGrupos)
                {
                    int cantSeries = series.ContainsKey(grp) ? series[grp] : 0;
                    int xBadge = 10 + (col * 96);
                    int yBadge = yInicioBadges + (fila * 25);
                    Color colorFondo = ObtenerColorMusculo(grp);
                    Color colorTextoBadge = niveles.ContainsKey(grp) && niveles[grp] > 0 ? Color.White : Color.FromArgb(31, 41, 55);
                    Rectangle rectTag = new Rectangle(xBadge, yBadge, 90, 20);
                    using (Brush bTag = new SolidBrush(colorFondo))
                    using (Brush bTxt = new SolidBrush(colorTextoBadge))
                    {
                        g.FillRectangle(bTag, rectTag);
                        g.DrawRectangle(penBorde, rectTag);
                        g.DrawString($"{grp}: {cantSeries}s", fontBadge, bTxt, xBadge + 4, yBadge + 2);
                    }
                    col++;
                    if (col >= 3)
                    {
                        col = 0;
                        fila++;
                    }
                }
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
                MessageBox.Show("Rutina vigente modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var result = MessageBox.Show($"¿Seguro que desea eliminar la rutina vigente #{rutinaSeleccionada.CodigoRutina}?\r\nLa rutina anterior pasará a ser la vigente.", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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