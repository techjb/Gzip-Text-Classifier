using gzip_text_classifier_lib;

namespace gzip_text_classifier_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string trainFile = @"C:\Users\Chus\Downloads\ag_news_train.csv";
            string testFile = @"C:\Users\Chus\Downloads\ag_news_test.csv";

            GzipClassifierOptions gzipClassifierOptions = new()
            {
                TrainFile = trainFile,  // Path for csv train file
                UseParallelism = true,  // User paralelism. Default: true
                K = 3,                  // Value of K in k-nearest-neighbor. Default: 3
                TextColumn = 0,         // Text column number in csv file. Default: 0
                LabelColumn = 1,        // Label column number in csv file. Default: 1
                HasHeaderRecord = true, // Csv has header record. Deault: true
                ConsoleOutput = true,   // Output console during file prediction. Default: true
            };

            GzipClassifier gzipClassifier = new(gzipClassifierOptions);
            double result = gzipClassifier.PredictFile(testFile);
            Console.WriteLine(result);

            string text = "Socialites unite dolphin groups Dolphin groups, or \"pods\", rely on socialites to keep them from collapsing, scientists claim.";
            var prediction = gzipClassifier.Predict(text);
            Console.WriteLine(prediction);
        }
    }
}