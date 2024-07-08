#nullable enable
using System.IO;
using RetroFinder.IO;

namespace RetroFinder
{
    internal class ProgramProps
    {
        public required string Filepath { get; set; }
        public required int Limit { get; set; }
        public required ExportFormat Formats { get; set; }
    }
    
    class Program
    {
        private static void Main(string[] args)
        {
            var props = HandleArgs(args);
            if (props is null)
                return;

            var finder = new RetroFinder(props.Formats, props.Limit);
            finder.Analyze(props.Filepath);
        }

        private static ProgramProps? HandleArgs(string[] args)
        {
            if (args.Length < 3)
            {
                Writer.InvalidArgsCount(args.Length);
                return null;
            }

            var path = args[0];
            if (!File.Exists(path))
            {
                Writer.InvalidFilepath(path);
            }

            if (!int.TryParse(args[1], out var limit) || limit < 1)
            {
                Writer.InvalidLimit(args[1]);
                return null;
            }

            var formats = HandleFormats(args[2..]);
            if (formats is null)
                return null;

            return new ProgramProps 
                { Filepath = path, Limit = limit, Formats = formats.Value };
        }

        private static ExportFormat? HandleFormats(string[] args)
        {
            var formats = ExportFormat.NONE;
            
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "JSON":
                        formats |= ExportFormat.JSON;
                        break;
                    case "XML":
                        formats |= ExportFormat.XML;
                        break;
                    default:
                        Writer.InvalidExportFormat(arg);
                        return null;
                }
            }
            
            return formats;
        }
    }
    
}
