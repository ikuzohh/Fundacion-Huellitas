using System;
using System.Collections.Generic;
using FundacionHuellitas.Core.Enums;
using FundacionHuellitas.Core.Models;
using Xunit;

namespace FundacionHuellitas.Tests
{
    /// <summary>
    /// Suite de auditoría exhaustiva de arquitectura, encapsulamiento defensivo,
    /// flujo transaccional de adopción y límites de agregación/composición.
    /// </summary>
    public class AuditoriaIntegracionTests
    {
        // =========================================================================
        // 1. AUDITORÍA DEL FLUJO DE ADOPCIÓN RESPONSABLE (ASOCIACIÓN)
        // =========================================================================

        [Fact]
        public void SolicitudAdopcion_DebeRechazarGato_SiViviendaNoTieneMallasDeSeguridad()
        {
            // Arrange: Adoptante en departamento sin mallas
            var adoptanteSinMallas = new Adoptante(
                rut: "19.345.678-9",
                nombreCompleto: "Felipe Soto",
                telefono: "+56 9 9123 4567",
                correo: "felipe.soto@email.com",
                direccion: "Av. Las Condes #1200",
                tipoVivienda: "Departamento",
                tieneMallasORejasSeguras: false, // Inadecuado para felinos
                tieneOtrasMascotas: false
            );

            var gato = new Gato("FEL-AUDIT-1", "Salem", 10, 4.0, "MACHO", estadoInicial: EstadoSalud.AptoParaAdopcion);
            var solicitud = new SolicitudAdopcion(adoptanteSinMallas, gato);

            // Act
            solicitud.EvaluarPreAprobacion();

            // Assert: Regla de tenencia responsable gatuna aplicada
            Assert.Equal(EstadoSolicitud.Rechazada, solicitud.Estado);
            Assert.Contains("Rechazada", solicitud.Observaciones);
            Assert.Contains("mallas de seguridad", solicitud.Observaciones);
        }

        [Fact]
        public void SolicitudAdopcion_DebeAgendarEntrevista_CuandoAnimalEstaAptoYPerfilCumple()
        {
            // Arrange: Adoptante apto con mallas para gato
            var adoptanteApto = new Adoptante(
                rut: "18.765.432-1",
                nombreCompleto: "Camila Fernandez",
                telefono: "+56 9 8234 5678",
                correo: "camila.f@email.com",
                direccion: "Pasaje Los Sauces 44",
                tipoVivienda: "Casa",
                tieneMallasORejasSeguras: true,
                tieneOtrasMascotas: true
            );

            var gato = new Gato("FEL-AUDIT-2", "Luna", 16, 3.5, "HEMBRA", estadoInicial: EstadoSalud.AptoParaAdopcion);
            var solicitud = new SolicitudAdopcion(adoptanteApto, gato);

            // Act
            solicitud.EvaluarPreAprobacion();

            // Assert
            Assert.Equal(EstadoSolicitud.EntrevistaAgendada, solicitud.Estado);
            Assert.Contains("Pre-aprobada", solicitud.Observaciones);
        }

        [Fact]
        public void SolicitudAdopcion_DebeMantenerEnEspera_SiAnimalAunNoEstaRehabilitado()
        {
            // Arrange
            var adoptante = new Adoptante(
                rut: "15.111.222-3",
                nombreCompleto: "Andrés Gomez",
                telefono: "+56 9 7111 2222",
                correo: "andres.g@email.com",
                direccion: "Calle Central 100",
                tipoVivienda: "Casa",
                tieneMallasORejasSeguras: true,
                tieneOtrasMascotas: false
            );

            var perroEnTratamiento = new Perro("CAN-AUDIT-3", "Negro", 6, 8.0, "MACHO", "Mestizo", estadoInicial: EstadoSalud.EnTratamiento);
            var solicitud = new SolicitudAdopcion(adoptante, perroEnTratamiento);

            // Act
            solicitud.EvaluarPreAprobacion();

            // Assert: No se puede agendar adopción mientras el animal esté en tratamiento médico
            Assert.Equal(EstadoSolicitud.PendienteRevision, solicitud.Estado);
            Assert.Contains("En espera", solicitud.Observaciones);
        }

