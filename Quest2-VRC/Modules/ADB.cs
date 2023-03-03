using AdvancedSharpAdbClient;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;



namespace Quest2_VRC
{
    public class ADB
    {
        static public AdbClient client;
        static public DeviceData device;
        public static bool StartADB(bool sender, bool receiver, string hostip, bool wireless)
        {
            Console.WriteLine("Make sure you connect the headset to your computer and turn on the controllers");
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                try
                {
                    bool exists = Directory.Exists("platform-tools"); // ADB Auto donwloader
                    if (!exists)
                    {
                        Console.WriteLine("ADB directory does not exist, creating...");
                        Console.WriteLine("Dowloading ADB");
                        var client = new WebClient();
                        string uri = "https://dl.google.com/android/repository/platform-tools-latest-windows.zip";
                        string filename = "platform-tools-latest-windows.zip";
                        string extractPath = AppDomain.CurrentDomain.BaseDirectory;
                        client.DownloadFile(uri, filename);
                        ZipFile.ExtractToDirectory(filename, extractPath);
                        File.Delete(filename);
                        Console.WriteLine("Download completed");
                    }
                    StartServerResult result = server.StartServer(@"platform-tools\adb.exe", false);
                    if (result != StartServerResult.Started)
                    {
                        Console.WriteLine("Can't start adb server, please try again");

                        return false;
                    }
                }
                catch (WebException)
                {
                    Console.WriteLine("Unable to download ADB from Google servers, try again or download files manually https://developer.android.com/studio/releases/platform-tools, press any key to exit");

                    return false;
                }

            }
            else
            {
                Console.WriteLine("ADB server is already running, no checks are required");
            }

            client = new AdbClient();
            client.Connect(hostip);;
            device = client.GetDevices().FirstOrDefault();
            Thread.Sleep(500);
            if (device == null || device.Serial == null)
            {
                Console.WriteLine("No devices found, please try again");
                //Console.WriteLine("Or you can connect your headset via Wireless ADB:\n1) Enter \"platform-tools\\adb tcpip 5555\" when your headset is connected to your computer via USB \n2) Use the \"Quest IP\" field for conndection\n \n \nIf this don't work\nIgnore the switch and  enter\n \"platform-tools\\adb tcpip 5555\" \n \"platform-tools\\adb connect  QUEST_IP:5555\"");

                return false;
            }
            if (device.Name != "hollywood" && device.Name != "vr_monterey" && device.Name != "monterey" && device.Name != "seacliff")
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
                     Task.Factory.StartNew(() => Sender.Run(wireless), TaskCreationOptions.LongRunning),
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
                     Task.Factory.StartNew(() => Sender.Run(wireless), TaskCreationOptions.LongRunning),
                     Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning),
                };

            }

            return true;
        }
    }
}
