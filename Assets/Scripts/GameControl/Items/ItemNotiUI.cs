using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNotiUI : MonoBehaviour {
    public ItemNotiData item;
    [SerializeField]
    Text txt_title;
    [SerializeField]
    GameObject obj_select;

    public void SetUI() {
        txt_title.text = item.Title;
        SetSelect(false);
    }
    public void SetSelect(bool isSelect) {
        obj_select.SetActive(isSelect);
    }
}
