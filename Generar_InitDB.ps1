param(
    [string]$RutaScriptCrudoSSMS = ".\Script_EsquemaDatos_BD.txt",
    [string]$RutaSalidaInitDB    = ".\GUI\InitDB.sql"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $RutaScriptCrudoSSMS)) {
    throw "No se encontró el archivo de entrada '$RutaScriptCrudoSSMS'."
}

Write-Host "Leyendo script exportado de SSMS: $RutaScriptCrudoSSMS ..." -ForegroundColor Cyan

$rutaEntradaAbsoluta = (Resolve-Path $RutaScriptCrudoSSMS).Path
$bytes = [System.IO.File]::ReadAllBytes($rutaEntradaAbsoluta)
if ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFF -and $bytes[1] -eq 0xFE) {
    $contenido = [System.Text.Encoding]::Unicode.GetString($bytes, 2, $bytes.Length - 2)
} elseif ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) {
    $contenido = [System.Text.Encoding]::UTF8.GetString($bytes, 3, $bytes.Length - 3)
} else {
    $contenido = [System.Text.Encoding]::UTF8.GetString($bytes)
    if ($contenido.Contains([char]0xFFFD)) {
        $contenido = [System.Text.Encoding]::Default.GetString($bytes)
    }
}

$matchInicio = [regex]::Match(
    $contenido,
    "/\*{6}\s*(?:Object|Objeto):\s*Table\s+\[dbo\]\.\[Bitacora\][^\r\n]*\*/",
    [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
)
if (-not $matchInicio.Success) {
    $matchInicio = [regex]::Match(
        $contenido,
        "(?:IF\s+NOT\s+EXISTS[^\r\n]*\[dbo\]\.\[Bitacora\]|CREATE\s+TABLE\s+\[dbo\]\.\[Bitacora\])",
        [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
    )
}
if (-not $matchInicio.Success) {
    throw "No se encontró la definición de la tabla [dbo].[Bitacora] en '$RutaScriptCrudoSSMS'."
}
$idxTabla = $matchInicio.Index

$matchFinInserts = [regex]::Match(
    $contenido,
    "/\*{6}\s*(?:Object|Objeto):\s*Index\s+\[IX_Bitacora_FechaHora\][^\r\n]*\*/",
    [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
)
if (-not $matchFinInserts.Success) {
    $matchFinInserts = [regex]::Match(
        $contenido,
        "CREATE\s+NONCLUSTERED\s+INDEX\s+\[IX_Bitacora_FechaHora\]",
        [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
    )
}
if (-not $matchFinInserts.Success) {
    throw "No se encontró el marcador del índice [IX_Bitacora_FechaHora] en '$RutaScriptCrudoSSMS'."
}
$idxFinInserts = $matchFinInserts.Index

$cuerpoTablasEInserts = $contenido.Substring($idxTabla, $idxFinInserts - $idxTabla)

$cuerpoTablasEInserts = [regex]::Replace(
    $cuerpoTablasEInserts,
    ",?\s*OPTIMIZE_FOR_SEQUENTIAL_KEY\s*=\s*(OFF|ON)",
    "",
    [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
)
$cuerpoTablasEInserts = [regex]::Replace(
    $cuerpoTablasEInserts,
    "IGNORE_DUP_KEY\s*=\s*ON",
    "IGNORE_DUP_KEY = OFF",
    [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
)

$lotes = [regex]::Split(
    $cuerpoTablasEInserts,
    "^\s*GO\s*$",
    [System.Text.RegularExpressions.RegexOptions]::Multiline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
)

$sbCuerpo = New-Object System.Text.StringBuilder

$regexCreateTable = [regex]::new("CREATE\s+TABLE\s+\[dbo\]\.\[([A-Za-z0-9_]+)\]", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
$regexComposite   = [regex]::new("^\s*INSERT\s+\[dbo\]\.\[(Permiso_Permiso|Usuario_Permiso)\]\s*\(\[(.*?)\],\s*\[(.*?)\]\)\s*VALUES\s*\(\s*([0-9]+)\s*,\s*([0-9]+)\s*\)", [System.Text.RegularExpressions.RegexOptions]::Singleline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
$regexDV          = [regex]::new("^\s*INSERT\s+\[dbo\]\.\[DigitoVerificador\]\s*\(\[(Entidad|NombreTabla)\],\s*\[DVV\]\)\s*VALUES\s*\(\s*N'([^']+)'", [System.Text.RegularExpressions.RegexOptions]::Singleline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
$regexNumericPK   = [regex]::new("^\s*INSERT\s+\[dbo\]\.\[([A-Za-z0-9_]+)\]\s*\(\[([A-Za-z0-9_]+)\]\s*,.*?\)\s*VALUES\s*\(\s*([0-9]+)\s*,", [System.Text.RegularExpressions.RegexOptions]::Singleline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

$insertsSinProteger = 0

foreach ($lote in $lotes) {
    $trimLote = $lote.Trim()
    if ([string]::IsNullOrWhiteSpace($trimLote)) { continue }

    $mTable = $regexCreateTable.Match($trimLote)
    if ($mTable.Success -and ($trimLote -notmatch "IF\s+NOT\s+EXISTS")) {
        $nomTabla = $mTable.Groups[1].Value
        [void]$sbCuerpo.AppendLine("IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[$nomTabla]') AND type in (N'U'))")
        [void]$sbCuerpo.AppendLine("BEGIN")
        [void]$sbCuerpo.AppendLine($trimLote)
        [void]$sbCuerpo.AppendLine("END")
        [void]$sbCuerpo.AppendLine("GO")
        continue
    }

    if ($trimLote.StartsWith("INSERT [dbo].", [System.StringComparison]::OrdinalIgnoreCase)) {
        $mComp = $regexComposite.Match($trimLote)
        if ($mComp.Success) {
            $tabla = $mComp.Groups[1].Value
            $col1  = $mComp.Groups[2].Value
            $col2  = $mComp.Groups[3].Value
            $val1  = $mComp.Groups[4].Value
            $val2  = $mComp.Groups[5].Value
            [void]$sbCuerpo.AppendLine("IF NOT EXISTS (SELECT 1 FROM [dbo].[$tabla] WHERE [$col1] = $val1 AND [$col2] = $val2)")
            [void]$sbCuerpo.AppendLine($trimLote)
            [void]$sbCuerpo.AppendLine("GO")
            continue
        }

        $mDV = $regexDV.Match($trimLote)
        if ($mDV.Success) {
            $colDV      = $mDV.Groups[1].Value
            $nomTablaDV = $mDV.Groups[2].Value
            [void]$sbCuerpo.AppendLine("IF NOT EXISTS (SELECT 1 FROM [dbo].[DigitoVerificador] WHERE [$colDV] = N'$nomTablaDV')")
            [void]$sbCuerpo.AppendLine($trimLote)
            [void]$sbCuerpo.AppendLine("GO")
            continue
        }

        $mNum = $regexNumericPK.Match($trimLote)
        if ($mNum.Success) {
            $tabla = $mNum.Groups[1].Value
            $colPK = $mNum.Groups[2].Value
            $valPK = $mNum.Groups[3].Value
            [void]$sbCuerpo.AppendLine("IF NOT EXISTS (SELECT 1 FROM [dbo].[$tabla] WHERE [$colPK] = $valPK)")
            [void]$sbCuerpo.AppendLine($trimLote)
            [void]$sbCuerpo.AppendLine("GO")
            continue
        }

        $insertsSinProteger++
        Write-Warning "INSERT no reconocido por regex: $($trimLote.Substring(0, [Math]::Min(80, $trimLote.Length)))"
    }

    [void]$sbCuerpo.AppendLine($trimLote)
    [void]$sbCuerpo.AppendLine("GO")
}

$encabezado = @'
USE [master]
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ProyectoCampo')
BEGIN
    CREATE DATABASE [ProyectoCampo]
END
GO
USE [ProyectoCampo]
GO
'@

$cierreMaestro = @'
/****** Índices y Restricciones UNIQUE (100% Idempotentes y Nombrados) ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Bitacora]') AND name = N'IX_Bitacora_FechaHora')
CREATE NONCLUSTERED INDEX [IX_Bitacora_FechaHora] ON [dbo].[Bitacora] ([FechaHora] DESC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ComprobantePago]') AND name = N'IX_ComprobantePago_Socio_Fecha')
CREATE NONCLUSTERED INDEX [IX_ComprobantePago_Socio_Fecha] ON [dbo].[ComprobantePago] ([ID_Socio] ASC, [Fecha] DESC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVentaProducto]') AND name = N'IX_DetalleVenta_Producto')
CREATE NONCLUSTERED INDEX [IX_DetalleVenta_Producto] ON [dbo].[DetalleVentaProducto] ([CodigoProducto] ASC, [NroVenta] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Etiqueta]') AND name = N'UQ_Etiqueta_Formulario_Control')
ALTER TABLE [dbo].[Etiqueta] ADD CONSTRAINT [UQ_Etiqueta_Formulario_Control] UNIQUE NONCLUSTERED ([Formulario] ASC, [NombreControl] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PlanNutricional]') AND name = N'IX_PlanNutricional_Socio_Fecha')
CREATE NONCLUSTERED INDEX [IX_PlanNutricional_Socio_Fecha] ON [dbo].[PlanNutricional] ([ID_Socio] ASC, [FechaCreacion] DESC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND name = N'UQ_Producto_SKU')
ALTER TABLE [dbo].[Producto] ADD CONSTRAINT [UQ_Producto_SKU] UNIQUE NONCLUSTERED ([CodigoSKU] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RutinaEntrenamiento]') AND name = N'IX_RutinaEntrenamiento_Socio_Fecha')
CREATE NONCLUSTERED INDEX [IX_RutinaEntrenamiento_Socio_Fecha] ON [dbo].[RutinaEntrenamiento] ([ID_Socio] ASC, [FechaCreacion] DESC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Socio]') AND is_unique = 1 AND name LIKE N'UQ%Socio%')
ALTER TABLE [dbo].[Socio] ADD CONSTRAINT [UQ_Socio_DNI] UNIQUE NONCLUSTERED ([DNI] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Traduccion]') AND name = N'UQ_Traduccion_Idioma_Etiqueta')
ALTER TABLE [dbo].[Traduccion] ADD CONSTRAINT [UQ_Traduccion_Idioma_Etiqueta] UNIQUE NONCLUSTERED ([ID_Idioma] ASC, [ID_Etiqueta] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Turno]') AND name = N'IX_Turno_Nutricionista_FechaHora')
CREATE NONCLUSTERED INDEX [IX_Turno_Nutricionista_FechaHora] ON [dbo].[Turno] ([ID_Nutricionista] ASC, [FechaHora] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Turno]') AND name = N'IX_Turno_Socio_Estado')
CREATE NONCLUSTERED INDEX [IX_Turno_Socio_Estado] ON [dbo].[Turno] ([ID_Socio] ASC, [Estado] ASC, [FechaHora] DESC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND name = N'UQ_Usuario_Nombre')
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [UQ_Usuario_Nombre] UNIQUE NONCLUSTERED ([Nombre] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND name = N'IX_Usuario_Nombre')
CREATE NONCLUSTERED INDEX [IX_Usuario_Nombre] ON [dbo].[Usuario] ([Nombre] ASC)
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[VentaProducto]') AND name = N'IX_VentaProducto_Fecha')
CREATE NONCLUSTERED INDEX [IX_VentaProducto_Fecha] ON [dbo].[VentaProducto] ([Fecha] DESC)
GO

/****** Restricciones DEFAULT (9 restricciones verificadas por columna) ******/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Bitacora]') AND name = N'Criticidad' AND default_object_id <> 0)
ALTER TABLE [dbo].[Bitacora] ADD CONSTRAINT [DF_Bitacora_Criticidad] DEFAULT ('INFO') FOR [Criticidad]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrdenReabastecimiento]') AND name = N'HorizonteDias' AND default_object_id <> 0)
ALTER TABLE [dbo].[OrdenReabastecimiento] ADD CONSTRAINT [DF_OrdenReab_Horizonte] DEFAULT ((30)) FOR [HorizonteDias]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND name = N'TiempoReposicionDias' AND default_object_id <> 0)
ALTER TABLE [dbo].[Producto] ADD CONSTRAINT [DF_Producto_LeadTime] DEFAULT ((5)) FOR [TiempoReposicionDias]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND name = N'Activo' AND default_object_id <> 0)
ALTER TABLE [dbo].[Producto] ADD CONSTRAINT [DF_Producto_Activo] DEFAULT ((1)) FOR [Activo]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Socio]') AND name = N'EstadoActivo' AND default_object_id <> 0)
ALTER TABLE [dbo].[Socio] ADD CONSTRAINT [DF_Socio_EstadoActivo] DEFAULT ((1)) FOR [EstadoActivo]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND name = N'IntentosFallidos' AND default_object_id <> 0)
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_IntentosFallidos] DEFAULT ((0)) FOR [IntentosFallidos]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND name = N'Bloqueado' AND default_object_id <> 0)
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_Bloqueado] DEFAULT ((0)) FOR [Bloqueado]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND name = N'NivelJerarquia' AND default_object_id <> 0)
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_NivelJerarquia] DEFAULT ((1)) FOR [NivelJerarquia]
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Usuario_Historico]') AND name = N'NivelJerarquia' AND default_object_id <> 0)
ALTER TABLE [dbo].[Usuario_Historico] ADD CONSTRAINT [DF_Usuario_Historico_NivelJerarquia] DEFAULT ((1)) FOR [NivelJerarquia]
GO

