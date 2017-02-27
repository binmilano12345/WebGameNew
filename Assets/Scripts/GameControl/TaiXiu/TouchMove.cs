using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchMove : MonoBehaviour,
                                IBeginDragHandler,
                                IDragHandler,
                                IEndDragHandler {
    float xLeft, xRight, xCenter;
    void Start() {
        //img.DOFade(0.4f, 1f).SetDelay(1);

        xLeft = transform.InverseTransformPoint(new Vector3(0, 0, 0)).x;
        xRight = transform.InverseTransformPoint(new Vector3(Screen.width, 0, 0)).x;
        xCenter = transform.InverseTransformPoint(new Vector3(Screen.width/2, 0, 0)).x;
        OnEndDrag(null);

    }
    //void Update() {
    //    if (isDrag) {
    //        transform.position = Vector2.Lerp(transform.position, Input.mousePosition, Time.deltaTime * 5);
    //    }
    //}
    [SerializeField]
    Image img;
    bool isDrag = false;
    #region IBeginDragHandler implement
    public void OnBeginDrag(PointerEventData eventData) {
        img.DOKill();
        img.color = new Color32(255, 255, 255, 255);
        isDrag = true;
    }
    #endregion    

    #region IDragHandler implement
    public void OnDrag(PointerEventData eventData) {
        //float dis = Vector3.Distance(transform.position, Input.mousePosition);
        //transform.DOMove(Input.mousePosition, dis/100);
        transform.position = Input.mousePosition;

    }
    #endregion

    #region IEndDragHandler implement
    public void OnEndDrag(PointerEventData eventData) {
        isDrag = false;
        img.DOKill();
        img.DOFade(0.4f, 1f).SetDelay(1);
        if (transform.localPosition.x > xCenter) {
            transform.DOLocalMoveX(xRight - 50, 0.6f);
        } else {
            transform.DOLocalMoveX(xLeft + 50, 0.6f);
        }
    }
    #endregion
}

