using Bespoke.Osc;
using Newtonsoft.Json.Linq;
using Quest2_VRC.Services;
using System;
using System.IO;
using System.Net;

namespace Quest2_VRC
{
    public class Receiver

    {
        static readonly int dataInt = 0;
        public static async void Run()
        {
            RGBControler.SendRGBData(dataInt); //Init OpenRGB
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            string Eyesmode = (string)vars["Receive_addr"];
            string EyesmodeTest = (string)vars["Receive_addr_test"];
            int ReceivePort = int.Parse((string)vars["ReceivePort"]);
            var IP = IPAddress.Parse((string)vars["HostIP"]);

            OscServer oscServer;
            oscServer = new OscServer((Bespoke.Common.Net.TransportType)TransportType.Udp, IP, ReceivePort);
            oscServer.FilterRegisteredMethods = true;
            oscServer.RegisterMethod(Eyesmode);
            oscServer.RegisterMethod(EyesmodeTest);
            oscServer.MessageReceived += new EventHandler<OscMessageReceivedEventArgs>(oscServer_MessageReceived);
            oscServer.Start();
            Logger.LogToConsole("Make sure you have all effects disabled in OpenRGB");
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
                //Console.WriteLine(string.Format("{0}", dataString)); //Debug

                int dataInt = Int32.Parse(dataString);
                RGBControler.SendRGBData(dataInt);
            }

        }


    }

}




