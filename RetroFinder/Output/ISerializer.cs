namespace RetroFinder.Output
{
    public interface ISerializer
    {
        // public const string Folder = "../../../Data";
        void SerializeAnalysisResult(SequenceAnalysis analysis);
    }
}
