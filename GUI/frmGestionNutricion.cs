using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using BE;
using BLL;
using Seguridad;

namespace GUI
{
    public partial class frmGestionNutricion : Form, IObserverIdioma
    {
        private TurnoBLL _turnoBLL;
        private SocioBLL _socioBLL;
        private PlanNutricionalBLL _planBLL;
        private Dictionary<string, string> _traducciones;
        private string T(string clave, string textoPorDefecto)
        {
            return _traducciones != null && _traducciones.ContainsKey(clave) ? _traducciones[clave] : textoPorDefecto;
        }
        private SocioBE _socioActual;
        private EvaluacionBiometricaBE _biometriaActual = new EvaluacionBiometricaBE();
        private List<PuntoEvolucionBE> _serieEvolucion = new List<PuntoEvolucionBE>();
        private bool _cargandoControles = false;

        public frmGestionNutricion()
        {
            InitializeComponent();
            _turnoBLL = new TurnoBLL();
            _socioBLL = new SocioBLL();
            _planBLL = new PlanNutricionalBLL();
            GestorIdioma.Instancia.Suscribir(this);
        }

        private void frmGestionNutricion_Load(object sender, EventArgs e)
        {
            _cargandoControles = true;
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnlGraficoNutricion, new object[] { true });
            CargarCombosBiometricos();
            ActualizarIdioma(GestorIdioma.Instancia.IdiomaActual);
            CargarAgendaDelDia();
            BloquearCamposPlan(true);
            _cargandoControles = false;
            ActualizarCalculosBiometricosEnVivo();
            pnlGraficoNutricion.Invalidate();
        }

        private void CargarCombosBiometricos()
        {
            cmbGenero.Items.Clear();
            foreach (var g in _planBLL.ObtenerGenerosDisponibles())
            {
                cmbGenero.Items.Add(g);
            }
            if (cmbGenero.Items.Count > 0)
            {
                cmbGenero.SelectedIndex = 0;
            }
            cmbObjetivoNutricional.Items.Clear();
            foreach (var obj in _planBLL.ObtenerObjetivosNutricionales())
            {
                cmbObjetivoNutricional.Items.Add(obj);
            }
            if (cmbObjetivoNutricional.Items.Count > 1)
            {
                cmbObjetivoNutricional.SelectedIndex = 1;
            }
        }

        private void ParametrosBiometricos_Changed(object sender, EventArgs e)
        {
            if (_cargandoControles) return;
            ActualizarCalculosBiometricosEnVivo();
        }

        private void ActualizarCalculosBiometricosEnVivo()
        {
            string genero = cmbGenero.SelectedItem != null ? cmbGenero.SelectedItem.ToString() : "Masculino";
            string objetivo = cmbObjetivoNutricional.SelectedItem != null ? cmbObjetivoNutricional.SelectedItem.ToString() : "Mantenimiento (Normocalórico)";
            _biometriaActual = _planBLL.CalcularBiometria(
                numPeso.Value,
                numAltura.Value,
                (int)numEdad.Value,
                genero,
                objetivo);
            lblValorIMC.Text = $"IMC ({_biometriaActual.DiagnosticoIMC}):\r\n{_biometriaActual.IndiceMasaCorporal:0.00} kg/m²";
            lblValorTMB.Text = $"TMB (Harris-Benedict):\r\n{_biometriaActual.TasaMetabolicaBasal:0} kcal/día";
            lblValorCalorias.Text = $"Meta Diaria Sugerida:\r\n{_biometriaActual.CaloriasObjetivo:0} kcal/día";
            string advertencia = _planBLL.ObtenerAdvertenciaClinica(_biometriaActual);
            lblAlertaIMC.Text = advertencia;
            if (_biometriaActual.IndiceMasaCorporal >= 30.0m)
            {
                lblAlertaIMC.BackColor = Color.FromArgb(254, 226, 226);
                lblAlertaIMC.ForeColor = Color.FromArgb(153, 27, 27);
            }
            else if (_biometriaActual.IndiceMasaCorporal < 18.5m || _biometriaActual.IndiceMasaCorporal >= 25.0m)
            {
                lblAlertaIMC.BackColor = Color.FromArgb(254, 243, 199);
                lblAlertaIMC.ForeColor = Color.FromArgb(146, 64, 14);
            }
            else
            {
                lblAlertaIMC.BackColor = Color.FromArgb(220, 252, 231);
                lblAlertaIMC.ForeColor = Color.FromArgb(22, 101, 52);
            }
        }

