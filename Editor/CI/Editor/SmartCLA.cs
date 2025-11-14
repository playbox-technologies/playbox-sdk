namespace Playbox.CI
{
    /// <summary>
    /// Provides static access to build arguments, validations, and constants for Playbox CI (Continuous Integration).
    /// </summary>
    public static class SmartCLA
    {
        /// <summary>
        /// Contains properties to retrieve command-line argument values for the build.
        /// </summary>
        public static class Arguments
        {
            /// <summary>Gets the build location path argument.</summary>
            public static string BuildLocation => SmartEnviroment.GetArgumentValue(Commands.BUILD_LOCATION);
            /// <summary>Gets the build version argument (e.g., "1.0.0").</summary>
            public static string BuildVersion => SmartEnviroment.GetArgumentValue(Commands.BUILD_VERSION);
            /// <summary>Alias for BuildVersion (used for bundle version).</summary>
            public static string BundleVersion => BuildVersion;
            /// <summary>Gets the build number argument (e.g., Jenkins build ID).</summary>
            public static int BuildNumber => SmartEnviroment.GetArgumentIntValue(Commands.BUILD_NUMBER);
            /// <summary>Gets the keystore password for signing.</summary>
            public static string KeystorePass => SmartEnviroment.GetArgumentValue(Commands.KEYSTORE_PASS);
            /// <summary>Gets the key alias name for signing.</summary>
            public static string KeyaliasName => SmartEnviroment.GetArgumentValue(Commands.KEYALIAS_NAME);
            /// <summary>Gets the key alias password for signing.</summary>
            public static string KeyaliasPass => SmartEnviroment.GetArgumentValue(Commands.KEYALIAS_PASS);
            /// <summary>Gets the keystore path for signing.</summary>
            public static string KeystorePath => SmartEnviroment.GetArgumentValue(Commands.KEYSTORE_PATH);
            /// <summary>Gets the iOS provisioning profile argument.</summary>
            public static string ProvisionProfileIos => SmartEnviroment.GetArgumentValue(Commands.PROVISION_PROFILE_IOS_SIGN);
            /// <summary>Gets the iOS code signing identity argument.</summary>
            public static string CodeSignIdentity => SmartEnviroment.GetArgumentValue(Commands.CODE_SIGN_IDENTITY);
            
            public static string TeamID => SmartEnviroment.GetArgumentValue(Commands.TEAM_ID);
        }

        /// <summary>
        /// Contains properties to check the presence of specific build arguments.
        /// </summary>
        public static class Validations
        {
            /// <summary>Returns true if development mode argument is present.</summary>
            public static bool HasDevelopmentMode => SmartEnviroment.HasArgument(Commands.DEVELOPMENT_MODE);
            /// <summary>Returns true if development mode argument is present.</summary>
            public static bool HasDebugMode => SmartEnviroment.HasArgument(Commands.DEBUG_MODE);
            /// <summary>Returns true if build location argument is present.</summary>
            public static bool HasBuildLocation => SmartEnviroment.HasArgument(Commands.BUILD_LOCATION);
            /// <summary>Returns true if splash screen argument is present.</summary>
            public static bool HasSplashScreen => SmartEnviroment.HasArgument(Commands.SPLASH_SCREEN);
            /// <summary>Returns true if build version argument is present.</summary>
            public static bool HasBuildVersion => SmartEnviroment.HasArgument(Commands.BUILD_VERSION);
            /// <summary>Returns true if build number argument is present.</summary>
            public static bool HasBuildNumber => SmartEnviroment.HasArgument(Commands.BUILD_NUMBER);
            /// <summary>Returns true if keystore password argument is present.</summary>
            public static bool HasKeystorePass => SmartEnviroment.HasArgument(Commands.KEYSTORE_PASS);
            /// <summary>Returns true if key alias name argument is present.</summary>
            public static bool HasKeyaliasName => SmartEnviroment.HasArgument(Commands.KEYALIAS_NAME);
            /// <summary>Returns true if key alias password argument is present.</summary>
            public static bool HasKeyaliasPass => SmartEnviroment.HasArgument(Commands.KEYALIAS_PASS);
            /// <summary>Returns true if keystore path argument is present.</summary>
            public static bool HasKeystorePath => SmartEnviroment.HasArgument(Commands.KEYSTORE_PATH);
            /// <summary>Returns true if store build argument is present.</summary>
            public static bool HasStoreBuild => SmartEnviroment.HasArgument(Commands.STORE_BUILD);
            /// <summary>Returns true if iOS manual sign argument is present.</summary>
            public static bool HasIosManualSign => SmartEnviroment.HasArgument(Commands.MANAUL_SIGN);
            /// <summary>Returns true if iOS provisioning profile argument is present.</summary>
            public static bool HasProvisionProfileIos => SmartEnviroment.HasArgument(Commands.PROVISION_PROFILE_IOS_SIGN);
            /// <summary>Returns true if iOS code signing identity argument is present.</summary>
            public static bool HasCodeSignIdentity => SmartEnviroment.HasArgument(Commands.CODE_SIGN_IDENTITY);
            /// <summary>Returns true if iOS profile development argument is present.</summary>
            public static bool HasProfileDevelopment => SmartEnviroment.HasArgument(Commands.PROFILE_DEVELOPMENT);
            /// <summary>Returns true if iOS profile distribution argument is present.</summary>
            public static bool HasProfileDistribution => SmartEnviroment.HasArgument(Commands.PROFILE_DISTRIBUTION);
            
            public static bool HasTeamID => SmartEnviroment.HasArgument(Commands.TEAM_ID);
        }

        /// <summary>
        /// Contains constant argument names used for CI/CD command-line parameters.
        /// </summary>
        public static class Commands
        {
            public const string DEVELOPMENT_MODE = "-dev"; // Enable development mode
            public const string DEBUG_MODE = "-debug"; // Enable debug mode
            public const string BUILD_LOCATION = "-build-location"; // Build output location
            public const string SPLASH_SCREEN = "-splash-screen"; // Show splash screen
            public const string BUILD_VERSION = "-build-version"; // App version (e.g., "0.0.1")
            public const string BUILD_NUMBER = "-build-number"; // CI build number (e.g., Jenkins ID)
            public const string KEYSTORE_PASS = "-keystorepass"; // Keystore password
            public const string KEYALIAS_NAME = "-keyaliasname"; // Key alias name
            public const string KEYALIAS_PASS = "-keyaliaspass"; // Key alias password
            public const string KEYSTORE_PATH = "-keystore-path"; // Path to keystore
            public const string STORE_BUILD = "-store-build"; // Flag for store build
            public const string MANAUL_SIGN = "-code-sign-manual"; // Manual code signing for iOS
            public const string PROVISION_PROFILE_IOS_SIGN = "-provision-profile"; // iOS provisioning profile
            public const string CODE_SIGN_IDENTITY = "-code-sign-identity"; // iOS code sign identity
            public const string PROFILE_DEVELOPMENT = "-profile-development"; // Development provisioning profile
            public const string PROFILE_DISTRIBUTION = "-profile-distribution"; // Distribution provisioning profile
            public const string TEAM_ID = "-team-id"; // teamId for publishing
        }
    }
}
