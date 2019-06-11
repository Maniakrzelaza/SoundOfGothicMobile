using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using SoundOfGothicMobile.Models;
using SoundOfGothicMobile.Views;
using SoundOfGothicMobile.ViewModels;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Android.Media;
using Plugin.MediaManager;
using Windows.UI.Xaml.Controls;
using Image = Xamarin.Forms.Image;
using SoundOfGothicMobile.Services;

namespace SoundOfGothicMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        //ItemsViewModel viewModel;
        ApiRecordViewModel viewModel;
        HttpClient HttpClient = new HttpClient();
        MediaPlayer Media;
        Plugin.MediaManager.Abstractions.IMediaManager mediaManager;
        string AppUrl = "https://api.soundofgothic.pl/?pageSize=50&page=0&filter=";
        String ScriptUrl = "https://api.soundofgothic.pl/source?pageSize=50&page=0&filter=";
        public ItemsPage()
        {
            BindingContext = viewModel = new ApiRecordViewModel();

            InitializeComponent();

            SearchEntry.Unfocused += async (sender, args) =>
            {
                List<ApiRecord> records = await GetRecordsFromApi(AppUrl, SearchEntry.Text);
                BindingContext = viewModel = new ApiRecordViewModel(records);
            };
        }

        void DownloadSound(object sender, SelectedItemChangedEventArgs args)
        {
            Image senderButton = sender as Image;
            if (senderButton == null)
                return;
            String fileName = ((ApiRecord)senderButton.BindingContext).Filename.ToString();
            DependencyService.Get<IFileSaver>().SaveFile("https://sounds.soundofgothic.pl/assets/gsounds/" + fileName.ToUpper() + ".WAV");
        }

        async Task<List<ApiRecord>> GetRecordsFromApi(String baseUrl, String name)
        {
            Task<string> downloadTask = HttpClient.GetStringAsync(baseUrl + name);
            string content = await downloadTask;
            JObject jObject = JObject.Parse(content);
            List<ApiRecord> records = JsonConvert.DeserializeObject<List<ApiRecord>>(jObject["records"].ToString());
            return records;
        }

        async void OnPlayButtonClickedUWP(object sender, SelectedItemChangedEventArgs args)
        {
            Image senderButton = sender as Image;
            if (senderButton == null)
                return;
            String fileName = ((ApiRecord)senderButton.BindingContext).Filename.ToString();
            mediaManager = CrossMediaManager.Current;
            mediaManager.MediaFinished += (a, b) => { CrossMediaManager.Current.Stop(); };
            await CrossMediaManager.Current.Play("https://sounds.soundofgothic.pl/assets/gsounds/" + fileName.ToUpper() + ".WAV");
        }
        void OnPlayButtonClickedAndroid(object sender, SelectedItemChangedEventArgs args)
        {

            Image senderButton = sender as Image;
            if (senderButton == null)
                return;
            String fileName = ((ApiRecord)senderButton.BindingContext).Filename.ToString();
            Media = new MediaPlayer();
            Media.SetDataSource("https://sounds.soundofgothic.pl/assets/gsounds/" + fileName.ToUpper() + ".WAV");
            Media.Prepared += (send, eventArgs) => { Media.Start(); };
            Media.PrepareAsync();

        }
        void OnVolumeSliderChange(object sender, SelectedItemChangedEventArgs args)
        {
            Xamarin.Forms.Slider senderSlider = sender as Xamarin.Forms.Slider;
            if (senderSlider == null)
                return;
            float volume = (float)senderSlider.Value;
            Media.SetVolume(volume, volume);
        }

        void OnProgressSliderChangeAndroid(object sender, SelectedItemChangedEventArgs args)
        {
            Xamarin.Forms.Slider senderSlider = sender as Xamarin.Forms.Slider;
            if (senderSlider == null || Media == null)
                return;
            float progress = (float)senderSlider.Value;
            Media.SeekTo((int)(Media.Duration * progress));
        }
        void OnProgressSliderChangeUWP(object sender, SelectedItemChangedEventArgs args)
        {
            Xamarin.Forms.Slider senderSlider = sender as Xamarin.Forms.Slider;
            if (senderSlider == null || mediaManager == null)
                return;
            float progress = (float)senderSlider.Value;
            mediaManager.Seek(TimeSpan.FromMilliseconds((int)(mediaManager.Duration.TotalMilliseconds * progress)));
        }

        async void OnSourceScriptClicked(object sender, SelectedItemChangedEventArgs args)
        {
            Label senderLabel = sender as Label;
            if (senderLabel == null)
                return;
            String scriptName = senderLabel.FormattedText.ToString().Replace("Plik:", "");
            List<ApiRecord> records = await GetRecordsFromApi(ScriptUrl, scriptName);
            BindingContext = viewModel = new ApiRecordViewModel(records);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            List<ApiRecord> records = await GetRecordsFromApi(AppUrl, "");
            BindingContext = viewModel = new ApiRecordViewModel(records);
            /*if (viewModel.ApiRecords.Count == 0)
                viewModel.Refresh.Execute(null);*/
        }
    }
}