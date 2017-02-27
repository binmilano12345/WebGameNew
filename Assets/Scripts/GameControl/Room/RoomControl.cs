using AppConfig;
using DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class RoomControl : MonoBehaviour {
    public static RoomControl instance;
    [SerializeField]
    MyScrollView myScrollView;

    List<ItemTableData> ListTable = new List<ItemTableData>();

    [SerializeField]
    Text txt_name, txt_id, txt_money, txt_game_name;
    [SerializeField]
    RawImage raw_avata;

    bool isAnBanFull = true;
    [SerializeField]
    GameObject obj_tick_ban_full;
    /// <summary>
    /// Sap xep danh sach ban: 1-ten ban, 2-muc cuoc, 3-tien can, 4-so nguoi
    /// </summary>
    int sorttype = 0;
    bool isOderBy = true;

    GameObject Itemtable;
    void Awake() {
        instance = this;
    }

    void Start() {
        GameControl.instance.CurrentCasino = null;
        GameControl.instance.UnloadScene(SceneName.SCENE_MAIN);
        GameControl.instance.UnloadScene(SceneName.SCENE_LOBBY);
        GameControl.instance.UnloadScene(SceneName.GAME_TLMN);
        GameControl.instance.UnloadSubScene();
        PopupAndLoadingScript.instance.ShowTaiXiu();
        SetInfo();
    }

    void SetInfo() {
        txt_name.text = ClientConfig.UserInfo.UNAME;
        txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
        txt_money.text = MoneyHelper.FormatAbsoluteWithoutUnit(ClientConfig.UserInfo.CASH_FREE);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, ClientConfig.UserInfo.AVATAR_ID + "");
        txt_game_name.text = GameConfig.GameName[(int)GameConfig.CurrentGameID];
        obj_tick_ban_full.SetActive(isAnBanFull);
    }

    public void CreateTable(List<ItemTableData> listTable) {
        this.ListTable.Clear();
        this.ListTable.AddRange(listTable);

        if (isAnBanFull) {
            ListTable.RemoveAll(x => (x.NUser == x.MaxUser));
        }

        myScrollView.ClearCells();
        if (Itemtable == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_TABLE, (objPre) => {
                Itemtable = objPre;
                PopupAndLoadingScript.instance.HideLoading();
                myScrollView.OnStartFillItem(Itemtable, listTable.Count);
                myScrollView.UpdateInfo = UpdateItemTable;
            });
        } else {
            PopupAndLoadingScript.instance.HideLoading();
            myScrollView.OnStartFillItem(Itemtable, listTable.Count);
            myScrollView.UpdateInfo = UpdateItemTable;
        }
    }

    public void UpdateListTable(List<ItemTableData> listTable) {
        this.ListTable.Clear();
        this.ListTable.AddRange(listTable);
        PopupAndLoadingScript.instance.HideLoading();
        if (isAnBanFull) {
            ListTable.RemoveAll(x => (x.NUser == x.MaxUser));
        }

        List<ItemTableData> listTemp = new List<ItemTableData>();
        listTemp.AddRange(ListTable);
        ListTable.Clear();
        switch (Mathf.Abs(sorttype)) {
            case 1:
                if (sorttype > 0) {
                    ListTable.AddRange(listTemp.OrderBy(r => r.TableName).ToList());
                } else {
                    ListTable.AddRange(listTemp.OrderByDescending(r => r.TableName).ToList());
                }
                break;
            case 2:
                if (sorttype > 0) {
                    ListTable.AddRange(listTemp.OrderBy(r => r.Money).ToList());
                } else {
                    ListTable.AddRange(listTemp.OrderByDescending(r => r.Money).ToList());
                }
                break;
            case 3:
                if (sorttype > 0) {
                    ListTable.AddRange(listTemp.OrderBy(r => r.NeedMoney).ToList());
                } else {
                    ListTable.AddRange(listTemp.OrderByDescending(r => r.NeedMoney).ToList());
                }
                break;
            case 4:
                if (sorttype > 0) {
                    ListTable.AddRange(listTemp.OrderBy(r => r.NUser).ToList());
                } else {
                    ListTable.AddRange(listTemp.OrderByDescending(r => r.NUser).ToList());
                }
                break;
            default:
                ListTable.AddRange(listTemp.OrderBy(r => r.TableName).ToList());
                break;
        }
        myScrollView.ClearCells();

        myScrollView.OnStartFillItem(Itemtable, listTable.Count);
        myScrollView.UpdateInfo = UpdateItemTable;
    }

    void UpdateItemTable(GameObject obj, int index) {
        if (obj != null) {
            obj.name = index + "";
            ItemTableUI it = obj.GetComponent<ItemTableUI>();
            it.itemData = ListTable[index];
            it.SetUI();
        }
    }
    #region Button Click
    public void OnClickBack() {
        LoadAssetBundle.LoadScene(SceneName.SCENE_LOBBY, SceneName.SCENE_LOBBY);
    }
    public void OnClickSetting() {
        LoadAssetBundle.LoadScene(SceneName.SUB_SETTING, SceneName.SUB_SETTING);
    }
    public void OnClickRefresh() {
        PopupAndLoadingScript.instance.ShowLoading();
        SendData.onUpdateRoom();
    }
    public void OnClickMail() {
    }
    public void OnClickHoTro() {
    }
    public void OnClickRank() {
        LoadAssetBundle.LoadScene(SceneName.SUB_RANK, SceneName.SUB_RANK);
    }
    public void OnClickAnBanFull() {
        isAnBanFull = !isAnBanFull;
        obj_tick_ban_full.SetActive(isAnBanFull);
        OnClickRefresh();
    }

    public void OnClickSortName() {
        isOderBy = sorttype != 1 ? true : !isOderBy;
        sorttype = isOderBy ? 1 : -1;
        OnClickRefresh();
    }
    public void OnClicSortkBet() {
        isOderBy = sorttype != 2 ? true : !isOderBy;
        sorttype = isOderBy ? 2 : -2;
        OnClickRefresh();
    }
    public void OnClickSortNeedMoney() {
        isOderBy = sorttype != 3 ? true : !isOderBy;
        sorttype = isOderBy ? 3 : -3;
        OnClickRefresh();
    }
    public void OnClickSortNUser() {
        isOderBy = sorttype != 4 ? true : !isOderBy;
        sorttype = isOderBy ? 4 : -4;
        OnClickRefresh();
    }
    #endregion
}
