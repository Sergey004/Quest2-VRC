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
        public static async Task CheckRGBVars()
        {
            bool exists1 = File.Exists("rgbvars\\1.txt");
            if (!exists1)
            {
                Console.WriteLine("vars.txt does not exist, creating...");
                string[] lines =
                {
                    "HMDBat = HMDBat", "ControllerBatL = ControllerBatL", "ControllerBatR = ControllerBatR", "Receive_addr = /avatar/parameters/Eyes mode", "Receive_addr_test = /avatar/parameters/Eyes_mode"  // Default settings for my avatar
                };
                File.WriteAllLines("vars.txt", lines);

            }
            bool exists2 = File.Exists("rgbvars\\2.txt");
            if (!exists2)
            {
                Console.WriteLine("vars.txt does not exist, creating...");
                string[] lines =
                {
                    "HMDBat = HMDBat", "ControllerBatL = ControllerBatL", "ControllerBatR = ControllerBatR", "Receive_addr = /avatar/parameters/Eyes mode", "Receive_addr_test = /avatar/parameters/Eyes_mode"  // Default settings for my avatar
                };
                File.WriteAllLines("vars.txt", lines);
            }
            bool exists3 = File.Exists("rgbvars\\3.txt");
            if (!exists3)
            {
                Console.WriteLine("vars.txt does not exist, creating...");
                string[] lines =
                {
                    "HMDBat = HMDBat", "ControllerBatL = ControllerBatL", "ControllerBatR = ControllerBatR", "Receive_addr = /avatar/parameters/Eyes mode", "Receive_addr_test = /avatar/parameters/Eyes_mode"  // Default settings for my avatar
                };
                File.WriteAllLines("vars.txt", lines);
            }
            bool exists4 = File.Exists("rgbvars\\4.txt");
            if (!exists4)
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