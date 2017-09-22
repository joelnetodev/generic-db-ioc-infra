using System.Collections.Generic;


namespace CustomInfra.DataBase.Simple.Repository
{
    /// <summary>
    /// Infra Repository interface
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public interface IDbInfraRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Add entity to database context
        /// </summary>
        /// <param name="obj"></param>
        void Add(TEntity obj);

        /// <summary>
        /// Add entities to database context
        /// </summary>
        /// <param name="listObj"></param>
        void Add(IEnumerable<TEntity> listObj);

        /// <summary>
        /// Set state entity to modified
        /// </summary>
        /// <param name="obj"></param>
        void Update(TEntity obj);

        /// <summary>
        /// Set state entities to modified
        /// </summary>
        /// <param name="objs"></param>
        void Update(IEnumerable<TEntity> objs);

        /// <summary>
        /// Remove entity from database context
        /// </summary>
        /// <param name="obj"></param>
        void Delete(TEntity obj);

        /// <summary>
        /// Remove entities from database context
        /// </summary>
        /// <param name="objs"></param>
        void Delete(IEnumerable<TEntity> objs);

        /// <summary>
        /// Find and remove entity by id
        /// </summary>
        /// <param name="id"></param>
        void DeleteById(int id);

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(int id);

        /// <summary>
        /// Get all entities in database
        /// </summary>
        /// <returns></returns>
        ICollection<TEntity> GetAll();

        /// <summary>
        /// Save changes in database context
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Detect changes in changetracker of context
        /// </summary>
        void DetectChanges();

        /// <summary>
        /// Call Dispose of DbContext
        /// </summary>
        void DisposeDbContext();
    }
}