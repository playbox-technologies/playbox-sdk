using System.Collections.Generic;
using CI.Utils.Extentions;
using UnityEngine;

namespace Playbox
{
    public class CrossPromoButton : MonoBehaviour
    {
        [SerializeField] private string androidPromoteId = "";

        [SerializeField] private string androidPromoteLink = "";

        [SerializeField] private string iosPromoteId = "";

        [SerializeField] private string iosPromoteLink = "";

        private string promotedID =>

#if UNITY_IOS

            iosPromoteId

#elif UNITY_ANDROID

        androidPromoteId

#endif
        ;

        [SerializeField] private string campaign = "";

        private string Af_Link(string ios_link, string android_link)
        {
#if UNITY_IOS
            return ios_link;
#endif
#if UNITY_ANDROID
            return android_link;
#endif

            return android_link;
        }

        private void OnEnable()
        {
            CrossPromo.OnInviteLinkGenerated += s =>
            {
                s.PlayboxInfo("LINK");
                s.PlayboxSplashLogUGUI();
            };
            CrossPromo.OnOpenStoreLinkGenerated += s =>
            {
                s.PlayboxInfo("LINK");
                s.PlayboxSplashLogUGUI();
                Application.OpenURL(s);
            };

            RecordCrossPromoImpression();
        }

        public void RecordCrossPromoImpression()
        {
            Dictionary<string, string> properties = new();

            properties.Add("campaign", campaign);
            properties.Add("promoted_id", promotedID);
            properties.Add("af_siteid", Application.identifier);
            properties.Add("af_sub1", Application.identifier);

            CrossPromo.RecordCrossPromoImpression(promotedID, campaign, properties);
        }

        public void Click()
        {
            Dictionary<string, string> properties = new();

            properties.Add("campaign", campaign);
            properties.Add("promoted_id", promotedID);
            properties.Add("af_siteid", Application.identifier);
            properties.Add("af_sub1", Application.identifier);

            CrossPromo.OpenStore(Af_Link(iosPromoteLink, androidPromoteLink), promotedID, campaign, properties, this);
        }

        public void GenerateLink()
        {
            Dictionary<string, string> properties = new();

            properties.Add("campaign", campaign);
            properties.Add("promoted_id", promotedID);
            properties.Add("af_siteid", Application.identifier);
            properties.Add("af_sub1", Application.identifier);

            CrossPromo.GenerateUserInviteLink(properties);
        }
    }
}