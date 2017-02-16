using ItemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTableUI : MonoBehaviour {
    public ItemTableData itemData { get; set; }
    [SerializeField]
    Text txt_name, txt_bet, txt_need, txt_nuser;
}
