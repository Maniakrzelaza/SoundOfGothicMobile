using System;
using System.Collections.Generic;
using System.Text;

namespace SoundOfGothicMobile.Services
{
    public interface ISoundPlayer
    {
        void Play(string sound);
        void Seek(float progress);
    }
}
