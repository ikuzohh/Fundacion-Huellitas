using System;
using System.Text;
using FundacionHuellitas.Core.Enums;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Subclase especializada que representa a un canino rescatado.
    /// Demuestra GENERALIZACIÓN/HERENCIA y especialización polimórfica de comportamiento.
    /// </summary>
    public class Perro : Animal
    {
        private string _raza;
        private string _nivelEnergia;

        public string Raza
        {
            get => _raza;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La raza no puede estar vacía.", nameof(value));
                _raza = value.Trim();
            }
        }

        public string NivelEnergia
        {
            get => _nivelEnergia;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nivel de energía no puede estar vacío.", nameof(value));
                _nivelEnergia = value.Trim();
            }
        }

        public bool EsAptoConNinos { get; set; }
        public bool VacunacionSextupleAlDia { get; private set; }

        public Perro(
            string idAnimal,
            string nombre,
            int edadMeses,
            double pesoKg,
            string sexo,
            string raza,
            string nivelEnergia = "Media",
            bool esAptoConNinos = true,
            EstadoSalud estadoInicial = EstadoSalud.EnTratamiento,
            bool estaEsterilizado = false,
            bool vacunacionSextupleAlDia = false)
            : base(idAnimal, nombre, edadMeses, pesoKg, sexo, estadoInicial, estaEsterilizado)
        {
            Raza = raza;
            NivelEnergia = nivelEnergia;
            EsAptoConNinos = esAptoConNinos;
            VacunacionSextupleAlDia = vacunacionSextupleAlDia;
        }

        public override string ObtenerEspecie() => "Canino (Canis lupus familiaris)";

        public override string HacerSonidoCaracteristico() => "¡Guau, guau!";

        /// <summary>
        /// Cálculo polimórfico de dosis farmacéutica para caninos:
        /// Considera el metabolismo estándar canino multiplicando por el factor base.
        /// </summary>
        public override double CalcularDosisMedicamento(double factorConcentracionMgPorKg)
        {
            if (factorConcentracionMgPorKg <= 0)
                throw new ArgumentException("El factor de concentración debe ser mayor a 0.", nameof(factorConcentracionMgPorKg));

            // Metabolismo canino base: peso en kg * dosis recomendada
            return Math.Round(PesoKg * factorConcentracionMgPorKg, 2);
        }

        /// <summary>
        /// Emisión polimórfica de ficha clínica con protocolos exclusivos para caninos.
        /// </summary>
        public override string EmitirFichaClinica()
        {
            var sb = new StringBuilder();
            sb.AppendLine("╔════════════════════════════════════════════════════════════════════════╗");
            sb.AppendLine($"║ FICHA CLÍNICA CANINA - FUNDACIÓN HUELLITAS                           ║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"║ Código: {IdAnimal,-20} Nombre: {Nombre,-25}║");
            sb.AppendLine($"║ Especie: {ObtenerEspecie(),-38} Raza: {Raza,-12}║");
            sb.AppendLine($"║ Edad: {EdadMeses} meses | Peso: {PesoKg} kg | Sexo: {Sexo,-10}║");
            sb.AppendLine($"║ Estado Clínico: {Estado,-20} Esterilizado: {(EstaEsterilizado ? "SÍ" : "NO"),-5}║");
            sb.AppendLine($"║ Nivel de Energía: {NivelEnergia,-15} Apto con Niños: {(EsAptoConNinos ? "SÍ" : "NO"),-5}║");
            sb.AppendLine($"║ Protocolo Canino: Vacuna Séxtuple: {(VacunacionSextupleAlDia ? "Al día" : "Pendiente"),-15} Paseo: Diario ║");
            sb.AppendLine($"║ Sonido Característico: {HacerSonidoCaracteristico(),-38}║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════════════════╣");
            sb.Append(GenerarResumenHistorial());
            sb.AppendLine("╚════════════════════════════════════════════════════════════════════════╝");
            return sb.ToString();
        }

        public void AplicarVacunaSextuple(string veterinario)
        {
            VacunacionSextupleAlDia = true;
            RegistrarAtencion(
                TipoAtencion.Vacunacion,
                "Inoculación de Vacuna Séxtuple Canina (Distemper, Parvovirus, Hepatitis, etc.)",
                "Monitoreo de posibles reacciones durante 24 horas.",
                veterinario,
                costo: 18000
            );
        }
    }
}
