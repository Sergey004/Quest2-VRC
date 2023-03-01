using Bespoke.Osc;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using Quest2_VRC.Services;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using static Quest2_VRC.Logger;
using static Quest2_VRC.PacketSender;

namespace Quest2_VRC
{
    public class Receiver

    {
        static readonly int dataInt = 0;
        static readonly int Port = 9001;
        public static async void Run()
        {
            RGBControler.SendRGBData(dataInt); //Init OpenRGB
            var dic = File.ReadAllLines("vars.txt")
            .Select(l => l.Split(new[] { '=' }))
            .ToDictionary(s => s[0].Trim(), s => s[1].Trim());
            string Eyesmode = dic["Receive_addr"];
            string EyesmodeTest = dic["Receive_addr_test"];
            OscServer oscServer;
            oscServer = new OscServer((Bespoke.Common.Net.TransportType)TransportType.Udp, IP, Port);
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




