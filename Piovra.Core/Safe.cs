using System;
using System.Threading.Tasks;

namespace Piovra {
    public static class Safe {
        public static Task Do(Func<Task> f, After after) =>
            Run(new Cmd<Task>(f), after);

        public static Task Do<T>(Func<T, Task> f, T arg, After after) =>
            Run(new Cmd<T, Task>(arg, f), after);

        public static Task Do<T1, T2>(Func<T1, T2, Task> f, T1 arg1, T2 arg2, After after) =>
            Run(new Cmd<T1, T2, Task>(arg1, arg2, f), after);

        public static Task<R> Do<T, R>(Func<T, Task<R>> f, T arg, R fail, After after) =>
            Run(new Cmd<T, Task<R>>(arg, f), fail, after);

        public static Task<R> Do<T1, T2, R>(Func<T1, T2, Task<R>> f, T1 arg1, T2 arg2, R fail, After after) =>
            Run(new Cmd<T1, T2, Task<R>>(arg1, arg2, f), fail, after);

        public static R Do<R>(Func<R> f, R fail, After after) =>
            Run(new Cmd<R>(f), fail, after);

        public static R Do<T, R>(Func<T, R> f, T arg, R fail, After after) =>
            Run(new Cmd<T, R>(arg, f), fail, after);

        public static R Do<T1, T2, R>(Func<T1, T2, R> f, T1 arg1, T2 arg2, R fail, After after) =>
            Run(new Cmd<T1, T2, R>(arg1, arg2, f), fail, after);

        public static void Do(Action f, After after) =>
            Run(new Cmd<int>(f.AsFunc()), after);

        public static void Do<T>(Action<T> f, T arg, After after) =>
            Run(new Cmd<T, int>(arg, f.AsFunc()), after);

        public static void Do<T1, T2>(Action<T1, T2> f, T1 arg1, T2 arg2, After after) =>
            Run(new Cmd<T1, T2, int>(arg1, arg2, f.AsFunc()), after);

        static Func<int> AsFunc(this Action action) => () => { action(); return 0; };
        static Func<T, int> AsFunc<T>(this Action<T> action) => arg => { action(arg); return 0; };
        static Func<T1, T2, int> AsFunc<T1, T2>(this Action<T1, T2> action) => (arg1, arg2) => { action(arg1, arg2); return 0; };

        public abstract class After {
            public After Next { get; set; }
            public void ProcessOk() {
                ExecuteOk();
                Next?.ProcessOk();
            }
            public void ProcessFailed(Exception e) {
                ExecuteFailed(e);
                Next?.ProcessFailed(e);
            }
            protected abstract void ExecuteOk();
            protected abstract void ExecuteFailed(Exception e);
        }

        static void Run(ICmd<int> cmd, After after) {
            try {
                cmd.Execute();
                after.ProcessOk();
            } catch (Exception e) {
                after.ProcessFailed(e);
            }
        }

        static R Run<R>(ICmd<R> cmd, R fail, After after) {
            try {
                var result = cmd.Execute();
                after.ProcessOk();
                return result;
            } catch (Exception e) {
                after.ProcessFailed(e);
                return fail;
            }
        }

        static async Task Run(ICmd<Task> cmd, After after) {
            try {
                await cmd.Execute();
                after.ProcessOk();
            } catch (Exception e) {
                after.ProcessFailed(e);
            }
        }

        static async Task<R> Run<R>(ICmd<Task<R>> cmd, R fail, After after) {
            try {
                var result = await cmd.Execute();
                after.ProcessOk();
                return result;
            } catch (Exception e) {
                after.ProcessFailed(e);
                return fail;
            }
        }

        public interface ICmd<R> {
            R Execute();
        }

        class Cmd<R> : ICmd<R> {
            protected Cmd() { }
            public Cmd(Func<R> f) => F0 = f;
            public Func<R> F0 { get; }
            public virtual R Execute() => F0();
        }

        class Cmd<T, R> : Cmd<R> {
            protected Cmd(T arg) => Arg = arg;
            public Cmd(T arg, Func<T, R> f) : this(arg) => F = f;
            public T Arg { get; }
            public Func<T, R> F { get; }
            public override R Execute() => F(Arg);
        }

        class Cmd<T1, T2, R> : Cmd<T1, R> {
            protected Cmd(T1 arg1, T2 arg2) : base(arg1) => Arg2 = arg2;
            public Cmd(T1 arg1, T2 arg2, Func<T1, T2, R> f) : this(arg1, arg2) => F2 = f;
            public T2 Arg2 { get; }
            public Func<T1, T2, R> F2 { get; }
            public override R Execute() => F2(Arg, Arg2);
        }
    }
}
