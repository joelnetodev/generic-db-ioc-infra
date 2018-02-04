using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using CustomInfra.DataBase.Simple.Attribute;
using CustomInfra.Injector.Simple.IoC;
using CustomInfra.DataBase.Simple.DbContext;
using System.Collections.ObjectModel;
using CustomInfra.Injector.Simple.Enums;
using SimpleInjector.Lifestyles;
using System.Data;
using SimpleInjector;
using System.Data.Entity;

namespace CustomInfra.DataBase.Simple.Configuration
{
    public class DbInfra
    {
        private static object _lockObj { get; set; }

        private static ICollection<Type> mappedClassesList;
        internal static ICollection<Type> MappedClassesList
        {
            get
            {
                lock(_lockObj)
                {
                    if (mappedClassesList == null || !mappedClassesList.Any())
                        mappedClassesList = GetMappedClasses();

                    return mappedClassesList;
                }
            }
        }

        static DbInfra()
        { _lockObj = new object(); }

        /// <summary>
        /// Register a DbContextInfra in the IoCInfraContainer and Mapping class with the 'DbInfraMap' attribute
        /// </summary>
        /// <param name="connStringName">Connection String Name</param>
        public static void StartDbContextConfiguration(string connStringName)
        {
            IoCInfra.Container.Register<IDbInfraContext, DbInfraContext>(IoCInfraLifeCycle.Scoped, connStringName);

            mappedClassesList = GetMappedClasses();
        }

        /// <summary>
        /// Load types by reflection that contains the 'DbInfraMap' attribute
        /// </summary>
        /// <returns></returns>
        private static List<Type> GetMappedClasses()
        {
            var assemblies = DbInfraAssemblyLocator.LoadDataBaseInfraAttributeAssemblies();

            if (!assemblies.Any()) return new List<Type>();

            var entityTypes = assemblies.SelectMany(ass => ass.GetTypes())
                    .Where(t => t.BaseType != null && t.BaseType.IsGenericType
                        && System.Attribute.IsDefined(t, typeof(DbInfraMapAttribute))).ToList();

            return entityTypes;
        }

        /// <summary>
        /// Begin a new DbContextScope for DbContex
        /// </summary>
        /// <returns></returns>
        public static DbContextScope BeginDbContextScope()
        {
            return new DbContextScope();
        }

        /// <summary>
        /// Begin a new DbContextScope for DbContex and start a new transaction with the chosen Isolation Level
        /// </summary>
        /// <returns></returns>
        public static DbContextScope BeginDbContextScopeWithTransaction(IsolationLevel isolationLevel)
        {
            return new DbContextScope(isolationLevel);
        }

        public class DbContextScope : IDisposable
        {
            private DbContextTransaction _transaction { get; set; }
            private Scope _scope { get; set; }

            /// <summary>
            /// Creates a new DbContextScope for DbContex
            /// </summary>
            /// <returns></returns>
            public DbContextScope()
            {
                _scope = AsyncScopedLifestyle.BeginScope(IoCInfra.Container.GetSimpleInjectorContainer);
            }

            /// <summary>
            /// Creates a new DbContextScope for DbContex and start a new transaction with the chosen Isolation Level
            /// </summary>
            /// <returns></returns>
            public DbContextScope(IsolationLevel isolationLevel)
            {
                _scope = AsyncScopedLifestyle.BeginScope(IoCInfra.Container.GetSimpleInjectorContainer);

                _transaction = IoCInfra.Container.GetInstance<IDbInfraContext>().BeginTransaction(isolationLevel);
            }

            /// <summary>
            /// Commit changes if transaction is created
            /// </summary>
            public void Commit()
            {
                if (IsWithTransactionActive)
                    _transaction.Commit();
            }

            /// <summary>
            /// Rollback changes if transaction is created
            /// </summary>
            public void RollBack()
            {
                if (IsWithTransactionActive)
                    _transaction.Rollback();
            }

            /// <summary>
            /// The current instance of DbContext
            /// </summary>
            public IDbInfraContext CurrentDbContext
            {
                get { return IoCInfra.Container.GetInstance<IDbInfraContext>(); }
            }

            public bool IsWithTransactionActive
            {
                get { return _transaction != null; }
            }

            public void Dispose()
            {
                if(IsWithTransactionActive)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                _scope.Dispose();
                _scope = null;
            }
        }
    }
}