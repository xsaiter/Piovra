using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Xml.Linq;

namespace Piovra.EfCoreExtensions;

public class XmlPropertyConverter : ValueConverter<XmlProperty, string> {
    public XmlPropertyConverter(string propertyName)
        : base(convertToProviderExpression: _ => _.ToString(),
            convertFromProviderExpression: _ => new XmlProperty(propertyName, XElement.Parse(_), null)) { }
}
