using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client
{
    public interface IClientIdentityProvider
    {
        Task<ClientIdentity> GetIdentity();
        void Logout();
        void LogIn();
    }

    public class DevClientIdentityProvider : IClientIdentityProvider
    {
        public Task<ClientIdentity> GetIdentity()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void LogIn()
        {
            throw new NotImplementedException();
        }
    }

    public class ClientIdentityProvider : IClientIdentityProvider
    {
        private readonly NavigationManager _manager;
        private readonly HttpClient _client;

        public ClientIdentityProvider(NavigationManager manager, HttpClient client)
        {
            _manager = manager;
            _client = client;
        }

        public Task<ClientIdentity> GetIdentity()
        {
            return _client.GetFromJsonAsync<ClientIdentity>(".auth/me");
        }

        public void Logout()
        {
            _manager.NavigateTo(".auth/logout");
        }

        public void LogIn()
        {
            _manager.NavigateTo(".auth/login/google");
        }
    }
}
