using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Tag
    {
        public Tag()
        {

        }
        public int id { get; set; }
        public int productid { get; set; }
        public string text { get; set; }
        public virtual Product Product { get; set; }
    }
}
