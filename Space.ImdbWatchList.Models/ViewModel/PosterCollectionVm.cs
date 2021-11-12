using System.Collections.Generic;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class PosterCollectionVm
    {
        public string ImDbId { get; set; }
        public List<PosterVm> Posters { get; set; }
        public List<PosterVm> Backdrops { get; set; }
        public string ErrorMessage { get; set; }
    }
}