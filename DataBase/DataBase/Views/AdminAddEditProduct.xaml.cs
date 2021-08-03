using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class AdminAddEditProduct : ContentPage
    {
        private List<byte[]> gallery = new List<byte[]>();
        byte[] mainimage;
        List<PrGroup> list;
        List<int> ids = new List<int>();
        List<string> f = new List<string>();
        PrGroup select;
        int prid;
        Product pp;
        public AdminAddEditProduct(Product product = null)
        {
            InitializeComponent();
            ActivitySpinner.IsRunning = false;
            menu();

            var response = Consts.client.GetAsync("http://localhost:1212/Admin/api/PrGroupIndex/").Result;
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<PrGroup>>(response.Content.ReadAsStringAsync().Result);
                foreach (var item in list)
                {
                    prgroupid.Items.Add(item.title);
                    if (product != null && product.prgroupid == item.id)
                    {
                        select = item;
                        prgroupid.SelectedItem = item.title;
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
                            var stack2 = new StackLayout { BackgroundColor = Color.Black };
                            var button = new Button { IsEnabled = true, Text = "مقداردهی فیلد", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                            button.Clicked += btn_field;
                            stack2.Children.Add(button);
                            stack.Children.Add(stack2);
                            stack.Children.Add(new Label { Text = product.FillFields.FirstOrDefault(w=>w.fieldid== item.Fields[i-1].id).text, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand });
                            f.Add(product.FillFields.FirstOrDefault(w => w.fieldid == item.Fields[i - 1].id).text);
                            fields.Children.Add(stack);
                        }
                    }
                }
            }
            if (product!=null)
            {
                pp = product;
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
            else
            {
                pp = new Product();
                pp.Galleries = new List<Gallery>();
            }

            uploadmainimage.Clicked += async (s, e) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("خطا!", "خطایی رخ داده است.", "باشه");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                asliimage.IsVisible = true;
                asliimage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
                using (var memoryStream = new MemoryStream())
                {
                    file.GetStream().CopyTo(memoryStream);
                    file.Dispose();
                    mainimage = memoryStream.ToArray();
                }

            };

            btn_gallery.Clicked += async (s, e) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("خطا!", "خطایی رخ داده است.", "باشه");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                using (var memoryStream = new MemoryStream())
                {
                    file.GetStream().CopyTo(memoryStream);
                    //file.Dispose();
                    gallery.Add(memoryStream.ToArray());
                }
                galleryadd(ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                }));
            };

            prgroupid.SelectedIndexChanged += async (s, e) =>
              {
                  var picker = (Picker)s;
                  int selectedIndex = picker.SelectedIndex;
                  PrGroup p = list.FirstOrDefault(w => w.title == prgroupid.Items[selectedIndex]);
                  select = p;
                  ids.Clear();
                  f.Clear();
                  fields.Children.Clear();
                  fields.RowDefinitions.Clear();
                  if (product!=null&&product.prgroupid==p.id)
                  {
                      for (int i = 0; i < p.Fields.Count / 4 + 1; i++)
                      {
                          fields.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                      }
                      for (int i = 1; i <= p.Fields.Count; i++)
                      {
                          var stack = new StackLayout { ClassId = (i - 1).ToString() };
                          Grid.SetRow(stack, (p.Fields.Count - i) / 4);
                          Grid.SetColumn(stack, (p.Fields.Count - i) % 4);
                          var lable = new Label { Text = p.Fields[i - 1].title, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                          stack.Children.Add(lable);
                          var stack2 = new StackLayout { BackgroundColor = Color.Black };
                          var button = new Button { IsEnabled = true, Text = "مقداردهی فیلد", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                          button.Clicked += btn_field;
                          stack2.Children.Add(button);
                          stack.Children.Add(stack2);
                          stack.Children.Add(new Label { Text = product.FillFields.FirstOrDefault(w => w.fieldid == p.Fields[i - 1].id).text, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand });
                          f.Add(product.FillFields.FirstOrDefault(w => w.fieldid == p.Fields[i - 1].id).text);
                          ids.Add(p.Fields[i-1].id);
                          fields.Children.Add(stack);
                      }
                  }
                  else
                  {
                      foreach (var item in p.Fields)
                      {
                          ids.Add(item.id);
                          f.Add(string.Empty);
                      }
                      fieldadd(p.Fields);
                  }
              };

            btn_laghv.Clicked += async (s, e) =>
              {
                  await Navigation.PopAsync();
              };

            btn_taiid.Clicked += async (s, e) =>
            {
                if (product!=null)
                {
                    List<string> g = new List<string>();
                    foreach (var item in gallery)
                    {
                        g.Add(Convert.ToBase64String(item));
                    }
                    StringContent content;
                    if (mainimage==null)
                    {
                        var items = new
                        {
                            id = product.id,
                            title = title.Text,
                            price = int.Parse(price.Text),
                            count = int.Parse(count.Text),
                            readyday = int.Parse(readyday.Text),
                            sendday = int.Parse(sendday.Text),
                            text = text.Text,
                            tag = tag.Text,
                            prgroupid = select.id,
                            gallery = g
                        };
                        content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");

                    }
                    else
                    {
                        var items = new
                        {
                            id = product.id,
                            title = title.Text,
                            price = int.Parse(price.Text),
                            count = int.Parse(count.Text),
                            readyday = int.Parse(readyday.Text),
                            sendday = int.Parse(sendday.Text),
                            text = text.Text,
                            tag = tag.Text,
                            prgroupid = select.id,
                            mainimage = Convert.ToBase64String(mainimage),
                            gallery = g
                        };
                        content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");

                    }

                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/ProductEdit", content).Result;
                    if (response2.IsSuccessStatusCode)
                    {

                        prid = product.id;

                        if (ids.Count > 0)
                        {
                            var formVariables = new List<KeyValuePair<string, string>>();
                            formVariables.Add(new KeyValuePair<string, string>("id", prid.ToString()));
                            foreach (var item in ids)
                            {
                                formVariables.Add(new KeyValuePair<string, string>("ids[]", item.ToString()));
                            }
                            foreach (var item in f)
                            {
                                formVariables.Add(new KeyValuePair<string, string>("fields[]", item));
                            }
                            var formContent = new FormUrlEncodedContent(formVariables);
                            var response3 = Consts.client.PostAsync("http://localhost:1212/Admin/api/ProductField", formContent).Result;
                            if (response3.IsSuccessStatusCode)
                            {
                                await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                                await Navigation.PushAsync(new AdminProduct());
                            }
                            else
                            {
                                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message, "باشه");

                            }
                        }
                        else
                        {
                            await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                            await Navigation.PushAsync(new AdminProduct());
                        }
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
                else
                {
                    List<string> g = new List<string>();
                    foreach (var item in gallery)
                    {
                        g.Add(Convert.ToBase64String(item));
                    }
                    var items = new
                    {
                        title = title.Text,
                        price = int.Parse(price.Text),
                        count = int.Parse(count.Text),
                        readyday = int.Parse(readyday.Text),
                        sendday = int.Parse(sendday.Text),
                        text = text.Text,
                        tag = tag.Text,
                        prgroupid = select.id,
                        mainimage = Convert.ToBase64String(mainimage),
                        gallery = g
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
                    var response2 = Consts.client.PostAsync("http://localhost:1212/Admin/api/ProductCreate", content).Result;
                    if (response2.IsSuccessStatusCode)
                    {

                        prid = JsonConvert.DeserializeObject<ActiveID>(response2.Content.ReadAsStringAsync().Result).id;

                        if (ids.Count > 0)
                        {
                            var formVariables = new List<KeyValuePair<string, string>>();
                            formVariables.Add(new KeyValuePair<string, string>("id", prid.ToString()));
                            foreach (var item in ids)
                            {
                                formVariables.Add(new KeyValuePair<string, string>("ids[]", item.ToString()));
                            }
                            foreach (var item in f)
                            {
                                formVariables.Add(new KeyValuePair<string, string>("fields[]", item));
                            }
                            var formContent = new FormUrlEncodedContent(formVariables);
                            var response3 = Consts.client.PostAsync("http://localhost:1212/Admin/api/ProductField", formContent).Result;
                            if (response3.IsSuccessStatusCode)
                            {
                                await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                                await Navigation.PushAsync(new AdminProduct());
                            }
                            else
                            {
                                await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response3.Content.ReadAsStringAsync().Result).message, "باشه");

                            }
                        }
                        else
                        {
                            await DisplayAlert("موفق", "با موفقیت ثبت شد", "مرسی");
                            await Navigation.PushAsync(new AdminProduct());
                        }
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                }
                
            };
        }

        public async void DownloadImage(string imageUrl)
        {
            mainimage = await Consts.client.GetByteArrayAsync(imageUrl);
        }
        private void btn_gallerydelete(object sender, EventArgs e)
        {
            var button2 = (Button)sender;
            var p = button2.Parent;
            var p2 = (StackLayout)p.Parent;
            var image = (Image)p2.Children[0];
            gallery.RemoveAt(int.Parse(p2.ClassId));
            //gallerys.Children.Clear();
            var b = gallerys.Children.Count;
            for (int i =b-1 ; i >= pp.Galleries.Count; i--)
            {
                gallerys.Children.RemoveAt(i);
            }
            for (int i = 1; i <= gallery.Count; i++)
            {
                var stack = new StackLayout { ClassId = (i - 1).ToString() };
                Grid.SetRow(stack, ((i + (pp.Galleries.Count-1))) / 4);
                Grid.SetColumn(stack, ((i + (pp.Galleries.Count - 1))) % 4);
                var lable = new Image { Source = ImageSource.FromStream(() => new MemoryStream(gallery[i - 1])), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
                var button = new Button { IsEnabled = true, Text = "حذف تصویر", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                button.Clicked += btn_gallerydelete;
                stack2.Children.Add(button);
                stack.Children.Add(stack2);
                gallerys.Children.Add(stack);
            }
        }

        private void galleryadd(ImageSource a)
        {
            var stack = new StackLayout { ClassId = (gallery.Count - 1).ToString() };
            Grid.SetRow(stack, ((gallery.Count + (pp.Galleries.Count - 1))) / 4);
            Grid.SetColumn(stack, ((gallery.Count + (pp.Galleries.Count - 1))) % 4);
            var lable = new Image { Source = a, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            stack.Children.Add(lable);
            var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
            var button = new Button { IsEnabled = true, Text = "حذف تصویر", BackgroundColor = Color.DarkGray, TextColor = Color.White };
            button.Clicked += btn_gallerydelete;
            stack2.Children.Add(button);
            stack.Children.Add(stack2);
            gallerys.Children.Add(stack);
        }

        private void oldgallerydelete(List<Gallery> g)
        {
            gallerys.Children.Clear();
            for (int i = 1; i <= g.Count; i++)
            {
                var stack = new StackLayout { ClassId = (g[i - 1].id).ToString() };
                Grid.SetRow(stack, (g.Count - i) / 4);
                Grid.SetColumn(stack, (g.Count - i) % 4);
                var lable = new Image { Source = ImageSource.FromUri(new Uri("http://localhost:1212/Files/Gallery/" + g[i - 1].imagename)), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
                var button = new Button { IsEnabled = true, Text = "حذف تصویر", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                button.Clicked += async (s, e) => {
                    var response2 = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductGalleryDelete/" + stack.ClassId).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        g.Remove(g.FirstOrDefault(w => w.id == int.Parse(stack.ClassId)));
                        oldgallerydelete(g);
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                };
                stack2.Children.Add(button);
                stack.Children.Add(stack2);
                gallerys.Children.Add(stack);
            }
            for (int i = 1; i <= gallery.Count; i++)
            {
                var stack = new StackLayout { ClassId = (i - 1).ToString() };
                Grid.SetRow(stack, ((i + (pp.Galleries.Count - 1))) / 4);
                Grid.SetColumn(stack, ((i + (pp.Galleries.Count - 1))) % 4);
                var lable = new Image { Source = ImageSource.FromStream(() => new MemoryStream(gallery[i - 1])), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
                var button = new Button { IsEnabled = true, Text = "حذف تصویر", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                button.Clicked += btn_gallerydelete;
                stack2.Children.Add(button);
                stack.Children.Add(stack2);
                gallerys.Children.Add(stack);
            }
        }
        private void oldgalleryadd(List<Gallery> g)
        {
            for (int i = 1; i <= g.Count; i++)
            {
                var stack = new StackLayout { ClassId = (g[i-1].id).ToString() };
                Grid.SetRow(stack, (g.Count - i) / 4);
                Grid.SetColumn(stack, (g.Count - i) % 4);
                var lable = new Image { Source = ImageSource.FromUri(new Uri("http://localhost:1212/Files/Gallery/" + g[i-1].imagename)), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                var stack2 = new StackLayout { BackgroundColor = Color.DarkRed };
                var button = new Button { IsEnabled = true, Text = "حذف تصویر", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                button.Clicked +=async (s,e)=> {
                    var response2 = Consts.client.GetAsync("http://localhost:1212/Admin/api/ProductGalleryDelete/"+stack.ClassId).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        g.Remove(g.FirstOrDefault(w=>w.id==int.Parse(stack.ClassId)));
                        oldgallerydelete(g);
                    }
                    else
                    {
                        await DisplayAlert("خطا", JsonConvert.DeserializeObject<ErrorMessages>(response2.Content.ReadAsStringAsync().Result).message, "باشه");
                    }
                };
                stack2.Children.Add(button);
                stack.Children.Add(stack2);
                gallerys.Children.Add(stack);
            }
        }

        private async void btn_field(object sender, EventArgs e)
        {
            var button2 = (Button)sender;
            var p = button2.Parent;
            var p2 = (StackLayout)p.Parent;
            var image = (Label)p2.Children[0];
            string result = await DisplayPromptAsync(image.Text, "لطفا مقدار فیلد را وارد کنید");
            f[int.Parse(p2.ClassId)] = result;
            p2.Children.Add(new Label { Text = result, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand });
        }

        private void fieldadd(List<Field> a)
        {
            for (int i = 0; i < a.Count / 4 + 1; i++)
            {
                fields.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }
            for (int i = 1; i <= a.Count; i++)
            {
                var stack = new StackLayout { ClassId = (i - 1).ToString() };
                Grid.SetRow(stack, (a.Count - i) / 4);
                Grid.SetColumn(stack, (a.Count - i) % 4);
                var lable = new Label { Text = a[i - 1].title, TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                stack.Children.Add(lable);
                var stack2 = new StackLayout { BackgroundColor = Color.Black };
                var button = new Button { IsEnabled = true, Text = "مقداردهی فیلد", BackgroundColor = Color.DarkGray, TextColor = Color.White };
                button.Clicked += btn_field;
                stack2.Children.Add(button);
                stack.Children.Add(stack2);
                fields.Children.Add(stack);
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