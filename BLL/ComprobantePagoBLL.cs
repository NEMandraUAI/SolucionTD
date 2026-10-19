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
        IntegridadDAL intDal = new IntegridadDAL();
        RegistroBLL regBll = new RegistroBLL();

        public void RegistrarVenta(ComprobantePagoBE comprobante)
        {
            if (comprobante.MontoTotal <= 0)
                throw new Exception("El monto total debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(comprobante.MetodoPago))
                throw new Exception("Debe seleccionar un método de pago.");
            if (comprobante.Socio == null || comprobante.Socio.ID_Socio == 0)
                throw new Exception("El comprobante debe estar asociado a un socio válido.");
            DateTime f = comprobante.Fecha == default ? DateTime.Now : comprobante.Fecha;
            comprobante.Fecha = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, f.Second);
            comprobante.DVH = string.Empty;
            dal.InsertarComprobante(comprobante);
            comprobante.DVH = DVManager.CalcularDVH(comprobante);
            intDal.ActualizarDVHRegistro("ComprobantePago", comprobante.NroComprobante, comprobante.DVH);
            List<string> listaDVH = dal.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            intDal.ActualizarDVV("ComprobantePago", nuevoDVV);
            regBll.RegistrarEvento($"Venta de plan registrada. Comprobante Nro: {comprobante.NroComprobante} - Socio DNI: {comprobante.Socio.DNI}", SessionManager.Instancia.UsuarioActual, "INFO");
        }

        public List<ComprobantePagoBE> ConsultarComprobantesFiltros(DateTime desde, DateTime hasta, string dniSocio)
        {
            if (desde > hasta)
                throw new Exception("La fecha 'Desde' no puede ser mayor a la fecha 'Hasta'.");
            return dal.ConsultarComprobantesFiltros(desde, hasta, dniSocio);
        }
    }
}
