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
    public partial class AnbarChangePass : ContentPage
    {
        public AnbarChangePass(int id)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            btn_reset.Clicked += async (s, e) =>
            {
                if (string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(repassword.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    if (password.Text.Length<8)
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
                            var formVariables = new List<KeyValuePair<string, string>>();
                            formVariables.Add(new KeyValuePair<string, string>("id", id.ToString()));
                            formVariables.Add(new KeyValuePair<string, string>("password", password.Text));
                            formVariables.Add(new KeyValuePair<string, string>("repassword", repassword.Text));
                            formVariables.Add(new KeyValuePair<string, string>("currentpassword", currentpassword.Text));
                            var formContent = new FormUrlEncodedContent(formVariables);
                            var response2 = Consts.client.PostAsync("http://localhost:1212/Anbar/api/ChangePass", formContent).Result;
                            if (response2.IsSuccessStatusCode)
                            {
                                await DisplayAlert("خطا!", "با موفقیت انجام شد.", "باشه");
                                await Navigation.PushAsync(new UserHome());
                            }
                            else
                            {
                                await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
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