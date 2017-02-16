using ItemData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour {
    public static RoomControl instance;
    void Awake() {
        instance = this;
    }

    public void InitTable(List<ItemTableData> listTable) {
        Debug.LogError(listTable.Count);
    }
}
