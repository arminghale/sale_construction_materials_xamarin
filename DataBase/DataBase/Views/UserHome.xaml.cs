using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserHome : ContentPage
    {
        public UserHome()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (Consts.User.Person!=null)
            {
                name.Text = Consts.User.Person.name + " " + Consts.User.Person.lastname;
            }
            else
            {
                name.Text = Consts.User.username;
            }

            btn_basket.Clicked += async (s, e) =>
             {
                 var response2 = Consts.client.GetAsync("http://localhost:1212/api/LastBasket/" + Consts.User.id).Result;
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

            btn_contactus.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new ContactUS());
            };

            btn_changepass.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new UserChangePass(Consts.User.id));
            };

            btn_changeinfo.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new UserChangeInfo());
            };

            btn_address.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new UserAddress());
            };

            btn_messages.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new UserMessages());
            };

            btn_orders.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new UserOrders());
            };

            btn_logout.Clicked += async (s, e) =>
            {
                Consts.isUserLoged = false;
                Consts.Username = null;
                Consts.Password = null;
                Consts.User = null;
                Consts.Token = null;
                Consts.client.DefaultRequestHeaders.Authorization = null;
                await Navigation.PushAsync(new Home());
            };
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
                await Navigation.PushAsync(new UserHome());
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