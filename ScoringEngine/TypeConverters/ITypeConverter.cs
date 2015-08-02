using System.Collections.Generic;

namespace ScoringEngine.TypeConverters
{
    public interface ITypeConverter<T>
    {
        List<T> ConvertToType(string filePath);
    }
}
