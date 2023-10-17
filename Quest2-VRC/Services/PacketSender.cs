using Bespoke.Osc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using static Quest2_VRC.Logger;

namespace Quest2_VRC
{

    public class PacketSender
    {


        static public void SendPacket(params VRChatMessage[] Params)
        {
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);


            int SendPort = int.Parse((string)vars["SendPort"]);
            var IP = IPAddress.Parse((string)vars["HostIP"]);
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

                    // Create a bundle that contains the target address and port (VRChat works on localhost:9000 or any IP:PORT)
                    OscBundle VRBundle = new OscBundle(VRChat);

                    // Create the message, and append the parameter to it
                    switch (Param.Parameter)
                    {
                        //Used to send commands to control the functions of the game itself
                        case var s when new[] { "MoveForward", "MoveBackward", "MoveLeft", "LookRight", "Jump", "Run", "ComfortLeft", "ComfortRight", "DropRight", "UseRight", "GrabRight", "DropLeft", "UseLeft", "GrabLeft", "PanicButton", "QuickMenuToggleLeft", "QuickMenuToggleRight", "Voice", "AFKToggle" }.Contains(s):
                            {
                                OscMessage Message = new OscMessage(VRChat, String.Format("/input/{0}", Param.Parameter));
                                Message.Append(Param.Data);
                                // Append the message to the bundle
                                VRBundle.Append(Message);

                                // Send the bundle to the target address and port
                                VRBundle.Send(VRChat);
                                break;
                            }
                        //Used to send text
                        case "input":
                            {
                                OscMessage Message = new OscMessage(VRChat, String.Format("/chatbox/{0}", Param.Parameter));
                                Message.Append(Param.Data);
                                Message.Append(true);
                                // Append the message to the bundle
                                VRBundle.Append(Message);

                                // Send the bundle to the target address and port
                                VRBundle.Send(VRChat);
                                break;

                            }
                        //Used to send avatar parameters
                        default:
                            {
                                OscMessage Message = new OscMessage(VRChat, String.Format("/avatar/parameters/{0}", Param.Parameter));
                                Message.Append(Param.Data);

                                // Append the message to the bundle

                                VRBundle.Append(Message);

                                // Send the bundle to the target address and port
                                VRBundle.Send(VRChat);
                                break;
                            }
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

