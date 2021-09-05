using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.ModelsView
{
    [Table("BusinessUserRequest")]
    public class BusinessUserRequest
    {

        [Key]
        public int BusinessRegId { get; set; } 
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String Contact { get; set; }
        public String Photo { get; set; }
        public String UserRole { get; set; }
        /*business user detail*/
        public int Office_No { get; set; }
        public string RegistrationNo { get; set; }
        public string PanNo { get; set; }
        public string Registration_Image { get; set; }
        public string Pan_Image { get; set; }
        public string About { get; set; }

      
    }
}
