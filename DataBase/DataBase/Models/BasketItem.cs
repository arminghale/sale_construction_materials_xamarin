using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase
{
    public class BasketItem
    {
        public BasketItem()
        {

        }
        public int id { get; set; }
        public int basketid { get; set; }
        public int productid { get; set; }
        public int count { get; set; }
        public int mablagh { get; set; }

        public string mab
        {
            get
            {
                return mablagh + "تومان";
            }
        }
        public string co
        {
            get
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupIndex/").Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                    return count + list.FirstOrDefault(w => w.id == Product.prgroupid).vahed;
                }
                return count.ToString();
            }
        }
        public string title
        {
            get
            {
                return Product.title+"--"  + (mablagh / count) + "تومان";
            }
        }
        public virtual Product Product { get; set; }
        public virtual Basket Basket { get; set; }
    }
}