/****** Restricciones FOREIGN KEY (24 claves foráneas verificadas por tabla/columna) ******/
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Bitacora]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Bitacora]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[Bitacora] WITH CHECK ADD CONSTRAINT [FK_Bitacora_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[ComprobantePago]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[ComprobantePago]'), N'ID_Socio', 'ColumnId'))
ALTER TABLE [dbo].[ComprobantePago] WITH CHECK ADD CONSTRAINT [FK_ComprobantePago_Socio] FOREIGN KEY([ID_Socio]) REFERENCES [dbo].[Socio] ([ID_Socio])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[ComprobantePago]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[ComprobantePago]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[ComprobantePago] WITH CHECK ADD CONSTRAINT [FK_ComprobantePago_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[DetalleOrdenReabastecimiento]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[DetalleOrdenReabastecimiento]'), N'NroOrden', 'ColumnId'))
ALTER TABLE [dbo].[DetalleOrdenReabastecimiento] WITH CHECK ADD CONSTRAINT [FK_DetalleOrden_Orden] FOREIGN KEY([NroOrden]) REFERENCES [dbo].[OrdenReabastecimiento] ([NroOrden]) ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[DetalleOrdenReabastecimiento]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[DetalleOrdenReabastecimiento]'), N'CodigoProducto', 'ColumnId'))
ALTER TABLE [dbo].[DetalleOrdenReabastecimiento] WITH CHECK ADD CONSTRAINT [FK_DetalleOrden_Producto] FOREIGN KEY([CodigoProducto]) REFERENCES [dbo].[Producto] ([CodigoProducto])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[DetalleVentaProducto]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[DetalleVentaProducto]'), N'CodigoProducto', 'ColumnId'))
ALTER TABLE [dbo].[DetalleVentaProducto] WITH CHECK ADD CONSTRAINT [FK_DetalleVenta_Producto] FOREIGN KEY([CodigoProducto]) REFERENCES [dbo].[Producto] ([CodigoProducto])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[DetalleVentaProducto]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[DetalleVentaProducto]'), N'NroVenta', 'ColumnId'))
ALTER TABLE [dbo].[DetalleVentaProducto] WITH CHECK ADD CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY([NroVenta]) REFERENCES [dbo].[VentaProducto] ([NroVenta]) ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[OrdenReabastecimiento]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[OrdenReabastecimiento]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[OrdenReabastecimiento] WITH CHECK ADD CONSTRAINT [FK_OrdenReab_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Permiso_Permiso]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Permiso_Permiso]'), N'ID_Hijo', 'ColumnId'))
ALTER TABLE [dbo].[Permiso_Permiso] WITH CHECK ADD CONSTRAINT [FK_PermisoPermiso_Hijo] FOREIGN KEY([ID_Hijo]) REFERENCES [dbo].[Permiso] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Permiso_Permiso]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Permiso_Permiso]'), N'ID_Padre', 'ColumnId'))
ALTER TABLE [dbo].[Permiso_Permiso] WITH CHECK ADD CONSTRAINT [FK_PermisoPermiso_Padre] FOREIGN KEY([ID_Padre]) REFERENCES [dbo].[Permiso] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[PlanNutricional]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[PlanNutricional]'), N'ID_Nutricionista', 'ColumnId'))
ALTER TABLE [dbo].[PlanNutricional] WITH CHECK ADD CONSTRAINT [FK_PlanNutricional_Nutricionista] FOREIGN KEY([ID_Nutricionista]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[PlanNutricional]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[PlanNutricional]'), N'ID_Socio', 'ColumnId'))
ALTER TABLE [dbo].[PlanNutricional] WITH CHECK ADD CONSTRAINT [FK_PlanNutricional_Socio] FOREIGN KEY([ID_Socio]) REFERENCES [dbo].[Socio] ([ID_Socio])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[RutinaEntrenamiento]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[RutinaEntrenamiento]'), N'ID_Socio', 'ColumnId'))
ALTER TABLE [dbo].[RutinaEntrenamiento] WITH CHECK ADD CONSTRAINT [FK_RutinaEntrenamiento_Socio] FOREIGN KEY([ID_Socio]) REFERENCES [dbo].[Socio] ([ID_Socio])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[RutinaEntrenamiento]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[RutinaEntrenamiento]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[RutinaEntrenamiento] WITH CHECK ADD CONSTRAINT [FK_RutinaEntrenamiento_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Socio]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Socio]'), N'CodigoPlan', 'ColumnId'))
ALTER TABLE [dbo].[Socio] WITH CHECK ADD CONSTRAINT [FK_Socio_PlanSuscripcion] FOREIGN KEY([CodigoPlan]) REFERENCES [dbo].[PlanSuscripcion] ([CodigoPlan])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Traduccion]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Traduccion]'), N'ID_Etiqueta', 'ColumnId'))
ALTER TABLE [dbo].[Traduccion] WITH CHECK ADD CONSTRAINT [FK_Traduccion_Etiqueta] FOREIGN KEY([ID_Etiqueta]) REFERENCES [dbo].[Etiqueta] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Traduccion]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Traduccion]'), N'ID_Idioma', 'ColumnId'))
ALTER TABLE [dbo].[Traduccion] WITH CHECK ADD CONSTRAINT [FK_Traduccion_Idioma] FOREIGN KEY([ID_Idioma]) REFERENCES [dbo].[Idioma] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Turno]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Turno]'), N'ID_Nutricionista', 'ColumnId'))
ALTER TABLE [dbo].[Turno] WITH CHECK ADD CONSTRAINT [FK_Turno_Nutricionista] FOREIGN KEY([ID_Nutricionista]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Turno]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Turno]'), N'ID_Socio', 'ColumnId'))
ALTER TABLE [dbo].[Turno] WITH CHECK ADD CONSTRAINT [FK_Turno_Socio] FOREIGN KEY([ID_Socio]) REFERENCES [dbo].[Socio] ([ID_Socio])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Usuario]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Usuario]'), N'ID_Idioma', 'ColumnId'))
ALTER TABLE [dbo].[Usuario] WITH CHECK ADD CONSTRAINT [FK_Usuario_Idioma] FOREIGN KEY([ID_Idioma]) REFERENCES [dbo].[Idioma] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Usuario_Historico]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Usuario_Historico]'), N'ID_Usuario_Autor', 'ColumnId'))
ALTER TABLE [dbo].[Usuario_Historico] WITH CHECK ADD CONSTRAINT [FK_UsuHist_Autor] FOREIGN KEY([ID_Usuario_Autor]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Usuario_Historico]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Usuario_Historico]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[Usuario_Historico] WITH CHECK ADD CONSTRAINT [FK_UsuHist_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Usuario_Permiso]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Usuario_Permiso]'), N'ID_Permiso', 'ColumnId'))
ALTER TABLE [dbo].[Usuario_Permiso] WITH CHECK ADD CONSTRAINT [FK_UsuarioPermiso_Permiso] FOREIGN KEY([ID_Permiso]) REFERENCES [dbo].[Permiso] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[Usuario_Permiso]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[Usuario_Permiso]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[Usuario_Permiso] WITH CHECK ADD CONSTRAINT [FK_UsuarioPermiso_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[VentaProducto]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[VentaProducto]'), N'ID_Socio', 'ColumnId'))
ALTER TABLE [dbo].[VentaProducto] WITH CHECK ADD CONSTRAINT [FK_VentaProducto_Socio] FOREIGN KEY([ID_Socio]) REFERENCES [dbo].[Socio] ([ID_Socio])
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_key_columns WHERE parent_object_id = OBJECT_ID(N'[dbo].[VentaProducto]') AND parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'[dbo].[VentaProducto]'), N'ID_Usuario', 'ColumnId'))
ALTER TABLE [dbo].[VentaProducto] WITH CHECK ADD CONSTRAINT [FK_VentaProducto_Usuario] FOREIGN KEY([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID])
GO

