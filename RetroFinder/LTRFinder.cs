using RetroFinder.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RetroFinder
{
    public class LTRFinder
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> IdentifyElements()
        {
            var transposons = new List<Transposon>();
            
            const string ltrPattern = "?<LTR>[ACGT]{100,300}";
            const string fullPattern = $"({ltrPattern}).{{1000,3500}}\\1";

            var matches = Regex.Matches(Sequence.Sequence, fullPattern);
            foreach (Match match in matches)
            {
                var ltrSize = match.Groups["LTR"].Length;
                transposons.Add( new Transposon 
                {
                    Location = (match.Index, match.Index + match.Length),
                    Features = new List<Feature>()
                    {
                        new()
                        {
                            Type = FeatureType.LTRLeft,
                            Location = (match.Index, match.Index + ltrSize),
                        },
                        new()
                        {
                            Type = FeatureType.LTRRight,
                            Location = (match.Index + match.Length - ltrSize, match.Index + match.Length),
                        }
                    }
                });
            }

            return transposons;
        }
    }
}
