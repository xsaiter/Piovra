using System;

namespace Piovra.Lia;

public abstract class LinearSystemSolver {
    public abstract Vector Solve(Matrix a, Vector b);
    public static LinearSystemSolver CreateJacobiMethod() => new JacobiMethod();
}

public class JacobiMethod : LinearSystemSolver {
    public override Vector Solve(Matrix a, Vector b) {
        var n = b.N;
        var r = Vector.Create(n, 1.0);
        var c = new Vector(n);
        double e;

        do {
            for (int i = 0; i < n; ++i) {
                c[i] = b[i];
                for (int j = 0; j < n; ++j) {
                    if (i != j) {
                        c[i] -= a[i, j] * r[j];
                    }
                }
                c[i] /= a[i, i];
            }
            e = Math.Abs(r[0] - c[0]);
            for (int i = 0; i < n; ++i) {
                var delta = Math.Abs(r[i] - c[i]);
                if (delta > e) {
                    e = delta;
                }
                r[i] = c[i];
            }
        } while (e > Help.EPSILON);

        return r;
    }
}