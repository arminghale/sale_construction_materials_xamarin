using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class FillField
    {
        public FillField()
        {

        }
        public int id { get; set; }
        public int fieldid { get; set; }
        public int productid { get; set; }
        public string text { get; set; }

        public string FieldString
        {
            get
            {
                return Field.title;
            }
        }
        public virtual Field Field { get; set; }
        public virtual Product Product { get; set; }
    }
}
