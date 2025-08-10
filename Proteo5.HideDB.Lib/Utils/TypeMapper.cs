using Proteo5.HideDB.Lib.Configuration;

namespace Proteo5.HideDB.Lib.Utils
{
    public static class TypeMapper
    {
        public static string MapYamlTypeToCSharp(string yamlType, bool nullable = true)
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

            // Handle nullable types
            if (nullable)
            {
                if (baseType == "string")
                {
                    return "string?"; // Nullable string in C# 8+
                }
                else if (baseType != "object")
                {
                    return $"{baseType}?"; // Nullable value types
                }
            }
            
            return baseType;
        }

        public static string MapYamlTypeToSql(string yamlType, int? maxLength, DatabaseProvider provider)
        {
            return provider switch
            {
                DatabaseProvider.SqlServer => MapToSqlServer(yamlType, maxLength),
                DatabaseProvider.PostgreSQL => MapToPostgreSQL(yamlType, maxLength),
                DatabaseProvider.MySQL => MapToMySQL(yamlType, maxLength),
                DatabaseProvider.Oracle => MapToOracle(yamlType, maxLength),
                DatabaseProvider.SQLite => MapToSQLite(yamlType, maxLength),
                _ => MapToSqlServer(yamlType, maxLength)
            };
        }

        private static string MapToSqlServer(string yamlType, int? maxLength)
        {
            return yamlType.ToLower() switch
            {
                "int" => "INT",
                "long" => "BIGINT",
                "string" => maxLength.HasValue ? $"NVARCHAR({maxLength})" : "NVARCHAR(MAX)",
                "datetime" => "DATETIME2",
                "bool" => "BIT",
                "decimal" => "DECIMAL(18,2)",
                "double" => "FLOAT",
                "float" => "REAL",
                "guid" => "UNIQUEIDENTIFIER",
                _ => "NVARCHAR(MAX)"
            };
        }

        private static string MapToPostgreSQL(string yamlType, int? maxLength)
        {
            return yamlType.ToLower() switch
            {
                "int" => "INTEGER",
                "long" => "BIGINT",
                "string" => maxLength.HasValue ? $"VARCHAR({maxLength})" : "TEXT",
                "datetime" => "TIMESTAMP",
                "bool" => "BOOLEAN",
                "decimal" => "NUMERIC(18,2)",
                "double" => "DOUBLE PRECISION",
                "float" => "REAL",
                "guid" => "UUID",
                _ => "TEXT"
            };
        }

        private static string MapToMySQL(string yamlType, int? maxLength)
        {
            return yamlType.ToLower() switch
            {
                "int" => "INT",
                "long" => "BIGINT",
                "string" => maxLength.HasValue ? $"VARCHAR({maxLength})" : "TEXT",
                "datetime" => "DATETIME",
                "bool" => "TINYINT(1)",
                "decimal" => "DECIMAL(18,2)",
                "double" => "DOUBLE",
                "float" => "FLOAT",
                "guid" => "CHAR(36)",
                _ => "TEXT"
            };
        }

        private static string MapToSQLite(string yamlType, int? maxLength)
        {
            return yamlType.ToLower() switch
            {
                "int" => "INTEGER",
                "long" => "INTEGER",
                "string" => "TEXT",
                "datetime" => "TEXT",
                "bool" => "INTEGER",
                "decimal" => "REAL",
                "double" => "REAL",
                "float" => "REAL",
                "guid" => "TEXT",
                _ => "TEXT"
            };
        }

        private static string MapToOracle(string yamlType, int? maxLength)
        {
            return yamlType.ToLower() switch
            {
                "int" => "NUMBER(10)",
                "long" => "NUMBER(19)",
                "string" => maxLength.HasValue ? $"VARCHAR2({maxLength})" : "CLOB",
                "datetime" => "TIMESTAMP",
                "bool" => "NUMBER(1)",
                "decimal" => "NUMBER(18,2)",
                "double" => "BINARY_DOUBLE",
                "float" => "BINARY_FLOAT",
                "guid" => "VARCHAR2(36)",
                _ => "CLOB"
            };
        }
    }
}