using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MvxNuExample.Api.Clients.Resolvers
{
    public interface IHttpContentResolver
    {
        HttpContent ResolveHttpContent<TContent>(TContent content);
    }

    public class SimpleJsonContentResolver : IHttpContentResolver
    {
        public HttpContent ResolveHttpContent<TContent>(TContent content)
        {
            var serializedContent = JsonConvert.SerializeObject(content);

            return new StringContent(serializedContent, Encoding.UTF8, "application/json");
        }
    }
}
