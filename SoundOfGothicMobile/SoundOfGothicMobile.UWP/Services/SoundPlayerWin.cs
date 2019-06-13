using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.MediaManager;
using SoundOfGothicMobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SoundOfGothicMobile.UWP.Services.SoundPlayerWin))]
namespace SoundOfGothicMobile.UWP.Services
{
    class SoundPlayerWin : ISoundPlayer
    {
        private Plugin.MediaManager.Abstractions.IMediaManager mediaManager = CrossMediaManager.Current;
        public async void Play(string sound)
        {
            mediaManager.MediaFinished += (a, b) => { CrossMediaManager.Current.Stop(); };
            await CrossMediaManager.Current.Play(sound);
        }

        public void Seek(float progress)
        {
            if (mediaManager == null)
                return;
            mediaManager.Seek(TimeSpan.FromMilliseconds((int)(mediaManager.Duration.TotalMilliseconds * progress)));
        }
    }
}
