using System.IO;
using System.Xml.Serialization;
using RetroFinder.Domains;
using RetroFinder.IO;
using RetroFinder.Models.Helper;

namespace RetroFinder.Output;

public class SerializerXml : ISerializer
{
    public void SerializeAnalysisResult(SequenceAnalysis analysis)
    {
        var analysisDto = new SequenceAnalysisDto(analysis);
        
        var filename = analysis.Sequence.Id.ToLower().Replace(" ", "_") + ".xml";
        var filepath = Path.Combine(DomainFinder.DataDirectory, filename);

        var serializer = new XmlSerializer(typeof(SequenceAnalysisDto));

        try
        {
            using var writer = new StreamWriter(filepath);
            serializer.Serialize(writer, analysisDto);
        }
        catch (IOException e)
        {
            Writer.IoError(filepath, e);
        }
    }
}