using System.Collections.Generic;
using CI.Utils.Extentions;
using UnityEngine;

namespace Playbox
{
    public class CrossPromoButton : MonoBehaviour
    {
        [SerializeField] private string AfPromoteLink = "";
        
        [SerializeField] private string androidPromoteId = "";

        [SerializeField] private string iosPromoteId = "";

        [SerializeField] private string campaign = "";

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
            //Dictionary<string, string> properties = new();

            //properties.Add("campaign", campaign);
           // properties.Add("promoted_id", promotedID);
           //properties.Add("af_siteid", Application.identifier);
            //properties.Add("af_sub1", Application.identifier);

            //CrossPromo.RecordCrossPromoImpression(promotedID, campaign, properties);
        }

        public void Click()
        {
            CrossPromo.OpenStore(AfPromoteLink);
        }
        
    }
}