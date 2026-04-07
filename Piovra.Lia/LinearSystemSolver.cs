namespace Piovra.Lia;

public abstract class LinearSystemSolver {
    public abstract Vector Solve(Matrix a, Vector b);
    public static LinearSystemSolver CreateJacobiMethod() => new JacobiMethod();
}