/****** Restricciones CHECK (5 restricciones de negocio y esquema) ******/
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DetalleOrden_Cant]'))
ALTER TABLE [dbo].[DetalleOrdenReabastecimiento] WITH CHECK ADD CONSTRAINT [CK_DetalleOrden_Cant] CHECK (([CantidadAprobadaAdmin]>=(0)))
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DetalleVenta_Cantidad]'))
ALTER TABLE [dbo].[DetalleVentaProducto] WITH CHECK ADD CONSTRAINT [CK_DetalleVenta_Cantidad] CHECK (([Cantidad]>(0)))
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Producto_Precios]'))
ALTER TABLE [dbo].[Producto] WITH CHECK ADD CONSTRAINT [CK_Producto_Precios] CHECK (([PrecioVenta]>(0) AND [CostoCompra]>=(0)))
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Producto_Stock]'))
ALTER TABLE [dbo].[Producto] WITH CHECK ADD CONSTRAINT [CK_Producto_Stock] CHECK (([StockActual]>=(0) AND [StockMinimo]>=(0) AND [TiempoReposicionDias]>=(1)))
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_VersionEsquema_ID]'))
ALTER TABLE [dbo].[VersionEsquema] WITH CHECK ADD CONSTRAINT [CK_VersionEsquema_ID] CHECK (([ID]=(1)))
GO

