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
    TrainFile = trainFile,  // Path of csv train file
    UseParallelism = true,  // User paralelism. Default: true
    K = 10,                 // Value of K in k-nearest-neighbor
    TextColumn = 0,         // Text column number in csv file
    LabelColumn = 1,        // Label column number in csv file
    ConsoleOutput = true,   // Output console during file prediction. Default: true
};
GzipClassifier gzipClassifier = new(gzipClassifierOptions);
double result = gzipClassifier.PredictFile(testFile);
Console.WriteLine(result);

```

Predict text:
```cs
string text = "Socialites unite dolphin groups Dolphin groups, or \"pods\", rely on socialites to keep them from collapsing, scientists claim.";
var prediction = gzipClassifier.Predict(text);
Console.WriteLine(prediction);    
```
