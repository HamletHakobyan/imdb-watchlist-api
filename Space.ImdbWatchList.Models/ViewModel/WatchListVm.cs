using System;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class WatchListVm
    {
        public FilmVm Film { get; set; }
        public DateTime Added { get; set; }
        public DateTime? WatchedDate { get; set; }
        public DateTime? OfferedDate { get; set; }
    }
}