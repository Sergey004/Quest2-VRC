using System;
using System.IO;
using System.Threading.Tasks;

namespace Quest2_VRC
{
    internal class Check_Vars
    {
        public static async Task CheckVars()
        {
            bool exists = File.Exists("vars.txt");
            if (!exists)
            {
                Console.WriteLine("vars.txt does not exist, creating...");
                string[] lines =
                {
                    "HMDBat = HMDBat", "ControllerBatL = ControllerBatL", "ControllerBatR = ControllerBatR", "Receive_addr = /avatar/parameters/Eyes mode", "Receive_addr_test = /avatar/parameters/Eyes_mode"  // Default settings for my avatar
                };
                File.WriteAllLines("vars.txt", lines);

            }
        }
    }
}
