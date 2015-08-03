using System;
using System.Collections.Generic;

namespace ScoringEngine.Enums
{
    public abstract class EnumConverter<T> : IEnumConverter<T>
    {
        #region class variables

        public Dictionary<string, T> TypeDictionary;

        #endregion

        #region constructor

        protected EnumConverter()
        {
            TypeDictionary = PopulateTypeDictionary();
        }

        #endregion

        #region abstract

        public abstract bool TryParseString(string str, out T type);

        #endregion

        #region private

        private Dictionary<string, T> PopulateTypeDictionary()
        {
            Dictionary<string, T> typeDictionary = new Dictionary<string, T>();
            foreach (T type in Enum.GetValues(typeof(T)))
            {
                typeDictionary.Add(type.ToString().ToLowerInvariant(), type);
            }
            return typeDictionary;
        }

        #endregion

    }
}