        [Fact]
        public void SolicitudAdopcion_ConcretarAdopcion_FinalizaProcesoYActualizaEstado()
        {
            // Arrange
            var adoptante = new Adoptante("17.444.555-6", "Loreto Silva", "+56 9 6444 5555", "loreto@email.cl", "Av. Brasil 300", "Casa", true, false);
            var perro = new Perro("CAN-AUDIT-4", "Oso", 30, 20.0, "MACHO", "Boyero", estadoInicial: EstadoSalud.AptoParaAdopcion);
            var voluntario = new Voluntario("16.888.999-0", "Pablo Herrera", "Evaluador", "+56 9 8888 9999");
            var solicitud = new SolicitudAdopcion(adoptante, perro);
            solicitud.AsignarVoluntario(voluntario);

            // Act
            solicitud.ConcretarAdopcion("Contrato firmado conforme a Ley Cholito.");

            // Assert
            Assert.Equal(EstadoSolicitud.FinalizadaConExito, solicitud.Estado);
            Assert.Equal(voluntario, solicitud.VoluntarioEvaluador);
            Assert.Contains("firmado", solicitud.Observaciones);
        }

        // =========================================================================
        // 2. AUDITORÍA DEL REFUGIO (AGREGACIÓN, LÍMITES Y POLIMORFISMO DE CONTEO)
        // =========================================================================

        [Fact]
        public void Refugio_DebeLanzarExcepcion_CuandoSeSuperaCapacidadMaxima()
        {
            // Arrange: Refugio pequeño con capacidad para 2 animales
            var refugioPequeno = new Refugio("Sede Emergencia", "Calle Sur 12", capacidadMaxima: 2);
            var animal1 = new Perro("CAN-LIM-1", "Perro1", 12, 10.0, "MACHO", "Quiltro");
            var animal2 = new Gato("FEL-LIM-2", "Gato2", 12, 4.0, "HEMBRA");
            var animalExcedente = new Perro("CAN-LIM-3", "Perro3", 12, 12.0, "MACHO", "Quiltro");

            refugioPequeno.RecibirAnimal(animal1);
            refugioPequeno.RecibirAnimal(animal2);

            // Act & Assert: Intentar ingresar un 3er animal debe lanzar InvalidOperationException
            Assert.Throws<InvalidOperationException>(() =>
            {
                refugioPequeno.RecibirAnimal(animalExcedente);
            });
        }

        [Fact]
        public void Refugio_NoDebeDuplicarAnimal_SiSeIngresaMasDeUnaVez()
        {
            // Arrange
            var refugio = new Refugio("Sede Central", "Av. España 500", capacidadMaxima: 10);
            var animal = new Perro("CAN-DUP-1", "Duplicado", 20, 15.0, "MACHO", "Pastor");

            // Act
            refugio.RecibirAnimal(animal);
            refugio.RecibirAnimal(animal); // Segundo intento del mismo animal

            // Assert
            Assert.Single(refugio.AnimalesAlbergados);
        }

        [Fact]
        public void Refugio_DebeContarEspeciesCorrectamente_MedianteMetodoGenerico()
        {
            // Arrange
            var refugio = new Refugio("Sede Recoleta", "Av. Recoleta 800", capacidadMaxima: 20);
            refugio.RecibirAnimal(new Perro("CAN-C1", "PerroA", 10, 8.0, "MACHO", "Fox Terrier"));
            refugio.RecibirAnimal(new Perro("CAN-C2", "PerroB", 15, 12.0, "HEMBRA", "Mestizo"));
            refugio.RecibirAnimal(new Gato("FEL-C1", "GatoA", 8, 3.5, "MACHO"));

            // Act & Assert
            Assert.Equal(2, refugio.ContarPorEspecie<Perro>());
            Assert.Equal(1, refugio.ContarPorEspecie<Gato>());
            Assert.Equal(3, refugio.AnimalesAlbergados.Count);

            // Retiro del gato por adopción
            var retirado = refugio.RetirarAnimal(refugio.AnimalesAlbergados[2]);
            Assert.True(retirado);
            Assert.Equal(0, refugio.ContarPorEspecie<Gato>());
            Assert.Equal(2, refugio.AnimalesAlbergados.Count);
        }

