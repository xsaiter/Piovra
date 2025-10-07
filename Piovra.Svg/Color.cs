namespace Piovra.Svg;

public class Color(byte r, byte g, byte b, double a = 1.0) {
    public byte R { get; } = r;
    public byte G { get; } = g;
    public byte B { get; } = b;
    public double A { get; } = a;

    public static Color RED => new(H, L, L);
    public static Color GREEN => new(L, H, L);
    public static Color BLUE => new(L, L, H);
    public static Color WHITE => new(H, H, H);
    public static Color BLACK => new(L, L, L);

    readonly static byte H = byte.MaxValue;
    readonly static byte L = byte.MinValue;

    public override string ToString() => $"rgba({R},{G},{B},{A})";
}
