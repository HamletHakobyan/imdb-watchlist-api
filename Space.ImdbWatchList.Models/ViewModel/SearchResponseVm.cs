using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Space.ImdbWatchList.Models.ViewModel
{
    public class SearchResponseVm
    {
        public ResultType SearchType { get; set; }
        public string Expression { get; set; }

        public List<SearchResultVm> Results { get; set; }

        public string ErrorMessage { get; set; }
    }
}