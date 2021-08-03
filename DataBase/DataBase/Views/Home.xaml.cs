using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        private int gid = 0;
        private string selectsort = "";
        public Home()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (Consts.isUserLoged&&Consts.User.roleid==2)
            {
                basket.IsVisible = true;
            }

            btn_basket.Clicked += async (s, e) =>
            {
                var response2 = Consts.client.GetAsync("http://localhost:1212/api/LastBasket/"+Consts.User.id).Result;
                if (response2.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<Basket>(response2.Content.ReadAsStringAsync().Result);
                    await Navigation.PushAsync(new HomeBasket(list));
                }
                else
                {
                    await DisplayAlert("", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                }
            };

            var response = Consts.client.GetAsync("http://localhost:1212/api/Groups/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                var groups = new ObservableCollection<PrGroup>();
                foreach (var item in list)
                {
                    groups.Add(item);
                }
                group.ItemsSource = groups;
            }
            getproduct();

            sort.Items.Add("همه");
            sort.Items.Add("ارزان تر");
            sort.Items.Add("گران تر");
            sort.Items.Add("جدیدترین");
            sort.Items.Add("تخفیف ها");


            sort.SelectedIndexChanged += (s, e) =>
            {
                if (sort.SelectedIndex == 0)
                {
                    gid = 0;
                    selectsort = "";
                    search.Text = "";
                    group.SelectedItem = null;
                }
                if (sort.SelectedIndex==3)
                {
                    selectsort = "createdate";
                }
                if (sort.SelectedIndex == 4)
                {
                    selectsort = "takhfif";
                }
                if (sort.SelectedIndex == 1)
                {
                    selectsort = "arzan";
                }
                if (sort.SelectedIndex == 2)
                {
                    selectsort = "gran";
                }
                
                getproduct();
            };

            btn_search.Clicked += (s, e) =>
            {
                sort.SelectedItem = null;
                getproduct();
            };
        }
        private void getproduct()
        {
            string par = "";
            if (gid!=0)
            {
                par += "gid=" + gid + "&";
            }
            if (!string.IsNullOrEmpty(selectsort))
            {
                par += "sortby=" + selectsort + "&";
            }
            if (!string.IsNullOrEmpty(search.Text))
            {
                par += "search=" + search.Text + "&";
                search.Text = "";
            }
            var response2 = Consts.client.GetAsync("http://localhost:1212/api/Products/?"+par).Result;
            if (response2.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<Product>>(response2.Content.ReadAsStringAsync().Result);
                var products = new ObservableCollection<Product>();
                foreach (var item in list)
                {
                    products.Add(item);
                }
                product.ItemsSource = products;
            }
        }
        private void group_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            gid = ((PrGroup)e.Item).id;
            sort.SelectedItem = null;
            getproduct();
        }
        private async void product_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var response2 = Consts.client.GetAsync("http://localhost:1212/api/Products/" + ((Product)e.Item).id).Result;
            if (response2.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Product>(response2.Content.ReadAsStringAsync().Result);
                await Navigation.PushAsync(new HomeProduct(list));
            }
        }


        private void menu()
        {
            if (Consts.isUserLoged)
            {
                logreg.IsEnabled = false;
            }
            else
            {
                prof.IsEnabled = false;
            }
        }
        private async void btn_notlog(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
        private async void btn_profile(object sender, EventArgs e)
        {
            if (Consts.User.roleid == 2)
            {
              await  Navigation.PushAsync(new UserHome());
            }
            else if (Consts.User.roleid == 1)
            {
                await Navigation.PushAsync(new AdminHome());
            }
            else
            {
                await Navigation.PushAsync(new AnbarHome());
            }
        }
        private async void btn_Home(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }

        
    }
}