using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionableFitandFlawless.Models
{
    public class SubCategory
    {
        [Key]
        public int SubId { get; set; }

        public int CategoryId { get; set; }

        public string Categories { get; set; }

        public string SubName { get; set; }

        public virtual Category category { get; set; }
    }
}