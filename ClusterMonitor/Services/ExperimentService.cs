using ClusterMonitor.Models;
using ClusterMonitor.Models.Experiment;
using Newtonsoft.Json;

namespace ClusterMonitor.Services;

public interface IExperimentService
{
    Task<ExperimentResultModel?> GetLastExperimentResult(string fileName);
    Task<List<HistoryItemModel>> GetExperimentsHistory();
    Task<List<ExperimentResultModel>> GetExperimentResults(int experimentNumber);
}

public class ExperimentService : IExperimentService
{
    private const string RootPath = "/mnt/experiments_data";

    public async Task<ExperimentResultModel?> GetLastExperimentResult(string boardName)
    {
        var directoryInfo = new DirectoryInfo(RootPath);
        var directories = directoryInfo.GetDirectories("*", SearchOption.AllDirectories)
            .OrderByDescending(x => x.LastWriteTime).ToArray();
        if (directories.Length == 0)
            return null;

        for (int i = 0; i < directories.Length; i++)
        {
            var lastEditedDirectory = directories[i];
            var fullPath = $"{lastEditedDirectory}/{boardName}.json";
            if (!File.Exists(fullPath))
                continue;
            var content = await File.ReadAllTextAsync(fullPath);

            var experimentResultModel = JsonConvert.DeserializeObject<ExperimentResultModel>(content);
            return experimentResultModel;
        }

        return null;
    }

    public async Task<List<HistoryItemModel>> GetExperimentsHistory()
    {
        var directoryInfo = new DirectoryInfo(RootPath);
        var directories = directoryInfo.GetDirectories("*", SearchOption.AllDirectories)
            .OrderByDescending(x => x.LastWriteTime).ToArray();

        var history = new List<HistoryItemModel>();
        foreach (var directory in directories)
        {
            var historyItem = new HistoryItemModel();
            historyItem.ExperimentNumber = Int32.Parse(directory.Name);
            historyItem.LastWriteTime = directory.LastWriteTime;
            historyItem.Boards = Directory.GetFiles(directory.FullName, "*.json")
                .Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            history.Add(historyItem);
        }

        history = history.OrderBy(x => x.ExperimentNumber).ToList();
        return history;
    }

    public async Task<List<ExperimentResultModel>> GetExperimentResults(int experimentNumber)
    {
        var path = $"{RootPath}/{experimentNumber}";
        var files = new DirectoryInfo(path).GetFiles("*.json");
        
        var experimentResults = new List<ExperimentResultModel>();
        foreach (var file in files)
        {
            var content = await File.ReadAllTextAsync(file.FullName);
            var experimentResultModel = JsonConvert.DeserializeObject<ExperimentResultModel>(content);
            experimentResultModel.ExperimentNumber = experimentNumber;
            experimentResults.Add(experimentResultModel);
        }
        return experimentResults;
    }

    
}

//todo pagination in history
