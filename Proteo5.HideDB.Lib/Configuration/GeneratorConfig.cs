namespace Proteo5.HideDB.Lib.Configuration
{
    public class GeneratorConfig
    {
        public string YamlPath { get; set; } = "./Entities";
        public string OutputPath { get; set; } = "./GeneratedCode";
        public string SqlOutputPath { get; set; } = "./GeneratedSQL";
        public string Namespace { get; set; } = "Proteo5.HideDB.Generated";
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.SqlServer;
        public bool GenerateInterfaces { get; set; } = true;
        public bool GenerateAsync { get; set; } = true;
        public bool GenerateSync { get; set; } = true;
        public bool AddDataAnnotations { get; set; } = true;
        public bool ValidateGenerated { get; set; } = true;
        public string ConnectionStringName { get; set; } = "DefaultConnection";
        public string FileEncoding { get; set; } = "UTF-8";
    }

    public enum DatabaseProvider
    {
        SqlServer,
        PostgreSQL,
        MySQL,
        Oracle,
        SQLite
    }
}