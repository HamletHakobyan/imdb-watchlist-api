using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Models.ResponseModel
{
    public class FullTitleResponseVm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string FullTitle { get; set; }
        public string Type { get; set; }
        public string Plot { get; set; }
        public string PlotLocal { get; set; }
        public bool PlotLocalIsRtl { get; set; }
        public string Genres { get; set; }
        public RatingsResponseVm Ratings { get; set; }
        public WikipediaVm Wikipedia { get; set; }
        public PosterCollectionVm Posters { get; set; }
        public string ErrorMessage { get; set; }
    }
}