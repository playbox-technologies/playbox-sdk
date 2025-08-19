using System;

namespace Playbox
{
    [Obsolete("The sandbox flag is obsolete; everything is automatically determined on the server.")]
    public static class InAppVerificationCongifuration
    {
        private static bool isSandbox = true;

        public static bool IsSandbox
        {
            get => isSandbox;
            set => isSandbox = value;
        }
    }
}