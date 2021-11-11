using System.Collections;
using System.Collections.Generic;

namespace Space.ImdbWatchList.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<WatchList> FilmsInWatchList { get; set; }
    }
}