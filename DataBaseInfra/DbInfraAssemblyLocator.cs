using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using CustomInfra.DataBase.Simple.Attribute;


namespace CustomInfra.DataBase.Simple
{
    static class DbInfraAssemblyLocator
    {
        private static ICollection<Assembly> localAssemblies { get; set; }

        /// <summary>
        /// Retorna uma lista com todas as dlls com o atributo 'DbInfraMapAttribute'
        /// </summary>
        /// <returns></returns>
        public static ICollection<Assembly> LoadDataBaseInfraAttributeAssemblies()
        {
            if (localAssemblies == null)
            {
                localAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                var config = (NameValueCollection)ConfigurationManager.GetSection("CustomInfra.DataBase.Simple");
                var startName = config["StartNameAssembly"] ?? string.Empty;
                if (!string.IsNullOrEmpty(startName))
                {
                    localAssemblies = localAssemblies.Where(x =>
                    x.FullName.StartsWith(startName)).ToArray();
                }

                localAssemblies = localAssemblies.ToArray();
            }

            return localAssemblies;
        }
    }
}
