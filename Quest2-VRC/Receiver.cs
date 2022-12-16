using Bespoke.Osc;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;


namespace Quest2_VRC
{
    internal class Receiver

    {
        static readonly int Port = 9001;
        public static async void Run()
        {
            string Eyesmode = "/avatar/parameters/Eyes_mode";
            string EyesmodeTest = "/avatar/parameters/Eyes mode";
            OscServer oscServer;
            oscServer = new OscServer((Bespoke.Common.Net.TransportType)TransportType.Udp, IPAddress.Loopback, Port);
            oscServer.FilterRegisteredMethods = true;
            oscServer.RegisterMethod(Eyesmode);
            oscServer.RegisterMethod(EyesmodeTest);
            oscServer.MessageReceived += new EventHandler<OscMessageReceivedEventArgs>(oscServer_MessageReceived);
            oscServer.Start();
        }

        private static void oscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            OscMessage message = e.Message;
            //Console.WriteLine(string.Format("\nMessage Received {0}", message.Address)); //Debug

            for (int i = 0; i < message.Data.Count; i++)
            {
                string dataString;

                if (message.Data[i] == null)
                {
                    dataString = "Nil";
                }
                else
                {
                    dataString = (message.Data[i] is byte[]? BitConverter.ToString((byte[])message.Data[i]) : message.Data[i].ToString());
                }
                //Console.WriteLine(string.Format("{0}", dataString));

                int dataInt = Int32.Parse(dataString);
                SendRGBData(dataInt);
            }
            
        }
        

        public static void SendRGBData(int dataInt)
        {
            using var client = new OpenRGBClient(name: "Quest2-VRC OSC Receiver", autoconnect: true, timeout: 1000);

            var deviceCount = client.GetControllerCount();
            var devices = client.GetAllControllerData();

            var r = ((byte)0);
            var g = ((byte)0);
            var b = ((byte)0);

            switch (dataInt)
            {
                case 1:
                    r = 191;
                    g = 0;
                    b = 178;
                    break;
                case 2:
                    r = 214;
                    g = 148;
                    b = 45;
                    break;
                case 3:
                    r = 214;
                    g = 148;
                    b = 45;
                    break;
                case 4:
                    r = 0;
                    g = 0;
                    b = 0;
                    break;
                default:
                    r = 0;
                    g = 192;
                    b = 255;
                    break;
            }

            for (int i = 0; i < devices.Length; i++)
            {
                var leds = Enumerable.Range(0, devices[i].Colors.Length)
                    .Select(_ => new Color(r, g, b))
                    .ToArray();
                client.UpdateLeds(i, leds);
            }
        }
    }

}

 


       