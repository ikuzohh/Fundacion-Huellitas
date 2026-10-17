using System;
using System.Text;
using FundacionHuellitas.Core.Enums;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Subclase especializada que representa a un felino rescatado.
    /// Demuestra GENERALIZACIÓN/HERENCIA y especialización polimórfica de comportamiento.
    /// </summary>
    public class Gato : Animal
    {
        private string _testFelvFiv;
        private string _tipoPelaje;

        public bool EsIndoor { get; set; }

        public string TestFelvFiv
        {
            get => _testFelvFiv;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El resultado de FeLV/FIV no puede estar vacío.", nameof(value));
                _testFelvFiv = value.Trim();
            }
        }

        public string TipoPelaje
        {
            get => _tipoPelaje;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El tipo de pelaje no puede estar vacío.", nameof(value));
                _tipoPelaje = value.Trim();
            }
        }

        public bool VacunaTripleFelinaAlDia { get; private set; }

        public Gato(
            string idAnimal,
            string nombre,
            int edadMeses,
            double pesoKg,
            string sexo,
            string tipoPelaje = "Corto",
            bool esIndoor = true,
            string testFelvFiv = "Negativo",
            EstadoSalud estadoInicial = EstadoSalud.EnTratamiento,
            bool estaEsterilizado = false,
            bool vacunaTripleFelinaAlDia = false)
            : base(idAnimal, nombre, edadMeses, pesoKg, sexo, estadoInicial, estaEsterilizado)
        {
            TipoPelaje = tipoPelaje;
            EsIndoor = esIndoor;
            TestFelvFiv = testFelvFiv;
            VacunaTripleFelinaAlDia = vacunaTripleFelinaAlDia;
        }

        public override string ObtenerEspecie() => "Felino (Felis catus)";

        public override string HacerSonidoCaracteristico() => "¡Miau, miau!";

        /// <summary>
        /// Cálculo polimórfico de dosis farmacéutica para felinos:
        /// Debido a la deficiencia hepática de glucuronidación en gatos, muchos fármacos
        /// requieren un factor de ajuste del 85% para prevenir toxicidad renal o hepática.
        /// </summary>
        public override double CalcularDosisMedicamento(double factorConcentracionMgPorKg)
        {
            if (factorConcentracionMgPorKg <= 0)
                throw new ArgumentException("El factor de concentración debe ser mayor a 0.", nameof(factorConcentracionMgPorKg));

            // Factor felino conservador (0.85) debido a sensibilidad metabólica
            return Math.Round(PesoKg * factorConcentracionMgPorKg * 0.85, 2);
        }

        /// <summary>
        /// Emisión polimórfica de ficha clínica con protocolos exclusivos para felinos.
        /// </summary>
        public override string EmitirFichaClinica()
        {
            var sb = new StringBuilder();
            sb.AppendLine("╔════════════════════════════════════════════════════════════════════════╗");
            sb.AppendLine($"║ FICHA CLÍNICA FELINA - FUNDACIÓN HUELLITAS                           ║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"║ Código: {IdAnimal,-20} Nombre: {Nombre,-25}║");
            sb.AppendLine($"║ Especie: {ObtenerEspecie(),-38} Pelaje: {TipoPelaje,-10}║");
            sb.AppendLine($"║ Edad: {EdadMeses} meses | Peso: {PesoKg} kg | Sexo: {Sexo,-10}║");
            sb.AppendLine($"║ Estado Clínico: {Estado,-20} Esterilizado: {(EstaEsterilizado ? "SÍ" : "NO"),-5}║");
            sb.AppendLine($"║ Estilo de Vida: {(EsIndoor ? "Indoor Exclusivo" : "Mixto"),-18} Test FeLV/FIV: {TestFelvFiv,-12}║");
            sb.AppendLine($"║ Protocolo Felino: Vacuna Triple: {(VacunaTripleFelinaAlDia ? "Al día" : "Pendiente"),-12} Mallas: Requeridas ║");
            sb.AppendLine($"║ Sonido Característico: {HacerSonidoCaracteristico(),-38}║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════════════════╣");
            sb.Append(GenerarResumenHistorial());
            sb.AppendLine("╚════════════════════════════════════════════════════════════════════════╝");
            return sb.ToString();
        }

        public void RealizarTestFelvFiv(string resultado, string veterinario)
        {
            TestFelvFiv = resultado;
            RegistrarAtencion(
                TipoAtencion.ConsultaGeneral,
                $"Test rápido de Leucemia (FeLV) e Inmunodeficiencia Felina (FIV): {resultado}",
                resultado == "Negativo" ? "Apto para convivencia con otros felinos." : "Requiere aislamiento preventivo o convivencia solo con positivos.",
                veterinario,
                costo: 22000
            );
        }

        public void AplicarVacunaTripleFelina(string veterinario)
        {
            VacunaTripleFelinaAlDia = true;
            RegistrarAtencion(
                TipoAtencion.Vacunacion,
                "Inoculación de Vacuna Triple Felina (Panleucopenia, Rinotraqueitis, Calicivirus)",
                "Refuerzo anual recomendado.",
                veterinario,
                costo: 16000
            );
        }
    }
}
