using System;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Representa a un colaborador de la fundación.
    /// Participa en una relación de AGREGACIÓN con la clase Refugio (el voluntario existe de manera
    /// independiente al refugio) y ASOCIACIÓN con el seguimiento de animales y solicitudes.
    /// </summary>
    public class Voluntario
    {
        private string _rut;
        private string _nombreCompleto;
        private int _horasSemanales;

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
                    throw new ArgumentException("El nombre del voluntario no puede estar vacío.", nameof(value));
                _nombreCompleto = value.Trim();
            }
        }

        public string Rol { get; set; } // Rescatista, Cuidador, Evaluador de Adopciones, Veterinario de Apoyo
        public string Telefono { get; set; }
        public bool EstaActivo { get; set; }

        public int HorasSemanales
        {
            get => _horasSemanales;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Las horas semanales no pueden ser negativas.");
                _horasSemanales = value;
            }
        }

        public Voluntario(string rut, string nombreCompleto, string rol, string telefono, int horasSemanales = 10, bool estaActivo = true)
        {
            Rut = rut;
            NombreCompleto = nombreCompleto;
            Rol = rol;
            Telefono = telefono;
            HorasSemanales = horasSemanales;
            EstaActivo = estaActivo;
        }

        public string ObtenerResumenVoluntario()
        {
            return $"Voluntario: {NombreCompleto} | Rol: {Rol} | Horas/Sem: {HorasSemanales} | Estado: {(EstaActivo ? "Activo" : "Inactivo")}";
        }

        public override string ToString() => ObtenerResumenVoluntario();
    }
}
