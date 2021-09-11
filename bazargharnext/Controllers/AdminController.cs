using bazargharnext.AllFunction;
using bazargharnext.ModelOnly;
using bazargharnext.Models;
using bazargharnext.ModelsView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bazargharnext.Controllers
{
    public class AdminController : Controller
    {
       
        DataContext _dal = new DataContext();
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("isLoggedin") != "true") {
                                
                
                
                return RedirectToAction("Login","Admin");
            }
            else{
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                if (user.UserRole == "admin")
                {

               

            var newRequest=_dal.BusinessUserRequests.Count();
            HttpContext.Session.SetInt32("requestCount", newRequest);

            var totlaProductCount = _dal.Products.Count();
            var NormalUser = _dal.Users.Where(x => x.UserRole == "user").Count();
            var BusinessUser = _dal.Users.Where(x => x.UserRole == "business").Count();
            ViewBag.TotalProduct = totlaProductCount;
            ViewBag.NormalUser = NormalUser;
            ViewBag.BusinessUser = BusinessUser;

            return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }
        public IActionResult Login(String email,String Password) {
            User user = _dal.Users.Where(x => x.UserRole=="admin").SingleOrDefault(x => x.Email.Equals(email));

            if (user != null)
            {
                bool isValidPassword = Encryption(Password).Equals(user.Password);
                if (isValidPassword)
                {

                    HttpContext.Session.SetString("isLoggedin", "true");

                    HttpContext.Session.SetString("uviva 07ser", JsonConvert.SerializeObject(user));
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["LoginError"] = "Email or Password Doesn`t Match ! ";
                    return View();
                }
            }
            else
            {
                TempData["LoginError"] = "User Doesn`t Exist ! ";
                return View();
            }
          }


        [Route("registerBusiness")]
        public IActionResult RegisterBusiness()
        {
           

                    return View();
           
        }

        public IActionResult ApproveBusiness(int id) {
            if (HttpContext.Session.GetString("isLoggedin") != "true")
            {
                return RedirectToAction("Login", "Admin");
            }
            var businessUserView = _dal.BusinessUserRequests.Where(x => x.BusinessRegId == id).FirstOrDefault();

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
                    User_Id = user.Userid,
                    RegistrationNo = businessUserView.RegistrationNo,
                    PanNo = businessUserView.PanNo,
                    Registration_Image = businessUserView.Registration_Image,
                    Pan_Image = businessUserView.Pan_Image,
                    About = businessUserView.About


                };
                _dal.BusinessUsers.Add(businessUser);
                _dal.SaveChanges();

            }
            catch (Exception e) {



            }



            return View();
        }
        [HttpGet]
        [Route("businessRequest")]
        public IActionResult BusinessRequest() {
            var businessRequest = _dal.BusinessUserRequests.ToList();
            ViewBag.BusinessRequest = businessRequest;
            return View();

        }
        [HttpGet]
        [Route("viewbusiness/{id}")]
        public IActionResult ViewBusiness(int id)
        {
            var businessRequest = _dal.BusinessUserRequests.Where(x => x.BusinessRegId == id).FirstOrDefault();
            ViewBag.BusinessRequest = businessRequest;
            return View();

        }


        [Route("acceptbusiness/{id}")]
        public IActionResult AcceptBusiness(int id) {
            Random r = new Random();
            int randNum = r.Next(100000000);
            string password = randNum.ToString("D8");


            var request = _dal.BusinessUserRequests.Where(x => x.BusinessRegId == id).FirstOrDefault();

            var user = new User() {
                Username = request.Username,
            Email = request.Email,
            Password = Encryption(password),
            Address = request.Address,
            Gender = request.Gender,
            UserRole = request.UserRole,
            Contact = request.Contact,
            Photo = request.Photo,
        };
            _dal.Users.Add(user);
            _dal.SaveChanges();

            var businessDetail = new BusinessUser()
            {
                User_Id= user.Userid,
                Office_No=request.Office_No,
                RegistrationNo=request.RegistrationNo,
                PanNo=request.PanNo,
                About=request.About,
                Registration_Image=request.Registration_Image,
                Pan_Image=request.Pan_Image

            };

            _dal.BusinessUsers.Add(businessDetail);
            
            SendApprove(user.Email, password);
            _dal.Remove(_dal.BusinessUserRequests.Single(a => a.BusinessRegId == id));
            _dal.SaveChanges();

            return RedirectToAction("Index", "Admin");

        }

        public bool Reject([FromForm]RejectSend rejectSend) {

            var user = _dal.BusinessUserRequests.Where(x => x.BusinessRegId == rejectSend.Id).FirstOrDefault();
            var email = user.Email;
            var Subject = "Rejected : Business User Request Rejected !";
            SendMail sendEmail = new SendMail();

             
            var response = sendEmail.SendEmail(email,Subject, rejectSend.Message);
            if (response)
            {
                _dal.Remove(_dal.BusinessUserRequests.Single(a => a.BusinessRegId == rejectSend.Id));
                _dal.SaveChanges();
                return true;
            }
            else {
                return false;
            }

            
        }

        public void SendApprove(string replyTo, string Password) {
            string subject = "Approved : Business Account request Approved";
                string message = "Welcome to the Bazarghar family.\n The request to create a business account " +
                "was approved. You can login to your account in Bazarghar using :\n Email : "+replyTo+"\n"+"Password : "+Password+"\n Thank You.";

            SendMail sendMail = new SendMail();
            var res = sendMail.SendEmail(replyTo, subject, message);
        
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
