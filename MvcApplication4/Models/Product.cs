using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication4.Models
{
    public class Product
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }
        public Decimal Price { get; set; }
    }
}