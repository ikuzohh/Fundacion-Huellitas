using System;
using System.Collections.Generic;
using System.Linq;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Representa un centro o sede de rescate de la Fundación Huellitas.
    /// Demuestra la relación de AGREGACIÓN:
    /// El Refugio contiene colecciones de Animales y Voluntarios, pero el ciclo de vida de estos
    /// es independiente del Refugio (si una sede cierra, los animales y voluntarios siguen existiendo).
    /// </summary>
    public class Refugio
    {
        private readonly List<Animal> _animalesAlbergados;
        private readonly List<Voluntario> _voluntariosRegistrados;

        public string NombreSede { get; set; }
        public string Direccion { get; set; }
        public int CapacidadMaxima { get; }

        // Encapsulamiento defensivo: sólo lectura desde el exterior
        public IReadOnlyList<Animal> AnimalesAlbergados => _animalesAlbergados.AsReadOnly();
        public IReadOnlyList<Voluntario> VoluntariosRegistrados => _voluntariosRegistrados.AsReadOnly();

        public Refugio(string nombreSede, string direccion, int capacidadMaxima = 50)
        {
            if (string.IsNullOrWhiteSpace(nombreSede))
                throw new ArgumentException("El nombre de la sede no puede estar vacío.", nameof(nombreSede));

            if (capacidadMaxima <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacidadMaxima), "La capacidad debe ser mayor a 0.");

            NombreSede = nombreSede.Trim();
            Direccion = direccion;
            CapacidadMaxima = capacidadMaxima;

            _animalesAlbergados = new List<Animal>();
            _voluntariosRegistrados = new List<Voluntario>();
        }

        /// <summary>
        /// AGREGACIÓN: El animal ya existe previamente y se agrega al refugio.
        /// </summary>
        public void RecibirAnimal(Animal animal)
        {
            if (animal == null)
                throw new ArgumentNullException(nameof(animal));

            if (_animalesAlbergados.Count >= CapacidadMaxima)
                throw new InvalidOperationException($"El refugio {NombreSede} ha alcanzado su capacidad máxima ({CapacidadMaxima}).");

            if (!_animalesAlbergados.Contains(animal))
            {
                _animalesAlbergados.Add(animal);
            }
        }

        /// <summary>
        /// AGREGACIÓN: El animal puede retirarse (por ejemplo adoptado o derivado) sin ser destruido.
        /// </summary>
        public bool RetirarAnimal(Animal animal)
        {
            return _animalesAlbergados.Remove(animal);
        }

        public void InscribirVoluntario(Voluntario voluntario)
        {
            if (voluntario == null)
                throw new ArgumentNullException(nameof(voluntario));

            if (!_voluntariosRegistrados.Contains(voluntario))
            {
                _voluntariosRegistrados.Add(voluntario);
            }
        }

        public int ContarPorEspecie<T>() where T : Animal
        {
            return _animalesAlbergados.OfType<T>().Count();
        }
    }
}
