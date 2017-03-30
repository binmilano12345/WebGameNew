using AppConfig;
using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
    public static GameControl instance;
    [SerializeField]
    Transform tf_parent;

    public BaseCasino CurrentCasino { get; set; }
    public GameObject objPlayerTLMN { get; set; }
    public GameObject objPlayerSam { get; set; }
    public GameObject objPlayerPhom { get; set; }
    public GameObject objCard { get; set; }
    public GameObject objPlayer;

    [HideInInspector]
    public List<int> ListCMDID = new List<int>();
    [HideInInspector]
    public List<Message> ListMsg = new List<Message>();

    public List<ItemRankData> ListRank = new List<ItemRankData>();
    public List<ItemNotiData> ListNoti = new List<ItemNotiData>();
    public bool IsShowNoti = true;

    public int TimerTurnInGame = 0;
    void Awake() {
        instance = this;
        Application.runInBackground = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Init() {
        new ListernerServer();
        SendData.onGetPhoneCSKH();
        PopupAndLoadingScript.instance.LoadPopupAndLoading();
        if (objPlayerTLMN == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_TLMN, (obj) => {
                objPlayerTLMN = obj;
                objPlayerTLMN.transform.SetParent(tf_parent.transform);
                objPlayerTLMN.transform.localScale = Vector3.one;
                objPlayerTLMN.gameObject.SetActive(false);
            });
        }
        if (objPlayerSam == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_SAM, (obj) => {
                objPlayerSam = obj;
                objPlayerSam.transform.SetParent(tf_parent.transform);
                objPlayerSam.transform.localScale = Vector3.one;
                objPlayerSam.gameObject.SetActive(false);
            });
        }
        if (objPlayerPhom == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_PHOM, (obj) => {
                objPlayerPhom = obj;
                objPlayerPhom.transform.SetParent(tf_parent.transform);
                objPlayerPhom.transform.localScale = Vector3.one;
                objPlayerPhom.gameObject.SetActive(false);
            });
        }
        if (objCard == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CARD, (obj) => {
                objCard = obj;
                objCard.transform.SetParent(tf_parent.transform);
                objCard.transform.localScale = Vector3.one;
                objCard.gameObject.SetActive(false);
            });
        }
    }

	#region SMS, Call Phone
	public void SendSMS(string port, string content){
		#if UNITY_EDITOR
		PopupAndLoadingScript.instance.messageSytem.OnShow ("Soạn tin nhắn theo cú pháp: "
			+ content + " gửi tới " + port);
		#else
		Application.OpenURL("sms:" + port + "?body=" + content);
		#endif
	}
	public void CallPhone(string port){		
		#if UNITY_EDITOR
		Debug.LogError ("tel://" + port);
		#else
		Application.OpenURL("tel://" + port);
		#endif
	}
	#endregion

    #region Unload Sub Scene
    public void UnloadSubScene() {
        UnloadScene(SceneName.SUB_REGISTER);
        UnloadScene(SceneName.SUB_LOGIN);
		UnloadScene(SceneName.SUB_RANK);
		UnloadScene(SceneName.SUB_SETTING);
		UnloadScene(SceneName.SUB_HELP);
		UnloadScene(SceneName.SUB_INFO_PLAYER);
		UnloadScene(SceneName.SUB_INVITE);
		UnloadScene(SceneName.SUB_CHAT);
		UnloadScene(SceneName.SUB_NOTI);
    }
    #endregion
    #region Unload Game Scene
    public void UnloadGameScene() {
        UnloadScene(SceneName.GAME_TLMN);
        UnloadScene(SceneName.GAME_TLMN_SOLO);
        UnloadScene(SceneName.GAME_SAM);
        UnloadScene(SceneName.GAME_PHOM);
    }
    #endregion
    #region Unload Scene
    public void UnloadScene(string name) {
        if (SceneManager.GetSceneByName(name).isLoaded)
            SceneManager.UnloadSceneAsync(name);
    }
    #endregion

    void OnApplicationQuit() {
        if (ClientConfig.UserInfo.SAVE_PASS != 1) {
            ClientConfig.UserInfo.UNAME = "";
            ClientConfig.UserInfo.PASSWORD = "";
        }
        NetworkUtil.GI().close();
    }
    public void SetCasino(int type, UnityAction callback) {
        switch (GameConfig.CurrentGameID) {
            #region TLMN
            case GameID.TLMN:
                Card.setCardType(1);
                objPlayer = objPlayerTLMN;
                ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
                LoadAssetBundle.LoadScene(SceneName.GAME_TLMN, SceneName.GAME_TLMN, () => {
				TLMNControl.instace.UnloadSceneGame ();
                    CurrentCasino = (TLMNControl.instace);
                    try {
                        callback.Invoke();
					Debug.LogError ("so luong: " + ListMsg.Count);
                        for (int i = 0; i < ListMsg.Count; i++) {
                            ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
                        }
                        ListCMDID.Clear();
                        ListMsg.Clear();
                    } catch (Exception e) {
                        Debug.LogException(e);
                    }
                });
                break;
            #endregion
            #region TLMN SL
            case GameID.TLMNSL:
                Card.setCardType(1);
                objPlayer = objPlayerTLMN;
                ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
                LoadAssetBundle.LoadScene(SceneName.GAME_TLMN_SOLO, SceneName.GAME_TLMN_SOLO, () => {
                    CurrentCasino = (TLMNSoloControl.instace);
                    try {
                        callback.Invoke();
                        for (int i = 0; i < ListMsg.Count; i++) {
                            ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
                        }
                        ListCMDID.Clear();
                        ListMsg.Clear();
                    } catch (Exception e) {
                        Debug.LogException(e);
                    }
                });
                break;
            #endregion
            #region SAM
            case GameID.SAM:
                objPlayer = objPlayerSam;
                Card.setCardType(1);
                ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
                LoadAssetBundle.LoadScene(SceneName.GAME_SAM, SceneName.GAME_SAM, () => {
                    CurrentCasino = (SamControl.instace);
                    try {
                        callback.Invoke();
                        for (int i = 0; i < ListMsg.Count; i++) {
                            ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
                        }
                        ListCMDID.Clear();
                        ListMsg.Clear();
                    } catch (Exception e) {
                        Debug.LogException(e);
                    }
                });
                break;
            #endregion
            #region PHOM
            case GameID.PHOM:
                Card.setCardType(0);
                objPlayer = objPlayerPhom;
                ProcessHandler.setSecondHandler(PHandler.getInstance());
                LoadAssetBundle.LoadScene(SceneName.GAME_PHOM, SceneName.GAME_PHOM, () => {
                    CurrentCasino = (PhomControl.instace);
                    try {
                        callback.Invoke();
                        for (int i = 0; i < ListMsg.Count; i++) {
                            ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
                        }
                        ListCMDID.Clear();
                        ListMsg.Clear();
                    } catch (Exception e) {
                        Debug.LogException(e);
                    }
                });
                break;
                #endregion
			#region MAU BINH
		case GameID.MAUBINH:
			Card.setCardType(0);
			objPlayer = objPlayerPhom;
			ProcessHandler.setSecondHandler(PHandler.getInstance());
			LoadAssetBundle.LoadScene(SceneName.GAME_MAUBINH, SceneName.GAME_MAUBINH, () => {
				CurrentCasino = (PhomControl.instace);
				try {
					callback.Invoke();
					for (int i = 0; i < ListMsg.Count; i++) {
						ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
					}
					ListCMDID.Clear();
					ListMsg.Clear();
				} catch (Exception e) {
					Debug.LogException(e);
				}
			});
			break;
			#endregion
        }
//        InitCardType();
    }

//    private void InitCardType() {
//        switch (gameID) {
//		case GameID.TLMN:
//		case GameID.TLMNSL:
//		case GameID.MAUBINH:
//		case GameID.SAM:
//		case GameID.POKER:
//                Card.setCardType(1);
//                break;
//		case GameID.BACAY:
//		case GameID.PHOM:
//		case GameID.XITO:
//		case GameID.LIENG:
//                Card.setCardType(0);
//                break;
//        }
//    }
}
