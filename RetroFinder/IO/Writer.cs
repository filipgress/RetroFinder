using System;

namespace RetroFinder.IO;

public static class Writer
{
    private const string Example = 
        "Usage: RetroFinder <filepath> <parallelization limit> <export formats>[JSON, XML] ";
        
    public static void InvalidArgsCount(int argsCount)
    {
        Console.WriteLine( $"[Error] Invalid number of arguments {argsCount}");
        Console.WriteLine(Example);
    }

    public static void InvalidLimit(string arg)
    {
        Console.WriteLine( $"[Error] Invalid format of limit '{arg}'. Expected number.");
        Console.WriteLine(Example);
    }

    public static void InvalidExportFormat(string arg)
    {
        Console.WriteLine( $"[Error] Invalid export format '{arg}'.");
        Console.WriteLine(Example);
    }

    public static void InvalidFilepath(string path)
    {
        Console.WriteLine($"[Error] Cannot find file '{path}'.");
    }

    public static void InvalidFile(Exception e)
    {
        Console.WriteLine($"[Error] Invalid file. {e.Message}");
    }

    public static void InvalidFastaSequence(string id)
    {
        Console.WriteLine($"Invalid sequence with id '{id}'");
    }
    
    public static void InvalidFastaId(string id)
    {
        Console.WriteLine($"Invalid id '{id}'");
    }

    public static void InvalidFastaMissingId()
    {
        Console.WriteLine("File is missing id.");
    }

    public static void IoError(string path, Exception e)
    {
        Console.WriteLine($"[Error] Unable to read/write to file '{path}'. {e.Message}");
    }
    
    public static void AnalysisError(string id, Exception e)
    {
        Console.WriteLine($"[Error] Unable to analyze sequence with id '{id}'. {e}");
    }
}