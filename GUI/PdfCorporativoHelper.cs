using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BE;

namespace GUI
{
    public static class PdfCorporativoHelper
    {
        private class PdfCanvas
        {
            private readonly StringBuilder _sb = new StringBuilder();

            public string ObtenerContenido() => _sb.ToString();

            private static string F(double v) => v.ToString("0.##", CultureInfo.InvariantCulture);

            public void RectanguloRelleno(double x, double yTop, double w, double h, double r, double g, double b)
            {
                double yPdf = 842.0 - yTop - h;
                _sb.AppendLine($"{F(r)} {F(g)} {F(b)} rg {F(x)} {F(yPdf)} {F(w)} {F(h)} re f");
            }

            public void RectanguloConBorde(double x, double yTop, double w, double h, double rFill, double gFill, double bFill, double rStroke, double gStroke, double bStroke, double lineWidth = 1.0)
            {
                double yPdf = 842.0 - yTop - h;
                _sb.AppendLine($"{F(lineWidth)} w {F(rFill)} {F(gFill)} {F(bFill)} rg {F(rStroke)} {F(gStroke)} {F(bStroke)} RG {F(x)} {F(yPdf)} {F(w)} {F(h)} re B");
            }

            public void Linea(double x1, double yTop1, double x2, double yTop2, double r, double g, double b, double lineWidth = 1.0)
            {
                double y1 = 842.0 - yTop1;
                double y2 = 842.0 - yTop2;
                _sb.AppendLine($"{F(lineWidth)} w {F(r)} {F(g)} {F(b)} RG {F(x1)} {F(y1)} m {F(x2)} {F(y2)} l S");
            }

            public void Texto(string texto, double x, double yTop, int fontIndex = 1, double fontSize = 10, double r = 0.12, double g = 0.16, double b = 0.22)
            {
                if (string.IsNullOrEmpty(texto)) return;
                double yPdf = 842.0 - yTop - (fontSize * 0.78);
                string fontName = fontIndex == 2 ? "/F2" : (fontIndex == 3 ? "/F3" : "/F1");
                _sb.AppendLine($"BT {fontName} {F(fontSize)} Tf {F(r)} {F(g)} {F(b)} rg {F(x)} {F(yPdf)} Td ({EscaparTextoPdf(texto)}) Tj ET");
            }

            public void TextoDerecha(string texto, double xRight, double yTop, int fontIndex = 1, double fontSize = 10, double r = 0.12, double g = 0.16, double b = 0.22)
            {
                if (string.IsNullOrEmpty(texto)) return;
                double factorAncho = fontIndex == 3 ? 0.60 : (fontIndex == 2 ? 0.54 : 0.50);
                double anchoEstimado = texto.Length * fontSize * factorAncho;
                Texto(texto, Math.Max(20, xRight - anchoEstimado), yTop, fontIndex, fontSize, r, g, b);
            }
        }

        private static string EscaparTextoPdf(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            string limpio = input
                .Replace("•", "-")
                .Replace("—", "-")
                .Replace("–", "-")
                .Replace("≥", ">=")
                .Replace("≤", "<=")
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
            StringBuilder sb = new StringBuilder(limpio.Length);
            foreach (char c in limpio)
            {
                if (c >= 32 && c <= 126)
                {
                    sb.Append(c);
                }
                else if (c >= 160 && c <= 255)
                {
                    sb.Append('\\');
                    sb.Append(Convert.ToString((int)c, 8).PadLeft(3, '0'));
                }
            }
            return sb.ToString();
        }

        private static void DibujarEncabezadoCorporativo(PdfCanvas c, string subtituloArea, string tituloDocumento, string badgeDerecha, string subBadgeDerecha)
        {
            c.RectanguloRelleno(0, 0, 595, 88, 0.12, 0.16, 0.22);
            c.RectanguloRelleno(0, 88, 595, 5, 0.15, 0.39, 0.92);
            c.RectanguloRelleno(36, 18, 42, 42, 0.15, 0.39, 0.92);
            c.Texto("UAI", 44, 26, 2, 13, 1, 1, 1);
            c.Texto("FIT", 46, 41, 2, 11, 0.60, 0.80, 1);
            c.Texto("UAI-FIT  |  HIGH PERFORMANCE GYM & HEALTH CENTER", 90, 20, 2, 11, 0.95, 0.97, 1.0);
            c.Texto(subtituloArea, 90, 36, 1, 9.5, 0.65, 0.74, 0.86);
            c.Texto(tituloDocumento, 90, 54, 2, 15, 1, 1, 1);
            c.RectanguloConBorde(415, 16, 144, 54, 0.17, 0.22, 0.30, 0.28, 0.35, 0.46, 1.0);
            c.TextoDerecha(badgeDerecha, 549, 25, 2, 11, 1, 1, 1);
            c.TextoDerecha(subBadgeDerecha, 549, 46, 1, 8.5, 0.75, 0.82, 0.92);
        }

