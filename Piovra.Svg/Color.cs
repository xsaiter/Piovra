namespace Piovra.Svg;

public record Color(byte R, byte G, byte B, double A = 1.0) {
    public static Color RED => new(H, L, L);
    public static Color GREEN => new(L, H, L);
    public static Color BLUE => new(L, L, H);
    public static Color WHITE => new(H, H, H);
    public static Color BLACK => new(L, L, L);

    readonly static byte H = byte.MaxValue;
    readonly static byte L = byte.MinValue;

    public override string ToString() {
        return string.Create(System.Globalization.CultureInfo.InvariantCulture, $"rgba({R},{G},{B},{A})");
    }
}
