using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using CustomInfra.DataBase.Simple.Attribute;
using CustomInfra.DataBase.Simple.Configuration;

namespace CustomInfra.DataBase.Simple.DbContext
{
    /// <summary>
    /// Infra DbContext class. DatabaseInitializer doesn't create or change database.
    /// </summary>
    public class DbInfraContext : System.Data.Entity.DbContext, IDbInfraContext
    {
        /// <summary>
        /// Indicates if the DbContext Object has been disposed
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Returns the Connction String name of DbContext
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// DbContext class
        /// </summary>
        /// <param name="connectionString">Connection string name</param>
        public DbInfraContext(string connectionString)
            : base(connectionString)
        {
            ConnectionString = connectionString;
            Database.SetInitializer<DbInfraContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);

                foreach (var type in DbInfra.MappedClassesList)
                {
                    dynamic instanciaMap = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(instanciaMap);
                }
            }
            catch (ReflectionTypeLoadException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DbRawSqlQuery<T> SqlQuery<T>(string query)
        {
            return base.Database.SqlQuery<T>(query, new object[] { });
        }
        public void SqlCommand(string command)
        {
            base.Database.ExecuteSqlCommand(command, new object[] { });
        }

        public void SaveChanges(bool detectChanges = false)
        {
            if (detectChanges)
                base.ChangeTracker.DetectChanges();

            base.SaveChanges();
        }

        protected new void Dispose()
        {
            this.Disposed = true;
            ConnectionString = null;

            base.Dispose();
        }
        protected override void Dispose(bool disposing)
        {
            this.Disposed = true;
            ConnectionString = null;

            base.Dispose(disposing);
        }
    }
}
