using System;
using System.Text;
using Proteo5.HideDB.SourceGenerator.Models;

namespace Proteo5.HideDB.SourceGenerator.Generators
{
    public static class ModelGenerator
    {
        public static string GenerateModel(EntityDefinition entityDef)
        {
            var sb = new StringBuilder();
            
            // Usings
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine("namespace Proteo5.HideDB.Generated.Models");
            sb.AppendLine("{");
            
            // XML Documentation
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Model for {entityDef.Entity} entity");
            if (!string.IsNullOrEmpty(entityDef.Description))
                sb.AppendLine($"    /// {entityDef.Description}");
            sb.AppendLine($"    /// Generated automatically - Version: {entityDef.Version}");
            sb.AppendLine("    /// </summary>");
            
            // Table attribute
            sb.AppendLine($"    [Table(\"{entityDef.Entity}\")]");
            
            // Class
            sb.AppendLine($"    public class {entityDef.Entity}Model");
            sb.AppendLine("    {");
            
            // Properties
            foreach (var field in entityDef.Fields)
            {
                GenerateProperty(sb, field);
                sb.AppendLine();
            }
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        private static void GenerateProperty(StringBuilder sb, FieldDefinition field)
        {
            // Data Annotations
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
            
            // XML Documentation
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// {field.Description ?? field.Name}");
            if (!string.IsNullOrEmpty(field.Catalog))
                sb.AppendLine($"        /// Catalog: {field.Catalog}");
            sb.AppendLine("        /// </summary>");
            
            // Property
            var csharpType = MapYamlTypeToCSharp(field.Type, field.Nullable);
            var defaultValue = GetDefaultValue(field);
            
            sb.AppendLine($"        public {csharpType} {field.Name} {{ get; set; }}{defaultValue}");
        }

        private static string MapYamlTypeToCSharp(string yamlType, bool nullable = true)
        {
            var baseType = yamlType.ToLower() switch
            {
                "int" => "int",
                "long" => "long", 
                "string" => "string",
                "datetime" => "DateTime",
                "bool" => "bool",
                "decimal" => "decimal",
                "double" => "double",
                "float" => "float",
                "guid" => "Guid",
                _ => "object"
            };

            if (nullable)
            {
                if (baseType == "string")
                {
                    return "string?";
                }
                else if (baseType != "object")
                {
                    return $"{baseType}?";
                }
            }
            
            return baseType;
        }

        private static string GetDefaultValue(FieldDefinition field)
        {
            if (field.Type.ToLower() == "string" && !field.Nullable)
            {
                return " = string.Empty;";
            }
            return string.Empty;
        }
    }
}