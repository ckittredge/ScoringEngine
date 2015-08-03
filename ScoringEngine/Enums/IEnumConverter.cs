using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine.Enums
{
    public interface IEnumConverter<T>
    {
        bool TryParseString(string str, out T type);
    }
}
