using Bespoke.Osc;
using Newtonsoft.Json.Linq;
using Quest2_VRC.Services;
using System;
using System.Collections.Concurrent;
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
        static readonly int dataInt = 0;
        static bool Raw_RGB = false; 
        static readonly int udpPort = 9001;
        private static readonly string R = "/avatar/parameters/R";
        private static readonly string G = "/avatar/parameters/G";
        private static readonly string B = "/avatar/parameters/B";
        private static readonly ConcurrentDictionary<string, int> rgbBuffer = new();
        private static readonly string[] rgbAddresses = { "/avatar/parameters/R", "/avatar/parameters/G", "/avatar/parameters/B" };
        public static async void Run()
        {
            RGBController.SendRGBRawData(0,195,255); // Init OpenRGB
            //var tcpPort = Extensions.GetAvailableTcpPort();
            //var udpPort = Extensions.GetAvailableUdpPort();
            
            
            //var oscQuery = new OSCQueryServiceBuilder()
            //    .WithTcpPort(tcpPort)
            //    .WithUdpPort(udpPort)
            //    .WithServiceName("Quest2-VRC OSCQuery Receiver")
            //    .WithDefaults()
            //    .Build();

            //oscQuery.AddEndpoint<int>("/avatar", Attributes.AccessValues.WriteOnly);

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

        private static void oscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            OscMessage message = e.Message;

            if (rgbAddresses.Contains(message.Address) && message.Data[0] is int intValue)
            {
                
                rgbBuffer[message.Address] = intValue;

                
                if (rgbBuffer.Count == rgbAddresses.Length)
                {
                    
                    int r = rgbBuffer["/avatar/parameters/R"];
                    int g = rgbBuffer["/avatar/parameters/G"];
                    int b = rgbBuffer["/avatar/parameters/B"];

                  
                    ProcessRGB(r, g, b);

                    
                    rgbBuffer.Clear();
                }
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



