namespace Piovra {
    public interface IVisitor<T> {
        void Visit(T element);
    }

    public interface IVisitor<T, R> {
        R Visit(T element);
    }

    public interface IVisitable { }

    public interface IVisitable<T> : IVisitable {
        void Accept(IVisitor<T> visitor);
    }

    public interface IVisitable<T, R> : IVisitable {
        R Accept(IVisitor<T, R> visitor);
    }

    public abstract class VisitableBase<T> where T : class, IVisitable {
        protected T Self => this as T;
    }

    public abstract class Visitable<T> : VisitableBase<T>, IVisitable<T> where T : class, IVisitable {
        public void Accept(IVisitor<T> visitor) => visitor.Visit(Self);
    }

    public abstract class Visitable<T, R> : VisitableBase<T>, IVisitable<T, R> where T : class, IVisitable {
        public R Accept(IVisitor<T, R> visitor) => visitor.Visit(Self);
    }
}