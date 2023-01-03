using System;
using System.Threading.Tasks;
using static Quest2_VRC.Program;

namespace Quest2_VRC
{
    internal class OSC
    {
        public static void StartOSC(bool sender, bool receiver)
        {
            if (receiver == false && sender == true)
            {
                Console.Title = "Tx Only";
                Console.WriteLine("You cannot enable data transfer with --no-adb, exiting");
                Handler(CtrlType.CTRL_CLOSE_EVENT);
            }
            else if (receiver == true && sender == false)
            {
                Console.Title = "Rx Only";
                Console.WriteLine("OSC transfer is inactive");
                Console.WriteLine("OSC receiver is active");
                var tasks = new[]
                {
                    Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning),
                };
            }
            else
            {
                Console.Title = "Tx + Rx";
                Console.WriteLine("You cannot enable data transfer with --no-adb, exiting");
                Handler(CtrlType.CTRL_CLOSE_EVENT);
            }
        }
    }
}
