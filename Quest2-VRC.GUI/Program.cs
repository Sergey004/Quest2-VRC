using System;
using System.Globalization;
using System.Threading;
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
            foreach (string arg in args)
            {

                switch (arg)
                {
                    case "--help":
                        Console.WriteLine("----Commands----\n--force-eng - Force enable English lang");
                        break;
                    case "--force-eng":
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
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
