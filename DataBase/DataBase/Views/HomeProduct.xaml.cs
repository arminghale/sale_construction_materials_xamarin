using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeProduct : ContentPage
    {
        private int selectimage = 0;
        public HomeProduct(Product product)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (Consts.isUserLoged&&Consts.User.roleid==2)
            {
                basket.IsVisible = true;
            }
            btn_add.Clicked +=async (s, e) =>
            {
                if (Consts.isUserLoged && Consts.User.roleid == 2)
                {
                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("userid", Consts.User.id.ToString()));
                    formVariables.Add(new KeyValuePair<string, string>("prid", product.id.ToString()));
                    formVariables.Add(new KeyValuePair<string, string>("tedad", tedad.Text));
                    var formContent = new FormUrlEncodedContent(formVariables);
                    var response5 = Consts.client.PostAsync("http://localhost:1212/api/BasketAdd/", formContent).Result;
                    if (response5.IsSuccessStatusCode)
                    {
                        var list = JsonConvert.DeserializeObject<Basket>(response5.Content.ReadAsStringAsync().Result);
                        await DisplayAlert("موفق", "با موفقیت انجام شد", "مرسی");
                        await Navigation.PushAsync(new HomeBasket(list));
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response5.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
                else
                {
                    await Navigation.PushAsync(new Login());
                }
            };

            ObservableCollection<ShowImages> showImages = new ObservableCollection<ShowImages>();
            showImages.Add(new ShowImages { ImageUrl= "http://localhost:1212/Files/"+product.imagename });
            foreach (var item in product.Galleries)
            {
                showImages.Add(new ShowImages { ImageUrl = "http://localhost:1212/Files/Gallery/" + item.imagename });
            }
            image.Source = ImageSource.FromUri(new Uri(showImages[selectimage].ImageUrl));
            pev.IsVisible = false;
            if (showImages.Count == 1)
            {
                next.IsVisible = false;
                pev.IsVisible = false;
            }
            btn_next.Clicked += (s, e) =>
            {
                selectimage++;
                image.Source = ImageSource.FromUri(new Uri(showImages[selectimage].ImageUrl));
                if (selectimage == 0)
                {
                    pev.IsVisible = false;
                    next.IsVisible = true;
                }else
                if (selectimage == showImages.Count - 1)
                {
                    next.IsVisible = false;
                    pev.IsVisible = true;
                }
                else
                {
                    next.IsVisible = true;
                    pev.IsVisible = true;
                }
            };
            btn_pev.Clicked += (s, e) =>
            {
                selectimage--;
                image.Source = ImageSource.FromUri(new Uri(showImages[selectimage].ImageUrl));
                if (selectimage == 0)
                {
                    pev.IsVisible = false;
                    next.IsVisible = true;
                }
                else
                if (selectimage == showImages.Count - 1)
                {
                    next.IsVisible = false;
                    pev.IsVisible = true;
                }
                else
                {
                    next.IsVisible = true;
                    pev.IsVisible = true;
                }
            };
            
            
            var response = Consts.client.GetAsync("http://localhost:1212/api/Groups/").Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                foreach (var item in list)
                {
                    if (item.id==product.prgroupid)
                    {
                        product.PrGroup = item;
                    }
                }
            }

            title.Text = product.title;
            prgroup.Text = product.PrGroup.title;
            text.Text = product.text;
            price.Text="قیمت به ازای هر "+product.PrGroup.vahed+" "+product.pricestring;
            var response3 = Consts.client.GetAsync("http://localhost:1212/api/ProductBaTakhfif/" + product.id).Result;
            if (response3.IsSuccessStatusCode)
            {
                if (int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message) < product.price)
                {
                    pricetakhfif.Text= "با تخفیف " + int.Parse(JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message).ToString("###/000") + "تومان";
                }
            }

            foreach (var item in product.FillFields)
            {
                foreach (var item2 in product.PrGroup.Fields)
                {
                    if (item.fieldid==item2.id)
                    {
                        item.Field = item2;
                    }
                }
            }
            field.ItemsSource = product.FillFields;
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