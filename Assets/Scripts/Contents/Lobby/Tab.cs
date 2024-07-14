using Sh.Contents.Common;
using Sh.Contents.Lobby.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Sh.Contents.Lobby
{
    public class Tab : MonoBehaviour, ITab
    {
        private LobbyUiManager manager = null;

        [SerializeField]
        private TabType tabType = TabType.None;

        [SerializeField]
        private GameObject normalBg = null;

        [SerializeField]
        private GameObject selectedBg = null;

        [SerializeField]
        private Text text = null;

        [SerializeField]
        private Button button = null;


        void ITab.Inject(TabModel model)
        {
            manager = model.Manager;
            tabType = model.TabType;
            text.text = model.Text;
        }


        void ITab.UpdateTab(TabType tabType)
        {
            normalBg.SetActive(tabType != this.tabType);
            selectedBg.SetActive(tabType == this.tabType);
        }


        private void Awake()
        {
            button.onClick.AddListener(OnTabClick);
        }


        private void OnDestroy()
        {
            manager?.Unsubscribe(this);
        }


        private void OnTabClick()
        {
            manager.SetTab(tabType);
        }
    }
}
