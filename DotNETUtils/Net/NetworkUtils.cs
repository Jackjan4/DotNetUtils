using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;



namespace Roslan.DotNetUtils.Net {



    /// <summary>
    /// 
    /// </summary>
    public static class NetworkUtils {



        /// <summary>
        /// Enumerates all network interfaces and gets their unicast addresses
        /// (The method is taken from Bibliothek.dll from Brandgroup (Volker Niemeyer), cleaned up and converted to C#)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAllIpAddresses() {

            var addresses = NetworkInterface.GetAllNetworkInterfaces()
                .Where(itf =>
                    itf.OperationalStatus == OperationalStatus.Up &&
                    itf.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(itf => itf.GetIPProperties().UnicastAddresses)
                .Where(ipAddr => ipAddr.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(ipAddr => ipAddr.Address.ToString());

            return addresses;
        }



        /// <summary>
        /// Pings a server.
        /// </summary>
        /// <param name="hostNameOrIpAddress">The DNS hostname or the IP address of the server that should be pinged.</param>
        /// <param name="timeout">The timeout after which the ping should fail.</param>
        /// <param name="dontFragment">Sets whether the packet should be allowed to be fragmented.</param>
        /// <param name="ttl">The number of gateways the packet is allowed to surpass.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is null or is an empty string ("").</exception>
        /// <exception cref="InvalidOperationException">A call to SendAsync(String, Object) method is already in progress.</exception>
        /// <exception cref="PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
        /// <exception cref="SocketException">hostNameOrAddress could not be resolved to a valid IP address</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static PingReply Ping(string hostNameOrIpAddress, int timeout = 5000, bool dontFragment = true, int ttl = 128) {
            using (var ping = new Ping()) {
                var pingOptions = new PingOptions {
                    DontFragment = dontFragment,
                    Ttl = ttl
                };

                var result = ping.Send(hostNameOrIpAddress, timeout);
                return result;
            }
        }



        /// <summary>
        /// Pings a server asynchronously.
        /// </summary>
        /// <param name="hostNameOrIpAddress">The DNS hostname or the IP address of the server that should be pinged.</param>
        /// <param name="timeout">The timeout after which the ping should fail.</param>
        /// <param name="dontFragment">Sets whether the packet should be allowed to be fragmented.</param>
        /// <param name="ttl">The number of gateways the packet is allowed to surpass.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">hostNameOrAddress is null or is an empty string ("").</exception>
        /// <exception cref="InvalidOperationException">A call to SendAsync(String, Object) method is already in progress.</exception>
        /// <exception cref="PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
        /// <exception cref="SocketException">hostNameOrAddress could not be resolved to a valid IP address</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static async Task<PingReply> PingAsync(string hostNameOrIpAddress, int timeout = 5000, bool dontFragment = true, int ttl = 128) {
            using (var ping = new Ping()) {
                var pingOptions = new PingOptions {
                    DontFragment = dontFragment,
                    Ttl = ttl
                };

                var result = await ping.SendPingAsync(hostNameOrIpAddress, timeout);
                return result;
            }
        }
    }
}

