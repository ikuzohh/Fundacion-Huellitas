using System.Collections.Generic;
using FundacionHuellitas.Core.Models;
using Xunit;

namespace FundacionHuellitas.Tests
{
    public class PolimorfismoTests
    {
        [Fact]
        public void Polimorfismo_ColeccionHeterogenea_ResuelveSonidoSegunSubclase()
        {
            // Arrange
            List<Animal> coleccion = new List<Animal>
            {
                new Perro("CAN-20", "Boby", 12, 10.0, "MACHO", "Poodle"),
                new Gato("FEL-20", "Garfield", 24, 6.0, "MACHO")
            };

            // Act & Assert
            Assert.Equal("¡Guau, guau!", coleccion[0].HacerSonidoCaracteristico());
            Assert.Equal("¡Miau, miau!", coleccion[1].HacerSonidoCaracteristico());
        }

        [Fact]
        public void Polimorfismo_CalculoDosis_AplicaFactoresDiferenciadosPorEspecie()
        {
            // Arrange
            const double factorMgPorKg = 10.0;
            Animal canino = new Perro("CAN-21", "Rex", 18, 20.0, "MACHO", "Pastor"); // 20 kg
            Animal felino = new Gato("FEL-21", "Milo", 18, 5.0, "MACHO");            // 5 kg

            // Act
            double dosisCanino = canino.CalcularDosisMedicamento(factorMgPorKg);
            double dosisFelino = felino.CalcularDosisMedicamento(factorMgPorKg);

            // Assert
            // Canino: 20 kg * 10 = 200 mg
            Assert.Equal(200.0, dosisCanino);

            // Felino: 5 kg * 10 * 0.85 (factor de seguridad metabólica) = 42.5 mg
            Assert.Equal(42.5, dosisFelino);
        }

        [Fact]
        public void Polimorfismo_EmitirFichaClinica_GeneraFormatoEspecializado()
        {
            // Arrange
            Animal canino = new Perro("CAN-22", "Zeus", 30, 25.0, "MACHO", "Rottweiler");
            Animal felino = new Gato("FEL-22", "Simba", 10, 4.0, "MACHO");

            // Act
            string fichaCanino = canino.EmitirFichaClinica();
            string fichaFelino = felino.EmitirFichaClinica();

            // Assert
            Assert.Contains("FICHA CLÍNICA CANINA", fichaCanino);
            Assert.Contains("Vacuna Séxtuple", fichaCanino);

            Assert.Contains("FICHA CLÍNICA FELINA", fichaFelino);
            Assert.Contains("Test FeLV/FIV", fichaFelino);
        }
    }
}
