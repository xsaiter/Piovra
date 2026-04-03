using System.Xml.Linq;

namespace Piovra.Svg;

public class Image {
    Image(SVG root, XDocument xml) {
        Root = root;
        Xml = xml;
    }
    public SVG Root { get; }
    public XDocument Xml { get; }

    public static Image Load(SVG root) {
        if (!root.IsRoot) {
            throw new ArgumentException("No root");
        }
        var xml = new XDocument(
            declaration: new XDeclaration("1.0", "utf-8", "no"),
            content: root.ToSvg());

        return new Image(root, xml);
    }
}
