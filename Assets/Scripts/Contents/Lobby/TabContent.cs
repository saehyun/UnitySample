using Sh.Contents.Common;
using Sh.Contents.Lobby.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Sh.Contents.Lobby
{
    public class TabContent : MonoBehaviour, ITab
    {
        private LobbyUiManager manager = null;

        [SerializeField]
        private TabType tabType = TabType.None;

        [SerializeField]
        private Text text = null;


        void ITab.Inject(TabModel model)
        {
            manager = model.Manager;
            tabType = model.TabType;
            text.text = model.Text;
        }


        void ITab.UpdateTab(TabType tabType)
        {
            gameObject.SetActive(tabType == this.tabType);
        }


        private void OnDestroy()
        {
            manager?.Unsubscribe(this);
        }
    }
}