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
using System.Windows.Forms;
using static Quest2_VRC.ADB;
using static Quest2_VRC.Logger;
using static Quest2_VRC.PacketSender;


namespace Quest2_VRC
{
    static class Sender
    {

        public static async void Run(bool wirlessmode)
        {
            Random rnd = new Random();
            int Uport = rnd.Next(1, 9999);
            Console.WriteLine("OSC UDP port is {0}", Uport);
            await questwd(Uport, wirlessmode);
        }

        public static async Task questwd(int Uport,bool wirlessmode)
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
                    string WifiRSSI = null;
                    int WifiInt = 0;
                    bool LowHMDBat = false;
                    bool HMDCrit = false;

                    ConsoleOutputReceiver Hbat_receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand("dumpsys CompanionService | grep Battery", device, Hbat_receiver);
                    ConsoleOutputReceiver Rbat_receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand("dumpsys OVRRemoteService | grep Right", device, Rbat_receiver);
                    ConsoleOutputReceiver Lbat_receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand("dumpsys OVRRemoteService | grep Left", device, Lbat_receiver);
                    if (wirlessmode == true)
                    {
                        ConsoleOutputReceiver Wifi_Singal = new ConsoleOutputReceiver();
                        client.ExecuteRemoteCommand("dumpsys wifi | grep RSSI:", device, Wifi_Singal);
                        var Wifi_match = Regex.Match(Wifi_Singal.ToString(), @"RSSI: \S*\d+");
                        WifiRSSI = Wifi_match.ToString().Substring(6);
                        WifiInt = int.Parse(WifiRSSI);
                        
                    }

                    var Hbat_match = Regex.Match(Hbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                    var Rbat_match = Regex.Match(Rbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                    var Lbat_match = Regex.Match(Lbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                    

                    Hbatlevelint = int.Parse(Hbat_match.Value);
                    Rbatlevelint = int.Parse(Rbat_match.Value);
                    Lbatlevelint = int.Parse(Lbat_match.Value);
                    
                    if (Hbatlevelint < 25)
                    {
                        LowHMDBat = true;
                        //SoundPlayer playSound = new SoundPlayer(Properties.Resources.HMDloworbelow25);
                        //playSound.Play();
                        if(HMDCrit = false && Hbatlevelint == 15)
                        {
                            string inputbox = "input";
                            LogToConsole("Headset battery is at critical value, headset is turns off.");
                            VRChatMessage MsgCrit = new VRChatMessage(inputbox, "Headset battery is at critical value, headset is turns off.");
                            SendPacket(MsgCrit);
                            HMDCrit = true;
                        }

                    }
                    if (Rbatlevelint < 25)
                    {
                        LogToConsole("Right controller is discharged, disabled or not connected");
                        //SoundPlayer playSound = new SoundPlayer(Properties.Resources.Rcrtloworbelow25);
                        //playSound.Play();

                    }
                    if (Lbatlevelint < 25)
                    {
                        LogToConsole("Left controller is discharged, disabled or not connected");
                        //SoundPlayer playSound = new SoundPlayer(Properties.Resources.Lctrloworbelow25);
                        //playSound.Play();

                    }

                    VRChatMessage Msg1 = new VRChatMessage(HMDBat, (float)Hbatlevelint / 100);
                    VRChatMessage Msg2 = new VRChatMessage(ControllerBatL, (float)Lbatlevelint / 100);
                    VRChatMessage Msg3 = new VRChatMessage(ControllerBatR, (float)Rbatlevelint / 100);
                    VRChatMessage Msg4 = new VRChatMessage("WifiRSSI", (float)WifiInt / 100);
                    VRChatMessage Msg5 = new VRChatMessage("LowHMDBat", LowHMDBat);
                    SendPacket(Msg1, Msg2, Msg3, Msg4, Msg5);

                    LogToConsole("Sending HMD status", Msg1, Msg2, Msg3, Msg4, Msg5);

                    await Task.Delay(3000);

                }
                catch (AdbException)
                {
                    string inputbox = "input";
                    LogToConsole("Error: Connection to the headset is lost! Trying to reconnect...");
                    VRChatMessage MsgErr = new VRChatMessage(inputbox, "Error: Connection to the headset is lost! Trying to reconnect...");
                    SendPacket(MsgErr);
                    await Task.Delay(3000);
                }


            }


        }
    }
}