using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Printing;
using static Quest2_VRC.PacketSender;
using static Quest2_VRC.Logger;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.Versioning;

namespace Quest2_VRC
{
    public class OculusStaff
    {
        [SupportedOSPlatform("windows")]
        public static async void DisableASW()
        {
            string OculusBase = "OculusBase";
            try{
                var value = System.Environment.GetEnvironmentVariable(OculusBase);
                Console.WriteLine(value + "Support\\oculus-diagnostics");
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
        public async static void DashWatchDog()
        {
            // It still doesn't work

            //while (true)
            //{
            //    EventLog myLog = new EventLog();
            //    myLog.Log = "Application";
            //    myLog.Source = "Application Error"; 

            //    var lastEntry = myLog.Entries[myLog.Entries.Count - 1];
            //    var last_error_Message = lastEntry.Message;

            //    for (int index = myLog.Entries.Count - 1; index > 0; index--)
            //    {
            //        var errLastEntry = myLog.Entries[index];
            //        if (errLastEntry.EntryType == EventLogEntryType.Error)
            //        {

            //            var appName = errLastEntry.Message;
            //            if (Regex.IsMatch(appName, @"OculusDash.exe")) // YES I HAVE THIS PROBLEM AND I HATE IT!
            //            {
            //                string inputbox = "input";
            //                LogToConsole("Error: Oculus dash crashed, waiting for app restart and enabling Voice Chat");
            //                VRChatMessage MsgErr = new VRChatMessage(inputbox, "Error: Oculus dash crashed, waiting for app restart and enabling Voice Chat");
            //                SendPacket(MsgErr);
            //                string input = "Voice";
            //                VRChatMessage VoiceReleased = new VRChatMessage(input, 0);
            //                SendPacket(VoiceReleased);
            //                VRChatMessage VoicePressed = new VRChatMessage(input, 1);
            //                SendPacket(VoicePressed);

            //            }


            //        }
            //    }
            //    await Task.Delay(100);
        }
    }
    }