        private static void DibujarPieDePaginaCorporativo(PdfCanvas c, string dvhSello, int paginaActual = 1, int totalPaginas = 1)
        {
            if (!string.IsNullOrWhiteSpace(dvhSello))
            {
                c.RectanguloConBorde(36, 738, 523, 44, 0.96, 0.97, 0.99, 0.82, 0.86, 0.91, 0.8);
                c.Texto("SELLO DE INTEGRIDAD CRIPTOGRAFICO (SHA-256 DVH):", 46, 746, 2, 8, 0.28, 0.33, 0.41);
                c.Texto(dvhSello, 46, 761, 3, 8, 0.12, 0.16, 0.22);
            }
            c.Linea(36, 792, 559, 792, 0.82, 0.86, 0.91, 0.8);
            c.Texto("UAI-FIT Sistema Integral de Gestion Deportiva y Clinica  |  Documento Oficial Auditable", 36, 800, 1, 8, 0.45, 0.50, 0.58);
            c.TextoDerecha($"Pag. {paginaActual} de {totalPaginas}  -  Generado: {DateTime.Now:dd/MM/yyyy HH:mm}", 559, 800, 1, 8, 0.45, 0.50, 0.58);
        }

        public static void GenerarReciboSuscripcionPDF(
            string rutaArchivo,
            ComprobantePagoBE comprobante,
            SocioBE socio,
            PlanSuscripcionBE plan,
            string nombreCobrador,
            bool abrirAlFinalizar = true)
        {
            PdfCanvas c = new PdfCanvas();
            int nroComp = comprobante != null ? comprobante.NroComprobante : 1;
            DateTime fechaEmision = comprobante != null && comprobante.Fecha != default ? comprobante.Fecha : DateTime.Now;
            decimal monto = comprobante != null ? comprobante.MontoTotal : (plan != null ? plan.Precio : 0m);
            string metodoPago = comprobante != null && !string.IsNullOrWhiteSpace(comprobante.MetodoPago) ? comprobante.MetodoPago : "Efectivo";
            string nombrePlan = plan != null && !string.IsNullOrWhiteSpace(plan.Nombre) ? plan.Nombre : "Plan de Suscripcion";
            int duracionDias = plan != null && plan.DuracionDias > 0 ? plan.DuracionDias : 30;
            DateTime fechaVenc = fechaEmision.Date.AddDays(duracionDias);
            DibujarEncabezadoCorporativo(
                c,
                "DEPARTAMENTO DE RECEPCION Y ADMINISTRACION DE SOCIOS",
                "RECIBO OFICIAL DE SUSCRIPCION",
                $"RECIBO #{nroComp:D6}",
                $"Fecha: {fechaEmision:dd/MM/yyyy HH:mm}");
            c.RectanguloConBorde(36, 112, 254, 108, 0.98, 0.99, 1.0, 0.85, 0.89, 0.94, 1.0);
            c.RectanguloRelleno(36, 112, 254, 24, 0.93, 0.95, 0.98);
            c.Texto("DATOS DEL SOCIO TITULAR", 48, 119, 2, 9.5, 0.12, 0.16, 0.22);
            string nomSocio = socio != null ? $"{socio.Nombre} {socio.Apellido}".Trim() : "-";
            string dniSocio = socio != null && !string.IsNullOrWhiteSpace(socio.DNI) ? socio.DNI : "-";
            string telSocio = socio != null && !string.IsNullOrWhiteSpace(socio.Telefono) ? socio.Telefono : "No informado";
            string emailSocio = socio != null && !string.IsNullOrWhiteSpace(socio.Email) ? socio.Email : "No informado";
            c.Texto("Socio:", 48, 146, 2, 9, 0.35, 0.41, 0.49);
            c.Texto(nomSocio, 105, 146, 2, 9.5, 0.12, 0.16, 0.22);
            c.Texto("DNI / ID:", 48, 165, 2, 9, 0.35, 0.41, 0.49);
            c.Texto(dniSocio, 105, 165, 1, 9.5, 0.12, 0.16, 0.22);
            c.Texto("Telefono:", 48, 184, 2, 9, 0.35, 0.41, 0.49);
            c.Texto(telSocio, 105, 184, 1, 9, 0.12, 0.16, 0.22);
            c.Texto("Email:", 48, 202, 2, 9, 0.35, 0.41, 0.49);
            c.Texto(emailSocio, 105, 202, 1, 8.5, 0.12, 0.16, 0.22);
            c.RectanguloConBorde(305, 112, 254, 108, 0.98, 0.99, 1.0, 0.85, 0.89, 0.94, 1.0);
            c.RectanguloRelleno(305, 112, 254, 24, 0.93, 0.95, 0.98);
            c.Texto("CONDICIONES DE LA TRANSACCION", 317, 119, 2, 9.5, 0.12, 0.16, 0.22);
            c.Texto("Nro. Operacion:", 317, 146, 2, 9, 0.35, 0.41, 0.49);
            c.Texto($"#{nroComp:D6}", 410, 146, 2, 9.5, 0.15, 0.39, 0.92);
            c.Texto("Medio de Pago:", 317, 165, 2, 9, 0.35, 0.41, 0.49);
            c.Texto(metodoPago, 410, 165, 2, 9.5, 0.12, 0.16, 0.22);
            c.Texto("Cajero / Emisor:", 317, 184, 2, 9, 0.35, 0.41, 0.49);
            c.Texto(!string.IsNullOrWhiteSpace(nombreCobrador) ? nombreCobrador : "Administracion", 410, 184, 1, 9, 0.12, 0.16, 0.22);
            c.Texto("Estado Acceso:", 317, 202, 2, 9, 0.35, 0.41, 0.49);
            c.Texto("HABILITADO - CUOTA AL DIA", 410, 202, 2, 8.5, 0.08, 0.50, 0.24);
            c.Texto("DETALLE DE MEMBRESIA Y PERIODO DE COBERTURA", 36, 242, 2, 10.5, 0.12, 0.16, 0.22);
            c.RectanguloRelleno(36, 260, 523, 26, 0.12, 0.16, 0.22);
            c.Texto("CONCEPTO / PLAN CONTRATADO", 46, 268, 2, 9, 1, 1, 1);
            c.Texto("DURACION", 240, 268, 2, 9, 1, 1, 1);
            c.Texto("INICIO VIGENCIA", 315, 268, 2, 9, 1, 1, 1);
            c.Texto("VENCIMIENTO", 408, 268, 2, 9, 1, 1, 1);
            c.TextoDerecha("IMPORTE ($)", 549, 268, 2, 9, 1, 1, 1);
            c.RectanguloConBorde(36, 286, 523, 38, 1, 1, 1, 0.85, 0.89, 0.94, 0.8);
            c.Texto(nombrePlan, 46, 294, 2, 10, 0.12, 0.16, 0.22);
            c.Texto("Acceso libre a musculacion, cardio y seguimiento", 46, 309, 1, 8, 0.45, 0.50, 0.58);
            c.Texto($"{duracionDias} dias", 240, 300, 1, 9.5, 0.12, 0.16, 0.22);
            c.Texto(fechaEmision.ToString("dd/MM/yyyy"), 315, 300, 1, 9.5, 0.12, 0.16, 0.22);
            c.Texto(fechaVenc.ToString("dd/MM/yyyy"), 408, 300, 2, 9.5, 0.08, 0.50, 0.24);
            c.TextoDerecha($"$ {monto:N2}", 549, 300, 2, 10, 0.12, 0.16, 0.22);
            decimal costoDiario = duracionDias > 0 ? Math.Round(monto / duracionDias, 2) : monto;
            c.RectanguloConBorde(36, 324, 523, 26, 0.97, 0.98, 0.99, 0.85, 0.89, 0.94, 0.8);
            c.Texto($"Tarifa diaria promedio: $ {costoDiario:N2} / dia  |  Incluye acceso a consultorio nutricional y diseno de rutinas", 46, 333, 1, 8.5, 0.35, 0.41, 0.49);
            c.RectanguloConBorde(315, 368, 244, 54, 0.93, 0.96, 1.0, 0.15, 0.39, 0.92, 1.2);
            c.Texto("TOTAL ABONADO:", 328, 380, 2, 10, 0.12, 0.23, 0.54);
            c.Texto($"Medio: {metodoPago}", 328, 398, 1, 8.5, 0.35, 0.41, 0.49);
            c.TextoDerecha($"$ {monto:N2}", 547, 384, 2, 15, 0.15, 0.39, 0.92);
            c.RectanguloConBorde(36, 444, 523, 95, 0.94, 0.99, 0.96, 0.65, 0.88, 0.74, 1.0);
            c.Texto("CONSTANCIA DE HABILITACION INMEDIATA", 48, 456, 2, 10, 0.08, 0.50, 0.24);
            c.Texto($"1. El socio titular {nomSocio} (DNI {dniSocio}) se encuentra habilitado hasta el {fechaVenc:dd/MM/yyyy} inclusive.", 48, 475, 1, 9, 0.12, 0.25, 0.18);
            c.Texto("2. Esta suscripcion habilita la asignacion y actualizacion de rutinas en el area de Entrenamiento.", 48, 493, 1, 9, 0.12, 0.25, 0.18);
            c.Texto("3. Habilita la reserva de turnos y evaluaciones antropometricas en el Consultorio Nutricional.", 48, 511, 1, 9, 0.12, 0.25, 0.18);
            c.Linea(370, 650, 535, 650, 0.45, 0.50, 0.58, 0.8);
            c.Texto("Firma y Sello - Recepcion UAI-FIT", 392, 658, 2, 8.5, 0.35, 0.41, 0.49);
            string dvh = comprobante != null && !string.IsNullOrWhiteSpace(comprobante.DVH)
                ? comprobante.DVH
                : "VERIFICADO-SISTEMA-UAIFIT";
            DibujarPieDePaginaCorporativo(c, dvh, 1, 1);
            GuardarPaginasPdf(rutaArchivo, new List<PdfCanvas> { c }, abrirAlFinalizar);
        }

