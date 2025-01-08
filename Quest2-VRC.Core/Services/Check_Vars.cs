using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

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
                new JProperty("UseCustomPort", "False"),
                new JProperty("SendPort", "9000"),
                new JProperty("HostIP", "127.0.0.1"),
                new JProperty("LastKnownIP", "127.0.0.1"));


                File.WriteAllText(@"vars.json", vars.ToString());

            }

            else
            {
                Console.WriteLine("vars.json exists");
            }
        }
        public static void WriteJSON(string lastip)
        {
            string jsonString = File.ReadAllText("vars.json");
            JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString) as JObject;
            JToken jToken = jObject.SelectToken("LastKnownIP");
            jToken.Replace(lastip);
            string updatedJsonString = jObject.ToString();
            File.WriteAllText("vars.json", updatedJsonString);
        }
        public static string ReadJSON(string lastip)
        {
            string jsonString = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(jsonString);
            
            return _ = ((string)vars["LastKnownIP"]);

        }
        
    }
}
