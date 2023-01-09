using System.Xml.Linq;

namespace Piovra.Svg;
public abstract class Drawing {
    public abstract XElement ToSvg();

    protected static readonly XNamespace NS = @"http://www.w3.org/2000/svg";
    protected static XElement NewXElement(string tag, params object[] content) => new(NS + tag, content);
}