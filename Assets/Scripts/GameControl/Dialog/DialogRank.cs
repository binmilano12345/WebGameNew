using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogRank : MonoBehaviour {
    [SerializeField]
    Transform tf_parent;

    // Use this for initialization
    void Start() {
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_RANK, (objPre) => {
            for (int i = 0; i < GameConfig.ListRank.Count; i++) {
                GameObject obj = Instantiate(objPre);
                obj.transform.SetParent(tf_parent);
                obj.transform.localScale = Vector3.one;
                ItemRankUI it = obj.GetComponent<ItemRankUI>();
                it.item = GameConfig.ListRank[i];
                it.SetUI();
            }
        });
    }
}
