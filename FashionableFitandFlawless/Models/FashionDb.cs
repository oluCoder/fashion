using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FashionableFitandFlawless.Models
{
    public class FashionDb : DbContext
    {

        public FashionDb() : base("name=Fit")
        {

        }
        public DbSet<Category> category { get; set; }

        public DbSet<SubCategory> subCategory { get; set; }

        public DbSet<Items> item { get; set; }
    }
}