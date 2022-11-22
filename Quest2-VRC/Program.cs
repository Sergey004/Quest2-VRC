using AdvancedSharpAdbClient;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace Quest2_VRC
{
    public class Program
    {
        static bool exitSystem = false;

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
            startInfo.Arguments = "/C adb.exe kill-server";
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
            Console.WriteLine("Make sure you connect the headset to your computer and turn on the controllers");
            Console.WriteLine("To quit the application press CTRL+C to close the ADB server");
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                try
                {
                    Console.WriteLine("Checking for adb components...");
                    Console.WriteLine(File.Exists(@"AdbWinApi.dll") ? "AdbWinApi.dll exists." : "AdbWinApi.dll does not exist.");
                    Console.WriteLine(File.Exists(@"AdbWinUsbApi.dll") ? "AdbWinUsbApi.dll exists." : "AdbWinUsbApi.dll does not exist.");
                    Console.WriteLine(File.Exists(@"adb.exe") ? "Adb.exe exists." : "Adb.exe does not exist.");
                    StartServerResult result = server.StartServer(@"adb.exe", false);
                    if (result != StartServerResult.Started)
                    {
                        Console.WriteLine("Can't start adb server, please restart app and try again");
                        Console.ReadLine();
                        return;
                    }
                    
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("ADB.exe, AdbWinApi.dll or AdbWinUsbApi.dll not found in root of program, you can download from https://developer.android.com/studio/releases/platform-tools, press any key to exit");
                    Console.ReadLine();
                    Environment.Exit(-1);
                }
              
            }
            else
            {
                Console.WriteLine("ADB server is already running, no checks are required");
            }
            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

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




