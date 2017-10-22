using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CustomInfra.DataBase.Simple.DbContext;
using CustomInfra.Injector.Simple.IoC;

namespace CustomInfra.DataBase.Simple.Repository
{
    /// <summary>
    /// Generic Infra Repository class.
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public class DbInfraRepository<TEntity> : IDbInfraRepository<TEntity> where TEntity : class
    {
        private IDbInfraContext _dbContext;

        private IDbInfraContext DbContext
        {
            get
            {
                if (_dbContext == null || _dbContext.Disposed)
                    _dbContext = IoCInfra.Container.GetInstance<IDbInfraContext>();

                return _dbContext;
            }
        }


        private DbSet<TEntity> _dbEntity;
        /// <summary>
        /// Entity of respository 
        /// </summary>
        protected DbSet<TEntity> DbEntity
        {
            get
            {
                if (_dbEntity == null || _dbContext == null || _dbContext.Disposed)
                    _dbEntity = DbContext.Set<TEntity>();

                return _dbEntity;
            }
        }

        /// <summary>
        /// Set other entity in  context
        /// </summary>
        protected DbSet<T> SetDbEntity<T>() where T : class
        {
            return DbContext.Set<T>();
        }



        /// <summary>
        /// Execute a Sql Query
        /// </summary>
        /// <typeparam name="T">Type of projection</typeparam>
        /// <param name="query">Sql query string</param>
        /// <returns>Sql Query projected on T</returns>
        protected DbRawSqlQuery<T> SqlQuery<T>(string query) where T : class
        {
            return DbContext.SqlQuery<T>(query);
        }
        /// <summary>
        /// Execute a Sql Query command
        /// </summary>
        /// <param name="command">Sql command string</param>
        protected void SqlCommand(string command)
        {
            DbContext.SqlCommand(command);
        }



        public void Add(TEntity obj)
        {
            Add(new Collection<TEntity> {obj});
        }

        public void Add(IEnumerable<TEntity> listObj)
        {
            foreach (var obj in listObj)
            {
                DbContext.Entry(obj).State = EntityState.Added;
            }
        }

        public void Update(TEntity obj)
        {
            Update(new Collection<TEntity> {obj});
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
            Delete(new Collection<TEntity> {obj});
        }

        public void Delete(IEnumerable<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                DbContext.Entry(obj).State = EntityState.Deleted;
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



        public void SaveChanges(bool detectChanges = false)
        {
            DbContext.SaveChanges(detectChanges);
        }
        
        public void DisposeDbContext()
        {
            DbContext.Dispose();
        }
    }
}
