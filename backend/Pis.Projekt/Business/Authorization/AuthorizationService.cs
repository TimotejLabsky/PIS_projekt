using System;
using System.IO;
using System.Threading.Tasks;
using FiitTaskList;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Framework;

namespace Pis.Projekt.Business.Authorization
{
    public class AuthorizationService
    {
        public AuthorizationService(FiitCustomerService.CustomerPortTypeClient client,
            ILogger<AuthorizationService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string id, string password)
        {
            if (AuthorizeStore(id, password))
            {
                return true;
            }
            
            var res = await _client.checkPasswordAsync(id, password)
                .ConfigureAwait(false);
            if (!res.exists)
            {
                return false;
            }
            _logger.LogDebug($"Login of user with id: {id} was successful");
            return true;
        }

        private bool AuthorizeStore(string id, string password)
        {
            if (id == "predajna1" || id == "predajna2" || id == "predajna3")
            {
                if (password == "root")
                {
                    return true;
                }
            }

            return false;
        }


        private readonly FiitCustomerService.CustomerPortTypeClient _client;
        private readonly ILogger<AuthorizationService> _logger;
    }
}