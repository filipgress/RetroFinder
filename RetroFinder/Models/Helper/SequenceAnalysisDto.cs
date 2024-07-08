
using System.Collections.Generic;

namespace RetroFinder.Models.Helper;

public class SequenceAnalysisDto
{
    public List<TransposonDto> Transposons { get; set; } = [];

    public SequenceAnalysisDto() {}
    public SequenceAnalysisDto(SequenceAnalysis o)
    {
            foreach (var trans in o.Transposons)
                    Transposons.Add(new TransposonDto(trans));
    }
}