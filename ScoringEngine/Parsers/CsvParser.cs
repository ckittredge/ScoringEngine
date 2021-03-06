﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ScoringEngine.CustomExceptions;
using ScoringEngine.Enums;
using ScoringEngine.Models;


namespace ScoringEngine.Parsers
{
    [ExcludeFromCodeCoverage]
    public class CsvParser : StreamReader, ICsvParser
    {

        #region class variables

        private int _totalRows;

        #endregion

        #region constructor

        public CsvParser(string filePath)
            : base(filePath)
        {
            _totalRows = File.ReadLines(@filePath).Count();
        }

        #endregion

        #region public

        public bool ReadRow(Row row)
        {
            row.LineStr = ReadLine();

            if (String.IsNullOrEmpty(row.LineStr))
            {
                if (row.RowNumber < _totalRows)
                {
                    throw new InvalidRowInputException("Row is empty");
                }
                return false;
            }

            int position = 0;
            int rows = 0;

            while (position < row.LineStr.Length)
            {
                string value;

                if (row.LineStr[position] == '"')
                {
                    position++;
                    int start = position;
                    while (position < row.LineStr.Length)
                    {
                        if (row.LineStr[position] == '"')
                        {
                            position++;

                            if (position >= row.LineStr.Length || row.LineStr[position] != '"')
                            {
                                position--;
                                break;
                            }
                        }
                        position++;
                    }
                    value = row.LineStr.Substring(start, position - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    int start = position;
                    while (position < row.LineStr.Length && row.LineStr[position] != ',')
                        position++;
                    value = row.LineStr.Substring(start, position - start);
                }

                if (rows < row.Columns.Count)
                {
                    row.Columns[rows] = value;
                }
                else
                {
                    row.Columns.Add(value);
                }
                rows++;

                while (position < row.LineStr.Length && row.LineStr[position] != ',')
                    position++;
                if (position < row.LineStr.Length) position++;
            }

            while (row.Columns.Count > rows) row.Columns.RemoveAt(rows);

            return (row.Columns.Count > 0);
        }

        #endregion

    }
}
