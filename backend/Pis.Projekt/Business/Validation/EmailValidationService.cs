using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using FiitValidatorService;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Business.Validation
{
    public class EmailValidationService
    {
        public EmailValidationService(ValidatorPortTypeClient validatorPortTypeClient, ILogger<EmailValidationService> logger)
        {
            _validatorPortTypeClient = validatorPortTypeClient;
            _logger = logger;
        }

        public async Task ValidateEmail(string email)
        {
           var res = await _validatorPortTypeClient.validateEmailAsync(email)
               .ConfigureAwait(false);

           if (!res.success)
           {
               throw new ValidationException($"Couldn't validate e-mail {email}");
           }
           _logger.LogDebug($"Email: {email} was successfully validated");
        }

        private readonly ValidatorPortTypeClient _validatorPortTypeClient;
        private readonly ILogger<EmailValidationService> _logger;
    }
}