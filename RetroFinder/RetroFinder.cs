using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RetroFinder.IO;
using RetroFinder.Output;

namespace RetroFinder
{
    public class RetroFinder
    {
        public ExportFormat Formats { get; set; }
        public int ParallelLimit { get; set; }

        private readonly List<ISerializer> _serializers = [];

        public RetroFinder(ExportFormat formats, int parallelLimit)
        {
            if (formats.HasFlag(ExportFormat.JSON)) 
                _serializers.Add(new SerializerJson());
            if (formats.HasFlag(ExportFormat.XML)) 
                _serializers.Add(new SerializerXml());

            ParallelLimit = parallelLimit;
        }
        
        public void Analyze(string path)
        {
            if (!FastaUtils.Validate(path))
                return;

            var sequences = FastaUtils.Parse(path);
            Parallel.ForEach(sequences, 
                new ParallelOptions {MaxDegreeOfParallelism = ParallelLimit}, 
                seq =>
            {
                try
                {
                    SequenceAnalysis analysis = new() { Sequence = seq };
                    analysis.Analyze();

                    foreach (var serializer in _serializers)
                        serializer.SerializeAnalysisResult(analysis);
                }
                catch (Exception e)
                {
                    Writer.AnalysisError(seq.Id, e);
                }
            });
        }
    }
}
