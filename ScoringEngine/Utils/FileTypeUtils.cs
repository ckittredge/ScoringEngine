using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoringEngine.Utils
{
    public class FileTypeUtils : IFileTypeUtils
    {
        public bool ContainsCsvExtension(string filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return false;
            List<string> splitArr = filePath.Split('.').ToList();
            return splitArr.Last().Trim().ToLowerInvariant() == "csv";

        }
    }
}
