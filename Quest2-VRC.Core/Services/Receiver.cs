using Bespoke.Osc;
using Newtonsoft.Json.Linq;
using Quest2_VRC.Services;
using System;
using System.Collections.Concurrent;
using System.Timers;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VRC.OSCQuery;
using Extensions = VRC.OSCQuery.Extensions;

namespace Quest2_VRC

{

    public class Receiver

    {

        private static readonly string R = "/avatar/parameters/R";
        private static readonly string G = "/avatar/parameters/G";
        private static readonly string B = "/avatar/parameters/B";
        private static readonly ConcurrentDictionary<string, int> rgbBuffer = new();
        private static readonly string[] rgbAddresses = { "/avatar/parameters/R", "/avatar/parameters/G", "/avatar/parameters/B" };
        private static readonly Timer processTimer = new(230); 
        public static async void Run()
        {       
            processTimer.Elapsed += ProcessBufferedData;
            processTimer.AutoReset = true; 
            processTimer.Start();

            RGBController.SendRGBRawData(255,255,255);  // Init OpenRGB
            await Task.Delay(20);          
            RGBController.SendRGBRawData(0, 0, 0);      // Set to Black
            var tcpPort = Extensions.GetAvailableTcpPort();
            var udpPort = Extensions.GetAvailableUdpPort();


            var oscQuery = new OSCQueryServiceBuilder()
                .WithTcpPort(tcpPort)
                .WithUdpPort(udpPort)
                .WithServiceName("Quest2-VRC OSCQuery Receiver")
                .WithDefaults()
                .Build();

            oscQuery.AddEndpoint<int>("/avatar", Attributes.AccessValues.WriteOnly);

            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);


            var IP = IPAddress.Parse((string)vars["HostIP"]);

            OscServer oscServer;
            oscServer = new OscServer((Bespoke.Common.Net.TransportType)TransportType.Udp, IP, udpPort);
            oscServer.FilterRegisteredMethods = true;
            oscServer.RegisterMethod(R);
            oscServer.RegisterMethod(G);
            oscServer.RegisterMethod(B);
            oscServer.MessageReceived += new EventHandler<OscMessageReceivedEventArgs>(oscServer_MessageReceived);
            oscServer.Start();
            Logger.LogToConsole("Make sure you have all effects disabled in OpenRGB");
            await Task.Delay(3000);
        }

        private static void ProcessBufferedData(object sender, ElapsedEventArgs e)
        {
            if (rgbBuffer.Count == 0) return;

            
            int r = rgbBuffer.ContainsKey("/avatar/parameters/R") ? rgbBuffer["/avatar/parameters/R"] : 0;
            int g = rgbBuffer.ContainsKey("/avatar/parameters/G") ? rgbBuffer["/avatar/parameters/G"] : 0;
            int b = rgbBuffer.ContainsKey("/avatar/parameters/B") ? rgbBuffer["/avatar/parameters/B"] : 0;

            Console.WriteLine($"Processing RGB: R={r}, G={g}, B={b}");

            
            ProcessRGB(r, g, b);

            
            rgbBuffer.Clear();
        }

        private static void oscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            OscMessage message = e.Message;

            if (rgbAddresses.Contains(message.Address) && message.Data[0] is int intValue)
            {
                
                rgbBuffer[message.Address] = intValue;

                Console.WriteLine($"Received {message.Address}: {intValue}");
            }
            else
            {
                Console.WriteLine($"Unhandled or invalid message: {message.Address}");
            }
        }

        private static void ProcessRGB(int r, int g, int b)
        {
            
            Console.WriteLine($"Received RGB: R={r}, G={g}, B={b}");
            RGBController.SendRGBRawData(r, g, b);
            
        }
    }

}



