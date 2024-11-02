using Newtonsoft.Json;

namespace Piovra.Json;

public static class JsonUtils {
    public static string To(object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented, new TupleJsonConverter());
    public static T From<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    public static string ToJson<T>(this T obj) => JsonConvert.SerializeObject(obj);
}
