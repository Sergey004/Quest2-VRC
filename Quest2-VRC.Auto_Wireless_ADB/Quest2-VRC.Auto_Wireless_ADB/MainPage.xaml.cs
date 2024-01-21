using Android;
using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Provider;
using Android.Util;
using Android.Widget;
using Plugin.LocalNotifications;
using System;
using System.Diagnostics;
using System.Security.Claims;
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
            try
            {
                Settings.Global.PutInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled", (int)1);



                int value = Settings.Global.GetInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled");


                if (value == 1)
                {
                    DisplayAlert("Alert", $"adb_wifi_enabled is {value}", "OK");
                    CrossLocalNotifications.Current.Show("Wireless ADB", "Wireless ADB is now configured", 0, DateTime.Now);
                   
                }
                else
                {
                    DisplayAlert("Alert", $"adb_wifi_enabled is {value}", "OK");
                    CrossLocalNotifications.Current.Show("Wireless ADB", "Wireless ADB is not configured", 0, DateTime.Now);

                }
            }
            catch (Java.Lang.SecurityException)
            {
                DisplayAlert("Error", $"android.permission.WRITE_SECURE_SETTINGS not set", "OK");

            }

        }


        
        

        [BroadcastReceiver(Enabled = true, DirectBootAware = true, Exported = true)]
        [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, Intent.ActionReboot }, Priority = (int)IntentFilterPriority.HighPriority)]
        public class BootReceiver : BroadcastReceiver
        {
         
            static readonly string TAG = "BootBroadcastReceiver";
            public override void OnReceive(Context context, Intent intent)
            {

                bool bootCompleted;
                string action = intent.Action;
                //TODO Switch to UserManagerCompat, BuildCompat
                Log.Info(TAG, $"Recieved action {action}, user unlocked: "); //{UserManagerCompat.IsUserUnlocked (context))}");

                if (Build.VERSION.SdkInt > BuildVersionCodes.M)
                    bootCompleted = Intent.ActionLockedBootCompleted == action;
                else
                    bootCompleted = Intent.ActionBootCompleted == action;

                if (!bootCompleted)
                    return;
                CrossLocalNotifications.Current.Show("Wireless ADB", "FUCK", 0, DateTime.Now);

            }


            
        }
    }
}
