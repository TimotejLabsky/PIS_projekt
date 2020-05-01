using System;

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
                throw new UnauthorizedAccessException();
            }
        }
        
        
        private readonly FiitCustomerService.CustomerPortTypeClient _client;
    }
}