using Samsung_Jellyfin_Installer.Shared.Models;
using System.Net;

namespace Samsung_Jellyfin_Installer.Shared.Services;

public interface INetworkService
{
    Task<IEnumerable<NetworkDevice>> GetLocalTizenAddresses(CancellationToken cancellationToken = default, bool virtualScan = false);
    Task<NetworkDevice?> ValidateManualTizenAddress(string ip, CancellationToken cancellationToken = default);
    IEnumerable<IPAddress> GetRelevantLocalIPs(bool virtualScan = false);
    Task<bool> IsPortOpenAsync(string ip, int port, CancellationToken ct);
    string GetLocalIPAddress();
    string InvertIPAddress(string ipAddress);
}
