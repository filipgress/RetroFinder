using System;

namespace RetroFinder;

[Flags]
public enum ExportFormat
{
    NONE = 0,
    JSON = 1,
    XML = 2
}