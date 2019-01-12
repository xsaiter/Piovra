using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piovra.Data {
    public interface IRepository<T, TIdentity>
        where T : class, IEntity<TIdentity> where TIdentity : IEquatable<TIdentity> {
        Task<T> GetById(TIdentity id);
        Task<List<T>> GetAllList();
        Task<List<T>> GetListBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        Task Add(T entity);
        Task Remove(T entity);
    }

    public class Repository<T, TIdentity> : IRepository<T, TIdentity>
        where T : class, IEntity<TIdentity> where TIdentity : IEquatable<TIdentity> {
        readonly UnitOfWork _unitOfWork;

        public Repository(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public Task<T> GetById(TIdentity id) {
            return Set().FindAsync(id);
        }

        public Task<List<T>> GetAllList() {
            return Set().ToListAsync();
        }

        public Task<List<T>> GetListBy(Expression<Func<T, bool>> predicate) {
            return Set().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAll() {
            return Set();
        }

        public Task Add(T entity) {
            return Set().AddAsync(entity);
        }

        public async Task Remove(T entity) {
            var existing = await Set().FindAsync(entity.Id);
            if (existing != null) {
                Set().Remove(existing);
            }
        }

        protected DbSet<T> Set() => _unitOfWork.Context.Set<T>();
    }
}
