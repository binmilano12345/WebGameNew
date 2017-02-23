using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPlayer : MonoBehaviour {
    [SerializeField]
    Text txt_chat;
    [SerializeField]
    RectTransform rectTransform;
    public void SetText(string msg) {
        SetVisible(true);
        txt_chat.text = msg;

        StopCoroutine(WaitHide());
        StartCoroutine(WaitHide());
    }

    IEnumerator WaitHide() {
        yield return new WaitForSeconds(5);
        SetVisible(false);
    }

    public void SetVisible(bool isShow) {
        gameObject.SetActive(isShow);
    }

    public void SetPosition(bool isLeft) {
        if (isLeft) {
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.localPosition = new Vector3(65, 0, 0);
        } else {
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.localPosition = new Vector3(-65, 0, 0);
        }
    }
}
