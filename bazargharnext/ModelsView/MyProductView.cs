using bazargharnext.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.ModelsView
{
    public class MyProductView
    {
        public int Product_Id { get; set; }

        public int Userid { get; set; }
        [Required(ErrorMessage = "Please Add Product Name")]
        public string Product_name { get; set; }
        public int Category_id { get; set; } 
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Please Add Product Price")]
        public int? Price { get; set; }
        public string Description { get; set; }

       


[Required(ErrorMessage = "Please choose some images")]
        public IFormFileCollection GalleryFiles{get; set;} 
        public List<GalleryModel> Gallery { get; set; }

        public List<Product_Details> Product_Details { get; set; }

        public string Category_name { get; set; }
        public List<MyCommentView> Comments { get; set; }


    }
}
