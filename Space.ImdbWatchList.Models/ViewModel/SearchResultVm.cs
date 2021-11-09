using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class SearchResultVm
    {
        public string Id { get; set; }
        public ResultType ResultType { get; set; }
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultType
    {
        Title = 1,
        Movie = 2,
        Series = 4,
        Name = 8,
        Episode = 16,
        Company = 32,
        Keyword = 64,
        All = 128
    }
}