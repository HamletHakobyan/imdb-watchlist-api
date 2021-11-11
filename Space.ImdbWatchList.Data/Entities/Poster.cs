namespace Space.ImdbWatchList.Data.Entities
{
    public class Poster
    {
        public string Id { get; set; }
        public string FilmId { get; set; }
        public Film Film { get; set; }
        public string Link { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}