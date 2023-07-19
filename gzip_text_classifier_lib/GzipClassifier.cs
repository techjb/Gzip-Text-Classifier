using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace gzip_text_classifier_lib
{
    public class GzipClassifier
    {
        private readonly GzipClassifierOptions GzipClassifierOptions;
        private readonly List<Tuple<string, string, long>> TrainingList = new();
        private List<Tuple<double, string>> distanceFromX1 = new();
        private int Total = 0;
        private int Processed = 0;
        private int Sucess = 0;

        public GzipClassifier(GzipClassifierOptions classifierConfiguration)
        {
            GzipClassifierOptions = classifierConfiguration;
            var trainList = CsvToList(GzipClassifierOptions.TrainFile);
            InitializeTrainingList(trainList);
        }

        public double PredictFile(string testFile)
        {
            var testList = CsvToList(testFile);
            Total = testList.Count;
            Processed = 0;
            Sucess = 0;

            foreach (var test in testList)
            {
                string predictedClass = Predict(test.Item1);
                Processed++;
                if (predictedClass.Equals(test.Item2.Trim()))
                {
                    Sucess++;
                }
                if (GzipClassifierOptions.ConsoleOutput)
                {
                    OutputConsole();
                }
            }

            return GetSucessRatio();
        }

        private void OutputConsole()
        {
            var successRatio = GetSucessRatio();
            var processedPercentage = Math.Round((float)Processed * 100 / Total, 2);

            Console.WriteLine(
                "Processed: " + Processed + "/" + Total + " (" + processedPercentage + "%) " +
                "Sucess ratio: " + Sucess + " (" + successRatio + ")");
        }

        private double GetSucessRatio()
        {
            return Math.Round((float)Sucess / Processed, 3);
        }

        private void InitializeTrainingList(List<Tuple<string, string>> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                long compressedLength = GZipLength(item.Item1);
                var tuple = Tuple.Create(item.Item1, item.Item2, compressedLength);
                TrainingList.Add(tuple);
            }
        }

        public string Predict(string x1)
        {
            distanceFromX1 = new();
            long Cx1 = GZipLength(x1);

            if (GzipClassifierOptions.UseParallelism)
            {
                PredictWithParallelism(x1, Cx1);
            }
            else
            {
                PredictWithNoParallelism(x1, Cx1);
            }

            var sortedIdx = distanceFromX1
                .OrderBy(item => item.Item1)
                .Take(GzipClassifierOptions.K)
                .Select(item => item.Item2)
                .ToList();
            var predictedClass = sortedIdx.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
            return predictedClass.Trim();
        }

        private void PredictWithParallelism(string x1, long Cx1)
        {
            var sync = new object();
            Parallel.ForEach(TrainingList,
               //new ParallelOptions() { MaxDegreeOfParallelism = 1 },
               item =>
               {
                   var ncd = CalculateNCD(x1, Cx1, item);
                   lock (sync)
                   {
                       distanceFromX1.Add(ncd);
                   }
               });
        }

        private void PredictWithNoParallelism(string x1, long Cx1)
        {
            foreach (var item in TrainingList)
            {
                var ncd = CalculateNCD(x1, Cx1, item);
                distanceFromX1.Add(ncd);
            }
        }

        private Tuple<double, string> CalculateNCD(string x1, long Cx1, Tuple<string, string, long> item)
        {
            var x2 = item.Item1;
            long Cx2 = item.Item3;
            string x1x2 = string.Join(" ", x1, x2);
            long Cx1x2 = GZipLength(x1x2);

            double ncd = (double)(Cx1x2 - Math.Min(Cx1, Cx2)) / Math.Max(Cx1, Cx2);
            return Tuple.Create(ncd, item.Item2);
        }

        private List<Tuple<string, string>> CsvToList(string file)
        {
            using var streamReader = new StreamReader(file);
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = null,
                HasHeaderRecord = GzipClassifierOptions.HasHeaderRecord,
            };
            using var csvReader = new CsvReader(streamReader, csvConfiguration);
            if (GzipClassifierOptions.HasHeaderRecord)
            {
                csvReader.Read();
            }
            var records = new List<Tuple<string, string>>();
            while (csvReader.Read())
            {
                var text = csvReader.GetField<string>(GzipClassifierOptions.TextColumn);
                var label = csvReader.GetField<string>(GzipClassifierOptions.LabelColumn);

                if (text == null || label == null)
                {
                    continue;
                }
                Tuple<string, string> record = Tuple.Create(text, label);
                records.Add(record);
            }
            return records;
        }

        private long GZipLength(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }
            return memoryStream.ToArray().Length;
        }
    }
}