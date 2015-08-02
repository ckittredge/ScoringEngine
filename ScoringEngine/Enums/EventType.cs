using System;

namespace ScoringEngine.Enums
{
    public enum EventType
    {
        Invalid,
        Email,
        Social,
        Web,
        Webinar
    }

    public class EventTypeConverter : EnumConverter<EventType>
    {
        public override bool TryParseString(string str, out EventType type)
        {
            if (String.IsNullOrEmpty(str) || !TypeDictionary.ContainsKey(str.Trim().ToLowerInvariant()))
            {
                type = EventType.Invalid;
                return false;
            }
            type = TypeDictionary[str.Trim().ToLowerInvariant()];
            return true;
        }
    }
}
