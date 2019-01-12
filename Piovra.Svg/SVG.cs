using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Piovra.Svg {
    public class SVG : Drawing {
        public static SVG Make(int w, int h) => new SVG(w, h, false);
        public static SVG MakeRoot(int w, int h) => new SVG(w, h, true);

        SVG(int w, int h, bool isRoot) {
            W = w;
            H = h;
            IsRoot = isRoot;
        }

        public int W { get; }
        public int H { get; }
        public bool IsRoot { get; }
        List<Drawing> Drawings { get; } = new List<Drawing>();

        public SVG AddShape(Shape shape) {
            return AddDrawing(shape);
        }

        public SVG AddSvg(SVG svg) {
            if (svg.IsRoot) {
                throw new Exception("SVG: must have exactly one root");
            }
            return AddDrawing(svg);
        }

        SVG AddDrawing(Drawing item) {
            Drawings.Add(item);
            return this;
        }

        public override XElement ToSvg() {
            var result = NewXElement("svg", new XAttribute("width", $"{W}"), new XAttribute("height", $"{H}"));
            if (IsRoot) {
                result.Add(new XAttribute("baseProfile", "full"));
                result.Add(new XAttribute(XNamespace.Xmlns + "xlink", "http://www.w3.org/1999/xlink"));
                result.Add(new XAttribute(XNamespace.Xmlns + "ev", "http://www.w3.org/2001/xml-events"));
            }
            Drawings.ForEach(_ => result.Add(_.ToSvg()));
            return result;
        }
    }
}
