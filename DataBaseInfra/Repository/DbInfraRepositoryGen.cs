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
        protected IDbInfraContext DbContext
        {
            get
            {
                return IoCInfra.Container.GetInstance<IDbInfraContext>();
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
                return DbContext.Set<TEntity>();
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
        protected DbRawSqlQuery<T> ExecuteSqlQuery<T>(string query) where T : class
        {
            return DbContext.ExecuteSqlQuery<T>(query);
        }
        /// <summary>
        /// Execute a Sql Query command
        /// </summary>
        /// <param name="command">Sql command string</param>
        protected void ExecuteSqlCommand(string command)
        {
            DbContext.ExecuteSqlCommand(command);
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



        public void SaveChanges(bool commitCurrentTransaction = false)
        {
            DbContext.SaveChanges(commitCurrentTransaction);
        }
        
        public void DisposeDbContext()
        {
            DbContext.Dispose();
        }
    }
}
