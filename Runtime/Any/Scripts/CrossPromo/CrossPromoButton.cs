using System.Collections.Generic;
using CI.Utils.Extentions;
using UnityEngine;

namespace Playbox
{
    public class CrossPromoButton: MonoBehaviour
    {
        //[SerializeField]
        //private string Link = "";
        
        [SerializeField]
        private string androidPromoteId = "";
        
        [SerializeField]
        private string iosPromoteId = "";

        private string promotedID =>

#if UNITY_IOS

            iosPromoteId

#elif UNITY_ANDROID

        androidPromoteId

#endif
        ;
        
        [SerializeField]
        private string campaign = "";
        
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
        }

        public void RecordCrossPromoImpression()
        {
            Dictionary<string,string> properties = new ();
            
            properties.Add("campaign", campaign);
            properties.Add("promoted_id", promotedID);
            properties.Add("af_siteid", Application.identifier);
            
            CrossPromo.RecordCrossPromoImpression(promotedID, campaign, properties);
        }

        public void Click()
        {
            Dictionary<string,string> properties = new ();
            
            properties.Add("campaign", campaign);
            properties.Add("promoted_id", promotedID);
            properties.Add("af_siteid", Application.identifier);
            
            CrossPromo.OpenStore(promotedID,campaign, properties,this);
        }

        public void GenerateLink()
        {
            Dictionary<string,string> properties = new ();
            
            properties.Add("campaign", campaign);
            properties.Add("promoted_id", promotedID);
            properties.Add("af_siteid", Application.identifier);
            
            CrossPromo.GenerateUserInviteLink(properties);
        }
    }
}