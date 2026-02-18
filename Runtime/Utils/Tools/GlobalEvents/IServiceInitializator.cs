using Utils.Tools.EventBus;

namespace Utils.Tools.GlobalEvents
{
    public interface IServiceInitializator: IGlobalSubscriber
    {
        public void Initialize();
    }
}