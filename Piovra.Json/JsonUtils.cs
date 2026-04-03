using Newtonsoft.Json;

namespace Piovra.Json;

public static class JsonUtils {
    public static string ToJsonWithTuple(object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented, new TupleJsonConverter());
    public static string ToJson<T>(this T obj) => JsonConvert.SerializeObject(obj);
    public static T? From<T>(string json) => JsonConvert.DeserializeObject<T>(json);
}
