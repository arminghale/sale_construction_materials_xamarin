using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Takhfif
    {
        public Takhfif()
        {

        }
        public int id { get; set; }
        public int productid { get; set; }
        public System.DateTime starttime { get; set; }
        public System.DateTime endtime { get; set; }
        public int darsad { get; set; }
        public bool del { get; set; }

        public string ad
        {
            get
            {
                return string.Format("{0} - {1}  :-:  {2}%", DateDifference.MiladiToShamsi2(starttime), DateDifference.MiladiToShamsi2(endtime), darsad);
            }
        }
        public string ad2
        {
            get
            {
                if (DateDifference.IsActive(endtime))
                {
                    del = true;
                    return "فعال";
                }
                else
                {
                    del = false;
                    return "اتمام زمان تخفیف";
                }
            }
        }
        public virtual Product Product { get; set; }
    }
}