        // =========================================================================
        // 3. AUDITORÍA DE ENCAPSULAMIENTO DEFENSIVO (INVARIANTES Y SEGURIDAD)
        // =========================================================================

        [Fact]
        public void Adoptante_DebeLanzarExcepcion_SiCorreoNoTieneArroba()
        {
            // Assert & Act
            Assert.Throws<ArgumentException>(() =>
            {
                new Adoptante("16.000.111-2", "Juan Perez", "+56 9 1234 5678", "correo_sin_arroba.cl", "Calle 1", "Casa", true, false);
            });
        }

        [Fact]
        public void Voluntario_DebeLanzarExcepcion_SiHorasSemanalesSonNegativas()
        {
            // Assert & Act
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Voluntario("17.888.777-6", "Diego Diaz", "Rescatista", "+56 9 8888 7777", horasSemanales: -5);
            });
        }

        [Fact]
        public void Composicion_HistorialClinico_NoPuedeCastearseAListaMutable()
        {
            // Arrange
            Animal perro = new Perro("CAN-CAST-1", "Protegido", 14, 11.0, "MACHO", "Mestizo");

            // Act & Assert: Demuestra que la lista está verdaderamente protegida contra cast destructivo
            // El getter expone _historialClinico.AsReadOnly(), por lo tanto no se puede convertir a List<HistorialMedico>
            Assert.Throws<InvalidCastException>(() =>
            {
                var listaMutable = (List<HistorialMedico>)perro.HistorialClinico;
            });
        }

        [Fact]
        public void Animal_CalcularDosis_DebeLanzarExcepcion_CuandoFactorEsCeroONegativo()
        {
            // Arrange
            Animal perro = new Perro("CAN-DOSIS-1", "Drako", 20, 18.0, "MACHO", "Boxer");
            Animal gato = new Gato("FEL-DOSIS-1", "Misu", 12, 4.0, "HEMBRA");

            // Assert & Act
            Assert.Throws<ArgumentException>(() => perro.CalcularDosisMedicamento(0));
            Assert.Throws<ArgumentException>(() => perro.CalcularDosisMedicamento(-5.0));
            Assert.Throws<ArgumentException>(() => gato.CalcularDosisMedicamento(0));
            Assert.Throws<ArgumentException>(() => gato.CalcularDosisMedicamento(-2.0));
        }

        // =========================================================================
        // 4. AUDITORÍA DE PROCEDIMIENTOS CLÍNICOS ESPECIALIZADOS
        // =========================================================================

        [Fact]
        public void Perro_AplicarVacunaSextuple_ActualizaBanderaYAgregaRegistroAlHistorial()
        {
            // Arrange
            var perro = new Perro("CAN-VAC-1", "Bobby", 8, 6.0, "MACHO", "Poodle", vacunacionSextupleAlDia: false);
            int atencionesPrevias = perro.HistorialClinico.Count;

            // Act
            perro.AplicarVacunaSextuple("Dr. Valenzuela");

            // Assert
            Assert.True(perro.VacunacionSextupleAlDia);
            Assert.Equal(atencionesPrevias + 1, perro.HistorialClinico.Count);
            Assert.Equal(TipoAtencion.Vacunacion, perro.HistorialClinico[perro.HistorialClinico.Count - 1].Tipo);
        }

        [Fact]
        public void Gato_RealizarTestFelvFiv_ActualizaResultadoYAgregaAtencionAlHistorial()
        {
            // Arrange
            var gato = new Gato("FEL-TEST-1", "Tom", 12, 4.2, "MACHO", testFelvFiv: "Pendiente");
            int atencionesPrevias = gato.HistorialClinico.Count;

            // Act
            gato.RealizarTestFelvFiv("Negativo", "Dra. Camila Morales");

            // Assert
            Assert.Equal("Negativo", gato.TestFelvFiv);
            Assert.Equal(atencionesPrevias + 1, gato.HistorialClinico.Count);
            Assert.Equal(TipoAtencion.ConsultaGeneral, gato.HistorialClinico[gato.HistorialClinico.Count - 1].Tipo);
            Assert.Contains("Negativo", gato.HistorialClinico[gato.HistorialClinico.Count - 1].Diagnostico);
        }
    }
}
