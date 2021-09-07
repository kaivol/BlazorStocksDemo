using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitHubBlazor.Api
{
    internal abstract class AlphaVantageConverter<T> : JsonConverter<T>
    {
        private const string ApiLimit = 
            "Thank you for using Alpha Vantage! Our standard API call frequency is 5 calls per " +
            "minute and 500 calls per day. Please visit https://www.alphavantage.co/premium/ if " +
            "you would like to target a higher API call frequency.";
        
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.ReadToken(JsonTokenType.StartObject);
            
            T? result = default;
            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString()!;
                reader.Read();

                if (propertyName == "Note")
                    throw reader.GetString() switch
                    {
                        string s when s.StartsWith(ApiLimit) => new AlphaVantageLimitException(),
                        string s => new AlphaVantageException("Note: " + s),
                        null => new AlphaVantageException(),
                    };
                if (propertyName == "Error Message")
                    throw reader.GetString() switch
                    {
                        string s when s.StartsWith("the parameter apikey is invalid or missing.") => new AlphaVantageApiKeyException(),
                        string s => new AlphaVantageException("Error Message: " + s),
                        null => new AlphaVantageException(),
                    };
                
                var parsed = ParseValue(ref reader, propertyName, ref result, options);
                if (!parsed)
                {
                    reader.Skip();
                    reader.Read();
                }
            }

            reader.ExpectToken(JsonTokenType.EndObject);

            if (result is null)
            {
                throw new AlphaVantageException("Invalid API response");
            }
            
            return result;
        }

        protected abstract bool ParseValue(ref Utf8JsonReader reader, string propertyName, ref T? result, JsonSerializerOptions options);

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }

    public class AlphaVantageException : Exception
    {
        public AlphaVantageException() { }
        protected AlphaVantageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public AlphaVantageException(string? message) : base(message) { }
        public AlphaVantageException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    public class AlphaVantageLimitException : AlphaVantageException
    {
        public AlphaVantageLimitException() { }
        protected AlphaVantageLimitException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public AlphaVantageLimitException(string? message) : base(message) { }
        public AlphaVantageLimitException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    public class AlphaVantageApiKeyException : AlphaVantageException
    {
        public AlphaVantageApiKeyException() { }
        protected AlphaVantageApiKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public AlphaVantageApiKeyException(string? message) : base(message) { }
        public AlphaVantageApiKeyException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}