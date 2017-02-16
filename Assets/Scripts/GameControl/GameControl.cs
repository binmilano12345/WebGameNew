using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {
    public static GameControl instance;
    [SerializeField]
    Transform tf_parent;

    GameObject obj_load;
    [HideInInspector]
    public Toast toast;
    [HideInInspector]
    public PanelMessageSytem messageSytem;
    [HideInInspector]
    public Alert alert;
    void Awake() {
        instance = this;
    }

    public void Init() {
        new ListernerServer();
        SendData.onGetPhoneCSKH();
        PopupAndLoadingScript.instance.LoadPopupAndLoading();
    }

    void OnApplicationQuit() {
        if (ClientConfig.UserInfo.SAVE_PASS != 1) {
            ClientConfig.UserInfo.UNAME = "";
            ClientConfig.UserInfo.PASSWORD = "";
        }
        NetworkUtil.GI().close();
    }
}
