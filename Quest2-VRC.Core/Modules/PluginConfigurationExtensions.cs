using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Quest2_VRC
{
    public static class PluginConfigurationExtensions
    {
        public static T LoadConfiguration<T>(this IPlugin plugin) where T : new()
        {
            var configPath = GetConfigPath(plugin);
            if (!File.Exists(configPath))
            {
                var defaultConfig = new T();
                SaveConfiguration(plugin, defaultConfig);
                Console.WriteLine($"[PluginConfig] Created default config for {plugin.Name} at {configPath}");
                return defaultConfig;
            }

            try
            {
                var json = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<T>(json) ?? new T();
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public static void SaveConfiguration<T>(this IPlugin plugin, T config)
        {
            var configPath = GetConfigPath(plugin);
            var directory = Path.GetDirectoryName(configPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

        private static string GetConfigPath(IPlugin plugin)
        {
            var pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            return Path.Combine(pluginsPath, $"{plugin.Name}.config.json");
        }
    }
}