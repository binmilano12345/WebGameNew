using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatActionViewScript : MonoBehaviour {
    [SerializeField]
    UIButton[] btn_actions;
    [SerializeField]
    UIButton btn_Kick;

    Vector3[] vt_right = new Vector3[] { new Vector3(0, 120, 0), new Vector3(65, 110, 0), new Vector3(110, 65, 0), new Vector3(120, 0, 0) };
    Vector3[] vt_left = new Vector3[] { new Vector3(0, 120, 0), new Vector3(-65, 110, 0), new Vector3(-110, 65, 0), new Vector3(-120, 0, 0) };
    Vector3[] vt_up = new Vector3[] { new Vector3(-120, 0, 0), new Vector3(-110, -65, 0), new Vector3(-65, -110, 0), new Vector3(0, -120, 0) };

    void Awake() {
    }
    void Start() {
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < btn_actions.Length; i++) {
            UIButton btn = btn_actions[i];
            btn.gameObject.name = "*f" + (i + 1);
            btn._onClick.AddListener(delegate {
                OnClickAction(btn.gameObject);
            });
        }
    }
    public string namePlayer { get; set; }
    public bool IsShowKick { get; set; }
    void OnClickAction(GameObject obj) {
        //Controller.OnHandleUIEvent("SendChatAction", new object[] { namePlayer, obj.name });
        OnHideAction();
    }
    Align_Anchor align = Align_Anchor.NONE;
    public void SetAnchor(Align_Anchor align) {
        this.align = align;
        switch (align) {
            case Align_Anchor.LEFT:
                left();
                break;
            case Align_Anchor.RIGHT:
                right();
                break;
            case Align_Anchor.BOT:
                bot();
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
        if (btn_Kick != null)
            btn_Kick.gameObject.SetActive(IsShowKick);
    }
    public bool isShow() {
        return gameObject.activeInHierarchy;
    }
    public void OnShowAction() {
        Debug.LogError("Show len nao");
        gameObject.SetActive(true);
        switch (align) {
            case Align_Anchor.LEFT:
                left();
                break;
            case Align_Anchor.RIGHT:
                right();
                break;
            case Align_Anchor.BOT:
                bot();
                break;
            default:
                gameObject.SetActive(false);
                break;
        }

        if (btn_Kick != null)
            btn_Kick.gameObject.SetActive(IsShowKick);
    }

    public void OnHideAction() {
        for (int i = 0; i < btn_actions.Length; i++) {
            btn_actions[i].transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(delegate { gameObject.SetActive(false); });
        }

        if (btn_Kick != null)
            btn_Kick.gameObject.SetActive(false);
    }
    void left() {
        for (int i = 0; i < btn_actions.Length; i++) {
            btn_actions[i].transform.localPosition = Vector3.zero;
            Vector3 vt = vt_right[i < 4 ? i : i - 4];
            if (i >= 4) {
                vt.y = -vt.y;
            }

            btn_actions[i].transform.DOLocalMove(vt, 0.2f);
        }
    }
    void right() {
        for (int i = 0; i < btn_actions.Length; i++) {
            btn_actions[i].transform.localPosition = Vector3.zero;

            Vector3 vt = vt_left[i < 4 ? i : i - 4];
            if (i >= 4) {
                vt.y = -vt.y;
            }

            btn_actions[i].transform.DOLocalMove(vt, 0.2f);
        }
    }
    void bot() {
        for (int i = 0; i < btn_actions.Length; i++) {
            btn_actions[i].transform.localPosition = Vector3.zero;

            Vector3 vt = vt_up[i < 4 ? i : i - 4];
            if (i >= 4) {
                vt.x = -vt.x;
            }

            btn_actions[i].transform.DOLocalMove(vt, 0.2f);
        }
    }

    public void OnClickKick() {
        //Controller.OnHandleUIEvent("SendKickPlayer", new object[] { namePlayer });
        OnHideAction();
    }

    void Update() {
        if (Input.GetButtonDown("Fire1") || Input.GetMouseButton(0)) {
            bool isTouchOut = true;
            if (CheckTouchOut(btn_Kick)) {
                isTouchOut = false;
            }
            if (isTouchOut) {
                for (int i = 0; i < btn_actions.Length; i++) {
                    if (CheckTouchOut(btn_actions[i])) {
                        isTouchOut = false;
                        break;
                    }
                }
            }
            if (isTouchOut) {
                OnHideAction();
            }
        }
    }
    //Camera GetCamera() {
    //    Camera ccc = null;
    //    foreach (Camera c in Camera.allCameras) {
    //        if (c.name.Equals("Camera")) {
    //            ccc = c;
    //            break;
    //        }
    //    }
    //    return ccc;
    //}
    bool CheckTouchOut(UIButton rect) {
        //Camera ccc = GetCamera();
        //if (ccc != null) {
        Vector3 vtScreen = /*ccc.ScreenToWorldPoint*/(Input.mousePosition);
        return RectTransformUtility.RectangleContainsScreenPoint(rect.GetComponent<RectTransform>(), vtScreen);
        //}
        //return false;
    }
}