        public static void GenerarInformeNutricionalCorporativoPDF(
            string rutaArchivo,
            SocioBE socio,
            EvaluacionBiometricaBE bio,
            string habitos,
            string nombreNutricionista,
            string dvhPlan = null,
            bool abrirAlFinalizar = true)
        {
            PdfCanvas c = new PdfCanvas();
            DibujarEncabezadoCorporativo(
                c,
                "DEPARTAMENTO DE NUTRICION CLINICA Y ANTROPOMETRIA DEPORTIVA",
                "INFORME BIOMETRICO Y PLAN ALIMENTARIO",
                $"DNI: {(socio != null ? socio.DNI : "-")}",
                $"Fecha: {DateTime.Now:dd/MM/yyyy}");
            c.RectanguloConBorde(36, 108, 523, 64, 0.98, 0.99, 1.0, 0.85, 0.89, 0.94, 1.0);
            c.RectanguloRelleno(36, 108, 6, 64, 0.15, 0.39, 0.92);
            string nomSocio = socio != null ? $"{socio.Nombre} {socio.Apellido}".Trim() : "-";
            string genero = bio != null && !string.IsNullOrWhiteSpace(bio.Genero) ? bio.Genero : "-";
            int edad = bio != null ? bio.Edad : 0;
            string objetivo = bio != null && !string.IsNullOrWhiteSpace(bio.ObjetivoNutricional) ? bio.ObjetivoNutricional : "Mantenimiento";
            c.Texto("PACIENTE / SOCIO:", 52, 118, 2, 8.5, 0.45, 0.50, 0.58);
            c.Texto($"{nomSocio}  (DNI: {(socio != null ? socio.DNI : "-")})", 145, 118, 2, 10, 0.12, 0.16, 0.22);
            c.Texto("PERFIL BIOLOGICO:", 52, 136, 2, 8.5, 0.45, 0.50, 0.58);
            c.Texto($"{genero}  |  {edad} anos", 145, 136, 1, 9.5, 0.12, 0.16, 0.22);
            c.Texto("PROTOCOLO CLINICO:", 52, 154, 2, 8.5, 0.45, 0.50, 0.58);
            c.Texto(objetivo, 145, 154, 2, 9.5, 0.15, 0.39, 0.92);
            c.Texto("PROFESIONAL:", 365, 118, 2, 8.5, 0.45, 0.50, 0.58);
            c.Texto(!string.IsNullOrWhiteSpace(nombreNutricionista) ? nombreNutricionista : "Nutricionista UAI-FIT", 440, 118, 2, 9.5, 0.12, 0.16, 0.22);
            c.Texto("CONTACTO SOCIO:", 365, 136, 2, 8.5, 0.45, 0.50, 0.58);
            c.Texto(socio != null && !string.IsNullOrWhiteSpace(socio.Telefono) ? socio.Telefono : "-", 450, 136, 1, 9, 0.12, 0.16, 0.22);
            c.Texto("INDICADORES ANTROPOMETRICOS Y METABOLICOS", 36, 186, 2, 10.5, 0.12, 0.16, 0.22);
            decimal peso = bio != null ? bio.PesoKg : 0m;
            decimal altura = bio != null ? bio.AlturaCm : 0m;
            decimal imc = bio != null ? bio.IndiceMasaCorporal : 0m;
            string diagImc = bio != null && !string.IsNullOrWhiteSpace(bio.DiagnosticoIMC) ? bio.DiagnosticoIMC : "-";
            decimal tmb = bio != null ? bio.TasaMetabolicaBasal : 0;
            int kcalObj = bio != null ? Convert.ToInt32(bio.CaloriasObjetivo) : 0;
            c.RectanguloConBorde(36, 204, 123, 66, 1, 1, 1, 0.85, 0.89, 0.94, 1.0);
            c.RectanguloRelleno(36, 204, 123, 4, 0.15, 0.39, 0.92);
            c.Texto("PESO / ALTURA", 46, 214, 2, 8, 0.45, 0.50, 0.58);
            c.Texto($"{peso:0.#} kg", 46, 230, 2, 14, 0.12, 0.16, 0.22);
            c.Texto($"Estatura: {altura:0} cm", 46, 252, 1, 8.5, 0.35, 0.41, 0.49);
            bool imcNormal = imc >= 18.5m && imc < 25m;
            bool imcAlerta = imc < 18.5m || imc >= 30m;
            double rImc = imcNormal ? 0.08 : (imcAlerta ? 0.80 : 0.85);
            double gImc = imcNormal ? 0.55 : (imcAlerta ? 0.15 : 0.45);
            double bImc = imcNormal ? 0.28 : (imcAlerta ? 0.15 : 0.06);
            c.RectanguloConBorde(169, 204, 123, 66, 1, 1, 1, 0.85, 0.89, 0.94, 1.0);
            c.RectanguloRelleno(169, 204, 123, 4, rImc, gImc, bImc);
            c.Texto("INDICE MASA CORP.", 179, 214, 2, 8, 0.45, 0.50, 0.58);
            c.Texto($"{imc:0.00}", 179, 230, 2, 14, rImc, gImc, bImc);
            c.Texto(diagImc, 179, 252, 2, 8.5, rImc, gImc, bImc);
            c.RectanguloConBorde(302, 204, 123, 66, 1, 1, 1, 0.85, 0.89, 0.94, 1.0);
            c.RectanguloRelleno(302, 204, 123, 4, 0.40, 0.45, 0.55);
            c.Texto("METABOLISMO BASAL", 312, 214, 2, 8, 0.45, 0.50, 0.58);
            c.Texto($"{tmb} kcal", 312, 230, 2, 14, 0.12, 0.16, 0.22);
            c.Texto("Formula Mifflin-St Jeor", 312, 252, 1, 8, 0.45, 0.50, 0.58);
            c.RectanguloConBorde(436, 204, 123, 66, 0.94, 0.98, 1.0, 0.15, 0.39, 0.92, 1.0);
            c.RectanguloRelleno(436, 204, 123, 4, 0.15, 0.39, 0.92);
            c.Texto("TARGET ENERGETICO", 446, 214, 2, 8, 0.15, 0.39, 0.92);
            c.Texto($"{kcalObj} kcal", 446, 230, 2, 14, 0.15, 0.39, 0.92);
            c.Texto("Requerimiento diario", 446, 252, 1, 8.5, 0.25, 0.35, 0.55);
            int gProt = (int)Math.Round(peso * 1.8m);
            int gGrasas = (int)Math.Round(peso * 0.9m);
            int kcalRest = Math.Max(400, kcalObj - (gProt * 4) - (gGrasas * 9));
            int gCarbs = kcalRest / 4;
            decimal litrosAgua = Math.Round(peso * 0.035m, 1);
            c.RectanguloConBorde(36, 284, 523, 86, 0.98, 0.99, 1.0, 0.85, 0.89, 0.94, 1.0);
            c.Texto("DISTRIBUCION DIARIA DE MACRONUTRIENTES E HIDRATACION", 48, 294, 2, 9.5, 0.12, 0.16, 0.22);
            c.TextoDerecha($"Hidratacion minima: {litrosAgua:0.0} Litros/dia", 547, 294, 2, 9, 0.15, 0.39, 0.92);
            double totalGramos = Math.Max(1, gProt + gCarbs + gGrasas);
            double wProt = Math.Max(25, (gProt / totalGramos) * 320.0);
            double wCarbs = Math.Max(25, (gCarbs / totalGramos) * 320.0);
            double wGrasas = Math.Max(25, (gGrasas / totalGramos) * 320.0);
            c.Texto("Proteinas (Sintesis):", 48, 314, 2, 8.5, 0.25, 0.30, 0.38);
            c.RectanguloRelleno(165, 314, 320, 10, 0.91, 0.93, 0.96);
            c.RectanguloRelleno(165, 314, wProt, 10, 0.15, 0.39, 0.92);
            c.Texto($"{gProt} g/dia", 495, 313, 2, 8.5, 0.12, 0.16, 0.22);
            c.Texto("Carbohidratos (Energia):", 48, 332, 2, 8.5, 0.25, 0.30, 0.38);
            c.RectanguloRelleno(165, 332, 320, 10, 0.91, 0.93, 0.96);
            c.RectanguloRelleno(165, 332, wCarbs, 10, 0.08, 0.65, 0.45);
            c.Texto($"{gCarbs} g/dia", 495, 331, 2, 8.5, 0.12, 0.16, 0.22);
            c.Texto("Grasas Saludables:", 48, 350, 2, 8.5, 0.25, 0.30, 0.38);
            c.RectanguloRelleno(165, 350, 320, 10, 0.91, 0.93, 0.96);
            c.RectanguloRelleno(165, 350, wGrasas, 10, 0.92, 0.58, 0.08);
            c.Texto($"{gGrasas} g/dia", 495, 349, 2, 8.5, 0.12, 0.16, 0.22);
            c.Texto("PRESCRIPCION NUTRICIONAL Y PAUTAS DE CONDUCTA", 36, 386, 2, 10.5, 0.12, 0.16, 0.22);
            c.RectanguloConBorde(36, 404, 523, 265, 1, 1, 1, 0.82, 0.86, 0.91, 1.0);
            c.RectanguloRelleno(36, 404, 523, 24, 0.12, 0.16, 0.22);
            c.Texto("INDICACIONES CLINICAS PERSONALIZADAS", 48, 411, 2, 9, 1, 1, 1);
            List<string> lineasHabitos = EnvolverTexto(habitos ?? string.Empty, 86);
            double yTexto = 438;
            foreach (string linea in lineasHabitos)
            {
                if (yTexto > 652) break;
                bool esTitulo = linea.TrimStart().StartsWith("[") || linea.TrimStart().StartsWith("-");
                c.Texto(linea, 48, yTexto, esTitulo ? 2 : 1, 9.2, 0.15, 0.20, 0.28);
                yTexto += 15;
            }
            c.Linea(365, 706, 545, 706, 0.45, 0.50, 0.58, 0.8);
            c.Texto($"Firma Profesional: {(!string.IsNullOrWhiteSpace(nombreNutricionista) ? nombreNutricionista : "Nutricionista")}", 375, 712, 2, 8.5, 0.35, 0.41, 0.49);
            DibujarPieDePaginaCorporativo(c, !string.IsNullOrWhiteSpace(dvhPlan) ? dvhPlan : "INFORME-CLINICO-UAIFIT-VERIFICADO", 1, 1);
            GuardarPaginasPdf(rutaArchivo, new List<PdfCanvas> { c }, abrirAlFinalizar);
        }

