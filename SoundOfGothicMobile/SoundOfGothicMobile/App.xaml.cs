using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SoundOfGothicMobile.Views;
using Android.Media;
using SoundOfGothicMobile.Services;
using SoundOfGothicMobile.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SoundOfGothicMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            Options.Instance.Load();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
