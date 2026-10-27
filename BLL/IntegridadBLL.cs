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
    public class IntegridadBLL
    {
        private IntegridadDAL integridadDAL = new IntegridadDAL();
        private readonly string[] tablasConDV = {
            "Usuario",
            "ComprobantePago",
            "PlanNutricional",
            "PlanSuscripcion",
            "RutinaEntrenamiento",
            "Socio",
            "Turno",
            "Producto",
            "VentaProducto",
            "OrdenReabastecimiento"
        };

        public void VerificarIntegridadSistema()
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            VerificarIntegridadSistema(unidad);
            unidad.Confirmar();
        }

        internal void VerificarIntegridadSistema(UnidadTrabajoSeguridadDAL unidad)
        {
            foreach (string tabla in tablasConDV) VerificarTabla(tabla, unidad);
        }

        public void ActualizarDVVGeneral()
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            foreach (string tabla in tablasConDV) VerificarTabla(tabla, unidad);
            IntegridadTransaccionalDAL.ActualizarDVV(unidad, tablasConDV);
            unidad.Confirmar();
        }

        public void ForzarRecalculoDeTodaLaBase()
        {
            using var unidad = new UnidadTrabajoSeguridadDAL();
            ForzarRecalculoDeTodaLaBase(unidad);
            unidad.Confirmar();
        }

        internal void ForzarRecalculoDeTodaLaBase(UnidadTrabajoSeguridadDAL unidad)
        {
            var dal = new IntegridadDAL(unidad);
            foreach (string tabla in tablasConDV)
            {
                var registros = ObtenerRegistrosDeTabla(tabla, unidad);
                var dvhs = new List<string>();
                foreach (var registro in registros)
                {
                    string dvh = DVManager.CalcularDVH(registro);
                    dal.ActualizarDVHRegistro(tabla, int.Parse(registro.ObtenerID()), dvh);
                    dvhs.Add(dvh);
                }
                dal.ActualizarDVV(tabla, DVManager.CalcularDVV(dvhs));
            }
        }

        private void VerificarTabla(string tabla, UnidadTrabajoSeguridadDAL unidad)
        {
            var dvhs = new List<string>();
            foreach (var registro in ObtenerRegistrosDeTabla(tabla, unidad))
            {
                string dvh = DVManager.CalcularDVH(registro);
                if (dvh != registro.DVH)
                    throw new InvalidOperationException($"Error de integridad: el registro {registro.ObtenerID()} de {tabla} requiere revisión administrativa.");
                dvhs.Add(dvh);
            }
            string almacenado = new IntegridadDAL(unidad).ObtenerDVV(tabla);
            if (string.IsNullOrWhiteSpace(almacenado) || DVManager.CalcularDVV(dvhs) != almacenado)
                throw new InvalidOperationException($"Error de integridad: el DVV de {tabla} falta o no coincide. No se aceptarán los datos automáticamente.");
        }

        internal void VerificarIntegridadUsuario(UnidadTrabajoSeguridadDAL unidad)
        {
            var usuarios = new UsuarioDAL(unidad).ListarTodos();
            var dvhs = new List<string>();
            foreach (var usuario in usuarios)
            {
                string calculado = DVManager.CalcularDVH(usuario);
                if (calculado != usuario.DVH)
                    throw new InvalidOperationException($"Error de Integridad: el usuario con ID {usuario.ID} requiere revisión administrativa.");
                dvhs.Add(calculado);
            }
            string almacenado = new IntegridadDAL(unidad).ObtenerDVV("Usuario");
            if (string.IsNullOrWhiteSpace(almacenado))
                throw new InvalidOperationException("Error de Integridad: falta el DVV de Usuario. No se realizarán cambios de seguridad.");
            if (DVManager.CalcularDVV(dvhs) != almacenado)
                throw new InvalidOperationException("Error de Integridad: el DVV de Usuario no coincide. Se requiere revisión administrativa.");
        }

        internal void ActualizarDVVUsuario(UnidadTrabajoSeguridadDAL unidad, bool permitirInicializar = false)
        {
            var dal = new IntegridadDAL(unidad);
            if (!permitirInicializar && string.IsNullOrWhiteSpace(dal.ObtenerDVV("Usuario")))
                throw new InvalidOperationException("Error de Integridad: falta el DVV de Usuario.");
            var usuarios = new UsuarioDAL(unidad).ListarTodos();
            foreach (var usuario in usuarios)
                if (usuario.DVH != DVManager.CalcularDVH(usuario))
                    throw new InvalidOperationException("La operación dejó un DVH de Usuario incoherente; se revertirán los cambios.");
            dal.ActualizarDVV("Usuario", DVManager.CalcularDVV(usuarios.Select(u => u.DVH).ToList()));
        }

        private List<IEntidadVerificable> ObtenerRegistrosDeTabla(string tabla, UnidadTrabajoSeguridadDAL unidad)
        {
            switch (tabla)
            {
                case "Usuario": return new UsuarioDAL(unidad).ListarTodos().Cast<IEntidadVerificable>().ToList();
                case "Socio": return new SocioDAL(unidad).LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "Turno": return new TurnoDAL(unidad).LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "ComprobantePago": return new ComprobantePagoDAL(unidad).LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "PlanNutricional": return new PlanNutricionalDAL(unidad).LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "PlanSuscripcion": return new PlanSuscripcionDAL(unidad).ListarPlanes().Cast<IEntidadVerificable>().ToList();
                case "RutinaEntrenamiento": return new RutinaEntrenamientoDAL(unidad).LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "Producto": return new ProductoDAL(unidad).LeerTodos().Cast<IEntidadVerificable>().ToList();
                case "VentaProducto": return new MercadoInternoDAL(unidad).LeerTodasLasVentas().Cast<IEntidadVerificable>().ToList();
                case "OrdenReabastecimiento": return new MercadoInternoDAL(unidad).LeerTodasLasOrdenes().Cast<IEntidadVerificable>().ToList();
                default: return new List<IEntidadVerificable>();
            }
        }
    }
}
