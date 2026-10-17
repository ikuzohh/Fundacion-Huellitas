using System;
using FundacionHuellitas.Core.Enums;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Representa el proceso formal de postulación para adoptar un animal.
    /// Demuestra ASOCIACIÓN al vincular conceptualmente un Adoptante con un Animal y un Voluntario supervisor.
    /// </summary>
    public class SolicitudAdopcion
    {
        public string IdSolicitud { get; }
        public DateTime FechaPostulacion { get; }
        public Adoptante AdoptantePostulante { get; }
        public Animal AnimalSolicitado { get; }
        public Voluntario VoluntarioEvaluador { get; private set; }
        public EstadoSolicitud Estado { get; private set; }
        public string Observaciones { get; private set; }

        public SolicitudAdopcion(Adoptante adoptante, Animal animal)
        {
            AdoptantePostulante = adoptante ?? throw new ArgumentNullException(nameof(adoptante));
            AnimalSolicitado = animal ?? throw new ArgumentNullException(nameof(animal));

            IdSolicitud = "SOL-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            FechaPostulacion = DateTime.Now;
            Estado = EstadoSolicitud.PendienteRevision;
            Observaciones = "Solicitud ingresada al sistema. En espera de evaluación de requisitos.";
        }

        public void AsignarVoluntario(Voluntario voluntario)
        {
            VoluntarioEvaluador = voluntario ?? throw new ArgumentNullException(nameof(voluntario));
            Observaciones += $" | Voluntario evaluador asignado: {voluntario.NombreCompleto}";
        }

        public void EvaluarPreAprobacion()
        {
            if (AnimalSolicitado is Gato gato && !AdoptantePostulante.EsAptoParaGatos())
            {
                Estado = EstadoSolicitud.Rechazada;
                Observaciones = "Rechazada: La vivienda no cuenta con mallas de seguridad requeridas para felinos.";
                return;
            }

            if (AnimalSolicitado.Estado != EstadoSalud.AptoParaAdopcion && AnimalSolicitado.Estado != EstadoSalud.Rehabilitado)
            {
                Estado = EstadoSolicitud.PendienteRevision;
                Observaciones = $"En espera: El animal aún se encuentra en estado '{AnimalSolicitado.Estado}'.";
                return;
            }

            Estado = EstadoSolicitud.EntrevistaAgendada;
            Observaciones = "Pre-aprobada: Cumple perfil inicial. Entrevista agendada con la familia.";
        }

        public void ConcretarAdopcion(string resolucionFinal)
        {
            Estado = EstadoSolicitud.FinalizadaConExito;
            Observaciones = $"Adopción concretada con éxito. {resolucionFinal}";
            AnimalSolicitado.ActualizarEstadoSalud(EstadoSalud.Rehabilitado);
        }

        public string ObtenerResumen()
        {
            return $"[{IdSolicitud}] Adoptante: {AdoptantePostulante.NombreCompleto} -> Animal: {AnimalSolicitado.Nombre} ({AnimalSolicitado.ObtenerEspecie()}) | Estado: {Estado}";
        }

        public override string ToString() => ObtenerResumen();
    }
}
