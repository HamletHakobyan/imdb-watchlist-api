using System;

namespace Space.ImdbWatchList.Common
{
    public class ImdbSettings
    {
        public const string SectionName = "Imdb";
        public string Host { get; set; }
        public string ApiKey { get; set; }
    }
}
