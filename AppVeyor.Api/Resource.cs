using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using AppVeyor.Common;
using AppVeyor.Common.Exceptions;
using RestSharp;

namespace AppVeyor.Api
{
    public class Resource
    {
        private readonly IRestClient _client;
        private readonly string _apiToken;

        protected Resource(IRestClient client, string apiToken)
        {
            _client = client;
            _apiToken = apiToken;
        }

        internal IRestResponse Execute(IRestRequest request)
        {
            AddHeaderInfo(request);
            IRestResponse response = _client.Execute(request);

            return response;
        }

        internal T Execute<T>(IRestRequest request) where T : new()
        {
            AddHeaderInfo(request);
            IRestResponse<T> response = _client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var applicationException = new ApplicationException(message, response.ErrorException);
                throw applicationException;
            }
            return response.Data;
        }

        internal void ExecuteAsync<T>(IRestRequest request, Action<T> callback) where T : new()
        {
            AddHeaderInfo(request);
            var tcs = new TaskCompletionSource<T>();
            _client.ExecuteAsync<T>(request, response =>
            {
                if (response.ErrorException != null)
                    tcs.TrySetException(response.ErrorException);
                else
                    tcs.TrySetResult(response.Data);
                callback(response.Data);
            });
        }

        public Task<T> ExecuteAsync<T>(IRestRequest request) where T : new()
        {
            AddHeaderInfo(request);
            _client.UserAgent = "AppVeyor/1.0";
            var tcs = new TaskCompletionSource<T>();

            _client.ExecuteAsync<T>(request, response =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                if (response.ErrorException != null)
                {
                    stopWatch.Stop();
                    Telemetry.Instance.TrackRequest(request.Resource.ToString(), DateTimeOffset.UtcNow, stopWatch.Elapsed, response.StatusCode.ToString(), false);
                   
                    tcs.TrySetException(response.ErrorException);
                }
                else if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
                {
                    stopWatch.Stop();
                    Telemetry.Instance.TrackRequest(request.Resource.ToString(), DateTimeOffset.UtcNow, stopWatch.Elapsed, response.StatusCode.ToString(), true);
                    
                    tcs.TrySetResult(response.Data);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    stopWatch.Stop();
                    Telemetry.Instance.TrackRequest(request.Resource.ToString(), DateTimeOffset.UtcNow, stopWatch.Elapsed, response.StatusCode.ToString(), false);
                    
                    tcs.TrySetException(
                        new AppVeyorUnauthorizedException("Unable to authorize with AppVeyor. Please check if your API token is correct."));
                }
                else
                {
                    stopWatch.Stop();
                    Telemetry.Instance.TrackRequest(request.Resource.ToString(), DateTimeOffset.UtcNow, stopWatch.Elapsed, response.StatusCode.ToString(), false);
                    
                    var errorMessage = !string.IsNullOrEmpty(response.ErrorMessage)
                        ? response.ErrorMessage
                        : string.Format("Error: {0}, Status Code: {1}", response.StatusDescription, (int)response.StatusCode);
                    tcs.TrySetException(new ApplicationException(errorMessage));
                }
                    
            });

            return tcs.Task;
        }
        private void AddHeaderInfo(IRestRequest request)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", string.Format("Bearer {0}", _apiToken));
        }
        
    }
}