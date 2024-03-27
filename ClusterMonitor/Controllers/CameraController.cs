using ClusterMonitor.Models.Camera;
using ClusterMonitor.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterMonitor.Controllers;

[Route("[controller]/[action]")]
public class CameraController: Controller
{
    private readonly ICameraService _cameraService;
    public CameraController(ICameraService cameraService)
    {
        _cameraService = cameraService;
    }
    
    [HttpPost]
    public async Task On()
    {
        await _cameraService.On();
    }
    
    [HttpPost]
    public async Task Off()
    {
        await _cameraService.Off();
    }
    
    [HttpPost]
    public async Task SetPoint([FromBody] float value)
    {
        await _cameraService.SetPoint(value);
    }
    
    [HttpGet]
    public async Task<CameraState> SetPoint()
    {
        var response = await _cameraService.CheckStatus();
        return response;
    }
}