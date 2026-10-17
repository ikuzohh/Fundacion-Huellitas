# Proyecto: Fundación Huellitas — Sistema de Gestión y Adopción
### AIEP | Escuela de Tecnologías de la Información y Comunicación
**Módulo:** Taller de Programación (PRO205)  
**Evaluación:** Evaluación Sumativa N° 3 (35% Ponderación) articulada con la Evaluación Nacional de Especialidad (ENE 2026)  
**Opción Seleccionada:** Opción A — Caso "Fundación Huellitas" (Modelado UML y Dominio POO)

---

## 📋 Resumen del Proyecto

Este repositorio contiene la solución completa de ingeniería de software para la **Fundación Huellitas**, una organización sin fines de lucro dedicada al rescate, atención veterinaria y adopción responsable de caninos y felinos.

El proyecto aborda integralmente los requerimientos base de la ENE (Subtareas 1A y 1B) y la pregunta complementaria práctica de la Sumativa 3:
- **Subtarea 1A:** Esquema conceptual preliminar y justificación técnica de las 4 relaciones clave de la POO (Herencia, Composición, Agregación y Asociación).
- **Subtarea 1B:** Diagrama de clases formal en notación UML (8 clases) y matriz de requerimientos del sistema cubiertos por el diseño.
- **Paso Extra Sumativa 3:** Implementación en código C# (.NET 8.0) con encapsulamiento defensivo, composición estricta entre `Animal` e `HistorialMedico`, y demostración de polimorfismo dinámico en consola sobre una colección heterogénea `List<Animal>`.
- **Verificación Automatizada:** Suite de 11 pruebas unitarias con **xUnit** (100% aprobadas).

---

## 🏗 Arquitectura de la Solución

```text
entrega final/
├── docs/                                    # Documentación técnica y diagramas
│   ├── INFORME_TECNICO_SUMATIVA_3_HUELLITAS.docx   # Informe técnico oficial en Word
│   ├── INFORME_TECNICO_SUMATIVA_3_HUELLITAS.md     # Informe técnico oficial en Markdown
│   ├── diagramas/
│   │   ├── diagrama_clases_formal_1b.svg           # Diagrama UML formal completo (vectorial HD)
│   │   ├── esquema_conceptual_1a.svg               # Esquema conceptual preliminar (vectorial HD)
│   │   ├── visor_diagramas_uml.html                # Visor interactivo web listo para imprimir
│   │   ├── diagrama_clases_formal_1b.mmd           # Definición Mermaid UML formal
│   │   └── esquema_conceptual_1a.mmd               # Definición Mermaid conceptual
│   └── evidencias/
│       ├── evidencia_consola.txt                   # Registro de salida en ejecución de consola
│       └── evidencia_tests.txt                     # Registro de resultados de pruebas unitarias
├── src/                                     # Código Fuente C# (.NET 8.0)
│   ├── FundacionHuellitas.sln               # Solución principal de Visual Studio / .NET CLI
│   ├── FundacionHuellitas.Core/             # Capa de Dominio (Modelos POO y Enums)
│   │   ├── Enums/
│   │   │   ├── EstadoSalud.cs
│   │   │   ├── TipoAtencion.cs
│   │   │   └── EstadoSolicitud.cs
│   │   └── Models/
│   │       ├── Animal.cs                    # Superclase abstracta con Composición de historial
│   │       ├── Perro.cs                     # Subclase especializada (Herencia Canina)
│   │       ├── Gato.cs                      # Subclase especializada (Herencia Felina)
│   │       ├── HistorialMedico.cs           # Composición con ciclo de vida gobernado por Animal
│   │       ├── Adoptante.cs                 # Asociación con SolicitudAdopcion
│   │       ├── Voluntario.cs                # Agregación con Refugio y Asociación
│   │       ├── SolicitudAdopcion.cs         # Orquestación de proceso de adopción
│   │       └── Refugio.cs                   # Agregación de animales y voluntarios
│   ├── FundacionHuellitas.App/              # Capa de Presentación / Consola
│   │   └── Program.cs                       # Demostración en vivo de polimorfismo dinámico
│   └── FundacionHuellitas.Tests/            # Capa de Pruebas Automatizadas (xUnit)
│       ├── AnimalTests.cs                   # Pruebas de encapsulamiento y validaciones
│       ├── ComposicionTests.cs              # Pruebas de composición estricta
│       └── PolimorfismoTests.cs             # Pruebas de polimorfismo dinámico
└── README.md
```

---

## 🚀 Guía de Compilación y Ejecución

### Requisitos
- .NET 8.0 SDK instalado (`dotnet --version`).

### 1. Compilar el Proyecto
```powershell
dotnet build src/FundacionHuellitas.sln
```

### 2. Ejecutar Pruebas Unitarias Automatizadas
```powershell
dotnet test src/FundacionHuellitas.sln --logger "console;verbosity=detailed"
```
*Total de pruebas: 11 superadas (100% de éxito).*

### 3. Ejecutar la Demostración Polimórfica en Consola
```powershell
dotnet run --project src/FundacionHuellitas.App
```

---

## 📊 Rúbrica Oficial AIEP (100 Puntos)

| Criterio | Puntaje | Estado |
| :--- | :---: | :---: |
| **Resolución Base del Caso ENE (Subtareas 1A y 1B)** | **40 Pts** | ✅ Resuelto exhaustivamente con justificación de 4 relaciones y diagrama UML formal |
| **Pregunta Complementaria (Paso Extra Sumativa 3)** | **35 Pts** | ✅ Implementación en C#, Composición demostrada y Polimorfismo dinámico |
| **Pruebas y Verificación Automatizada** | **15 Pts** | ✅ 11 pruebas unitarias xUnit automatizadas 100% exitosas |
| **Calidad Técnica y Buenas Prácticas** | **5 Pts** | ✅ Encapsulamiento defensivo, nombres significativos, tipado estricto y SOLID |
| **Estructura de Entrega y Control de Versiones** | **5 Pts** | ✅ Informe en PDF/DOCX, manual técnico, evidencias y Git con commits ordenados |
| **TOTAL** | **100 Pts** | **Nota Máxima: 7.0** |
