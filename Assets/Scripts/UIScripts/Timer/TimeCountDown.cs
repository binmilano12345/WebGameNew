using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            gameObject.SetActive(false);
        }
    }

    public void SetTime(float time) {
        if (time > 0) gameObject.SetActive(true);
        timeCurrent = time;
    }
}
