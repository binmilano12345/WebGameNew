using AppConfig;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
	public UnityEvent _onClick;
	public UnityEngine.UI.Image image;
    private const float effectTime = 0.1f;

    private const float minScale = 0.95f;
    private const float maxScale = 1.05f;

    private Vector3 normalScale;
    public bool isPlayingEffect;

    void Awake()
    {
        normalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData e)
    {
        DOTween.Complete(transform);
        transform.DOScale(normalScale * minScale, effectTime).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData e)
    {
        DOTween.Complete(transform);
        transform.DOScale(normalScale, effectTime).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData e)
    {
//        if (isPlayingEffect) return;
        DOTween.Complete(transform);
        transform.localScale = normalScale * minScale;
        CoReleaseAnimation();
        //ClientConfig.Sound.PlaySound(ClientConfig.Sound.SoundId.Button_Click);
    }

    private void CoReleaseAnimation()
    {
//        isPlayingEffect = true;
		_onClick.Invoke();
        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(normalScale * maxScale, effectTime));
        sequence.Append(transform.DOScale(normalScale, effectTime));

        sequence.SetUpdate(true);

        sequence.OnComplete(()=> {
//            isPlayingEffect = false;
        });
    }

    private void OnEnable()
    {
        DOTween.Complete(transform);
        transform.localScale = normalScale;
        isPlayingEffect = false;
    }

    private void OnDisable()
    {
        DOTween.Complete(transform);
        transform.localScale = normalScale;
        isPlayingEffect = false;
    }
}