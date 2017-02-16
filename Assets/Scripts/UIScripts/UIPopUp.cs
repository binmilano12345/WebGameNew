using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

public class UIPopUp : MonoBehaviour {

    // Use this for initialization
    public GameObject dialogPopup;
    public GameObject blackBackground;
    public TweenType tweenType;
    public float tweenTime = 0.2f;
    public bool ShowWhenEnable = true;
    public bool UseRectranform = false;
    public Ease EaseTypeIn = Ease.InBack;
    public Ease EaseTypeOut = Ease.OutBack;
    private ITween tweener;
    private UnityAction hideCallback, showCallback;

    [Header("Only for Tween Location")]
    public Vector2 MoveFromPos;
    public Vector2 MoveToPos;
    public Vector3 OldBackgroundPos = Vector3.zero;
    void OnEnable() {
        if (blackBackground != null)
            OldBackgroundPos = blackBackground.transform.localPosition;
        if (ShowWhenEnable)
            ShowDialog();
    }

    public ITween Tweener {
        get {
            if (tweener == null)
                switch (tweenType) {
                    case TweenType.MoveX:
                        tweener = new TweenMoveX();
                        //tweener.UseRectransform = UseRectranform;
                        //tweener.EaseTypeIn = EaseTypeIn;
                        //tweener.EaseTypeOut = EaseTypeOut;
                        break;
                    case TweenType.TweenMoveLocation:
                        tweener = new TweenMoveLocation(MoveFromPos, MoveToPos);
                        //tweener.UseRectransform = UseRectranform;
                        break;
                    case TweenType.Scale:
                        tweener = new TweenScale();
                        //tweener.UseRectransform = UseRectranform;
                        break;
                    default:
                        tweener = new TweenMoveX();
                        //tweener.UseRectransform = UseRectranform;
                        break;
                }
            tweener.UseRectransform = UseRectranform;
            tweener.EaseTypeIn = EaseTypeIn;
            tweener.EaseTypeOut = EaseTypeOut;
            return tweener;
        }
    }

    public void ShowDialog() {
        ShowDialog(null);
    }

    public void ShowDialog(UnityAction showFinishCallback) {
        showCallback = showFinishCallback;
        if (blackBackground != null)
            blackBackground.transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        Tweener.ShowFinishCallback = onTweenShowComplete;
        Tweener.ShowDialog(dialogPopup.transform, tweenTime);
    }

    void onTweenShowComplete() {
        if (showCallback != null)
            showCallback();
    }

    void onTweenHideComplete() {
        gameObject.SetActive(false);
        if (blackBackground != null)
            blackBackground.transform.localPosition = OldBackgroundPos;
        if (hideCallback != null)
            hideCallback();
    }

    public void HideDialog() {
        HideDialog(null);
    }

    public void HideDialog(UnityAction hideFinishCallback) {
        hideCallback = hideFinishCallback;

        if (DOTween.IsTweening(dialogPopup.transform))
            return;
        Tweener.HideFinishCallback = onTweenHideComplete;
        Tweener.HideDialog(dialogPopup.transform, tweenTime);
    }
}

#region Tween

public enum TweenType {
    MoveX,
    Scale,
    TweenMoveLocation
}

public interface ITween {
    Ease EaseTypeIn { get; set; }
    Ease EaseTypeOut { get; set; }
    bool UseRectransform { get; set; }
    UnityAction ShowFinishCallback { get; set; }
    UnityAction HideFinishCallback { get; set; }
    void ShowDialog(Transform target, float tweenTime);
    void HideDialog(Transform target, float tweenTime);
}

public class TweenMoveX : ITween {
    public Ease EaseTypeIn { get; set; }
    public Ease EaseTypeOut { get; set; }
    public bool UseRectransform { get; set; }
    public UnityAction ShowFinishCallback { get; set; }

    public UnityAction HideFinishCallback { get; set; }

