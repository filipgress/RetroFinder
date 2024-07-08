using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace RetroFinder.Domains
{
    public class DomainFinder
    {
       public static readonly string DataDirectory = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Data");
        
        private static readonly IEnumerable<FastaSequence> Domains = 
            FastaUtils.Parse(Path.Combine(DataDirectory, "protein_domains.fa"));
        // private static readonly IEnumerable<FastaSequence> Domains = 
        //     FastaUtils.Parse("../../../Data/protein_domains.fa");
        
        public required string InnerSeq { get; set; }
        public required int Offset { get; set; }
        
        private const int Match = 3;
        private const int Mismatch = -3;
        private const int Gap = -2;

        private ((int start, int end) location, int len, int score) SmithWaterman(string domain)
        {
            var maxScore = 0;
            (int i, int j) maxPos = (0, 0);
            
            // initialization
            var m = InnerSeq.Length + 1;
            var n = domain.Length + 1;
            
            var h = new ((int i, int j) parent, int score)[m, n];
            for (var i = 0; i < m; i++)
                h[i, 0].score = 0;
            
            for (var j = 0; j < n; j++)
                h[0, j].score = 0;

            // fill matrix
            for (var i = 1; i < m; i++)
            {
                for (var j = 1; j < n; j++)
                {
                    var up = h[i, j - 1].score + Gap;
                    var left = h[i - 1, j].score + Gap;
                    var diag = h[i - 1, j - 1].score +
                               (InnerSeq[i - 1] == domain[j - 1] ? Match : Mismatch);
                    
                    var max = Math.Max(0, Math.Max(diag, Math.Max(up, left)));
                    
                    h[i, j].score = max;
                    h[i, j].parent =
                        max == diag ? (i - 1, j - 1) : 
                        max == up ? (i, j - 1) : (i - 1, j);

                    if (max > maxScore)
                    {
                        maxScore = max;
                        maxPos = (i, j);
                    }
                }
            }
            
            // traceback
            var matchLen = 0;
            var currPos = maxPos;
            
            while (h[currPos.i, currPos.j].score != 0)
            {
                matchLen++;
                currPos = h[currPos.i, currPos.j].parent;
            }

            return ((currPos.i,maxPos.i), matchLen, maxScore);
        }

        private FeatureType GetFeatureType(string id)
        {
            var type = id.Split(' ')[0];
            return type switch
            {
                "GAG" => FeatureType.GAG,
                "PROT" => FeatureType.PROT,
                "INT" => FeatureType.INT,
                "RT" => FeatureType.RT,
                "RH" => FeatureType.RH,
                _ => throw new SystemException($"Unknown feature type {type}")
            };
        }
        
        public IEnumerable<Feature> IdentifyDomains()
        {
            List<Feature> features = [];
            foreach (var domain in Domains)
            {
                var (location, len, score) = 
                    SmithWaterman(domain.Sequence);
                
                if (len < domain.Sequence.Length / 2)
                    continue;
                
                features.Add(new Feature()
                {
                    Location = (location.start + Offset, location.end + Offset),
                    Score = score,
                    Type = GetFeatureType(domain.Id)
                });
            }

            return features;
        }
    }
}