        public static void GenerarReporteArqueoCajaPDF(
            string rutaArchivo,
            DateTime desde,
            DateTime hasta,
            string filtroDNI,
            string filtroMetodo,
            List<ComprobantePagoBE> comprobantes,
            string usuarioAuditor,
            bool abrirAlFinalizar = true)
        {
            if (comprobantes == null) comprobantes = new List<ComprobantePagoBE>();
            decimal totalGeneral = comprobantes.Sum(x => x.MontoTotal);
            int cantTotal = comprobantes.Count;
            decimal totalEfectivo = comprobantes
                .Where(x => x.MetodoPago != null && x.MetodoPago.IndexOf("Efectivo", StringComparison.OrdinalIgnoreCase) >= 0)
                .Sum(x => x.MontoTotal);
            decimal totalDigital = totalGeneral - totalEfectivo;
            decimal ticketPromedio = cantTotal > 0 ? Math.Round(totalGeneral / cantTotal, 2) : 0m;
            int filasPrimeraPagina = 14;
            int filasPorPaginaExtra = 24;
            int totalPaginas = 1;
            if (cantTotal > filasPrimeraPagina)
            {
                totalPaginas += (int)Math.Ceiling((cantTotal - filasPrimeraPagina) / (double)filasPorPaginaExtra);
            }
            List<PdfCanvas> paginas = new List<PdfCanvas>();
            PdfCanvas c1 = new PdfCanvas();
            DibujarEncabezadoCorporativo(
                c1,
                "DEPARTAMENTO DE ADMINISTRACION Y AUDITORIA CONTABLE",
                "REPORTE DE ARQUEO DE CAJA E INGRESOS",
                $"OPERACIONES: {cantTotal}",
                $"Periodo: {desde:dd/MM/yy} - {hasta:dd/MM/yy}");
            c1.RectanguloConBorde(36, 104, 523, 28, 0.96, 0.97, 0.99, 0.85, 0.89, 0.94, 0.8);
            string dniTxt = string.IsNullOrWhiteSpace(filtroDNI) ? "Todos" : filtroDNI;
            string metTxt = string.IsNullOrWhiteSpace(filtroMetodo) ? "Todos" : filtroMetodo;
            c1.Texto($"Periodo Auditado: {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}   |   Filtro DNI: {dniTxt}   |   Medio: {metTxt}   |   Auditor: {usuarioAuditor}", 46, 113, 2, 8.5, 0.25, 0.30, 0.38);
            c1.RectanguloConBorde(36, 142, 123, 64, 0.94, 0.98, 1.0, 0.15, 0.39, 0.92, 1.0);
            c1.RectanguloRelleno(36, 142, 123, 4, 0.15, 0.39, 0.92);
            c1.Texto("RECAUDACION TOTAL", 44, 152, 2, 7.8, 0.15, 0.39, 0.92);
            c1.Texto($"$ {totalGeneral:N2}", 44, 168, 2, 12, 0.12, 0.16, 0.22);
            c1.Texto($"{cantTotal} comprobante(s)", 44, 189, 1, 8, 0.35, 0.41, 0.49);
            decimal pctEfec = totalGeneral > 0 ? Math.Round(totalEfectivo * 100m / totalGeneral, 1) : 0m;
            c1.RectanguloConBorde(169, 142, 123, 64, 1, 1, 1, 0.85, 0.89, 0.94, 1.0);
            c1.RectanguloRelleno(169, 142, 123, 4, 0.08, 0.58, 0.32);
            c1.Texto("CAJA FISICA (EFECTIVO)", 177, 152, 2, 7.5, 0.08, 0.50, 0.24);
            c1.Texto($"$ {totalEfectivo:N2}", 177, 168, 2, 12, 0.08, 0.50, 0.24);
            c1.Texto($"{pctEfec:0.#}% del total", 177, 189, 1, 8, 0.35, 0.41, 0.49);
            decimal pctDig = totalGeneral > 0 ? Math.Round(totalDigital * 100m / totalGeneral, 1) : 0m;
            c1.RectanguloConBorde(302, 142, 123, 64, 1, 1, 1, 0.85, 0.89, 0.94, 1.0);
            c1.RectanguloRelleno(302, 142, 123, 4, 0.45, 0.25, 0.85);
            c1.Texto("MEDIOS DIGITALES", 310, 152, 2, 7.8, 0.40, 0.20, 0.75);
            c1.Texto($"$ {totalDigital:N2}", 310, 168, 2, 12, 0.12, 0.16, 0.22);
            c1.Texto($"{pctDig:0.#}% del total", 310, 189, 1, 8, 0.35, 0.41, 0.49);
            c1.RectanguloConBorde(436, 142, 123, 64, 1, 1, 1, 0.85, 0.89, 0.94, 1.0);
            c1.RectanguloRelleno(436, 142, 123, 4, 0.90, 0.52, 0.06);
            c1.Texto("TICKET PROMEDIO", 444, 152, 2, 7.8, 0.70, 0.38, 0.05);
            c1.Texto($"$ {ticketPromedio:N2}", 444, 168, 2, 12, 0.12, 0.16, 0.22);
            c1.Texto("Promedio por venta", 444, 189, 1, 8, 0.35, 0.41, 0.49);
            c1.RectanguloConBorde(36, 218, 523, 96, 0.98, 0.99, 1.0, 0.85, 0.89, 0.94, 1.0);
            c1.Texto("DESGLOSE POR MEDIO DE PAGO", 48, 226, 2, 9, 0.12, 0.16, 0.22);
            var gruposMetodo = comprobantes
                .GroupBy(x => string.IsNullOrWhiteSpace(x.MetodoPago) ? "Otro" : x.MetodoPago)
                .Select(g => new { Metodo = g.Key, Monto = g.Sum(x => x.MontoTotal), Cant = g.Count() })
                .OrderByDescending(g => g.Monto)
                .Take(4)
                .ToList();
            double yBarra = 244;
            foreach (var gm in gruposMetodo)
            {
                decimal pct = totalGeneral > 0 ? Math.Round(gm.Monto * 100m / totalGeneral, 1) : 0m;
                double anchoBarra = totalGeneral > 0 ? Math.Max(8, (double)(gm.Monto / totalGeneral) * 250.0) : 8;
                c1.Texto($"{gm.Metodo} ({gm.Cant})", 48, yBarra, 2, 8.2, 0.25, 0.30, 0.38);
                c1.RectanguloRelleno(175, yBarra + 1, 250, 9, 0.91, 0.93, 0.96);
                c1.RectanguloRelleno(175, yBarra + 1, anchoBarra, 9, 0.15, 0.39, 0.92);
                c1.TextoDerecha($"$ {gm.Monto:N2} ({pct:0.#}%)", 547, yBarra, 2, 8.2, 0.12, 0.16, 0.22);
                yBarra += 16;
            }
            c1.Texto("DETALLE DE COMPROBANTES INCLUIDOS EN EL ARQUEO", 36, 326, 2, 10, 0.12, 0.16, 0.22);
            DibujarEncabezadoTablaArqueo(c1, 342);
            var listaOrdenada = comprobantes.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.NroComprobante).ToList();
            double yFila = 366;
            int idx = 0;
            while (idx < Math.Min(cantTotal, filasPrimeraPagina))
            {
                DibujarFilaComprobanteArqueo(c1, listaOrdenada[idx], yFila, idx % 2 == 1);
                yFila += 24;
                idx++;
            }
            DibujarPieDePaginaCorporativo(c1, $"ARQUEO-CAJA-{DateTime.Now:yyyyMMddHHmmss}-TOTAL-{totalGeneral:0.00}", 1, totalPaginas);
            paginas.Add(c1);
            int nroPag = 2;
            while (idx < cantTotal)
            {
                PdfCanvas cp = new PdfCanvas();
                DibujarEncabezadoCorporativo(
                    cp,
                    "DEPARTAMENTO DE ADMINISTRACION Y AUDITORIA CONTABLE",
                    "CONTINUACION DETALLE DE COMPROBANTES",
                    $"PAGINA {nroPag}/{totalPaginas}",
                    $"Periodo: {desde:dd/MM/yy} - {hasta:dd/MM/yy}");
                DibujarEncabezadoTablaArqueo(cp, 110);
                double yExtra = 134;
                int contPag = 0;
                while (idx < cantTotal && contPag < filasPorPaginaExtra)
                {
                    DibujarFilaComprobanteArqueo(cp, listaOrdenada[idx], yExtra, idx % 2 == 1);
                    yExtra += 24;
                    idx++;
                    contPag++;
                }
                DibujarPieDePaginaCorporativo(cp, $"ARQUEO-CAJA-PAGINA-{nroPag}", nroPag, totalPaginas);
                paginas.Add(cp);
                nroPag++;
            }
            GuardarPaginasPdf(rutaArchivo, paginas, abrirAlFinalizar);
        }

