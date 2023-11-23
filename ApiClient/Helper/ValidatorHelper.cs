using ApiClient.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiClient.Helper
{
    public static class ValidatorHelper
    {
        public static bool ValidateSecurityCode(string code)
        {
            var model = new SecurityModel { ISINCode = code };

            return Validator.TryValidateObject(
                model,
                new ValidationContext(model, null, null),
                new List<ValidationResult>(),
                true);

        }
    }
}