/****** Procedimientos Almacenados ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Usuario_ActualizarEstado]') AND type in (N'P', N'PC'))
BEGIN
    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_Usuario_ActualizarEstado] AS'
END
GO
ALTER PROCEDURE [dbo].[sp_Usuario_ActualizarEstado]
    @ID INT,
    @Intentos INT,
    @Bloqueado BIT,
    @DVH NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario 
    SET IntentosFallidos = @Intentos, 
        Bloqueado = @Bloqueado, 
        DVH = @DVH 
    WHERE ID = @ID;
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Usuario_ObtenerPorUsername]') AND type in (N'P', N'PC'))
BEGIN
    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_Usuario_ObtenerPorUsername] AS'
END
GO
ALTER PROCEDURE [dbo].[sp_Usuario_ObtenerPorUsername]
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Nombre, Clave, IntentosFallidos, Bloqueado, DVH, ID_Idioma, NivelJerarquia 
    FROM Usuario 
    WHERE Nombre = @Nombre;
END
GO
USE [master]
GO
'@

$scriptFinal = $encabezado + "`r`n" + $sbCuerpo.ToString() + "`r`n" + $cierreMaestro
$rutaDestinoAbsoluta = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($RutaSalidaInitDB)

$utf8Bom = New-Object System.Text.UTF8Encoding($true)
[System.IO.File]::WriteAllText($rutaDestinoAbsoluta, $scriptFinal, $utf8Bom)

Write-Host "InitDB.sql generado exitosamente en '$rutaDestinoAbsoluta' (INSERTs sin proteger: $insertsSinProteger)!" -ForegroundColor Green