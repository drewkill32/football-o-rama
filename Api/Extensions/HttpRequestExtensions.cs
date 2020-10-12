using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Http;

namespace BlazorApp.Api.Extensions
{
    public static class HttpRequestExtensions
    {


        public static ClientIdentity GetIdentity(this HttpRequest req)
        {
            var header = req.Headers["x-ms-client-principal"];
            if (header.Count == 0)
            {
                return new ClientIdentity();
            }
            var data = header[0];
            var decoded = Convert.FromBase64String(data);
            var json = Encoding.ASCII.GetString(decoded);
            var principal = JsonSerializer.Deserialize<ClientIdentity>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
            if (principal == null)
            {
                return new ClientIdentity();
            }
            principal.UserRoles = principal.UserRoles.Except(new [] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase).ToArray();

            return principal;
        }
    }
}