using System.Text.Json.Serialization;

namespace RetroFinder.Models.Helper;

public class FeatureDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public FeatureType Type { get; set; }
    
    public int Start { get; set; }
    public int End { get; set; }

    public FeatureDto() {}
    public FeatureDto(Feature o)
    {
        Type = o.Type;
        
        Start = o.Location.start;
        End = o.Location.end;
    }
}