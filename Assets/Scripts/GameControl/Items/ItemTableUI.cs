using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class ItemTableUI : MonoBehaviour {
    public ItemTableData itemData { get; set; }
    [SerializeField]
    Text txt_name, txt_bet, txt_need, txt_nuser;
    [SerializeField]
    GameObject obj_lock;
    [SerializeField]
    RectTransform rect_bkg_nuser, rect_fg_nuser;

    public void SetUI() {
        txt_name.text = itemData.TableName;
        txt_bet.text = MoneyHelper.FormatMoneyNormal(itemData.Money);
        txt_need.text = MoneyHelper.FormatMoneyNormal(itemData.NeedMoney);
        txt_nuser.text = itemData.NUser + "/" + itemData.MaxUser;
        rect_bkg_nuser.sizeDelta = new Vector2(17 * itemData.MaxUser, 22);
        rect_fg_nuser.sizeDelta = new Vector2(17 * itemData.NUser, 22);
        obj_lock.SetActive(itemData.IsLock == 1);
    }

    public void OnClick() {
		PopupAndLoadingScript.instance.ShowLoading();
        SendData.onJoinTablePlay(itemData.Id, -1);
    }
}