    public void ShowDialog(Transform target, float tweenTime) {
        target.localScale = Vector3.one;
        if (UseRectransform) {
            target.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1500f, target.GetComponent<RectTransform>().anchoredPosition.y);
            target.GetComponent<RectTransform>().DOAnchorPosX(0, tweenTime, true).SetEase(EaseTypeOut).OnComplete(ShowFinish);
        } else {
            target.localPosition = new Vector2(-1500f, target.localPosition.y);
            target.DOLocalMove(new Vector3(0, 0, 0), tweenTime, true).SetEase(EaseTypeOut).OnComplete(ShowFinish);
        }
    }

    private void ShowFinish() {
        if (ShowFinishCallback != null)
            ShowFinishCallback();
    }

    public void HideDialog(Transform target, float tweenTime) {
        //target.DOLocalMove(new Vector3(-1500f, 0, 0), tweenTime, true).SetEase(Ease.InBack).OnComplete(HideFinish);
        target.GetComponent<RectTransform>().DOAnchorPosX(-1500f, tweenTime, true).SetEase(EaseTypeIn).OnComplete(HideFinish);
    }

    private void HideFinish() {
        if (HideFinishCallback != null)
            HideFinishCallback();
    }
}

public class TweenMoveLocation : ITween {
    public Ease EaseTypeIn { get; set; }
    public Ease EaseTypeOut { get; set; }
    public bool UseRectransform { get; set; }
    public UnityAction ShowFinishCallback { get; set; }

    public UnityAction HideFinishCallback { get; set; }

    public Vector2 FromPos;
    public Vector2 ToPos;

    public TweenMoveLocation(Vector2 fromPos, Vector2 toPos) {
        FromPos = fromPos;
        ToPos = toPos;
    }

    public void ShowDialog(Transform target, float tweenTime) {
        //target.localPosition = FromPos;
        //target.DOLocalMove(ToPos, tweenTime, true).SetEase(Ease.OutBack).OnComplete(ShowFinish);

        target.localScale = Vector3.one;
        if (UseRectransform) {
            target.GetComponent<RectTransform>().anchoredPosition = FromPos;
            target.GetComponent<RectTransform>().DOAnchorPos(ToPos, tweenTime, true).SetEase(EaseTypeOut).OnComplete(ShowFinish);
        } else {
            target.localPosition = FromPos;
            target.DOLocalMove(ToPos, tweenTime, true).SetEase(EaseTypeOut).OnComplete(ShowFinish);
        }
    }

    private void ShowFinish() {
        if (ShowFinishCallback != null)
            ShowFinishCallback();
    }

    public void HideDialog(Transform target, float tweenTime) {
        target.DOLocalMove(FromPos, tweenTime, true).SetEase(EaseTypeIn).OnComplete(HideFinish);
    }

    private void HideFinish() {
        if (HideFinishCallback != null)
            HideFinishCallback();
    }
}

public class TweenScale : ITween {
    public Ease EaseTypeIn { get; set; }
    public Ease EaseTypeOut { get; set; }
    public bool UseRectransform { get; set; }
    public UnityAction ShowFinishCallback { get; set; }

    public UnityAction HideFinishCallback { get; set; }

    public void ShowDialog(Transform target, float tweenTime) {
        target.localPosition = new Vector2(target.localPosition.x, target.localPosition.y);
        target.localScale = Vector3.zero;
        target.transform.DOScale(Vector3.one, tweenTime).SetEase(EaseTypeOut).SetUpdate(true).OnComplete(ShowFinish);
    }

    private void ShowFinish() {
        if (ShowFinishCallback != null)
            ShowFinishCallback();
    }

    public void HideDialog(Transform target, float tweenTime) {
        target.DOScale(Vector3.zero, tweenTime).SetEase(EaseTypeIn).SetUpdate(true).OnComplete(HideFinish);
    }

    private void HideFinish() {
        if (HideFinishCallback != null)
            HideFinishCallback();
    }
}

#endregion