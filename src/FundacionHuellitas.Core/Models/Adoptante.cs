using System;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Representa a la persona postulante a la adopción responsable de un animal.
    /// Participa en una relación de ASOCIACIÓN con la clase SolicitudAdopcion y Animal.
    /// </summary>
    public class Adoptante
    {
        private string _rut;
        private string _nombreCompleto;
        private string _telefono;
        private string _correo;

        public string Rut
        {
            get => _rut;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El RUT no puede estar vacío.", nameof(value));
                _rut = value.Trim();
            }
        }

        public string NombreCompleto
        {
            get => _nombreCompleto;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío.", nameof(value));
                _nombreCompleto = value.Trim();
            }
        }

        public string Direccion { get; set; }

        public string Telefono
        {
            get => _telefono;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El teléfono no puede estar vacío.", nameof(value));
                _telefono = value.Trim();
            }
        }

        public string Correo
        {
            get => _correo;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("El correo debe tener un formato válido con '@'.", nameof(value));
                _correo = value.Trim();
            }
        }

        public string TipoVivienda { get; set; } // Casa, Departamento, Parcela
        public bool TieneMallasORejasSeguras { get; set; }
        public bool TieneOtrasMascotas { get; set; }

        public Adoptante(string rut, string nombreCompleto, string telefono, string correo, string direccion, string tipoVivienda, bool tieneMallasORejasSeguras, bool tieneOtrasMascotas)
        {
            Rut = rut;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Correo = correo;
            Direccion = direccion;
            TipoVivienda = tipoVivienda;
            TieneMallasORejasSeguras = tieneMallasORejasSeguras;
            TieneOtrasMascotas = tieneOtrasMascotas;
        }

        public bool EsAptoParaGatos()
        {
            // Regla de tenencia responsable para gatos: requiere mallas de protección
            return TieneMallasORejasSeguras;
        }

        public override string ToString()
        {
            return $"{NombreCompleto} (RUT: {Rut}) - Tel: {Telefono} - Vivienda: {TipoVivienda}";
        }
    }
}
