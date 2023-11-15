using System.ComponentModel.DataAnnotations;

namespace ApiClient.Models
{
    public class SecurityModel
    {
        public string ISINCode { get; set; }
        public decimal Price { get; set; }
    }
}
