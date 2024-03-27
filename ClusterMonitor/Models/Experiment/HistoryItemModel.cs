namespace ClusterMonitor.Models.Experiment;

public class HistoryItemModel
{
    public int ExperimentNumber { get; set; }
    public DateTime LastWriteTime { get; set; }
    public List<string> Boards { get; set; }
}