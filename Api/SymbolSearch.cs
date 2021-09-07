using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Refit;

// ReSharper disable ClassNeverInstantiated.Global

namespace GitHubBlazor.Api
{
    internal partial interface IAlphaVantage
    {
        [Get(
            "/query?" +
            "function=SYMBOL_SEARCH&" +
            "keywords={keywords}&" +
            "apikey={apikey}&"
        )]
        Task<List<SymbolSearchMatch>> SymbolSearch(string keywords, string apikey);
    }

    internal record SymbolSearchMatch
    {
        public SymbolSearchMatch(string symbol, string name, string region, string currency)
        {
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Region = region ?? throw new ArgumentNullException(nameof(region));
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        [JsonPropertyName("1. symbol")]
        public string Symbol { get; }
        
        [JsonPropertyName("2. name")]
        public string Name { get; }
        
        [JsonPropertyName("4. region")]
        public string Region { get; }
        
        [JsonPropertyName("8. currency")]
        public string Currency { get; }
    }

    internal class SymbolSearchConverter : AlphaVantageConverter<List<SymbolSearchMatch>>
    {
        protected override bool ParseValue(ref Utf8JsonReader reader, string propertyName, ref List<SymbolSearchMatch>? result, JsonSerializerOptions options)
        {
            if (propertyName == "bestMatches")
            {
                result = new List<SymbolSearchMatch>();
                var converter = (JsonConverter<SymbolSearchMatch>) options.GetConverter(typeof(SymbolSearchMatch));
                
                reader.ReadToken(JsonTokenType.StartArray);
                while (reader.TokenType == JsonTokenType.StartObject)
                {
                    result.Add(converter.Read(ref reader, typeof(SymbolSearchMatch), options) ?? throw new AlphaVantageException());
                    reader.Read();
                }
    
                reader.ReadToken(JsonTokenType.EndArray);
                return true;
            }

            return false;
        }
    }
}