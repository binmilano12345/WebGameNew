using AppConfig;
using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class BasePlayer : MonoBehaviour {
    [SerializeField]
    Text txt_name, txt_money;
    [SerializeField]
    RawImage raw_avata;
    [SerializeField]
    GameObject master;

    public PlayerData playerData { get; set; }

    public void SetInfo() {
        txt_name.text = playerData.Name;
        txt_money.text = MoneyHelper.FormatMoneyNormal(playerData.Money);
        SetMaster(playerData.IsMaster);
        LoadAssetBundle.LoadTexture(raw_avata, BundleName.AVATAS, playerData.Avata_Id + "");
    }

    public void SetMaster(bool isMaster) {
        master.SetActive(isMaster);
    }
}
