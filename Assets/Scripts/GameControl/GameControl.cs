using AppConfig;
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

    BaseCasino currentCasino;
    public GameObject objPlayer, objCard;

    public List<int> ListCMDID = new List<int>();
    public List<Message> ListMsg = new List<Message>();
    void Awake() {
        instance = this;
        Application.runInBackground = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Init() {
        new ListernerServer();
        SendData.onGetPhoneCSKH();
        PopupAndLoadingScript.instance.LoadPopupAndLoading();
        if (objPlayer == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER, (obj) => {
                objPlayer = obj;
                objPlayer.transform.SetParent(tf_parent.transform);
                objPlayer.transform.localScale = Vector3.one;
                objPlayer.gameObject.SetActive(false);
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

    #region Unload Sub Scene
    public void UnloadSubScene() {
        UnloadScene(SceneName.SUB_REGISTER);
        UnloadScene(SceneName.SUB_LOGIN);
    }
    #endregion
    #region Unload Game Scene
    public void UnloadGameScene() {
        UnloadScene(SceneName.GAME_TLMN);
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
    public BaseCasino GetCurrentCasino() {
        return currentCasino;
    }
    public void SetCurrentCasino(BaseCasino casino) {
        currentCasino = casino;
    }
    public void SetCasino(int type, UnityAction callback) {
        switch (GameConfig.CurrentGameID) {
            case GameID.TLMN:
                Card.setCardType(1);
                LoadAssetBundle.LoadScene(SceneName.GAME_TLMN, SceneName.GAME_TLMN, () => {
                    SetCurrentCasino(TLMNControl.instace);
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
                //        setStage(StateGame.TLMN);
                //        curentCasino = (CasinoStage)currenStage;
                break;
                //    case GameID.TLMNSL:
                //        setStage(StateGame.TLMNSL);
                //        curentCasino = (CasinoStage)currenStage;
                //        break;
                //    case GameID.LIENG:
                //        if (type == 0) { // 5
                //            setStage(StateGame.LIENG5);
                //            curentCasino = (CasinoStage)currenStage;
                //        } else { // 9
                //                 // setStage(StateGame.LIENG9);
                //                 // curentCasino = (CasinoStage) currenStage;
                //        }
                //        break;
                //    case GameID.BACAY:
                //        if (type == 0) { // 5
                //            setStage(StateGame.BACAY5);
                //            curentCasino = (CasinoStage)currenStage;
                //        } else { // 9
                //                 // setStage(StateGame.BACAY9);
                //                 // curentCasino = (CasinoStage) currenStage;
                //        }
                //        break;
                //    case GameID.PHOM:
                //        setStage(StateGame.PHOM);
                //        curentCasino = (CasinoStage)currenStage;
                //        break;
                //    case GameID.POKER:
                //        if (type == 0) { // 5
                //            setStage(StateGame.POKER5);
                //            curentCasino = (CasinoStage)currenStage;
                //        } else { // 9
                //                 // setStage(StateGame.POKER9);
                //                 // curentCasino = (CasinoStage) currenStage;
                //        }
                //        break;
                //    case GameID.XITO:
                //        setStage(StateGame.XITO);
                //        curentCasino = (CasinoStage)currenStage;
                //        break;
                //    case GameID.MAUBINH:
                //        setStage(StateGame.MAUBINH);
                //        curentCasino = (CasinoStage)currenStage;
                //        break;
                //    case GameID.SAM:
                //        setStage(StateGame.SAM);
                //        curentCasino = (CasinoStage)currenStage;
                //        break;
                //    case GameID.XOCDIA:
                //        setStage(StateGame.XOCDIA);
                //        curentCasino = (CasinoStage)currenStage;
                //        break;
                //    default:
                //        break;
        }
        InitCardType();

    }

    private void InitCardType() {
        //switch (gameID) {
        //    case GameID.TLMN:
        //        Card.setCardType(1);
        //        break;
        //    case GameID.TLMNSL:
        //        Card.setCardType(1);
        //        break;
        //    case GameID.LIENG:
        //        Card.setCardType(0);
        //        break;
        //    case GameID.BACAY:
        //        Card.setCardType(0);
        //        break;
        //    case GameID.PHOM:
        //        Card.setCardType(0);
        //        break;
        //    case GameID.POKER:
        //        Card.setCardType(1);
        //        break;
        //    case GameID.XITO:
        //        Card.setCardType(0);
        //        break;
        //    case GameID.MAUBINH:
        //        Card.setCardType(1);
        //        break;
        //    case GameID.SAM:
        //        Card.setCardType(1);
        //        break;
        //    default:
        //        Card.setCardType(1);
        //        break;
        //}
    }

}