        private static void DibujarEncabezadoTablaArqueo(PdfCanvas c, double yTop)
        {
            c.RectanguloRelleno(36, yTop, 523, 24, 0.12, 0.16, 0.22);
            c.Texto("NRO.", 44, yTop + 7, 2, 8.5, 1, 1, 1);
            c.Texto("FECHA Y HORA", 95, yTop + 7, 2, 8.5, 1, 1, 1);
            c.Texto("DNI SOCIO", 190, yTop + 7, 2, 8.5, 1, 1, 1);
            c.Texto("SOCIO TITULAR", 255, yTop + 7, 2, 8.5, 1, 1, 1);
            c.Texto("MEDIO DE PAGO", 380, yTop + 7, 2, 8.5, 1, 1, 1);
            c.TextoDerecha("MONTO ($)", 549, yTop + 7, 2, 8.5, 1, 1, 1);
        }

        private static void DibujarFilaComprobanteArqueo(PdfCanvas c, ComprobantePagoBE comp, double yTop, bool filaAlterna)
        {
            double rBg = filaAlterna ? 0.97 : 1.0;
            double gBg = filaAlterna ? 0.98 : 1.0;
            double bBg = filaAlterna ? 0.99 : 1.0;
            c.RectanguloConBorde(36, yTop, 523, 24, rBg, gBg, bBg, 0.88, 0.91, 0.95, 0.5);
            c.Texto($"#{comp.NroComprobante:D5}", 44, yTop + 7, 2, 8.5, 0.15, 0.39, 0.92);
            c.Texto(comp.Fecha.ToString("dd/MM/yyyy HH:mm"), 95, yTop + 7, 1, 8.5, 0.12, 0.16, 0.22);
            c.Texto(comp.Socio != null ? comp.Socio.DNI : "-", 190, yTop + 7, 1, 8.5, 0.12, 0.16, 0.22);
            string nomSocio = comp.Socio != null ? $"{comp.Socio.Nombre} {comp.Socio.Apellido}".Trim() : "-";
            if (nomSocio.Length > 22) nomSocio = nomSocio.Substring(0, 22) + ".";
            c.Texto(nomSocio, 255, yTop + 7, 1, 8.5, 0.12, 0.16, 0.22);
            c.Texto(comp.MetodoPago ?? "-", 380, yTop + 7, 1, 8.5, 0.25, 0.30, 0.38);
            c.TextoDerecha($"$ {comp.MontoTotal:N2}", 549, yTop + 7, 2, 8.5, 0.12, 0.16, 0.22);
        }

