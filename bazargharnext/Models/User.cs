using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public int Userid { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String Contact { get; set; }
        public String Photo { get; set; }
        public String UserRole { get; set; }





    }
}
