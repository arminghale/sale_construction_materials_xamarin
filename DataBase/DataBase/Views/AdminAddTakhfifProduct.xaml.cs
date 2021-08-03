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
    public partial class AdminAddTakhfifProduct : ContentPage
    {
        public AdminAddTakhfifProduct(int id)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
           
            btn_laghv.Clicked += async (s, e) =>
              {
                  await Navigation.PopAsync();
              };

            btn_taiid.Clicked += async (s, e) =>
            {
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("id", id.ToString()));
                formVariables.Add(new KeyValuePair<string, string>("darsad", darsad.Text));
                formVariables.Add(new KeyValuePair<string, string>("start", starttime.Text));
                formVariables.Add(new KeyValuePair<string, string>("end", endtime.Text));
                var formContent = new FormUrlEncodedContent(formVariables);
                var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/ProductTakhfif", formContent).Result;
                if (response2.IsSuccessStatusCode)
                {
                    await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                    AdminProduct.id = id;
                    await Navigation.PushAsync(new AdminTakhfifsProduct(id));
                }
                else
                {
                    await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
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