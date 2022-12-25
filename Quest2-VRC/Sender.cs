using AdvancedSharpAdbClient;
using Bespoke.Osc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Quest2_VRC.Logger;
using static Quest2_VRC.Program;

namespace Quest2_VRC
{
    static class Sender
    {
        static readonly IPAddress IP = IPAddress.Loopback;
        static readonly int Port = 9000;
        static readonly IPEndPoint VRChat = new IPEndPoint(IP, Port);

        public static async void Run()
        {
            Random rnd = new Random();
            int Uport = rnd.Next(1, 9999);
            Console.WriteLine("OSC UDP port is {0}", Uport);
            await questwd(Uport);

        }

        public static async Task questwd(int Uport)
        {
            // Create a bogus port for the client
            OscPacket.UdpClient = new UdpClient(Uport);

            while (true)
            {
                try
                {
                    var dic = File.ReadAllLines("vars.txt")
                    .Select(l => l.Split(new[] { '=' }))
                    .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

                    string HMDBat = dic["HMDBat"];
                    string ControllerBatL = dic["ControllerBatL"];
                    string ControllerBatR = dic["ControllerBatR"];

                    int Hbatlevelint = 0;
                    int Rbatlevelint = 0;
                    int Lbatlevelint = 0;
                    bool LowHMDBat = false;

                    ConsoleOutputReceiver Hbat_receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand("dumpsys CompanionService | grep Battery", device, Hbat_receiver);
                    ConsoleOutputReceiver Rbat_receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand("dumpsys OVRRemoteService | grep Right", device, Rbat_receiver);
                    ConsoleOutputReceiver Lbat_receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand("dumpsys OVRRemoteService | grep Left", device, Lbat_receiver);

                    var Hbat_match = Regex.Match(Hbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                    var Rbat_match = Regex.Match(Rbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                    var Lbat_match = Regex.Match(Lbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);

                    Hbatlevelint = int.Parse(Hbat_match.Value);
                    Rbatlevelint = int.Parse(Rbat_match.Value);
                    Lbatlevelint = int.Parse(Lbat_match.Value);
                    float Hbatlevelf = Hbatlevelint;
                    float Rbatlevelf = Rbatlevelint;
                    float Lbatlevelf = Lbatlevelint;

                    if (Hbatlevelf < 15)
                    {
                        LowHMDBat = true;
                        //SoundPlayer playSound = new SoundPlayer(Properties.Resources.HMDloworbelow15);
                        //playSound.Play();

                    }
                    if (Rbatlevelf < 15)
                    {
                        LogToConsole("Right controller is discharged, disabled or not connected");
                        //SoundPlayer playSound = new SoundPlayer(Properties.Resources.Rcrtloworbelow15);
                        //playSound.Play();

                    }
                    if (Lbatlevelf < 15)
                    {
                        LogToConsole("Left controller is discharged, disabled or not connected");
                        //SoundPlayer playSound = new SoundPlayer(Properties.Resources.Lctrloworbelow15);
                        //playSound.Play();

                    }

                    VRChatMessage Msg1 = new VRChatMessage(HMDBat, Hbatlevelf);
                    VRChatMessage Msg2 = new VRChatMessage(ControllerBatL, Lbatlevelf);
                    VRChatMessage Msg3 = new VRChatMessage("ControllerBatR", Rbatlevelf);
                    VRChatMessage Msg4 = new VRChatMessage("LowHMDBat", LowHMDBat);

                    SendPacket(Msg1, Msg2, Msg3, Msg4);

                    LogToConsole("Sending HMD status", Msg1, Msg2, Msg3, Msg4);

                    Thread.Sleep(3000);

                }
                catch
                {
                    LogToConsole("Error!");

                }


            }

            static void SendPacket(params VRChatMessage[] Params)
            {
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
                            Param.Data.GetType() != typeof(bool))

                            throw new Exception(String.Format("Param of type {0} is not supported by VRChat!", Param.Data.GetType()));

                        // Create a bundle that contains the target address and port (VRChat works on localhost:9000)
                        OscBundle VRBundle = new OscBundle(VRChat);

                        // Create the message, and append the parameter to it
                        OscMessage Message = new OscMessage(VRChat, String.Format("/avatar/parameters/{0}", Param.Parameter));
                        Message.Append(Param.Data);

                        // Append the message to the bundle
                        VRBundle.Append(Message);

                        // Send the bundle to the target address and port
                        VRBundle.Send(VRChat);

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
}
