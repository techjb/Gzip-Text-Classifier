namespace gzip_text_classifier_lib
{
    public struct GzipClassifierOptions
    {
        public string TrainFile { get; set; } = string.Empty;

        public bool UseParallelism { get; set; } = true;

        public int K { get; set; } = 10;

        public int TextColumn { get; set; } = 0;

        public int LabelColumn { get; set; } = 1;

        public bool ConsoleOutput { get; set; } = true;

        public GzipClassifierOptions()
        {

        }
    }
}
