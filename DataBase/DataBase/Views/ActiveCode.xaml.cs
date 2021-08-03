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
    public partial class ActiveCode : ContentPage
    {
        public ActiveCode(int id,bool isregister)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            btn_code.Clicked += async (s, e) =>
            {
                if (string.IsNullOrEmpty(code.Text))
                {
                    await DisplayAlert("خطا!", "لطفا فیلد را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    ActivitySpinner.IsRunning = true;
                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("id", id.ToString()));
                    formVariables.Add(new KeyValuePair<string, string>("activecode", code.Text));
                    var formContent = new FormUrlEncodedContent(formVariables);
                    var response2 = Consts.client.PostAsync("http://localhost:1212/api/ActiveAccount/" + id, formContent).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        if (isregister)
                        {
                            await Navigation.PushAsync(new Register2(id));
                        }
                        else
                        {
                            await Navigation.PushAsync(new ChangePass(id, code.Text));
                        }

                    }
                    else
                    {
                        await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                }
            };

            btn_repeat.Clicked += async (s, e) =>
            {
                ActivitySpinner.IsRunning = true;
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("id", id.ToString()));
                var formContent = new FormUrlEncodedContent(formVariables);
                var response2 = Consts.client.PostAsync("http://localhost:1212/api/SendActiveCode/" + id, formContent).Result;
                if (response2.IsSuccessStatusCode)
                {
                    await DisplayAlert("موفق", "کد جدید مجددا برای شما ارسال شد", "مرسی");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    await DisplayAlert("خطا!", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    ActivitySpinner.IsRunning = false;
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