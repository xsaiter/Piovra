using Xunit;

namespace Piovra.Tests {
    public class RegistryTests {
        public interface ITestService { }
        public class TestService : ITestService { }
        public class Registry : RegistryBase {
            public static ITestService GetTestService() => GetConfig().Get<ITestService>();            
        }

        [Fact]
        public void Test() {            
            var cfg = RegistryBase.Config.New().Add<ITestService, TestService>();
            RegistryBase.ChangeConfig(cfg);
            var service = Registry.GetTestService();

            Assert.NotNull(service);
        }
    }
}