        private static List<string> EnvolverTexto(string texto, int maxCharsPorLinea)
        {
            List<string> resultado = new List<string>();
            if (string.IsNullOrEmpty(texto)) return resultado;
            string[] parrafos = texto.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            foreach (string p in parrafos)
            {
                if (string.IsNullOrWhiteSpace(p))
                {
                    resultado.Add("");
                    continue;
                }
                string lineaActual = p;
                while (lineaActual.Length > maxCharsPorLinea)
                {
                    int corte = lineaActual.LastIndexOf(' ', maxCharsPorLinea);
                    if (corte <= 0) corte = maxCharsPorLinea;
                    resultado.Add(lineaActual.Substring(0, corte));
                    lineaActual = "   " + lineaActual.Substring(corte).TrimStart();
                }
                if (lineaActual.Length > 0) resultado.Add(lineaActual);
            }
            return resultado;
        }

        private static void GuardarPaginasPdf(string rutaArchivo, List<PdfCanvas> paginas, bool abrirAlFinalizar)
        {
            using (FileStream fs = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write))
            {
                void WriteAscii(string s)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                    fs.Write(bytes, 0, bytes.Length);
                }
                WriteAscii("%PDF-1.4\n");
                List<long> offsets = new List<long> { 0 };
                offsets.Add(fs.Position);
                WriteAscii("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");
                int firstPageObjId = 6;
                List<string> kidsRefs = new List<string>();
                for (int i = 0; i < paginas.Count; i++)
                {
                    kidsRefs.Add($"{firstPageObjId + (i * 2)} 0 R");
                }
                offsets.Add(fs.Position);
                WriteAscii($"2 0 obj\n<< /Type /Pages /Kids [ {string.Join(" ", kidsRefs)} ] /Count {paginas.Count} >>\nendobj\n");
                offsets.Add(fs.Position);
                WriteAscii("3 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>\nendobj\n");
                offsets.Add(fs.Position);
                WriteAscii("4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>\nendobj\n");
                offsets.Add(fs.Position);
                WriteAscii("5 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Courier /Encoding /WinAnsiEncoding >>\nendobj\n");
                for (int i = 0; i < paginas.Count; i++)
                {
                    int pageObjId = firstPageObjId + (i * 2);
                    int contentObjId = pageObjId + 1;
                    offsets.Add(fs.Position);
                    WriteAscii($"{pageObjId} 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 3 0 R /F2 4 0 R /F3 5 0 R >> >> /Contents {contentObjId} 0 R >>\nendobj\n");
                    byte[] streamBytes = Encoding.ASCII.GetBytes(paginas[i].ObtenerContenido());
                    offsets.Add(fs.Position);
                    WriteAscii($"{contentObjId} 0 obj\n<< /Length {streamBytes.Length} >>\nstream\n");
                    fs.Write(streamBytes, 0, streamBytes.Length);
                    WriteAscii("\nendstream\nendobj\n");
                }
                long xrefPos = fs.Position;
                int totalObjs = offsets.Count;
                WriteAscii($"xref\n0 {totalObjs}\n0000000000 65535 f \n");
                for (int i = 1; i < totalObjs; i++)
                {
                    WriteAscii($"{offsets[i]:D10} 00000 n \n");
                }
                WriteAscii($"trailer\n<< /Size {totalObjs} /Root 1 0 R >>\nstartxref\n{xrefPos}\n%%EOF");
            }

            if (abrirAlFinalizar && File.Exists(rutaArchivo))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = rutaArchivo,
                        UseShellExecute = true
                    });
                }
                catch { }
            }
        }
    }
}