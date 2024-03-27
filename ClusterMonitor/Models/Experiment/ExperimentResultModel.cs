namespace ClusterMonitor.Models;

public class ExperimentResultModel
{
    public int ExperimentNumber { get; set; } //todo  is need
    public string BoardName { get; set; }
    public RatioResults RatioResults { get; set; }
    
    public double AverageHammingWeight { get; set; }
    public HammingWeightResult MinHammingWeight { get; set; }
    public HammingWeightResult MaxHammingWeight { get; set; }
    
    public double HammingDistance { get; set; }
    
    public RepeatedBitsResult RepeatedZeroMaxValue { get; set; }
    public RepeatedBitsResult RepeatedOneMaxValue { get; set; }
    
    public RepeatedBitsResult RepeatedZeroMinValue { get; set; }
    public RepeatedBitsResult RepeatedOneMinValue { get; set; }
    
}

public class RatioResults
{
    public double ZeroRatio { get; set; }
    public double OneRatio { get; set; }
}

public class HammingWeightResult
{
    public double Value { get; set; }
    public int Index { get; set; }
}

public class RepeatedBitsResult
{
    public int Count { get; set; }
    public int StartIndex { get; set; }
}