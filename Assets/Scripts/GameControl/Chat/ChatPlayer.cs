using AppConfig;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPlayer : MonoBehaviour {
    [SerializeField]
    Text txt_chat;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    Image img_emotion;

    bool isOne = true;
    public void SetText(string msg) {
        string temp;
        bool check = DialogChat.emoticons.TryGetValue(msg, out temp);

        if (check) {
            rectTransform.gameObject.SetActive(false);
            img_emotion.gameObject.SetActive(true);
            LoadAssetBundle.LoadSprite(img_emotion, BundleName.EMOTIONS, temp);
            if (isOne) {
                isOne = false;
                img_emotion.transform.DOScale(1.1f, 0.4f).SetLoops(-1);
            }
        } else {
            rectTransform.gameObject.SetActive(true);
            img_emotion.gameObject.SetActive(false);
            txt_chat.text = msg;
        }

        SetVisible(true);

        StopCoroutine(WaitHide());
        StartCoroutine(WaitHide());
    }

    IEnumerator WaitHide() {
        yield return new WaitForSeconds(3);
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
