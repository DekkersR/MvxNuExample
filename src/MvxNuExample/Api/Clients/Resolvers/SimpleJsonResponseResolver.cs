using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvxNuExample.Api.Clients.Resolvers
{
    public interface IHttpResponseResolver
    {
        Task<TResult> ResolveHttpResponseAsync<TResult>(HttpResponseMessage responseMessage);
    }

    public class SimpleJsonResponseResolver : IHttpResponseResolver
    {
        public async Task<TResult> ResolveHttpResponseAsync<TResult>(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                return default(TResult);
            }

            try
            {
                var responseAsString = await responseMessage.Content.ReadAsStringAsync();
                var jObj = (JObject)JsonConvert.DeserializeObject(responseAsString);
                return JsonConvert.DeserializeObject<TResult>(jObj["results"].ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

           
            return default(TResult);
        }
    }
}
