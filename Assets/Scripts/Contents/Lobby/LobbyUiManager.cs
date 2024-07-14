using Sh.Base.Resources;
using Sh.Common;
using Sh.Contents.Common;
using Sh.Contents.Lobby.Interface;
using Sh.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Sh.Contents.Lobby
{
    public class LobbyUiManager : MonoBehaviour
    {
        [SerializeField]
        private Transform tabRoot = null;

        [SerializeField]
        private Transform tabContentRoot = null;

        [SerializeField]
        private Button btnPrev = null;

        [SerializeField]
        private Button btnNext = null;

        private TabType currentTab = TabType.None;

        private readonly List<ITab> receivers = new();


        private void Awake()
        {
            AddButtonClickListeners();
            SetupAsync();
        }


        private void AddButtonClickListeners()
        {
            btnPrev.onClick.AddListener(() => OnNavigationButtonClick(isNext: false));
            btnNext.onClick.AddListener(() => OnNavigationButtonClick(isNext: true));
        }


        private void OnNavigationButtonClick(bool isNext)
        {
            var isDirty = false;

            if (isNext)
            {
                if (currentTab < TabType.Last)
                {
                    currentTab++;
                    isDirty = true;
                }   
            }
            else
            {
                currentTab--;

                if (currentTab == TabType.None)
                    currentTab = TabType.First;
                else
                    isDirty = true;
            }

            if (isDirty)
                Notify();
        }


        private async void SetupAsync()
        {
            await RequestTabDataAsync();
        }


        private async Task RequestTabDataAsync()
        {
            var response = await NetworkManager.Instance.LobbyTabDataAsync(new Network.Protocol.ReqLobbyTabData());

            if (response.tabData.Length == 0)
                throw new Exception("tabData is null");

            currentTab = response.tabData.First().TabType;

            foreach (var tabData in response.tabData)
            {
                SetupTab(new TabModel { Manager = this, TabType = tabData.TabType, Text = tabData.TabText });
                SetupTabContent(new TabModel { Manager = this, TabType = tabData.TabType, Text = tabData.ContentText });
            }

            RefreshButtonUi();
        }


        private void SetupTab(TabModel model)
        {
            var instance = ResourceManager.Instance.Instantiate<Tab>(Const.Prefab.LOBBY_TAB, tabRoot);
            var tab = instance as ITab;

            tab.Inject(model);
            Subscribe(tab);
        }


        private void SetupTabContent(TabModel model)
        {
            var instance = ResourceManager.Instance.Instantiate<TabContent>(Const.Prefab.LOBBY_TABCONTENT, tabContentRoot);
            var rectTransform = instance.transform as RectTransform;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            var tab = instance as ITab;

            tab.Inject(model);
            Subscribe(tab);
        }


        internal void SetTab(TabType tab)
        {
            currentTab = tab;
            Notify();
        }


        private void Subscribe(ITab receiver)
        {
            receivers.Add(receiver);
            receiver.UpdateTab(currentTab);
        }


        internal void Unsubscribe(ITab receiver)
        {
            if (!receivers.Contains(receiver))
                return;

            receivers.Remove(receiver);
        }


        private void Notify()
        {
            receivers.ForEach(receiver => receiver.UpdateTab(currentTab));
            RefreshButtonUi();
        }


        private void RefreshButtonUi()
        {
            btnPrev.gameObject.SetActive(currentTab > TabType.First);
            btnNext.gameObject.SetActive(currentTab < TabType.Last);
        }
    }
}
