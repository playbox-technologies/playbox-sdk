using System.Collections.Generic;
using UnityEngine;
using Utils.Tools.Extentions;

namespace Playbox
{
    public class CrossPromoButton : MonoBehaviour
    {
        [SerializeField] private string AfPromoteLink = "";

        private void OnEnable()
        {
          

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