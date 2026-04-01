using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace Playbox.Consent
{
    public static class GoogleUmpManager
    {
        public static void RequestConsentInfo()
        {
            ConsentRequestParameters requestParameters = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false
            };
            
            ConsentInformation.Update(requestParameters, (error) =>
                {
                    if (error != null)
                    {
                        ConsentData.ConsentDeny();
                        return;
                    }
                    ConsentForm.LoadAndShowConsentFormIfRequired((err) =>
                    {
                        if (error != null)
                        {
                            ConsentData.ConsentDeny();
                            return;
                        }

                        if (ConsentInformation.CanRequestAds())
                        {
                            ConsentData.ConsentAllow();
                        }
                        else
                        {
                            ConsentData.ConsentDeny();
                        }
                    });
                });
        }
        
        public static void RequestConsentInfoDebug(ConsentDebugSettings consentDebugSettings)
        {
            ConsentRequestParameters requestParameters = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings =  consentDebugSettings
            };
            
            ConsentInformation.Update(requestParameters, (error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Consent form error: " + error.Message);
                    
                    ConsentData.ConsentDeny();
                    return;
                }
                    
                ConsentForm.LoadAndShowConsentFormIfRequired((err) =>
                {
                    if (error != null)
                    {
                        ConsentData.ConsentDeny();
                        return;
                    }
                    if (ConsentInformation.CanRequestAds())
                    {
                        ConsentData.ConsentAllow();
                    }
                    else
                    {
                        ConsentData.ConsentDeny();
                    }
                });
            });
        }
    }
}