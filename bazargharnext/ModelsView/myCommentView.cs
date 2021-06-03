using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.ModelsView
{
    public class MyCommentView
    {

        public int Comment_Id { get; set; }
        public int Product_Id { get; set; }
        public string Comment_Text { get; set; }
        public int User_By { get; set; }
        public int Reply_To { get; set; }
        public DateTime Date { get; set; }
        public bool React { get; set; }

        public string SenderProfile { get; set; }



    }
}
