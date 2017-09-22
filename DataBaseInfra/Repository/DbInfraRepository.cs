using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using CustomInfra.DataBase.Simple.DbContext;
using CustomInfra.Injector.Simple.IoC;

namespace CustomInfra.DataBase.Simple.Repository
{
    /// <summary>
    /// Infra Repository class.
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public class DbInfraRepository<TEntity> : IDbInfraRepository<TEntity> where TEntity : class
    {
        private IDbInfraContext _dbContext;
        protected IDbInfraContext DbContext
        {
            get
            {
                if (_dbContext == null || _dbContext.Disposed)
                    _dbContext = IoCInfra.Container.GetInstance<IDbInfraContext>();

                return _dbContext;
            }
        }


        private DbSet<TEntity> _dbEntity;
        protected DbSet<TEntity> DbEntity
        {
            get
            {
                if (_dbEntity == null || _dbContext == null || _dbContext.Disposed)
                    _dbEntity = DbContext.Set<TEntity>();

                return _dbEntity;
            }
        }



        public void Add(TEntity obj)
        {
            Add(new Collection<TEntity> { obj });
        }
        public void Add(IEnumerable<TEntity> listObj)
        {
            foreach (var entity in listObj)
            {
                DbEntity.Add(entity);
            }
        }

        public void Update(TEntity obj)
        {
            Update(new Collection<TEntity> { obj });
        }
        public void Update(IEnumerable<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                DbContext.Entry(obj).State = EntityState.Modified;
            }
        }

        public void Delete(TEntity obj)
        {
            Delete(new Collection<TEntity> { obj });
        }
        public void Delete(IEnumerable<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                DbEntity.Remove(obj);
            }
        }



        public void DeleteById(int id)
        {
            var obj = DbEntity.Find(id);
            if (obj != null)
            {
                DbEntity.Remove(obj);
            }
        }
        public TEntity GetById(int id)
        {
            return DbEntity.Find(id);
        }
        public ICollection<TEntity> GetAll()
        {
            return DbEntity.ToList();
        }



        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
        public void DetectChanges()
        {
            DbContext.DetectChanges();
        }



        public void DisposeDbContext()
        {
            DbContext.Dispose();
        }
    }
}
