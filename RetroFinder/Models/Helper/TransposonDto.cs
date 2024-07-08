using System.Collections.Generic;

namespace RetroFinder.Models.Helper;

public class TransposonDto
{
    public int Start { get; set; }
    public int End { get; set; }
    public List<FeatureDto> Features { get; set; } = [];

    public TransposonDto() {}
    public TransposonDto(Transposon o)
    {
        Start = o.Location.start;
        End = o.Location.end;
        
        foreach (var feature in o.Features)
               Features.Add(new FeatureDto(feature)); 
    }
}