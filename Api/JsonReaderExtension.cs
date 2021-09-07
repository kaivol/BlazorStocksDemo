using System.Text.Json;

namespace GitHubBlazor.Api
{
    public static class JsonReaderExtension
    {
        public static void ExpectToken(this ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            if (reader.TokenType != tokenType)
            {
                throw new JsonException(
                    $"Expected TokenType '{tokenType}', found '{reader.TokenType}'."
                );
            }
        }
        
        public static void ReadToken(this ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            reader.ExpectToken(tokenType);
            reader.Read();
        }
    }
}