using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Address
    {
        public Address()
        {

        }
        public int id { get; set; }
        public int personid { get; set; }
        public string ostan { get; set; }
        public string shahr { get; set; }
        public string codeposti { get; set; }
        public string text { get; set; }
        public string girandename { get; set; }
        public string girandelastname { get; set; }
        public string girandephonenumber { get; set; }

        public string ad
        {
            get
            {
                return string.Format("{0} - {1} - {2}", ostan, shahr, text);
            }
        }
        public string ad2
        {
            get
            {
                return string.Format("{0} - {1} - {2} :-: {3} {4} - {5}", ostan, shahr, text,girandename,girandelastname,girandephonenumber);
            }
        }
    }
}
