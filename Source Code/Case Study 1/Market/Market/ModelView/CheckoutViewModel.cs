using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Market.ModelView
{
    public class CheckoutViewModel
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("Card Holder's Name")]
        public string CardHolderName { get; set; }

        [Required]
        [DisplayName("Card Number")]
        [RegularExpression("([0-9]{12})", ErrorMessage = "Card Number must must have 12 digits")]
        public string CardNumber { get; set; }

        [Required]
        [DisplayName("Expiration Date")]
        [RegularExpression("([0-9]{4})", ErrorMessage = "Expiration date must have 4 digits")]
        public string ExpirationDate { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Full Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required]
        [DisplayName("ZIP Code")]
        [RegularExpression("([0-9]+)", ErrorMessage = "ZIP Code must be numeric")]
        public int ZipCode { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Phone Number must me numeric")]
        public int PhoneNumber { get; set; }

        [Required]
        [DisplayName("Security Code")]
        [RegularExpression("([0-9]{3})", ErrorMessage = "Security code must have 3 digits")]
        public int SecurityCode { get; set; }
    }
}