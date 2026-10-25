using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BE
{
    public class VentaProductoBE : IEntidadVerificable
    {
        [DigitoVerificador(1)]
        public int NroVenta { get; set; }

        [DigitoVerificador(2)]
        public DateTime Fecha { get; set; }

        [DigitoVerificador(3)]
        public decimal MontoTotal { get; set; }

        [DigitoVerificador(4)]
        public decimal CostoTotalEstimado { get; set; }

        [DigitoVerificador(5)]
        public string MetodoPago { get; set; } = string.Empty;

        [Browsable(false)]
        public int ID_Socio { get; set; }

        [Browsable(false)]
        public int ID_Usuario { get; set; }

        [Browsable(false)]
        public SocioBE Socio { get; set; }

        [Browsable(false)]
        public UsuarioBE Vendedor { get; set; }

        [DigitoVerificador(6)]
        [Browsable(false)]
        public int ID_SocioParaDVH
        {
            get { return Socio != null && Socio.ID_Socio > 0 ? Socio.ID_Socio : ID_Socio; }
        }

        [DigitoVerificador(7)]
        [Browsable(false)]
        public int ID_UsuarioParaDVH
        {
            get { return Vendedor != null && Vendedor.ID > 0 ? Vendedor.ID : ID_Usuario; }
        }

        public string DNISocio
        {
            get { return Socio != null ? Socio.DNI : string.Empty; }
        }

        public string NombreSocio
        {
            get { return Socio != null ? $"{Socio.Nombre} {Socio.Apellido}".Trim() : string.Empty; }
        }

        public string NombreVendedor
        {
            get { return Vendedor != null ? Vendedor.Nombre : string.Empty; }
        }

        public decimal GananciaBruta
        {
            get { return Math.Max(0m, MontoTotal - CostoTotalEstimado); }
        }

        [Browsable(false)]
        public List<DetalleVentaProductoBE> Detalles { get; set; } = new List<DetalleVentaProductoBE>();

        [Browsable(false)]
        public string DVH { get; set; } = string.Empty;

        public string ObtenerID()
        {
            return NroVenta.ToString();
        }
    }
}