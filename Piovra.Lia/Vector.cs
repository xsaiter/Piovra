using System;

namespace Piovra.Lia {
    public class Vector {
        readonly double[] _a;

        public Vector(double[] a) {
            N = a.Length;
            _a = a;
        }

        public Vector(int length) : this(new double[length]) { }        

        public static Vector Create(int lenght, double initialValue) {
            var v = new Vector(lenght);            
            v.SetForAll(initialValue);
            return v;
        }

        public int N { get; }

        public double this[int i] {
            get {
                return _a[i];
            }
            set {
                _a[i] = value;
            }
        }

        public void SetForAll(double value) {
            for (var i = 0; i < N; ++i) {
                _a[i] = value;
            }
        }

        public Vector Transform(Func<double, double> transform) => Transform(transform, _ => true);

        public Vector Transform(Func<double, double> transform, Func<double, bool> canTransform) {
            for (var i = 0; i < N; ++i) {
                if (canTransform(_a[i])) {
                    _a[i] = transform(_a[i]);
                }
            }
            return this;
        }

        public static Vector operator +(Vector a, double v) => a.Copy().Transform(x => x + v);
        public static Vector operator -(Vector a, double v) => a.Copy().Transform(x => x - v);
        public static Vector operator *(Vector a, double v) => a.Copy().Transform(x => x * v);
        public static Vector operator /(Vector a, double v) => a.Copy().Transform(x => x / v);

        void AssertEqLengths(Vector other) {
            if (N != other.N) {
                throw new ArgumentException("lengths not equal");
            }
        }

        public Vector Copy() {
            return new Vector(CopyArray());
        }

        double[] CopyArray() {
            return _a.Clone() as double[];
        }
    }
}