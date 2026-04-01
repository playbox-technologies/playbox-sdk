using System;

namespace Playbox
{
    public class RewardedAdEvents
    {
        public Action<string> OnClicked;
        public Action<string,MaxSdkBase.Reward> OnReceivedReward;
        public Action<string> OnDisplayed;
        public Action<string> OnDisplayedFailed;
        public Action<string> OnAdClosed;
    }
}