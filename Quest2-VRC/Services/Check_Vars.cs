using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Quest2_VRC
{
    public class Check_Vars
    {
        public static void CheckVars()
        {
            bool exists = File.Exists("vars.json");
            if (!exists)
            {
                Console.WriteLine("vars.json does not exist, creating...");


                JObject vars = new JObject( // Default settings
                new JProperty("HMDBat", "HMDBat"),
                new JProperty("ControllerBatL", "ControllerBatL"),
                new JProperty("ControllerBatR", "ControllerBatR"),
                new JProperty("Receive_addr", "/avatar/parameters/Eyes mode"),
                new JProperty("Receive_addr_test", "/avatar/parameters/Eyes_mode"),
                new JProperty("SendPort", "9000"),
                new JProperty("HostIP", "127.0.0.1"));


                File.WriteAllText(@"vars.json", vars.ToString());

            }

            else
            {
                Console.WriteLine("vars.json exists");
            }
        }
    }
}
