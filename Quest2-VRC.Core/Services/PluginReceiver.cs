using Bespoke.Osc;
using Newtonsoft.Json.Linq;
using Quest2_VRC.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Quest2_VRC.Vars;
using Extensions = VRC.OSCQuery.Extensions;
using VRC.OSCQuery;

namespace Quest2_VRC
{
    public static class PluginReceiver
    {
        private static OscServer? pluginOscServer;
        private static readonly HashSet<string> pluginAddresses = new();
        private static readonly object locker = new();
        private static int pluginUdpPort;
        private static OSCQueryService? oscQueryService;

        public static event Action<string, object>? PluginCommandReceived;

        public static void RegisterPluginAddress(string address)
        {
            lock (locker)
            {
                if (pluginAddresses.Add(address) && pluginOscServer != null)
                {
                    pluginOscServer.RegisterMethod(address);
                    Console.WriteLine($"[PluginReceiver] Registered address: {address}");
                }
                else if (pluginOscServer == null)
                {
                    Console.WriteLine($"[PluginReceiver] Queued address until server starts: {address}");
                }
            }
        }

        public static async void Run()
        {
            try
            {
                string json = File.ReadAllText("vars.json");

                int tcpPort = Extensions.GetAvailableTcpPort();
                if (Global.UseCustomPort != true)
                {
                    pluginUdpPort = Extensions.GetAvailableUdpPort();
                    oscQueryService = new OSCQueryServiceBuilder()
                        .WithTcpPort(tcpPort)
                        .WithUdpPort(pluginUdpPort)
                        .WithServiceName("Quest2-VRC Plugin OSCQuery")
                        .WithDefaults()
                        .Build();
                }
                else
                {
                    // Use main receive port + 1 for plugin channel
                    pluginUdpPort = ((int)Global.ReceivePort + 1);
                }

                var ip = IPAddress.Parse((string)Global.HostIP);
                pluginOscServer = new OscServer((Bespoke.Common.Net.TransportType)TransportType.Udp, ip, pluginUdpPort)
                {
                    FilterRegisteredMethods = true
                };

                pluginOscServer.MessageReceived += PluginOscServer_MessageReceived;
                pluginOscServer.Start();

                // Register any queued plugin addresses
                lock (locker)
                {
                    foreach (var addr in pluginAddresses)
                    {
                        pluginOscServer.RegisterMethod(addr);
                        Console.WriteLine($"[PluginReceiver] Registered queued address: {addr}");
                    }
                }

                Console.WriteLine($"[PluginReceiver] Started on UDP {pluginUdpPort}");
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PluginReceiver] Failed to start: {ex.Message}");
            }
        }

        private static void PluginOscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            var message = e.Message;
            bool isPluginAddress;
            lock (locker)
            {
                isPluginAddress = pluginAddresses.Contains(message.Address);
            }
            if (!isPluginAddress) return;

            if (message.Data.Count == 0) return;
            var val = message.Data[0];

            Console.WriteLine($"[PluginReceiver] Received: {message.Address} = {val}");
            PluginCommandReceived?.Invoke(message.Address, val);
        }
    }
}