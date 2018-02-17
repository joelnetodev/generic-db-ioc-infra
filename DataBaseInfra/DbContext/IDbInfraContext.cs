using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CustomInfra.DataBase.Simple.DbContext
{
    /// <summary>
    /// Infra DbContext interface.
    /// </summary>
    public interface IDbInfraContext : IDisposable
    {
        /// <summary>
        /// Connection string name
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Set entity of database context
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Execute a Sql Query that return results
        /// </summary>
        /// <typeparam name="T">Type of projection</typeparam>
        /// <param name="query">Sql query string</param>
        /// <returns>Sql Query projected on T</returns>
        DbRawSqlQuery<T> ExecuteSqlQuery<T>(string query) where T : class;

        /// <summary>
        /// Execute a Sql command
        /// </summary>
        /// <param name="command">Sql command string</param>
        void ExecuteSqlCommand(string command);

        /// <summary>
        /// Save changes on DbContext
        /// </summary>
        /// <param name="commitCurrentTransaction">Commit the current transaction if it was created</param>
        void SaveChanges(bool commitCurrentTransaction = false);

        /// <summary>
        /// DbEntity of database context
        /// </summary>
        /// <param name="obj">Ocject Entity</param>
        /// <returns></returns>
        DbEntityEntry Entry(object obj);
    }
}
