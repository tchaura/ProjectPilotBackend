using Atlassian.Jira;
using Newtonsoft.Json.Linq;

namespace ProjectPilotWeb.Helpers;

public class CustomFieldSerializers
{
    public class StoryPointSerializer : ICustomFieldValueSerializer
    {
        public string[] FromJson(JToken json)
        {
            return [json.Value<int>().ToString()];
        }

        public JToken ToJson(string[] values)
        {
            return int.Parse(values[0]);
        }
    }
}