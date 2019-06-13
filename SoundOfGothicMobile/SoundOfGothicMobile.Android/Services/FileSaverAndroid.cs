using System;
using System.IO;
using System.Net;
using SoundOfGothicMobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SoundOfGothicMobile.Droid.Services.FileSaverAndroid))]
namespace SoundOfGothicMobile.Droid.Services
{
    class FileSaverAndroid : IFileSaver
    {
        public async void SaveFile(string url)
        {
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Download");
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                await webClient.DownloadFileTaskAsync(new Uri(url), pathToNewFile);
            } 
            catch(Exception ex)
            {
                
            }
        }
    }
}