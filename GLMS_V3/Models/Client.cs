using System.ComponentModel.DataAnnotations;

namespace GLMS.Models
{
    public class Client
    {
        [Display(Name = "Client Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Contact number must start with 0 and contain exactly 10 digits.")]
        [Display(Name = "Contact Details")]
        public string ContactDetails { get; set; }

        [Required]
        [StringLength(100)]
        public string Region { get; set; }

        // Navigation Property
        public ICollection<Contract> Contracts { get; set; }
            = new List<Contract>();
    }
}