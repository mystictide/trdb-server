using Newtonsoft.Json;
using trdb.entity.Films;
using trdb.entity.Helpers;

namespace trdb.entity.Returns
{
    public class Browser
    {
        [JsonProperty("films")]
        public FilteredList<Films.Films>? Films { get; set; }
        [JsonProperty("people")]
        public FilteredList<People>? People { get; set; }
    }
}
