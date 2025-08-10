using System.Collections.Generic;

namespace Proteo5.HideDB.Lib.Models
{
    public class EntityDefinition
    {
        public string Entity { get; set; } = string.Empty;
        public string EntityVersion { get; set; } = "1";
        public string Version { get; set; } = "1.0";
        public string Description { get; set; } = string.Empty;
        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
        public Dictionary<string, List<CatalogItem>> Catalogs { get; set; } = new Dictionary<string, List<CatalogItem>>();
        public List<StatementDefinition> Statements { get; set; } = new List<StatementDefinition>();
    }

    public class FieldDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "string";
        public bool PrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }
        public bool Required { get; set; }
        public bool Nullable { get; set; } = true;
        public int? MaxLength { get; set; }
        public string DefaultValue { get; set; } = string.Empty;
        public string Catalog { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class StatementDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Select";
        public string Return { get; set; } = "nothing";
        public string Description { get; set; } = string.Empty;
        public string Sql { get; set; } = string.Empty;
    }

    public class CatalogItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}