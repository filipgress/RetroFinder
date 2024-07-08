using RetroFinder.Models;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainPicker
    {
        private readonly Feature _root;
        private readonly Dictionary<Feature, List<Feature>> _adjacent = new();

        public DomainPicker(IEnumerable<Feature> domains)
        {
            var ordDomains = domains.OrderBy(domain => domain.Type).ToList();
            _root = ordDomains[0];
            
            for (var i = 0; i < ordDomains.Count; i++)
            {
                var curr = ordDomains[i];
                _adjacent[curr] = [];
                
                for (var j = i + 1; j < ordDomains.Count; j++)
                {
                    var next = ordDomains[j];
                    if (next.Type == curr.Type ||
                        curr.Location.end > next.Location.start)
                        continue;

                    _adjacent[curr].Add(next);
                }
            }
        }

        private void GetMaxLenRec( Feature at, int currLen, ref int maxLen)
        {
            if (at.Type == FeatureType.LTRRight)
            {
                if (currLen > maxLen)
                    maxLen = currLen;
                
                return;
            }
            
            foreach (var next in _adjacent[at]) 
                GetMaxLenRec(next, currLen + 1, ref maxLen);
        }

        private int GetMaxLen()
        {
            var maxLen = 0;
            GetMaxLenRec(_root, 1, ref maxLen);
            
            return maxLen;
        }

        private void GetBestFeatures(
            Feature at, int currLen, int maxLen, int currScore, 
            List<Feature> currPath, ref (List<Feature> features, int score) res)
        {
            currPath.Add(at);
            currScore += at.Score;

            if (at.Type == FeatureType.LTRRight)
            {
                if (currLen == maxLen && currScore >= res.score)
                {
                    res.features = [..currPath];
                    res.score = currScore;
                }
            }
            else
            {
                foreach (var next in _adjacent[at])
                    GetBestFeatures(next, currLen + 1, maxLen, currScore, currPath, ref res);
            }
            
            currPath.Remove(at);
        }
        
        public IEnumerable<Feature> PickDomains()
        {
            (List<Feature> features, int score) res = new([], 0);
            GetBestFeatures(_root, 1, GetMaxLen(), 0, [], ref res);
            
            return res.features;
        }
    }
}
