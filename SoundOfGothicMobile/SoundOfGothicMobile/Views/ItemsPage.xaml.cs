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
        public int CurrentPage = 0;
        private string GetAppUrl()
        {
            const string Format = "https://api.soundofgothic.pl/?pageSize={0}&page={1}&filter=";
            string result = string.Format(Format, Options.Instance.Paging.ToString(), this.CurrentPage.ToString());
            return result;
        }
        const string ScriptUrl = "https://api.soundofgothic.pl/source?pageSize=50&page=0&filter=";
        public async void NextPage(object sender, EventArgs args)
        {
            this.CurrentPage += 1;
            if (this.CurrentPage > 0)
            {
                PrevButton.IsEnabled = true;
                PrevButton.IsVisible = true;
            }
            await this.Search();
        }
        public async void PrevPage(object sender, EventArgs args)
        {
            this.CurrentPage -= 1;
            if (this.CurrentPage <= 0)
            {
                PrevButton.IsEnabled = false;
                PrevButton.IsVisible = false;
            }
                
            await this.Search();
        }
        public ItemsPage()
        {
            _cache = new MemoryCache(new MemoryCacheOptions() { });
            BindingContext = viewModel = new ApiRecordViewModel();
            InitializeComponent();
            ViewsCominicator.Instance.Register("ItemsPage", this);
            if (this.CurrentPage <= 0)
            {
                PrevButton.IsEnabled = false;
                PrevButton.IsVisible = false;
            }
            SearchBar.SearchCommand = new Command(async () =>
            {
                this.CurrentPage = 0;
                await this.Search(); 
            });
        }
        public async Task Search()
        {
            string text = SearchBar.Text ?? "";
            ApiRecordViewModel viewModel = await GetRecordsFromApi(this.GetAppUrl(), text);
            BindingContext = viewModel;
        }

        void DownloadSound(object sender, EventArgs args)
        {
            Image senderButton = sender as Image;
            if (senderButton == null)
                return;
            String fileName = ((ApiRecord)senderButton.BindingContext).Filename.ToString();
            DependencyService.Get<IFileSaver>().SaveFile("https://sounds.soundofgothic.pl/assets/gsounds/" + fileName.ToUpper() + ".WAV");
        }

        async Task<ApiRecordViewModel> GetRecordsFromApi(String baseUrl, String name)
        {
            string content = "";
            if (!_cache.TryGetValue(name + this.CurrentPage, out content))
            {
                Task<string> downloadTask = HttpClient.GetStringAsync(baseUrl + name);
                content = await downloadTask;
                _cache.Set(name, content);
            }
            JObject jObject = JObject.Parse(content);
            List<ApiRecord> records = JsonConvert.DeserializeObject<List<ApiRecord>>(jObject["records"].ToString());
            ApiRecordViewModel resultView = new ApiRecordViewModel(records);
            resultView.DefaultPageSize = (int)jObject["defaultPageSize"];
            resultView.RecordCoundTotal = (int)jObject["recordCountTotal"];
            resultView.PageNumber = (int)jObject["pageNumber"];
            resultView.RecordsOnPage = (int)jObject["recordsOnPage"];
            resultView.FirstRecordOnPageNumber = (resultView.PageNumber * resultView.DefaultPageSize) + 1;
            int lastNumber = resultView.FirstRecordOnPageNumber + resultView.RecordsOnPage - 1;
            resultView.LastRecordOnPageNumber = lastNumber;
            return resultView;
        }
        void OnPlayButtonClicked(object sender, EventArgs args)
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

        async void OnSourceScriptClicked(object sender, EventArgs args)
        {
            Label senderLabel = sender as Label;
            if (senderLabel == null)
                return;
            String scriptName = senderLabel.FormattedText.ToString().Replace("Plik:", "");
            ApiRecordViewModel viewModel = await GetRecordsFromApi(ScriptUrl, scriptName);
            BindingContext = viewModel;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            ApiRecordViewModel viewModel = await GetRecordsFromApi(this.GetAppUrl(), "");
            BindingContext = viewModel;
        }
    }
}