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
    public partial class Register1 : ContentPage
    {
        public Register1()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            btn_register.Clicked += async (s, e) =>
            {
                if (string.IsNullOrEmpty(username.Text)  || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(repassword.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    if (password.Text.Length < 8)
                    {
                        await DisplayAlert("خطا!", "رمز عبور باید بیشتر از 8 کاراکتر باشد.", "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                    else
                    {
                        if (password.Text != repassword.Text)
                        {
                            await DisplayAlert("خطا!", "رمز عبور و تکرار رمز عبور متفاوت اند.", "باشه");
                            ActivitySpinner.IsRunning = false;
                        }
                        else
                        {
                            ActivitySpinner.IsRunning = true;
                            var user = new StringContent(JsonConvert.SerializeObject(new { username = username.Text, password = password.Text, email = email.Text, repassword = repassword.Text }), Encoding.UTF8, "application/json");

                            var response = Consts.client.PostAsync("http://localhost:1212/api/register", user).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                await Navigation.PushAsync(new ActiveCode(JsonConvert.DeserializeObject<ActiveID>(response.Content.ReadAsStringAsync().Result).id, true));
                            }
                            else
                            {
                                await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response.Content.ReadAsStringAsync().Result).message, "باشه");
                                ActivitySpinner.IsRunning = false;
                            }
                        }
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