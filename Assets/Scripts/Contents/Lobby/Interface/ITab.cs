using Sh.Contents.Common;

namespace Sh.Contents.Lobby.Interface
{
    interface ITab
    {
        void Inject(TabModel model);

        void UpdateTab(TabType tabType);
    }
}