        private void CargarAgendaDelDia()
        {
            List<TurnoBE> turnosHoy = _turnoBLL.ObtenerAgenda(DateTime.Today);
            List<TurnoBE> misTurnos = new List<TurnoBE>();
            int miID = SessionManager.Instancia.UsuarioActual != null ? SessionManager.Instancia.UsuarioActual.ID : 0;
            foreach (var t in turnosHoy)
            {
                if (t.ID_Nutricionista == miID)
                {
                    misTurnos.Add(t);
                }
            }
            dgvAgendaHoy.DataSource = misTurnos;
            if (dgvAgendaHoy.Columns["ID_Socio"] != null) dgvAgendaHoy.Columns["ID_Socio"].Visible = false;
            if (dgvAgendaHoy.Columns["ID_Nutricionista"] != null) dgvAgendaHoy.Columns["ID_Nutricionista"].Visible = false;
            if (dgvAgendaHoy.Columns["DVH"] != null) dgvAgendaHoy.Columns["DVH"].Visible = false;
            if (dgvAgendaHoy.Columns["Socio"] != null) dgvAgendaHoy.Columns["Socio"].Visible = false;
            if (dgvAgendaHoy.Columns["Nutricionista"] != null) dgvAgendaHoy.Columns["Nutricionista"].Visible = false;
        }

