using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proteo5.HideDB.Lib.Configuration;
using Proteo5.HideDB.Lib.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Proteo5.HideDB.Lib.Generators
{
    public class YamlDslGenerator : IDisposable
    {
        private readonly GeneratorConfig _config;
        private readonly ILogger<YamlDslGenerator>? _logger;
        private readonly IDeserializer _yamlDeserializer;
        private readonly ModelGenerator _modelGenerator;
        private readonly RepositoryGenerator _repositoryGenerator;
        private readonly SqlGenerator _sqlGenerator;
        private readonly EnumGenerator _enumGenerator;
        private FileSystemWatcher? _watcher;

        public YamlDslGenerator(GeneratorConfig config, ILogger<YamlDslGenerator>? logger = null)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
            
            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            _modelGenerator = new ModelGenerator();
            _repositoryGenerator = new RepositoryGenerator();
            _sqlGenerator = new SqlGenerator();
            _enumGenerator = new EnumGenerator();

            EnsureDirectoriesExist();
        }

        public void StartWatching()
        {
            _watcher = new FileSystemWatcher(_config.YamlPath)
            {
                Filter = "*.yaml",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            _watcher.Changed += OnYamlFileChanged;
            _watcher.Created += OnYamlFileChanged;
            _watcher.Deleted += OnYamlFileDeleted;
            _watcher.Renamed += OnYamlFileRenamed;

            LogInfo($"?? Observando archivos YAML en: {Path.GetFullPath(_config.YamlPath)}");
            
            ProcessExistingFiles();
        }

        private async void OnYamlFileChanged(object sender, FileSystemEventArgs e)
        {
            LogInfo($"?? YAML modificado: {Path.GetFileName(e.FullPath)}");
            await Task.Delay(500); // Evitar múltiples eventos
            await ProcessYamlFileAsync(e.FullPath);
        }

        private async void OnYamlFileDeleted(object sender, FileSystemEventArgs e)
        {
            LogInfo($"??? YAML eliminado: {Path.GetFileName(e.FullPath)}");
            await CleanupGeneratedFiles(e.Name ?? "");
        }

        private async void OnYamlFileRenamed(object sender, RenamedEventArgs e)
        {
            LogInfo($"?? YAML renombrado: {e.OldName} -> {e.Name}");
            await CleanupGeneratedFiles(e.OldName ?? "");
            await ProcessYamlFileAsync(e.FullPath);
        }

        public async Task ProcessYamlFileAsync(string yamlFilePath)
        {
            try
            {
                if (!File.Exists(yamlFilePath)) return;

                LogInfo($"?? Procesando: {Path.GetFileName(yamlFilePath)}");

                var yamlContent = await File.ReadAllTextAsync(yamlFilePath);
                var entityDef = _yamlDeserializer.Deserialize<EntityDefinition>(yamlContent);

                if (entityDef?.Entity == null)
                {
                    LogError($"? YAML inválido en {yamlFilePath}: falta 'entity'");
                    return;
                }

                await GenerateAllFilesAsync(entityDef);
                LogSuccess($"? Generación completa para {entityDef.Entity}");
            }
            catch (Exception ex)
            {
                LogError($"? Error procesando {yamlFilePath}: {ex.Message}");
            }
        }

        private async Task GenerateAllFilesAsync(EntityDefinition entityDef)
        {
            var entityName = entityDef.Entity;
            var entityFolderName = $"{entityName}E"; // Add "E" suffix for entity folder
            
            // 1. Generar modelo
            var modelCode = _modelGenerator.GenerateModel(entityDef, _config);
            await WriteFileAsync($"{entityFolderName}/{entityName}Model.cs", modelCode);
            
            // 2. Generar interfaz
            if (_config.GenerateInterfaces)
            {
                var interfaceCode = _repositoryGenerator.GenerateInterface(entityDef, _config);
                await WriteFileAsync($"{entityFolderName}/I{entityName}Repository.cs", interfaceCode);
            }
            
            // 3. Generar repositorio
            var repositoryCode = _repositoryGenerator.GenerateRepository(entityDef, _config);
            await WriteFileAsync($"{entityFolderName}/{entityName}Repository.cs", repositoryCode);
            
            // 4. Generar SQL
            var sqlCode = _sqlGenerator.GenerateCreateTableScript(entityDef, _config);
            await WriteSqlFileAsync($"{entityName}_CreateTable.sql", sqlCode);
            
            // 5. Generar enums
            if (entityDef.Catalogs?.Count > 0)
            {
                var enumsCode = _enumGenerator.GenerateEnums(entityDef, _config);
                await WriteFileAsync($"{entityFolderName}/{entityName}Enums.cs", enumsCode);
            }
        }

        private void ProcessExistingFiles()
        {
            try
            {
                var yamlFiles = Directory.GetFiles(_config.YamlPath, "*.yaml");
                LogInfo($"?? Procesando {yamlFiles.Length} archivos existentes...");
                
                foreach (var file in yamlFiles)
                {
                    ProcessYamlFileAsync(file).Wait();
                }
            }
            catch (Exception ex)
            {
                LogError($"? Error procesando archivos existentes: {ex.Message}");
            }
        }

        private async Task WriteFileAsync(string relativePath, string content)
        {
            var fullPath = Path.Combine(_config.OutputPath, relativePath);
            var directory = Path.GetDirectoryName(fullPath);
            if (directory != null)
                Directory.CreateDirectory(directory);
            
            await File.WriteAllTextAsync(fullPath, content, System.Text.Encoding.UTF8);
            LogInfo($"? Generado: {relativePath}");
        }

        private async Task WriteSqlFileAsync(string fileName, string content)
        {
            var fullPath = Path.Combine(_config.SqlOutputPath, fileName);
            Directory.CreateDirectory(_config.SqlOutputPath);
            
            await File.WriteAllTextAsync(fullPath, content, System.Text.Encoding.UTF8);
            LogInfo($"? SQL generado: {fileName}");
        }

        private Task CleanupGeneratedFiles(string yamlFileName)
        {
            if (string.IsNullOrEmpty(yamlFileName)) return Task.CompletedTask;
            
            var baseName = Path.GetFileNameWithoutExtension(yamlFileName);
            var entityFolderName = $"{baseName}E"; // Add "E" suffix for entity folder
            
            var filesToDelete = new[]
            {
                Path.Combine(_config.OutputPath, $"{entityFolderName}/{baseName}Model.cs"),
                Path.Combine(_config.OutputPath, $"{entityFolderName}/I{baseName}Repository.cs"),
                Path.Combine(_config.OutputPath, $"{entityFolderName}/{baseName}Repository.cs"),
                Path.Combine(_config.OutputPath, $"{entityFolderName}/{baseName}Enums.cs"),
                Path.Combine(_config.SqlOutputPath, $"{baseName}_CreateTable.sql")
            };

            foreach (var file in filesToDelete)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                    LogInfo($"??? Eliminado: {Path.GetFileName(file)}");
                }
            }

            // Also try to delete the entity folder if it's empty
            var entityFolderPath = Path.Combine(_config.OutputPath, entityFolderName);
            if (Directory.Exists(entityFolderPath) && !Directory.EnumerateFileSystemEntries(entityFolderPath).Any())
            {
                Directory.Delete(entityFolderPath);
                LogInfo($"?? Carpeta eliminada: {entityFolderName}");
            }
            
            return Task.CompletedTask;
        }

        private void EnsureDirectoriesExist()
        {
            Directory.CreateDirectory(_config.YamlPath);
            Directory.CreateDirectory(_config.OutputPath);
            Directory.CreateDirectory(_config.SqlOutputPath);
            // Entity-specific folders will be created dynamically in WriteFileAsync
        }

        private void LogInfo(string message)
        {
            if (_logger != null)
                _logger.LogInformation(message);
            else
                Console.WriteLine(message);
        }
        
        private void LogWarning(string message)
        {
            if (_logger != null)
                _logger.LogWarning(message);
            else
                Console.WriteLine(message);
        }
        
        private void LogError(string message)
        {
            if (_logger != null)
                _logger.LogError(message);
            else
                Console.WriteLine(message);
        }
        
        private void LogSuccess(string message)
        {
            if (_logger != null)
                _logger.LogInformation(message);
            else
                Console.WriteLine(message);
        }

        public void Stop()
        {
            _watcher?.Dispose();
            LogInfo("?? Generador DSL detenido");
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}