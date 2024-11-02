using Piovra.Json;
using Xunit;

namespace Piovra.Tests;

public class JsonTests {
    [Fact]
    public void Test() {
        var person = new Person {
            Name = "Test",
            Age = 100,
            Passport = ("123", "45", ("any", 10))
        };
        var json = JsonUtils.To(person);
        var tmp = json;
    }

    public class Person {
        public string Name { get; set; }
        public int Age { get; set; }
        public (string Series, string Number, (string Street, int House) Address) Passport { get; set; }
    }
}
