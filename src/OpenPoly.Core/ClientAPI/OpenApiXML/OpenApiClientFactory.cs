using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPoly.Core.ClientAPI.OpenApiXML;

public class OpenApiClientFactory
{
    private readonly IAuthenticationProvider _authenticationProvider;
    private readonly HttpClient _httpClient;

    public OpenApiClientFactory(HttpClient httpClient)
    {
        _authenticationProvider = new AnonymousAuthenticationProvider();
        _httpClient = httpClient;
    }

    public OpenApiClient GetClient()
    {
        return new OpenApiClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
    }
}
