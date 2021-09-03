using bazargharnext.Models;
using bazargharnext.ModelsView;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Controllers
{
    public class Admin : Controller
    {
        DataContext _dal = new DataContext();
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult CreateBusiness()
        {

            return View();
        }

        public IActionResult RegisterBusiness(BusinessUserView businessUserView) {

            try
            {
                var user = new User()
                {
                    Username = businessUserView.Username,
                    Email = businessUserView.Email,
                    Password = "",
                    Address = businessUserView.Address,
                    Photo = businessUserView.Photo,
                    UserRole = "business",
                    Contact = businessUserView.Contact,
                    Gender = "none"

                };

                var userid = _dal.Users.Add(user);
                var businessUser = new BusinessUser() { 
                    User_Id=user.Userid,
                RegistrationNo=businessUserView.RegistrationNo,
                PanNo=businessUserView.PanNo,
                Registration_Image=businessUserView.Registration_Image,
                Pan_Image=businessUserView.Pan_Image,
                About=businessUserView.About
                
                
                };

            }
            catch (Exception e) { 
            
            
            
            }



            return View();
        }


    }
}
