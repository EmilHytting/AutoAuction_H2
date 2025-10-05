using System;
using System.Net.Http;

namespace AutoAuction_H2.Services;

public abstract class ApiBase
{
    protected readonly HttpClient _client;

    protected ApiBase(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(AppState.Instance.ApiBaseUrl);
    }
}
