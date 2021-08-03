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
    public partial class AdminHome : ContentPage
    {
        public AdminHome()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            name.Text = "مدیریت";

            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/IsNewMessage/").Result;
            if (response.IsSuccessStatusCode)
            {
                btn_messages.BackgroundColor = Color.DarkRed;
            }

            btn_prgroup.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AdminPrGroup());
            };

            btn_product.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AdminProduct());
            };

            btn_users.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AdminUser());
            };

            btn_messages.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AdminRecMessage());
            };

            btn_baskets.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AdminBasket());
            };

            btn_changepass.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AdminChangePass(Consts.User.id));
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