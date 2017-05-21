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
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns>Lista de T</returns>
        DbRawSqlQuery<T> SqlQuery<T>(string queryString);

        /// <summary>
        /// Save changes in database context
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Detect changes in changetracker of context
        /// </summary>
        void DetectChanges();

        /// <summary>
        /// DbEntity of database context
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        DbEntityEntry Entry(object obj);
    }
}
