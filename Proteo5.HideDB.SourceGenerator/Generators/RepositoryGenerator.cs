using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proteo5.HideDB.SourceGenerator.Models;

namespace Proteo5.HideDB.SourceGenerator.Generators
{
    public static class RepositoryGenerator
    {
        public static string GenerateInterface(EntityDefinition entityDef)
        {
            var sb = new StringBuilder();
            var modelName = $"{entityDef.Entity}Model";
            
            // Usings
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Proteo5.HideDB.Generated.Models;");
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine("namespace Proteo5.HideDB.Generated.Repositories");
            sb.AppendLine("{");
            
            // XML Documentation
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Repository interface for {entityDef.Entity}");
            sb.AppendLine("    /// </summary>");
            
            // Interface
            sb.AppendLine($"    public interface I{entityDef.Entity}Repository");
            sb.AppendLine("    {");
            
            // Methods
            foreach (var statement in entityDef.Statements)
            {
                var returnType = MapReturnType(statement.Return, modelName);
                var parameters = ExtractParameters(statement.Sql);
                var parameterString = GenerateParameterString(parameters);

                // Async method
                var asyncReturnType = MapAsyncReturnType(returnType);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {statement.Description ?? statement.Name} (Async)");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        {asyncReturnType} {statement.Name}Async({parameterString});");
                sb.AppendLine();
            }
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        public static string GenerateRepository(EntityDefinition entityDef)
        {
            var sb = new StringBuilder();
            var modelName = $"{entityDef.Entity}Model";
            
            // Usings
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using Microsoft.Data.SqlClient;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Proteo5.HideDB.Generated.Models;");
            sb.AppendLine("using Proteo5.HideDB.Generated.Repositories;");
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine("namespace Proteo5.HideDB.Generated.Repositories");
            sb.AppendLine("{");
            
            // XML Documentation
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Repository for {entityDef.Entity} entity");
            sb.AppendLine($"    /// Generated automatically - Version: {entityDef.Version}");
            sb.AppendLine("    /// </summary>");
            
            // Class
            sb.AppendLine($"    public class {entityDef.Entity}Repository : I{entityDef.Entity}Repository");
            sb.AppendLine("    {");
            
            // Constructor
            sb.AppendLine("        private readonly string _connectionString;");
            sb.AppendLine();
            sb.AppendLine($"        public {entityDef.Entity}Repository(string connectionString)");
            sb.AppendLine("        {");
            sb.AppendLine("            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));");
            sb.AppendLine("        }");
            sb.AppendLine();
            
            // Methods
            foreach (var statement in entityDef.Statements)
            {
                var returnType = MapReturnType(statement.Return, modelName);
                var parameters = ExtractParameters(statement.Sql);
                var parameterString = GenerateParameterString(parameters);

                // Async method
                var asyncReturnType = MapAsyncReturnType(returnType);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {statement.Description ?? statement.Name} (Async)");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public async {asyncReturnType} {statement.Name}Async({parameterString})");
                sb.AppendLine("        {");
                GenerateMethodBody(sb, statement, returnType, parameters, true, modelName);
                sb.AppendLine("        }");
                sb.AppendLine();
            }
            
            // Mapper method
            GenerateMapperMethod(sb, entityDef, modelName);
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        private static string MapReturnType(string returnType, string modelName)
        {
            return returnType?.ToLower() switch
            {
                "many" => $"List<{modelName}>",
                "one" => $"{modelName}?",
                "scalar" => "object?",
                "nothing" or _ => "int"
            };
        }

        private static string MapAsyncReturnType(string returnType)
        {
            return $"Task<{returnType}>";
        }

        private static string[] ExtractParameters(string sql)
        {
            var parameters = new List<string>();
            var lines = sql.Split(new char[] { '\n' }, StringSplitOptions.None);
            
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                var words = trimmed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var word in words)
                {
                    if (word.StartsWith("@") && word.Length > 1)
                    {
                        var param = word.TrimEnd(',', ';', ')', '(', '\'', '"');
                        if (param.StartsWith("@"))
                        {
                            var paramName = param.Substring(1);
                            if (!parameters.Contains(paramName))
                            {
                                parameters.Add(paramName);
                            }
                        }
                    }
                }
            }
            
