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
        var json = JsonUtils.ToJsonWithTuple(person);
        var tmp = json;
    }

    public class Person {
        public required string Name { get; init; }
        public required int Age { get; init; }
        public required (string Series, string Number, (string Street, int House) Address) Passport { get; init; }
    }
}
