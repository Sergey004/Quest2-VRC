using System;
using System.Management;
using System.Runtime.Versioning;

namespace Quest2_VRC
{

    public class Device_Management
    {

        private static readonly string DeviceName = "Oculus Composite ADB Interface";
        private static readonly string FallbackName = "ADB Interface";
        [SupportedOSPlatform("windows")]
        public static bool CheckDevice()
        {

            bool deviceConnected = false;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where Caption='Oculus Composite ADB Interface'");
            ManagementObjectSearcher fallback = new ManagementObjectSearcher("select * from Win32_PnPEntity where Caption='ADB Interface'");

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
            foreach (ManagementObject hmd in fallback.Get())
            {
                foreach (PropertyData prop in hmd.Properties)
                {
                    if (Convert.ToString(prop.Value).Contains(FallbackName))
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
