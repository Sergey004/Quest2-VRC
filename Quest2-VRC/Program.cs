using AdvancedSharpAdbClient;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Quest2_VRC
{
    public class Program
    {
        static bool exitSystem = false;
        static public AdvancedAdbClient client;
        static public DeviceData device;

        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            //do your cleanup here
            
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C ADB\\adb.exe kill-server";
            process.StartInfo = startInfo;
            process.Start();
            
            Console.WriteLine("Cleanup complete");
            Thread.Sleep(100);
            //allow main to run off
            exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion

        static void Main(string[] args)
        {
            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Console.WriteLine("Make sure you connect the headset to your computer and turn on the controllers");
            Console.WriteLine("To quit the application press CTRL+C to close the ADB server");
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                try
                {
                    Console.WriteLine("Checking for adb components...");
                    bool exists = Directory.Exists("ADB");
                    if (!exists)
                    {
                        Console.WriteLine("ADB directory does not exist, creating...");
                        Directory.CreateDirectory("ADB");
                        File.Create("ADB\\Put ADB files here.txt");
                    }

                    Console.WriteLine(File.Exists(@"ADB\\AdbWinApi.dll") ? "AdbWinApi.dll exists." : "AdbWinApi.dll does not exist.");
                    Console.WriteLine(File.Exists(@"ADB\\AdbWinUsbApi.dll") ? "AdbWinUsbApi.dll exists." : "AdbWinUsbApi.dll does not exist.");
                    Console.WriteLine(File.Exists(@"ADB\\adb.exe") ? "Adb.exe exists." : "Adb.exe does not exist.");
                    StartServerResult result = server.StartServer(@"ADB\\adb.exe", false);
                    if (result != StartServerResult.Started)
                    {
                        Console.WriteLine("Can't start adb server, please restart app and try again");
                        Console.ReadLine();
                        Environment.Exit(-1);
                    }

                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("ADB.exe, AdbWinApi.dll or AdbWinUsbApi.dll not found in ADB directory, you can download from https://developer.android.com/studio/releases/platform-tools, press any key to exit");
                    Console.ReadLine();
                    Environment.Exit(-1);
                }

            }
            else
            {
                Console.WriteLine("ADB server is already running, no checks are required");
            }

            client = new AdvancedAdbClient();
            client.Connect("127.0.0.1:62001");
            device = client.GetDevices().FirstOrDefault();
            if (device == null)
            {
                Console.WriteLine("No devices found, please restart app and try again");
                Console.WriteLine("Or you can connect your headset via Wireless ADB: ADB\\adb.exe connect HEADSET_IP:5555");
                Console.ReadLine();
                return;
            }
            if (device.Name != "hollywood" && device.Name != "vr_monterey" && device.Name != "monterey" && device.Name != "seacliff")
            {
                Console.WriteLine("Oculus/Meta device is not detected or is not authorized, please disconnect all non Oculus/Meta devices and close all emulators on PC, restart app and try again");
                Console.ReadLine();
                Environment.Exit(-1);
            }
            if (device is not null)
            {
                Console.WriteLine("Selecting device:\nSerial or IP: {0}\nModel: {1}\nCodename: {2}", device.Serial, device.Model, device.Name);

            }

            //start your multi threaded program here
            VRCProgram.Run();

            //hold the console so it doesn’t run off the end
            while (!exitSystem)
            {
                Thread.Sleep(500);
            }
            
        }


    }

    
}




