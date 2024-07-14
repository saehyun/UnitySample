using Sh.Base.Addressable;
using Sh.Common;
using UnityEngine;
using UnityEngine.UI;

public class AuthScene : MonoBehaviour
{
    [SerializeField]
    private Button moveToLobbyButton = null;


    private void Awake()
    {
        moveToLobbyButton.onClick.AddListener(() =>
        {
            AddressableSceneLoader.Instance.LoadSceneAsync(Const.Scene.LOBBY);
        });
    }
}
