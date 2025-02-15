using Newtonsoft.Json;
using System;
using System.Collections;

namespace RedditEmblemAPI.Models.Output.System.Interfaces
{
    /// <summary>
    /// Requires class to include a <c>Matched</c> flag.
    /// </summary>
    public interface IMatchable
    {
        bool Matched { get; }

        void FlagAsMatched();
    }


    /// <summary>
    /// Custom JSON serializer rule for <c>IReadOnlyDictionary<string, IMatchable></c> dictionaries. Only serializes dictionary objects that have their <c>Matched</c> attribute set to true.
    /// </summary>
    public class OmitUnmatchedObjectsFromIMatchableDictionaryConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //We know the incoming object is a dictionary thanks to CanConvert().
            //Cast to the most generic dictionary possible so we can iterate.
            IDictionary dictionary = (IDictionary)value;
            writer.WriteStartObject();

            foreach (DictionaryEntry entry in dictionary)
            {
                //Skip serializing any key/value pairs not marked as matched
                IMatchable matchable = (IMatchable)entry.Value;
                if (!matchable.Matched)
                    continue;

                //Serialize the base object values
                writer.WritePropertyName(entry.Key.ToString());
                serializer.Serialize(writer, entry.Value);
            }

            writer.WriteEndObject();
        }
    }
}
