using System;

namespace Proteo5.HideDB.SourceGenerator.Models
{
    public class EntityDefinition
    {
        public string Entity { get; set; } = string.Empty;
        public string EntityVersion { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public FieldDefinition[] Fields { get; set; } = Array.Empty<FieldDefinition>();
        public CatalogDefinition Catalogs { get; set; } = new();
        public StatementDefinition[] Statements { get; set; } = Array.Empty<StatementDefinition>();
    }

    public class FieldDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool PrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }
        public bool Required { get; set; }
        public bool Nullable { get; set; }
        public int? MaxLength { get; set; }
        public string DefaultValue { get; set; } = string.Empty;
        public string Catalog { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class StatementDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Return { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Sql { get; set; } = string.Empty;
    }

    public class CatalogDefinition
    {
        public CatalogItem[] Statuses { get; set; } = Array.Empty<CatalogItem>();
    }

    public class CatalogItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}