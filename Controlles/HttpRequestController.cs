using Cwi.TreinamentoTesteAutomatizado.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cwi.TreinamentoTesteAutomatizado.Controlles
{
    public class HttpRequestController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Uri BaseUrl;
        private HttpRequestMessage HttpRequestMessage;
        private HttpResponseMessage HttpResponseMessage;

        public HttpRequestController(IHttpClientFactory httpClientFactory, string baseUrl)
        {
            this._httpClientFactory = httpClientFactory;
            BaseUrl = new Uri(baseUrl);
        }

        public void AddJsonBody(object body)
        {
            GetRequestMessage().Content = PreperJsonBody(body);
        }

        public async Task SendAsync(string endpoint, string httpMethodName)
        {
            var request = GetRequestMessage();

            request.RequestUri = new Uri(BaseUrl, endpoint);

            request.Method = GetHttpMethodFromName(httpMethodName);

            HttpResponseMessage = await _httpClientFactory.CreateClient().SendAsync(request);

            HttpRequestMessage.Dispose();

            HttpRequestMessage = null;
        }

        public HttpStatusCode GetResponseHttpStatusCode()
        {
            return HttpResponseMessage.StatusCode;
        }

        public async Task<T> GetTypedResponseBody<T>()
        {
            var responseContent = await GetResponseBodyContent();

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task<List<T>> GetListTypedResponseBody<T>()
        {
            var responseContent = await GetResponseBodyContent();

            return JsonConvert.DeserializeObject<List<T>>(responseContent);

        }

        public async Task<string> GetResponseBodyContent()
        {
            using (var HttpResponse = HttpResponseMessage.Content)
            {
                return await HttpResponse.ReadAsStringAsync();
            }
        }

        public void AddHeader(string name, string value)
        {
            GetRequestMessage().Headers.Add(name, value);
        }

        public void RemoveHeader(string name)
        {
            GetRequestMessage().Headers.Remove(name);
        }

        private HttpContent PreperJsonBody(object body)
        {
            if (body.GetType().IsPrimitive || body is string)
                return new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            else
                return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        }

        private HttpMethod GetHttpMethodFromName(string httpMethodName)
        {
            return httpMethodName.ToLower() switch
            {
                "get" => HttpMethod.Get,
                "post" => HttpMethod.Post,
                "patch" => HttpMethod.Patch,
                "put" => HttpMethod.Put,
                "delete" => HttpMethod.Delete,
                "options" => HttpMethod.Options,
                "head" => HttpMethod.Head,
                _ => HttpMethod.Get,
            };
        }

        private HttpRequestMessage GetRequestMessage()
        {
            if (HttpRequestMessage is null)
                HttpRequestMessage = new HttpRequestMessage();

            return HttpRequestMessage;
        }

    }
}
