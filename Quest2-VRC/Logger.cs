using System;
using System.Text;
using static Quest2_VRC.VRCProgram;

namespace Quest2_VRC
{
    internal class Logger
    {
        static public void LogToConsole(string Message, params VRChatMessage[] Parameters)
        {
            StringBuilder MessageBuilder = new StringBuilder();

            MessageBuilder.Append(String.Format("{0} - {1}", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss.ffff"), Message));

            if (Parameters.Length > 0)
            {
                MessageBuilder.Append(" (");

                var LastParam = Parameters[Parameters.Length - 1];
                foreach (var Parameter in Parameters)
                {
                    MessageBuilder.Append(String.Format("{0} of type {1}", Parameter.Data, Parameter.Data.GetType()));

                    if (Parameter != LastParam)
                        MessageBuilder.Append(", ");
                }

                MessageBuilder.Append(")");
            }

            Console.WriteLine(MessageBuilder.ToString());
        }
    }
}
