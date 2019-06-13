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
using Microsoft.Extensions.Caching.Memory;
using System.Windows.Input;

namespace SoundOfGothicMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        private ApiRecordViewModel viewModel;
        private HttpClient HttpClient = new HttpClient();
        private readonly IMemoryCache _cache;
        const string AppUrl = "https://api.soundofgothic.pl/?pageSize=50&page=0&filter=";
        const string ScriptUrl = "https://api.soundofgothic.pl/source?pageSize=50&page=0&filter=";

        public ItemsPage()
        {
            _cache = new MemoryCache(new MemoryCacheOptions() { });
            BindingContext = viewModel = new ApiRecordViewModel();

            InitializeComponent();
            SearchBar.SearchCommand = new Command(async () =>
            {
                string text = SearchBar.Text;
                List<ApiRecord> records = await GetRecordsFromApi(AppUrl, text);
                BindingContext = viewModel = new ApiRecordViewModel(records);
            });
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
            string content = "";
            if (!_cache.TryGetValue(name, out content))
            {
                Task<string> downloadTask = HttpClient.GetStringAsync(baseUrl + name);
                content = await downloadTask;
                _cache.Set(name, content);
            }
            JObject jObject = JObject.Parse(content);
            List<ApiRecord> records = JsonConvert.DeserializeObject<List<ApiRecord>>(jObject["records"].ToString());
            return records;
        }

        void OnPlayButtonClicked(object sender, SelectedItemChangedEventArgs args)
        {
            Image senderButton = sender as Image;
            if (senderButton == null)
                return;
            String fileName = ((ApiRecord)senderButton.BindingContext).Filename.ToString();
            DependencyService.Get<ISoundPlayer>().Play("https://sounds.soundofgothic.pl/assets/gsounds/" + fileName.ToUpper() + ".WAV");
        }
        void OnVolumeSliderChange(object sender, SelectedItemChangedEventArgs args)
        {
            Xamarin.Forms.Slider senderSlider = sender as Xamarin.Forms.Slider;
            if (senderSlider == null)
                return;
            float volume = (float)senderSlider.Value;
            //Media.SetVolume(volume, volume);
        }
        Command cc = new Command<float>((progress) => DependencyService.Get<ISoundPlayer>().Seek(progress));

        void OnProgressSliderChange(object sender, SelectedItemChangedEventArgs args)
        {
            Xamarin.Forms.Slider senderSlider = sender as Xamarin.Forms.Slider;
            if (senderSlider == null)
                return;
            float progress = (float)senderSlider.Value;
            DependencyService.Get<ISoundPlayer>().Seek(progress);
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

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            List<ApiRecord> records = await GetRecordsFromApi(AppUrl, "");
            BindingContext = viewModel = new ApiRecordViewModel(records);
        }
    }
}