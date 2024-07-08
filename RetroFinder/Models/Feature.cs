
namespace RetroFinder.Models
{
    public class Feature
    {
        public FeatureType Type { get; set; }
        public int Score { get; init; } 
        public (int start, int end) Location { get; set; }
    }
}
