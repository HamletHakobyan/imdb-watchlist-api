namespace Space.ImdbWatchList.Models.ViewModel
{
    public class WikipediaVm
    {
        public string ImDbId { get; set; }
        public Plot PlotShort { get; set; }
        public Plot PlotFull { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Plot
    {
        public string PlainText { get; set; }
        public string Html { get; set; }
    }
}