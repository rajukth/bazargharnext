using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int Customerid { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
      
    }

}
