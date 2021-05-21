using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    [Table("product_details")]
    public class Product_Details
    {
        [Key]
        public int Id { get; set; }
       
        [ForeignKey("product")]
        public int Product_Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }



    }
}
