using AppConfig;
using DataBase;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DialogInvite : MonoBehaviour {
    public static DialogInvite instance;
    [SerializeField]
    Transform parent;
    void Awake() {
        instance = this;
    }
    List<ItemInviteUI> listUI = new List<ItemInviteUI>();
    public void Init(List<ItemInviteData> list) {
        for (int i = 0; i < listUI.Count; i++) {
            Destroy(listUI[i].gameObject);
        }
        listUI.Clear();
        //Thread t = new Thread(new ThreadStart(delegate {
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ITEM_INVITE, (objPre) => {
            for (int i = 0; i < list.Count; i++) {
                GameObject obj = Instantiate(objPre);
                obj.transform.SetParent(parent);
                obj.transform.localScale = Vector3.zero;
                obj.transform.DOScale(1, 0.2f).SetDelay(i * 0.05f);
                ItemInviteUI it = obj.GetComponent<ItemInviteUI>();
                it.item = list[i];
                it.SetUI();
                it.GetComponent<UIButton>()._onClick.AddListener(delegate {
                    OnClickInvite(it);
                });
                listUI.Add(it);
            }
        });
        //}));
        //t.Start();
    }
    void OnClickInvite(ItemInviteUI ite) {
        SendData.onInviteFriend(ite.item.Name);
        ite.transform.DOScale(0, 0.4f).OnComplete(delegate {
            Destroy(ite.gameObject);
            listUI.Remove(ite);
        });
        if (listUI.Count <= 1) {
            GetComponent<UIPopUp>().HideDialog();
        }
    }

    public void OnClickInviteAll() {
        for (int i = 0; i < listUI.Count; i++) {
            SendData.onInviteFriend(listUI[i].item.Name);
            Destroy(listUI[i].gameObject);
            listUI.RemoveAt(i);
            i--;
        }
        GetComponent<UIPopUp>().HideDialog();
    }
}
