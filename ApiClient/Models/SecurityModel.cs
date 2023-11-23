using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ApiClient.Models
{
    public class SecurityModel : IValidatableObject
    {
        [Required]
        [StringLength(12,MinimumLength =12, ErrorMessage = "12 caracters required")]
        [JsonPropertyName("isin")]
        public string ISINCode { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {   
            if (string.IsNullOrWhiteSpace(this.ISINCode) || this.ISINCode.Length != 12)
                yield return new ValidationResult($"ISIN Code should have 12 caracters");

        }
    }


}
