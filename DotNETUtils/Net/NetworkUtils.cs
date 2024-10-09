using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
        /// Pings a server.
        /// </summary>
        /// <param name="hostNameOrIpAddress">The DNS hostname or the IP address of the server that should be pinged.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is null or is an empty string ("").</exception>
        /// <exception cref="InvalidOperationException">A call to SendAsync(String, Object) method is already in progress.</exception>
        /// <exception cref="PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
        /// <exception cref="SocketException">hostNameOrAddress could not be resolved to a valid IP address</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static bool Ping(string hostNameOrIpAddress) {
            using (var ping = new Ping()) {
                var pingOptions = new PingOptions {
                    DontFragment = true
                };

                var result = ping.Send(hostNameOrIpAddress);
                return result.Status == IPStatus.Success;
            }
        }



        /// <summary>
        /// Pings a server asynchronously.
        /// </summary>
        /// <param name="hostNameOrIpAddress">The DNS hostname or the IP address of the server that should be pinged.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is null or is an empty string ("").</exception>
        /// <exception cref="InvalidOperationException">A call to SendAsync(String, Object) method is already in progress.</exception>
        /// <exception cref="PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
        /// <exception cref="SocketException">hostNameOrAddress could not be resolved to a valid IP address</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static async Task<bool> PingAsync(string hostNameOrIpAddress) {
            using (var ping = new Ping()) {
                var pingOptions = new PingOptions {
                    DontFragment = true
                };

                var result = await ping.SendPingAsync(hostNameOrIpAddress);
                return result.Status == IPStatus.Success;
            }
        }
    }
}

