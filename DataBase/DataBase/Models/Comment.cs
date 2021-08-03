using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Comment
    {
        public Comment()
        {

        }
        public int id { get; set; }
        public int productid { get; set; }
        public int userid { get; set; }
        public string text { get; set; }

        public System.DateTime createdate { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual List<CommentReplay> CommentReplays { get; set; }
    }
}
