using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using FiitValidatorService;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Business.Validation
{
    public class EmailValidationService
    {
        public EmailValidationService(ValidatorPortTypeClient validatorPortTypeClient)
        {
            _validatorPortTypeClient = validatorPortTypeClient;
        }

        public async Task ValidateEmail(string email)
        {
           var res = await _validatorPortTypeClient.validateEmailAsync(email)
               .ConfigureAwait(false);

           if (!res.success)
           {
               throw new ValidationException($"Couldn't validate e-mail {email}");
           }
        }

        private readonly ValidatorPortTypeClient _validatorPortTypeClient;
    }
}