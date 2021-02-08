using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using RequestProcessor.App.Models;

namespace RequestProcessor.App.Services
{
    internal class RequestHandler : IRequestHandler
    {
        private readonly HttpClient _httpClient;

        public RequestHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions)
        {
            if (requestOptions == null || !requestOptions.IsValid)
                throw new ArgumentException(" Request options are null or invalid.");

            HttpRequestMessage requestMessage = new HttpRequestMessage(MapHttpMethod(requestOptions.Method), requestOptions.Address);
            
            if (requestOptions.Body != null && requestOptions.ContentType != null)
                requestMessage.Content = new StringContent(requestOptions.Body, Encoding.UTF8, requestOptions.ContentType);

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);
            return new Response((int)responseMessage.StatusCode, await responseMessage.Content.ReadAsStringAsync());
        }

        public HttpMethod MapHttpMethod(RequestMethod requestMethod)
        {
            return requestMethod switch
            {
                RequestMethod.Get => HttpMethod.Get,
                RequestMethod.Post => HttpMethod.Post,
                RequestMethod.Put => HttpMethod.Put,
                RequestMethod.Delete => HttpMethod.Delete,
                RequestMethod.Patch => HttpMethod.Patch,
                _ => throw new InvalidOperationException(nameof(requestMethod)),
            };
        }
    }
}
