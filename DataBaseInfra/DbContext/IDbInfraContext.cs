using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace CustomInfra.DataBase.Simple.DbContext
{
    /// <summary>
    /// Infra DbContext interface.
    /// </summary>
    public interface IDbInfraContext : IDisposable
    {
        /// <summary>
        /// Indicates if the DbContext has been disposed
        /// </summary>
        bool Disposed { get; }

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
        DbRawSqlQuery<T> SqlQuery<T>(string query) where T : class;

        /// <summary>
        /// Execute a Sql Query command
        /// </summary>
        /// <param name="command">Sql command string</param>
        void SqlCommand(string command);

        /// <summary>
        /// Save changes on DbContext
        /// </summary>
        /// <param name="detectChanges">Indicates if search for changes in tracker</param>
        void SaveChanges(bool detectChanges = false);

        /// <summary>
        /// DbEntity of database context
        /// </summary>
        /// <param name="obj">Ocject Entity</param>
        /// <returns></returns>
        DbEntityEntry Entry(object obj);
    }
}
