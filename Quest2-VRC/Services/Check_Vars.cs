using System;
using System.IO;

namespace Quest2_VRC
{
    public class Check_Vars
    {
        public static void CheckVars()
        {
            bool exists = File.Exists("vars.txt");
            if (!exists)
            {
                Console.WriteLine("vars.txt does not exist, creating...");
                string[] lines = // Default settings
                {
                    "HMDBat = HMDBat",
                    "ControllerBatL = ControllerBatL",
                    "ControllerBatR = ControllerBatR",
                    "Receive_addr = /avatar/parameters/Eyes mode",
                    "Receive_addr_test = /avatar/parameters/Eyes_mode",
                    "SendPort = 9000",
                    "ReceivePort = 9001",
                    "HostIP = 127.0.0.1" 
                };
                File.WriteAllLines("vars.txt", lines);
            }

            else
            {
                Console.WriteLine("vars.txt exists");
            }
        }
    }
}
