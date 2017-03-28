using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour {
    [SerializeField]
    Text txt_time;
    public float timeCurrent { get; set; }
    // Update is called once per frame
    void Update() {
        if (timeCurrent > 0) {
            timeCurrent -= Time.deltaTime * .8f;
            txt_time.text = (int)timeCurrent + "";
        } else {
            if (CallBack != null)
                CallBack.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void SetTime(float time, UnityAction callback = null) {
        if (time > 0) gameObject.SetActive(true);
        timeCurrent = time;
        CallBack = callback;
    }
    UnityAction CallBack;
}
