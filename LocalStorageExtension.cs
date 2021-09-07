using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace GitHubBlazor
{
    public static class LocalStorageExtension
    {
        private const string KeyApiKey = "alphaVantageApiKey";
        public static ValueTask<string> GetApiKey(this ILocalStorageService @this) => 
            @this.GetItemAsync<string>(KeyApiKey);
        public static ValueTask SetApiKey(this ILocalStorageService @this, string key) => 
            @this.SetItemAsync(KeyApiKey, key);
    }
}