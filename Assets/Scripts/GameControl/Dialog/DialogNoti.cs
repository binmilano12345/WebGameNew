using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogNoti : MonoBehaviour {
    [SerializeField]
    Transform tf_parent;
    [SerializeField]
    Text txt_content;

    // Use this for initialization
    void Start() {
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_NOTI, (objPre) => {
            GameObject objInit = null;
            for (int i = 0; i < GameControl.instance.ListNoti.Count; i++) {
                GameObject obj = Instantiate(objPre);
                obj.transform.SetParent(tf_parent);
                obj.transform.localScale = Vector3.one;
                obj.name = i + "";
                ItemNotiUI it = obj.GetComponent<ItemNotiUI>();
                it.item = GameControl.instance.ListNoti[i];
                it.SetUI();
                it.GetComponent<UIButton>()._onClick.AddListener(delegate {
                    OnClickItem(obj);
                });
                if (i == 0)
                    objInit = obj;
            }
            Destroy(objPre);
            if (objInit != null)
                OnClickItem(objInit);
        });
    }

    void OnClickItem(GameObject obj) {
        int index = int.Parse(obj.name);
        txt_content.text = GameControl.instance.ListNoti[index].Content;
    }

    public void OnHide() {
        GetComponent<UIPopUp>().HideDialog(() => {
            GameControl.instance.UnloadScene(SceneName.SUB_NOTI);
        });
    }
}
