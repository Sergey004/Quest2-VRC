using AdvancedSharpAdbClient;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.IO.Compression;

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
            Console.WriteLine("Exiting program due to external CTRL-C, or process kill, or shutdown, or program exiting");

            //do your cleanup here

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C platform-tools\\adb.exe kill-server";
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
            
            Console.Title = "Quest2-VRC";
            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--help":
                        Console.WriteLine("----Commands----\n--help - show help\n--no-sender - disable osc sender\n--no-receiver - disable osc receiver");
                        Handler(CtrlType.CTRL_CLOSE_EVENT);
                        break;
                    case "--no-sender":
                        StartADB(false, true);
                        break;
                    case "--no-receiver":
                        StartADB(true, false);
                        break;
                    default:
                        Console.WriteLine("Invalid argiment");
                        Handler(CtrlType.CTRL_CLOSE_EVENT);
                        break;

                }
            }
            while (!exitSystem)
            {
                Thread.Sleep(500);
            }
        }
        static void StartADB(bool sender, bool receiver)
        {
            Console.WriteLine("When you use this program you agree to the Terms and Conditions of the ADB, if you do not agree immediately close this program!");
            Console.WriteLine("Make sure you connect the headset to your computer and turn on the controllers");
            Console.WriteLine("To quit the application press CTRL+C to close the ADB server");
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                try
                {
                    bool exists = Directory.Exists("platform-tools");
                    if (!exists)
                    {
                        Console.WriteLine("ADB directory does not exist, creating...");
                        var client = new WebClient();
                        string uri = "https://dl.google.com/android/repository/platform-tools-latest-windows.zip";
                        string filename = "platform-tools-latest-windows.zip";
                        string extractPath = AppDomain.CurrentDomain.BaseDirectory;
                        client.DownloadFile(uri, filename);
                        client.DownloadFileCompleted += (sender, e) => Console.WriteLine("Finished");
                        ZipFile.ExtractToDirectory(filename, extractPath);
                        File.Delete(filename);
                    }
                    StartServerResult result = server.StartServer(@"platform-tools\adb.exe", false);
                    if (result != StartServerResult.Started)
                    {
                        Console.WriteLine("Can't start adb server, please restart app and try again");
                        Console.Title = "Error!";
                        Console.ReadLine();
                        Handler(CtrlType.CTRL_CLOSE_EVENT); 
                    }

                }
                catch (WebException)
                {
                    Console.WriteLine("Unable to download ADB from Google servers, try again or download files manually https://developer.android.com/studio/releases/platform-tools, press any key to exit");
                    Console.Title = "Error!";
                    Console.ReadLine();
                    Handler(CtrlType.CTRL_CLOSE_EVENT);
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
                Console.Title = "Error!";
                Console.ReadLine();
                Handler(CtrlType.CTRL_CLOSE_EVENT);
                return;
            }
            if (device.Name != "hollywood" && device.Name != "vr_monterey" && device.Name != "monterey" && device.Name != "seacliff")
            {
                Console.WriteLine("Oculus/Meta device is not detected or is not authorized, please disconnect all non Oculus/Meta devices and close all emulators on PC, restart app and try again");
                Console.Title = "Error!";
                Console.ReadLine();
                Handler(CtrlType.CTRL_CLOSE_EVENT);
            }
            if (device is not null)
            {
                Console.WriteLine("Selecting device:\nSerial or IP: {0}\nModel: {1}\nCodename: {2}", device.Serial, device.Model, device.Name);
                Console.Title = "Starting...";

            }


            //start your multi threaded program here
            if (receiver == false && sender == true)
            {
                Console.Title = "Tx + Rx";
                Console.WriteLine("OSC transfer is active");
                Console.WriteLine("OSC receiver is inactive");
                var tasks = new[]
                {
                     Task.Factory.StartNew(() => Sender.Run(), TaskCreationOptions.LongRunning)
                };


            }
            else if (receiver == true && sender == false)
            {
                Console.Title = "Rx Only";
                Console.WriteLine("OSC transfer is inactive");
                Console.WriteLine("OSC receiver is active");
                var tasks = new[]
                {
                    Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning)
                };
            }
            else 
            {
                Console.Title = "Tx Only";
                Console.WriteLine("OSC transfer is active");
                Console.WriteLine("OSC receiver is active");
                var tasks = new[]
                {
                     Task.Factory.StartNew(() => Sender.Run(), TaskCreationOptions.LongRunning),
                     Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning)
                };
            }
        }
    } 
}