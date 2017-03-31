using AppConfig;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class LobbyControl : MonoBehaviour {
    public static LobbyControl instance;
    [SerializeField]
    Transform tf_parent;
    [SerializeField]
    Text txt_name, txt_id, txt_money, txt_noti;
    [SerializeField]
    RawImage raw_avata;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        yield return new WaitForEndOfFrame();
        GameControl.instance.UnloadScene(SceneName.SCENE_MAIN);
        GameControl.instance.UnloadScene(SceneName.SCENE_ROOM);
        GameControl.instance.UnloadSubScene();
        GameControl.instance.UnloadGameScene();
        if (GameControl.instance.ListNoti.Count > 0 && GameControl.instance.IsShowNoti) {
            LoadAssetBundle.LoadScene(SceneName.SUB_NOTI, SceneName.SUB_NOTI);
            GameControl.instance.IsShowNoti = false;
        }
        PopupAndLoadingScript.instance.ShowTaiXiu();
        InitIconGame();
        SetInfo();
    }

    void SetInfo() {
        txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
		SetAvatar ();
		SetDisplayName ();
		SetMoney ();
        txt_noti.text = GameConfig.TXT_NOTI;
        txt_noti.transform.localPosition = new Vector3(600, 0, 0);
        float w = LayoutUtility.GetPreferredWidth(txt_noti.rectTransform);
        float time = (1200 + w) / 100;
        txt_noti.transform.DOLocalMoveX(-600 - w, time).SetLoops(-1).SetEase(Ease.Linear);
    }
	public void SetAvatar(){
		LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, ClientConfig.UserInfo.AVATAR_ID + "");
	}
	public void SetDisplayName(){
		txt_name.text = ClientConfig.UserInfo.DISPLAY_NAME;
	}
	public void SetMoney(){
		txt_money.text = MoneyHelper.FormatAbsoluteWithoutUnit(ClientConfig.UserInfo.CASH_FREE);
	}

    public void SetNoti() {
        txt_noti.transform.DOKill();
        txt_noti.text = GameConfig.TXT_NOTI;
        txt_noti.transform.localPosition = new Vector3(600, 0, 0);
        float w = LayoutUtility.GetPreferredWidth(txt_noti.rectTransform);
        float time = (1200 + w) / 100;
        txt_noti.transform.DOLocalMoveX(-600 - w, time).SetLoops(-1).SetEase(Ease.Linear);
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
            listGame[i].transform.DOScale(1, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    void OnClickGame(GameObject obj) {
        int index = int.Parse(obj.name);

        SendData.onSendGameID((byte)GameConfig.IdGame[index]);
        PopupAndLoadingScript.instance.ShowLoading();
    }
    #region Click
    public void OnClickBack() {
        PopupAndLoadingScript.instance.messageSytem.OnShow(ClientConfig.Language.GetText("popup_quitgame_thoatgame"), delegate {
            NetworkUtil.GI().close();
            LoadAssetBundle.LoadScene(SceneName.SCENE_MAIN, SceneName.SCENE_MAIN);
        });
    }
    public void OnClickRank() {
        LoadAssetBundle.LoadScene(SceneName.SUB_RANK, SceneName.SUB_RANK);
    }
    public void OnClickNap() {
        //LoadAssetBundle.LoadScene(SceneName.SUB_RANK, SceneName.SUB_RANK);
    }
    public void OnClickMail() {
        //LoadAssetBundle.LoadScene(SceneName.SUB_RANK, SceneName.SUB_RANK);
    }
    public void OnClickHelp() {
        LoadAssetBundle.LoadScene(SceneName.SUB_HELP, SceneName.SUB_HELP);
    }
    public void OnClickMenu() {
        //LoadAssetBundle.LoadScene(SceneName.SUB_RANK, SceneName.SUB_RANK);
    }

	public void OnClickInfoPlayer() {
		LoadAssetBundle.LoadScene(SceneName.SUB_INFO_PLAYER, SceneName.SUB_INFO_PLAYER);
	}
    #endregion
}
