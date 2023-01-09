using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piovra.Data;

public interface IRepository<T, TIdentity>
    where T : class, IEntity<TIdentity>
    where TIdentity : IEquatable<TIdentity> {
    Task<T> GetByIdAsync(TIdentity id);

    Task<List<T>> GetAllListAsync();

    Task<List<T>> GetListByAsync(Expression<Func<T, bool>> predicate);

    IQueryable<T> GetAll();

    Task AddAsync(T entity);

    Task RemoveAsync(T entity);
}
