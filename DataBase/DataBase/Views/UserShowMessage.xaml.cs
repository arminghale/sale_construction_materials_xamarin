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
    public partial class UserShowMessage : ContentPage
    {
        public UserShowMessage(Message message,bool send)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            if (send)
            {
                name.Text = "از طرف مدیریت";
                var response = Consts.client.GetAsync("http://localhost:1212/User/api/OneMessages/" + message.id).Result;
            }
            else
            {
                name.Text = "ارسال به مدیریت";
            }
            text.Text = message.text;
            
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