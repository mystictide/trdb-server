using Newtonsoft.Json.Linq;

namespace trdb.api.Helpers
{
    public class CustomHelpers
    {
        public static bool IsResponseSuccessful(dynamic data)
        {
            try
            {
                foreach (JProperty property in data)
                {
                    var prop = (property.Name, property.Value);
                    if (prop.Name == "success" && prop.Value.ToString() == "{False}")
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }
    }
}
