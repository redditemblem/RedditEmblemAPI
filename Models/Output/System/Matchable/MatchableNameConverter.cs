using Newtonsoft.Json;
using System;

namespace RedditEmblemAPI.Models.Output.System.Match
{
    /// <summary>
    /// Serializes the <c>Matchable</c> object's <c>Name</c> value to a string.
    /// </summary>
    public class MatchableNameConverter : JsonConverter<Matchable>
    {
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, Matchable? value, JsonSerializer serializer)
        {
            if (value is not null)
                writer.WriteValue(value.Name);
        }

        #region Unimplemented

        // Deserialization for this class is not needed
        public override bool CanRead => false;

        public override Matchable ReadJson(JsonReader reader, Type objectType, Matchable existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion Unimplemented
    }
}
