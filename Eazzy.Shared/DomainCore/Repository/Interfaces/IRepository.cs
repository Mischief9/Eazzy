using Eazzy.Shared.DomainCore;
using System.Collections.Generic;
using System.Linq;

namespace Eazzy.Infrastructure.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> Table { get; }

        TEntity Find(int id);

        void Add(TEntity entity);

        void Add(IEnumerable<TEntity> entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);
    }
}
