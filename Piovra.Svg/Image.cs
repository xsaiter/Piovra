using System;
using System.Xml.Linq;

namespace Piovra.Svg {
    public class Image {
        public static Image Make(SVG root) {
            if (!root.IsRoot) {
                throw new ArgumentException("no root");
            }
            return new Image(root) {
                Xml = new XDocument(new XDeclaration("1.0", "utf-8", "no"), root.ToSvg())
            };
        }
        Image(SVG root) => Root = root;        
        public SVG Root { get; }
        public XDocument Xml { get; private set; }
    }
}