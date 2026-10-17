using System;
using System.Collections.Generic;
using FundacionHuellitas.Core.Enums;
using FundacionHuellitas.Core.Models;

namespace FundacionHuellitas.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MostrarEncabezado();

            // =========================================================================
            // 1. CREACIÓN DE OBJETOS Y DEMOSTRACIÓN DE HERENCIA Y ESPECIALIZACIÓN
            // =========================================================================
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================================================================");
            Console.WriteLine(" 1. MODELO DE DOMINIO: HERENCIA Y ESPECIALIZACIÓN (ANIMAL -> PERRO / GATO)");
            Console.WriteLine("================================================================================");
            Console.ResetColor();

            Perro perro1 = new Perro(
                idAnimal: "CAN-001",
                nombre: "Rocky",
                edadMeses: 24,
                pesoKg: 18.5,
                sexo: "MACHO",
                raza: "Quiltro Mestizo",
                nivelEnergia: "Alta",
                esAptoConNinos: true,
                estadoInicial: EstadoSalud.AptoParaAdopcion,
                estaEsterilizado: true,
                vacunacionSextupleAlDia: true
            );

            Gato gato1 = new Gato(
                idAnimal: "FEL-002",
                nombre: "Michi",
                edadMeses: 14,
                pesoKg: 4.2,
                sexo: "HEMBRA",
                tipoPelaje: "Calicó / Corto",
                esIndoor: true,
                testFelvFiv: "Negativo",
                estadoInicial: EstadoSalud.AptoParaAdopcion,
                estaEsterilizado: true,
                vacunaTripleFelinaAlDia: true
            );

            Perro perro2 = new Perro(
                idAnimal: "CAN-003",
                nombre: "Luna",
                edadMeses: 8,
                pesoKg: 12.0,
                sexo: "HEMBRA",
                raza: "Pastor Alemán Mix",
                nivelEnergia: "Muy Alta",
                esAptoConNinos: true,
                estadoInicial: EstadoSalud.EnTratamiento,
                estaEsterilizado: false
            );

            Gato gato2 = new Gato(
                idAnimal: "FEL-004",
                nombre: "Oliver",
                edadMeses: 36,
                pesoKg: 5.1,
                sexo: "MACHO",
                tipoPelaje: "Atigrado Gris",
                esIndoor: true,
                testFelvFiv: "Negativo",
                estadoInicial: EstadoSalud.Estable,
                estaEsterilizado: true
            );

            // =========================================================================
            // 2. DEMOSTRACIÓN DE COMPOSICIÓN (ANIMAL CONTROLA HISTORIAL MÉDICO)
            // =========================================================================
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n================================================================================");
            Console.WriteLine(" 2. DEMOSTRACIÓN DE COMPOSICIÓN EN CÓDIGO (Animal *-- HistorialMedico)");
            Console.WriteLine("    * El ciclo de vida de cada HistorialMedico depende 100% de la instancia Animal.");
            Console.WriteLine("    * Se administran exclusivamente mediante métodos miembros de Animal.");
            Console.WriteLine("================================================================================");
            Console.ResetColor();

            // Agregar atenciones clínicas directamente a través de los métodos de Animal
            perro1.RegistrarAtencion(
                TipoAtencion.ConsultaGeneral,
                "Control post-operatorio de esterilización y revisión dental.",
                "Cicatrización limpia. Limpieza de tártaro dental realizada.",
                "Dra. Camila Morales",
                costo: 25000
            );

            perro1.AplicarVacunaSextuple("Dr. Felipe Valenzuela");

            gato1.RealizarTestFelvFiv("Negativo", "Dra. Camila Morales");
            gato1.AplicarVacunaTripleFelina("Dra. Camila Morales");

            Console.WriteLine($"✔ Se agregaron atenciones al historial médico de {perro1.Nombre} ({perro1.HistorialClinico.Count} registros).");
            Console.WriteLine($"✔ Se agregaron atenciones al historial médico de {gato1.Nombre} ({gato1.HistorialClinico.Count} registros).");
            Console.WriteLine("✔ Encapsulamiento defensivo: HistorialClinico expone 'IReadOnlyList' para impedir manipulaciones externas.");

            // =========================================================================
            // 3. DEMOSTRACIÓN DE POLIMORFISMO DINÁMICO EN COLECCIÓN HETEROGÉNEA
            // =========================================================================
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================================================================================");
            Console.WriteLine(" 3. POLIMORFISMO DINÁMICO EN CONSOLA (Colección Heterogénea List<Animal>)");
            Console.WriteLine("    * Se procesan instancias de Perro y Gato a través de la referencia abstracta Animal.");
            Console.WriteLine("    * Los métodos EmitirFichaClinica(), HacerSonidoCaracteristico() y");
            Console.WriteLine("      CalcularDosisMedicamento() se resuelven dinámicamente en tiempo de ejecución.");
            Console.WriteLine("================================================================================");
            Console.ResetColor();

            // Colección heterogénea de tipo Animal
            List<Animal> animalesRescatados = new List<Animal>
            {
                perro1,
                gato1,
                perro2,
                gato2
            };

            const double factorDosisAntibiotico = 10.0; // 10 mg/kg

            foreach (Animal animal in animalesRescatados)
            {
                Console.WriteLine();
                // Invocación polimórfica de emisión de ficha clínica
                Console.WriteLine(animal.EmitirFichaClinica());

                // Invocación polimórfica de cálculo metabólico de dosis y sonido
                double dosisCalculada = animal.CalcularDosisMedicamento(factorDosisAntibiotico);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"  [DEMOSTRACIÓN POLIMÓRFICA EN TIEMPO DE EJECUCIÓN]");
                Console.WriteLine($"  • Tipo Concreto: {animal.GetType().Name}");
                Console.WriteLine($"  • Sonido Polimórfico: {animal.HacerSonidoCaracteristico()}");
                Console.WriteLine($"  • Cálculo Farmacológico Polimórfico (Dosis Base 10 mg/kg): {dosisCalculada} mg");
                Console.WriteLine($"    (Explicación: Los caninos calculan directamente por peso; los felinos aplican factor de ajuste renal del 85%).");
                Console.ResetColor();
                Console.WriteLine(new string('-', 80));
            }

            // =========================================================================
            // 4. DEMOSTRACIÓN DE ASOCIACIÓN Y AGREGACIÓN
            // =========================================================================
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n================================================================================");
            Console.WriteLine(" 4. DEMOSTRACIÓN DE AGREGACIÓN Y ASOCIACIÓN");
            Console.WriteLine("================================================================================");
            Console.ResetColor();

            // AGREGACIÓN: Refugio agrupa Animales y Voluntarios
            Refugio refugioCentral = new Refugio("Refugio Central Santiago", "Av. Providencia #1540", capacidadMaxima: 30);
            Voluntario voluntario1 = new Voluntario("18.452.339-2", "Matías Silva", "Evaluador de Adopciones", "+56 9 8877 6655", horasSemanales: 15);
            
            refugioCentral.InscribirVoluntario(voluntario1);
            refugioCentral.RecibirAnimal(perro1);
            refugioCentral.RecibirAnimal(gato1);

            Console.WriteLine($"[AGREGACIÓN]");
            Console.WriteLine($"Sede: {refugioCentral.NombreSede} - Capacidad: {refugioCentral.CapacidadMaxima}");
            Console.WriteLine($"Animales albergados: {refugioCentral.AnimalesAlbergados.Count} (Caninos: {refugioCentral.ContarPorEspecie<Perro>()}, Felinos: {refugioCentral.ContarPorEspecie<Gato>()})");
            Console.WriteLine($"Voluntarios activos: {refugioCentral.VoluntariosRegistrados.Count}");

            // ASOCIACIÓN: Solicitud de Adopción vincula Adoptante con Animal y Voluntario
            Console.WriteLine($"\n[ASOCIACIÓN]");
            Adoptante adoptante1 = new Adoptante(
                rut: "17.982.114-K",
                nombreCompleto: "Constanza Morales",
                telefono: "+56 9 7654 3210",
                correo: "constanza.morales@correo.cl",
                direccion: "Calle Las Flores 450, Providencia",
                tipoVivienda: "Departamento",
                tieneMallasORejasSeguras: true,
                tieneOtrasMascotas: false
            );

            SolicitudAdopcion solicitud = new SolicitudAdopcion(adoptante1, gato1);
            solicitud.AsignarVoluntario(voluntario1);
            solicitud.EvaluarPreAprobacion();

            Console.WriteLine(solicitud.ObtenerResumen());
            Console.WriteLine($"Estado actual de la solicitud: {solicitud.Estado}");
            Console.WriteLine($"Observaciones del proceso: {solicitud.Observaciones}");

            // Concreción exitosa de la adopción
            solicitud.ConcretarAdopcion("Contrato firmado. Compromiso de seguimiento post-adopción a los 30 y 90 días.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✔ {solicitud.Observaciones}");
            Console.ResetColor();

            Console.WriteLine("\n================================================================================");
            Console.WriteLine(" FIN DE LA DEMOSTRACIÓN - SISTEMA FUNDACIÓN HUELLITAS");
            Console.WriteLine("================================================================================");
        }

        private static void MostrarEncabezado()
        {
            try { Console.Clear(); } catch { }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 INSTITUTO PROFESIONAL AIEP - ESCUELA TIC                     ║");
            Console.WriteLine("║            MÓDULO: TALLER DE PROGRAMACIÓN (PRO205) - CICLO 2026              ║");
            Console.WriteLine("║         EVALUACIÓN SUMATIVA N° 3 / ARTICULACIÓN ENE - CASO HUELLITAS         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
