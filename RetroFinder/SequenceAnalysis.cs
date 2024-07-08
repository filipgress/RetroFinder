using RetroFinder.Models;
using System.Collections.Generic;
using System.Linq;
using RetroFinder.Domains;

namespace RetroFinder
{
    public class SequenceAnalysis
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> Transposons { get; set; }

        public void Analyze()
        {
            var ltrFinder = new LTRFinder() { Sequence = Sequence };
            Transposons = ltrFinder.IdentifyElements();

            foreach (var trans in Transposons)
            {
                var (start, end) = trans.Location;
                var domainFinder = new DomainFinder
                {
                    InnerSeq = Sequence.Sequence[start..end],
                    Offset = trans.Location.start
                };

                var domains = domainFinder.IdentifyDomains().ToList();
                domains.AddRange(trans.Features);

                var domainPicker = new DomainPicker(domains);
                trans.Features = domainPicker.PickDomains();
            }
        }
    }
}
