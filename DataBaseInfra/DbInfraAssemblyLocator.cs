using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using CustomInfra.DataBase.Simple.Attribute;


namespace CustomInfra.DataBase.Simple
{
    internal static class DbInfraAssemblyLocator
    {
        private static ICollection<Assembly> _localAssemblies { get; set; }

        /// <summary>
        /// Loads the assemblies in StartNameAssemblies configuration from CurrentAppDomain
        /// </summary>
        /// <returns></returns>
        public static ICollection<Assembly> LoadDataBaseInfraAttributeAssemblies()
        {
            if (_localAssemblies == null)
            {
                _localAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                var config = (NameValueCollection)ConfigurationManager.GetSection("CustomInfra.DataBase.Simple");
                var startName = config["StartNameAssemblies"] ?? string.Empty;
                if (!string.IsNullOrEmpty(startName))
                {
                    var namesSplited = startName.Split(',');

                    _localAssemblies = _localAssemblies.Where(
                        (x) => {
                            bool retorno = false;

                            foreach (var itemName in namesSplited)
                                if (x.FullName.StartsWith(itemName))
                                    retorno = true;

                            return retorno;
                        }).ToArray();
                }
            }

            return _localAssemblies;
        }
    }
}
