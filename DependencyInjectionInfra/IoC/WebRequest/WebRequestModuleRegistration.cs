using CustomInfra.Injector.Simple.AspNet;
using System.Web;

//Put an attribute on the assemby to call 'Run' within 'WebRequestModuleRegistration' class
//Both class and method must be public and static
[assembly: PreApplicationStartMethod(typeof (WebRequestModuleRegistration), "Run")]


namespace CustomInfra.Injector.Simple.AspNet
{
    /// <summary>
    /// WebRequest Module Registrator 
    /// </summary>
    //
    public static class WebRequestModuleRegistration
    {
        /// <summary>
        /// Register the custom module in the HttpApplication
        /// </summary>
        public static void Run()
        {
            HttpApplication.RegisterModule(typeof(WebRequestModule));
        }
    }
}
