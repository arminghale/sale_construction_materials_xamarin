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
    public partial class AdminDetailsProduct : ContentPage
    {
        private List<string> f = new List<string>();
        public AdminDetailsProduct(Product product)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();
            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                var  list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                foreach (var item in list)
                {
                    if (product != null && product.prgroupid == item.id)
                    {
                        prgroup.Text = item.title;
                        for (int i = 0; i < item.Fields.Count / 4 + 1; i++)
                        {
                            fields.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                        }
                        for (int i = 1; i <= item.Fields.Count; i++)
                        {
                            var stack = new StackLayout { ClassId = (i - 1).ToString() };
                            Grid.SetRow(stack, (item.Fields.Count - i) / 4);
                            Grid.SetColumn(stack, (item.Fields.Count - i) % 4);
                            var lable = new Label { Text = item.Fields[i - 1].title, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                            stack.Children.Add(lable);
                            stack.Children.Add(new Label { Text = product.FillFields.FirstOrDefault(w => w.fieldid == item.Fields[i - 1].id).text, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand });
                            f.Add(product.FillFields.FirstOrDefault(w => w.fieldid == item.Fields[i - 1].id).text);
                            fields.Children.Add(stack);
                        }
                    }
                }
            }
            if (product != null)
            {
                title.Text = product.title;
                readyday.Text = product.readyday.ToString();
                sendday.Text = product.sendday.ToString();
                count.Text = product.count.ToString();
                text.Text = product.text;
                price.Text = product.price.ToString();
                asliimage.Source = ImageSource.FromUri(new Uri("http://localhost:1212/Files/" + product.imagename));
                asliimage.IsVisible = true;
                //DownloadImage("http://localhost:1212/Files/" + product.imagename);
                foreach (var item in product.Tags)
                {
                    tag.Text += item.text + "-";
                }
                tag.Text = tag.Text.Remove(tag.Text.Length - 1);
                oldgalleryadd(product.Galleries);
            }
        }
        private void oldgalleryadd(List<Gallery> g)
        {
            for (int i = 1; i <= g.Count; i++)
            {
                var stack = new StackLayout { ClassId = (g[i - 1].id).ToString() };
                Grid.SetRow(stack, (g.Count - i) / 4);
                Grid.SetColumn(stack, (g.Count - i) % 4);
                var lable = new Image { Source = ImageSource.FromUri(new Uri("http://localhost:1212/Files/Gallery/" + g[i - 1].imagename)), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                gallerys.Children.Add(stack);
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