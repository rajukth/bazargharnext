using bazargharnext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace bazargharnext.Controllers
{
    public class RegisterController : Controller
    {
      readonly private DataContext _dal = new DataContext();
      

       
        [HttpPost]
        public IActionResult RegisterUser(User users)
        {
            var schema = HttpContext.Request.Scheme;
            var userExist = _dal.Users.ToList().Exists(x => x.Email.Equals(users.Email,StringComparison.CurrentCultureIgnoreCase));

            if (userExist)
            {

                TempData["UserError"] = "Email Already Exist ! ";
                TempData["Username"] = users.Username;
                TempData["Email"] = users.Email;
                TempData["Password"] = users.Password;
                TempData["Address"] = users.Address;
                TempData["Contact"] = users.Contact;
                TempData["Gender"] = users.Gender;

                return RedirectToAction(schema);

            }
            else
            {
                users.Password = Encryption(users.Password);
                users.Photo = "/image/profile/user.png";
                users.UserRole = "users";
                _dal.Users.Add(users);
                _dal.SaveChanges();

                TempData["LoginError"] = "Most be Logged in First ! ";
                return RedirectToAction("Index", "Home");
            }


        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var schema = HttpContext.Request.Headers["Referer"];
            User user = _dal.Users.SingleOrDefault(x => x.Email.Equals(email));

            if (user != null)
            {
                bool isValidPassword = Encryption(password).Equals(user.Password);
                if (isValidPassword)
                {
                    
                    HttpContext.Session.SetString("isLoggedin", "true");
                    HttpContext.Session.SetString("userAs", "user");
                    HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                    return Redirect(schema);
                }
                else
                {
                    TempData["LoginError"] = "Email or Password Doesn`t Match ! ";
                    return Redirect(schema);
                }
            }
            else
            {
                TempData["LoginError"] = "User Doesn`t Exist ! ";
                return Redirect(schema);
            }
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Remove("User");

            HttpContext.Session.SetString("isLoggedin","false");
            return RedirectToAction("Index", "home");
        
        }
        public IActionResult ChangePassword(Password password) {
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            String myPassword = user.Password;
            String oldPassword = Encryption(password.oldPassword);
            String newPassword = Encryption(password.newPassword);

            if (oldPassword.Equals(myPassword))
            {

                var users = new User() { Userid = user.Userid, Password = newPassword };
                
                    _dal.Users.Attach(users);
                    _dal.Entry(users).Property(x => x.Password).IsModified = true;
                    _dal.SaveChanges();
                

                TempData["UpdateMsg"] = "updated";
                return RedirectToAction("UpdateProfile", "MyProfile");

            }
            else
            {
                TempData["UpdateMsg"] = "error";
                return RedirectToAction("UpdateProfile", "MyProfile");
            }
            
        }

        public string Encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

    }
  


}
