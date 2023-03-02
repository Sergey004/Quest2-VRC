using AdvancedSharpAdbClient;
using System;
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

        #region Trap application termination
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        public delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting program due to external CTRL-C, or process kill, or shutdown, or program exiting");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C platform-tools\\adb.exe kill-server";
            process.StartInfo = startInfo;
            process.Start();

            Console.WriteLine("Cleanup complete");
            Thread.Sleep(100);
            exitSystem = true;

            Environment.Exit(-1);

            return true;
        }
        #endregion

        static void Main(string[] args)
        {

            Console.Title = "Quest2-VRC";
            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
            Console.WriteLine("When you use this program you agree to the Terms and Conditions of the ADB, if you do not agree immediately close this program!");
            Console.WriteLine("To quit the application press CTRL+C");
            CheckVars(); // Check if a file with parameters exists
            foreach (string arg in args)
            {
                
                switch (arg)
                {
                    case "--help":
                        Console.WriteLine("----Commands----\n--help - show help\n--no-sender - disable osc sender\n--no-receiver - disable osc receiver\n--no-adb - disable adb functions");
                        Handler(CtrlType.CTRL_CLOSE_EVENT);
                        break;
                    case "--no-adb":
                        StartOSC(false, true);
                        break;
                    case "--no-sender":
                        
                        StartADB(false, true, questip,false);
                        break;
                    case "--no-receiver":
                        
                        StartADB(true, false,questip,false);
                        break;
                    default:
                        Console.WriteLine("Invalid argiment");
                        Handler(CtrlType.CTRL_CLOSE_EVENT);
                        break;

                }

            }
           

            if (args.Length == 0)

            {
                StartADB(true, true,questip,false);
            }
            while (!exitSystem)
            {
                Thread.Sleep(500);
            }
        }
    }
}