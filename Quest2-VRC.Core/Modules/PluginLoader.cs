using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Quest2_VRC
{
    public static class PluginLoader
    {
        public static List<IPlugin> LoadedPlugins { get; } = new();
        private static readonly string PluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        public static void LoadPlugins()
        {
            if (!Directory.Exists(PluginsPath))
                Directory.CreateDirectory(PluginsPath);

            foreach (var file in Directory.GetFiles(PluginsPath, "*.dll"))
            {
                try
                {
                    var asm = Assembly.LoadFrom(file);
                    foreach (var type in asm.GetTypes())
                    {
                        if (!typeof(IPlugin).IsAssignableFrom(type) || type.IsAbstract)
                            continue;
                        if (Activator.CreateInstance(type) is IPlugin plugin)
                        {
                            // Basic security: only allow plugins from Plugins folder, must implement IPlugin
                            LoadedPlugins.Add(plugin);
                            plugin.Init();
                            Console.WriteLine($"Loaded plugin: {plugin.Name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load plugin from {file}: {ex.Message}");
                }
            }
        }

        public static void StartAll()
        {
            Console.WriteLine($"Starting {LoadedPlugins.Count} plugins...");
            foreach (var plugin in LoadedPlugins)
            {
                try 
                { 
                    plugin.Start();
                    Console.WriteLine($"Started plugin: {plugin.Name}");
                } 
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting plugin {plugin.Name}: {ex.Message}");
                }
            }
        }

        public static void StopAll()
        {
            Console.WriteLine($"Stopping {LoadedPlugins.Count} plugins...");
            foreach (var plugin in LoadedPlugins)
            {
                try 
                { 
                    plugin.Stop();
                    Console.WriteLine($"Stopped plugin: {plugin.Name}");
                } 
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stopping plugin {plugin.Name}: {ex.Message}");
                }
            }
        }
    }
}
