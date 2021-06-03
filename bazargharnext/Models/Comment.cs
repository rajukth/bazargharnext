using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    public class Comments
    {
        [Key]
        public int Comment_Id { get; set; }
        
        public int Product_Id { get; set; }
       
        public string Comment_Text { get; set; }
        public int User_By { get; set; }
        public int Reply_To { get; set; }
        public DateTime Date { get; set; }
        public bool React { get; set; }



    }
}
