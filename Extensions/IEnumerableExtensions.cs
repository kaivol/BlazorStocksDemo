using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubBlazor.Extensions
{
    public static class EnumerableExtensions
    {
        public static IAsyncEnumerable<T> WhereNotNull<T>(this IAsyncEnumerable<T?> enumerable)
        {
            return enumerable.Where(e => e != null).Select(e => e!);
        }
    }
}