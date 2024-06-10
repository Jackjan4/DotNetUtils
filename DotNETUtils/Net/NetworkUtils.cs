using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Roslan.DotNetUtils.Net {
    public static class NetworkUtils {



        /// <summary>
        /// Enumerates all network interfaces and gets their unicast addresses
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllIpAddresses() {
            var result = new List<string>();

            // Accessing network can cause exceptions. Caller of this method has to handle them.
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces()) {

                // Skip loopback interfaces and interfaces that are not up
                if (netInterface.OperationalStatus != OperationalStatus.Up || netInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                foreach (UnicastIPAddressInformation ipAddr in netInterface.GetIPProperties().UnicastAddresses) {
                    if (ipAddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                        result.Add(ipAddr.Address.ToString());
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostNameOrIpAddress"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Test</exception>
        public static bool Ping(string hostNameOrIpAddress) {
            var ping = new Ping();
            var pingOptions = new PingOptions {
                DontFragment = true
            };

            var result = ping.Send(hostNameOrIpAddress);
            return result.Status == IPStatus.Success;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostNameOrIpAddress"></param>
        /// <returns></returns>
        public static async Task<bool> PingAsync(string hostNameOrIpAddress) {
            var ping = new Ping();
            var pingOptions = new PingOptions {
                DontFragment = true
            };

            var result = await ping.SendPingAsync(hostNameOrIpAddress);
            return result.Status == IPStatus.Success;
        }
    }
}

