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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            btn_login.Clicked += async (s, e) =>
            {
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
                {
                    await DisplayAlert("خطا!", "لطفا تمام فیلد ها را تکمیل کنید.", "باشه");
                    ActivitySpinner.IsRunning = false;
                }
                else
                {
                    Consts.Username = username.Text;
                    Consts.Password = password.Text;
                    ActivitySpinner.IsRunning = true;
                    string result = Consts.checkInfo();
                    if (result == "ok")
                    {
                        Consts.isUserLoged = true;
                        if (!Consts.User.isactive&& Consts.User.Person==null)
                        {
                            await DisplayAlert("خطا", "حساب شما فعال نیست", "فعالسازی");
                            var formVariables = new List<KeyValuePair<string, string>>();
                            formVariables.Add(new KeyValuePair<string, string>("id", Consts.User.id.ToString()));
                            var formContent = new FormUrlEncodedContent(formVariables);
                            var response2 = Consts.client.PostAsync("http://localhost:1212/api/SendActiveCode/" + Consts.User.id, formContent).Result;
                            await Navigation.PushAsync(new ActiveCode(Consts.User.id,true));
                        }
                        else if (!Consts.User.isactive && Consts.User.Person != null)
                        {
                            await DisplayAlert("خطا", "حساب شما فعال نیست", "فعالسازی");
                            var formVariables = new List<KeyValuePair<string, string>>();
                            formVariables.Add(new KeyValuePair<string, string>("id", Consts.User.id.ToString()));
                            var formContent = new FormUrlEncodedContent(formVariables);
                            var response2 = Consts.client.PostAsync("http://localhost:1212/api/SendActiveCode/" + Consts.User.id, formContent).Result;
                            await Navigation.PushAsync(new ActiveCode(Consts.User.id, false));
                        }
                        else if (Consts.User.isactive && Consts.User.Person == null&& Consts.User.roleid==2)
                        {
                            await DisplayAlert("خطا", "اطلاعات کاربری شما تکمیل نیست", "تکمیل");
                            await Navigation.PushAsync(new Register2(Consts.User.id));
                        }
                        else
                        {
                            await DisplayAlert("موفق", "با موفقیت وارد شدید.", "مرسی");
                            await Navigation.PushAsync(new Home());
                        }
                    }
                    else
                    {
                        await DisplayAlert("خطا!", result, "باشه");
                        ActivitySpinner.IsRunning = false;
                    }
                }
            };

            btn_register.Clicked += async (s, e) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await Navigation.PushAsync(new Register1());
                }
                else
                {
                    await Navigation.PushAsync(new NotNet());
                }
            };

            btn_forget.Clicked += async (s, e) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await Navigation.PushAsync(new ForgetPass());
                }
                else
                {
                    await Navigation.PushAsync(new NotNet());
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