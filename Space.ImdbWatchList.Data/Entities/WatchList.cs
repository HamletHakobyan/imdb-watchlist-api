using System;

namespace Space.ImdbWatchList.Data.Entities
{
    public class WatchList
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string FilmId { get; set; }
        public Film Film { get; set; }
        public DateTime Added { get; set; }
        public DateTime? WatchedDate { get; set; }
        public DateTime? OfferedDate { get; set; }
    }
}