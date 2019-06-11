using SoundOfGothicMobile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Foundation;
using Windows.Storage.Pickers;

[assembly: Xamarin.Forms.Dependency(typeof(SoundOfGothicMobile.UWP.Services.FileSaverWin))]
namespace SoundOfGothicMobile.UWP.Services
{
    class FileSaverWin : IFileSaver
    {
        public async void SaveFile(string url)
        {
            BackgroundDownloader _downloader = new BackgroundDownloader(); ;
            String fileName = Path.GetFileName(url);
            IStorageFile newFile = await DownloadsFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            var newDownload = _downloader.CreateDownload(new Uri(url), newFile);
            await newDownload.StartAsync();
        }
    }
}
