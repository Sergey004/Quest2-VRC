using System;
using System.Windows.Forms;
using static Quest2_VRC.ADB;

namespace Quest2_VRC
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 
        
        [STAThread] 
        

        static void Main()
        {
            var a = new ADB();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
    }
}
