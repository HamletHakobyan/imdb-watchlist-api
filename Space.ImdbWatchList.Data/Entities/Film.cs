using System;
using System.Collections;
using System.Collections.Generic;

namespace Space.ImdbWatchList.Data.Entities
{
    public class Film
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Rating { get; set; }
        public string WikiDescription { get; set; }
        public ICollection<WatchList> InterestedUsers { get; set; }
        public ICollection<Poster> Posters { get; set; }
    }
}