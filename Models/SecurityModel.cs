using System.ComponentModel.DataAnnotations;

namespace BNPISINClient.Models
{
    public class SecurityModel
    {
        public string ISINCode { get; set; }
        public decimal Price { get; set; }
    }
}
