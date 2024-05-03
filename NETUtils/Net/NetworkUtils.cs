using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace De.JanRoslan.NETUtils.Net {
    public static class NetworkUtils {



        /// <summary>
        /// Enumerates all network interfaces and gets their unicast addresses
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <param name="localIp"></param>
        /// <returns></returns>
        public static List<string> GetAllIpAddresses() {
            List<string> result = new List<string>();

            // Accessing network can cause exceptions. Caller of this method has to handle them.

            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces()) {
                if (netInterface.OperationalStatus == OperationalStatus.Up && netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback) {
                    foreach (UnicastIPAddressInformation ipAddr in netInterface.GetIPProperties().UnicastAddresses) {
                        if (ipAddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                            result.Add(ipAddr.Address.ToString());
                        }
                    }
                }
            }
            return result;
        }
    }
}

