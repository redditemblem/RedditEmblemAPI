using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditEmblemAPI.Models.Exceptions.Validation;
using System;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    public class IntTupleConverter : JsonConverter<(int, int)>
    {
        public override bool CanRead => true;

        public override (int, int) ReadJson(JsonReader reader, Type objectType, (int, int) existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            //Suppress any parsing or conversion errors
            //We'll just throw the generic error at the end of the function if something goes wrong
            try
            {
                //If the incoming json value is a single integer, return it as a (0, n) tuple.
                if (reader.ValueType == typeof(Int64))
                {
                    int value = Convert.ToInt32(reader.Value);
                    if (value >= 0)
                        return (0, value);
                }

                //If the incoming json value is an int array, return it as a (n1, n2) tuple.
                if (reader.TokenType == JsonToken.StartArray)
                {
                    JToken token = JToken.ReadFrom(reader);
                    int[] values = token.ToObject<int[]>();
                    if (values.Length == 2)
                    {
                        int val1 = values[0];
                        int val2 = values[1];
                        if (val1 >= 0 && val2 >= 0)
                            return (val1, val2);
                    }
                }
            }
            catch { }

            throw new InvalidIntTupleConverterInputException();
        }

        #region Unimplemented

        // Serialization for this converter is not needed
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, (int, int) value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion Unimplemented
    }
}
