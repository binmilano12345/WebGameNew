using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AppConfig;

public class Alert : MonoBehaviour {
    [SerializeField]
    Text txt_alert;
    const float POS_X_DEFAULT = 300.0f;
    Vector3 End_Position, Start_Position;
    float Speed = 150;
    List<string> listMsg = new List<string>();
    public void SetAlert(string str) {
        if (string.IsNullOrEmpty(str) || SceneManager.GetSceneByName(SceneName.SCENE_MAIN).isLoaded) {
            gameObject.SetActive(false);
            return;
        }

        if (!listMsg.Contains(str)) {
            listMsg.Add(str);
        }
        if (!gameObject.activeSelf) {
            txt_alert.text = listMsg[0];
            gameObject.SetActive(true);
            txt_alert.transform.localPosition = new Vector3(POS_X_DEFAULT, 0, 0);
            float w = LayoutUtility.GetPreferredWidth(txt_alert.rectTransform);
            End_Position.x = -POS_X_DEFAULT - w;
        }
    }
    void Update() {
        Vector3 vt = txt_alert.transform.localPosition;
        vt.x -= Time.deltaTime * Speed;
        txt_alert.transform.localPosition = vt;
        if (vt.x <= End_Position.x) {
            listMsg.RemoveAt(0);
            if (listMsg.Count <= 0) {
                gameObject.SetActive(false);
            } else {
                txt_alert.text = listMsg[0];
                float w = LayoutUtility.GetPreferredWidth(txt_alert.rectTransform);
                End_Position.x = -POS_X_DEFAULT - w;
                txt_alert.transform.localPosition = new Vector3(POS_X_DEFAULT, 0, 0);
            }
        }
    }
}
