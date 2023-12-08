using System;
using System.Management;
using System.Runtime.Versioning;

namespace Quest2_VRC
{

    public class Device_Management
    {

        private static readonly string DeviceName = "Oculus Composite ADB Interface";
        [SupportedOSPlatform("windows")]
        public static bool CheckDevice()
        {

            bool deviceConnected = false;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where Caption='Oculus Composite ADB Interface'");
            foreach (ManagementObject hmd in searcher.Get())
            {
                foreach (PropertyData prop in hmd.Properties)
                {
                    if (Convert.ToString(prop.Value).Contains(DeviceName))
                    {
                        deviceConnected = true;
                        break;
                    }
                }

            }
            return deviceConnected;
        }
        


    }
}
