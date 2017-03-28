using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PanelWaiting : MonoBehaviour {

    public static PanelWaiting instance;

    void Awake() {
        instance = this;
    }

    void OnEnable() {
        StopCoroutine(waitHide());
        StartCoroutine(waitHide());
    }
    public void HideLoading() {
        gameObject.SetActive(false);
    }

    IEnumerator waitHide() {
        yield return new WaitForSeconds(10);
        HideLoading();
    }
}
