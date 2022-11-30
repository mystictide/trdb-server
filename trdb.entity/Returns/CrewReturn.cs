using Newtonsoft.Json;
using trdb.entity.Films;

namespace trdb.entity.Returns
{
    public class CrewReturn
    {
        [JsonProperty("directors")]
        public IEnumerable<People> Directors { get; set; }
        [JsonProperty("producers")]
        public IEnumerable<People> Producers { get; set; }
        [JsonProperty("writers")]
        public IEnumerable<People> Writers { get; set; }
        [JsonProperty("editors")]
        public IEnumerable<People> Editors { get; set; }
        [JsonProperty("photographers")]
        public IEnumerable<People> Photographers { get; set; }
        [JsonProperty("designers")]
        public IEnumerable<People> Designers { get; set; }
        [JsonProperty("art")]
        public IEnumerable<People> Artists { get; set; }
        [JsonProperty("decorators")]
        public IEnumerable<People> Decorators { get; set; }
        [JsonProperty("composers")]
        public IEnumerable<People> Composers { get; set; }
        [JsonProperty("sound")]
        public IEnumerable<People> Sound { get; set; }
        [JsonProperty("costume")]
        public IEnumerable<People> Costume { get; set; }
        [JsonProperty("makeup")]
        public IEnumerable<People> Makeup { get; set; }
    }
}
