using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomInfra.Injector.Simple.Enums
{
    /// <summary>
    /// Enum of types of LifeCycle to register
    /// </summary>
    public enum IoCInfraLifeCycle
    {
        /// <summary>
        /// New instance every call.
        /// </summary>
        New = 1,

        /// <summary>
        /// Same instance every call.
        /// </summary>
        Singleton = 3,

        /// <summary>
        /// Same instance every call inside a scope. Disposed when scope ends.
        /// </summary>
        Scoped = 7
    }
}
