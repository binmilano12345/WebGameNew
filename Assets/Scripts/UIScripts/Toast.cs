using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class Toast : MonoBehaviour {
    [SerializeField]
    Text txt;
    [SerializeField]
    Image img;
    public void showToast(string mess) {
        if (!string.IsNullOrEmpty(mess)) {
            txt.text = mess.Trim();
            gameObject.SetActive(true);
            img.color = new Color32(255, 255, 255, 0);
            img.DOFade(1, 0.2f).OnComplete(wait);
        } else {
            gameObject.SetActive(false);
        }
    }

    void wait() {
        img.DOFade(0, 0.2f).SetDelay(2).OnComplete(delegate {
            gameObject.SetActive(false);
        });
    }
}
