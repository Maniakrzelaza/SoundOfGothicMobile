using System;
using System.Collections.Generic;
using System.Text;

namespace SoundOfGothicMobile.Models
{
    class Options
    {
        public bool ShoudlSayHello { get; set; } = true;
        public static Options Instance
        {
            get; private set;
        }
        static Options()
        {
            Instance = new Options();
        }
        private Options()
        {

        }
    }
}
