using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataBase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminDetailsUser : ContentPage
    {
        public AdminDetailsUser(User user)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (user != null)
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/RoleIndex/").Result;
                if (response.IsSuccessStatusCode)
                {
                   var list = JsonConvert.DeserializeObject<List<Role>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in list)
                    {
                        if (item.title != "admin")
                        {
                            if (user != null && item.id == user.roleid)
                            {
                                roleid.Text = item.title;
                            }
                        }
                    }
                }

                username.Text = user.username;
                password.Text = user.password;
                email.Text = user.email;
                if (user.Person!=null)
                {
                    personal.IsVisible = true;
                    personal2.IsVisible = true;
                    name.Text = user.Person.name;
                    lastname.Text = user.Person.lastname;
                    phonenumber.Text = user.Person.phonenumber;
                }
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