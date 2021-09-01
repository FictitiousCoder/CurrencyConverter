using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CurrencyConvert.Infrastructure.Services
{
    public class FixerApiHandler : DelegatingHandler
    {
        private const string FixerApiAccessKey = "12d2780b96e50aa322259e903b6cbd82";

        public FixerApiHandler()
        {
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellation)
        {
            request.RequestUri = GetRequestUriWithAccessKey(request.RequestUri);
            return await base.SendAsync(request, cancellation);
        }

        private Uri GetRequestUriWithAccessKey(Uri requestUri)
        {
            var uriBuilder = new UriBuilder(requestUri);
            var parameters = HttpUtility.ParseQueryString(uriBuilder.Query);
            parameters.Add("access_key", FixerApiAccessKey);
            uriBuilder.Query = parameters.ToString() ?? string.Empty;
            return uriBuilder.Uri;
        }
    }
}
