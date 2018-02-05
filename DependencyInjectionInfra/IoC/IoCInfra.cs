using System;
using System.Linq;
using System.Reflection;
using CustomInfra.Injector.Simple.Attribute;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using CustomInfra.Injector.Simple.Enums;

namespace CustomInfra.Injector.Simple.IoC
{
    /// <summary>
    /// Principal class of Infra IoC
    /// </summary>
    public static class IoCInfra
    {
        private static object _lockObj { get; set; }

        private static SimpleInjector.Container _simpleContainer;
        private static SimpleInjector.Container SimpleContainer
        {
            get
            {
                lock (_lockObj)
                {
                    if (_simpleContainer == null)
                    {
                        _simpleContainer = new SimpleInjector.Container();
                        _simpleContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
                    }
                    return _simpleContainer;
                }             
            }
        }

        /// <summary>
        /// Initiate a new scope for 'IoCInfraLifeCycle.Scoped' type registered
        /// </summary>
        /// <returns>Simple Scope</returns>
        internal static Scope BeginScope()
        {
            return AsyncScopedLifestyle.BeginScope(SimpleContainer);
        }

        static IoCInfra()
        {
            _lockObj = new object();
        }

        /// <summary>
        /// Register all interfaces with 'IoCInfraRegisterAttribute' and its implementations
        /// </summary>
        public static void StartAttributeRegistration()
        {
            try
            {
                var allAssemblies = IoCInfraAssemblyLocator.LoadDependencyInjectorAttributeAssemblies();

                if (!allAssemblies.Any()) return;

                var interfacesWithAtt = allAssemblies.SelectMany(types => types.GetTypes()).Where(type =>
                            type.GetCustomAttributes(typeof(IoCInfraRegisterAttribute), true).Length > 0).ToList();

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
            /// <param name="life">Lifecycle of the instance</param>
            public static void Register<T1, T2>(IoCInfraLifeCycle life = IoCInfraLifeCycle.Singleton, params object[] objs)
                where T1 : class
                where T2 : class
            {
                SimpleContainer.Register<T1>(() => (T1)Activator.CreateInstance(typeof(T2), objs), ChooseLife(life));
            }
            /// <summary>
            /// Register a interface and implementation in container
            /// </summary>
            /// <typeparam name="T1">Interface</typeparam>
            /// <typeparam name="T2">Implementation</typeparam>
            /// <param name="life">Lifecycle of the instance</param>
            public static void Register<T1, T2>(IoCInfraLifeCycle life = IoCInfraLifeCycle.Singleton)
                where T1 : class
                where T2 : class
            {
                SimpleContainer.Register(typeof(T1), typeof(T2), ChooseLife(life));
            }
            /// <summary>
            /// Register a interface and implementation in container
            /// </summary>
            /// <param name="T1">Interface</param>
            /// <param name="T2">Implementation</param>
            /// <param name="life">Lifecycle of the instance</param>
            public static void Register(Type T1, Type T2, IoCInfraLifeCycle life = IoCInfraLifeCycle.Singleton)
            {
                SimpleContainer.Register(T1, T2, ChooseLife(life));
            }

            /// <summary>
            /// Get instance in container
            /// </summary>
            /// <typeparam name="T">Interface</typeparam>
            /// <returns>Instance of T</returns>
            public static T GetInstance<T>() where T : class
            {
                return SimpleContainer.GetInstance<T>();
            }
            /// <summary>
            /// Get instance in container
            /// </summary>
            /// <param name="T">Interface</param>
            /// <returns>Instance of T</returns>
            public static object GetInstance(Type T)
            {
                return SimpleContainer.GetInstance(T);
            }

            private static Lifestyle ChooseLife(IoCInfraLifeCycle life)
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