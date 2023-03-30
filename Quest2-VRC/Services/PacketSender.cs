using Bespoke.Osc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using static Quest2_VRC.Logger;

namespace Quest2_VRC
{

    public class PacketSender
    {
        

        static public void SendPacket(params VRChatMessage[] Params)
        {
            var dic = File.ReadAllLines("vars.txt")
           .Select(l => l.Split(new[] { '=' }))
           .ToDictionary(s => s[0].Trim(), s => s[1].Trim());
            int SendPort = int.Parse(dic["SendPort"]);
            var IP = IPAddress.Parse(dic["HostIP"]);
            IPEndPoint VRChat = new IPEndPoint(IP, SendPort);
            foreach (var Param in Params)
            {
                try
                {
                    // Check if there's a valid target
                    if (Param.Parameter == null || string.IsNullOrEmpty(Param.Parameter))
                        throw new Exception("Parameter target not set!");

                    // Check if Parameter is not null and of one of the following supported types
                    if (Param.Data != null &&
                        Param.Data.GetType() != typeof(int) &&
                        Param.Data.GetType() != typeof(float) &&
                        Param.Data.GetType() != typeof(bool) &&
                        Param.Data.GetType() != typeof(string))


                        throw new Exception(String.Format("Param of type {0} is not supported by VRChat!", Param.Data.GetType()));

                    // Create a bundle that contains the target address and port (VRChat works on localhost:9000)
                    OscBundle VRBundle = new OscBundle(VRChat);

                    // Create the message, and append the parameter to it
                    if (Param.Parameter == "input")
                    {

                        OscMessage Message = new OscMessage(VRChat, String.Format("/chatbox/{0}", Param.Parameter));
                        Message.Append(Param.Data);
                        Message.Append(true);
                        // Append the message to the bundle
                        VRBundle.Append(Message);

                        // Send the bundle to the target address and port
                        VRBundle.Send(VRChat);
                    }
                    else
                    {
                        OscMessage Message = new OscMessage(VRChat, String.Format("/avatar/parameters/{0}", Param.Parameter));
                        Message.Append(Param.Data);

                        // Append the message to the bundle

                        VRBundle.Append(Message);

                        // Send the bundle to the target address and port
                        VRBundle.Send(VRChat);
                    }



                }
                catch (Exception ex)
                {
                    LogToConsole(ex.ToString());
                }
            }
        }
    }


    public class VRChatMessage
    {
        // The target of the data
        public string? Parameter { get; }

        // The data itself
        public object? Data { get; }

        public VRChatMessage(string A, object B)
        {
            Parameter = A;
            Data = B;
        }

    }
}

