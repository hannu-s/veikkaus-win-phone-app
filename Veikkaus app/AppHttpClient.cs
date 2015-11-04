using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AdvancedREI.Net.Http.Compression;
using System.Net.Http;

namespace Veikkaus_app
{
    public class AppHttpClient
    {
        private async Task<string> FetchGzipCompressedHttpContent(Uri uri)
        {
            var handler = new CompressedHttpClientHandler();
            var client = new HttpClient(handler);
            var result = await client.GetStringAsync(uri);
            return result;
        }

        public async Task<string> GetMatchesAsync()
        {
            var uri = new Uri(Resources.AppResources.UriVeikkausliigaMatches);
            return await FetchGzipCompressedHttpContent(uri);
        }

        public async Task<string> GetMatchDataAsync(string id)
        {
            var uri = new Uri(Resources.AppResources.UriVeikkausliigaMatches + "/" + id);
            return await FetchGzipCompressedHttpContent(uri);
        }
    }
}
