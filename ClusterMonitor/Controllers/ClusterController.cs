using ClusterMonitor.Models.Cluster;
using ClusterMonitor.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterMonitor.Controllers;

[Route("[controller]/[action]")]
public class ClusterController:Controller
{
    private readonly IClusterService _clusterService;
    public ClusterController(IClusterService clusterService)
    {
        _clusterService = clusterService;
    }

    [HttpGet]
    public async Task<List<ClusterItem>> GetClusterItems()
    {
       var model = await _clusterService.GetClusterItems();
       return model;
    }
}