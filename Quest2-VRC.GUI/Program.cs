using System;
using System.Globalization;
using System.Text;
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

        [STAThread]


        static void Main(string[] args)
        {
            Check_Vars.CheckVars();
            foreach (string arg in args)
            {

                switch (arg)
                {
                    case "--help":
                        Console.WriteLine("----Commands----\n--force-eng - Force enable English lang\n --enhanced-oculus-control - Enables enhanced management of Oculus services (Like disable ASW, sets High Priority for Oculus services");
                        break;
                    case "--force-eng":
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                        GUI();
                        break;
                    case "--enhanced-oculus-control":
                        
                        OculusStaff.DisableASW();
                        OculusStaff.HighPriority();
                        var tasks = new[]
                {
                     Task.Factory.StartNew(() => OculusStaff.DashWatchDog(), TaskCreationOptions.LongRunning),
                };
                        
                        GUI();
                        break;
                    default:
                        Console.WriteLine("Invalid argiment");
                        break;


                }
            }
            GUI();
        }
        static void GUI()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
