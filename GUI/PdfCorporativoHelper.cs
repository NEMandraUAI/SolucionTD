using BE;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using QContainer = QuestPDF.Infrastructure.IContainer;
using QLicenseType = QuestPDF.Infrastructure.LicenseType;
using QPageSizes = QuestPDF.Helpers.PageSizes;
using QUnit = QuestPDF.Infrastructure.Unit;

namespace GUI
{
    public static class PdfCorporativoHelper
    {
        private const string FuenteCorporativa = "Lato";
        private const string ColorOscuroInstitucional = "#1F2937";
        private const string ColorAzulPrimario = "#2563EB";
        private const string ColorAzulClaroFondo = "#EFF6FF";
        private const string ColorVerdeExito = "#15803D";
        private const string ColorVerdeClaroFondo = "#F0FDF4";
        private const string ColorAmbarAlerta = "#B45309";
        private const string ColorAmbarClaroFondo = "#FEF3C7";
        private const string ColorRojoCritico = "#B91C1C";
        private const string ColorRojoClaroFondo = "#FEE2E2";
        private const string ColorVioletaAcento = "#6D28D9";
        private const string ColorGrisTextoSecundario = "#475569";
        private const string ColorGrisBorde = "#CBD5E1";
        private const string ColorGrisCebra = "#F8FAFC";

        static PdfCorporativoHelper()
        {
            QuestPDF.Settings.License = QLicenseType.Community;
            QuestPDF.Settings.UseSystemFonts = true;
            QuestPDF.Settings.ThrowOnMissingFontFamilies = false;
            QuestPDF.Settings.ThrowOnMissingTextGlyphs = false;
        }

        private static string LimpiarTextoPdf(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            return texto
                .Replace("🚨 ", "[CRÍTICO] ")
                .Replace("⚠️ ", "[ALERTA] ")
                .Replace("✅ ", "[ÓPTIMO] ")
                .Replace("❄️ ", "[SIN HISTORIAL] ")
                .Replace("📈 ", "[+] ")
                .Replace("📉 ", "[-] ")
                .Replace("➡️ ", "[=] ")
                .Replace("🚨", "")
                .Replace("⚠️", "")
                .Replace("✅", "")
                .Replace("❄️", "")
                .Replace("📈", "")
                .Replace("📉", "")
                .Replace("➡️", "");
        }

        private static void ComponerEncabezadoCorporativo(
            QContainer container,
            string subtituloArea,
            string tituloDocumento,
            string badgePrincipal,
            string subBadgeDerecha)
        {
            container.Column(col =>
            {
                col.Item().Background(ColorOscuroInstitucional).PaddingHorizontal(20).PaddingVertical(14).Row(row =>
                {
                    row.ConstantItem(46).Height(46).Background(ColorAzulPrimario).AlignCenter().AlignMiddle().Column(logoCol =>
                    {
                        logoCol.Item().AlignCenter().Text("UAI").FontSize(12).Bold().FontColor("#FFFFFF");
                        logoCol.Item().AlignCenter().Text("FIT").FontSize(10).Bold().FontColor("#BFDBFE");
                    });
                    row.ConstantItem(14);
                    row.RelativeItem().AlignMiddle().Column(tCol =>
                    {
                        tCol.Item().Text("UAI-FIT  |  HIGH PERFORMANCE GYM & HEALTH CENTER")
                            .FontSize(9.5f).Bold().FontColor("#E2E8F0");
                        tCol.Item().PaddingTop(1).Text(subtituloArea)
                            .FontSize(8.5f).FontColor("#93C5FD");
                        tCol.Item().PaddingTop(3).Text(tituloDocumento)
                            .FontSize(14).Bold().FontColor("#FFFFFF");
                    });
                    row.ConstantItem(155).Border(1).BorderColor("#475569").Background("#111827")
                        .PaddingHorizontal(10).PaddingVertical(6).AlignMiddle().Column(bCol =>
                        {
                            bCol.Item().AlignRight().Text(badgePrincipal).FontSize(10.5f).Bold().FontColor("#FFFFFF");
                            bCol.Item().PaddingTop(2).AlignRight().Text(subBadgeDerecha).FontSize(8f).FontColor("#CBD5E1");
                        });
                });
                col.Item().Height(4).Background(ColorAzulPrimario);
            });
        }

