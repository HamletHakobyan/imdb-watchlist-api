using System.Collections.Generic;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class PosterCollectionVm
    {
        public string ImDbId { get; set; }
        public List<PosterVM> Posters { get; set; }
        public List<PosterVM> Backdrops { get; set; }
        public string ErrorMessage { get; set; }
    }
}