using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using GitHubBlazor.Api;
using GitHubBlazor.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GitHubBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddAntDesign()
                .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
                .AddBlazoredLocalStorage()
                .AddScoped<Database>();
            builder.Services.AddRefitClient<IAlphaVantage>(
                    new RefitSettings(
                        new SystemTextJsonContentSerializer(
                            new JsonSerializerOptions
                            {
                                Converters =
                                {
                                    new SymbolSearchConverter(),
                                    new TimeSeriesConverter(),
                                },
                            }
                        )
                    )
                )
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://www.alphavantage.co"));
            await builder.Build().RunAsync();
        }
    }
}