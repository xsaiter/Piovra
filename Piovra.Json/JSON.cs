using Newtonsoft.Json;

namespace Piovra.Json {
    public static class JSON {
        public static string To(object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new TupleJsonConverter());
        }

        public static T From<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson<T>(this T obj) => JsonConvert.SerializeObject(obj);
    }
}