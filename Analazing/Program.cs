using System.Text.Json.Serialization;
using IronSoftware.Drawing;
using Newtonsoft.Json;
using Color = System.Drawing.Color;

internal class Program
{
    static async Task Main(string[] args) // todo chunksizeAdd
    {
        //var fileName = args[0]; bits
        //var ip = args[1];

        var fileName = "/home/derri/PycharmProjects/pythonProject2/random_bits.txt";
        var ip = "192.168.0.18";
        
        var seq = await File.ReadAllTextAsync(fileName);
        // #1 Подсчёт соотношения 0/1 для файла с данными эксперимента
        var ratioResults = GetNumbersRatio(seq);
        
        // #2 Уд. вес Хэмминга
        var values = GetHammingWeight(seq, 4);
        var averageHammingWeight = AverageHammingWeight(values);
        var minHammingWeight = MinHammingWeight(values);
        var maxHammingWeight = MaxHammingWeight(values);

        // #3 Уд. расстояние Хэмминга
        var hammingDistance = GetHammingDistances(seq, 8000); //todo work a lot of time

        // #4. Тест на повторяемость бит
        var repeatedZeroMaxValue = RepeatedBitsMaxValue(seq, '0');
        var repeatedOneMaxValue = RepeatedBitsMaxValue(seq, '1');
        var repeatedZeroMixValue= RepeatedBitsMinValue(seq,  '0');
        var repeatedOneMixValue= RepeatedBitsMinValue(seq,  '1');
        
        // #5 2D Графический тест
        Test_2D(seq, 4, ip);

        var experimentResultModel = new ExperimentResultModel()
        {
            BoardName = ip,
            RatioResults = ratioResults,
            AverageHammingWeight = averageHammingWeight,
            MaxHammingWeight = maxHammingWeight,
            MinHammingWeight = minHammingWeight,
            HammingDistance = hammingDistance,
            RepeatedZeroMaxValue = repeatedZeroMaxValue,
            RepeatedZeroMinValue = repeatedZeroMixValue,
            RepeatedOneMaxValue = repeatedOneMaxValue,
            RepeatedOneMinValue = repeatedOneMixValue
        };
        
        var jsonString = JsonConvert.SerializeObject(experimentResultModel);
        await File.WriteAllTextAsync($"{ip}.json", jsonString);
        Console.WriteLine("Success!");
    }


    // #1. Подсчёт соотношения 0/1 для файла с данными эксперимента
    static RatioResults GetNumbersRatio(string seq)
    {
        int zeroCount = 0;
        int oneCount = 1;

        for (int i = 0; i < seq.Length; i++)
        {
            if (seq[i] == '0')
                zeroCount++;
            else
                oneCount++;
        }

        var ratioResults = new RatioResults()
        {
            ZeroRatio = (double)zeroCount / seq.Length,
            OneRatio = (double)oneCount / seq.Length
        };

        return ratioResults;
    }


    // #2. Уд. вес Хэмминга
    static List<double> GetHammingWeight(string seq, int chunkSize)
    {
        var values = new List<double>();
        int count = 0;

        int tempChunkSize = chunkSize;
        for (int i = 0; (i < seq.Length || tempChunkSize == 0); i++)
        {
            if (tempChunkSize == 0)
            {
                values.Add((double)count / chunkSize);
                count = 0;

                tempChunkSize = chunkSize;
            }

            if (i < seq.Length && seq[i] == '1')
                count += 1;

            tempChunkSize--;
        }

        return values;
    }

    static double AverageHammingWeight(List<double> values)
    {
        var result = values.Average();
        return result;
    }

    static HammingWeightResult MinHammingWeight(List<double> values)
    {
        var min = values.Min();
        var result = new HammingWeightResult()
        {
            Value = min,
            Index = values.IndexOf(min)
        };
        return result;
    }

    static HammingWeightResult MaxHammingWeight(List<double> values)
    {
        var max = values.Max();
        var result = new HammingWeightResult()
        {
            Value = max,
            Index = values.IndexOf(max)
        };
        return result;
    }

    // #3. Уд. расстояние Хэмминга
    static double GetHammingDistances(string seq, int chunkSize)
    {
        int chunksCount = seq.Length / chunkSize;
        int chunkIndex = 0;

        double result = 0;
        int resultsCounter = 0;
        while (chunkIndex <= chunksCount)
        {
            var j = 1;
            while (j + chunkIndex < chunksCount)
            {
                var i = 0;
                var counter = 0;

                while (i < chunkSize)
                {
                    if (seq[i + chunkIndex * chunkSize] != seq[i + (chunkIndex + j) * chunkSize])
                        counter += 1;
                    i++;
                }

                result += (double)counter / chunkSize;
                resultsCounter++;

                j++;
            }

            chunkIndex++;
        }

        var average = result / resultsCounter;
        return average;
    }

