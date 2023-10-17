using AdvancedSharpAdbClient;
using System;
using System.CommandLine;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static Quest2_VRC.ADB;
using static Quest2_VRC.Check_Vars;
using static Quest2_VRC.OSC;

namespace Quest2_VRC
{

    public class Program
    {

        static bool exitSystem = false;
        static public AdbClient client;
        public DeviceData device;
        static string questip = "127.0.0.1:62001";

        static void Main(string[] args)
        {

            Console.Title = "Quest2-VRC";
           
            

            var forceeng = new Option<bool>(new[] { "--force-eng", "-en" }, () => { return false; }, "Force enable English lang");
            var enhancedoculuscontrol = new Option<bool>(new[] { "--enhanced-oculus-control", "-eoc" }, () => { return false; }, "Enables enhanced management of Oculus services (Like disable ASW, sets High Priority for Oculus services)");
            RootCommand _cmd = new("Quest 2 (and Quest 1, Quest Pro and newer) OSC and ADB powered battery information sender")
            {
               enhancedoculuscontrol
            };

            _cmd.SetHandler<bool>(Handler, enhancedoculuscontrol);
            _cmd.Invoke(args);

            static void Handler(bool enhancedoculuscontrol)
            {
                if (enhancedoculuscontrol == false)
                {
                    Check_Vars.CheckVars();
                    Console.WriteLine("Logs redirected to main window");
                    
                }
                if (enhancedoculuscontrol == true)
                {
                    OculusStaff.DisableASW();
                    OculusStaff.HighPriority();
                    var tasks = new[]
                        {
                             Task.Factory.StartNew(() => OculusStaff.DashWatchDog(), TaskCreationOptions.LongRunning),
                        };
                    Check_Vars.CheckVars();
                    Console.WriteLine("Logs redirected to main window");
                    
                }
                

            }
            Environment.Exit(1987); //Hehe yep I FNAF fan :) (This exit code = 0)
        }
    }
}