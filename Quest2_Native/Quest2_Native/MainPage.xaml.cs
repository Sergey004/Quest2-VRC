using Java.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Quest2_Native.Sender;

namespace Quest2_Native
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var device = DeviceInfo.Name;
            if (device != "Quest")
            {
                DisplayAlert("Alert", $"Your device {device}\nStart VRChat first, then start the application, otherwise it will not work", "OK");
            }
            else
            {
                DisplayAlert("Alert", $"Your device {device}\nApplication may not work correctly", "OK");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            try

            {
                var tasks = new[]
                {
                     Task.Factory.StartNew(() => Sender.Run(), TaskCreationOptions.LongRunning),
                };
                button1.Text = "Sending!";

            }
            catch (System.Net.Sockets.SocketException)
            {
                DisplayAlert("Alert", "VRChat (or OSC app) is not running", "OK");
            }
        }


            private void Button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           //CtrLeft();



        }
    }
}
