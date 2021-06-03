using bazargharnext.Models;
using bazargharnext.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class GetAllProductsByUserId
    {
         DataContext _dal;
        public IEnumerable<MyProductView> GetProductByUserId(int id)
        {
            _dal = new DataContext();
            if (_dal.Products.Any())
            {
                IEnumerable<MyProductView> myProductView = _dal.Products.Where(x => x.Userid == id).Select(product => new MyProductView()
                {
                    Product_Id = product.Product_Id,
                    Product_name = product.Product_name,
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
                    Comments = product.Comments.Select(comments => new MyCommentView()
                    {
                        Product_Id = comments.Product_Id,
                        Comment_Id = comments.Comment_Id,
                        Comment_Text = comments.Comment_Text,
                        User_By = comments.User_By,
                        Reply_To = comments.Reply_To,
                        React = comments.React,
                        Date = comments.Date
                    }).ToList()


                }).ToList();
                _dal.Dispose();
                return myProductView;
            }
            return new List<MyProductView>();
        }
        

    }
}
