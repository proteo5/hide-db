using System;
using System.Linq;
using System.Text;
using Proteo5.HideDB.Lib.Configuration;
using Proteo5.HideDB.Lib.Models;
using Proteo5.HideDB.Lib.Utils;

namespace Proteo5.HideDB.Lib.Generators
{
    public class RepositoryGenerator
    {
        public string GenerateInterface(EntityDefinition entityDef, GeneratorConfig config)
        {
            var sb = new StringBuilder();
            var modelName = $"{entityDef.Entity}Model";
            
            // Usings
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine($"using {config.Namespace}.Models;");
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine($"namespace {config.Namespace}.Repositories");
            sb.AppendLine("{");
            
            // XML Documentation
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Interfaz del repositorio para {entityDef.Entity}");
            sb.AppendLine("    /// </summary>");
            
            // Interface
            sb.AppendLine($"    public interface I{entityDef.Entity}Repository");
            sb.AppendLine("    {");
            
            // Métodos
            foreach (var statement in entityDef.Statements)
            {
                var returnType = MapReturnType(statement.Return, modelName);
                var parameters = SqlParameterExtractor.ExtractParameters(statement.Sql);
                var parameterString = GenerateParameterString(parameters);

                // Método síncrono
                if (config.GenerateSync)
                {
                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// {statement.Description ?? statement.Name}");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine($"        {returnType} {statement.Name}({parameterString});");
                    sb.AppendLine();
                }

                // Método asíncrono
                if (config.GenerateAsync)
                {
                    var asyncReturnType = MapAsyncReturnType(returnType);
                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// {statement.Description ?? statement.Name} (Async)");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine($"        {asyncReturnType} {statement.Name}Async({parameterString});");
                    sb.AppendLine();
                }
            }
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        public string GenerateRepository(EntityDefinition entityDef, GeneratorConfig config)
        {
            var sb = new StringBuilder();
            var modelName = $"{entityDef.Entity}Model";
            
            // Usings
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using Microsoft.Data.SqlClient;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.Configuration;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine($"using {config.Namespace}.Models;");
            if (config.GenerateInterfaces)
                sb.AppendLine($"using {config.Namespace}.Repositories;");
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine($"namespace {config.Namespace}.Repositories");
            sb.AppendLine("{");
            
            // XML Documentation
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Repositorio para la entidad {entityDef.Entity}");
            sb.AppendLine($"    /// Generado automáticamente desde YAML - Version: {entityDef.Version}");
            sb.AppendLine("    /// </summary>");
            
            // Clase
            var implementsInterface = config.GenerateInterfaces ? $" : I{entityDef.Entity}Repository" : "";
            sb.AppendLine($"    public class {entityDef.Entity}Repository{implementsInterface}");
            sb.AppendLine("    {");
            
            // Campo connection string
            sb.AppendLine("        private readonly string _connectionString;");
            sb.AppendLine();
            
            // Constructores
            GenerateConstructors(sb, entityDef.Entity, config);
            
            // Métodos para cada statement
            foreach (var statement in entityDef.Statements)
            {
                GenerateRepositoryMethods(sb, statement, modelName, entityDef, config);
                sb.AppendLine();
            }
            
            // Método mapper
            GenerateMapperMethod(sb, entityDef, modelName);
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        private void GenerateConstructors(StringBuilder sb, string entityName, GeneratorConfig config)
        {
            // Constructor por defecto
            sb.AppendLine($"        public {entityName}Repository()");
            sb.AppendLine("        {");
            sb.AppendLine($"            _connectionString = ConfigurationManager.ConnectionStrings[\"{config.ConnectionStringName}\"]?.ConnectionString");
            sb.AppendLine($"                ?? throw new InvalidOperationException(\"Connection string '{config.ConnectionStringName}' not found\");");
            sb.AppendLine("        }");
            sb.AppendLine();
            
            // Constructor con connection string
            sb.AppendLine($"        public {entityName}Repository(string connectionString)");
            sb.AppendLine("        {");
            sb.AppendLine("            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        private void GenerateRepositoryMethods(StringBuilder sb, StatementDefinition statement, 
                                             string modelName, EntityDefinition entityDef, GeneratorConfig config)
        {
            var returnType = MapReturnType(statement.Return, modelName);
            var parameters = SqlParameterExtractor.ExtractParameters(statement.Sql);
            var parameterString = GenerateParameterString(parameters);

            // Método síncrono
            if (config.GenerateSync)
            {
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {statement.Description ?? statement.Name}");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public {returnType} {statement.Name}({parameterString})");
                sb.AppendLine("        {");
                GenerateMethodBody(sb, statement, returnType, parameters, false, modelName);
                sb.AppendLine("        }");
                sb.AppendLine();
            }

            // Método asíncrono
            if (config.GenerateAsync)
            {
                var asyncReturnType = MapAsyncReturnType(returnType);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {statement.Description ?? statement.Name} (Async)");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public async {asyncReturnType} {statement.Name}Async({parameterString})");
                sb.AppendLine("        {");
                GenerateMethodBody(sb, statement, returnType, parameters, true, modelName);
                sb.AppendLine("        }");
            }
        }

        private void GenerateMethodBody(StringBuilder sb, StatementDefinition statement, string returnType, 
                                      List<string> parameters, bool isAsync, string modelName)
        {
            var asyncKeyword = isAsync ? "await " : "";
            var asyncSuffix = isAsync ? "Async" : "";
            
            sb.AppendLine("            using (var connection = new SqlConnection(_connectionString))");
            sb.AppendLine("            using (var command = new SqlCommand())");
            sb.AppendLine("            {");
            sb.AppendLine($"                {asyncKeyword}connection.Open{asyncSuffix}();");
            sb.AppendLine("                command.Connection = connection;");
            sb.AppendLine($"                command.CommandText = @\"{statement.Sql.Replace("\"", "\"\"")}\";");
            
