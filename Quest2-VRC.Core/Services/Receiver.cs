using Bespoke.Osc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Timers;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Quest2_VRC.Vars;
using Extensions = VRC.OSCQuery.Extensions;
using VRC.OSCQuery;

namespace Quest2_VRC
{
    public class Receiver
    {
        private static OscServer? oscServer;
        public static event Action<string, object> PluginCommandReceived;

        public static async void Run()
        {
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            var tcpPort = Extensions.GetAvailableTcpPort();
            int udpPort;
            if (Global.UseCustomPort==false)
            {
                udpPort = Extensions.GetAvailableUdpPort();
                var oscQuery = new OSCQueryServiceBuilder()
                    .WithTcpPort(tcpPort)
                    .WithUdpPort(udpPort)
                    .WithServiceName("Quest2-VRC OSCQuery Receiver")
                    .WithDefaults()
                    .Build();
            }
            else
            {
                udpPort = (int)Global.ReceivePort;
            }

            var IP = IPAddress.Parse((string)Global.HostIP);

            oscServer = new OscServer((Bespoke.Common.Net.TransportType)TransportType.Udp, IP, udpPort);
            oscServer.FilterRegisteredMethods = false;
            
            oscServer.MessageReceived += OscServer_MessageReceived;
            oscServer.Start();
            Logger.LogToConsole($"Receiver started on port: {udpPort}");
            await Task.Delay(3000);
        }

        public static void RegisterAddress(string address)
        {
            oscServer?.RegisterMethod(address);
        }

        private static void OscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            OscMessage message = e.Message;
            if (message.Data.Count > 0)
            {
                PluginCommandReceived?.Invoke(message.Address, message.Data[0]);
            }
            else
            {
                Console.WriteLine($"Received message without data: {message.Address}");
            }
        }
    }
}





