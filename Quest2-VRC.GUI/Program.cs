using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.CommandLine;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quest2_VRC;


namespace Quest2_VRC
{
    internal static class Program
    {
        public static bool DebugLoggingEnabled = false;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        [STAThread]


        static async Task Main(string[] args)
        {
            if (!AttachConsole(ATTACH_PARENT_PROCESS))
            {
                Console.WriteLine("Failed to attach console.");
            }

            var forceeng = new Option<bool>(new[] { "--force-eng", "-en" }, () => { return false; }, "Force enable English lang");
            var enhancedoculuscontrol = new Option<bool>(new[] { "--enhanced-oculus-control", "-eoc" }, () => { return false; }, "Enables enhanced management of Oculus services (Like disable ASW, sets High Priority for Oculus services)");
            var debug = new Option<bool>(new[] { "--debug", "-d" }, () => false, "Enable debug logging to console");
            RootCommand _cmd = new("Quest 2 (and Quest 1, Quest Pro and newer) OSC and ADB powered battery information sender")
            {
                forceeng,
                enhancedoculuscontrol,
                debug
            };

            _cmd.SetHandler<bool, bool, bool>(Handler, forceeng, enhancedoculuscontrol, debug);
            _cmd.Invoke(args);
            // PluginLoader.LoadPlugins(); // <-- move to GUI

            static void Handler(bool forceeng, bool enhancedoculuscontrol, bool debug)
            {
                Logger.DebugEnabled = debug;
                if (forceeng == false && enhancedoculuscontrol == false)
                {
                    Vars.CheckVars();
                    if (Logger.DebugEnabled) Console.WriteLine("Debug logging enabled");
                    Console.WriteLine("Logs redirected to main window");
                    GUI();
                }
                if (forceeng == true && enhancedoculuscontrol == false)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    Vars.CheckVars();
                    if (Logger.DebugEnabled) Console.WriteLine("Debug logging enabled");
                    Console.WriteLine("Logs redirected to main window");
                    GUI();
                }
            }
            ToastNotificationManagerCompat.Uninstall();
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
                Notify_service.NotfyStarting();
                PluginLoader.LoadPlugins();
                Console.WriteLine("Starting plugins...");
                PluginLoader.StartAll();
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
        }
    }
}
