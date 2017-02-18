using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MyScrollView : LoopScrollRect {
    public GameObject prfObject;
    public UnityAction<GameObject> RecycleItem;
    public UnityAction<GameObject, int> UpdateInfo;

    protected override void Awake() {
        base.Awake();
    }

    public override GameObject GetItemToAdd() {
        if (prfObject != null) {
            GameObject obj = Instantiate(prfObject, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.localScale = Vector3.one;
            StartCoroutine(WaitSeconds());
            return obj;
        }
        return null;
    }

    private IEnumerator WaitSeconds() {
        yield return new WaitForSeconds(.1f);
    }

    public override void RemoveItem(GameObject go) {
        if (RecycleItem != null) RecycleItem(go);
        else Destroy(go);
    }

    public override void UpdateItemInfo(GameObject item, int index) {
        if (UpdateInfo != null)
            UpdateInfo(item, index);
    }

    public void OnStartFillItem(GameObject obj, int length) {
        if (obj != null) {
            prfObject = obj;
            totalCount = length;
            //Debug.LogError("totalCount " + totalCount);
            RefillCells();
            //Debug.LogError("RefillCells ");
        }
    }
}
