using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class Basket
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int addressid { get; set; }
        public int total { get; set; }
        public bool ispay { get; set; }
        public bool isready { get; set; }
        public bool issend { get; set; }
        public bool iscansel { get; set; }
        public System.DateTime createdate { get; set; }
        public System.DateTime paydate { get; set; }
        public System.DateTime senddate { get; set; }

        public int paymentid { get; set; }

        public virtual User User { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<BasketItem> BasketItems { get; set; }

        public bool sendbool { get; set; }
        public bool readybool { get; set; }
        public bool canselbool { get; set; }

        public string total2
        {
            get
            {
                if (total>=1000000)
                {
                    return string.Format("{0}تومان", total.ToString("###/###/000"));
                }
                return string.Format("{0}تومان", total.ToString("###/000"));
            }
        }

        public string vaziat
        {
            get
            {
                if (iscansel==true)
                {
                    sendbool = false;
                    readybool = false;
                    canselbool = false;
                    return "کنسل شده";
                }
                if (isready==false)
                {
                    sendbool = false;
                    readybool = true;
                    canselbool = true;
                    return "در حال بررسی";
                }
                else if(isready==true&&issend==false)
                {
                    sendbool = true;
                    readybool = false;
                    canselbool = true;
                    return "آماده شده";
                }
                else if (isready == true && issend == true)
                {
                    sendbool = false;
                    readybool = false;
                    canselbool = false;
                    return "ارسال شده";
                }
                else
                {
                    sendbool = false;
                    readybool = true;
                    canselbool = true;
                    return "پرداخت شده";
                }
            }
        }

        public string title
        {
            get
            {
                if (total >= 1000000)
                {
                    return "مبلغ کل: " + total.ToString("###/###/000") + " -- تاریخ پرداخت: " + DateDifference.MiladiToShamsi(paydate).Split(' ')[0];
                }
                return "مبلغ کل: "+ total.ToString("###/000")+" -- تاریخ پرداخت: "+ DateDifference.MiladiToShamsi(paydate).Split(' ')[0];
            }
        }
    }
}
