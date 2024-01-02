using AdvancedSharpAdbClient;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Zeroconf;



namespace Quest2_VRC
{
    public class ADB
    {
        static public AdbClient client;
        static public DeviceData device;
        public static bool StartADB(bool sender, bool receiver, string hostip, bool wireless, bool audioEnadled, bool disableerrmsg)
        {

            Console.WriteLine("Make sure you connect the headset to your computer and turn on the controllers");


            client = new AdbClient();
            client.Connect(hostip);
            device = client.GetDevices().FirstOrDefault();
            Thread.Sleep(500);
            if (device == null || device.Serial == null)
            {
                Console.WriteLine("No devices found, please try again");

                return false;
            }
            if (device.Name != "hollywood" && device.Name != "vr_monterey" && device.Name != "monterey" && device.Name != "seacliff" && device.Name != "eureka") // 
            /* Based on documentation and rumors
            List of internal names Oculus/Meta VR devices 
            Quest 1 (Android 7.0) = vr_monterey
            Quest 1 (Android 10) = monterey
            Quest 2 = hollywood
            Quest Pro = seacliff
            Quest 3 = eureka */
            {
                Console.WriteLine("Device is: \nModel: {0}\nCodename: {1} \nState: {2}", device.Model, device.Name, device.State);
                Console.WriteLine("Oculus/Meta device is not detected or is not authorized, please disconnect all non Oculus/Meta devices and close all emulators on PC, try again");

                return false;
            }

            Console.WriteLine("Selecting device:\nSerial or IP: {0}\nModel: {1}\nCodename: {2}", device.Serial, device.Model, device.Name);



            if (receiver == false && sender == true)
            {

                Console.WriteLine("OSC transfer is active");
                Console.WriteLine("OSC receiver is inactive");
                var tasks = new[]
                {
                     Task.Factory.StartNew(() => Sender.Run(wireless, audioEnadled, disableerrmsg, hostip), TaskCreationOptions.LongRunning),
                };


            }
            else if (receiver == true && sender == false)
            {

                Console.WriteLine("OSC transfer is inactive");
                Console.WriteLine("OSC receiver is active");
                var tasks = new[]
                {
                    Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning)
                };
            }
            else
            {

                Console.WriteLine("OSC transfer is active");
                Console.WriteLine("OSC receiver is active");
                var tasks = new[]
                {
                     Task.Factory.StartNew(() => Sender.Run(wireless,audioEnadled, disableerrmsg,hostip), TaskCreationOptions.LongRunning),
                     Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning),
                };

            }

            return true;
        }
        [SupportedOSPlatform("windows")]
        public static void StartTCPIP()
        {

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C platform-tools\\adb.exe tcpip 5555";
            process.StartInfo = startInfo;
            process.Start();

        }
        public static void ForceKillADB()
        {

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C platform-tools\\adb.exe kill-server";
            process.StartInfo = startInfo;
            process.Start();

        }
        public static string GetIP()
        {
            string deviceip = null;
            client = new AdbClient();
            client.Connect("127.0.0.1:62001");
            device = client.GetDevices().FirstOrDefault();
            Thread.Sleep(500);
            ConsoleOutputReceiver ipquery = new ConsoleOutputReceiver();
            client.ExecuteRemoteCommand("ip route | grep wlan0", device, ipquery);
            deviceip = Regex.Match(ipquery.ToString(), @"\S*\d+", RegexOptions.RightToLeft).ToString();


            return deviceip;
        }
        public static async Task<string> GetZeroConfIP()
        {
            string deviceip = null;

            var adbtls = await ZeroconfResolver.ResolveAsync("_adb-tls-connect._tcp.local."); // Android 11+
            foreach (var headset in adbtls)
            {
                foreach (IService service in headset.Services.Values)
                {
                    deviceip = headset.IPAddress + ":" + service.Port;
                }
            }
            var secadb = await ZeroconfResolver.ResolveAsync("_adb_secure_connect._tcp.local."); // Android 10 and below, not actually tested
            foreach (var headset in secadb)
            {
                foreach (IService service in headset.Services.Values)
                {
                    deviceip = headset.IPAddress + ":" + service.Port;
                }
            }

            return await Task.FromResult(deviceip);
        }

        public static void DownLoadADB()
        {
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                try
                {

                    bool exists = Directory.Exists("platform-tools"); // ADB Auto downloader
                    if (!exists)
                    {
                        Console.WriteLine("ADB directory does not exist, creating...");
                        Console.WriteLine("Downloading ADB");
                        var client = new WebClient();
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
                        {
                            string uri = "https://dl.google.com/android/repository/platform-tools-latest-windows.zip";
                            string filename = "platform-tools-latest-windows.zip";
                            string extractPath = AppDomain.CurrentDomain.BaseDirectory;
                            client.DownloadFile(uri, filename);
                            ZipFile.ExtractToDirectory(filename, extractPath);
                            File.Delete(filename);
                        }
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true) // Really Mac OS?!
                        {
                            string uri = "https://dl.google.com/android/repository/platform-tools-latest-darwin.zip";
                            string filename = "platform-tools-latest-darwin.zip";
                            string extractPath = AppDomain.CurrentDomain.BaseDirectory;
                            client.DownloadFile(uri, filename);
                            ZipFile.ExtractToDirectory(filename, extractPath);
                            File.Delete(filename);
                        }
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == true) // Well, I can't imagine who would use this lib under Linux and in Mac OS XD
                        {
                            string uri = "https://dl.google.com/android/repository/platform-tools-latest-linux.zip";
                            string filename = "platform-tools-latest-linux.zip";
                            string extractPath = AppDomain.CurrentDomain.BaseDirectory;
                            client.DownloadFile(uri, filename);
                            ZipFile.ExtractToDirectory(filename, extractPath);
                            File.Delete(filename);
                        }
                        Console.WriteLine("Download completed");
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
                    {
                        StartServerResult result = server.StartServer(@"platform-tools\adb.exe", false);
                        if (result != StartServerResult.Started)
                        {
                            Console.WriteLine("Can't start adb server, please try again");

                            
                        }
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == true)
                    {
                        StartServerResult result = server.StartServer(@"platform-tools/adb", false);
                        if (result != StartServerResult.Started)
                        {
                            Console.WriteLine("Can't start adb server, please try again");

                            
                        }
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
                    {
                        StartServerResult result = server.StartServer(@"platform-tools/adb", false);
                        if (result != StartServerResult.Started)
                        {
                            Console.WriteLine("Can't start adb server, please try again");

                            
                        }
                    }
                }
                catch (WebException)
                {
                    Console.WriteLine("Unable to download ADB from Google servers, try again or download files manually https://developer.android.com/studio/releases/platform-tools, press any key to exit");

                    
                }

            }
            else
            {
                Console.WriteLine("ADB server is already running, no checks are required");
            }
        }

        public static void StopADB()
        {
            if (!AdbServer.Instance.GetStatus().IsRunning == false)
            {
                try
                
                {
                    
                    ForceKillADB();
                    Environment.Exit(1987);
                }
                catch 
                { 
                    // IDK how this works
                }
                
            }
            else
            {
                Environment.Exit(1987);
            }

        }

    }
}