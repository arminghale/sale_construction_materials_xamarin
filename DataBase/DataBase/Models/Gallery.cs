using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Gallery
    {
        public Gallery()
        {

        }
        public int id { get; set; }
        public int productid { get; set; }
        public string imagename { get; set; }
        public virtual Product Product { get; set; }
    }
}
