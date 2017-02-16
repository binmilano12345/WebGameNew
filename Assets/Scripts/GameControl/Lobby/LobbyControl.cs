using AppConfig;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class LobbyControl : MonoBehaviour {
    [SerializeField]
    Transform tf_parent;
    [SerializeField]
    Text txt_name, txt_id, txt_money;
    [SerializeField]
    RawImage raw_avata;
    // Use this for initialization
    void Start() {
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        yield return new WaitForEndOfFrame();
        InitIconGame();
        SetInfo();
    }

    void SetInfo() {
        txt_name.text = ClientConfig.UserInfo.UNAME;
        txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
        txt_money.text = MoneyHelper.FormatAbsoluteWithoutUnit(ClientConfig.UserInfo.CASH_FREE);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, ClientConfig.UserInfo.AVATAR_ID + "");
    }
    List<GameObject> listGame = new List<GameObject>();
    void InitIconGame() {
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_GAME, (obj) => {
            for (int i = 0; i < GameConfig.NUM_GAME; i++) {
                GameObject itemGame = Instantiate(obj);
                itemGame.transform.SetParent(tf_parent);
                itemGame.transform.localScale = Vector3.zero;
                itemGame.name = i + "";
                itemGame.GetComponent<UIButton>()._onClick.AddListener(delegate {
                    OnClickGame(itemGame);
                });
                LoadAssetBundle.LoadSprite(itemGame.GetComponent<Image>(), BundleName.ICON_GAME, UIName.UI_GAME[i]);
                listGame.Add(itemGame);
            }

            StartCoroutine(RunEffectGame());
        });
    }
    IEnumerator RunEffectGame() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < listGame.Count; i++) {
            listGame[i].transform.DOScale(1, 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void OnClickGame(GameObject obj) {
        Debug.LogError(obj.name);
        int gameID = int.Parse(obj.name);
        SendData.onSendGameID((byte)gameID);
    }

    public void OnClickBack() {
        PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("popup_quitgame_thoatgame"), delegate {
            LoadAssetBundle.LoadScene(SceneName.SCENE_MAIN, SceneName.SCENE_MAIN);
        });
    }
}
