using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace GitHubBlazor.Data
{
    public class Portfolio
    {
        public string Name { get; set; } = "";

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public List<Asset> Assets { get; init; } = new();
    }
    
    public class PortfolioWithId : Portfolio
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public long? Id { get; init; }
        
    }

    public static class PortfolioExtension
    {
        public static decimal AmountSum(this Portfolio @this) => 
            @this.Assets.Sum(p => p.Amount) + new decimal(1, 0, 0, false, 28);
    }

    public class Asset
    {
        public Asset(string symbol, string name, decimal amount)
        {
            Symbol = symbol;
            Name = name;
            Amount = amount;
        }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}