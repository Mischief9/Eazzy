using Eazzy.Infrastructure.Models;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eazzy.Infrastructure.Repository.Abstract
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly EazzyDbContext _db;

        public Repository(EazzyDbContext db)
        {
            _db = db;
        }

        public IQueryable<TEntity> Table => _db.Set<TEntity>();

        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _db.Set<TEntity>().Add(entity);

            _db.SaveChanges();
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _db.Set<TEntity>().AddRange(entities);

            _db.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _db.Set<TEntity>().Remove(entity);

            _db.SaveChanges();
        }

        public TEntity Find(int id)
        {
            var entity = _db.Set<TEntity>().Find(id);

            return entity;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _db.Set<TEntity>().Update(entity);

            _db.SaveChanges();
        }
    }
}
