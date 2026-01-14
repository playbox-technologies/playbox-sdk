using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Any.Scripts.Backend.Verificator
{
    public static class HttpService
    {
        private static HttpClient _client;

        private static string _baseUrl = "https://api.playbox.services/v1";

        private static void InitService()
        {
            var handler = new HttpClientHandler
            {
            };

            _client = new HttpClient(handler);

            _client.BaseAddress = new Uri(_baseUrl);
            
            _client.DefaultRequestHeaders.Accept.Clear();

            _client.DefaultRequestHeaders.Add("X-User-ID", Playbox.Data.Playbox.PlayboxKey);
            _client.DefaultRequestHeaders.Add("X-Bundle-ID", Playbox.Data.Playbox.GameId);
            _client.DefaultRequestHeaders.Add("X-App-Version", Playbox.Data.Playbox.AppVersion);
            _client.DefaultRequestHeaders.Add("X-Platform", Playbox.Data.Playbox.Platform);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Playbox.Data.Playbox.PlayboxKey);
        }

        public static Task<HttpResult> GetAsync(string url, CancellationToken ct = default)
            => SendAsync(HttpMethod.Get, url, bodyJson: null, ct);

        public static Task<HttpResult> DeleteAsync(string url, CancellationToken ct = default)
            => SendAsync(HttpMethod.Delete, url, bodyJson: null, ct);

        public static Task<HttpResult> PostJsonAsync(string url, string bodyJson, CancellationToken ct = default)
            => SendAsync(HttpMethod.Post, url, bodyJson, ct);

        public static Task<HttpResult> PutJsonAsync(string url, string bodyJson, CancellationToken ct = default)
            => SendAsync(HttpMethod.Put, url, bodyJson, ct);

        public static async Task<HttpResult> SendAsync(HttpMethod method, string url, string bodyJson,
            CancellationToken ct = default)
        {
            if (_client == null)
                InitService();
            
            if(_client == null)
                Debug.Log("Client is not initialized");
            
            using var req = new HttpRequestMessage(method, _client.BaseAddress + url);

            if (bodyJson != null)
            {
                req.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            }

            var startedAt = DateTime.UtcNow;

            try
            {
                using var resp = await _client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct)
                    .ConfigureAwait(false);

                var responseText = resp.Content != null
                    ? await resp.Content.ReadAsStringAsync().ConfigureAwait(false)
                    : null;

                return new HttpResult
                {
                    IsSuccess = resp.IsSuccessStatusCode,
                    StatusCode = (int)resp.StatusCode,
                    Body = responseText,
                    Error = resp.IsSuccessStatusCode ? null : $"{(int)resp.StatusCode} {resp.ReasonPhrase}",
                    DurationMs = (long)(DateTime.UtcNow - startedAt).TotalMilliseconds
                };
            }
            catch (OperationCanceledException oce) when (!ct.IsCancellationRequested)
            {
                return new HttpResult
                {
                    IsSuccess = false,
                    StatusCode = 0,
                    Body = null,
                    Error = $"Timeout: {oce.Message}",
                    DurationMs = (long)(DateTime.UtcNow - startedAt).TotalMilliseconds
                };
            }
            catch (OperationCanceledException oce)
            {
                return new HttpResult
                {
                    IsSuccess = false,
                    StatusCode = 0,
                    Body = null,
                    Error = $"Canceled: {oce.Message}",
                    DurationMs = (long)(DateTime.UtcNow - startedAt).TotalMilliseconds
                };
            }
            catch (HttpRequestException hre)
            {
                return new HttpResult
                {
                    IsSuccess = false,
                    StatusCode = 0,
                    Body = null,
                    Error = $"HttpRequestException: {hre.Message}",
                    DurationMs = (long)(DateTime.UtcNow - startedAt).TotalMilliseconds
                };
            }
            catch (Exception ex)
            {
                return new HttpResult
                {
                    IsSuccess = false,
                    StatusCode = 0,
                    Body = null,
                    Error = $"Unexpected: {ex.GetType().Name}: {ex.Message}",
                    DurationMs = (long)(DateTime.UtcNow - startedAt).TotalMilliseconds
                };
            }
        }
    }


    public sealed class HttpResult
    {
        public bool IsSuccess;
        public int StatusCode;
        public string Body;
        public string Error;
        public long DurationMs;
    }
}