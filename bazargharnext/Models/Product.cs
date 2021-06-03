using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        public int Userid { get; set; }

        public String Product_name { get; set; }
        public int Category_id { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public String Description { get; set; }

        public List<Product_Details> Product_Details { get; set; } //imp
        public ICollection<GalleryModel> GalleryModel { get; set; }
        public List<Comments> Comments { get; set; }

    }
}
