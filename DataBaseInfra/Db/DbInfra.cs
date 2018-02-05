using System;
using System.Collections.Generic;
using System.Linq;
using CustomInfra.DataBase.Simple.Attribute;
using CustomInfra.Injector.Simple.IoC;
using CustomInfra.DataBase.Simple.DbContext;
using CustomInfra.Injector.Simple.Enums;
using System.Data;

namespace CustomInfra.DataBase.Simple.Db
{
    public class DbInfra
    {
        private static object _lockObj { get; set; }

        private static ICollection<Type> _mappedClassesList;
        internal static ICollection<Type> MappedClassesList
        {
            get
            {
                lock(_lockObj)
                {
                    if (_mappedClassesList == null || !_mappedClassesList.Any())
                        _mappedClassesList = GetMappedClasses();

                    return _mappedClassesList;
                }
            }
        }

        static DbInfra()
        { _lockObj = new object(); }

        /// <summary>
        /// Register a DbContextInfra in the IoCInfraContainer per WebRequest and store classes with the 'DbInfraMap' attribute
        /// </summary>
        /// <param name="connStringName">Connection String Name</param>
        public static void StartDbContextConfiguration(string connStringName)
        {
            IoCInfra.Container.Register<IDbInfraContext, DbInfraContext>(IoCInfraLifeCycle.WebRequest, connStringName);

            _mappedClassesList = GetMappedClasses();
        }

        /// <summary>
        /// Register a DbContextInfra with default isolation level for transactions per WebRequest in the IoCInfraContainer and store classes with the 'DbInfraMap' attribute
        /// </summary>
        /// <param name="connStringName">Connection String Name</param>
        /// /// <param name="isolationLevel">Isolation Level of transacations</param>
        public static void StartDbContextConfiguration(string connStringName, IsolationLevel isolationLevel)
        {
            IoCInfra.Container.Register<IDbInfraContext, DbInfraContext>(IoCInfraLifeCycle.WebRequest, connStringName, isolationLevel);

            _mappedClassesList = GetMappedClasses();
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
    }
}