using System;
using System.Collections.Generic;
using System.Text;

namespace SoundOfGothicMobile.Services
{
    class ViewsCominicator
    {
        private Dictionary<string, object> Pages;
        public static ViewsCominicator Instance
        {
            get; private set;
        }
        static ViewsCominicator()
        {
            Instance = new ViewsCominicator();
        }
        private ViewsCominicator()
        {
            Pages = new Dictionary<string, object>();
        }
        public void Register(string key, object page)
        {
            if (!Pages.ContainsKey(key))
            {
                Pages.Add(key, page);
            }
        }
        public object GetPage(string key)
        {
            if (!Pages.ContainsKey(key))
                return null;
            else
                return Pages[key];
        }
    }
}
