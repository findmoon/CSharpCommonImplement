namespace System.IO
{
    class WatcherCopyModel
    {
        public string Name { get; set; }
        public string SourceFile { get; set; }
        public string TargetFile { get; set; }
        public bool OverWriteExist { get; set; }
    }
}
