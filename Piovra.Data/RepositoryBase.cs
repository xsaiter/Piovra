using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Piovra.Data;

public class RepositoryBase<T, TIdentity> : IRepository<T, TIdentity>
    where T : class, IEntity<TIdentity>
    where TIdentity : IEquatable<TIdentity> {

    protected readonly UnitOfWork _unitOfWork;

    protected RepositoryBase(UnitOfWork unitOfWork) {
        _unitOfWork = Requires.NotNull(unitOfWork, nameof(unitOfWork));
    }

    public Task<T?> GetByIdAsync(TIdentity id) =>
        Set().FindAsync(id).AsTask();

    public Task<List<T>> GetAllListAsync() =>
        Set().ToListAsync();

    public Task<List<T>> GetListByAsync(Expression<Func<T, bool>> predicate) =>
        Set().Where(predicate).ToListAsync();

    public IQueryable<T> GetAll() => Set();

    public Task AddAsync(T entity) =>
        Set().AddAsync(entity).AsTask();

    public async Task RemoveAsync(T entity) {
        var existing = await Set().FindAsync(entity.Id);
        if (existing != null) {
            Set().Remove(existing);
        }
    }

    protected DbSet<T> Set() => _unitOfWork.Context.Set<T>();
}
