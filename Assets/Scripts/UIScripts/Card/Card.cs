using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using AppConfig;

public class Card : MonoBehaviour {
    #region CONST
    public const float WIDTH = 100;
    const float HEIGHT = 135;
    const float DISTANCE_HEIGHT = 50;
    public const float RATE_SMALL = 0.75f;
    #endregion
    #region VARIABLES
    private int id;
    public int ID {
        get { return id; }
        set { id = value; }
    }
    private float w_Card;
    public float W_Card {
        get { return w_Card; }
        set { w_Card = value; }
    }
    private float h_Card;
    public float H_Card {
        get { return h_Card; }
        set { h_Card = value; }
    }
    public bool isBatHayChua = false;

    #endregion
    #region UI
    [SerializeField]
    Image img_card;
    //[SerializeField]
    //DragDropHandler dragdrop;
    #endregion
    #region ARRAY
    public static int[] LIENG_BACAY_PHOM_XITO = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
            25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52 };

    public static int[] GAME_CON_LAI = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0, 1, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
            25, 13, 14, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 26, 27, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51,
            39, 40, 52 };
    //public static int[] GAME_MAU_BINH = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 1,15, 16,
    //        17, 18, 19, 20, 21, 22, 23, 24, 25,26, 14,28, 29, 30, 31, 32, 33,
    //        34, 35, 36, 37, 38,39, 27,41,  42, 43, 44, 45, 46, 47, 48, 49, 50,
    //        51,52, 40, 53 };
    #endregion

    public static int[] cardPaint = GAME_CON_LAI;
    public delegate void CallBack();
    public CallBack onClickOK;
    Vector3 vtDefault;
    void Awake() {
        img_card.rectTransform.sizeDelta = new Vector2(W_Card, H_Card);
        vtDefault = transform.localPosition;
    }

    public static void setCardType(int type) {
        if (type == 0) {// phom
            cardPaint = LIENG_BACAY_PHOM_XITO;
        //} else if (type == 99) {
        //    cardPaint = GAME_MAU_BINH;
        } else {
            cardPaint = GAME_CON_LAI;
        }
    }

    public void SetCardWithId(int _id) {
        if (_id > 52 || _id < 0) {
            _id = 52;
        }
        ID = _id;
        LoadAssetBundle.LoadSprite(img_card, "card", "bai" + cardPaint[_id]);
    }

    public void SetTouched(bool istouched) {
        img_card.raycastTarget = istouched;
    }
    public bool isDark;
    public void SetDarkCard(bool isDark) {
        this.isDark = isDark;
        if (isDark)
            img_card.color = new Color32(105, 105, 105, 255);
        else
            img_card.color = new Color32(255, 255, 255, 255);
    }

    public void SetVisible(bool isVisible) {
        isBatHayChua = isVisible;
        gameObject.SetActive(isVisible);
    }


    public void setSmall(bool isSmall) {
        if (isSmall) {
            W_Card = WIDTH * RATE_SMALL;
            H_Card = HEIGHT * RATE_SMALL;
        } else {
            W_Card = WIDTH;
            H_Card = HEIGHT;
        }
        img_card.rectTransform.sizeDelta = new Vector2(W_Card, H_Card);
        //transform.localScale = new Vector3(RATE_SMALL, RATE_SMALL, RATE_SMALL);
    }
    private bool isChoose;
    public bool IsChoose {
        get { return isChoose; }
        set {
            isChoose = value;
            if (!isChoose) {
                Vector3 vt = transform.localPosition;
                transform.DOLocalMoveY(0, 0.2f);
            } else {
                Vector3 vt = transform.localPosition;
                vt.y = 0;
                transform.DOLocalMoveY(vt.y + DISTANCE_HEIGHT, 0.2f);
            }
        }
    }
    public void OnClickCard() {
        //if (!isMauBinh || SceneManager.GetSceneByName(SceneName.GAME_TALA).isLoaded) {
            if (!isAuto)
                IsChoose = !IsChoose;
            else {
                if (onClickOK != null) {
                    onClickOK.Invoke();
                } else {
                    IsChoose = !IsChoose;
                }
            }
        //}
    }

    public bool isAuto { get; set; }
    public void setListenerClick(CallBack click) {
        if (onClickOK == null)
            this.onClickOK = click;
    }

    bool isMauBinh = false;
    public void SetIsCardMauBinh(bool isMauBinh = false) {
        this.isMauBinh = isMauBinh;
        //if (dragdrop != null)
        //    dragdrop.enabled = isMauBinh;
    }

    public IEnumerator MoveFromTo(Vector3 from, Vector3 to, float dur, float wait) {
        yield return new WaitForSeconds(wait);
        //GameControl.instance.sound.startchiabaiAudio();
        SetVisible(true);
        transform.localPosition = from;
        transform.DOLocalMove(to, dur);
    }

    public IEnumerator MoveTo(Vector3 to, float dur, float wait) {
        yield return new WaitForSeconds(wait);
        //GameControl.instance.sound.startchiabaiAudio();
        SetVisible(true);
        transform.DOLocalMove(to, dur);
    }
    public IEnumerator MoveFrom(Vector3 from, float dur, float wait, UnityAction callback = null) {
        yield return new WaitForSeconds(wait);
        //GameControl.instance.sound.startchiabaiAudio();     
        //		Debug.LogError ("from " + from);
        SetVisible(true);
        Vector3 vt = transform.localPosition;
        transform.localPosition = from;
        transform.localScale = Vector3.zero;
        transform.DOScale(1, dur);
        transform.DOLocalMove(vt, dur);
        if (callback != null) {
            callback();
        }
    }

    public IEnumerator MoveFromCardHand(Vector3 from, float dur, float wait) {
        yield return new WaitForSeconds(wait);
        //GameControl.instance.sound.startchiabaiAudio();     
        //		Debug.LogError ("from " + from);
        SetVisible(true);
        Vector3 vt = transform.localPosition;
        transform.localPosition = from;
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        setSmall(true);
        transform.DOScale(1, dur);
        //transform.DOScale(.5f, dur);
        transform.DOLocalMove(vt, dur);
    }
}
