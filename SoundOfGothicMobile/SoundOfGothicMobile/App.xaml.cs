using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SoundOfGothicMobile.Views;
using Android.Media;
using SoundOfGothicMobile.Services;

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
            DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/INFO_SFB_5_EINERVONEUCHWERDEN_02_00.WAV");
        }

        protected override void OnSleep()
        {
            DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/DIA_ADDON_MORGAN_PERM_15_00.WAV");

        }

        protected override void OnResume()
        {
            DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/DIA_LOTHAR_ADD_01_47.WAV");

        }
    }
}
