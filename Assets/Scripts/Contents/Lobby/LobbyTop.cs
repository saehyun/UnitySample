using Sh.Base.Addressable;
using Sh.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Sh.Contents.Lobby
{
    public class LobbyTop : MonoBehaviour
    {
        [SerializeField]
        private Button backButton = null;


        private void Awake()
        {
            backButton.onClick.AddListener(() =>
            {
                AddressableSceneLoader.Instance.LoadSceneAsync(Const.Scene.AUTH);
            });
        }
    }
}