//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Market.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Orders_Has_Products = new HashSet<Orders_Has_Products>();
        }
    
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [AllowHtml]
        public string description { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public double price { get; set; }

        public bool isActive { get; set; }

        [Required]
        public string imageURL { get; set; }

        [MaxLength(100)]
        public string shortDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders_Has_Products> Orders_Has_Products { get; set; }
    }
}