        private static void ComponerPieDePaginaCorporativo(QContainer container, string dvhSello)
        {
            container.Column(col =>
            {
                if (!string.IsNullOrWhiteSpace(dvhSello))
                {
                    col.Item().PaddingBottom(6).Border(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra)
                        .PaddingHorizontal(10).PaddingVertical(5).Column(sCol =>
                        {
                            sCol.Item().Text("SELLO DE INTEGRIDAD CRIPTOGRÁFICO (SHA-256 DVH):")
                                .FontSize(7.5f).Bold().FontColor(ColorGrisTextoSecundario);
                            sCol.Item().PaddingTop(1).Text(dvhSello)
                                .FontSize(8f).Bold().FontColor(ColorOscuroInstitucional);
                        });
                }
                col.Item().BorderTop(1).BorderColor(ColorGrisBorde).PaddingTop(4).Row(row =>
                {
                    row.RelativeItem().Text("UAI-FIT Sistema Integral de Gestión Deportiva, Clínica y Comercial  |  Documento Oficial Auditable (QuestPDF)")
                        .FontSize(7.5f).FontColor(ColorGrisTextoSecundario);
                    row.ConstantItem(175).AlignRight().Text(txt =>
                    {
                        txt.DefaultTextStyle(s => s.FontSize(7.5f).FontColor(ColorGrisTextoSecundario));
                        txt.Span("Pág. ");
                        txt.CurrentPageNumber();
                        txt.Span(" de ");
                        txt.TotalPages();
                        txt.Span($"  —  {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            });
        }

        private static void ComponerTarjetaKpi(
            QContainer container,
            string titulo,
            string valorPrincipal,
            string subtitulo,
            string colorAcentoHex,
            string colorFondoHex = "#FFFFFF")
        {
            container.Border(1).BorderColor(ColorGrisBorde).Background(colorFondoHex).Column(col =>
            {
                col.Item().Height(4).Background(colorAcentoHex);
                col.Item().PaddingHorizontal(10).PaddingVertical(7).Column(inner =>
                {
                    inner.Item().Text(titulo).FontSize(7.5f).Bold().FontColor(ColorGrisTextoSecundario);
                    inner.Item().PaddingTop(3).Text(valorPrincipal).FontSize(13f).Bold().FontColor(colorAcentoHex);
                    inner.Item().PaddingTop(2).Text(subtitulo).FontSize(7.8f).FontColor(ColorGrisTextoSecundario);
                });
            });
        }

        private static void GuardarYAbrirPdf(Document documento, string rutaArchivo, bool abrirAlFinalizar)
        {
            byte[] pdfBytes = documento.GeneratePdf();
            File.WriteAllBytes(rutaArchivo, pdfBytes);
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

        public static void GenerarReciboSuscripcionPDF(
            string rutaArchivo,
            ComprobantePagoBE comprobante,
            SocioBE socio,
            PlanSuscripcionBE plan,
            string nombreCobrador,
            bool abrirAlFinalizar = true)
        {
            int nroComp = comprobante != null ? comprobante.NroComprobante : 1;
            DateTime fechaEmision = comprobante != null && comprobante.Fecha != default ? comprobante.Fecha : DateTime.Now;
            decimal monto = comprobante != null ? comprobante.MontoTotal : (plan != null ? plan.Precio : 0m);
            string metodoPago = comprobante != null && !string.IsNullOrWhiteSpace(comprobante.MetodoPago) ? comprobante.MetodoPago : "Efectivo";
            string nombrePlan = plan != null && !string.IsNullOrWhiteSpace(plan.Nombre) ? plan.Nombre : "Plan de Suscripción";
            int duracionDias = plan != null && plan.DuracionDias > 0 ? plan.DuracionDias : 30;
            DateTime fechaVenc = fechaEmision.Date.AddDays(duracionDias);
            decimal costoDiario = duracionDias > 0 ? Math.Round(monto / duracionDias, 2) : monto;
            string nomSocio = socio != null ? $"{socio.Nombre} {socio.Apellido}".Trim() : "-";
            string dniSocio = socio != null && !string.IsNullOrWhiteSpace(socio.DNI) ? socio.DNI : "-";
            string telSocio = socio != null && !string.IsNullOrWhiteSpace(socio.Telefono) ? socio.Telefono : "No informado";
            string emailSocio = socio != null && !string.IsNullOrWhiteSpace(socio.Email) ? socio.Email : "No informado";
            string cajero = !string.IsNullOrWhiteSpace(nombreCobrador) ? nombreCobrador : "Administración";
            string dvh = comprobante != null && !string.IsNullOrWhiteSpace(comprobante.DVH) ? comprobante.DVH : "VERIFICADO-SISTEMA-UAIFIT";
            var doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(QPageSizes.A4);
                    page.Margin(28, QUnit.Point);
                    page.PageColor("#FFFFFF");
                    page.DefaultTextStyle(x => x.FontFamily(FuenteCorporativa).FontSize(9.5f).FontColor(ColorOscuroInstitucional));
                    page.Header().Element(h => ComponerEncabezadoCorporativo(
                        h,
                        "DEPARTAMENTO DE RECEPCIÓN Y ADMINISTRACIÓN DE SOCIOS",
                        "RECIBO OFICIAL DE SUSCRIPCIÓN",
                        $"RECIBO #{nroComp:D6}",
                        $"Fecha: {fechaEmision:dd/MM/yyyy HH:mm}"));
                    page.Content().PaddingVertical(14).Column(col =>
                    {
                        col.Spacing(14);
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).BorderColor(ColorGrisBorde).Column(left =>
                            {
                                left.Item().Background(ColorGrisCebra).PaddingHorizontal(10).PaddingVertical(6)
                                    .Text("DATOS DEL SOCIO TITULAR").FontSize(9f).Bold();
                                left.Item().Padding(10).Column(info =>
                                {
                                    info.Spacing(4);
                                    info.Item().Text(t => { t.Span("Socio Titular: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(nomSocio).Bold(); });
                                    info.Item().Text(t => { t.Span("DNI / Identificación: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(dniSocio); });
                                    info.Item().Text(t => { t.Span("Teléfono: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(telSocio); });
                                    info.Item().Text(t => { t.Span("Correo Electrónico: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(emailSocio); });
                                });
                            });
                            row.ConstantItem(14);
                            row.RelativeItem().Border(1).BorderColor(ColorGrisBorde).Column(right =>
                            {
                                right.Item().Background(ColorGrisCebra).PaddingHorizontal(10).PaddingVertical(6)
                                    .Text("CONDICIONES DE LA TRANSACCIÓN").FontSize(9f).Bold();
                                right.Item().Padding(10).Column(info =>
                                {
                                    info.Spacing(4);
                                    info.Item().Text(t => { t.Span("Nro. Operación: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span($"#{nroComp:D6}").Bold().FontColor(ColorAzulPrimario); });
                                    info.Item().Text(t => { t.Span("Medio de Pago: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(metodoPago).Bold(); });
                                    info.Item().Text(t => { t.Span("Cajero / Emisor: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(cajero); });
                                    info.Item().Text(t => { t.Span("Estado de Acceso: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span("HABILITADO — CUOTA AL DÍA").Bold().FontColor(ColorVerdeExito); });
                                });
                            });
                        });
                        col.Item().Text("DETALLE DE MEMBRESÍA Y PERÍODO DE COBERTURA").FontSize(10f).Bold();
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3.2f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.5f);
                            });
                            table.Header(header =>
                            {
                                header.Cell().Background(ColorOscuroInstitucional).Padding(7).Text("CONCEPTO / PLAN CONTRATADO").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(7).AlignCenter().Text("DURACIÓN").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(7).AlignCenter().Text("INICIO VIGENCIA").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(7).AlignCenter().Text("VENCIMIENTO").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(7).AlignRight().Text("IMPORTE ($)").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                            });
                            table.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(8).Column(c =>
                            {
                                c.Item().Text(nombrePlan).FontSize(10f).Bold();
                                c.Item().Text("Acceso libre a salón de musculación, cardio, rutinas y consultorio nutricional").FontSize(8f).FontColor(ColorGrisTextoSecundario);
                            });
                            table.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(8).AlignCenter().AlignMiddle().Text($"{duracionDias} días");
                            table.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(8).AlignCenter().AlignMiddle().Text($"{fechaEmision:dd/MM/yyyy}");
                            table.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(8).AlignCenter().AlignMiddle().Text($"{fechaVenc:dd/MM/yyyy}").Bold().FontColor(ColorVerdeExito);
                            table.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(8).AlignRight().AlignMiddle().Text($"$ {monto:N2}").FontSize(10f).Bold();
                        });
                        col.Item().Background(ColorGrisCebra).Border(1).BorderColor(ColorGrisBorde).Padding(8)
                            .Text($"Tarifa diaria promedio: $ {costoDiario:N2} / día  |  Incluye seguimiento personalizado de entrenamiento y nutrición clínica.")
                            .FontSize(8.5f).FontColor(ColorGrisTextoSecundario);
                        col.Item().AlignRight().Width(240).Border(1.5f).BorderColor(ColorAzulPrimario).Background(ColorAzulClaroFondo)
                            .Padding(12).Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("TOTAL ABONADO").FontSize(9f).Bold().FontColor(ColorAzulPrimario);
                                    c.Item().Text($"Medio: {metodoPago}").FontSize(8f).FontColor(ColorGrisTextoSecundario);
                                });
                                row.RelativeItem().AlignRight().AlignMiddle().Text($"$ {monto:N2}").FontSize(14f).Bold().FontColor(ColorAzulPrimario);
                            });
                        col.Item().Border(1).BorderColor("#86EFAC").Background(ColorVerdeClaroFondo).Padding(12).Column(cert =>
                        {
                            cert.Spacing(4);
                            cert.Item().Text("CONSTANCIA DE HABILITACIÓN INMEDIATA").FontSize(9.5f).Bold().FontColor(ColorVerdeExito);
                            cert.Item().Text($"1. El socio titular {nomSocio} (DNI {dniSocio}) se encuentra habilitado para ingresar desde el {fechaEmision:dd/MM/yyyy} hasta el {fechaVenc:dd/MM/yyyy} inclusive.").FontSize(8.5f);
                            cert.Item().Text("2. Esta membresía habilita la asignación y actualización de rutinas en el área de Entrenamiento.").FontSize(8.5f);
                            cert.Item().Text("3. Habilita la reserva de turnos, controles antropométricos y compras en el Mercado Interno de UAI-FIT.").FontSize(8.5f);
                        });
                        col.Item().PaddingTop(30).AlignRight().Width(200).Column(firma =>
                        {
                            firma.Item().BorderTop(1).BorderColor(ColorGrisTextoSecundario).PaddingTop(4).AlignCenter()
                                .Text("Firma y Sello — Recepción UAI-FIT").FontSize(8.5f).Bold().FontColor(ColorGrisTextoSecundario);
                        });
                    });
                    page.Footer().Element(f => ComponerPieDePaginaCorporativo(f, dvh));
                });
            });
            GuardarYAbrirPdf(doc, rutaArchivo, abrirAlFinalizar);
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
            string nomSocio = socio != null ? $"{socio.Nombre} {socio.Apellido}".Trim() : "-";
            string dniSocio = socio != null && !string.IsNullOrWhiteSpace(socio.DNI) ? socio.DNI : "-";
            string telSocio = socio != null && !string.IsNullOrWhiteSpace(socio.Telefono) ? socio.Telefono : "-";
            string genero = bio != null && !string.IsNullOrWhiteSpace(bio.Genero) ? bio.Genero : "-";
            int edad = bio != null ? bio.Edad : 0;
            string objetivo = bio != null && !string.IsNullOrWhiteSpace(bio.ObjetivoNutricional) ? bio.ObjetivoNutricional : "Mantenimiento (Normocalórico)";
            string nutricionista = !string.IsNullOrWhiteSpace(nombreNutricionista) ? nombreNutricionista : "Nutricionista UAI-FIT";
            decimal peso = bio != null ? bio.PesoKg : 0m;
            decimal altura = bio != null ? bio.AlturaCm : 0m;
            decimal imc = bio != null ? bio.IndiceMasaCorporal : 0m;
            string diagImc = bio != null && !string.IsNullOrWhiteSpace(bio.DiagnosticoIMC) ? bio.DiagnosticoIMC : "-";
            decimal tmb = bio != null ? bio.TasaMetabolicaBasal : 0m;
            int kcalObj = bio != null ? Convert.ToInt32(bio.CaloriasObjetivo) : 0;
            int gProt = (int)Math.Round(peso * 1.8m);
            int gGrasas = (int)Math.Round(peso * 0.9m);
            int kcalRest = Math.Max(400, kcalObj - (gProt * 4) - (gGrasas * 9));
            int gCarbs = kcalRest / 4;
            decimal litrosAgua = Math.Round(peso * 0.035m, 1);
            bool imcNormal = imc >= 18.5m && imc < 25m;
            bool imcCritico = imc < 18.5m || imc >= 30m;
            string colorImc = imcNormal ? ColorVerdeExito : (imcCritico ? ColorRojoCritico : ColorAmbarAlerta);
            var doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(QPageSizes.A4);
                    page.Margin(28, QUnit.Point);
                    page.PageColor("#FFFFFF");
                    page.DefaultTextStyle(x => x.FontFamily(FuenteCorporativa).FontSize(9.5f).FontColor(ColorOscuroInstitucional));
                    page.Header().Element(h => ComponerEncabezadoCorporativo(
                        h,
                        "DEPARTAMENTO DE NUTRICIÓN CLÍNICA Y ANTROPOMETRÍA DEPORTIVA",
                        "INFORME BIOMÉTRICO Y PLAN ALIMENTARIO",
                        $"DNI: {dniSocio}",
                        $"Fecha: {DateTime.Now:dd/MM/yyyy}"));
                    page.Content().PaddingVertical(12).Column(col =>
                    {
                        col.Spacing(12);
                        col.Item().Border(1).BorderColor(ColorGrisBorde).BorderLeft(5).BorderColor(ColorAzulPrimario)
                            .Background(ColorGrisCebra).Padding(10).Row(row =>
                            {
                                row.RelativeItem(1.4f).Column(c1 =>
                                {
                                    c1.Spacing(3);
                                    c1.Item().Text(t => { t.Span("PACIENTE / SOCIO: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span($"{nomSocio} (DNI: {dniSocio})").Bold(); });
                                    c1.Item().Text(t => { t.Span("PERFIL BIOLÓGICO: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span($"{genero}  |  {edad} años"); });
                                    c1.Item().Text(t => { t.Span("PROTOCOLO CLÍNICO: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(objetivo).Bold().FontColor(ColorAzulPrimario); });
                                });
                                row.RelativeItem(1f).Column(c2 =>
                                {
                                    c2.Spacing(3);
                                    c2.Item().Text(t => { t.Span("PROFESIONAL: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(nutricionista).Bold(); });
                                    c2.Item().Text(t => { t.Span("CONTACTO SOCIO: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(telSocio); });
                                    c2.Item().Text(t => { t.Span("HIDRATACIÓN MÍNIMA: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span($"{litrosAgua:0.0} litros / día").Bold().FontColor(ColorVerdeExito); });
                                });
                            });
                        col.Item().Text("INDICADORES ANTROPOMÉTRICOS Y METABÓLICOS").FontSize(10f).Bold();
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "PESO / ESTATURA", $"{peso:0.#} kg", $"Altura: {altura:0} cm", ColorAzulPrimario));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "ÍNDICE MASA CORP.", $"{imc:0.00}", diagImc, colorImc));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "METABOLISMO BASAL", $"{tmb:0} kcal", "Fórmula Harris-Benedict", ColorGrisTextoSecundario));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "TARGET ENERGÉTICO", $"{kcalObj} kcal", "Requerimiento diario", ColorVioletaAcento, ColorAzulClaroFondo));
                        });
                        col.Item().Border(1).BorderColor(ColorGrisBorde).Padding(10).Column(mCol =>
                        {
                            mCol.Spacing(6);
                            mCol.Item().Text("DISTRIBUCIÓN DIARIA DE MACRONUTRIENTES SUGERIDA").FontSize(9.5f).Bold();
                            mCol.Item().Table(t =>
                            {
                                t.ColumnsDefinition(cd =>
                                {
                                    cd.RelativeColumn(2f);
                                    cd.RelativeColumn(1.2f);
                                    cd.RelativeColumn(1.2f);
                                    cd.RelativeColumn(2.6f);
                                });
                                t.Header(h =>
                                {
                                    h.Cell().Background(ColorOscuroInstitucional).Padding(5).Text("MACRONUTRIENTE").FontSize(8f).Bold().FontColor("#FFFFFF");
                                    h.Cell().Background(ColorOscuroInstitucional).Padding(5).AlignCenter().Text("GRAMOS / DÍA").FontSize(8f).Bold().FontColor("#FFFFFF");
                                    h.Cell().Background(ColorOscuroInstitucional).Padding(5).AlignCenter().Text("APORTE KCAL").FontSize(8f).Bold().FontColor("#FFFFFF");
                                    h.Cell().Background(ColorOscuroInstitucional).Padding(5).Text("FUNCIÓN FISIOLÓGICA").FontSize(8f).Bold().FontColor("#FFFFFF");
                                });
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).Text("Proteínas").Bold().FontColor(ColorAzulPrimario);
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).AlignCenter().Text($"{gProt} g/día").Bold();
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).AlignCenter().Text($"{gProt * 4} kcal");
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).Text("Síntesis y preservación de masa muscular (~1.8 g/kg)").FontSize(8.2f);
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra).Padding(5).Text("Carbohidratos Complejos").Bold().FontColor(ColorVerdeExito);
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra).Padding(5).AlignCenter().Text($"{gCarbs} g/día").Bold();
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra).Padding(5).AlignCenter().Text($"{gCarbs * 4} kcal");
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra).Padding(5).Text("Sustrato energético para el entrenamiento").FontSize(8.2f);
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).Text("Grasas Saludables").Bold().FontColor(ColorAmbarAlerta);
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).AlignCenter().Text($"{gGrasas} g/día").Bold();
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).AlignCenter().Text($"{gGrasas * 9} kcal");
                                t.Cell().BorderBottom(1).BorderColor(ColorGrisBorde).Padding(5).Text("Soporte hormonal y ácidos grasos esenciales (~0.9 g/kg)").FontSize(8.2f);
                            });
                        });
                        col.Item().Text("PRESCRIPCIÓN NUTRICIONAL Y PAUTAS DE CONDUCTA").FontSize(10f).Bold();
                        col.Item().Border(1).BorderColor(ColorGrisBorde).Column(pCol =>
                        {
                            pCol.Item().Background(ColorOscuroInstitucional).PaddingHorizontal(10).PaddingVertical(6)
                                .Text("INDICACIONES CLÍNICAS PERSONALIZADAS").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                            pCol.Item().Padding(10).Text(string.IsNullOrWhiteSpace(habitos) ? "Sin observaciones adicionales." : LimpiarTextoPdf(habitos))
                                .FontSize(9f).LineHeight(1.25f);
                        });
                        col.Item().PaddingTop(18).AlignRight().Width(220).Column(firma =>
                        {
                            firma.Item().BorderTop(1).BorderColor(ColorGrisTextoSecundario).PaddingTop(4).AlignCenter()
                                .Text($"Firma Profesional: {nutricionista}").FontSize(8.5f).Bold().FontColor(ColorGrisTextoSecundario);
                        });
                    });
                    page.Footer().Element(f => ComponerPieDePaginaCorporativo(
                        f,
                        !string.IsNullOrWhiteSpace(dvhPlan) ? dvhPlan : "INFORME-CLÍNICO-UAIFIT-VERIFICADO"));
                });
            });
            GuardarYAbrirPdf(doc, rutaArchivo, abrirAlFinalizar);
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
            decimal pctEfec = totalGeneral > 0 ? Math.Round(totalEfectivo * 100m / totalGeneral, 1) : 0m;
            decimal pctDig = totalGeneral > 0 ? Math.Round(totalDigital * 100m / totalGeneral, 1) : 0m;
            string dniTxt = string.IsNullOrWhiteSpace(filtroDNI) ? "Todos" : filtroDNI;
            string metTxt = string.IsNullOrWhiteSpace(filtroMetodo) ? "Todos" : filtroMetodo;
            string auditor = !string.IsNullOrWhiteSpace(usuarioAuditor) ? usuarioAuditor : "Auditor";
            var gruposMetodo = comprobantes
                .GroupBy(x => string.IsNullOrWhiteSpace(x.MetodoPago) ? "Otro" : x.MetodoPago)
                .Select(g => new { Metodo = g.Key, Monto = g.Sum(x => x.MontoTotal), Cant = g.Count() })
                .OrderByDescending(g => g.Monto)
                .ToList();
            var listaOrdenada = comprobantes
                .OrderByDescending(x => x.Fecha)
                .ThenByDescending(x => x.NroComprobante)
                .ToList();
            var doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(QPageSizes.A4);
                    page.Margin(28, QUnit.Point);
                    page.PageColor("#FFFFFF");
                    page.DefaultTextStyle(x => x.FontFamily(FuenteCorporativa).FontSize(9f).FontColor(ColorOscuroInstitucional));
                    page.Header().Element(h => ComponerEncabezadoCorporativo(
                        h,
                        "DEPARTAMENTO DE ADMINISTRACIÓN Y AUDITORÍA CONTABLE",
                        "REPORTE DE ARQUEO DE CAJA E INGRESOS",
                        $"OPERACIONES: {cantTotal}",
                        $"Período: {desde:dd/MM/yy} - {hasta:dd/MM/yy}"));
                    page.Content().PaddingVertical(12).Column(col =>
                    {
                        col.Spacing(12);
                        col.Item().Border(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra).Padding(8)
                            .Text($"Período Auditado: {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}   |   Filtro DNI: {dniTxt}   |   Medio: {metTxt}   |   Auditor: {auditor}")
                            .FontSize(8.5f).Bold().FontColor(ColorGrisTextoSecundario);
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "RECAUDACIÓN TOTAL", $"$ {totalGeneral:N2}", $"{cantTotal} comprobante(s)", ColorAzulPrimario, ColorAzulClaroFondo));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "CAJA FÍSICA (EFECTIVO)", $"$ {totalEfectivo:N2}", $"{pctEfec:0.#}% del total", ColorVerdeExito));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "MEDIOS DIGITALES", $"$ {totalDigital:N2}", $"{pctDig:0.#}% del total", ColorVioletaAcento));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(c, "TICKET PROMEDIO", $"$ {ticketPromedio:N2}", "Promedio por venta", ColorAmbarAlerta));
                        });
                        if (gruposMetodo.Count > 0)
                        {
                            col.Item().Border(1).BorderColor(ColorGrisBorde).Padding(10).Column(bCol =>
                            {
                                bCol.Spacing(4);
                                bCol.Item().Text("PARTICIPACIÓN POR MEDIO DE PAGO").FontSize(9f).Bold();
                                foreach (var gm in gruposMetodo)
                                {
                                    decimal pct = totalGeneral > 0 ? Math.Round(gm.Monto * 100m / totalGeneral, 1) : 0m;
                                    bCol.Item().Row(r =>
                                    {
                                        r.ConstantItem(150).Text($"{gm.Metodo} ({gm.Cant} op.)").FontSize(8.5f).Bold();
                                        r.RelativeItem().AlignRight().Text($"$ {gm.Monto:N2}  ({pct:0.#}%)").FontSize(8.5f).Bold().FontColor(ColorAzulPrimario);
                                    });
                                }
                            });
                        }
                        col.Item().Text("DETALLE DE COMPROBANTES INCLUIDOS EN EL ARQUEO").FontSize(9.5f).Bold();
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(52);
                                columns.ConstantColumn(95);
                                columns.ConstantColumn(75);
                                columns.RelativeColumn(1.8f);
                                columns.RelativeColumn(1.3f);
                                columns.ConstantColumn(88);
                            });
                            table.Header(header =>
                            {
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("NRO.").FontSize(8f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("FECHA Y HORA").FontSize(8f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("DNI SOCIO").FontSize(8f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("SOCIO TITULAR").FontSize(8f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("MEDIO DE PAGO").FontSize(8f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).AlignRight().Text("MONTO ($)").FontSize(8f).Bold().FontColor("#FFFFFF");
                            });
                            for (int i = 0; i < listaOrdenada.Count; i++)
                            {
                                var comp = listaOrdenada[i];
                                string bg = (i % 2 == 1) ? ColorGrisCebra : "#FFFFFF";
                                string nomSocio = comp.Socio != null ? $"{comp.Socio.Nombre} {comp.Socio.Apellido}".Trim() : "-";
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(5).Text($"#{comp.NroComprobante:D5}").Bold().FontColor(ColorAzulPrimario);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(5).Text($"{comp.Fecha:dd/MM/yyyy HH:mm}");
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(5).Text(comp.Socio != null ? comp.Socio.DNI : "-");
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(5).Text(nomSocio);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(5).Text(comp.MetodoPago ?? "-");
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(5).AlignRight().Text($"$ {comp.MontoTotal:N2}").Bold();
                            }
                        });
                    });
                    page.Footer().Element(f => ComponerPieDePaginaCorporativo(
                        f,
                        $"ARQUEO-CAJA-{DateTime.Now:yyyyMMddHHmmss}-TOTAL-{totalGeneral:0.00}"));
                });
            });
            GuardarYAbrirPdf(doc, rutaArchivo, abrirAlFinalizar);
        }

        public static void GenerarComprobanteVentaProductoPDF(
            string rutaArchivo,
            VentaProductoBE venta,
            bool abrirAlFinalizar = true)
        {
            if (venta == null) throw new Exception("No se especificó la venta a exportar.");
            int nroVenta = venta.NroVenta > 0 ? venta.NroVenta : 1;
            DateTime fecha = venta.Fecha != default ? venta.Fecha : DateTime.Now;
            string nomSocio = !string.IsNullOrWhiteSpace(venta.NombreSocio) ? venta.NombreSocio : "-";
            string dniSocio = !string.IsNullOrWhiteSpace(venta.DNISocio) ? venta.DNISocio : "-";
            string vendedor = !string.IsNullOrWhiteSpace(venta.NombreVendedor) ? venta.NombreVendedor : "Vendedor UAI-FIT";
            string metodo = !string.IsNullOrWhiteSpace(venta.MetodoPago) ? venta.MetodoPago : "Efectivo";
            string dvh = !string.IsNullOrWhiteSpace(venta.DVH) ? venta.DVH : "VENTA-COMERCIAL-VERIFICADA";
            int totalUnidades = venta.Detalles != null ? venta.Detalles.Sum(d => d.Cantidad) : 0;
            var doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(QPageSizes.A4);
                    page.Margin(28, QUnit.Point);
                    page.PageColor("#FFFFFF");
                    page.DefaultTextStyle(x => x.FontFamily(FuenteCorporativa).FontSize(9.5f).FontColor(ColorOscuroInstitucional));
                    page.Header().Element(h => ComponerEncabezadoCorporativo(
                        h,
                        "GESTIÓN COMERCIAL Y DE MERCADO INTERNO — PUNTO DE VENTA",
                        "COMPROBANTE OFICIAL DE VENTA DE PRODUCTOS",
                        $"OP. #{nroVenta:D6}",
                        $"Fecha: {fecha:dd/MM/yyyy HH:mm}"));
                    page.Content().PaddingVertical(14).Column(col =>
                    {
                        col.Spacing(14);
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).BorderColor(ColorGrisBorde).Column(left =>
                            {
                                left.Item().Background(ColorGrisCebra).PaddingHorizontal(10).PaddingVertical(6)
                                    .Text("DATOS DEL SOCIO COMPRADOR").FontSize(9f).Bold();
                                left.Item().Padding(10).Column(info =>
                                {
                                    info.Spacing(4);
                                    info.Item().Text(t => { t.Span("Socio: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(nomSocio).Bold(); });
                                    info.Item().Text(t => { t.Span("DNI / Legajo: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(dniSocio); });
                                    info.Item().Text(t => { t.Span("Unidades Adquiridas: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span($"{totalUnidades} unidad(es)"); });
                                });
                            });
                            row.ConstantItem(14);
                            row.RelativeItem().Border(1).BorderColor(ColorGrisBorde).Column(right =>
                            {
                                right.Item().Background(ColorGrisCebra).PaddingHorizontal(10).PaddingVertical(6)
                                    .Text("DATOS DE LA OPERACIÓN EN MOSTRADOR").FontSize(9f).Bold();
                                right.Item().Padding(10).Column(info =>
                                {
                                    info.Spacing(4);
                                    info.Item().Text(t => { t.Span("Nro. Transacción: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span($"#{nroVenta:D6}").Bold().FontColor(ColorAzulPrimario); });
                                    info.Item().Text(t => { t.Span("Medio de Pago: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(metodo).Bold(); });
                                    info.Item().Text(t => { t.Span("Vendedor Responsable: ").Bold().FontColor(ColorGrisTextoSecundario); t.Span(vendedor); });
                                });
                            });
                        });
                        col.Item().Text("DETALLE DE ARTÍCULOS Y SUPLEMENTOS ENTREGADOS").FontSize(10f).Bold();
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(78);
                                columns.RelativeColumn(2.4f);
                                columns.RelativeColumn(1.4f);
                                columns.ConstantColumn(45);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(85);
                            });
                            table.Header(header =>
                            {
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("SKU").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("PRODUCTO / ARTÍCULO").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).Text("CATEGORÍA").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).AlignCenter().Text("CANT.").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).AlignRight().Text("P. UNIT. ($)").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(6).AlignRight().Text("SUBTOTAL ($)").FontSize(8.5f).Bold().FontColor("#FFFFFF");
                            });
                            if (venta.Detalles != null)
                            {
                                for (int i = 0; i < venta.Detalles.Count; i++)
                                {
                                    var d = venta.Detalles[i];
                                    string bg = (i % 2 == 1) ? ColorGrisCebra : "#FFFFFF";
                                    table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(6).Text(d.CodigoSKU).Bold().FontColor(ColorAzulPrimario);
                                    table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(6).Text(d.NombreProducto).Bold();
                                    table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(6).Text(d.Categoria).FontSize(8.5f).FontColor(ColorGrisTextoSecundario);
                                    table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(6).AlignCenter().Text(d.Cantidad.ToString()).Bold();
                                    table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(6).AlignRight().Text($"$ {d.PrecioUnitario:N2}");
                                    table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(6).AlignRight().Text($"$ {d.Subtotal:N2}").Bold();
                                }
                            }
                        });
                        col.Item().AlignRight().Width(250).Border(1.5f).BorderColor(ColorAzulPrimario).Background(ColorAzulClaroFondo)
                            .Padding(12).Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("TOTAL ABONADO").FontSize(9f).Bold().FontColor(ColorAzulPrimario);
                                    c.Item().Text($"Medio: {metodo}").FontSize(8f).FontColor(ColorGrisTextoSecundario);
                                });
                                row.RelativeItem().AlignRight().AlignMiddle().Text($"$ {venta.MontoTotal:N2}").FontSize(14f).Bold().FontColor(ColorAzulPrimario);
                            });
                        col.Item().Border(1).BorderColor("#86EFAC").Background(ColorVerdeClaroFondo).Padding(10)
                            .Text("Constancia de entrega de mercadería: Las unidades detalladas en el presente comprobante fueron entregadas en mostrador y descontadas automáticamente del inventario físico local de UAI-FIT.")
                            .FontSize(8.5f).FontColor(ColorVerdeExito);
                    });
                    page.Footer().Element(f => ComponerPieDePaginaCorporativo(f, dvh));
                });
            });
            GuardarYAbrirPdf(doc, rutaArchivo, abrirAlFinalizar);
        }

        public static void GenerarReportePrediccionYReabastecimientoPDF(
            string rutaArchivo,
            List<PrediccionProductoBE> predicciones,
            string metodoPrediccion,
            int horizonteDias,
            bool incluyeSinergia,
            decimal facturacionPeriodo,
            decimal costoPeriodo,
            decimal gananciaPeriodo,
            string resumenSinergia,
            OrdenReabastecimientoBE ordenGenerada = null,
            string hashSerializado = null,
            bool abrirAlFinalizar = true)
        {
            if (predicciones == null) predicciones = new List<PrediccionProductoBE>();
            decimal margenProm = facturacionPeriodo > 0m
                ? Math.Round((gananciaPeriodo / facturacionPeriodo) * 100m, 1)
                : 0m;
            int cantProductosCriticos = predicciones.Count(p => p.CodigoSeveridadRiesgo >= 2);
            int totalUnidadesComprar = predicciones.Sum(p => p.CantidadAComprarAdmin);
            decimal inversionTotalReposicion = Math.Round(predicciones.Sum(p => p.CostoTotalReposicion), 2);
            string adminNombre = ordenGenerada != null && !string.IsNullOrWhiteSpace(ordenGenerada.NombreAdministrador)
                ? ordenGenerada.NombreAdministrador
                : "Administrador UAI-FIT";
            string badgeDerecha = ordenGenerada != null && ordenGenerada.NroOrden > 0
                ? $"ORDEN #{ordenGenerada.NroOrden:D5}"
                : $"HORIZONTE: {horizonteDias} DÍAS";
            string selloFinal = !string.IsNullOrWhiteSpace(hashSerializado)
                ? hashSerializado
                : (ordenGenerada != null && !string.IsNullOrWhiteSpace(ordenGenerada.DVH)
                    ? ordenGenerada.DVH
                    : $"PREDICCION-UAIFIT-{DateTime.Now:yyyyMMddHHmmss}");
            var doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(QPageSizes.A4.Landscape());
                    page.Margin(24, QUnit.Point);
                    page.PageColor("#FFFFFF");
                    page.DefaultTextStyle(x => x.FontFamily(FuenteCorporativa).FontSize(8.5f).FontColor(ColorOscuroInstitucional));
                    page.Header().Element(h => ComponerEncabezadoCorporativo(
                        h,
                        "GERENCIA COMERCIAL Y DE MERCADO INTERNO — TOMA DE DECISIONES E INVENTARIO",
                        "REPORTE DE PREDICCIÓN DE DEMANDA Y PLAN DE REABASTECIMIENTO",
                        badgeDerecha,
                        $"Emisión: {DateTime.Now:dd/MM/yyyy HH:mm}"));
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(10);
                        col.Item().Border(1).BorderColor(ColorGrisBorde).Background(ColorGrisCebra).Padding(7).Column(pCol =>
                        {
                            pCol.Spacing(2);
                            pCol.Item().Text(t =>
                            {
                                t.Span("Modelo Estadístico Aplicado: ").Bold().FontColor(ColorGrisTextoSecundario);
                                t.Span($"{metodoPrediccion}   |   ").Bold().FontColor(ColorAzulPrimario);
                                t.Span("Horizonte Proyectado: ").Bold().FontColor(ColorGrisTextoSecundario);
                                t.Span($"{horizonteDias} días corridos   |   ").Bold();
                                t.Span("Sinergia Entrenamiento/Nutrición: ").Bold().FontColor(ColorGrisTextoSecundario);
                                t.Span(incluyeSinergia ? "ACTIVA (Demanda inducida ponderada)   |   " : "DESACTIVADA (Serie pura)   |   ").Bold().FontColor(incluyeSinergia ? ColorVerdeExito : ColorGrisTextoSecundario);
                                t.Span("Decisor: ").Bold().FontColor(ColorGrisTextoSecundario);
                                t.Span(adminNombre).Bold();
                            });
                            if (!string.IsNullOrWhiteSpace(resumenSinergia))
                            {
                                pCol.Item().Text($"Cruce Inter-Áreas con Padrón de Socios: {resumenSinergia}")
                                    .FontSize(8f).FontColor(ColorGrisTextoSecundario);
                            }
                        });
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(
                                c,
                                "FACTURACIÓN HISTÓRICA (4 MESES)",
                                $"$ {facturacionPeriodo:N2}",
                                $"Costo Ventas: $ {costoPeriodo:N2}",
                                ColorAzulPrimario));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(
                                c,
                                "UTILIDAD BRUTA Y MARGEN",
                                $"$ {gananciaPeriodo:N2}",
                                $"Margen Bruto Promedio: {margenProm:0.0}%",
                                ColorVerdeExito));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(
                                c,
                                "ARTÍCULOS EN RIESGO DE RUPTURA",
                                $"{cantProductosCriticos} de {predicciones.Count} SKUs",
                                "Stock disponible <= Punto de Reorden (ROP)",
                                cantProductosCriticos > 0 ? ColorRojoCritico : ColorVerdeExito,
                                cantProductosCriticos > 0 ? ColorRojoClaroFondo : ColorVerdeClaroFondo));
                            row.ConstantItem(8);
                            row.RelativeItem().Element(c => ComponerTarjetaKpi(
                                c,
                                "INVERSIÓN DE REABASTECIMIENTO",
                                $"$ {inversionTotalReposicion:N2}",
                                $"Total decidido: {totalUnidadesComprar} unidad(es)",
                                ColorVioletaAcento,
                                ColorAzulClaroFondo));
                        });
                        col.Item().Text("MATRIZ DE PREDICCIÓN DE DEMANDA, PUNTO DE REORDEN (ROP) Y DECISIÓN DE REABASTECIMIENTO").FontSize(9.5f).Bold();
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(62);
                                columns.RelativeColumn(1.8f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.1f);
                                columns.ConstantColumn(52);
                                columns.ConstantColumn(68);
                                columns.ConstantColumn(55);
                                columns.ConstantColumn(42);
                                columns.RelativeColumn(1.2f);
                                columns.ConstantColumn(46);
                                columns.ConstantColumn(48);
                                columns.ConstantColumn(76);
                            });
                            table.Header(header =>
                            {
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).Text("SKU").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).Text("PRODUCTO / AFINIDAD").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).Text("HISTORIAL MENSUAL").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).Text("TENDENCIA").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignCenter().Text("DEMANDA PROY.").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignCenter().Text("ERROR (MAPE)").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignCenter().Text("STOCK (+TRÁNS.)").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignCenter().Text("ROP").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).Text("RIESGO DE STOCK").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignCenter().Text("SUGER. SIST.").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignCenter().Text("APROB. ADMIN").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                                header.Cell().Background(ColorOscuroInstitucional).Padding(4).AlignRight().Text("INVERSIÓN ($)").FontSize(7.5f).Bold().FontColor("#FFFFFF");
                            });
                            for (int i = 0; i < predicciones.Count; i++)
                            {
                                var p = predicciones[i];
                                string bg = (i % 2 == 1) ? ColorGrisCebra : "#FFFFFF";
                                string colorRiesgo = p.CodigoSeveridadRiesgo == 3
                                    ? ColorRojoCritico
                                    : (p.CodigoSeveridadRiesgo == 2 ? ColorAmbarAlerta : ColorVerdeExito);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignMiddle()
                                    .Text(p.CodigoSKU).FontSize(7.5f).Bold().FontColor(ColorAzulPrimario);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).Column(c =>
                                {
                                    c.Item().Text(p.Producto).FontSize(8f).Bold();
                                    c.Item().Text($"{p.Categoria} | {p.AfinidadDeportivaNutricional}").FontSize(6.8f).FontColor(ColorGrisTextoSecundario);
                                });
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignMiddle()
                                    .Text(p.HistorialMensual).FontSize(7.2f);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignMiddle()
                                    .Text(LimpiarTextoPdf(p.Tendencia)).FontSize(7.2f).Bold();
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignCenter().AlignMiddle()
                                    .Text($"{p.DemandaProyectada:0.0} u.").FontSize(8f).Bold().FontColor(ColorAzulPrimario);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignCenter().AlignMiddle()
                                    .Text(p.ErrorPronosticoMAPE).FontSize(7f).FontColor(ColorGrisTextoSecundario);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignCenter().AlignMiddle()
                                    .Text($"{p.StockActual} (+{p.StockEnTransito})").FontSize(7.8f).Bold();
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignCenter().AlignMiddle()
                                    .Text($"{p.PuntoReordenROP} u.").FontSize(7.5f);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignMiddle()
                                    .Text(LimpiarTextoPdf(p.NivelRiesgoStock)).FontSize(7.2f).Bold().FontColor(colorRiesgo);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignCenter().AlignMiddle()
                                    .Text($"{p.CantidadSugerida} u.").FontSize(8f).FontColor(ColorGrisTextoSecundario);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignCenter().AlignMiddle()
                                    .Text($"{p.CantidadAComprarAdmin} u.").FontSize(8.5f).Bold().FontColor(p.CantidadAComprarAdmin > 0 ? ColorVerdeExito : ColorGrisTextoSecundario);
                                table.Cell().Background(bg).BorderBottom(1).BorderColor("#E2E8F0").Padding(4).AlignRight().AlignMiddle()
                                    .Text($"$ {p.CostoTotalReposicion:N2}").FontSize(8f).Bold();
                            }
                        });
                    });
                    page.Footer().Element(f => ComponerPieDePaginaCorporativo(f, selloFinal));
                });
            });
            GuardarYAbrirPdf(doc, rutaArchivo, abrirAlFinalizar);
        }
    }
}