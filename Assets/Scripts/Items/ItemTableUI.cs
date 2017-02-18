﻿using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class ItemTableUI : MonoBehaviour {
    public ItemTableData itemData { get; set; }
    [SerializeField]
    Text txt_name, txt_bet, txt_need, txt_nuser;

    public void SetUI() {
        txt_name.text = itemData.TableName;
        txt_bet.text = MoneyHelper.FormatMoneyNormal(itemData.Money);
        txt_need.text = MoneyHelper.FormatMoneyNormal(itemData.NeedMoney);
        txt_nuser.text = itemData.NUser + "/" + itemData.MaxUser;
    }

    public void OnClick() {
        SendData.onJoinTablePlay(itemData.Id, -1);
    }
}
