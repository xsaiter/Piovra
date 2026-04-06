using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Xml.Linq;

namespace Piovra.EfCoreExtensions;

public class XmlPropertyConverter(string propertyName)
    : ValueConverter<XmlProperty?, string>(
        convertToProviderExpression: x => x != null ? x.ToString() : "",
        convertFromProviderExpression: _ => new XmlProperty(propertyName, XElement.Parse(_)));
