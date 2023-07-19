using gzip_text_classifier_lib;

namespace gzip_text_classifier_console
{
    internal class Program
    {
        private static DateTime DateStart;
        static void Main(string[] args)
        {
            Start();
            Run();
            End();
        }

        private static void Start()
        {
            DateStart = DateTime.Now;
            string textStarted =
                "STARTED at " + DateStart.ToShortDateString() + " " + DateStart.ToString(@"hh\:mm\:ss") + "\n";
            Console.WriteLine(textStarted);
        }

        private static void Run()
        {
            /* K = 3 => 0.889
             * K = 2 =>
             */
            string trainFile = @"C:\Users\Chus\Downloads\ag_news_train.csv";
            string testFile = @"C:\Users\Chus\Downloads\ag_news_test.csv";

            GzipClassifierOptions gzipClassifierOptions = new()
            {
                TrainFile = trainFile,          // File path for csv train file
                ParallelismOnCalc = true,       // Use paralelism on distance calc. Default: true
                ParallelismOnTestFile = false,  // Use paralelism for each test. Default: false
                K = 2,                          // Value of K in k-nearest-neighbor. Default: 3
                TextColumn = 0,                 // Text column number in csv file. Default: 0
                LabelColumn = 1,                // Label column number in csv file. Default: 1
                HasHeaderRecord = true,         // Csv has header record. Deault: true
                ConsoleOutput = true,           // Output console during file prediction. Default: true
            };

            GzipClassifier gzipClassifier = new(gzipClassifierOptions);
            double result = gzipClassifier.PredictFile(testFile);
            Console.WriteLine(result);

            //string text = "Socialites unite dolphin groups Dolphin groups, or \"pods\", rely on socialites to keep them from collapsing, scientists claim.";
            //var prediction = gzipClassifier.Predict(text);
            //Console.WriteLine(prediction);
        }
        private static void End()
        {
            DateTime dateFinished = DateTime.Now;
            string textFinished =
                "FINISHED at " + dateFinished.ToShortDateString() + " " + dateFinished.ToString(@"hh\:mm\:ss") +
                "\nDuration: " + (dateFinished - DateStart).ToString(@"dd\:hh\:mm\:ss\.fff") + ". ";
            Console.WriteLine("\n" + textFinished);

            Console.Beep(500, 500);
        }
    }
}