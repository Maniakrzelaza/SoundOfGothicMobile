using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace SoundOfGothicMobile.Models
{
    class Options
    {
        public bool ShoudlSayHello { get; set; } = true;
        public int Paging { get; set; } = 50;
        public float Volume { get; set; } = 1.0f;
        public static Options Instance
        {
            get; private set;
        }
        static Options()
        {
            Instance = new Options();
            Instance.Load();
        }
        private Options()
        {

        }
        public void Save()
        {
            Application.Current.Properties["ShoudlSayHello"] = ShoudlSayHello;
            Application.Current.Properties["Paging"] = Paging;
            Application.Current.Properties["Volume"] = Volume;
        }
        public void Load()
        {
            if (Application.Current.Properties.ContainsKey("ShoudlSayHello"))
                ShoudlSayHello = (bool)Application.Current.Properties["ShoudlSayHello"];
            else
            {
                ShoudlSayHello = true;
                Application.Current.Properties["ShoudlSayHello"] = ShoudlSayHello;
            }
            if (Application.Current.Properties.ContainsKey("Paging"))
                Paging = (int)Application.Current.Properties["Paging"];
            else
            {
                Paging = 50;
                Application.Current.Properties["Paging"] = Paging;
            }
            if (Application.Current.Properties.ContainsKey("Volume"))
                Volume = (float)Application.Current.Properties["Volume"];
            else
            {
                Volume = 1.0f;
                Application.Current.Properties["Volume"] = Volume;
            }
        }
    }
}
