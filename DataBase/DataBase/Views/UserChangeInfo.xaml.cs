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
    public partial class UserChangeInfo : ContentPage
    {
        public UserChangeInfo()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            name.Text = Consts.User.Person.name;
            lastname.Text = Consts.User.Person.lastname;
            phonenumber.Text = Consts.User.Person.phonenumber;
            email.Text = Consts.User.email;
            username.Text = Consts.User.username;

            btn_change.Clicked += async (s, e) => 
            {
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(lastname.Text) || string.IsNullOrEmpty(phonenumber.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    ActivitySpinner.IsRunning = true;
                    var user = new StringContent(JsonConvert.SerializeObject(new { name = name.Text, username = username.Text, lastname = lastname.Text, email = email.Text, phonenumber = phonenumber.Text }), Encoding.UTF8, "application/json");

                    var response = Consts.client.PostAsync("http://localhost:1212/User/api/ChangeInfo", user).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("خطا!", "با موفقیت انجام شد.", "باشه");
                        await Navigation.PushAsync(new UserHome());
                    }
                    else
                    {
                        await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                }
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