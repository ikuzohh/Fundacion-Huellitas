using System;
using FundacionHuellitas.Core.Enums;
using FundacionHuellitas.Core.Models;
using Xunit;

namespace FundacionHuellitas.Tests
{
    public class AnimalTests
    {
        [Fact]
        public void Animal_DebeLanzarExcepcion_CuandoNombreEsVacio()
        {
            // Assert & Act
            Assert.Throws<ArgumentException>(() =>
            {
                new Perro("CAN-01", "", 12, 10.0, "MACHO", "Mestizo");
            });
        }

        [Fact]
        public void Animal_DebeLanzarExcepcion_CuandoPesoEsCeroONegativo()
        {
            // Assert & Act
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Gato("FEL-01", "Pelusa", 12, -2.5, "HEMBRA");
            });
        }

        [Fact]
        public void Animal_DebeLanzarExcepcion_CuandoSexoEsInvalido()
        {
            // Assert & Act
            Assert.Throws<ArgumentException>(() =>
            {
                new Perro("CAN-02", "Toby", 24, 15.0, "DESCONOCIDO", "Labrador");
            });
        }

        [Fact]
        public void Animal_DebeActualizarEstadoSalud_Correctamente()
        {
            // Arrange
            var perro = new Perro("CAN-03", "Max", 36, 22.0, "MACHO", "Pastor Alemán");

            // Act
            perro.ActualizarEstadoSalud(EstadoSalud.AptoParaAdopcion);

            // Assert
            Assert.Equal(EstadoSalud.AptoParaAdopcion, perro.Estado);
        }

        [Fact]
        public void Perro_Esterilizacion_NoPermiteRepetirse()
        {
            // Arrange
            var perro = new Perro("CAN-04", "Balto", 18, 14.0, "MACHO", "Husky", estaEsterilizado: true);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                perro.RealizarEsterilizacion("Dra. Morales");
            });
        }
    }
}
