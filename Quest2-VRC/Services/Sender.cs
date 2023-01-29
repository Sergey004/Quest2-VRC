using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Exceptions;
using Bespoke.Osc;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Quest2_VRC.ADB;
using static Quest2_VRC.Logger;
using static Quest2_VRC.PacketSender;


namespace Quest2_VRC
{
    static class Sender
    {

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
                    VRChatMessage Msg3 = new VRChatMessage(ControllerBatR, Rbatlevelf);
                    VRChatMessage Msg4 = new VRChatMessage("LowHMDBat", LowHMDBat);

                    SendPacket(Msg1, Msg2, Msg3, Msg4);

                    LogToConsole("Sending HMD status", Msg1, Msg2, Msg3, Msg4);

                    Thread.Sleep(3000);

                }
                catch (AdbException)
                {
                    string inputbox = "input";
                    LogToConsole("Error: connection to the headset is lost!");
                    VRChatMessage MsgErr = new VRChatMessage(inputbox, "Error: connection to the headset is lost! Reconnecting...");
                    SendPacket(MsgErr);
                    Thread.Sleep(3000);

                }


            }


        }
    }
}