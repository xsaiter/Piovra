using System.Xml.Linq;

namespace Piovra.Svg {
    public class Style {
        public Color FillColor { get; set; }
        public Color StrokeColor { get; set; }
        public int? StrokeWidth { get; set; }

        public void WriteTo(XElement x) {
            if (FillColor != null) {
                x.Add(new XAttribute("fill", FillColor.ToString()));
            }
            if (StrokeColor != null) {
                x.Add(new XAttribute("stroke", StrokeColor.ToString()));
            }
            if (StrokeWidth.HasValue) {
                x.Add(new XAttribute("stroke-width", StrokeWidth));
            }
        }
    }
}