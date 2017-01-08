using Fusillade;
using ModernHttpClient;
using MvxNuExample.Api.Clients.Resolvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvxNuExample.Api.Clients
{
    public interface IWebApiClient : IDisposable
    {
        string AcceptHeader { get; set; }
        IDictionary<string, string> Headers { get; }
        IHttpContentResolver HttpContentResolver { get; set; }
        IHttpResponseResolver HttpResponseResolver { get; set; }

        Task<TResult> GetAsync<TResult>(Priority priority, string path, CancellationToken? cancellationToken = null);
        Task<TResult> PostAsync<TContent, TResult>(Priority priority, string path, TContent content = default(TContent));
        Task<TResult> PutAsync<TContent, TResult>(Priority priority, string path, TContent content = default(TContent));
        Task<TResult> DeleteAsync<TContent, TResult>(Priority priority, string path, CancellationToken? cancellationToken = null);
    }

    public class WebApiClient : IWebApiClient
    {
        private IHttpContentResolver _httpContentResolver;
        private IHttpResponseResolver _httpResponseResolver;
        private bool _isDisposed;
        private Lazy<HttpClient> _explicit;
        private Lazy<HttpClient> _background;
        private Lazy<HttpClient> _userInitiated;
        private Lazy<HttpClient> _speculative;

        public WebApiClient(string apiBaseAddress)
        {
            if (apiBaseAddress == null)
                throw new ArgumentNullException(nameof(apiBaseAddress));

            Func<HttpMessageHandler, HttpClient> createClient = messageHandler => new HttpClient(messageHandler) { BaseAddress = new Uri(apiBaseAddress) };

            _explicit = new Lazy<HttpClient>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Explicit)));

            _background = new Lazy<HttpClient>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background)));

            _userInitiated = new Lazy<HttpClient>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated)));

            _speculative = new Lazy<HttpClient>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative)));
        }

        public IHttpContentResolver HttpContentResolver
        {
            get { return _httpContentResolver ?? (_httpContentResolver = new SimpleJsonContentResolver()); }
            set { _httpContentResolver = value; }
        }

        public IHttpResponseResolver HttpResponseResolver
        {
            get { return _httpResponseResolver ?? (_httpResponseResolver = new SimpleJsonResponseResolver()); }
            set { _httpResponseResolver = value; }
        }

        public string AcceptHeader { get; set; } = "application/json";

        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public async Task<TResult> GetAsync<TResult>(Priority priority, string path, CancellationToken? cancellationToken = null)
        {
            var httpClient = GetWebApiClient(priority);

            SetHttpRequestHeaders(httpClient);

            System.Net.Http.HttpResponseMessage response = null;

            try
            {
                response = cancellationToken == null
                    ? await httpClient.GetAsync(path).ConfigureAwait(false)
                    : await httpClient.GetAsync(path, (CancellationToken)cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


            return await HttpResponseResolver.ResolveHttpResponseAsync<TResult>(response);
        }

        public async Task<TResult> PostAsync<TContent, TResult>(Priority priority, string path, TContent content = default(TContent))
        {
            var httpClient = GetWebApiClient(priority);

            SetHttpRequestHeaders(httpClient);

            HttpContent httpContent = null;

            if (content != null)
                httpContent = HttpContentResolver.ResolveHttpContent(content);

            var response = await httpClient
                .PostAsync(path, httpContent)
                .ConfigureAwait(false);

            return await HttpResponseResolver.ResolveHttpResponseAsync<TResult>(response);
        }
        
        public async Task<TResult> PutAsync<TContent, TResult>(Priority priority, string path, TContent content = default(TContent))
        {
            var httpClient = GetWebApiClient(priority);

            SetHttpRequestHeaders(httpClient);

            HttpContent httpContent = null;

            if (content != null)
                httpContent = HttpContentResolver.ResolveHttpContent(content);

            var response = await httpClient
                .PutAsync(path, httpContent)
                .ConfigureAwait(false);

            return await HttpResponseResolver.ResolveHttpResponseAsync<TResult>(response);
        }

        public async Task<TResult> DeleteAsync<TContent, TResult>(Priority priority, string path, CancellationToken? cancellationToken = null)
        {
            var httpClient = GetWebApiClient(priority);

            SetHttpRequestHeaders(httpClient);

            var response = cancellationToken == null
                ? await httpClient.DeleteAsync(path).ConfigureAwait(false)
                : await httpClient.DeleteAsync(path, (CancellationToken)cancellationToken).ConfigureAwait(false);

            return await HttpResponseResolver.ResolveHttpResponseAsync<TResult>(response);
        }

        private HttpClient GetWebApiClient(Priority prioriy)
        {
            switch (prioriy)
            {
                case Priority.UserInitiated:
                    return _userInitiated.Value;
                case Priority.Speculative:
                    return _speculative.Value;
                case Priority.Background:
                    return _background.Value;
                case Priority.Explicit:
                    return _explicit.Value;
                default:
                    return _background.Value;
            }
        }

        private void SetHttpRequestHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AcceptHeader));

            foreach (var header in Headers)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _background.Value?.Dispose();
                _explicit.Value?.Dispose();
                _speculative.Value?.Dispose();
                _userInitiated.Value?.Dispose();
            }

            _background = null;
            _explicit = null;
            _speculative = null;
            _userInitiated = null;

            _isDisposed = true;
        }


    }
}
