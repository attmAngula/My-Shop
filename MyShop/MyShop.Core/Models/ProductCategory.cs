using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory
    {
        public string Id { get; set; }
        
        public string Category { get; set; }

        // ctor
        public ProductCategory()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
