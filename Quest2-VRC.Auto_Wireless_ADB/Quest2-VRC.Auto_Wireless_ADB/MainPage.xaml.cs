﻿using Android;
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
            try
            { Settings.Global.PutInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled", (int)1);
            }
            catch (Java.Lang.SecurityException)
            {
                DisplayAlert("Error", $"android.permission.WRITE_SECURE_SETTINGS not set", "OK");
                
            }
            
            int value = Settings.Global.GetInt(Android.App.Application.Context.ContentResolver, "adb_wifi_enabled");

            
            if (value == 1)
            {
                DisplayAlert("Alert", $"adb_wifi_enabled is {value}", "OK");
                CrossLocalNotifications.Current.Show("Wireless ADB", "Wireless ADB is now configured", 0, DateTime.Now.AddSeconds(1));
            }
            else
            {
                DisplayAlert("Alert", $"adb_wifi_enabled is {value}", "OK");
                CrossLocalNotifications.Current.Show("Wireless ADB", "Wireless ADB is not configured", 0, DateTime.Now.AddSeconds(1));

            }
            
        }
        
        [BroadcastReceiver(Name = "com.quest2_vrc.auto_wireless_adb.BootBroadcastReceiver", Enabled = true)]
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