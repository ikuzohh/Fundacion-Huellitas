using System;
using System.Collections.Generic;
using FundacionHuellitas.Core.Enums;
using FundacionHuellitas.Core.Models;
using Xunit;

namespace FundacionHuellitas.Tests
{
    public class ComposicionTests
    {
        [Fact]
        public void Composicion_AnimalDebeInicializarHistorialClinicoConRegistroDeIngreso()
        {
            // Arrange & Act
            Animal perro = new Perro("CAN-10", "Thor", 10, 8.0, "MACHO", "Beagle");

            // Assert: Al nacer el Animal, nace su Historial Médico (Composición)
            Assert.NotNull(perro.HistorialClinico);
            Assert.NotEmpty(perro.HistorialClinico);
            Assert.Equal(TipoAtencion.IngresoYRescate, perro.HistorialClinico[0].Tipo);
        }

        [Fact]
        public void Composicion_AnimalGestionaDirectamenteColeccionDeHistorialMedico()
        {
            // Arrange
            Animal gato = new Gato("FEL-10", "Misi", 12, 3.8, "HEMBRA");

            // Act: La interacción con el historial médico es delegada y gobernada por Animal
            var atencion = gato.RegistrarAtencion(
                TipoAtencion.Vacunacion,
                "Control de vacunación anual",
                "Triple Felina administrada",
                "Dr. Valenzuela",
                15000
            );

            // Assert
            Assert.Contains(atencion, gato.HistorialClinico);
            Assert.Equal(2, gato.HistorialClinico.Count); // 1 de ingreso + 1 nueva
            Assert.Equal("Dr. Valenzuela", atencion.Veterinario);
            Assert.Equal(15000, atencion.Costo);
        }

        [Fact]
        public void Composicion_HistorialClinicoProtegidoPorEncapsulamientoDefensivo()
        {
            // Arrange
            Animal perro = new Perro("CAN-11", "Chispa", 6, 5.0, "HEMBRA", "Mestizo");

            // Assert: La propiedad expuesta debe ser de sólo lectura (IReadOnlyList)
            Assert.IsAssignableFrom<IReadOnlyList<HistorialMedico>>(perro.HistorialClinico);
        }
    }
}
