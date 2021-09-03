using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    [Table("BusinessUser")]
    public class BusinessUser
    {
        [Key]
        public int id{ get; set; }
        [ForeignKey("user")]
        public int User_Id { get; set; }
        public int Office_No { get; set; }
        public string RegistrationNo { get; set; }
        public string PanNo { get; set; }
        public string Registration_Image { get; set; }
        public string Pan_Image { get; set; }
        public string About { get; set; }







    }
}
