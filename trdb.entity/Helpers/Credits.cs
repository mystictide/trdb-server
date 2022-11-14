using Newtonsoft.Json;
using trdb.entity.Films;

namespace trdb.entity.Helpers
{
    public class Credits
    {
        [JsonProperty("cast")]
        public List<People> Cast { get; set; }
        [JsonProperty("crew")]
        public List<People> Crew { get; set; }
    }
}
