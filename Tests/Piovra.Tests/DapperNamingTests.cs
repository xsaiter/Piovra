using Piovra.DapperExtensions;
using Xunit;

namespace Piovra.Tests;

public class DapperNamingTests {
    [Fact]
    public void Test() {
        var s1 = Naming.ConvertFromCamelToSnakeCase("GetAgeForPerson");
        Assert.Equal("get_age_for_person", s1);

        var s2 = Naming.ConvertFromCamelToSnakeCase("Get");
        Assert.Equal("get", s2);

        var s3 = Naming.ConvertFromCamelToSnakeCase("GetA");
        Assert.Equal("get_a", s3);

        var s4 = Naming.ConvertFromCamelToSnakeCase("GetATM");
        Assert.Equal("get_atm", s4);

        var s5 = Naming.ConvertFromCamelToSnakeCase("GetATMPerson", useAbbreviations: true);
        Assert.Equal("get_atm_person", s5);

        var s6 = Naming.ConvertFromCamelToSnakeCase("GetATMPerson", useAbbreviations: false);
        Assert.Equal("get_atmperson", s6);
    }
}
