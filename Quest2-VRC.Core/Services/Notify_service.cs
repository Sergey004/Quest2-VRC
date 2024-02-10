using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quest2_VRC
{
   public class Notify_service
    {
        static public void NotfyDowload()
        {
            new ToastContentBuilder()
    .AddText("ADB Dowload")
    .AddText("Started")
    .Show();
        }
        static public void NotfyComplited()
        {
            new ToastContentBuilder()
    .AddText("ADB Dowload")
    .AddText("Complited")
    .Show();
            Thread.Sleep(5000);

        }
    }
}
