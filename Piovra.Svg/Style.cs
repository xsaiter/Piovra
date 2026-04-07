using System.Xml.Linq;

namespace Piovra.Svg;

public record Style(Color? FillColor = null, Color? StrokeColor = null, int? StrokeWidth = null) {
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
