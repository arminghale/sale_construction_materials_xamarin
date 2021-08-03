using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class CommentReplay
    {
        public CommentReplay()
        {

        }
        public int id { get; set; }
        public int commentid { get; set; }
        public string text { get; set; }

        public System.DateTime createdate { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
