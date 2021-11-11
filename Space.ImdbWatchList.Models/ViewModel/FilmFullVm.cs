using Space.ImdbWatchList.Models.ResponseModel;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class FilmFullVm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Rating { get; set; }
        public PosterCollectionVm Posters { get; set; }
        public WikipediaVm WikipediaInfo { get; set; }
    }
}