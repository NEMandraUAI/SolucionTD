# UAI-FIT (v5.0.0) — Sistema Integral de Gestión Deportiva, Nutricional y Comercial

Plataforma transaccional de escritorio desarrollada en **C# (.NET 9.0 Windows Forms)** y **Microsoft SQL Server / LocalDB**, diseñada para la administración integral de un establecimiento deportivo, consultorio nutricional y mercado interno.

📥 **[Descargar Instalador Oficial v5.0.0 (UAI-FIT_Setup_5.0.0_x64.exe)](https://github.com/NEMandraUAI/SolucionTD/releases/tag/v5.0.0)**

---

## 🏛️ Arquitectura del Sistema
El software se estructura bajo una estricta **arquitectura en capas (4 capas lógicas + 1 componente transversal)**:
- **`BE` (Business Entities)**: Clases de dominio, contratos de verificación (`IEntidadVerificable`) y metadatos sin acoplamiento externo.
- **`DAL` (Data Access Layer)**: Comunicación mediante ADO.NET puro (`Microsoft.Data.SqlClient`), consultas parametrizadas, control transaccional robusto y exclusividad por `sys.sp_getapplock`.
- **`BLL` (Business Logic Layer)**: Reglas de negocio, cálculos biométricos avanzados, modelos estadísticos de pronóstico de demanda (**WMA y Holt**) y algoritmos de retención (*Churn*).
- **`GUI` (Presentation Layer)**: Formularios Windows Forms con soporte multilenguaje en tiempo real, renderizado gráfico vectorial con `GDI+` y exportación de reportes corporativos en PDF mediante **QuestPDF**.
- **`Seguridad`**: Componente transversal que provee derivación de claves (`PBKDF2-SHA256`), hashing (`SHA-256`), gestión de sesión (*Singleton*), internacionalización (*Observer*) y cálculo reflexivo de integridad (`DVH`/`DVV`).

---

## ⚙️ Procesos de Negocio Principales
1. **Gestión Administrativa y de Suscripciones (`PN1`)**: Padrón de socios, venta de planes (Mensual, Trimestral, Anual), control de superposición de períodos, recibos en PDF, diseño de rutinas asistido con mapa de calor anatómico y curva de fuerza, y tablero proactivo de retención de socios (*Churn*).
2. **Consultorio Nutricional y Agenda Médica (`PN2`)**: Agenda diaria de profesionales, agendamiento predictivo basado en modas estadísticas históricas, calculadora antropométrica en tiempo real (IMC y TMB por Harris-Benedict), calorías objetivo, asignación transaccional de planes alimentarios e informe clínico en PDF.
3. **Mercado Interno y Predicción de Demanda (`PN3`)**: Punto de venta (POS) con filtrado inteligente de productos afines al perfil físico/nutricional del socio, motor estadístico de proyección de demanda sobre 4 meses (**WMA y Holt**), cálculo de MAE/MAPE, demanda inducida por sinergia de planes, Punto de Reorden (**ROP**), stock en tránsito, órdenes de compra y serialización JSON firmada con SHA-256 y autorreparación.

---

## 🔒 Mecanismos de Seguridad e Integridad
- **Autenticación Fuerte**: Almacenamiento irreversible de credenciales mediante `PBKDF2-SHA256` (16 bytes de sal aleatoria y 600.000 iteraciones) con comparación en tiempo constante (`FixedTimeEquals`) y bloqueo automático ante 3 intentos fallidos.
- **Integridad de Datos (DVH / DVV)**: Verificación criptográfica horizontal por fila y vertical por tabla basada en hashes SHA-256 sobre **10 tablas críticas** del negocio para la detección inmediata de alteraciones o manipulaciones externas.
- **Patrones de Diseño GoF**: Implementación estricta de *Singleton* (Conexión, Sesión, Idioma), *Composite* (RBAC jerárquico de Familias y Patentes), *Observer* (Traducción dinámica) y *Memento* (Control de cambios e historial de usuarios).
- **Recuperación ante Desastres**: Módulo de respaldo transaccional (`BACKUP DATABASE` con `CHECKSUM` y aislamiento NTFS estricto en directorios de staging) y restauración de emergencia por elevación administrativa.

---

## 📦 Instalador Automatizado
El sistema cuenta con un instalador autocontenido de archivo único (`win-x64`) generado mediante **Inno Setup** y scripts de aprovisionamiento en **PowerShell**:
- Publicación autocontenida que elimina la necesidad de preinstalar el Runtime de .NET en las terminales cliente.
- Detección inteligente del entorno: evalúa la presencia del motor SQL Server / LocalDB y, de no existir, automatiza la instalación silenciosa de `SqlLocalDB.msi`.
- Inicialización idempotente de la base de datos a través de `InitDB.sql`, con migración transparente de esquemas (`SchemaMigradorBLL`), re-adjuntado de archivos huérfanos ante el error SQL 5170 y recálculo automático de dígitos verificadores en el primer arranque.

---

## 🛠️ Stack Tecnológico
- **Lenguaje**: C# 12 (.NET 9.0)
- **Interfaz**: Windows Forms (GDI+)
- **Base de Datos**: Microsoft SQL Server / SQL Server LocalDB (ADO.NET)
- **Librerías Externas**: QuestPDF (Generación de reportes), System.Text.Json (Serialización)
- **Herramientas de Build / Despliegue**: Inno Setup 7, PowerShell CLI, Git / GitHub Actions (CI/CD)

---

## 🚀 Guía de Instalación Rápida para Desarrolladores

Para poner en marcha el entorno de desarrollo y probar el sistema localmente, seguí estos pasos:

### 1. Requisitos previos
- Tener instalado Visual Studio 2022 (o superior) con la carga de trabajo de desarrollo de escritorio de .NET.
- SDK de .NET 9.0 instalado en el equipo.
- SQL Server (LocalDB o instancia estándar) habilitado.

### 2. Clonar el repositorio
Abrí tu terminal y cloná el repositorio ejecutando:
git clone [https://github.com/nemandrauai/soluciontd.git](https://github.com/nemandrauai/soluciontd.git)

### 3. Abrir la solución
Navegá a la carpeta del proyecto clonado y abrí el archivo de solución principal con Visual Studio:
SolucionTD.sln

### 4. Configurar la Base de Datos y el Conector
El sistema utiliza una cadena de conexión centralizada gestionada por ConexionDAL. 
- Por defecto, si no existe una configuración previa, el sistema buscará conectarse a la instancia predeterminada de SQL Server LocalDB ((localdb)\UAIFIT) y a la base de datos ProyectoCampo.
- El archivo de configuración de conexión se genera y almacena automáticamente en el perfil de usuario bajo la ruta:
%LOCALAPPDATA%\UAI-FIT\dbconfig.cfg

### 5. Compilar y Ejecutar
1. En el Explorador de soluciones de Visual Studio, asegurate de que el proyecto de inicio (Startup Project) sea GUI.
2. Establecé la configuración de compilación en Release o Debug.
3. Presioná F5 para compilar la solución completa y correr la aplicación. Al iniciar, el sistema validará automáticamente la disponibilidad del esquema, los dígitos verificadores (DVH/DVV) y desplegará la pantalla de inicio de sesión (frmLogIn).
