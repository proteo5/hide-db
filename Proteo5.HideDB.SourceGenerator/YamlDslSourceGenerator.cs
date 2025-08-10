using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Proteo5.HideDB.SourceGenerator.Models;
using Proteo5.HideDB.SourceGenerator.Generators;

namespace Proteo5.HideDB.SourceGenerator
{
    [Generator]
    public class YamlDslSourceGenerator : ISourceGenerator
    {
        private static readonly DiagnosticDescriptor YamlParseError = new DiagnosticDescriptor(
            "HIDEDB001",
            "YAML Parse Error",
            "Error parsing YAML file '{0}': {1}",
            "HideDB",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor YamlNotFoundWarning = new DiagnosticDescriptor(
            "HIDEDB002",
            "No YAML files found",
            "No YAML entity definition files found in AdditionalFiles",
            "HideDB",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor GenerationInfo = new DiagnosticDescriptor(
            "HIDEDB003",
            "Code Generation",
            "Generated code for entity '{0}' from file '{1}'",
            "HideDB",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this generator
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var yamlFiles = context.AdditionalFiles
                .Where(file => file.Path.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (yamlFiles.Length == 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(YamlNotFoundWarning, Location.None));
                return;
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            foreach (var yamlFile in yamlFiles)
            {
                try
                {
                    var yamlContent = yamlFile.GetText(context.CancellationToken)?.ToString();
                    if (string.IsNullOrWhiteSpace(yamlContent))
                        continue;

                    var entityDef = deserializer.Deserialize<EntityDefinition>(yamlContent);
                    
                    if (entityDef?.Entity == null)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(YamlParseError, Location.None, 
                            yamlFile.Path, "Missing 'entity' property"));
                        continue;
                    }

                    // Generate Model
                    var modelCode = ModelGenerator.GenerateModel(entityDef);
                    context.AddSource($"{entityDef.Entity}Model.g.cs", 
                        SourceText.From(modelCode, Encoding.UTF8));

                    // Generate Repository Interface
                    var interfaceCode = RepositoryGenerator.GenerateInterface(entityDef);
                    context.AddSource($"I{entityDef.Entity}Repository.g.cs", 
                        SourceText.From(interfaceCode, Encoding.UTF8));

                    // Generate Repository Implementation
                    var repositoryCode = RepositoryGenerator.GenerateRepository(entityDef);
                    context.AddSource($"{entityDef.Entity}Repository.g.cs", 
                        SourceText.From(repositoryCode, Encoding.UTF8));

                    // Report successful generation
                    context.ReportDiagnostic(Diagnostic.Create(GenerationInfo, Location.None, 
                        entityDef.Entity, System.IO.Path.GetFileName(yamlFile.Path)));
                }
                catch (Exception ex)
                {
                    context.ReportDiagnostic(Diagnostic.Create(YamlParseError, Location.None, 
                        yamlFile.Path, ex.Message));
                }
            }
        }
    }
}