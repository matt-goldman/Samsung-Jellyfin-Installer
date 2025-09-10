using Samsung_Jellyfin_Installer.Shared.Models;
using Samsung_Jellyfin_Installer.Shared.Services;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace DesktopUI.Services
{
    public class MauiNetworkService : INetworkService
    {
        public async Task<IEnumerable<NetworkDevice>> GetLocalTizenAddresses(CancellationToken cancellationToken = default, bool virtualScan = false)
        {
            var devices = new List<NetworkDevice>();

            try
            {
                // Get local IP addresses to determine the network range
                var localIPs = GetRelevantLocalIPs(virtualScan);
                
                foreach (var localIP in localIPs)
                {
                    await ScanNetworkRange(localIP, devices, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning for Tizen devices: {ex.Message}");
            }

            return devices;
        }

        public async Task<NetworkDevice?> ValidateManualTizenAddress(string ip, CancellationToken cancellationToken = default)
        {
            try
            {
                if (IPAddress.TryParse(ip, out var address))
                {
                    // Try to connect to common Tizen ports
                    if (await IsPortOpenAsync(ip, 26101, cancellationToken)) // Tizen SDB port
                    {
                        return new NetworkDevice
                        {
                            IpAddress = ip,
                            DeviceName = "Manual Tizen Device"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error validating manual Tizen address {ip}: {ex.Message}");
            }

            return null;
        }

        public IEnumerable<IPAddress> GetRelevantLocalIPs(bool virtualScan = false)
        {
            var relevantIPs = new List<IPAddress>();

            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                
                foreach (var networkInterface in networkInterfaces)
                {
                    if (networkInterface.OperationalStatus != OperationalStatus.Up)
                        continue;

                    if (!virtualScan && (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        networkInterface.Description.ToLower().Contains("virtual") ||
                        networkInterface.Description.ToLower().Contains("vmware") ||
                        networkInterface.Description.ToLower().Contains("virtualbox")))
                        continue;

                    var ipProperties = networkInterface.GetIPProperties();
                    foreach (var address in ipProperties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            relevantIPs.Add(address.Address);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting local IPs: {ex.Message}");
            }

            return relevantIPs;
        }

        public async Task<bool> IsPortOpenAsync(string ip, int port, CancellationToken ct)
        {
            try
            {
                using var client = new TcpClient();
                var connectTask = client.ConnectAsync(ip, port);
                var timeoutTask = Task.Delay(2000, ct); // 2 second timeout

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                
                if (completedTask == connectTask && client.Connected)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Port {port} not open on {ip}: {ex.Message}");
            }

            return false;
        }

        public string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting local IP address: {ex.Message}");
            }

            return "127.0.0.1";
        }

        public string InvertIPAddress(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out var address))
            {
                var bytes = address.GetAddressBytes();
                if (bytes.Length == 4)
                {
                    return $"{bytes[3]}.{bytes[2]}.{bytes[1]}.{bytes[0]}";
                }
            }
            return ipAddress;
        }

        private async Task ScanNetworkRange(IPAddress localIP, List<NetworkDevice> devices, CancellationToken cancellationToken)
        {
            try
            {
                var localBytes = localIP.GetAddressBytes();
                var tasks = new List<Task>();

                // Scan the local network range (assumes /24 subnet)
                for (int i = 1; i < 255; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var targetBytes = new byte[] { localBytes[0], localBytes[1], localBytes[2], (byte)i };
                    var targetIP = new IPAddress(targetBytes).ToString();

                    tasks.Add(ScanDevice(targetIP, devices, cancellationToken));
                    
                    // Limit concurrent operations to avoid overwhelming the network
                    if (tasks.Count >= 20)
                    {
                        await Task.WhenAll(tasks);
                        tasks.Clear();
                    }
                }

                if (tasks.Count > 0)
                {
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning network range: {ex.Message}");
            }
        }

        private async Task ScanDevice(string ip, List<NetworkDevice> devices, CancellationToken cancellationToken)
        {
            try
            {
                // Check for Tizen SDB port
                if (await IsPortOpenAsync(ip, 26101, cancellationToken))
                {
                    var device = new NetworkDevice
                    {
                        IpAddress = ip,
                        DeviceName = "Tizen Device"
                    };

                    lock (devices)
                    {
                        devices.Add(device);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning device {ip}: {ex.Message}");
            }
        }
    }
}
