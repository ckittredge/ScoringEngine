using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine.CustomExceptions
{
    public class InvalidRowInputException : Exception
    {
        #region class variables

        public List<int> InvalidColumns;
        public new string Message;

        #endregion

        #region constructors

        public InvalidRowInputException(string message)
        {
            InvalidColumns = new List<int>();
            Message = message;
        }

        public InvalidRowInputException(List<int> invalidColumns)
        {
            InvalidColumns = invalidColumns;
        }

        public InvalidRowInputException(List<int> invalidColumns, string message)
            : base(message)
        {
            InvalidColumns = invalidColumns;
        }

        #endregion
    }
}
