using CustomInfra.Injector.Simple.AspNet;
using CustomInfra.Injector.Simple.Enums;
using SimpleInjector;
using System;

namespace CustomInfra.Injector.Simple.IoC
{
    internal class WebRequestLifestyle : ScopedLifestyle
    {
        public WebRequestLifestyle() : base("Web Request")
        {
        }

        /// <summary>
        /// Implementation for this method to get the scope from the current request
        /// </summary>
        /// <param name="container">Simple container</param>
        /// <returns></returns>
        protected override Func<Scope> CreateCurrentScopeProvider(Container container)
        {
            return () => { return GetCurrentScopeCore(container); };
        }

        /// <summary>
        /// Overrided for this method to get the scope from the current request
        /// </summary>
        /// <param name="container">Simple container</param>
        /// <returns></returns>
        protected override Scope GetCurrentScopeCore(Container container)
        {
            return WebRequestModule.GetScopeFromHttpContext();
        }

        /// <summary>
        /// Creates a new Scope for the container
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        internal static Scope BeginScope(Container container)
        {
            return new Scope(container);
        }
    }
}