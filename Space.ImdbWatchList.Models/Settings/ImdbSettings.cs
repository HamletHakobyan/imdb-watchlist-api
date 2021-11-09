namespace Space.ImdbWatchList.Models.Settings
{
    public class ImdbSettings
    {
        public const string SectionName = "Imdb";
        public string Host { get; set; }
        public string ApiKey { get; set; }
    }
}