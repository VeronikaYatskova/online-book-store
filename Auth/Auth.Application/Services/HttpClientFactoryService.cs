using Auth.Application.Abstractions.Interfaces.Services;

namespace Auth.Application.Services
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
       private readonly IHttpClientFactory _httpClientFactory;
        
        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Execute(string token)
        {
            return await GetGoogleUserData(token);
        }

        private async Task<string> GetGoogleUserData(string token)
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            string cliUrl = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={token}";

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cliUrl);
            using HttpResponseMessage response = await httpClient.SendAsync(request);

            var jsonUserData = await response.Content.ReadAsStringAsync();

            return jsonUserData;
        }
    }
}
