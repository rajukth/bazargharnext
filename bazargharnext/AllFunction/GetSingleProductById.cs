using bazargharnext.Models;
using bazargharnext.ModelsView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class GetSingleProductById
    {
        DataContext _dal;

        public async Task<MyProductView> GetProductById(int id)
        {
            _dal = new DataContext();
            MyProductView myProductView= await _dal.Products.Where(x => x.Product_Id == id).Select(product => new MyProductView()
            {
                Product_Id = product.Product_Id,
                Product_name = product.Product_name,
                Userid=product.Userid,
                Price = product.Price,
                Category_id = product.Category_id,
                Category_name = _dal.Category.Where(x => x.Category_id == product.Category_id).First().Category_name,
                Date = product.Date,
                Description = product.Description,
                Gallery = product.GalleryModel.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList(),
                Product_Details = product.Product_Details.Select(p => new Product_Details()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description
                }).ToList(),
                Comments = product.Comments.Select(comments=>new MyCommentView()
                {
                    Product_Id = comments.Product_Id,
                    Comment_Id = comments.Comment_Id,
                    Comment_Text = comments.Comment_Text,
                    User_By = comments.User_By,
                    Reply_To = comments.Reply_To,
                    React = comments.React,
                    Date = comments.Date,
                    SenderProfile= _dal.Users.Where(x => x.Userid == comments.User_By).First().Photo

                }).ToList()



            }).FirstOrDefaultAsync();
            _dal.Dispose();
            return myProductView;
        }

    }
}
