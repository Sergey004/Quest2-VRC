using System;
using System.Threading.Tasks;


namespace Quest2_VRC
{
    public class OSC
    {
        public static void StartOSC(bool sender, bool receiver)
        {
            if (receiver == false && sender == true)
            {

                Console.WriteLine("You cannot enable data transfer with --no-adb, exiting");
                
            }
            else if (receiver == true && sender == false)
            {

                Console.WriteLine("OSC transfer is inactive");
                Console.WriteLine("OSC receiver is active");
                var tasks = new[]
                {
                    Task.Factory.StartNew(() => Receiver.Run(), TaskCreationOptions.LongRunning),
                };
            }
            else
            {

                Console.WriteLine("You cannot enable data transfer with --no-adb, exiting");
                
            }
        }
    }
}
