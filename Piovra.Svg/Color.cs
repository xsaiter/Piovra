namespace Piovra.Svg {
    public class Color {
        public Color(byte r, byte g, byte b, double a = 1.0) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public double A { get; }

        public static Color RED => new Color(H, L, L);
        public static Color GREEN => new Color(L, H, L);
        public static Color BLUE => new Color(L, L, H);
        public static Color WHITE => new Color(H, H, H);
        public static Color BLACK => new Color(L, L, L);

        readonly static byte H = byte.MaxValue;
        readonly static byte L = byte.MinValue;

        public override string ToString() => $"rgba({R},{G},{B},{A})";
    }
}