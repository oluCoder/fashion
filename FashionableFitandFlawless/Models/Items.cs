using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionableFitandFlawless.Models
{
    public class Items
    {
        [Key]
        public int ItemId { get; set; }

        public string Name { get; set; }

        [Display(Name = "Upload Picture")]
        [MaxLength]
        public string ImagePath { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageByte { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Url)]
        [UIHint("OpenInNewWindow")]
        public string Url { get; set; }

        public string Style { get; set; }

        public string Style2 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Season { get; set; }

        public string SubCategory { get; set; }

        public string Shipping { get; set; }

        //public string Note { get; set; }

        public void UpdateFromExisting(Items existing)
        {
            foreach (var prop in GetType().GetProperties().Where(prop => !prop.Name.ToLower().StartsWith("image")))
            {
                prop.SetValue(this, prop.GetValue(existing));
            }
        }
    }
}