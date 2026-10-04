using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Seguridad;

namespace BLL
{
    public class TurnoBLL
    {
        private TurnoDAL _turnoDAL = new TurnoDAL();
        private IntegridadDAL _integridadDAL = new IntegridadDAL();
        private RegistroBLL _registroBLL = new RegistroBLL();

        public void AsentarReserva(TurnoBE turno)
        {
            if (turno.FechaHora <= DateTime.Now)
                throw new Exception("La fecha del turno debe ser futura.");
            turno.Estado = "Pendiente";
            turno.NroTurno = _turnoDAL.InsertarTurno(turno);
            turno.DVH = DVManager.CalcularDVH(turno);
            _turnoDAL.ActualizarDVH(turno.NroTurno, turno.DVH);
            List<string> listaDVH = _turnoDAL.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            _integridadDAL.ActualizarDVV("Turno", nuevoDVV);
            UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
            _registroBLL.RegistrarEvento($"Turno reservado (Nro: {turno.NroTurno}) para el socio ID {turno.ID_Socio}", usuarioActual, "INFO");
        }

        public void CambiarEstadoTurno(TurnoBE turno, string nuevoEstado)
        {
            turno.Estado = nuevoEstado;
            turno.DVH = DVManager.CalcularDVH(turno);
            _turnoDAL.ActualizarEstado(turno.NroTurno, nuevoEstado, turno.DVH);
            List<string> listaDVH = _turnoDAL.ObtenerTodosLosDVH();
            string nuevoDVV = DVManager.CalcularDVV(listaDVH);
            _integridadDAL.ActualizarDVV("Turno", nuevoDVV);
            UsuarioBE usuarioActual = SessionManager.Instancia.UsuarioActual;
            _registroBLL.RegistrarEvento($"Estado de Turno {turno.NroTurno} modificado a: {nuevoEstado}", usuarioActual, "INFO");
        }

        public List<TurnoBE> ObtenerAgenda(DateTime fecha)
        {
            return _turnoDAL.ConsultarTurnosPorFecha(fecha);
        }

        public void ValidarMargenDeTiempo(int idNutricionista, DateTime fechaHoraSolicitada)
        {
            TurnoDAL turnoDAL = new TurnoDAL();
            List<TurnoBE> turnosDelDia = turnoDAL.ObtenerTurnosPorNutricionistaYFecha(idNutricionista, fechaHoraSolicitada);
            foreach (TurnoBE turnoExistente in turnosDelDia)
            {
                double diferenciaMinutos = Math.Abs((turnoExistente.FechaHora - fechaHoraSolicitada).TotalMinutes);
                if (diferenciaMinutos < 30)
                {
                    throw new Exception("El profesional ya tiene un turno asignado a las " + turnoExistente.FechaHora.ToString("HH:mm") + " hs. Debe dejar un margen mínimo de 30 minutos entre consultas.");
                }
            }
        }

        public List<TurnoBE> ObtenerTurnosPendientesPorSocio(int idSocio)
        {
            return _turnoDAL.ObtenerTurnosPendientesPorSocio(idSocio);
        }
    }
}
