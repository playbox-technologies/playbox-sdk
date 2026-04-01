using Utils.Tools.EventBus;

namespace Playbox.Tools.GlobalEvents
{
    public interface IServiceInitializator: IGlobalSubscriber
    {
        public void Initialize();
    }
}