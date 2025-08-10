using System.Linq;
using System.Text;
using Proteo5.HideDB.Lib.Configuration;
using Proteo5.HideDB.Lib.Models;

namespace Proteo5.HideDB.Lib.Generators
{
    public class EnumGenerator
    {
        public string GenerateEnums(EntityDefinition entityDef, GeneratorConfig config)
        {
            var sb = new StringBuilder();
            
            // Usings
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine();
            
            // Namespace - Updated for entity-first structure
            sb.AppendLine($"namespace {config.Namespace}.{entityDef.Entity}E");
            sb.AppendLine("{");

            foreach (var catalog in entityDef.Catalogs)
            {
                var enumName = $"{entityDef.Entity}{FirstCharToUpper(catalog.Key)}";
                
                sb.AppendLine("    /// <summary>");
                sb.AppendLine($"    /// Enum para el catálogo {catalog.Key} de {entityDef.Entity}");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine($"    public enum {enumName}");
                sb.AppendLine("    {");

                for (int i = 0; i < catalog.Value.Count; i++)
                {
                    var item = catalog.Value[i];
                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// {item.Description ?? item.Name}");
                    sb.AppendLine("        /// </summary>");
                    
                    if (!string.IsNullOrEmpty(item.Description))
                        sb.AppendLine($"        [Description(\"{item.Description}\")]");
                    
                    sb.AppendLine($"        {FirstCharToUpper(item.Name)} = {i}");
                    
                    if (i < catalog.Value.Count - 1)
                        sb.AppendLine(",");
                    sb.AppendLine();
                }

                sb.AppendLine("    }");
                sb.AppendLine();
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}