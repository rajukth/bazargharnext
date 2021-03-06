using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.ModelsView
{
    public class BusinessUserView
    {
        public int User_Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String Contact { get; set; }
        public IFormFile Photo { get; set; }
        public String UserRole { get; set; }
        /*business user detail*/
        public int Office_No { get; set; }
        public string RegistrationNo { get; set; }
        public string PanNo { get; set; }
        public IFormFile Registration_Image { get; set; }
        public IFormFile Pan_Image { get; set; }
        public string About { get; set; }

    }
}
