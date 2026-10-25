using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BE
{
    public class OrdenReabastecimientoBE : IEntidadVerificable
    {
        [DigitoVerificador(1)]
        public int NroOrden { get; set; }

        [DigitoVerificador(2)]
        public DateTime FechaGeneracion { get; set; }

        public DateTime? FechaRecepcion { get; set; }

        [DigitoVerificador(3)]
        public string MetodoPrediccion { get; set; } = string.Empty;

        [DigitoVerificador(4)]
        public int HorizonteDias { get; set; } = 30;

        [DigitoVerificador(5)]
        public string Estado { get; set; } = "Pendiente";

        [DigitoVerificador(6)]
        public decimal CostoTotalOrden { get; set; }

        public string Observaciones { get; set; } = string.Empty;

        [Browsable(false)]
        public int ID_Usuario { get; set; }

        [Browsable(false)]
        public UsuarioBE Administrador { get; set; }

        [DigitoVerificador(7)]
        [Browsable(false)]
        public int ID_UsuarioParaDVH
        {
            get { return Administrador != null && Administrador.ID > 0 ? Administrador.ID : ID_Usuario; }
        }

        public string NombreAdministrador
        {
            get { return Administrador != null ? Administrador.Nombre : string.Empty; }
        }

        [Browsable(false)]
        public List<DetalleOrdenReabastecimientoBE> Detalles { get; set; } = new List<DetalleOrdenReabastecimientoBE>();

        [Browsable(false)]
        public string DVH { get; set; } = string.Empty;

        public string ObtenerID()
        {
            return NroOrden.ToString();
        }
    }
}