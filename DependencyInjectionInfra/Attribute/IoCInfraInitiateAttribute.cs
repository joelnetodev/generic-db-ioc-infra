using System;

namespace CustomInfra.Injector.Simple.Attribute
{
    /// <summary>
    /// Attribute that register interface and its implementation when 'IniciateAttributeRegistration' method is called
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class IoCInfraInitiateAttribute : System.Attribute
    {

    }
}
