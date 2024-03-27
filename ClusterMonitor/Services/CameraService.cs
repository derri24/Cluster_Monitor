using ClusterMonitor.Models.Camera;
using Microsoft.AspNetCore.Mvc;

namespace ClusterMonitor.Services;

public interface ICameraService
{
    Task On();
    Task Off();
    Task SetPoint(float value);
    public Task<CameraState> CheckStatus();
}

public class CameraService : ICameraService
{
    private string _url = "http://localhost:5000/";
    private HttpClient _httpClient;

    public CameraService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_url);
    }

    public async Task On()
    {
        var request = await _httpClient.PostAsync($"Temp/On?", null);
    }

    public async Task Off()
    {
        var request = await _httpClient.PostAsync($"Temp/Off?", null);
    }

    public async Task SetPoint(float value)
    {
        // only with point
        var request = await _httpClient.PostAsync($"Temp/WriteSetPoint?temp={value}", null);
    }
    
    public async Task<CameraState> CheckStatus()
    {
        var request = await _httpClient.GetFromJsonAsync<CameraState>($"Temp/CheckCameraStatus?");
        return request;
    }
    
}