# Gzip Text Classifier

This project is a C# implementation of the Normalized Compression Distance (NCD) 
classification algorithm with Gzip compression, which can be read about in [this paper](https://aclanthology.org/2023.findings-acl.426/).

The original repository written in Python, can be found at [this link](https://github.com/bazingagin/npc_gzip).

Also available as a [Nuget package](https://www.nuget.org/packages/Gzip.TextClassifier/).

## Usage

Predict csv test file:

```cs

string trainFile = @"C:\Users\Chus\Downloads\ag_news_train.csv";
string testFile = @"C:\Users\Chus\Downloads\ag_news_test.csv";

GzipClassifierOptions gzipClassifierOptions = new()
{
    TrainFile = trainFile,          // File path for csv train file
    ParallelismOnCalc = true,       // Use paralelism on distance calc. Default: true
    ParallelismOnTestFile = false,  // Use paralelism for each test. Default: false
    K = 3,                          // Value of K in k-nearest-neighbor. Default: 3
    TextColumn = 0,                 // Text column number in csv file. Default: 0
    LabelColumn = 1,                // Label column number in csv file. Default: 1
    HasHeaderRecord = true,         // Csv has header record. Deault: true
    ConsoleOutput = true,           // Output console during file prediction. Default: true
};

GzipClassifier gzipClassifier = new(gzipClassifierOptions);
double result = gzipClassifier.PredictFile(testFile);
Console.WriteLine(result);

```

Single text prediction:
```cs
string text = "Socialites unite dolphin groups Dolphin groups, or \"pods\", rely on socialites to keep them from collapsing, scientists claim.";
var prediction = gzipClassifier.Predict(text);
Console.WriteLine(prediction);    
```
