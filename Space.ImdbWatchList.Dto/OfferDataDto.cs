using System.Collections.Generic;

namespace Space.ImdbWatchList.Dto
{
    public class OfferDataDto
    {
        public string FilmId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public decimal? Rating { get; set; }
        public PosterDto Poster { get; set; }
        public string WikiShortDesc { get; set; }
    }
}