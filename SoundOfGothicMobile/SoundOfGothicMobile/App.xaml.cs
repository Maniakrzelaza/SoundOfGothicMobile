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
            if(Options.Instance.ShoudlSayHello)
                DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/INFO_SFB_5_EINERVONEUCHWERDEN_02_00.WAV");
        }

        protected override void OnSleep()
        {
            if (Options.Instance.ShoudlSayHello)
                DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/DIA_ADDON_MORGAN_PERM_15_00.WAV");
        }

        protected override void OnResume()
        {
            if (Options.Instance.ShoudlSayHello)
                DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/DIA_LOTHAR_ADD_01_47.WAV");
        }
    }
}
