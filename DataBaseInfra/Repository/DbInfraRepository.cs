using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomInfra.DataBase.Simple.DbContext;
using CustomInfra.Injector.Simple.IoC;

namespace CustomInfra.DataBase.Simple.Repository
{
    /// <summary>
    /// Infra Repository class
    /// </summary>
    public class DbInfraRepository
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

        /// <summary>
        /// Execute a Sql Query
        /// </summary>
        /// <typeparam name="T">Type of projection</typeparam>
        /// <param name="query">Sql query string</param>
        /// <returns>Sql Query projected on T</returns>
        protected DbRawSqlQuery<T> SqlQuery<T>(string query) where T:class
        {
            return DbContext.SqlQuery<T>(query);
        }

        /// <summary>
        /// Execute a Sql Query command
        /// </summary>
        /// <param name="command">Sql command string</param>
        protected void ExecuteSqlCommand(string command)
        {
            DbContext.SqlCommand(command);
        }
    }
}
