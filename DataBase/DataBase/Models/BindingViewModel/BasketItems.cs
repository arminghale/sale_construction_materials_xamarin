using DataBase.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class BasketItems
    {
        public ObservableCollection<BasketItem> items { get; set; }
        public BasketItems()
        {
            if (AdminBasket.id!=0)
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/BasketDetails/" + AdminBasket.id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<Basket>(response.Content.ReadAsStringAsync().Result);
                    items = new ObservableCollection<BasketItem>();
                    foreach (var item in list.BasketItems)
                    {
                        items.Add(item);
                    }
                }
            }
            else
            {
                var response = Consts.client.GetAsync("http://localhost:1212/User/api/OneOrders/" + UserOrders.id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<Basket>(response.Content.ReadAsStringAsync().Result);
                    items = new ObservableCollection<BasketItem>();
                    foreach (var item in list.BasketItems)
                    {
                        items.Add(item);
                    }
                }
            }
            
        }
    }
}
