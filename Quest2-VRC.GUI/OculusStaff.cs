using AdvancedSharpAdbClient;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Quest2_VRC.ADB;
using static Quest2_VRC.Logger;
using static Quest2_VRC.PacketSender;

namespace Quest2_VRC
{
    [SupportedOSPlatform("windows")]
    public class OculusStaff
    {
        [SupportedOSPlatform("windows")]
        public static async void DisableASW()
        {
            string OculusBase = "OculusBase";
            try
            {
                var value = System.Environment.GetEnvironmentVariable(OculusBase);

                string loc = "-f " + AppDomain.CurrentDomain.BaseDirectory;
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = value + "Support\\oculus-diagnostics\\OculusDebugToolCLI.exe",
                        Arguments = loc + "\\odtclicommands.txt",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                    File.AppendAllText("odtout.txt", line + Environment.NewLine);

                }
            }
            catch (Win32Exception)
            {
                Application.EnableVisualStyles();
                MessageBox.Show("Oculus Software not installed!", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            await Task.Delay(1);
        }
        [SupportedOSPlatform("windows")]
        public async static void HighPriority()
        {

            Process[] OculusClient = Process.GetProcessesByName("OculusClient");
            foreach (Process process in OculusClient)
            {
                Console.WriteLine("Process name: {0}, ID: {1}", process.ProcessName, process.Id);
                process.PriorityClass = ProcessPriorityClass.High;
                Console.WriteLine(process.PriorityClass);
            }
            Process[] OculusDash = Process.GetProcessesByName("OculusDash");
            foreach (Process process in OculusDash)
            {
                Console.WriteLine("Process name: {0}, ID: {1}", process.ProcessName, process.Id);
                process.PriorityClass = ProcessPriorityClass.High;
                Console.WriteLine(process.PriorityClass);
            }
            Process[] OVRServer_x64 = Process.GetProcessesByName("OVRServer_x64");
            foreach (Process process in OVRServer_x64)
            {
                Console.WriteLine("Process name: {0}, ID: {1}", process.ProcessName, process.Id);
                process.PriorityClass = ProcessPriorityClass.High;
                Console.WriteLine(process.PriorityClass);
            }
            Console.WriteLine("Priority set to High");
            await Task.Delay(1);

        }
        [SupportedOSPlatform("windows")]
        public async static void NormalPriority()
        {
            Process[] OculusClient = Process.GetProcessesByName("OculusClient");
            foreach (Process process in OculusClient)
            {
                Console.WriteLine("Process name: {0}, ID: {1}", process.ProcessName, process.Id);
                process.PriorityClass = ProcessPriorityClass.Normal;
                Console.WriteLine(process.PriorityClass);
            }
            Process[] OculusDash = Process.GetProcessesByName("OculusDash");
            foreach (Process process in OculusDash)
            {
                Console.WriteLine("Process name: {0}, ID: {1}", process.ProcessName, process.Id);
                process.PriorityClass = ProcessPriorityClass.Normal;
                Console.WriteLine(process.PriorityClass);
            }
            Process[] OVRServer_x64 = Process.GetProcessesByName("OVRServer_x64");
            foreach (Process process in OVRServer_x64)
            {
                Console.WriteLine("Process name: {0}, ID: {1}", process.ProcessName, process.Id);
                process.PriorityClass = ProcessPriorityClass.Normal;
                Console.WriteLine(process.PriorityClass);
            }
            Console.WriteLine("Priority set to Normal");
            await Task.Delay(1);

        }
        [SupportedOSPlatform("windows")]
        public async static void StartLink()
        {
           ConsoleOutputReceiver Hbat_receiver = new ConsoleOutputReceiver();
           await client.ExecuteRemoteCommandAsync("am start \"xrstreamingclient://?\"", device, null);

        }
        [SupportedOSPlatform("windows")]
        public async static void DashWatchDog()
        {
            while (true)
            {
                EventLog log = new
                EventLog("Application");

                DateTime dt = DateTime.Now;
                foreach (EventLogEntry entry in log.Entries)
                {

                    if (entry.Source.Equals("Application Error") && (entry.TimeGenerated > dt))

                    {
                        var appName = entry.Message;
                        if (Regex.IsMatch(appName, @"OculusDash.exe")) // YES I HAVE THIS PROBLEM AND I HATE IT!
                        {
                            string inputbox = "input";
                            LogToConsole("Warning: Oculus Dash crashed, waiting for Dash to restart");
                            VRChatMessage MsgErr = new VRChatMessage(inputbox, "Warning: Oculus Dash crashed, waiting for Dash to restart");
                            SendPacket(MsgErr);


                        }
                    }
                }
            }

        }
    }
}

