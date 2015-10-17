using System;
using RestSharp;

namespace AppVeyor.Api
{
    public class AppVeyorService : IAppVeyorService
    {
        private readonly Uri _baseUrl = new Uri("https://ci.appveyor.com/api");
        private readonly ProjectApi _projectApi;

        public ProjectApi ProjectApi
        {
            get
            {
                return _projectApi;
            }
        }

        public AppVeyorService(string apiToken, Uri baseUrl = null)
        {
            if (baseUrl != null)
                _baseUrl = baseUrl;

            RestClient client = new RestClient(_baseUrl);
            _projectApi = new ProjectApi(client, apiToken);
        }
    }
}
