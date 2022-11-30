using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace trdb.data
{
    public class JSONTypeHandler
    {
        class LongNameContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
                foreach (JsonProperty prop in list)
                {
                    prop.PropertyName = prop.UnderlyingName;
                }
                return list;
            }
        }

        public static string CustomSerializer(object obj, bool useJSONProperty)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            if (useJSONProperty)
            {
                settings.ContractResolver = new LongNameContractResolver();
            }

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings()
            {
                ContractResolver = new LongNameContractResolver()
            });
        }
    }
}
