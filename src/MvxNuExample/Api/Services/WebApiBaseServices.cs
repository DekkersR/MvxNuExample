using MvxNuExample.Api.Clients;
using Polly;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace MvxNuExample.Api.Services
{
    internal abstract class WebApiBaseService
    {
        protected readonly IWebApiClient WebApiClient;

        protected WebApiBaseService(IWebApiClient webApiClient)
        {
            if (webApiClient == null)
                throw new ArgumentNullException(nameof(webApiClient));

            WebApiClient = webApiClient;
        }

        protected async Task<TResult> ExecuteRemoteRequest<TResult>(Func<Task<TResult>> action)
        {
            TResult result = default(TResult);

            try
            {
                result = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync(
                        retryCount: 5,
                        sleepDurationProvider: retryAttamp => TimeSpan.FromSeconds(Math.Pow(2, retryAttamp))
                    )
                    .ExecuteAsync(action);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }
    }
}
