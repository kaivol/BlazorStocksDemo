using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GitHubBlazor.Extensions;
using Refit;

// ReSharper disable ClassNeverInstantiated.Global

namespace GitHubBlazor.Api
{
    internal partial interface IAlphaVantage
    {
        [Get(
            "/query?" +
            "function=TIME_SERIES_DAILY_ADJUSTED&" +
            "outputsize=full&" +
            "symbol={symbol}&" +
            "apikey={apikey}&"
        )]
        Task<ImmutableSortedArray<(DateOnly date, decimal value)>> TimeSeriesDailyAdjusted(string symbol, string apikey);

        static IComparer<(DateOnly, T)> DateTupleComparer<T>() => Comparer<(DateOnly, T)>.Create(
            (x, y) => x.Item1.CompareTo(y.Item1)
        );
    }

    // internal record TimeSeries
    // {
    //     // ReSharper disable once UnusedMember.Global
    //     public TimeSeries(ImmutableList<DataPoint> dataPoints)
    //     {
    //         DataPoints = dataPoints ?? throw new ArgumentNullException(nameof(dataPoints));
    //     }
    //
    //     public ImmutableList<DataPoint> DataPoints { get; }
    // }

    // internal class DataPoint
    // {
    //     public DataPoint(DateOnly date, decimal value)
    //     {
    //         Value = value;
    //         Date = date;
    //     }
    //
    //     public DateOnly Date { get; }
    //     public decimal Value { get; }
    // }

    internal class TimeSeriesConverter : AlphaVantageConverter<ImmutableSortedArray<(DateOnly, decimal)>>
    {
        protected override bool ParseValue(ref Utf8JsonReader reader, string propertyName, ref ImmutableSortedArray<(DateOnly, decimal)>? result, JsonSerializerOptions options)
        {
            if (propertyName == "Time Series (Daily)")
            {
                reader.ReadToken(JsonTokenType.StartObject);
                result = ParseDataPoints(ref reader);
                reader.ReadToken(JsonTokenType.EndObject);
                return true;
            }

            return false;
        }

        // ReSharper disable once VariableHidesOuterVariable
        private static ImmutableSortedArray<(DateOnly, decimal)> ParseDataPoints(ref Utf8JsonReader reader)
        {
            var result = new SortedSet<(DateOnly, decimal)>(IAlphaVantage.DateTupleComparer<decimal>());
            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                var date = DateOnly.Parse(reader.GetString()!);
                reader.Read();

                var value = ParseEntry(ref reader);

                result.Add((date, value));
            }

            return result.ToImmutableSortedArray();
        }

        // ReSharper disable once VariableHidesOuterVariable
        private static decimal ParseEntry(ref Utf8JsonReader reader)
        {
            reader.ReadToken(JsonTokenType.StartObject);

            decimal? value = null;
            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                
                if (propertyName == "5. adjusted close")
                {
                    value = decimal.Parse(
                        reader.GetString() ?? throw new NullReferenceException(),
                        NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture
                    );
                }
                reader.Read();
            }

            reader.ReadToken(JsonTokenType.EndObject);
            return value ?? throw new NullReferenceException();
        }

        public override void Write(Utf8JsonWriter writer, ImmutableSortedArray<(DateOnly, decimal)> value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }
}