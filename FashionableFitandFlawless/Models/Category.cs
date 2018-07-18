using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionableFitandFlawless.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public String Section { get; set; }

        public virtual ICollection<SubCategory> subCategory { get; set; }
    }
}