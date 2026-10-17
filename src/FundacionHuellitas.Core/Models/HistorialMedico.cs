using System;
using FundacionHuellitas.Core.Enums;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Representa un registro clínico o atención médica efectuada a un animal.
    /// Forma parte integral de la COMPOSICIÓN con la clase Animal (su ciclo de vida
    /// depende exclusivamente de la existencia del Animal).
    /// </summary>
    public class HistorialMedico
    {
        private string _diagnostico;
        private string _tratamiento;
        private string _veterinario;
        private decimal _costo;

        public string IdAtencion { get; }
        public DateTime Fecha { get; }
        public TipoAtencion Tipo { get; }

        public string Diagnostico
        {
            get => _diagnostico;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El diagnóstico no puede estar vacío.", nameof(value));
                _diagnostico = value.Trim();
            }
        }

        public string Tratamiento
        {
            get => _tratamiento;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El tratamiento no puede estar vacío.", nameof(value));
                _tratamiento = value.Trim();
            }
        }

        public string Veterinario
        {
            get => _veterinario;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del veterinario no puede estar vacío.", nameof(value));
                _veterinario = value.Trim();
            }
        }

        public decimal Costo
        {
            get => _costo;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "El costo de la atención no puede ser negativo.");
                _costo = value;
            }
        }

        /// <summary>
        /// Constructor interno/protegido: Sólo debe ser instanciado a través de la clase Animal
        /// respetando el principio de Composición estricta.
        /// </summary>
        internal HistorialMedico(TipoAtencion tipo, string diagnostico, string tratamiento, string veterinario, decimal costo = 0)
        {
            IdAtencion = "MED-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Fecha = DateTime.Now;
            Tipo = tipo;
            Diagnostico = diagnostico;
            Tratamiento = tratamiento;
            Veterinario = veterinario;
            Costo = costo;
        }

        public string ObtenerFichaResumida()
        {
            return $"[{Fecha:dd/MM/yyyy HH:mm}] [{Tipo}] - Dx: {Diagnostico} | Tx: {Tratamiento} | Vet: {Veterinario} | Costo: ${Costo:N0}";
        }
    }
}
