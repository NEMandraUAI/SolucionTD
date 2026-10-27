using BE;
using DAL;
using Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ComprobantePagoBLL
    {
        ComprobantePagoDAL dal = new ComprobantePagoDAL();
        SocioDAL socioDal = new SocioDAL();
        SocioBLL socioBll = new SocioBLL();
        IntegridadDAL intDal = new IntegridadDAL();
        RegistroBLL regBll = new RegistroBLL();

        private void ValidarPermisoVentaPlan()
        {
            UsuarioBE actual = SessionManager.Instancia.UsuarioActual;
            if (actual == null || (!actual.TienePermiso("VENDER_PLAN") && !actual.TienePermiso("GESTION_ROLES")))
            {
                throw new Exception("Operación denegada: No posee el permiso requerido (VENDER_PLAN) para registrar ventas de planes.");
            }
        }

        public void RegistrarVentaSuscripcionCompleta(SocioBE socio, PlanSuscripcionBE nuevoPlan, ComprobantePagoBE comprobante)
        {
            ValidarPermisoVentaPlan();
            if (socio == null || string.IsNullOrWhiteSpace(socio.DNI))
                throw new Exception("El DNI es un campo obligatorio.");
            string dniLimpio = socio.DNI.Trim();
            if (dniLimpio.Length < 7 || dniLimpio.Length > 8 || !dniLimpio.All(char.IsDigit))
                throw new Exception("El DNI debe contener únicamente entre 7 y 8 dígitos numéricos.");
            if (string.IsNullOrWhiteSpace(socio.Nombre) || string.IsNullOrWhiteSpace(socio.Apellido))
                throw new Exception("El nombre y el apellido del socio son obligatorios.");
            if (nuevoPlan == null || nuevoPlan.CodigoPlan <= 0)
                throw new Exception("Debe seleccionar un plan de suscripción.");
            if (comprobante == null || string.IsNullOrWhiteSpace(comprobante.MetodoPago))
                throw new Exception("Debe seleccionar un método de pago.");
            if (socio.Nombre.Length > 100 || socio.Apellido.Length > 100 || (socio.Telefono?.Length ?? 0) > 50 || (socio.Email?.Length ?? 0) > 100)
                throw new ArgumentException("Los datos del socio superan el largo permitido (nombre/apellido/email: 100; teléfono: 50).");
            if (!new[] { "Efectivo", "Tarjeta de Débito", "Tarjeta de Crédito", "Transferencia" }.Contains(comprobante.MetodoPago))
                throw new ArgumentException("El método de pago no está permitido.");
            if (comprobante.Fecha != default && comprobante.Fecha.Date != DateTime.Today)
                throw new ArgumentException("La venta debe registrarse con la fecha actual.");
            socio.DNI = dniLimpio;
            socio.Nombre = socio.Nombre.Trim();
            socio.Apellido = socio.Apellido.Trim();
            socio.PlanAsociado = nuevoPlan;
            socio.EstadoActivo = true;
            DateTime f = comprobante.Fecha == default ? DateTime.Now : comprobante.Fecha;
            comprobante.Fecha = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, f.Second);
            comprobante.MontoTotal = nuevoPlan.Precio;
            comprobante.Socio = socio;
            comprobante.EmpleadoCobrador = SessionManager.Instancia.UsuarioActual;
            if (comprobante.MontoTotal <= 0)
                throw new Exception("El monto total debe ser mayor a cero.");
            dal.RegistrarVentaSuscripcionTransaccional(
                socio,
                comprobante,
                entidad => DVManager.CalcularDVH(entidad),
                out bool fueAltaNuevoSocio);
        }

        public void RegistrarVenta(ComprobantePagoBE comprobante)
        {
            ArgumentNullException.ThrowIfNull(comprobante);
            if (comprobante.Socio == null || comprobante.Socio.ID_Socio <= 0)
                throw new ArgumentException("El comprobante debe estar asociado a un socio válido.");
            var socio = socioDal.ObtenerPorID(comprobante.Socio.ID_Socio)
                ?? throw new ArgumentException("El socio no existe.");
            var plan = new PlanSuscripcionDAL().ListarPlanes().SingleOrDefault(p => p.CodigoPlan == socio.PlanAsociado?.CodigoPlan)
                ?? throw new InvalidOperationException("Debe seleccionar un plan antes de registrar el pago.");
            RegistrarVentaSuscripcionCompleta(socio, plan, comprobante);
        }
        public List<ComprobantePagoBE> ConsultarComprobantesFiltros(DateTime desde, DateTime hasta, string dniSocio)
        {
            if (desde > hasta)
                throw new Exception("La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'.");
            return dal.ConsultarComprobantesFiltros(desde, hasta, dniSocio);
        }
    }
}