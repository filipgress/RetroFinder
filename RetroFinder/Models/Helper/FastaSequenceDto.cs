namespace RetroFinder.Models.Helper;

public class FastaSequenceDto
{
    public string Id { get; set; }
    public string Sequence { get; set; }

    public FastaSequenceDto() {}
    public FastaSequenceDto(FastaSequence o)
    {
        Id = o.Id;
        Sequence = o.Sequence;
    }
}