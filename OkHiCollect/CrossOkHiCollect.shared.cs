using System;

namespace Plugin.OkHiCollect
{
    /// <summary>
    /// Cross OkHiCollect
    /// </summary>
    public static class CrossOkHiCollect
    {
        static Lazy<IOkHiCollect> implementation = new Lazy<IOkHiCollect>(() => CreateOkHiCollect(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IOkHiCollect Current
        {
            get
            {
                IOkHiCollect ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IOkHiCollect CreateOkHiCollect()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new OkHiCollectImplementation();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    }
}
