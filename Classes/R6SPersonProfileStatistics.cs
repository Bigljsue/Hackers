using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp;

namespace WPF_HackersList.Classes
{
    public class R6SPersonProfileStatistics
    {
        public string adress = "https://r6.tracker.network/profile/pc/Ongin_rasp";

        public R6SPersonProfileStatistics()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync(adress);
        }
    }
}
