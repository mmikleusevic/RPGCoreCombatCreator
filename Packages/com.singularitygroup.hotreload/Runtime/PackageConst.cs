#if ENABLE_MONO && (DEVELOPMENT_BUILD || UNITY_EDITOR)
namespace SingularityGroup.HotReload
{
    internal static class PackageConst
    {
        //CI changes this property to 'true' for asset store builds.
        //Don't touch unless you know what you are doing
        public static bool IsAssetStoreBuild => true;


        public const string Version = "1.12.9";
        // Never higher than Version
        // Used for the download
        public const string ServerVersion = "1.12.9";
        public const string PackageName = "com.singularitygroup.hotreload";
        public const string LibraryCachePath = "Library/" + PackageName;
        public const string ConfigFileName = "hot-reload-config.json";
    }
}
#endif
