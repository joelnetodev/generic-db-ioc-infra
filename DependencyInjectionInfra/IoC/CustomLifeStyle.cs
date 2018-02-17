
using CustomInfra.Injector.Simple.AspNet;
using CustomInfra.Injector.Simple.IoC.WebRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomInfra.Injector.Simple.IoC
{
    internal class CustomLifeStyle
    {
        public static readonly WebRequestLifestyle WebRequest = new WebRequestLifestyle();
    }
}
