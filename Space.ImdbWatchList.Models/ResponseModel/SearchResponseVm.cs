using System.Collections.Generic;

namespace Space.ImdbWatchList.Models.ResponseModel
{
    public class SearchResponseVm
    {
        public ResultType SearchType { get; set; }
        public string Expression { get; set; }

        public List<SearchResultVm> Results { get; set; }

        public string ErrorMessage { get; set; }
    }
}