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
            comprobante.DVH = DVManager.CalcularDVH(comprobante);
            dal.InsertarComprobante(comprobante);
            List<string> listaDVH = dal.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            intDal.ActualizarDVV("ComprobantePago", nuevoDVV);
            regBll.RegistrarEvento($"Venta de plan registrada. Comprobante Nro: {comprobante.NroComprobante} - Socio DNI: {comprobante.Socio.DNI}", SessionManager.Instancia.UsuarioActual, "INFO");
        }
    }
}