            return parameters.ToArray();
        }

        private static string GenerateParameterString(string[] parameters)
        {
            if (parameters.Length == 0) return string.Empty;
            
            return string.Join(", ", parameters.Select(p => $"object {p}"));
        }

        private static void GenerateMethodBody(StringBuilder sb, StatementDefinition statement, string returnType, 
                                             string[] parameters, bool isAsync, string modelName)
        {
            var asyncKeyword = isAsync ? "await " : "";
            var asyncSuffix = isAsync ? "Async" : "";
            
            sb.AppendLine("            using (var connection = new SqlConnection(_connectionString))");
            sb.AppendLine("            using (var command = new SqlCommand())");
            sb.AppendLine("            {");
            sb.AppendLine($"                {asyncKeyword}connection.Open{asyncSuffix}();");
            sb.AppendLine("                command.Connection = connection;");
            sb.AppendLine($"                command.CommandText = @\"{statement.Sql.Replace("\"", "\"\"")}\";");
            
            // Parameters
            foreach (var param in parameters)
            {
                sb.AppendLine($"                command.Parameters.AddWithValue(\"@{param}\", {param} ?? DBNull.Value);");
            }

            // Execution based on return type
            switch (statement.Return?.ToLower())
            {
                case "many":
                    sb.AppendLine($"                var result = new List<{ExtractModelFromReturnType(returnType)}>();");
                    sb.AppendLine($"                using (var reader = {asyncKeyword}command.ExecuteReader{asyncSuffix}())");
                    sb.AppendLine("                {");
                    sb.AppendLine($"                    while ({asyncKeyword}reader.Read{asyncSuffix}())");
                    sb.AppendLine("                    {");
                    sb.AppendLine("                        result.Add(MapToModel(reader));");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    sb.AppendLine("                return result;");
                    break;
                
                case "one":
                    sb.AppendLine($"                using (var reader = {asyncKeyword}command.ExecuteReader{asyncSuffix}())");
                    sb.AppendLine("                {");
                    sb.AppendLine($"                    if ({asyncKeyword}reader.Read{asyncSuffix}())");
                    sb.AppendLine("                    {");
                    sb.AppendLine("                        return MapToModel(reader);");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    sb.AppendLine("                return null;");
                    break;
                
                case "scalar":
                    sb.AppendLine($"                var result = {asyncKeyword}command.ExecuteScalar{asyncSuffix}();");
                    sb.AppendLine("                return result == DBNull.Value ? null : result;");
                    break;
                
                case "nothing":
                default:
                    sb.AppendLine($"                return {asyncKeyword}command.ExecuteNonQuery{asyncSuffix}();");
                    break;
            }

            sb.AppendLine("            }");
        }

        private static string ExtractModelFromReturnType(string returnType)
        {
            if (returnType.StartsWith("List<") && returnType.EndsWith(">"))
            {
                return returnType.Substring(5, returnType.Length - 6);
            }
            return returnType.TrimEnd('?');
        }

        private static void GenerateMapperMethod(StringBuilder sb, EntityDefinition entityDef, string modelName)
        {
            sb.AppendLine($"        private {modelName} MapToModel(IDataReader reader)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return new {modelName}");
            sb.AppendLine("            {");

            foreach (var field in entityDef.Fields)
            {
                var conversion = GetReaderConversion(field);
                sb.AppendLine($"                {field.Name} = {conversion},");
            }

            sb.AppendLine("            };");
            sb.AppendLine("        }");
        }

        private static string GetReaderConversion(FieldDefinition field)
        {
            var fieldName = field.Name;
            var baseConversion = field.Type.ToLower() switch
            {
                "int" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(int) : (int)reader[\"{fieldName}\"]",
                "long" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(long) : (long)reader[\"{fieldName}\"]",
                "string" => $"reader[\"{fieldName}\"] == DBNull.Value ? string.Empty : (reader[\"{fieldName}\"].ToString() ?? string.Empty)",
                "datetime" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(DateTime) : (DateTime)reader[\"{fieldName}\"]",
                "bool" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(bool) : (bool)reader[\"{fieldName}\"]",
                "decimal" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(decimal) : (decimal)reader[\"{fieldName}\"]",
                "double" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(double) : (double)reader[\"{fieldName}\"]",
                "float" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(float) : (float)reader[\"{fieldName}\"]",
                "guid" => $"reader[\"{fieldName}\"] == DBNull.Value ? default(Guid) : (Guid)reader[\"{fieldName}\"]",
                _ => $"reader[\"{fieldName}\"]"
            };

            // Handle nullable types
            if (field.Nullable && field.Type.ToLower() == "string")
            {
                return $"reader[\"{fieldName}\"] == DBNull.Value ? null : reader[\"{fieldName}\"].ToString()";
            }

            return baseConversion;
        }
    }
}