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
        /// Same instance every call inside of the scope of WebRequest. Disposes when EndRequest.
        /// </summary>
        WebRequest = 7,
    }
}
