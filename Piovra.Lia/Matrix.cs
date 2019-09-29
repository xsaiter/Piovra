using System;

namespace Piovra.Lia {
    public class Matrix {
        readonly double[,] _a;

        public Matrix(double[,] a) {
            _a = a;
            M = a.Rows();
            N = a.Cols();
        }

        public Matrix(int m, int n) : this(new double[m, n]) { }
        public static Matrix Square(int n) => new Matrix(n, n);

        public int M { get; }
        public int N { get; }

        public double this[int i, int j] {
            get => _a[i, j];
            set => _a[i, j] = value;
        }

        public static Matrix operator *(Matrix a, Matrix b) {
            var c = new Matrix(a.M, b.N);
            for (var i = 0; i < a.M; ++i) {
                for (var j = 0; j < b.N; ++j) {
                    for (var k = 0; k < a.N; ++k) {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return c;
        }

        public Matrix Transform(Func<double, double> transform) => Transform(transform, _ => true);

        public Matrix Transform(Func<double, double> transform, Func<double, bool> canTransform) {
            for (var i = 0; i < M; ++i) {
                for (var j = 0; j < N; ++j) {
                    if (canTransform(_a[i, j])) {
                        _a[i, j] = transform(_a[i, j]);
                    }
                }
            }
            return this;
        }

        public static Matrix operator +(Matrix a, double v) => a.Copy().Transform(x => x + v);
        public static Matrix operator -(Matrix a, double v) => a.Copy().Transform(x => x - v);
        public static Matrix operator *(Matrix a, double v) => a.Copy().Transform(x => x * v);
        public static Matrix operator /(Matrix a, double v) => a.Copy().Transform(x => x / v);

        public static bool Eq(double[,] a, double[,] b) {
            var ra = a.Rows();
            var ca = a.Cols();
            var rb = b.Rows();
            var cb = b.Cols();
            if (ra != rb || ca != cb) {
                return false;
            }
            for (var i = 0; i < ra; ++i) {
                for (var j = 0; j < ca; ++j) {
                    if (a[i, j].Eq(b[i, j])) {
                        return false;
                    }
                }
            }
            return true;
        }

        public Matrix Copy() => new Matrix(CopyArray());
        double[,] CopyArray() => _a.Clone() as double[,];
    }
}