            // Parámetros
            foreach (var param in parameters)
            {
                sb.AppendLine($"                command.Parameters.AddWithValue(\"@{param}\", {param} ?? DBNull.Value);");
            }

            // Ejecución según tipo de retorno
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

        private void GenerateMapperMethod(StringBuilder sb, EntityDefinition entityDef, string modelName)
        {
            sb.AppendLine($"        private {modelName} MapToModel(IDataReader reader)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return new {modelName}");
            sb.AppendLine("            {");

            foreach (var field in entityDef.Fields)
            {
                if (field.Type.ToLower() == "string")
                {
                    if (field.Nullable)
                    {
                        sb.AppendLine($"                {field.Name} = reader[\"{field.Name}\"] == DBNull.Value ? null : reader[\"{field.Name}\"].ToString(),");
                    }
                    else
                    {
                        sb.AppendLine($"                {field.Name} = reader[\"{field.Name}\"] == DBNull.Value ? string.Empty : (reader[\"{field.Name}\"].ToString() ?? string.Empty),");
                    }
                }
                else if (IsValueType(field.Type))
                {
                    var csharpType = TypeMapper.MapYamlTypeToCSharp(field.Type, field.Nullable);
                    var baseType = GetBaseTypeName(csharpType);
                    if (field.Nullable)
                    {
                        sb.AppendLine($"                {field.Name} = reader[\"{field.Name}\"] == DBNull.Value ? null : ({baseType}?)reader[\"{field.Name}\"],");
                    }
                    else
                    {
                        sb.AppendLine($"                {field.Name} = reader[\"{field.Name}\"] == DBNull.Value ? default({baseType}) : ({baseType})reader[\"{field.Name}\"],");
                    }
                }
                else
                {
                    var csharpType = TypeMapper.MapYamlTypeToCSharp(field.Type, field.Nullable);
                    sb.AppendLine($"                {field.Name} = ({csharpType})reader[\"{field.Name}\"],");
                }
            }

            sb.AppendLine("            };");
            sb.AppendLine("        }");
        }
        
        private string GetBaseTypeName(string csharpType)
        {
            return csharpType.EndsWith("?") ? csharpType.Substring(0, csharpType.Length - 1) : csharpType;
        }
        
        private bool IsValueType(string yamlType)
        {
            return yamlType.ToLower() switch
            {
                "int" or "long" or "bool" or "decimal" or "double" or "float" or "datetime" or "guid" => true,
                _ => false
            };
        }

        // Métodos auxiliares
        private string MapReturnType(string returnType, string modelName)
        {
            return returnType?.ToLower() switch
            {
                "many" => $"List<{modelName}>",
                "one" => $"{modelName}?",
                "scalar" => "object?",
                "nothing" => "int",
                _ => "int"
            };
        }

        private string MapAsyncReturnType(string returnType)
        {
            if (returnType == "void") return "Task";
            return $"Task<{returnType}>";
        }

        private string GenerateParameterString(List<string> parameters)
        {
            return string.Join(", ", parameters.Select(p => $"object {p}"));
        }

        private string ExtractModelFromReturnType(string returnType)
        {
            if (returnType.StartsWith("List<") && returnType.EndsWith(">"))
            {
                return returnType.Substring(5, returnType.Length - 6);
            }
            return returnType;
        }
    }
}