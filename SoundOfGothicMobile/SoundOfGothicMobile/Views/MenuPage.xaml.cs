using SoundOfGothicMobile.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundOfGothicMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            PagingPicker.Items.Add("10");
            PagingPicker.Items.Add("20");
            PagingPicker.Items.Add("30");
            ShouldSayHelloSwitch.IsToggled = Options.Instance.ShoudlSayHello;
            //PagingSlider.Value = Options.Instance.Paging;
            PagingPicker.Title = Options.Instance.Paging.ToString();
           /* menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Browse, Title="Browse" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };*/
        }
        public void ChangeGreetingsOptions(object sender, ToggledEventArgs args)
        {
            Xamarin.Forms.Switch greetingsSwitch = sender as Xamarin.Forms.Switch;
            if (greetingsSwitch == null)
                return;
            Options.Instance.ShoudlSayHello = greetingsSwitch.IsToggled;
            Options.Instance.Save();
        }
        public void OnPagingSliderChanged(object sender, SelectedItemChangedEventArgs args)
        {
            Xamarin.Forms.Slider senderSlider = sender as Xamarin.Forms.Slider;
            if (senderSlider == null)
                return;
            int paging = (int)senderSlider.Value;
            Options.Instance.Paging = paging;
            Options.Instance.Save();
        }
    }
}