using System.Runtime.InteropServices.JavaScript;
using ClusterMonitor.Models;
using ClusterMonitor.Models.Experiment;
using ClusterMonitor.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterMonitor.Controllers;

[Route("[controller]/[action]")]
public class ExperimentController:Controller
{
    private IExperimentService _experimentService;
    public ExperimentController(IExperimentService experimentService)
    {
        _experimentService = experimentService;
    }

    [HttpGet]
    public async Task<ExperimentResultModel?> GetLastExperimentResult([FromQuery]string boardName)
    {
      var model = await _experimentService.GetLastExperimentResult(boardName); //todo create page 
      return model;
    }

    [HttpGet]
    public async Task<List<HistoryItemModel>> GetExperimentsHistory()
    {
        var model = await _experimentService.GetExperimentsHistory();
        return model;
    }
    
    [HttpGet]
    public async Task<List<ExperimentResultModel>> GetExperimentResults([FromQuery]int experimentNumber)
    {
        var model =  await _experimentService.GetExperimentResults(experimentNumber);
        return model;
    }
}