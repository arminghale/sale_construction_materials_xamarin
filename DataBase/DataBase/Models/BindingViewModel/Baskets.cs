using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class Baskets
    {
        public ObservableCollection<Basket> orders { get; set; }
        public Baskets()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/BasketIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Basket>>(response.Content.ReadAsStringAsync().Result);
                orders = new ObservableCollection<Basket>();
                foreach (var item in list)
                {
                    orders.Add(item);
                }
            }
        }
    }
}
