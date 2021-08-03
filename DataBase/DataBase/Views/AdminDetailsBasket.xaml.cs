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
    public partial class AdminDetailsBasket : ContentPage
    {
        public AdminDetailsBasket(Basket basket)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/UsersDetails/" + basket.userid).Result;
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
                basket.User = list;
            }
            user.Text = basket.User.Person.name + " " + basket.User.Person.lastname;
            paydate.Text = DateDifference.MiladiToShamsi(basket.paydate);
            payid.Text = basket.paymentid.ToString();
            createdate.Text = DateDifference.MiladiToShamsi(basket.createdate);
            vaziat.Text = basket.vaziat;
            senddate.Text = DateDifference.MiladiToShamsi(basket.senddate);
            if (basket.issend)
            {
                se.IsVisible = true;
            }
            address.Text = basket.Address.ostan + "-" + basket.Address.shahr + "-" + basket.Address.text + "--" + basket.Address.codeposti;
            girande.Text = basket.Address.girandename + " " + basket.Address.girandelastname + "-" + basket.Address.girandephonenumber;
            total.Text = basket.total + "تومان";
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