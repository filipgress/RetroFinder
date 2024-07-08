#nullable enable

using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RetroFinder.IO;

namespace RetroFinder
{
    public class FastaUtils
    {
        public static bool Validate(string path)
        {
            var ids = new HashSet<string>();
            var dnaRegex = new Regex(@"^[ACGTN]+$");
            var hasSeq = true;

            try
            {
                using var sr = new StreamReader(path);
                while (sr.ReadLine() is { } line)
                {
                    if (line.StartsWith('>')) // id
                    {
                        var id = line[1..].Trim();
                        
                        if (!hasSeq)
                        {
                            Writer.InvalidFastaSequence(ids.Last());
                            return false;
                        }

                        if (string.IsNullOrEmpty(id) || !ids.Add(id))
                        {
                            Writer.InvalidFastaId(id);
                            return false;
                        }
                        
                        hasSeq = false;
                    }
                    
                    else // sequence
                    {
                        if (ids.Count == 0)
                        {
                            Writer.InvalidFastaMissingId();
                            return false;
                        }

                        if (!dnaRegex.IsMatch(line)) {
                            Writer.InvalidFastaSequence(ids.Last());
                            return false;
                        }
                        
                        hasSeq = true;
                    }
                }

                if (ids.Count == 0)
                {
                    Writer.InvalidFastaMissingId();
                    return false;
                }
                
                if (!hasSeq)
                {
                    Writer.InvalidFastaSequence(ids.Last());
                    return false;
                }

                return true;
            }
            catch (IOException e)
            {
                Writer.IoError(path, e);
            }
            catch (Exception e)
            {
                Writer.InvalidFile(e);
            }

            return false;
        }

        public static IEnumerable<FastaSequence> Parse(string path)
        {
            var sequences = new List<FastaSequence>();
            using var sr = new StreamReader(path);

            var id = "";
            var seq = "";
            
            while (sr.ReadLine() is { } line)
            {
                if (line.StartsWith('>')) // id
                {
                    if (id != "")
                        sequences.Add(new FastaSequence(id, seq));

                    id = line[1..].Trim();
                    seq = "";
                }
                
                else // sequence
                {
                    seq += line;
                }
            }
            
            sequences.Add(new FastaSequence(id, seq));
            return sequences;
        }
    }
}