    // #4. Тест на повторяемость бит
    static RepeatedBitsResult RepeatedBitsMaxValue(string seq, char bit)
    {
        int tempStartIndex = -1;
        int tempCount = 0;

        int startIndex = -1;
        int maxCount = 0;

        for (int i = 0; i < seq.Length; i++)
        {
            if (seq[i] == bit)
            {
                if (tempCount == 0)
                    tempStartIndex = i;
                tempCount += 1;
            }
            else
            {
                if (maxCount < tempCount)
                {
                    maxCount = tempCount;
                    startIndex = tempStartIndex;
                }

                tempCount = 0;
                tempStartIndex = -1;
            }
        }

        var result = new RepeatedBitsResult()
        {
            Count = maxCount,
            StartIndex = startIndex,
        };
        return result;
    }

    static RepeatedBitsResult RepeatedBitsMinValue(string seq, char bit)
    {
        int tempStartIndex = -1;
        int tempCount = 0;

        int startIndex = -1;
        int minCount = int.MaxValue;

        for (int i = 0; i < seq.Length; i++)
        {
            if (seq[i] == bit)
            {
                if (tempCount == 0)
                    tempStartIndex = i;
                tempCount += 1;
            }
            else
            {
                if (minCount > tempCount && tempCount != 0)
                {
                    minCount = tempCount;
                    startIndex = tempStartIndex;
                }

                tempCount = 0;
                tempStartIndex = -1;
            }
        }

        if (minCount == int.MaxValue)
            minCount = -1;

        var result = new RepeatedBitsResult()
        {
            Count = minCount,
            StartIndex = startIndex,
        };

        return result;
    }


    // #5. 2D Графический тест
    static void Test_2D(string seq, int chunkSize, string fileName)
    {
        var chunksCount = seq.Length / chunkSize;
        var imgSize = (int)Math.Pow(2, chunkSize);
        var chunkIndex = 1;

        var bitmap = new AnyBitmap(imgSize, imgSize, IronSoftware.Drawing.Color.White);
        var y = 0;
        var i = chunkSize - 1;
        var temp = 1;
        char value;

        while (i >= 0)
        {
            value = seq[i];
            if (value == '1')
                y += temp;
            temp *= 2;
            i--;
        }

        while (chunkIndex < chunksCount)
        {
            i = chunkSize - 1;

            int x = y;
            y = 0;
            temp = 1;

            while (i >= 0)
            {
                value = seq[i + chunkIndex * chunkSize];
                if (value == '1')
                {
                    y += temp;
                }

                i -= 1;
                temp *= 2;
            }

            bitmap.SetPixel(x, y, Color.Black);
            chunkIndex += 1;
        }

        bitmap.SaveAs($"{fileName}_2D.png");
    }


    // #6. 3D Графический тест //todo добавить работу с графиком
    // void Test_3D(string seq, int chunkSize)
    // {
    //     var arrayX = new List<int>();
    //     var arrayY = new List<int>();
    //     var arrayZ = new List<int>();
    //     
    //     var widthArray =  new List<int>();
    //     var zeroArray = new List<int>();
    //     
    //     var chunksCount = seq.Length / chunkSize;
    //     var imgSize = (int)Math.Pow(2, chunkSize);
    //     
    //   
    //     var chunkIndex = 2;
    //
    //     var y = 0;
    //     var i = chunkSize - 1;
    //     var temp = 1;
    //     while (i>=0)
    //     {
    //         var value = seq[i];
    //         if (value == '1')
    //             y += temp;
    //         temp *= 2;
    //         i -= 1;
    //     }
    //
    //     var z = 0;
    //     i = chunkSize * 2 - 1;
    //     temp = 1;
    //     while (i >= chunkSize)
    //     {
    //         var value = seq[i];
    //         if (value == '1')
    //             z += temp;
    //         temp *= 2;
    //         i -= 1;
    //     }
    //
    //     while (chunkIndex < chunksCount)
    //     {
    //         i = chunkSize - 1;
    //
    //         var x = y;
    //         y = z;
    //         z = 0;
    //         temp = 1;
    //
    //         while (i >= 0)
    //         {
    //             var value = seq[i + (chunkIndex) * chunkSize];
    //             if (value == '1')
    //                 z += temp;
    //             i -= 1;
    //             temp *= 2;
    //         }
    //
    //         arrayX.Add(x);
    //         arrayY.Add(y);
    //         arrayZ.Add(z);
    //
    //         widthArray.Add(1);
    //         zeroArray.Add(0);
    //         
    //         chunkIndex += 1;
    //     }
    // }
    // 

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
    
    public class ExperimentResultModel
    {
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
}

