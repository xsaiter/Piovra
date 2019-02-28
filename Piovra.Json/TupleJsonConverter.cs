using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Piovra.Json {
    public class TupleJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var t = JToken.FromObject(value);
            if (t.Type == JTokenType.Object) {
                Prepare((JObject) t, value);
            }
            t.WriteTo(writer);
        }

        void Prepare(JObject obj, object value) {
            var ps = obj.Properties().ToList();
            var pis = value.GetType().GetProperties();

            for (var i = 0; i < pis.Length; ++i) {
                var p = ps[i];
                var pi = pis[i];

                var typeName = pi.ToString();

                if (IsValueTuple(typeName)) {
                    var tree = new Parser(typeName).Parse();

                    tree.PropertyName = pi.Name;
                    tree.Property = p;

                    var names = GetNamesFromTuple(pi);

                    tree.Visit(names);
                    tree.Visit(p);

                    tree.Replace();
                } else {
                    if (!pi.PropertyType.IsPrimitive) {
                        var t = JToken.FromObject(p.Value);
                        if (t.Type == JTokenType.Object) {
                            if (p.Value is JObject nestedObj) {
                                var nestedValue = pi.GetValue(value);
                                Prepare(nestedObj, nestedValue);
                            }
                        }
                    }
                }
            }
        }

        static Names GetNamesFromTuple(PropertyInfo pi) {
            var result = new Names();
            var attr = pi.CustomAttributes.First();
            var typedArg = attr.ConstructorArguments.First();
            var args = typedArg.Value as IEnumerable;
            foreach (dynamic arg in args) {
                result.Items.Add(arg.Value);
            }
            return result;
        }

        static bool IsValueTuple(string str) {
            return str.StartsWith("System.ValueTuple");
        }

        public class Names {
            public List<string> Items { get; } = new List<string>();
            public int N { get; set; }
        }

        public class Node {
            public Node(string typeName) => TypeName = typeName;
            public string TypeName { get; }
            public string PropertyName { get; set; }
            public JProperty Property { get; set; }
            public List<Node> Nodes { get; } = new List<Node>();

            public void Visit(Names names) {
                Nodes.ForEach(x => x.PropertyName = names.Items[names.N++]);
                Nodes.ForEach(x => x.Visit(names));
            }

            public void Visit(JProperty p) {
                var children = p.Value.Children().ToList();
                for (var i = 0; i < Nodes.Count; ++i) {
                    var node = Nodes[i];
                    node.Property = (JProperty) children[i];
                    node.Visit(node.Property);
                }
            }

            public void Replace() {
                if (Property != null) {
                    Nodes.ForEach(x => x.Replace());
                    Property.Replace(new JProperty(PropertyName, Property.Value));
                }
            }
        }

        public class Parser {
            readonly string s;
            int i;
            char ch;

            public Parser(string s) {
                this.s = s;
            }

            public Node Parse() {
                var token = GetToken();
                return ParseTuple(token);
            }

            Node ParseTuple(string type) {
                var result = new Node(type);
                while (ch != ']') {
                    var token = GetToken();
                    if (IsValueTuple(token)) {
                        var nested = ParseTuple(token);
                        result.Nodes.Add(nested);
                    } else {
                        var node = new Node(token);
                        result.Nodes.Add(node);
                    }
                }
                Read();
                return result;
            }

            string GetToken() {
                var result = string.Empty;
                while (Read() && ch != ',' && ch != '[' && ch != ']') {
                    result += ch;
                }
                return result;
            }

            bool Read() {
                if (i < s.Length) {
                    ch = s[i++];
                    if (char.IsWhiteSpace(ch)) {
                        return Read();
                    }
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}