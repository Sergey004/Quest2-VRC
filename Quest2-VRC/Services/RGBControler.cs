using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using static Quest2_VRC.Logger;


namespace Quest2_VRC.Services
{
    public class RGBControler
    {
        public static void SendRGBData(int dataInt) // Sends commands to OpenRGB (every time there is a change at the OSC address)
        {
            try
            {
                using var client = new OpenRGBClient(name: "Quest2-VRC OSC Receiver", autoconnect: true, timeout: 1000);

                var deviceCount = client.GetControllerCount();
                var devices = client.GetAllControllerData();

                var r = ((byte)0);
                var g = ((byte)0);
                var b = ((byte)0);
                // Default settings for my avatar
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
                    case 5:
                        r = 255;
                        g = 236;
                        b = 153;
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
            catch (TimeoutException)
            {
                LogToConsole("OpenRGB server is not enabled");
            }
        }
    }
}

