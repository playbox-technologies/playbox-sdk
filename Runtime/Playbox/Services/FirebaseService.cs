using Firebase;
using Firebase.Crashlytics;
using Firebase.Extensions;
using UnityEngine;

namespace Playbox.Services
{
    public class FirebaseService : PlayboxService
    {
        public override void Initialization()
        {
            base.Initialization();
            
            ServiceType = ServiceType.Firebase;
            
            InitializeCrashlytics();
 
        }

        public void InitializeCrashlytics()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Init();
                }
                else
                {
                    Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
                }
            });
        }

        private void Init()
        {
            Crashlytics.ReportUncaughtExceptionsAsFatal = true; 
            Crashlytics.IsCrashlyticsCollectionEnabled = true;
            
            ApproveInitialization();
            
        }
    }
}
