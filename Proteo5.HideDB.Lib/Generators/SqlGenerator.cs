using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proteo5.HideDB.Lib.Configuration;
using Proteo5.HideDB.Lib.Models;
using Proteo5.HideDB.Lib.Utils;

namespace Proteo5.HideDB.Lib.Generators
{
    public class SqlGenerator
    {
        public string GenerateCreateTableScript(EntityDefinition entityDef, GeneratorConfig config)
        {
            var sb = new StringBuilder();
            
            // Header
            sb.AppendLine($"-- Tabla generada automáticamente para {entityDef.Entity}");
            sb.AppendLine($"-- Version: {entityDef.Version} | Entity Version: {entityDef.EntityVersion}");
            sb.AppendLine($"-- Generado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
            
            // Check si tabla existe
            sb.AppendLine(GenerateTableExistsCheck(entityDef.Entity, config.Provider));
            sb.AppendLine("BEGIN");
            sb.AppendLine($"    CREATE TABLE {GetTableName(entityDef.Entity, config.Provider)} (");

            // Campos
            var fieldLines = new List<string>();
            string? primaryKeyField = null;

            foreach (var field in entityDef.Fields)
            {
                var sqlType = TypeMapper.MapYamlTypeToSql(field.Type, field.MaxLength, config.Provider);
                var nullable = GetNullableClause(field, config.Provider);
                var identity = GetIdentityClause(field, config.Provider);
                var defaultValue = GetDefaultValueClause(field, config.Provider);

                if (field.PrimaryKey)
                {
                    primaryKeyField = field.Name;
                }

                var fieldLine = $"        {GetColumnName(field.Name, config.Provider)} {sqlType} {identity} {nullable} {defaultValue}".Trim();
                fieldLines.Add(fieldLine);
            }

            sb.AppendLine(string.Join(",\n", fieldLines));

            // Primary key constraint
            if (!string.IsNullOrEmpty(primaryKeyField))
            {
                sb.AppendLine(",");
                sb.AppendLine(GeneratePrimaryKeyConstraint(entityDef.Entity, primaryKeyField, config.Provider));
            }

            sb.AppendLine("    );");
            sb.AppendLine("END");
            sb.AppendLine();

            // Comentarios para catálogos
            if (entityDef.Catalogs?.Any() == true)
            {
                sb.AppendLine("-- Catálogos disponibles:");
                foreach (var catalog in entityDef.Catalogs)
                {
                    sb.AppendLine($"-- {catalog.Key}: {string.Join(", ", catalog.Value.Select(c => c.Name))}");
                }
            }

            return sb.ToString();
        }

        private string GenerateTableExistsCheck(string tableName, DatabaseProvider provider)
        {
            return provider switch
            {
                DatabaseProvider.SqlServer => $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' AND xtype='U')",
                DatabaseProvider.PostgreSQL => $"DO $$ BEGIN IF NOT EXISTS (SELECT FROM pg_tables WHERE tablename = '{tableName.ToLower()}') THEN",
                DatabaseProvider.MySQL => $"CREATE TABLE IF NOT EXISTS `{tableName}` (",
                DatabaseProvider.Oracle => $"BEGIN EXECUTE IMMEDIATE 'CREATE TABLE \"{tableName}\" (",
                DatabaseProvider.SQLite => $"CREATE TABLE IF NOT EXISTS [{tableName}] (",
                _ => $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' AND xtype='U')"
            };
        }

        private string GetTableName(string tableName, DatabaseProvider provider)
        {
            return provider switch
            {
                DatabaseProvider.SqlServer => $"[{tableName}]",
                DatabaseProvider.PostgreSQL => $"\"{tableName.ToLower()}\"",
                DatabaseProvider.MySQL => $"`{tableName}`",
                DatabaseProvider.Oracle => $"\"{tableName.ToUpper()}\"",
                DatabaseProvider.SQLite => $"[{tableName}]",
                _ => $"[{tableName}]"
            };
        }

        private string GetColumnName(string columnName, DatabaseProvider provider)
        {
            return provider switch
            {
                DatabaseProvider.SqlServer => $"[{columnName}]",
                DatabaseProvider.PostgreSQL => $"\"{columnName.ToLower()}\"",
                DatabaseProvider.MySQL => $"`{columnName}`",
                DatabaseProvider.Oracle => $"\"{columnName.ToUpper()}\"",
                DatabaseProvider.SQLite => $"[{columnName}]",
                _ => $"[{columnName}]"
            };
        }

        private string GetNullableClause(FieldDefinition field, DatabaseProvider provider)
        {
            var isNullable = field.Nullable && !field.PrimaryKey && !field.Required;
            return isNullable ? "NULL" : "NOT NULL";
        }

        private string GetIdentityClause(FieldDefinition field, DatabaseProvider provider)
        {
            if (!field.AutoIncrement) return "";
            
            return provider switch
            {
                DatabaseProvider.SqlServer => "IDENTITY(1,1)",
                DatabaseProvider.PostgreSQL => "", // Se maneja en el tipo (SERIAL)
                DatabaseProvider.MySQL => "AUTO_INCREMENT",
                DatabaseProvider.Oracle => "", // Se maneja con secuencias
                DatabaseProvider.SQLite => "AUTOINCREMENT",
                _ => "IDENTITY(1,1)"
            };
        }

        private string GetDefaultValueClause(FieldDefinition field, DatabaseProvider provider)
        {
            if (string.IsNullOrEmpty(field.DefaultValue) || field.AutoIncrement) return "";
            
            if (field.DefaultValue == "CURRENT_TIMESTAMP_UTC")
            {
                return provider switch
                {
                    DatabaseProvider.SqlServer => "DEFAULT GETUTCDATE()",
                    DatabaseProvider.PostgreSQL => "DEFAULT NOW()",
                    DatabaseProvider.MySQL => "DEFAULT UTC_TIMESTAMP()",
                    DatabaseProvider.Oracle => "DEFAULT SYS_EXTRACT_UTC(SYSTIMESTAMP)",
                    DatabaseProvider.SQLite => "DEFAULT (datetime('now'))",
                    _ => "DEFAULT GETUTCDATE()"
                };
            }
            
            return $"DEFAULT '{field.DefaultValue}'";
        }

        private string GeneratePrimaryKeyConstraint(string tableName, string keyField, DatabaseProvider provider)
        {
            return provider switch
            {
                DatabaseProvider.SqlServer => $"        CONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED ([{keyField}])",
                DatabaseProvider.PostgreSQL => $"        CONSTRAINT pk_{tableName.ToLower()} PRIMARY KEY (\"{keyField.ToLower()}\")",
                DatabaseProvider.MySQL => $"        PRIMARY KEY (`{keyField}`)",
                DatabaseProvider.Oracle => $"        CONSTRAINT PK_{tableName.ToUpper()} PRIMARY KEY (\"{keyField.ToUpper()}\")",
                DatabaseProvider.SQLite => $"        PRIMARY KEY ([{keyField}])",
                _ => $"        CONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED ([{keyField}])"
            };
        }
    }
}