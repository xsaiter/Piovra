using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piovra.Data {

    public class Repository<T, TIdentity> : IRepository<T, TIdentity>
        where T : class, IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity> {
        readonly UnitOfWork _unitOfWork;

        public Repository(UnitOfWork unitOfWork) => _unitOfWork = ARG.NotNull(unitOfWork, nameof(unitOfWork));

        public Task<T> GetById(TIdentity id) => Set().FindAsync(id).AsTask();

        public Task<List<T>> GetAllList() => Set().ToListAsync();

        public Task<List<T>> GetListBy(Expression<Func<T, bool>> predicate) => Set().Where(predicate).ToListAsync();

        public IQueryable<T> GetAll() => Set();

        public Task Add(T entity) => Set().AddAsync(entity).AsTask();

        public async Task Remove(T entity) {
            var existing = await Set().FindAsync(entity.Id);
            if (existing != null) {
                Set().Remove(existing);
            }
        }

        protected DbSet<T> Set() => _unitOfWork.Context.Set<T>();
    }
}
