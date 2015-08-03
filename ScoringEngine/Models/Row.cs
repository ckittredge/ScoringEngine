using System.Collections.Generic;

namespace ScoringEngine.Models
{
    public class Row
    {
        public int RowNumber { get; set; }
        public string LineStr { get; set; }
        public List<string> Columns;

        public Row()
        {
            Columns = new List<string>();
        }

    }
}
