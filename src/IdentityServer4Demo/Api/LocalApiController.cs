using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using IdentityModel.Client;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IdentityServer4Demo.Api
{
    [Route("localApi")]
    //[Authorize(LocalApi.PolicyName)]
    
    public class LocalApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LocalApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var client = _httpClientFactory.CreateClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "m2m",
                ClientSecret = "secret",
                Scope = "IdentityServerApi"
            });

            
          
            return Ok(
                new
                {
                    access_token = tokenResponse.AccessToken,
                    expires_in = tokenResponse.ExpiresIn,
                    token_type = tokenResponse.TokenType,
                    scope = tokenResponse.Scope
                });
        }
    }
}
