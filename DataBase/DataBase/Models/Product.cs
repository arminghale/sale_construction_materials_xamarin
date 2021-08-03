using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataBase
{
    public class Product
    {
        public Product()
        {

        }
        public int id { get; set; }
        public int prgroupid { get; set; }
        public string title { get; set; }
        public int price { get; set; }
        public string imagename { get; set; }
        public int count { get; set; }
        public int readyday { get; set; }
        public int sendday { get; set; }
        public string text { get; set; }
        public System.DateTime createdate { get; set; }
        public int seen { get; set; }
        public bool istakhfif = false;

        public ImageSource imagesource
        {
            get
            {
                return ImageSource.FromUri(new Uri("http://localhost:1212/Files/Thumb/" + imagename));
            }
        }
        public string pricestring
        {
            get
            {
                if (price>=1000000)
                {
                    return price.ToString("###/###/000") + "تومان";
                }
                return price.ToString("###/000") + "تومان";
            }
        }
        public string pricewithtakhfif
        {
            get
            {
                var response3 = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductBaTakhfif/"+id).Result;
                if (response3.IsSuccessStatusCode)
                {
                    if (int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message)<price)
                    {
                        istakhfif = true;
                    }
                    if (int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message) >= 1000000)
                    {
                        return int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message).ToString("###/###/000") + "تومان";
                    }
                    return int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message).ToString("###/000")+ "تومان";
                }
                return price.ToString("###/000") + "تومان";
            }
        }
        public string pricewithtakhfif2
        {
            get
            {
                var response3 = Consts.client.GetAsync("http://localhost:1212/api/ProductBaTakhfif/" + id).Result;
                if (response3.IsSuccessStatusCode)
                {
                    if (int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message) < price)
                    {
                        istakhfif = true;
                        if (int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message)>=1000000)
                        {
                            return "با تخفیف " + int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message).ToString("###/###/000") + "تومان";
                        }
                        return "با تخفیف " + int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message).ToString("###/000") + "تومان";
                    }
                    else
                    {
                        istakhfif = false;
                        return null;
                    }
                }
                return null;
            }
        }

        public int pricewithtakhfifuse
        {
            get
            {
                var response3 = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductBaTakhfif/" + id).Result;
                if (response3.IsSuccessStatusCode)
                {
                    return int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message);
                }
                return price;
            }
        }

        public virtual PrGroup PrGroup { get; set; }
        public virtual List<FillField> FillFields { get; set; }
        public virtual List<Gallery> Galleries { get; set; }
        public virtual List<Takhfif> Takhfifs { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<BasketItem> BasketItems { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
