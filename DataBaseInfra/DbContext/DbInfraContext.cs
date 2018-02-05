using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using CustomInfra.DataBase.Simple.Db;

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

        private bool IsTransatcionCommited { get; set; }
        private DbContextTransaction CurrentTransacion { get; set; }


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

        /// <summary>
        /// DbContext class with transaction started
        /// </summary>
        /// <param name="connectionString">Connection string name</param>
        ///  /// <param name="isolationLevel">Isolation Level of the transaction</param>
        public DbInfraContext(string connectionString, IsolationLevel isolationLevel)
           : base(connectionString)
        {
            ConnectionString = connectionString;
            Database.SetInitializer<DbInfraContext>(null);

            CurrentTransacion = base.Database.BeginTransaction(isolationLevel);
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


        public DbRawSqlQuery<T> ExecuteSqlQuery<T>(string query) where T:class
        {
            return base.Database.SqlQuery<T>(query, new object[] { });
        }
        public void ExecuteSqlCommand(string command)
        {
            base.Database.ExecuteSqlCommand(command, new object[] { });
        }


        public void SaveChanges(bool commitCurrentTransaction = false)
        {
            base.ChangeTracker.DetectChanges();
            base.SaveChanges();

            if(commitCurrentTransaction && CurrentTransacion != null)
            {
                CurrentTransacion.Commit();
                IsTransatcionCommited = true;
            }
        }


        public new void Dispose()
        {
            DisposeObjects();
            base.Dispose();
        }

        private void DisposeObjects()
        {
            this.Disposed = true;
            ConnectionString = null;

            if(CurrentTransacion != null)
            {
                if(!IsTransatcionCommited)
                    CurrentTransacion.Rollback();

                CurrentTransacion.Dispose();
            }
        }
    }
}