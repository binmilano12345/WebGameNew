using AppConfig;
using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class RoomControl : MonoBehaviour {
    public static RoomControl instance;
    [SerializeField]
    MyScrollView myScrollView;

    List<ItemTableData> listTable = new List<ItemTableData>();

    [SerializeField]
    Text txt_name, txt_id, txt_money, txt_game_name;
    [SerializeField]
    RawImage raw_avata;

    void Awake() {
        instance = this;
    }

    void Start() {
        GameControl.instance.UnloadScene(SceneName.SCENE_MAIN);
        GameControl.instance.UnloadScene(SceneName.SCENE_LOBBY);
        GameControl.instance.UnloadSubScene();
        SetInfo();
    }

    void SetInfo() {
        txt_name.text = ClientConfig.UserInfo.UNAME;
        txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
        txt_money.text = MoneyHelper.FormatAbsoluteWithoutUnit(ClientConfig.UserInfo.CASH_FREE);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, ClientConfig.UserInfo.AVATAR_ID + "");
        txt_game_name.text = GameConfig.GameName[(int)GameConfig.CurrentGameID];
    }

    public void CreateTable(List<ItemTableData> listTable) {
        this.listTable = listTable;

        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_TABLE, (objPre) => {
            PopupAndLoadingScript.instance.HideLoading();
            myScrollView.OnStartFillItem(objPre, listTable.Count);
            myScrollView.UpdateInfo = UpdateItemTable;
        });

    }
    void UpdateItemTable(GameObject obj, int index) {
        if (obj != null) {
            obj.name = index + "";
            ItemTableUI it = obj.GetComponent<ItemTableUI>();
            it.itemData = listTable[index];
            it.SetUI();
        }
    }
    #region Button Click
    public void OnClickBack() {
        LoadAssetBundle.LoadScene(SceneName.SCENE_LOBBY, SceneName.SCENE_LOBBY);
    }
    public void OnClickSetting() {
    }
    public void OnClickNap() {
    }
    public void OnClickMail() {
    }
    public void OnClickHoTro() {
    }
    public void OnClickRank() {
    }
    public void OnClickMenu() {
    }
    #endregion
}
