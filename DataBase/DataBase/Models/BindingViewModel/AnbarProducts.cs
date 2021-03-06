using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataBase
{
   public class AnbarProducts
    {
        public ObservableCollection<Product> products { get; set; }
        public AnbarProducts()
        {
            var response = Consts.client.GetAsync("http://localhost:1212/Anbar/api/ProductIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);
                products = new ObservableCollection<Product>();
                foreach (var item in list)
                {
                    products.Add(item);
                }
            }
        }
    }
}
