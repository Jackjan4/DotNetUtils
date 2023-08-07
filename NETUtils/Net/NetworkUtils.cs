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
        /// Returns the local IPv4 address as string.
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpv4() {
            string result = null;

            // Accessing network can cause exceptions. Caller of this method has to handle them.

            List<string> ipAddresses = new List<string>();

            int i = 0;
            foreach (IPAddress ipAddr in Dns.GetHostAddresses(Dns.GetHostName())) {
                string currentIpAddr = Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].ToString();

                if (!currentIpAddr.Contains(":"))
                    ipAddresses.Add(currentIpAddr);
                i += 1;
            }

            if (ipAddresses.Count > 0)
                result = ipAddresses[0];

            return result;
        }


        /// <summary>
        /// Enumerates all network interfaces and gets their unicast addresses
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <param name="localIp"></param>
        /// <returns></returns>
        public static List<string> GetAllIpAddresses(out string localIp) {
            localIp = null;
            List<string> result = new List<string>();

            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces()) {
                if (netInterface.OperationalStatus == OperationalStatus.Up && netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback) {
                    foreach (UnicastIPAddressInformation ipAddr in netInterface.GetIPProperties().UnicastAddresses) {
                        if (ipAddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {

                            if (!ipAddr.Address.ToString().StartsWith("192.168")) {
                                localIp = ipAddr.Address.ToString();
                            }
                            result.Add(ipAddr.Address.ToString());
                        }
                    }
                }
            }
            return result;
        }
    }
}

