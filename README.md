# RetroFinder

Tool designed to identify and analyze retrotransposons of the Copia family within DNA sequences provided in FASTA format. The program validates the input sequences, identifies potential retrotransposons based on specific criteria, determines their internal structure by aligning protein domains using the Smith-Waterman algorithm, and outputs the results in either JSON or XML format.

## Usage

### CLI

To run RetroFinder from the command line interface (CLI), use the following syntax:

```bash
dotnet run <path-to-fasta-file> <parallelization-limit> <output-format>
```

Where:
- `<path-to-fasta-file>` is the path to the FASTA file containing DNA sequences.
- `<parallelization-limit>` specifies the maximum number of parallel tasks.
- `<output-format>` specifies the desired output format, which can be `json`, `xml`, or `both`.

Example:

```bash
dotnet run Data/test_sequence.fa 10 JSON XML
```

### Generating Test Sequences

You can generate test sequences of DNA using the `seq_gen.py` script located in the `Data` subfolder. Here's how you can use it:

```bash
python Data/seq_gen.py