        private void dgvAgendaHoy_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvAgendaHoy.Rows[e.RowIndex].DataBoundItem is TurnoBE turno)
            {
                if (!string.IsNullOrWhiteSpace(turno.DNISocio))
                {
                    txtDNISocio.Text = turno.DNISocio;
                }
            }
        }

        private void CargarPlanesHistoricos()
        {
            var planes = _planBLL.ObtenerPlanesSocio(_socioActual.ID_Socio)
                .OrderByDescending(p => p.FechaCreacion)
                .ThenByDescending(p => p.CodigoPlan)
                .ToList();
            dgvPlanesHistoricos.DataSource = null;
            dgvPlanesHistoricos.DataSource = planes;
            if (dgvPlanesHistoricos.Columns["ID_Socio"] != null) dgvPlanesHistoricos.Columns["ID_Socio"].Visible = false;
            if (dgvPlanesHistoricos.Columns["ID_Nutricionista"] != null) dgvPlanesHistoricos.Columns["ID_Nutricionista"].Visible = false;
            if (dgvPlanesHistoricos.Columns["DVH"] != null) dgvPlanesHistoricos.Columns["DVH"].Visible = false;
            if (dgvPlanesHistoricos.Columns["Socio"] != null) dgvPlanesHistoricos.Columns["Socio"].Visible = false;
            if (dgvPlanesHistoricos.Columns["Nutricionista"] != null) dgvPlanesHistoricos.Columns["Nutricionista"].Visible = false;
            if (dgvPlanesHistoricos.Columns["EvaluacionAntropometrica"] != null) dgvPlanesHistoricos.Columns["EvaluacionAntropometrica"].Visible = false;
            if (dgvPlanesHistoricos.Columns["ResumenBiometrico"] != null) dgvPlanesHistoricos.Columns["ResumenBiometrico"].HeaderText = "Evaluación Biométrica";
            _serieEvolucion = _planBLL.ObtenerEvolucionBiometrica(_socioActual.ID_Socio);
            pnlGraficoNutricion.Invalidate();
            if (planes.Count > 0 && dgvPlanesHistoricos.Rows.Count > 0)
            {
                dgvPlanesHistoricos.ClearSelection();
                dgvPlanesHistoricos.Rows[0].Selected = true;
                DataGridViewColumn primeraColVisible = dgvPlanesHistoricos.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                if (primeraColVisible != null)
                {
                    dgvPlanesHistoricos.CurrentCell = dgvPlanesHistoricos.Rows[0].Cells[primeraColVisible.Index];
                }
                CargarPlanEnControles(planes[0]);
            }
            else
            {
                txtHabitos.Clear();
                ActualizarCalculosBiometricosEnVivo();
            }
        }

        private void CargarPlanEnControles(PlanNutricionalBE plan)
        {
            if (plan == null) return;
            txtHabitos.Text = plan.DetalleHabitos;
            if (plan.DatosBiometricos != null && plan.DatosBiometricos.PesoKg > 0 && plan.DatosBiometricos.AlturaCm > 0)
            {
                _cargandoControles = true;
                numPeso.Value = Math.Clamp(plan.DatosBiometricos.PesoKg, numPeso.Minimum, numPeso.Maximum);
                numAltura.Value = Math.Clamp(plan.DatosBiometricos.AlturaCm, numAltura.Minimum, numAltura.Maximum);
                numEdad.Value = Math.Clamp(plan.DatosBiometricos.Edad > 0 ? plan.DatosBiometricos.Edad : 25, (int)numEdad.Minimum, (int)numEdad.Maximum);
                if (!string.IsNullOrWhiteSpace(plan.DatosBiometricos.Genero) && cmbGenero.Items.Contains(plan.DatosBiometricos.Genero))
                {
                    cmbGenero.SelectedItem = plan.DatosBiometricos.Genero;
                }
                if (!string.IsNullOrWhiteSpace(plan.DatosBiometricos.ObjetivoNutricional) && cmbObjetivoNutricional.Items.Contains(plan.DatosBiometricos.ObjetivoNutricional))
                {
                    cmbObjetivoNutricional.SelectedItem = plan.DatosBiometricos.ObjetivoNutricional;
                }
                _cargandoControles = false;
            }
            ActualizarCalculosBiometricosEnVivo();
        }

        private void pnlGraficoNutricion_Paint(object sender, PaintEventArgs e)
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
            using (Pen penCurva = new Pen(Color.FromArgb(37, 99, 235), 2.5f))
            {
                g.DrawString(T("titGraficoNutricion", "Curva de Evolución de Peso Corporal (kg) e IMC"), fontTitulo, brushTexto, 12, 8);
                if (_serieEvolucion == null || _serieEvolucion.Count == 0)
                {
                    g.DrawString(T("msgGraficoNutricionVacio", "Busque un socio con evaluaciones biométricas para visualizar su curva de progreso."), fontEje, brushGris, 14, 88);
                    return;
                }
                decimal pesoInicial = _serieEvolucion.First().ValorPrimario;
                decimal pesoFinal = _serieEvolucion.Last().ValorPrimario;
                decimal delta = pesoFinal - pesoInicial;
                string signo = delta > 0 ? "+" : "";
                string textoDelta = $"Δ Peso: {signo}{delta:0.0} kg | IMC actual: {_serieEvolucion.Last().ValorSecundario:0.00}";
                SizeF tamDelta = g.MeasureString(textoDelta, fontPunto);
                g.DrawString(textoDelta, fontPunto, brushTexto, pnlGraficoNutricion.Width - tamDelta.Width - 14, 8);
                int izq = 48, der = pnlGraficoNutricion.Width - 24, top = 36, bot = pnlGraficoNutricion.Height - 28;
                int ancho = Math.Max(10, der - izq);
                int alto = Math.Max(10, bot - top);
                decimal minVal = _serieEvolucion.Min(p => p.ValorPrimario);
                decimal maxVal = _serieEvolucion.Max(p => p.ValorPrimario);
                if (minVal == maxVal)
                {
                    minVal -= 5m;
                    maxVal += 5m;
                }
                else
                {
                    decimal margen = Math.Max(1m, (maxVal - minVal) * 0.2m);
                    minVal -= margen;
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
                int n = _serieEvolucion.Count;
                PointF[] pts = new PointF[n];
                for (int i = 0; i < n; i++)
                {
                    float x = n == 1 ? izq + (ancho / 2f) : izq + (ancho * i / (float)(n - 1));
                    float ratio = (float)((_serieEvolucion[i].ValorPrimario - minVal) / (maxVal - minVal));
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
                            Color.FromArgb(90, 37, 99, 235),
                            Color.FromArgb(10, 37, 99, 235),
                            LinearGradientMode.Vertical))
                        {
                            g.FillPath(brushArea, pathArea);
                        }
                    }
                    g.DrawLines(penCurva, pts);
                }
                StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center };
                using (Brush brushPunto = new SolidBrush(Color.FromArgb(37, 99, 235)))
                using (Brush brushBlanco = new SolidBrush(Color.White))
                {
                    for (int i = 0; i < n; i++)
                    {
                        g.FillEllipse(brushBlanco, pts[i].X - 5, pts[i].Y - 5, 10, 10);
                        g.FillEllipse(brushPunto, pts[i].X - 4, pts[i].Y - 4, 8, 8);
                        g.DrawString($"{_serieEvolucion[i].ValorPrimario:0.0}kg", fontPunto, brushTexto, pts[i].X, pts[i].Y - 17, sfCenter);
                        g.DrawString(_serieEvolucion[i].EtiquetaFecha, fontEje, brushGris, pts[i].X, bot + 4, sfCenter);
                    }
                }
            }
        }

        private void dgvPlanesHistoricos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPlanesHistoricos.Rows[e.RowIndex].DataBoundItem is PlanNutricionalBE plan)
            {
                CargarPlanEnControles(plan);
            }
        }

        private void btnBuscarSocio_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDNISocio.Text)) throw new Exception("Ingrese el DNI.");
                _socioActual = _socioBLL.ConsultarSocio(txtDNISocio.Text.Trim());
                if (_socioActual == null) throw new Exception("Socio no encontrado.");
                bool planVigente = _socioBLL.ObtenerEstadoVigenciaPlan(
                    _socioActual,
                    out DateTime? fechaVenc,
                    out string nomPlan,
                    out int diasRest,
                    out string msgVigencia);
                CargarPlanesHistoricos();
                if (!planVigente)
                {
                    BloquearCamposPlan(true);
                    btnExportarPDF.Enabled = true;
                    MessageBox.Show(
                        $"{T("msgCuotaVencidaNutricion", "Atención denegada para nueva consulta: el socio posee su cuota vencida. Se habilita únicamente la visualización de su historial clínico.")}\r\n\r\n({_socioActual.Nombre} {_socioActual.Apellido} — {msgVigencia})",
                        T("titCuotaVencida", "Cuota Vencida"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                BloquearCamposPlan(false);
                MessageBox.Show($"{T("msgSocioHabilitado", "Socio encontrado y habilitado: ")}{_socioActual.Nombre} {_socioActual.Apellido}\r\n{msgVigencia}", T("titSocioHabilitado", "Socio Habilitado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                BloquearCamposPlan(true);
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSugerirHabitos_Click(object sender, EventArgs e)
        {
            ActualizarCalculosBiometricosEnVivo();
            txtHabitos.Text = _planBLL.GenerarSugerenciaHabitos(_biometriaActual);
        }

        private void btnAsignarPlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (_socioActual == null) throw new Exception("Primero debe buscar y cargar un socio.");
                ActualizarCalculosBiometricosEnVivo();
                if (string.IsNullOrWhiteSpace(txtHabitos.Text))
                {
                    txtHabitos.Text = _planBLL.GenerarSugerenciaHabitos(_biometriaActual);
                }
                PlanNutricionalBE nuevoPlan = new PlanNutricionalBE
                {
                    FechaCreacion = DateTime.Now,
                    DatosBiometricos = _biometriaActual,
                    DetalleHabitos = txtHabitos.Text,
                    ID_Socio = _socioActual.ID_Socio,
                    ID_Nutricionista = SessionManager.Instancia.UsuarioActual.ID
                };
                nuevoPlan.SincronizarBiometriaAJson();
                _planBLL.AsignarPlan(nuevoPlan);
                MessageBox.Show(T("msgPlanGuardadoExito", "Plan nutricional y evaluación biométrica guardados correctamente."), T("titExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarPlanesHistoricos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (_socioActual == null)
                    throw new Exception("Primero debe buscar un socio para exportar su informe nutricional.");
                ActualizarCalculosBiometricosEnVivo();
                string habitosReporte = !string.IsNullOrWhiteSpace(txtHabitos.Text)
                    ? txtHabitos.Text
                    : _planBLL.GenerarSugerenciaHabitos(_biometriaActual);
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                    sfd.Title = "PDF";
                    sfd.FileName = $"Informe_Nutricional_{_socioActual.DNI}_{DateTime.Now:yyyyMMdd}.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string nomNutri = SessionManager.Instancia.UsuarioActual != null
                            ? SessionManager.Instancia.UsuarioActual.Nombre
                            : "Nutricionista UAI-FIT";
                        PdfCorporativoHelper.GenerarInformeNutricionalCorporativoPDF(
                            sfd.FileName,
                            _socioActual,
                            _biometriaActual,
                            habitosReporte,
                            nomNutri,
                            null,
                            true);
                        MessageBox.Show(T("msgPDFExito", "Informe PDF generado exitosamente."), T("titPDFExito", "Exportación Exitosa"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(T(ex.Message, ex.Message), T("titError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GenerarArchivoPDFNativo(string rutaArchivo, SocioBE socio, EvaluacionBiometricaBE bio, string detalleHabitos)
        {
            string nutricionista = SessionManager.Instancia.UsuarioActual != null ? SessionManager.Instancia.UsuarioActual.Nombre : "Profesional UAI-FIT";
            List<string> lineas = new List<string>();
            lineas.Add("==========================================================================");
            lineas.Add("              UAI-FIT - INFORME CLINICO NUTRICIONAL Y BIOMETRICO          ");
            lineas.Add("==========================================================================");
            lineas.Add($"Fecha de Emision: {DateTime.Now:dd/MM/yyyy HH:mm}");
            lineas.Add($"Socio: {NormalizarTextoPDF(socio.Nombre)} {NormalizarTextoPDF(socio.Apellido)} (DNI: {socio.DNI})");
            lineas.Add($"Profesional Nutricionista: {NormalizarTextoPDF(nutricionista)}");
            lineas.Add("--------------------------------------------------------------------------");
            lineas.Add("1. EVALUACION ANTROPOMETRICA Y METABOLICA (CALCULADORA BIOMETRICA)");
            lineas.Add("--------------------------------------------------------------------------");
            lineas.Add($"   * Peso Corporal:         {bio.PesoKg:0.0} kg");
            lineas.Add($"   * Altura:                {bio.AlturaCm:0} cm");
            lineas.Add($"   * Edad / Genero:         {bio.Edad} anios / {NormalizarTextoPDF(bio.Genero)}");
            lineas.Add($"   * Indice Masa Corporal:  {bio.IndiceMasaCorporal:0.00} kg/m2 ({NormalizarTextoPDF(bio.DiagnosticoIMC)})");
            lineas.Add($"   * TMB (Harris-Benedict): {bio.TasaMetabolicaBasal:0} kcal/dia");
            AgregarLineaConAjuste(lineas, $"   * Protocolo Asignado:    {bio.ObjetivoNutricional}", 74, "                            ");
            lineas.Add($"   * Meta Calorica Diaria:  {bio.CaloriasObjetivo:0} kcal/dia");
            AgregarLineaConAjuste(lineas, $"   * Diagnostico Clinico:   {_planBLL.ObtenerAdvertenciaClinica(bio)}", 74, "                            ");
            lineas.Add("--------------------------------------------------------------------------");
            lineas.Add("2. PAUTA NUTRICIONAL Y DETALLE DE HABITOS");
            lineas.Add("--------------------------------------------------------------------------");
            foreach (string parrafo in (detalleHabitos ?? string.Empty).Replace("\r\n", "\n").Split('\n'))
            {
                AgregarLineaConAjuste(lineas, "   " + parrafo, 74, "     ");
            }
            try
            {
                RutinaEntrenamientoBLL rutBLL = new RutinaEntrenamientoBLL();
                var rutinas = rutBLL.ConsultarRutinasPorDNI(socio.DNI);
                if (rutinas != null && rutinas.Count > 0)
                {
                    var ultimaRutina = rutinas.OrderByDescending(r => r.FechaCreacion).First();
                    lineas.Add("--------------------------------------------------------------------------");
                    AgregarLineaConAjuste(lineas, $"3. SINERGIA DEPORTIVA - RUTINA VIGENTE ({ultimaRutina.ObjetivoFisico} - {ultimaRutina.FrecuenciaSemanal} dias/sem)", 74, "   ");
                    lineas.Add("--------------------------------------------------------------------------");
                    foreach (var ej in ultimaRutina.Ejercicios)
                    {
                        string textoEj = $"   - {ej.Nombre} [{ej.GrupoMuscular}]: {ej.Series}x{ej.Repeticiones} | Desc: {ej.TiempoDescansoSegundos}s | Carga: {ej.CargaSugeridaKg:0.#} kg";
                        AgregarLineaConAjuste(lineas, textoEj, 74, "     ");
                    }
                }
            }
            catch { }
            lineas.Add("==========================================================================");
            StringBuilder sbStream = new StringBuilder();
            sbStream.AppendLine("BT");
            sbStream.AppendLine("/F1 9.5 Tf");
            sbStream.AppendLine("13.5 TL");
            sbStream.AppendLine("38 795 Td");
            foreach (string lin in lineas.Take(54))
            {
                string esc = NormalizarTextoPDF(lin).Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
                sbStream.AppendLine($"({esc}) Tj T*");
            }
            sbStream.AppendLine("ET");
            byte[] contentBytes = Encoding.ASCII.GetBytes(sbStream.ToString());
            using (FileStream fs = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
            {
                List<long> offsets = new List<long>();
                void WriteObj(string body)
                {
                    sw.Flush();
                    offsets.Add(fs.Position);
                    sw.Write(body);
                    sw.Flush();
                }
                sw.Write("%PDF-1.4\n");
                WriteObj("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");
                WriteObj("2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");
                WriteObj("3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n");
                WriteObj("4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n");
                WriteObj($"5 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n{sbStream}endstream\nendobj\n");
                long xrefPos = fs.Position;
                sw.Write($"xref\n0 {offsets.Count + 1}\n");
                sw.Write("0000000000 65535 f \n");
                foreach (long off in offsets)
                {
                    sw.Write($"{off:D10} 00000 n \n");
                }
                sw.Write($"trailer\n<< /Size {offsets.Count + 1} /Root 1 0 R >>\nstartxref\n{xrefPos}\n%%EOF");
                sw.Flush();
            }
        }

        private void AgregarLineaConAjuste(List<string> destino, string textoOriginal, int maxCaracteres, string sangriaContinuacion)
        {
            string limpio = NormalizarTextoPDF(textoOriginal ?? string.Empty).TrimEnd();
            if (limpio.Length <= maxCaracteres)
            {
                destino.Add(limpio);
                return;
            }
            string[] palabras = limpio.Split(' ');
            StringBuilder lineaActual = new StringBuilder();
            bool esPrimeraLinea = true;
            foreach (string palabra in palabras)
            {
                string prefijo = esPrimeraLinea ? "" : sangriaContinuacion;
                int largoProyectado = lineaActual.Length == 0
                    ? prefijo.Length + palabra.Length
                    : lineaActual.Length + 1 + palabra.Length;
                if (largoProyectado > maxCaracteres && lineaActual.Length > 0)
                {
                    destino.Add(lineaActual.ToString());
                    esPrimeraLinea = false;
                    lineaActual.Clear();
                    lineaActual.Append(sangriaContinuacion).Append(palabra);
                }
                else
                {
                    if (lineaActual.Length == 0)
                    {
                        lineaActual.Append(prefijo).Append(palabra);
                    }
                    else
                    {
                        lineaActual.Append(' ').Append(palabra);
                    }
                }
            }
            if (lineaActual.Length > 0)
            {
                destino.Add(lineaActual.ToString());
            }
        }

        private string NormalizarTextoPDF(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            string formD = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (c >= 32 && c <= 126) sb.Append(c);
                    else if (c == '•') sb.Append('*');
                }
            }
            return sb.ToString();
        }

        private void BloquearCamposPlan(bool bloquear)
        {
            bool puedeAsignar = SessionManager.Instancia.UsuarioActual != null &&
                    SessionManager.Instancia.UsuarioActual.TienePermiso("ASIGNAR_PLAN_NUTRICIONAL");
            bool habilitarEdicion = !bloquear && puedeAsignar;
            pnlCalculadoraBiometrica.Enabled = habilitarEdicion;
            txtHabitos.Enabled = habilitarEdicion;
            btnSugerirHabitos.Enabled = habilitarEdicion;
            btnAsignarPlan.Enabled = habilitarEdicion;
            btnExportarPDF.Enabled = !bloquear;
        }

        public void ActualizarIdioma(IdiomaBE idioma)
        {
            if (idioma == null) return;
            _traducciones = new IdiomaBLL().ObtenerTraducciones(idioma, this.Name);
            TraducirControlesRecursivo(this.Controls);
            this.Text = T("titGestionNutricion", "Consultorio Nutricional");
            pnlGraficoNutricion.Invalidate();
        }

        private void TraducirControlesRecursivo(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (_traducciones != null && _traducciones.ContainsKey(c.Name))
                    c.Text = _traducciones[c.Name];
                if (c is DataGridView dgv)
                {
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (_traducciones != null && _traducciones.ContainsKey(col.DataPropertyName))
                        {
                            col.HeaderText = _traducciones[col.DataPropertyName];
                        }
                    }
                }
                if (c.HasChildren)
                    TraducirControlesRecursivo(c.Controls);
            }
        }

        private void frmGestionNutricion_FormClosing(object sender, FormClosingEventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}