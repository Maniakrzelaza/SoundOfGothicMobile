using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SoundOfGothicMobile.Services;
using Android.Media;

[assembly: Xamarin.Forms.Dependency(typeof(SoundOfGothicMobile.Droid.Services.SoundPlayerAndroid))]
namespace SoundOfGothicMobile.Droid.Services
{
    class SoundPlayerAndroid : ISoundPlayer
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public void Play(string sound)
        {
            mediaPlayer.Stop();
            mediaPlayer.Reset();
            mediaPlayer.SetDataSource(sound);
            mediaPlayer.Prepared += (send, eventArgs) => { mediaPlayer.Start(); };
            mediaPlayer.PrepareAsync();
        }

        public void Seek(float progress)
        {
            if (mediaPlayer == null)
                return;
            mediaPlayer.SeekTo((int)(mediaPlayer.Duration * progress));
        }
    }
}