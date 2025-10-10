using System;
using System.Collections.Generic;
using AppsFlyerSDK;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Playbox
{
    public static class CrossPromo
    {
        private static InviteLinkGenerator inviteLinkGenerator;

        public static InviteLinkGenerator InviteLinkGenerator
        {
            get
            {
                return inviteLinkGenerator;
            }
        }


        public static Action<string> OnInviteLinkGenerated;
        public static Action<string> OnOpenStoreLinkGenerated;
        public static Action<string> OnConversionDataSucces;
        public static Action<string> OnAppOpenAttribution;


        public static void Initialize()
        {
            if (inviteLinkGenerator == null)
                inviteLinkGenerator = UnityEngine.Object.FindFirstObjectByType<InviteLinkGenerator>();
        }

        /// <summary>
        /// Recorm Cross Promo
        /// </summary>
        /// <param name="promotedID">id of the promoted application from AppsFlyer</param>
        /// <param name="campaign"></param>
        /// <param name="parameters"></param>
        public static void RecordCrossPromoImpression(string promotedID,string campaign, Dictionary<string, string> parameters)
        {
            if (Analytics.isAppsFlyerInit) AppsFlyer.recordCrossPromoteImpression(promotedID,campaign,parameters);
        }
        
        public static void GenerateUserInviteLink(Dictionary<string, string> parameters)
        {
            if(inviteLinkGenerator == null)
                return;
            
            if (Analytics.isAppsFlyerInit) AppsFlyer.generateUserInviteLink(parameters,inviteLinkGenerator);
        }

        public static void OpenStore(string af_link,string promotedID, string campaign, Dictionary<string, string> parameters,
            MonoBehaviour monoBehaviour)
        {
            if (Analytics.isAppsFlyerInit)
            {
                AppsFlyerConfiguration.LoadJsonConfig();
          
                AppsFlyer.attributeAndOpenStore(promotedID, campaign, parameters, monoBehaviour);
                
                Application.OpenURL(af_link);
            }
        }
    }
}