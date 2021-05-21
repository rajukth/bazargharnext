using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.ModelsView
{
    public class MyProfileView
    {
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String Contact { get; set; }

        public IFormFile Photo { get; set; }
    }
}
