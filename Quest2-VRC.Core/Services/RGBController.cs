using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Data;
using System.Linq;
using static Quest2_VRC.Logger;


namespace Quest2_VRC.Services
{
    public class RGBController
    {
        public static void SendRGBRawData(int R, int G, int B)
        {
            try
            { 
            using var client = new OpenRGBClient(name: "Quest2-VRC OSC Receiver", autoconnect: true, timeout: 1000);

            var deviceCount = client.GetControllerCount();
            var devices = client.GetAllControllerData();

            var R_Byte = ((byte)R);
            var G_Byte = ((byte)G);
            var B_Byte= ((byte)B);

                for (int i = 0; i < devices.Length; i++)
                {
                    var leds = Enumerable.Range(0, devices[i].Colors.Length)
                        .Select(_ => new Color(R_Byte,G_Byte,B_Byte))   
                        .ToArray();
                    client.UpdateLeds(i, leds);
                }
            }

            catch (TimeoutException)
            {
                LogToConsole("OpenRGB server is not enabled, or not installed");
            }
        }

    }

}


