using bazargharnext.AllFunction;
using bazargharnext.Models;
using bazargharnext.ModelsView;
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
         IWebHostEnvironment _webHostEnvironment;
        UploadImageFunction UploadImage;
        public RegisterController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            UploadImage = new UploadImageFunction(webHostEnvironment);
        }



        [HttpPost]
        public IActionResult RegisterUser(User users)
        {
            var schema = HttpContext.Request.Headers["Referer"]; ;
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

                return Redirect(schema);

            }
            else
            {
                var regCode = Encryption(SendRegisterEmail(reply_to: users.Email));
                HttpContext.Session.SetString("regCode", regCode);
                HttpContext.Session.SetString("Register", JsonConvert.SerializeObject(users));
                return RedirectToAction("RegistrationConfirm");
             }
         }


        /*registration confirmation */
        public IActionResult RegistrationConfirm()
        {
            
            return View();
        }
        public bool Verify(string VCode)
        {
            var regCode = HttpContext.Session.GetString("regCode");
            VCode = Encryption(VCode);
            if (checkCode(VCode, regCode))
            {
                var users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("Register"));
                users.Password = Encryption(users.Password);
                users.Photo = "/images/profile/user.png";
                users.UserRole = "user";

                _dal.Users.Add(users);
                _dal.SaveChanges();

                HttpContext.Session.Remove("regCode");
                HttpContext.Session.Remove("Register");

                TempData["LoginError"] = "Most be Logged in First ! ";
                return true;
            }
            else
            {
                TempData["Error"] = "Verification code doesnt matched";
                return false;
            }
        }

        public void Resend_Code() {
            var users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("Register"));
            var regCode = Encryption(SendRegisterEmail(reply_to: users.Email));
            HttpContext.Session.SetString("regCode", regCode);
          }

        /**/


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
        [HttpPost]
        public IActionResult BusinessLogin(string email, string password)
        {
            
            User user = _dal.Users.SingleOrDefault(x => x.Email.Equals(email));

            if (user != null)
            {
                bool isValidPassword = Encryption(password).Equals(user.Password);
                if (isValidPassword)
                {
                    if (user.UserRole.Equals("business"))
                    {
                        HttpContext.Session.SetString("isLoggedin", "true");

                        HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                        return RedirectToAction("Index", "Business");
                    }
                    else {
                        TempData["LoginError"] = "User is not a business User!" ;
                        return RedirectToAction("Login", "Business");
                    }
                }
                else
                {
                    TempData["LoginError"] = "Email or Password Doesn`t Match ! ";
                    return RedirectToAction("Login","Business");
                }
            }
            else
            {
                TempData["LoginError"] = "User Doesn`t Exist ! ";
                return RedirectToAction("Login", "Business");
            }
        }
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("User");

            HttpContext.Session.SetString("isLoggedin", "false"); 
            HttpContext.Session.SetString("userAs", "user");
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
        //forget password
        public IActionResult ForgetPassword() {

            return View();
        }

        public bool ForgetPasswordSendEmail([FromForm]string email) {

           var userExist = _dal.Users.ToList().Exists(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            if (userExist)
            {
                User users = _dal.Users.Where(x => x.Email == email).Select(u => new User()
                {
                    Userid = u.Userid,
                    Username=u.Username,
                    Email=u.Email
                
                }).FirstOrDefault();
                
                HttpContext.Session.SetString("ForgetUser", JsonConvert.SerializeObject(users));
                
                SendForgetPasswordEmail(email);
                return true;
            }
            else {
                return false;
            }
        }
        public void Resend_Forget_Password_Code()
        {
            if (HttpContext.Session.GetString("ForgetUser") != null)
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("ForgetUser"));

                SendForgetPasswordEmail(user.Email);
            }
            else {

                var id = "sdsd ";
            }
        }

        public bool Forget_Password_Verify_Code([FromForm]string VCode ) {
            var regCode = HttpContext.Session.GetString("ForgetPassword");
            VCode = Encryption(VCode);
            if (checkCode(VCode, regCode))
            {
                return true;
            }
            else {
                return false;
            }
        }
        public IActionResult ChangeForgetPassword([FromForm] string password)
        {
            
            password = Encryption(password);
            var user= JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("ForgetUser"));
            var users = new User() { Userid = user.Userid, Password = password };

            _dal.Users.Attach(users);
            _dal.Entry(users).Property(x => x.Password).IsModified = true;
            _dal.SaveChanges();
            HttpContext.Session.Remove("ForgetUser");
            HttpContext.Session.Remove("ForgetPassword");
            return RedirectToAction("Index","Home");
        }

        //forget Password

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

        public string SendRegisterEmail(string reply_to) {
            
            Random r = new Random();
            int randNum = r.Next(1000000);
            string code = randNum.ToString("D6");


            
            string reply_subject = code +" Verification code for bazarghar";
            string messageDetail = "Please us following code to register the account.<br/><h1>"+code+"</h1>";
            
            SendMail sendMail = new SendMail();
            sendMail.SendEmail(reply_to, reply_subject, messageDetail);

            return code;
        }
        public void SendForgetPasswordEmail(string reply_to)
        {

            Random r = new Random();
            int randNum = r.Next(1000000);
            string code = randNum.ToString("D6");
            HttpContext.Session.SetString("ForgetPassword", Encryption(code));
            string reply_subject = code + " Password reset code for bazarghar";
            string messageDetail = "<h3>Please us following code to reset your password of bazarghar's account.</h3><br/><h1>" + code + "</h1>";
            SendMail sendMail = new SendMail();
            sendMail.SendEmail(reply_to, reply_subject, messageDetail);
        }

        public bool checkCode(string userCode,string systemCode) {
            if (userCode.Equals(systemCode))
            {
                return  true;
            }
            else {
                return false;

            }

        }

        [HttpPost]
        public async Task<bool> RegisterBusiness([FromForm]BusinessUserView buv) {
            string LogoFolder = "images/BusinessLogo/";
            string RegistrationFolder = "images/Registration/";
            string PanFolder = "images/PAN/";

            var businessUserRequest = new BusinessUserRequest()
            {
                Username = buv.Username,
                Email = buv.Email,
                Password = "",
                Gender = "none",
                Address = buv.Address,
                Contact = buv.Contact,
                Photo = await UploadImage.UploadImage(LogoFolder, buv.Photo),
                UserRole = "business",
                Office_No = buv.Office_No,
                RegistrationNo = buv.RegistrationNo,
                PanNo = buv.PanNo,
                Registration_Image = await UploadImage.UploadImage(RegistrationFolder, buv.Registration_Image),
                Pan_Image=await UploadImage.UploadImage(PanFolder,buv.Pan_Image),
                About=buv.About
            };

            _dal.BusinessUserRequests.Add(businessUserRequest);
            await _dal.SaveChangesAsync();

            





            return true;
        
        }


    }
  


}
