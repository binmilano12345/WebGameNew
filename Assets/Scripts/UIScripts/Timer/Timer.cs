using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    [SerializeField]
    Image img_time;
    float time = 0;
    float timeCurrent = 0;
    //bool isTurnTime = false;

    Color m_FullTimeColor = Color.green;
    Color m_ZeroTimeColor = Color.red;

    UnityAction CallBack;
    // Update is called once per frame
    void Update() {
        //if (isTurnTime) {
        if(timeCurrent > 0) {
            timeCurrent -= Time.deltaTime;
            if (timeCurrent <= 0) {
                if (CallBack != null)
                    CallBack.Invoke();
                gameObject.SetActive(false);
                return;
            }

            img_time.fillAmount = timeCurrent / time;
            float va = timeCurrent / time;
            img_time.fillAmount = va;
            img_time.color = Color.Lerp(m_ZeroTimeColor, m_FullTimeColor, va);
        }
    }

    public void SetTime(float time, UnityAction callback = null) {
        CallBack = callback;
        if (time <= 0) {
            gameObject.SetActive(false);
        } else {
            this.time = time;
            timeCurrent = time;
            float va = timeCurrent / time;
            img_time.fillAmount = va;
            img_time.color = Color.Lerp(m_ZeroTimeColor, m_FullTimeColor, va);
            gameObject.SetActive(true);
        }
    }

    //public void TurnTime() {
    //    isTurnTime = false;
    //    img_time.color = m_FullTimeColor;
    //    gameObject.SetActive(true);
    //}
}
