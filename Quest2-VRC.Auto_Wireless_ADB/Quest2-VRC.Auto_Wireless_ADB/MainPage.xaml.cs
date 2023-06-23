using Android;
using Android.App;
using Android.Content;
using Android.Provider;
using Plugin.LocalNotifications;
using System;
using Xamarin.Forms;
[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]
namespace Quest2_VRC.Auto_Wireless_ADB
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private void button1_Clicked(object sender, EventArgs e)
        {

            Settings.Global.PutInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled", (int)1);
            int value = Settings.Global.GetInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled");

            DisplayAlert("Alert", $"adb_wifi_enabled is {value}", "OK");
            CrossLocalNotifications.Current.Show("Wireless ADB", "Wireless ADB is now configured", 0, DateTime.Now.AddSeconds(1));
        }
        
        [BroadcastReceiver(Name = "com.companyname.quest2_vrc.auto_wireless_adb.BootBroadcastReceiver", Enabled = true)]
        [IntentFilter(new[] { Intent.ActionBootCompleted })]
        public class BootBroadcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                Settings.Global.PutInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled", (int)1);
                CrossLocalNotifications.Current.Show("Wireless ADB", "Wireless ADB is now enabled", 0, DateTime.Now.AddSeconds(1));
            }
        }
    }
}
