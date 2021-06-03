using bazargharnext.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;
using bazargharnext.ModelsView;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using bazargharnext.AllFunction;

namespace bazargharnext.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private DataContext _dal=new DataContext();
        GetAllProductsByUserId getAllProductsByUserId = new GetAllProductsByUserId();
        UploadImageFunction uploadImage;
        User user;
        public MyProfileController( IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
           uploadImage = new UploadImageFunction(hostEnvironment);
        }

        public async Task<ViewResult> Index()
        {

            user = JsonConvert.DeserializeObject<User>(value: HttpContext.Session.GetString("User"));
            ViewBag.MyProfile = user;

            var data = getAllProductsByUserId.GetProductByUserId(user.Userid);
            ViewBag.Product = data;        

            return await Task.FromResult(View());
        }
        public IActionResult UpdateProfile()
        {
            user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            ViewBag.MyProfile = user;
            if (user.Gender.Equals("Male"))
            {
                ViewBag.Male = true;
            }
            else {
                ViewBag.Female = true;
            }

            return (IActionResult)View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(MyProfileView model) {
            _dal = new DataContext();
            user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

           

            String uniqueFileName;
            if (model.Photo == null)
            {
                uniqueFileName = user.Photo;
            }
            else { 
            uniqueFileName =await UploadedFile(model,user.Photo);
                
               

            }
            var users = new User() {
                Userid = user.Userid,
                Username = model.Username,
                Email=model.Email,
                Address=model.Address,
                Gender=model.Gender,
                Contact=model.Contact,
                Password=user.Password,
                  Photo=uniqueFileName
               };

          
            _dal.Entry(users).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
             
           _dal.SaveChanges();
            _dal.Dispose();
          

             UpdateSession(users.Userid);
              TempData["PhotoUpload"] = "updated";
              return RedirectToAction("UpdateProfile", "MyProfile");


        }

        private void UpdateSession(int Userid)
        {
            _dal = new DataContext();
            User user = _dal.Users.SingleOrDefault(x => x.Userid == Userid);
            HttpContext.Session.Remove("User");
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
            _dal.Dispose();
        }

        private async Task<string> UploadedFile(MyProfileView model,string oldFileName)
        {
           
            string filePath = null;
            string folder = "images/profile/";
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                /* string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                  filePath = Path.Combine(uploadsFolder, uniqueFileName);
                 using var fileStream = new FileStream(filePath, FileMode.Create);
                 model.Photo.CopyTo(fileStream);
                */
                filePath = await uploadImage.UploadImage(folderPath: folder, file: model.Photo);

                //removing from folder
                string oldPath = Path.Combine(uploadsFolder, oldFileName);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

            }
            return filePath;
        }

        //repo
        
        //


    }
}