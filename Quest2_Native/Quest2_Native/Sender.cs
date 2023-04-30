using Bespoke.Osc;
using System;
using Xamarin.Essentials;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Quest2_Native.Logger;
using static Quest2_Native.PacketSender;
using System.IO;


namespace Quest2_Native
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

            bool LowHMDBat = false;
            bool HMDCrit = false;

            while (true)
            {

                string HMDBat = "HMDBat";
                //string ControllerBatL = "ControllerBatL";
                //string ControllerBatR = "ControllerBatR";

                int Hbatlevelint = 0;
                //int Rbatlevelint = 0;
                //int Lbatlevelint = 0;


                var Hbat_receiver = Battery.ChargeLevel;


                //var Lbat_receiver = "100"; //Plaseholder
                //var Rbat_receiver = "100"; //Plaseholder




                var Hbat_match = Regex.Match(Hbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                //var Rbat_match = Regex.Match(Rbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);
                //var Lbat_match = Regex.Match(Lbat_receiver.ToString(), @"\d+", RegexOptions.RightToLeft);


                Hbatlevelint = int.Parse(Hbat_match.Value);
                //Rbatlevelint = int.Parse(Rbat_match.Value);
                //Lbatlevelint = int.Parse(Lbat_match.Value);

                if (Hbatlevelint < 25)
                {
                    LowHMDBat = true;
                    if (HMDCrit = false && Hbatlevelint == 15)
                    {
                        string inputbox = "input";
                        LogToConsole("Headset battery is at critical value, headset is turns off.");
                        VRChatMessage MsgCrit = new VRChatMessage(inputbox, "Headset battery is at critical value, headset is turns off.");
                        SendPacket(MsgCrit);
                        HMDCrit = true;
                    }

                }
                //if (Rbatlevelint < 25)
                //{
                //    LogToConsole("Right controller is discharged, disabled or not connected");
                //}
                //if (Lbatlevelint < 25)
                //{
                //    LogToConsole("Left controller is discharged, disabled or not connected");

                //}

                VRChatMessage Msg1 = new VRChatMessage(HMDBat, (float)Hbatlevelint / 100);
                //VRChatMessage Msg2 = new VRChatMessage(ControllerBatL, (float)Lbatlevelint / 100);
                //VRChatMessage Msg3 = new VRChatMessage(ControllerBatR, (float)Rbatlevelint / 100);
                VRChatMessage Msg4 = new VRChatMessage("LowHMDBat", LowHMDBat);
                SendPacket(Msg1, Msg4);

                LogToConsole("Sending HMD status", Msg1, Msg4);

                await Task.Delay(3000);


            }
        }
        //public static void CtrLeft() 
        //{
        //    Java.Lang.Process process;
        //    process = Java.Lang.Runtime.GetRuntime().Exec("dumpsys"); WORKS ONLY IN ADB
        //    process.WaitFor();
        //    var log = process.InputStream;
        //    using (StreamReader sr = new StreamReader(log))
        //    {
        //        string line;
        //        // Read and display lines from the file until the end of
        //        // the file is reached.
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            System.Console.WriteLine(line);

        //        }

        //   }

        //}
    }

}