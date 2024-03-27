using ClusterMonitor.Models.Cluster;

namespace ClusterMonitor.Services;

public interface IClusterService
{
    Task<List<ClusterItem>> GetClusterItems();
}

public class ClusterService : IClusterService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<ClusterItem>> GetClusterItems() //todo change to socket
    {
        var clusterItems = await _httpClient.GetFromJsonAsync<List<ClusterItem>>("url");
        if (clusterItems == null)
            return new List<ClusterItem>();
        return clusterItems;
    }
}