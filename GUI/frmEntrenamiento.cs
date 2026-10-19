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
    public partial class frmEntrenamiento : Form, IObserverIdioma
    {
        SocioBLL socioBLL = new SocioBLL();
        RutinaEntrenamientoBLL rutinaBLL = new RutinaEntrenamientoBLL();
        IdiomaBLL idiomaBLL = new IdiomaBLL();
        SocioBE socioActual = null;
        BindingList<EjercicioBE> _ejerciciosBinding = new BindingList<EjercicioBE>();
        List<RutinaEntrenamientoBE> _rutinasHistoricasSocio = new List<RutinaEntrenamientoBE>();
        List<PuntoEvolucionBE> _serieEvolucionFuerza = new List<PuntoEvolucionBE>();
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
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnlMapaCalor, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnlGraficoFuerza, new object[] { true });
            CargarCombosIniciales();
            ConfigurarGrillaEjercicios();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            BloquearControlesRutina(true);
            _cargandoInicial = false;
            RefrescarVisualizacionesEntrenamiento();
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
            dgvEjercicios.CurrentCellDirtyStateChanged += (s, ev) =>
            {
                if (dgvEjercicios.IsCurrentCellDirty && dgvEjercicios.CurrentCell is DataGridViewComboBoxCell)
                {
                    dgvEjercicios.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
            dgvEjercicios.CellValueChanged += (s, ev) => RefrescarVisualizacionesEntrenamiento();
            dgvEjercicios.RowsRemoved += (s, ev) => RefrescarVisualizacionesEntrenamiento();
            dgvEjercicios.UserAddedRow += (s, ev) => RefrescarVisualizacionesEntrenamiento();
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

        private void RefrescarVisualizacionesEntrenamiento()
        {
            pnlMapaCalor.Invalidate();
            ActualizarDatosGraficoFuerza();
            pnlGraficoFuerza.Invalidate();
        }

        private void ActualizarDatosGraficoFuerza()
        {
            if (socioActual == null || !socioActual.EstadoActivo)
            {
                _serieEvolucionFuerza = new List<PuntoEvolucionBE>();
                return;
            }
            var puntos = rutinaBLL.ObtenerEvolucionFuerza(_rutinasHistoricasSocio);
            if (_ejerciciosBinding != null && _ejerciciosBinding.Count > 0)
            {
                var conCarga = _ejerciciosBinding
                    .Where(e => e != null && !string.IsNullOrWhiteSpace(e.Nombre) && e.CargaSugeridaKg > 0)
                    .ToList();
                if (conCarga.Count > 0)
                {
                    decimal cargaPromNueva = Math.Round(conCarga.Average(e => e.CargaSugeridaKg), 1);
                    decimal volNuevo = Math.Round(_ejerciciosBinding
                        .Where(e => e != null && !string.IsNullOrWhiteSpace(e.Nombre))
                        .Sum(e => e.Series * e.CargaSugeridaKg), 0);
                    puntos.Add(new PuntoEvolucionBE
                    {
                        Fecha = DateTime.Now,
                        EtiquetaFecha = "Nueva (Actual)",
                        ValorPrimario = cargaPromNueva,
                        ValorSecundario = volNuevo,
                        Detalle = $"{cargaPromNueva:0.0} kg prom."
                    });
                }
            }
            _serieEvolucionFuerza = puntos;
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
            _ejerciciosBinding.ListChanged += (s, ev) => RefrescarVisualizacionesEntrenamiento();
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = _ejerciciosBinding;
            dgvEjercicios.Refresh();
            RefrescarVisualizacionesEntrenamiento();
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

        private void ActualizarTagSinergiaNutricional()
        {
            if (socioActual == null || !socioActual.EstadoActivo)
            {
                lblAlertaNutricional.Text = "ℹ️ SINERGIA INTER-ÁREAS: Valide un socio activo para consultar su estado clínico-nutricional.";
                lblAlertaNutricional.BackColor = Color.FromArgb(241, 245, 249);
                lblAlertaNutricional.ForeColor = Color.FromArgb(71, 85, 105);
                return;
            }
            string objetivo = cmbObjetivo.SelectedItem != null
                ? cmbObjetivo.SelectedItem.ToString()
                : (!string.IsNullOrWhiteSpace(cmbObjetivo.Text) ? cmbObjetivo.Text : "Hipertrofia");
            int frecuencia = (int)numFrecuencia.Value;
            string mensaje = rutinaBLL.EvaluarSinergiaNutricional(socioActual.ID_Socio, objetivo, frecuencia, out int severidad);
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                socioActual = socioBLL.ConsultarSocio(txtDNI.Text.Trim());
                if (socioActual != null)
                {
                    bool planVigente = socioBLL.ObtenerEstadoVigenciaPlan(
                        socioActual,
                        out DateTime? fechaVenc,
                        out string nomPlan,
                        out int diasRest,
                        out string msgVigencia);
                    if (!planVigente)
                    {
                        MessageBox.Show($"No se puede asignar una nueva rutina al socio {socioActual.Nombre} {socioActual.Apellido}:\r\n{msgVigencia}\r\n\r\nDerívelo a Recepción para renovar su membresía.", "Suscripción Vencida / Inactiva", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblInfoSocio.Text = $"Socio: {socioActual.Nombre} {socioActual.Apellido} — [{msgVigencia}]";
                        BloquearControlesRutina(true);
                        _rutinasHistoricasSocio.Clear();
                        _ejerciciosBinding.Clear();
                        ActualizarTagSinergiaNutricional();
                        RefrescarVisualizacionesEntrenamiento();
                        return;
                    }
                    lblInfoSocio.Text = $"Socio Validado: {socioActual.Nombre} {socioActual.Apellido} ({msgVigencia})";
                    _rutinasHistoricasSocio = rutinaBLL.ConsultarRutinasPorDNI(socioActual.DNI);
                    BloquearControlesRutina(false);
                    _varianteSugerencia = 0;
                    CargarRutinaSugeridaInteligente();
                    ActualizarTagSinergiaNutricional();
                }
                else
                {
                    MessageBox.Show("Socio no encontrado. Verifique el DNI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblInfoSocio.Text = "-";
                    socioActual = null;
                    _rutinasHistoricasSocio.Clear();
                    _ejerciciosBinding.Clear();
                    BloquearControlesRutina(true);
                    ActualizarTagSinergiaNutricional();
                    RefrescarVisualizacionesEntrenamiento();
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
                ActualizarTagSinergiaNutricional();
            }
        }

        private void numFrecuencia_ValueChanged(object sender, EventArgs e)
        {
            if (socioActual != null && socioActual.EstadoActivo)
            {
                CargarRutinaSugeridaInteligente();
                ActualizarTagSinergiaNutricional();
            }
        }

        private void btnSugerirRutina_Click(object sender, EventArgs e)
        {
            _varianteSugerencia = (_varianteSugerencia + 1) % 3;
            CargarRutinaSugeridaInteligente();
            char letraVariante = (char)('A' + _varianteSugerencia);
            if (socioActual != null)
            {
                lblInfoSocio.Text = $"Socio Validado: {socioActual.Nombre} {socioActual.Apellido} (Sugerencia: Variante {letraVariante})";
            }
        }

        private void btnQuitarEjercicio_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow != null && !dgvEjercicios.CurrentRow.IsNewRow)
            {
                dgvEjercicios.Rows.Remove(dgvEjercicios.CurrentRow);
                RefrescarVisualizacionesEntrenamiento();
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
                g.DrawString("Progresión de Fuerza del Socio (Historial + Rutina en Diseño)", fontTitulo, brushTexto, 12, 6);
                if (_serieEvolucionFuerza == null || _serieEvolucionFuerza.Count == 0)
                {
                    g.DrawString("Valide un socio para visualizar su progresión histórica de cargas (kg) y la proyección de la nueva rutina.", fontEje, brushGris, 14, 72);
                    return;
                }
                decimal cargaIni = _serieEvolucionFuerza.First().ValorPrimario;
                decimal cargaFin = _serieEvolucionFuerza.Last().ValorPrimario;
                decimal delta = cargaFin - cargaIni;
                string signo = delta > 0 ? "+" : "";
                string textoDelta = $" Rutinas previas: {_rutinasHistoricasSocio.Count} | Δ Proyectado: {signo}{delta:0.0} kg";
                SizeF tamDelta = g.MeasureString(textoDelta, fontPunto);
                g.DrawString(textoDelta, fontPunto, brushTexto, pnlGraficoFuerza.Width - tamDelta.Width - 12, 6);
                int izq = 46, der = pnlGraficoFuerza.Width - 36, top = 32, bot = pnlGraficoFuerza.Height - 24;
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
                    g.DrawString($"{valRef:0.#}", fontEje, brushGris, 6, y - 7);
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
                            Color.FromArgb(85, 16, 185, 129),
                            Color.FromArgb(10, 16, 185, 129),
                            LinearGradientMode.Vertical))
                        {
                            g.FillPath(brushArea, pathArea);
                        }
                    }
                    g.DrawLines(penCurva, pts);
                }
                StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center };
                using (Brush brushHistorico = new SolidBrush(Color.FromArgb(16, 185, 129)))
                using (Brush brushNuevo = new SolidBrush(Color.FromArgb(37, 99, 235)))
                using (Brush brushBlanco = new SolidBrush(Color.White))
                {
                    for (int i = 0; i < n; i++)
                    {
                        bool esPuntoNuevo = _serieEvolucionFuerza[i].EtiquetaFecha.StartsWith("Nueva");
                        Brush bPunto = esPuntoNuevo ? brushNuevo : brushHistorico;
                        g.FillEllipse(brushBlanco, pts[i].X - 5, pts[i].Y - 5, 10, 10);
                        g.FillEllipse(bPunto, pts[i].X - 4, pts[i].Y - 4, 8, 8);
                        g.DrawString($"{_serieEvolucionFuerza[i].ValorPrimario:0.0}kg", fontPunto, brushTexto, pts[i].X, pts[i].Y - 16, sfCenter);
                        g.DrawString(_serieEvolucionFuerza[i].EtiquetaFecha, fontEje, brushGris, pts[i].X, bot + 3, sfCenter);
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
            using (Font fontLeyenda = new Font("Segoe UI", 7.5F, FontStyle.Regular))
            using (Font fontBadge = new Font("Segoe UI", 7.5F, FontStyle.Bold))
            using (Pen penBorde = new Pen(Color.FromArgb(71, 85, 105), 1.4f))
            using (Brush brushTexto = new SolidBrush(Color.FromArgb(31, 41, 55)))
            using (Brush brushCabeza = new SolidBrush(Color.FromArgb(226, 232, 240)))
            {
                int cxFrente = 82;
                int cxDorso = 232;
                int yBase = 26;
                StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString("FRENTE", fontSub, brushTexto, cxFrente, 6, sfCenter);
                g.DrawString("DORSO", fontSub, brushTexto, cxDorso, 6, sfCenter);
                g.FillEllipse(brushCabeza, cxFrente - 12, yBase, 24, 26);
                g.DrawEllipse(penBorde, cxFrente - 12, yBase, 24, 26);
                g.FillRectangle(brushCabeza, cxFrente - 5, yBase + 25, 10, 7);
                using (Brush bHombros = new SolidBrush(ObtenerColorMusculo("Hombros")))
                {
                    g.FillEllipse(bHombros, cxFrente - 38, yBase + 30, 18, 18);
                    g.DrawEllipse(penBorde, cxFrente - 38, yBase + 30, 18, 18);
                    g.FillEllipse(bHombros, cxFrente + 20, yBase + 30, 18, 18);
                    g.DrawEllipse(penBorde, cxFrente + 20, yBase + 30, 18, 18);
                }
                using (Brush bPecho = new SolidBrush(ObtenerColorMusculo("Pecho")))
                {
                    Rectangle rectPechoIzq = new Rectangle(cxFrente - 22, yBase + 32, 21, 24);
                    Rectangle rectPechoDer = new Rectangle(cxFrente + 1, yBase + 32, 21, 24);
                    g.FillRectangle(bPecho, rectPechoIzq);
                    g.DrawRectangle(penBorde, rectPechoIzq);
                    g.FillRectangle(bPecho, rectPechoDer);
                    g.DrawRectangle(penBorde, rectPechoDer);
                }
                using (Brush bCore = new SolidBrush(ObtenerColorMusculo("Core")))
                {
                    Rectangle rectCore = new Rectangle(cxFrente - 18, yBase + 58, 36, 42);
                    g.FillRectangle(bCore, rectCore);
                    g.DrawRectangle(penBorde, rectCore);
                    g.DrawLine(penBorde, cxFrente, yBase + 58, cxFrente, yBase + 100);
                    g.DrawLine(penBorde, cxFrente - 18, yBase + 72, cxFrente + 18, yBase + 72);
                    g.DrawLine(penBorde, cxFrente - 18, yBase + 86, cxFrente + 18, yBase + 86);
                }
                using (Brush bBiceps = new SolidBrush(ObtenerColorMusculo("Bíceps")))
                {
                    Rectangle bicIzq = new Rectangle(cxFrente - 39, yBase + 49, 14, 32);
                    Rectangle bicDer = new Rectangle(cxFrente + 25, yBase + 49, 14, 32);
                    g.FillRectangle(bBiceps, bicIzq);
                    g.DrawRectangle(penBorde, bicIzq);
                    g.FillRectangle(bBiceps, bicDer);
                    g.DrawRectangle(penBorde, bicDer);
                }
                g.FillRectangle(brushCabeza, cxFrente - 40, yBase + 83, 12, 26);
                g.DrawRectangle(penBorde, cxFrente - 40, yBase + 83, 12, 26);
                g.FillRectangle(brushCabeza, cxFrente + 28, yBase + 83, 12, 26);
                g.DrawRectangle(penBorde, cxFrente + 28, yBase + 83, 12, 26);
                using (Brush bPiernas = new SolidBrush(ObtenerColorMusculo("Piernas")))
                {
                    Rectangle cuadIzq = new Rectangle(cxFrente - 20, yBase + 102, 18, 48);
                    Rectangle cuadDer = new Rectangle(cxFrente + 2, yBase + 102, 18, 48);
                    g.FillRectangle(bPiernas, cuadIzq);
                    g.DrawRectangle(penBorde, cuadIzq);
                    g.FillRectangle(bPiernas, cuadDer);
                    g.DrawRectangle(penBorde, cuadDer);
                    Rectangle gemIzq = new Rectangle(cxFrente - 18, yBase + 153, 14, 40);
                    Rectangle gemDer = new Rectangle(cxFrente + 4, yBase + 153, 14, 40);
                    g.FillRectangle(bPiernas, gemIzq);
                    g.DrawRectangle(penBorde, gemIzq);
                    g.FillRectangle(bPiernas, gemDer);
                    g.DrawRectangle(penBorde, gemDer);
                }
                g.FillEllipse(brushCabeza, cxDorso - 12, yBase, 24, 26);
                g.DrawEllipse(penBorde, cxDorso - 12, yBase, 24, 26);
                g.FillRectangle(brushCabeza, cxDorso - 5, yBase + 25, 10, 7);
                using (Brush bHombros = new SolidBrush(ObtenerColorMusculo("Hombros")))
                {
                    g.FillEllipse(bHombros, cxDorso - 38, yBase + 30, 18, 18);
                    g.DrawEllipse(penBorde, cxDorso - 38, yBase + 30, 18, 18);
                    g.FillEllipse(bHombros, cxDorso + 20, yBase + 30, 18, 18);
                    g.DrawEllipse(penBorde, cxDorso + 20, yBase + 30, 18, 18);
                }
                using (Brush bEspalda = new SolidBrush(ObtenerColorMusculo("Espalda")))
                {
                    Point[] ptsEspalda = new Point[]
                    {
                        new Point(cxDorso - 22, yBase + 32),
                        new Point(cxDorso + 22, yBase + 32),
                        new Point(cxDorso + 16, yBase + 82),
                        new Point(cxDorso - 16, yBase + 82)
                    };
                    g.FillPolygon(bEspalda, ptsEspalda);
                    g.DrawPolygon(penBorde, ptsEspalda);
                    g.DrawLine(penBorde, cxDorso, yBase + 32, cxDorso, yBase + 82);
                }
                using (Brush bCore = new SolidBrush(ObtenerColorMusculo("Core")))
                {
                    Rectangle rectLumbar = new Rectangle(cxDorso - 16, yBase + 83, 32, 17);
                    g.FillRectangle(bCore, rectLumbar);
                    g.DrawRectangle(penBorde, rectLumbar);
                }
                using (Brush bTriceps = new SolidBrush(ObtenerColorMusculo("Tríceps")))
                {
                    Rectangle triIzq = new Rectangle(cxDorso - 39, yBase + 49, 14, 32);
                    Rectangle triDer = new Rectangle(cxDorso + 25, yBase + 49, 14, 32);
                    g.FillRectangle(bTriceps, triIzq);
                    g.DrawRectangle(penBorde, triIzq);
                    g.FillRectangle(bTriceps, triDer);
                    g.DrawRectangle(penBorde, triDer);
                }
                g.FillRectangle(brushCabeza, cxDorso - 40, yBase + 83, 12, 26);
                g.DrawRectangle(penBorde, cxDorso - 40, yBase + 83, 12, 26);
                g.FillRectangle(brushCabeza, cxDorso + 28, yBase + 83, 12, 26);
                g.DrawRectangle(penBorde, cxDorso + 28, yBase + 83, 12, 26);
                using (Brush bPiernas = new SolidBrush(ObtenerColorMusculo("Piernas")))
                {
                    Rectangle isqIzq = new Rectangle(cxDorso - 20, yBase + 102, 18, 48);
                    Rectangle isqDer = new Rectangle(cxDorso + 2, yBase + 102, 18, 48);
                    g.FillRectangle(bPiernas, isqIzq);
                    g.DrawRectangle(penBorde, isqIzq);
                    g.FillRectangle(bPiernas, isqDer);
                    g.DrawRectangle(penBorde, isqDer);
                    Rectangle gemIzq = new Rectangle(cxDorso - 18, yBase + 153, 14, 40);
                    Rectangle gemDer = new Rectangle(cxDorso + 4, yBase + 153, 14, 40);
                    g.FillRectangle(bPiernas, gemIzq);
                    g.DrawRectangle(penBorde, gemIzq);
                    g.FillRectangle(bPiernas, gemDer);
                    g.DrawRectangle(penBorde, gemDer);
                }
                int yLeyenda = 236;
                using (Brush bRojo = new SolidBrush(Color.FromArgb(220, 38, 38)))
                using (Brush bNaranja = new SolidBrush(Color.FromArgb(249, 115, 22)))
                using (Brush bGris = new SolidBrush(Color.FromArgb(203, 213, 225)))
                {
                    g.FillRectangle(bRojo, 14, yLeyenda, 11, 11);
                    g.DrawRectangle(penBorde, 14, yLeyenda, 11, 11);
                    g.DrawString("Primario (≥5s)", fontLeyenda, brushTexto, 28, yLeyenda - 2);
                    g.FillRectangle(bNaranja, 114, yLeyenda, 11, 11);
                    g.DrawRectangle(penBorde, 114, yLeyenda, 11, 11);
                    g.DrawString("Secundario (1-4s)", fontLeyenda, brushTexto, 128, yLeyenda - 2);
                    g.FillRectangle(bGris, 228, yLeyenda, 11, 11);
                    g.DrawRectangle(penBorde, 228, yLeyenda, 11, 11);
                    g.DrawString("Inactivo", fontLeyenda, brushTexto, 242, yLeyenda - 2);
                }
                string[] ordenGrupos = { "Pecho", "Espalda", "Piernas", "Hombros", "Bíceps", "Tríceps", "Core" };
                int col = 0;
                int fila = 0;
                int yInicioBadges = 264;
                foreach (string grp in ordenGrupos)
                {
                    int cantSeries = series.ContainsKey(grp) ? series[grp] : 0;
                    int xBadge = 14 + (col * 100);
                    int yBadge = yInicioBadges + (fila * 28);
                    Color colorFondo = ObtenerColorMusculo(grp);
                    Color colorTextoBadge = niveles.ContainsKey(grp) && niveles[grp] > 0 ? Color.White : Color.FromArgb(31, 41, 55);
                    Rectangle rectTag = new Rectangle(xBadge, yBadge, 92, 22);
                    using (Brush bTag = new SolidBrush(colorFondo))
                    using (Brush bTxt = new SolidBrush(colorTextoBadge))
                    {
                        g.FillRectangle(bTag, rectTag);
                        g.DrawRectangle(penBorde, rectTag);
                        g.DrawString($"{grp}: {cantSeries}s", fontBadge, bTxt, xBadge + 5, yBadge + 3);
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