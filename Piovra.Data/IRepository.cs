using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piovra.Data {
    public interface IRepository<T, TIdentity>
        where T : class, IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity> {
        Task<T> GetById(TIdentity id);

        Task<List<T>> GetAllList();

        Task<List<T>> GetListBy(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        Task Add(T entity);

        Task Remove(T entity);
    }
}
