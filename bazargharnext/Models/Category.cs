using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    [Table("category")]
    public class Category
    {
        [Key]
        public int Category_id {
            get;set;
        }
        public string Category_name {
            get;set;
        }
        public string Category_description { get; set; }

        public List<Product> Product { get; set; }
    }
}
