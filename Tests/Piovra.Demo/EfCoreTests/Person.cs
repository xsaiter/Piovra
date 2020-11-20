using Piovra.EfCore.Extensions;
using System.Xml.Linq;

namespace Piovra.Demo.EfCoreTests {
    public class Person : EntityBase {
        public int Id { get; set; }
        public XmlProperty Info { get; set; }
    }
}
