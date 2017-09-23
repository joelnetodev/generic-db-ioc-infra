using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using CustomInfra.DataBase.Simple.Attribute;

namespace CustomInfra.DataBase.Simple.DbContext
{
    /// <summary>
    /// Infra DbContext class. DatabaseInitializer doesn't create or change database.
    /// </summary>
    public class DbContextInfra : System.Data.Entity.DbContext, IDbInfraContext
    {
        public bool Disposed { get; private set; }

        public string ConnectionString { get; private set; }

        /// <summary>
        /// DbContext class
        /// </summary>
        /// <param name="connectionString">Connection string name</param>
        public DbContextInfra(string connectionString)
            : base(connectionString)
        {
            ConnectionString = connectionString;
            Database.SetInitializer<DbContextInfra>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);

                var assemblies = DbInfraAssemblyLocator.LoadDataBaseInfraAttributeAssemblies();

                if (!assemblies.Any()) return;

                //Recupera todos os tipos que herdam de EntityTypeConfiguration com atributo DbContextInfraAttribute que tenham a mesma Connectionstring
                var entityTypes = assemblies.SelectMany(ass => ass.GetTypes())
                        .Where(t => t.BaseType != null && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                            && System.Attribute.IsDefined(t, typeof(DbInfraMapAttribute)));

                foreach (var type in entityTypes)
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
