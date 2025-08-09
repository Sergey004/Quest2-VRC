using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Quest2_VRC
{
    public class Vars
    {
        public class Global
        {
            public static string HMDBat { get; set; }
            public static string ControllerBatL { get; set; }
            public static string ControllerBatR { get; set; }
            public static bool UseCustomPort { get; set; }
            public static int SendPort { get; set; }
            public static int ReceivePort { get; set; }
            public static string HostIP { get; set; }
            public static string LastKnownIP { get;  set; }
        }

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
                new JProperty("UseCustomPort", "False"),
                new JProperty("SendPort", "9000"),
                new JProperty("ReceivePort", "9001"),
                new JProperty("HostIP", "127.0.0.1"),
                new JProperty("UseCustomPort", "False"),

                new JProperty("LastKnownIP", "127.0.0.1"));

                File.WriteAllText(@"vars.json", vars.ToString());
            }
            else
            {
                Console.WriteLine("vars.json exists");
            }

            LoadVars();
        }

        public static void WriteJSON(string lastip)
        {
            string jsonString = File.ReadAllText("vars.json");
            JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString) as JObject;
            JToken jToken = jObject.SelectToken("LastKnownIP");
            jToken.Replace(lastip);
            string updatedJsonString = jObject.ToString();
            File.WriteAllText("vars.json", updatedJsonString);

            Global.LastKnownIP = lastip;
        }

        public static string ReadJSON(string lastip)
        {
            string jsonString = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(jsonString);

            return _ = ((string)vars["LastKnownIP"]);
        }

        private static void LoadVars()
        {
            string jsonString = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(jsonString);

            Global.HMDBat = (string)vars["HMDBat"];
            Global.ControllerBatL = (string)vars["ControllerBatL"];
            Global.ControllerBatR = (string)vars["ControllerBatR"];
            Global.UseCustomPort = bool.Parse((string)vars["UseCustomPort"]);
            Global.SendPort = int.Parse((string)vars["SendPort"]);
            Global.ReceivePort = int.Parse((string)vars["ReceivePort"]);
            Global.HostIP = (string)vars["HostIP"];
            Global.LastKnownIP = (string)vars["LastKnownIP"];
        }
    }
}
