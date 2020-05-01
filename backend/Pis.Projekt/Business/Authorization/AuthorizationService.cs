using System;
using FiitTaskList;
using Microsoft.AspNetCore.Builder;
using Pis.Projekt.Framework;

namespace Pis.Projekt.Business.Authorization
{
    public class AuthorizationService
    {

        public AuthorizationService(FiitCustomerService.CustomerPortTypeClient client)
        {
            _client = client;
        }

        public async void Login(string id, string password)
        {
            var res = await _client.checkPasswordAsync(id, password)
                .ConfigureAwait(false);
            if (!res.exists)
            {
                throw new UnauthorizedAccessException($"User {id} has not been authorized");
            }
        }
        
        
        private readonly FiitCustomerService.CustomerPortTypeClient _client;
    }
}