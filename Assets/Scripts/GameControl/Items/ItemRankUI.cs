using AppConfig;
using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class ItemRankUI : MonoBehaviour {
    [SerializeField]
    Image img_avata, img_rank;
    [SerializeField]
    Text txt_name, txt_money, txt_rank;

    public ItemRankData item { get; set; }

    public void SetUI() {
        img_rank.gameObject.SetActive((item.Rank == 1 || item.Rank == 2 || item.Rank == 3));
        if (item.Rank == 1 || item.Rank == 2 || item.Rank == 3)
            LoadAssetBundle.LoadSprite(img_rank, BundleName.UI, "rank_" + item.Rank);

        txt_name.text = item.Name;
        txt_money.text = MoneyHelper.FormatRelativelyWithoutUnit(item.Money);
        txt_rank.text = item.Rank + "";

        LoadAssetBundle.LoadSprite(img_avata, BundleName.AVATAS, item.Avata_Id + "");
    }
}
