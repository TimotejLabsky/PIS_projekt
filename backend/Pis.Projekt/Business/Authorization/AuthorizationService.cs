using System;
using FiitTaskList;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Framework;

namespace Pis.Projekt.Business.Authorization
{
    public class AuthorizationService
    {

        public AuthorizationService(FiitCustomerService.CustomerPortTypeClient client, ILogger<AuthorizationService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async void Login(string id, string password)
        {
            var res = await _client.checkPasswordAsync(id, password)
                .ConfigureAwait(false);
            if (!res.exists)
            {
                throw new UnauthorizedAccessException($"User {id} has not been authorized");
            }
            _logger.LogDebug($"Login of user with id: {id} was successful");
        }
        
        
        private readonly FiitCustomerService.CustomerPortTypeClient _client;
        private readonly ILogger<AuthorizationService> _logger;
    }
}