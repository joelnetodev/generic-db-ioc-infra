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

namespace CustomInfra.DataBase.Simple.Configuration
{
    public static class DbInfra
    {
        private static ICollection<Type> mappedClassesList;
        internal static ICollection<Type> MappedClassesList
        {
            get
            {
                if (mappedClassesList == null || !mappedClassesList.Any())
                    mappedClassesList = GetMappedClasses();

                return mappedClassesList;
            }
            private set
            {
                mappedClassesList = value;
            }
        }

        /// <summary>
        /// Register a DbContextInfra in the IoCInfraContainer and Mapping class with the 'DbInfraMap' attribute
        /// </summary>
        /// <param name="connStringName">Connection String Name</param>
        public static void StartDbContextConfiguration(string connStringName)
        {
            IoCInfra.Container.Register<IDbInfraContext, DbInfraContext>(IoCInfraLifeCycle.Scoped, connStringName);

            MappedClassesList = GetMappedClasses();
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
