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

        
        protected override Func<Scope> CreateCurrentScopeProvider(Container container)
        {
            return () => { return WebRequestModule.GetScopeInContext(); };
        }

        protected override Scope GetCurrentScopeCore(Container container)
        {
            return WebRequestModule.GetScopeInContext();
        }

        internal static Scope BeginScope(Container container)
        {
            return new Scope(container);
        }
    }
}