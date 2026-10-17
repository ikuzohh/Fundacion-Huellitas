using System;
using System.Collections.Generic;
using System.Text;
using FundacionHuellitas.Core.Enums;

namespace FundacionHuellitas.Core.Models
{
    /// <summary>
    /// Superclase abstracta que define las características y comportamientos base de cualquier animal
    /// rescatado por la Fundación Huellitas.
    /// Implementa el principio de abstracción, encapsulamiento defensivo y gestiona la relación
    /// de COMPOSICIÓN estricta con la colección de HistorialMedico.
    /// </summary>
    public abstract class Animal
    {
        // Atributos privados para encapsulamiento estricto
        private string _idAnimal;
        private string _nombre;
        private int _edadMeses;
        private double _pesoKg;
        private string _sexo;
        private EstadoSalud _estado;

        // COMPOSICIÓN: La colección es instanciada internamente y su ciclo de vida depende de Animal.
        // Se expone como IReadOnlyList para evitar alteraciones externas de la colección (encapsulamiento defensivo).
        private readonly List<HistorialMedico> _historialClinico;

        // Propiedades públicas con getters y validaciones
        public string IdAnimal
        {
            get => _idAnimal;
            protected set => _idAnimal = value;
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del animal no puede estar vacío.", nameof(value));
                _nombre = value.Trim();
            }
        }

        public int EdadMeses
        {
            get => _edadMeses;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "La edad no puede ser negativa.");
                _edadMeses = value;
            }
        }

        public double PesoKg
        {
            get => _pesoKg;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "El peso debe ser mayor a 0 kg.");
                _pesoKg = value;
            }
        }

        public string Sexo
        {
            get => _sexo;
            set
            {
                var upper = value?.Trim().ToUpper();
                if (upper != "MACHO" && upper != "HEMBRA")
                    throw new ArgumentException("El sexo debe ser 'MACHO' o 'HEMBRA'.", nameof(value));
                _sexo = upper;
            }
        }

        public EstadoSalud Estado
        {
            get => _estado;
            private set => _estado = value;
        }

        public DateTime FechaIngreso { get; }
        public bool EstaEsterilizado { get; private set; }

        // Encapsulamiento defensivo de la composición: sólo lectura desde el exterior
        public IReadOnlyList<HistorialMedico> HistorialClinico => _historialClinico.AsReadOnly();

        /// <summary>
        /// Constructor base protegido: Inicializa los atributos comunes y el ciclo de vida de la composición.
        /// </summary>
        protected Animal(string idAnimal, string nombre, int edadMeses, double pesoKg, string sexo, EstadoSalud estadoInicial = EstadoSalud.EnTratamiento, bool estaEsterilizado = false)
        {
            if (string.IsNullOrWhiteSpace(idAnimal))
                throw new ArgumentException("El ID del animal no puede estar vacío.", nameof(idAnimal));

            _idAnimal = idAnimal.Trim().ToUpper();
            Nombre = nombre;
            EdadMeses = edadMeses;
            PesoKg = pesoKg;
            Sexo = sexo;
            _estado = estadoInicial;
            EstaEsterilizado = estaEsterilizado;
            FechaIngreso = DateTime.Now;

            // Instanciación directa requerida por la relación de Composición
            _historialClinico = new List<HistorialMedico>();

            // Registro automático de ingreso al historial clínico
            RegistrarAtencion(
                TipoAtencion.IngresoYRescate,
                "Ingreso a las instalaciones de Fundación Huellitas tras rescate.",
                "Evaluación clínica inicial preventiva.",
                "Equipo Veterinario Huellitas"
            );
        }

        /// <summary>
        /// Demostración de COMPOSICIÓN en código:
        /// El Animal gestiona directamente la creación y almacenamiento de instancias de HistorialMedico.
        /// Ningún objeto externo puede inyectar atenciones huérfanas sin pasar por este método de control.
        /// </summary>
        public virtual HistorialMedico RegistrarAtencion(TipoAtencion tipo, string diagnostico, string tratamiento, string veterinario, decimal costo = 0)
        {
            var nuevaAtencion = new HistorialMedico(tipo, diagnostico, tratamiento, veterinario, costo);
            _historialClinico.Add(nuevaAtencion);
            return nuevaAtencion;
        }

        public void ActualizarEstadoSalud(EstadoSalud nuevoEstado)
        {
            _estado = nuevoEstado;
        }

        public void RealizarEsterilizacion(string veterinario)
        {
            if (EstaEsterilizado)
                throw new InvalidOperationException($"El animal {Nombre} ya se encuentra esterilizado.");

            EstaEsterilizado = true;
            RegistrarAtencion(
                TipoAtencion.EsterilizacionCirugia,
                "Procedimiento quirúrgico de esterilización exitoso.",
                "Reposo por 7 días y antibiótico profiláctico.",
                veterinario,
                costo: 35000
            );
        }

        // =========================================================================
        // MÉTODOS POLIMÓRFICOS ABSTRACTOS (Obligan a las subclases a especializar)
        // =========================================================================

        /// <summary>
        /// Devuelve el nombre de la especie específica (ej. "Canino" o "Felino").
        /// </summary>
        public abstract string ObtenerEspecie();

        /// <summary>
        /// Emite una ficha clínica integral con reglas de negocio personalizadas por especie.
        /// </summary>
        public abstract string EmitirFichaClinica();

        /// <summary>
        /// Calcula la dosis de medicamento adaptada al metabolismo fisiológico de la especie y su peso corporal.
        /// </summary>
        public abstract double CalcularDosisMedicamento(double factorConcentracionMgPorKg);

        /// <summary>
        /// Comportamiento acústico característico de cada especie (sonido).
        /// </summary>
        public abstract string HacerSonidoCaracteristico();

        /// <summary>
        /// Método de utilidad común para generar el listado de atenciones previas.
        /// Evita duplicación de código en subclases.
        /// </summary>
        protected string GenerarResumenHistorial()
        {
            var sb = new StringBuilder();
            sb.AppendLine("    --- HISTORIAL CLÍNICO (Composición) ---");
            if (_historialClinico.Count == 0)
            {
                sb.AppendLine("    Sin registros médicos.");
            }
            else
            {
                foreach (var h in _historialClinico)
                {
                    sb.AppendLine($"    • {h.ObtenerFichaResumida()}");
                }
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"[{IdAnimal}] {Nombre} ({ObtenerEspecie()}) - {EdadMeses} meses - {PesoKg} kg - Estado: {Estado}";
        }
    }
}
