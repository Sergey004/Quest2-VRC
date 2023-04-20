using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Quest2_Native
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var device = DeviceInfo.Manufacturer;
            if (device != "Oculus")
            {
                
            }
            else
            {
                DisplayAlert("Alert", $"Your device {device}\nApplication may not work correctly", "OK");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            Sender.Run();
            button1.Text = "Sending?";

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

     
    }
}
