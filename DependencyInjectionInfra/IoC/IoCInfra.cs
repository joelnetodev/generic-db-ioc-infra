﻿using System;
using System.Linq;
using System.Reflection;
using CustomInfra.Injector.Simple.Attribute;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CustomInfra.Injector.Simple.IoC
{
    /// <summary>
    /// Principal management class of Infra IoC
    /// </summary>
    public static class IoCInfra
    {
        private static readonly SimpleInjector.Container _container;

        static IoCInfra()
        {
            if (_container == null)
            {
                _container = new SimpleInjector.Container();
                _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            }
        }


        /// <summary>
        /// Initiate a new scope for 'IoCInfraLifeCycle.Scoped' type registred
        /// </summary>
        /// <returns>Simple Scope</returns>
        public static Scope Scope()
        {
            return AsyncScopedLifestyle.BeginScope(_container);
        }


        /// <summary>
        /// Register all interfaces with 'IoCInfraInitiateAttribute' and its implementations
        /// </summary>
        public static void InitiateAttributeRegistration()
        {
            try
            {
                var allAssemblies = IoCInfraAssemblyLocator.LoadDependencyInjectorAttributeAssemblies();

                if (!allAssemblies.Any()) return;

                var interfacesWithAtt = allAssemblies.SelectMany(types => types.GetTypes()).Where(type =>
                            type.GetCustomAttributes(typeof(IoCInfraInitiateAttribute), true).Length > 0).ToList();

                var assembliesWithClassImplementation = allAssemblies.Where(types =>
                            types.GetTypes().Any(typeImp => interfacesWithAtt.Any(i => i.IsAssignableFrom(typeImp)))).ToList();

                var classImplementation = assembliesWithClassImplementation.SelectMany(g => g.GetTypes()).Where(types =>
                            types.IsClass && interfacesWithAtt.Any(i => i.IsAssignableFrom(types))).ToList();


                foreach (var itemInterface in interfacesWithAtt)
                {
                    foreach (var itemClass in classImplementation.Where(x => x.GetInterfaces().Contains(itemInterface)))
                    {
                        Container.Register(itemInterface, itemClass);
                    }
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

        /// <summary>
        /// Custom client IoC container API
        /// </summary>
        public static class Container
        {
                       
            /// <summary>
            /// Register a interface and implementation in container
            /// </summary>
            /// <typeparam name="T1"></typeparam>
            /// <typeparam name="T2"></typeparam>
            /// <param name="objs">Constructor parameter</param>
            /// <param name="transient">Indicates if is transient or singleton</param>
            public static void Register<T1, T2>(IoCInfraLifeCycle life = IoCInfraLifeCycle.Singleton, params object[] objs)
                where T1 : class
                where T2 : class
            {
                _container.Register<T1>(() => (T1)Activator.CreateInstance(typeof(T2), objs), ChoseLife(life));
            }
            
            /// <summary>
            /// Register a interface and implementation in container
            /// </summary>
            /// <typeparam name="T1">Interface</typeparam>
            /// <typeparam name="T2">Implementation</typeparam>
            /// <param name="transient">Indicates if is transient singleton</param>
            public static void Register<T1, T2>(IoCInfraLifeCycle life = IoCInfraLifeCycle.Singleton)
                where T1 : class
                where T2 : class
            {
                _container.Register(typeof(T1), typeof(T2), ChoseLife(life));
            }

            /// <summary>
            /// Register a interface and implementation in container
            /// </summary>
            /// <param name="T1">Interface</param>
            /// <param name="T2">Implementation</param>
            /// <param name="transient">Indicates if is transient or not</param>
            public static void Register(Type T1, Type T2, IoCInfraLifeCycle life = IoCInfraLifeCycle.Singleton)
            {
                _container.Register(T1, T2, ChoseLife(life));
            }



            /// <summary>
            /// Get instance in container
            /// </summary>
            /// <typeparam name="T">Interface</typeparam>
            /// <returns>Instance of T</returns>
            public static T GetInstance<T>() where T : class
            {
                return _container.GetInstance<T>();
            }

            /// <summary>
            /// Get instance in container
            /// </summary>
            /// <param name="T">Interface</param>
            /// <returns>Instance of T</returns>
            public static object GetInstance(Type T)
            {
                return _container.GetInstance(T);
            }

            private static Lifestyle ChoseLife(IoCInfraLifeCycle life)
            {
                switch (life)
                {
                    case IoCInfraLifeCycle.New: return Lifestyle.Transient;
                    case IoCInfraLifeCycle.Singleton: return Lifestyle.Singleton;
                    default: return Lifestyle.Scoped;
                }
            }
        }
    }
}