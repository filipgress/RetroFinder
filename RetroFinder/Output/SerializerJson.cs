using System.IO;
using System.Text.Json;
using RetroFinder.Domains;
using RetroFinder.IO;
using RetroFinder.Models.Helper;

namespace RetroFinder.Output;

public class SerializerJson : ISerializer
{
    private static JsonSerializerOptions Options { get; set; } = new()
    {
        IncludeFields = true,
        WriteIndented = true
    };
        
    public void SerializeAnalysisResult(SequenceAnalysis analysis)
    {
        var analysisDto = new SequenceAnalysisDto(analysis);
        
        var filename = analysis.Sequence.Id.ToLower().Replace(" ", "_") + ".json";
        var filepath = Path.Combine(DomainFinder.DataDirectory, filename);

        var json = JsonSerializer.Serialize(analysisDto, Options);
        
        try
        {
            File.WriteAllText(filepath, json);
        }
        catch (IOException e)
        {
            Writer.IoError(filepath, e);
        }
    }
}