using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Net.Http
{
    internal static class HttpContentJsonExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            string json = await content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            string json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return client.PostAsync(requestUri, content);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            string json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return client.PutAsync(requestUri, content);
        }
    }
}
