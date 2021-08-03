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
    public partial class AdminUserShowMessage : ContentPage
    {
        public AdminUserShowMessage(Message message,bool send)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();

            if (send)
            {
                name.Text = "به کاربر "+message.User.username;
            }
            else
            {
                var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/SeenMessage/" + message.id).Result;
                res.IsVisible = true;
                if (message.senderemail!=null)
                {
                    name.Text = "از طرف کاربر مهمان "+message.senderlastname+" "+message.sendername;

                }
                else
                {
                    name.Text = "از طرف کاربر " + message.User2.username;
                }
                
            }
            text.Text = message.text;
            date.Text = DateDifference.MiladiToShamsi(message.createdate);

            btn_response.Clicked += async (s, e) =>
            {
                if (message.senderemail != null)
                {
                    await Navigation.PushAsync(new AdminAddMessage(message.senderemail));

                }
                else
                {
                    await Navigation.PushAsync(new AdminAddMessage("",message.user2id.Value));
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