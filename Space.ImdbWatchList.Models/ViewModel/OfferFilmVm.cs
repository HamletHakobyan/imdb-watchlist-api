using System.Collections.Generic;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class OfferFilmVm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal? Rating { get; set; }
        public List<PosterVm> Posters { get; set; }
        public string WikiShortDesc { get; set; }
    }
}