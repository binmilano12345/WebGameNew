using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class ItemInviteUI : MonoBehaviour {
    public ItemInviteData item { get; set; }
    [SerializeField]
    Text txt_name, txt_money;
    public void SetUI() {
        txt_name.text = item.Name.Length < 10 ? item.Name : item.Name.Substring(0, 9) + "..";
        txt_money.text = MoneyHelper.FormatMoneyNormal(item.Money);
    }
}
