using System;

namespace Space.ImdbWatchList.Dto
{
    public class WatchListDto
    {
        public FilmDto Film { get; set; }
        public DateTime Added { get; set; }
        public DateTime? WatchedDate { get; set; }
        public DateTime? OfferedDate { get; set; }
    }
}