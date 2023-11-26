using System;
using System.CommandLine;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Quest2_VRC
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        [STAThread]


        static void Main(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);

            var forceeng = new Option<bool>(new[] { "--force-eng", "-en" }, () => { return false; }, "Force enable English lang");
            var enhancedoculuscontrol = new Option<bool>(new[] { "--enhanced-oculus-control", "-eoc" }, () => { return false; }, "Enables enhanced management of Oculus services (Like disable ASW, sets High Priority for Oculus services)");
            RootCommand _cmd = new("Quest 2 (and Quest 1, Quest Pro and newer) OSC and ADB powered battery information sender")
            {
                forceeng,
                enhancedoculuscontrol
            };

            _cmd.SetHandler<bool, bool>(Handler, forceeng, enhancedoculuscontrol);
            _cmd.Invoke(args);

            static void Handler(bool forceeng, bool enhancedoculuscontrol)
            {
                if (forceeng == false && enhancedoculuscontrol == false)
                {
                    Check_Vars.CheckVars();
                    Console.WriteLine("Logs redirected to main window");
                    GUI();
                }
                if (forceeng == true && enhancedoculuscontrol == false)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    Check_Vars.CheckVars();
                    Console.WriteLine("Logs redirected to main window");
                    GUI();
                }
                if (forceeng == false && enhancedoculuscontrol == true)
                {
                    OculusStaff.DisableASW();
                    OculusStaff.HighPriority();
                    var tasks = new[]
                        {
                             Task.Factory.StartNew(() => OculusStaff.DashWatchDog(), TaskCreationOptions.LongRunning),
                        };
                    Check_Vars.CheckVars();
                    Console.WriteLine("Logs redirected to main window");
                    GUI();
                }
                if (forceeng == true && enhancedoculuscontrol == true)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    OculusStaff.DisableASW();
                    OculusStaff.HighPriority();
                    var tasks = new[]
                        {
                             Task.Factory.StartNew(() => OculusStaff.DashWatchDog(), TaskCreationOptions.LongRunning),
                        };
                    Check_Vars.CheckVars();
                    Console.WriteLine("Logs redirected to main window");
                    GUI();
                }

            }
            Environment.Exit(1987); //Hehe yep I FNAF fan :) (This exit code = 0)
        }
        static void GUI()
        {

            Process[] processes = Process.GetProcessesByName(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            
            if (processes.Length > 1)
            {
                
                Application.EnableVisualStyles();
                MessageBox.Show("Only one instance of the program can be opened!", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1987);
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainWindow());
                }
                catch (SystemException)
                {
                    // Ignores errors
                }

            }


        }
    }
}
