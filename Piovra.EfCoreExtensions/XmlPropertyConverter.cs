using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Xml.Linq;

namespace Piovra.EfCoreExtensions;

public class XmlPropertyConverter(string propertyName)
    : ValueConverter<XmlProperty, string>(
        convertToProviderExpression: _ => _.ToString(),
        convertFromProviderExpression: _ => new XmlProperty(propertyName, XElement.Parse(_)));
