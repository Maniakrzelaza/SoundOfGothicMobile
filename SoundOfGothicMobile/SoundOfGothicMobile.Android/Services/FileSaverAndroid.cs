using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SoundOfGothicMobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SoundOfGothicMobile.Droid.Services.FileSaverAndroid))]
namespace SoundOfGothicMobile.Droid.Services
{
    class FileSaverAndroid : IFileSaver
    {
        public void SaveFile(string url)
        {
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Download");
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            } 
            catch(Exception ex)
            {
                
            }
        }
    }
}