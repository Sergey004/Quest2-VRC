using Microsoft.Toolkit.Uwp.Notifications;
using System.Threading;

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
        static public void NotfyStarting()
        {
            new ToastContentBuilder()
            .AddText("App")
            .AddText("Starting")
            .Show();
            Thread.Sleep(5000);
        }
        static public void NotfyConneced()
        {
            new ToastContentBuilder()
            .AddText("App")
            .AddText("Conneced to HMD")
            .Show();
            Thread.Sleep(5000);
        }
    }
}
