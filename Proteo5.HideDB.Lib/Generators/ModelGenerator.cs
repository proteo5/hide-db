using System;
using System.Linq;
using System.Text;
using Proteo5.HideDB.Lib.Configuration;
using Proteo5.HideDB.Lib.Models;
using Proteo5.HideDB.Lib.Utils;

namespace Proteo5.HideDB.Lib.Generators
{
    public class ModelGenerator
    {
        public string GenerateModel(EntityDefinition entityDef, GeneratorConfig config)
        {
            var sb = new StringBuilder();
            
            // Usings
            sb.AppendLine("using System;");
            if (config.AddDataAnnotations)
            {
                sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            }
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine($"namespace {config.Namespace}.Models");
            sb.AppendLine("{");
            
            // XML Documentation
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Modelo para la entidad {entityDef.Entity}");
            if (!string.IsNullOrEmpty(entityDef.Description))
                sb.AppendLine($"    /// {entityDef.Description}");
            sb.AppendLine($"    /// Generado automáticamente - Version: {entityDef.Version}");
            sb.AppendLine($"    /// </summary>");
            
            // Data Annotations para tabla
            if (config.AddDataAnnotations)
            {
                sb.AppendLine($"    [Table(\"{entityDef.Entity}\")]");
            }
            
            // Clase
            sb.AppendLine($"    public class {entityDef.Entity}Model");
            sb.AppendLine("    {");
            
            // Propiedades
            foreach (var field in entityDef.Fields)
            {
                GenerateProperty(sb, field, config);
                sb.AppendLine();
            }
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        private void GenerateProperty(StringBuilder sb, FieldDefinition field, GeneratorConfig config)
        {
            // Data Annotations
            if (config.AddDataAnnotations)
            {
                if (field.PrimaryKey)
                {
                    sb.AppendLine("        [Key]");
                    if (field.AutoIncrement)
                        sb.AppendLine("        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                }
                
                if (field.Required)
                    sb.AppendLine("        [Required]");
                
                if (field.MaxLength.HasValue && field.Type.ToLower() == "string")
                    sb.AppendLine($"        [MaxLength({field.MaxLength.Value})]");
            }
            
            // XML Documentation
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// {field.Description ?? field.Name}");
            if (!string.IsNullOrEmpty(field.Catalog))
                sb.AppendLine($"        /// Catálogo: {field.Catalog}");
            sb.AppendLine("        /// </summary>");
            
            // Use the explicit nullable flag from the YAML
            var csharpType = TypeMapper.MapYamlTypeToCSharp(field.Type, field.Nullable);
            
            // Handle initialization for non-nullable reference types
            var initialization = "";
            if (field.Type.ToLower() == "string" && !field.Nullable)
            {
                initialization = " = string.Empty;";
            }
            
            sb.AppendLine($"        public {csharpType} {field.Name} {{ get; set; }}{initialization}");
        }
    }
}