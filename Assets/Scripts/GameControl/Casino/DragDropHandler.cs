using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using AppConfig;
using UnityEngine.UI;

public class DragDropHandler : MonoBehaviour,
                                IBeginDragHandler,
                                IDragHandler,
                                IEndDragHandler,
                                IDropHandler {
    public static GameObject itemBeingDragged;
    static Vector3 startPosition, currentPosition;
    bool isDrop = true;
    [SerializeField]
    Card card;
    [SerializeField]
    CanvasGroup canvasGroup;

    Camera cameraInGame;
    Camera GetCamera() {
        Camera ccc = null;
        foreach (Camera c in Camera.allCameras) {
            if (c.name.Equals("Camera")) {
                ccc = c;
                break;
            }
        }
        return ccc;
    }

    #region IBeginDragHandler implement
    public void OnBeginDrag(PointerEventData eventData) {
        if (cameraInGame == null)
            cameraInGame = GetCamera();
        itemBeingDragged = gameObject;
        startPosition = transform.localPosition;
        canvasGroup.blocksRaycasts = false;
        isDrop = false;
        int id = card.ID;
        ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasBeginDrag(id));
    }
    #endregion    

    #region IDragHandler implement
    public void OnDrag(PointerEventData eventData) {
        Vector3 vtScreen = Input.mousePosition;
        if (cameraInGame != null) {
            vtScreen = transform.parent.InverseTransformPoint(cameraInGame.ScreenToWorldPoint(Input.mousePosition));
        }
        vtScreen.z = 0;
        transform.localPosition = vtScreen;

        ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasDrag(transform.position));
    }
    #endregion

    #region IEndDragHandler implement
    public void OnEndDrag(PointerEventData eventData) {

        if (ClientConfig.UserInfo.CURRENT_GAME_ID.Equals(GameID.PHOM.ToString())) {
            if (itemBeingDragged != null) {
                transform.localPosition = startPosition;
            }
        } else {
            if (!isDrop) {
                transform.localPosition = startPosition;
            }
        }
        isDrop = true;
        itemBeingDragged = null;
        ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasEndDrag());
        canvasGroup.blocksRaycasts = true;
    }
    #endregion
    public void OnDrop(PointerEventData eventData) {
        if (itemBeingDragged != null) {
            if (ClientConfig.UserInfo.CURRENT_GAME_ID.Equals(GameID.PHOM.ToString())) {
                #region CARD PHOM
                Card temp = itemBeingDragged.GetComponent<Card>();
                ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasDrop(temp, card));

                itemBeingDragged = null;
                #endregion
            } else {
                #region CARD MAUBINH
                itemBeingDragged.transform.localPosition = startPosition;
                Card temp = itemBeingDragged.GetComponent<Card>();

                int id = temp.ID;

                temp.SetCardWithId(card.ID);
                temp.SetDarkCard(false);
                itemBeingDragged = null;

                card.SetCardWithId(id);
                card.SetDarkCard(false);

                ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasDrop());
                #endregion
            }
        } else {
            transform.localPosition = startPosition;
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasDrop(null, null));
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasDrop());
        }
        isDrop = true;
        canvasGroup.blocksRaycasts = true;
    }
}
namespace UnityEngine.EventSystems {
    public interface IHasChanged : IEventSystemHandler {
        void HasDrop();
        void HasDrop(Card idDrag, Card idDrop);
        void HasBeginDrag(int id);
        void HasDrag(Vector3 vtPos);
        void HasEndDrag();
    }
}
