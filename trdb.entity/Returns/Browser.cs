using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.entity.Returns
{
    public class Browser
    {
        [JsonProperty("movies")]
        public FilteredList<Movies.Movies>? Movies { get; set; }
        [JsonProperty("people")]
        public FilteredList<People>? People { get; set; }
    }
}
