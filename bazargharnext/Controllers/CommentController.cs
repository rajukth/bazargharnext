using bazargharnext.Models;
using bazargharnext.ModelsView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Controllers
{
    public class CommentController : Controller
    {
        DataContext _dal;
        User users = new User();
        [HttpPost]
        public async Task<MyCommentView> PostComment(int id,Comments comment) {
            _dal = new DataContext();
            users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            Product mpv = _dal.Products.First(c => c.Product_Id == id);
            MyCommentView myCommentView=new MyCommentView();

            //code to update database tables 
            bool SaveFailed;
            do
            {
                SaveFailed = false;
                try
                {
                    mpv.Comments = new List<Comments>();
                    
                    var comments=new Comments()
                    {
                        Comment_Text = comment.Comment_Text,
                        Product_Id = comment.Product_Id,
                        Reply_To = comment.Reply_To,
                        User_By = users.Userid,
                        Date = DateTime.Now
                    };
                    mpv.Comments.Add(comments);
                    int comment_id = await _dal.SaveChangesAsync();
                    myCommentView = new MyCommentView()
                    {
                        Comment_Id = comment_id,
                        Comment_Text = comments.Comment_Text,
                        Reply_To = comments.Reply_To,
                        User_By = comments.User_By,
                        Date = comments.Date,
                        SenderProfile = _dal.Users.Where(x => x.Userid == comments.User_By).First().Photo,
                        React = false

                    };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    SaveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }
            } while (SaveFailed);
//

            
            _dal.Dispose();
            return myCommentView;
        }

        
    }
}
