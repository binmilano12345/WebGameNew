using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace UnityEngine.UI {
    public enum ScrollType {
        Horizontal,
        Vertical
    }

    [AddComponentMenu("UI/Loop Scroll Rect", 16)]
    [SelectionBase]
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public abstract class LoopScrollRect : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement {
        //==========LoopScrollRect==========

        public bool initInStart = true;

        public int totalCount;  //negative means INFINITE mode

        //public float threshold = 100;

        public bool reverseDirection = false;

        protected int itemStart = 0;
        protected int itemEnd = 0;
        protected int moveTheoHuong = 0;
        [SerializeField]
        private RectTransform m_ListItem;
        [SerializeField]
        private ScrollType defaultScrollType = ScrollType.Horizontal;
        public ScrollType scrollType { get { return defaultScrollType; } set { defaultScrollType = value; } }
        public RectTransform listItem { get { return m_ListItem; } set { m_ListItem = value; } }

        public enum MovementType {
            Unrestricted, // Unrestricted movement -- can scroll forever
            Elastic, // Restricted but flexible -- can go past the edges, but springs back in place
            Clamped, // Restricted movement where it's not possible to go past the edges
        }

        [Serializable]
        public class ScrollRectEvent : UnityEvent<Vector2> { }

        public enum ScrollbarVisibility {
            Permanent,
            AutoHide,
            AutoHideAndExpandViewport,
        }

        [SerializeField]
        private MovementType m_MovementType = MovementType.Elastic;
        public MovementType movementType { get { return m_MovementType; } set { m_MovementType = value; } }

        //dan hoi
        [SerializeField]
        private float m_Elasticity = 0.1f; // Only used for MovementType.Elastic
        public float elasticity { get { return m_Elasticity; } set { m_Elasticity = value; } }

        //quan tinh
        [SerializeField]
        private bool m_Inertia = true;
        public bool inertia { get { return m_Inertia; } set { m_Inertia = value; } }

        //giam toc
        [SerializeField]
        private float m_DecelerationRate = 0.135f; // Only used when inertia is enabled
        public float decelerationRate { get { return m_DecelerationRate; } set { m_DecelerationRate = value; } }

        [SerializeField]
        private float m_ScrollSensitivity = 1.0f;
        public float scrollSensitivity { get { return m_ScrollSensitivity; } set { m_ScrollSensitivity = value; } }

        [SerializeField]
        private RectTransform m_Viewport;
        public RectTransform viewport { get { return m_Viewport; } set { m_Viewport = value; SetDirtyCaching(); } }
        #region scroll bar
        /*
        [SerializeField]
        private Scrollbar m_HorizontalScrollbar;
        public Scrollbar horizontalScrollbar
        {
            get
            {
                return m_HorizontalScrollbar;
            }
            set
            {
                if (m_HorizontalScrollbar)
                    m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
                m_HorizontalScrollbar = value;
                if (m_HorizontalScrollbar)
                    m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
                SetDirtyCaching();
            }
        }

        [SerializeField]
        private Scrollbar m_VerticalScrollbar;
        public Scrollbar verticalScrollbar
        {
            get
            {
                return m_VerticalScrollbar;
            }
            set
            {
                if (m_VerticalScrollbar)
                    m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
                m_VerticalScrollbar = value;
                if (m_VerticalScrollbar)
                    m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
                SetDirtyCaching();
            }
        }

        [SerializeField]
        private ScrollbarVisibility m_HorizontalScrollbarVisibility;
        public ScrollbarVisibility horizontalScrollbarVisibility { get { return m_HorizontalScrollbarVisibility; } set { m_HorizontalScrollbarVisibility = value; SetDirtyCaching(); } }

        [SerializeField]
        private ScrollbarVisibility m_VerticalScrollbarVisibility;
        public ScrollbarVisibility verticalScrollbarVisibility { get { return m_VerticalScrollbarVisibility; } set { m_VerticalScrollbarVisibility = value; SetDirtyCaching(); } }

        [SerializeField]
        private float m_HorizontalScrollbarSpacing;
        public float horizontalScrollbarSpacing { get { return m_HorizontalScrollbarSpacing; } set { m_HorizontalScrollbarSpacing = value; SetDirty(); } }

        [SerializeField]
        private float m_VerticalScrollbarSpacing;
        public float verticalScrollbarSpacing { get { return m_VerticalScrollbarSpacing; } set { m_VerticalScrollbarSpacing = value; SetDirty(); } }
        */
        #endregion

        [SerializeField]
        private ScrollRectEvent m_OnValueChanged = new ScrollRectEvent();
        public ScrollRectEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

        // The offset from handle position to mouse down position
        private Vector2 m_PointerStartLocalCursor = Vector2.zero;
        private Vector2 m_ContentStartPosition = Vector2.zero;

        private RectTransform m_ViewRect;

        protected RectTransform viewRect {
            get {
                if (m_ViewRect == null)
                    m_ViewRect = m_Viewport;
                if (m_ViewRect == null)
                    m_ViewRect = (RectTransform)transform;
                return m_ViewRect;
            }
        }

        private Bounds m_ContentBounds;
        private Bounds m_ViewBounds;

        private Vector2 m_Velocity;
        public Vector2 velocity { get { return m_Velocity; } set { m_Velocity = value; } }

        private bool m_Dragging;

        private Vector2 m_PrevPosition = Vector2.zero;
        private Bounds m_PrevContentBounds;
        private Bounds m_PrevViewBounds;
        [NonSerialized]
        private bool m_HasRebuiltLayout = false;

        private bool m_HSliderExpand;
        private bool m_VSliderExpand;
        private float m_HSliderHeight;
        private float m_VSliderWidth;

        [System.NonSerialized]
        private RectTransform m_Rect;
        private RectTransform rectTransform {
            get {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        private RectTransform m_HorizontalScrollbarRect;
        private RectTransform m_VerticalScrollbarRect;

        private DrivenRectTransformTracker m_Tracker;

        //CARE FROM HERE======================================================================================================= 201 -> 680
        protected override void Awake() {
            if (listItem == null)
                listItem = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            if (scrollType == ScrollType.Vertical) {
                moveTheoHuong = -1;

                //GridLayoutGroup layout = content.GetComponent<GridLayoutGroup> ();
                //if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedColumnCount) {
                //	Debug.LogError ("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
                //}
            } else {
                moveTheoHuong = 1;

                //GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
                //if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedRowCount)
                //{
                //	Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
                //}
            }
        }

        #region basic get infomation
        //lay size cua item + spacing
        protected float GetSize(RectTransform item) {
            if (scrollType == ScrollType.Horizontal) {
                return LayoutUtility.GetPreferredWidth(item) + itemSpacing;
            } else {
                return LayoutUtility.GetPreferredHeight(item) + itemSpacing;
            }
        }


        protected float GetScrollDirection(Vector2 vector) {
            if (scrollType == ScrollType.Horizontal) {
                return vector.x;
            } else {
                return vector.y;
            }
        }

        protected Vector2 GetVector(float value) {
            if (scrollType == ScrollType.Horizontal) {
                return new Vector2(-value, 0);
            } else {
                return new Vector2(0, value);
            }
        }

        private float ItemSpacing = -1;
        protected float itemSpacing {
            get {
                if (ItemSpacing >= 0) {
                    return ItemSpacing;
                }
                ItemSpacing = 0;
                if (listItem != null) {
                    HorizontalOrVerticalLayoutGroup layout1 = listItem.GetComponent<HorizontalOrVerticalLayoutGroup>();
                    if (layout1 != null) {
                        ItemSpacing = layout1.spacing;
                    } else {
                        GridLayoutGroup layout2 = listItem.GetComponent<GridLayoutGroup>();
                        if (layout2 != null) {
                            ItemSpacing = GetScrollDirection(layout2.spacing);
                        }
                    }
                }
                return ItemSpacing;
            }
        }
        #endregion
        public int maxLength;
        protected int MaxLength {
            get {
                if (maxLength > 0) {
                    return maxLength;
                }
                //maxLength = 16;
                if (listItem != null) {
                    GridLayoutGroup layout2 = listItem.GetComponent<GridLayoutGroup>();
                    if (layout2 != null) {
                        if (layout2.constraint == GridLayoutGroup.Constraint.Flexible) {
                            Debug.LogWarning("[LoopScrollRect] Flexible not supported yet");
                        }
                        maxLength = layout2.constraintCount;
                    }
                }
                return maxLength;
            }
        }

        private float currentFirst = 0;
        private float currentLast = 0;

        protected virtual bool UpdateItems(Bounds viewBounds, Bounds contentBounds) {
            if (scrollType == ScrollType.Horizontal)
                return CheckIfItNeedToInstantiateHorizontal(viewBounds, contentBounds);
            else
                return CheckIfItNeedToInstantiateVerical(viewBounds, contentBounds);
        }

        private bool CheckIfItNeedToInstantiateVerical(Bounds viewBounds, Bounds contentBounds) {
            bool changed = false;
            if (totalCount > 0 && totalCount < maxLength && listItem.transform.childCount < totalCount || totalCount >= maxLength && listItem.transform.childCount < maxLength) {
                GameObject objNew = NewItemAtEnd();
                float size = (objNew != null) ? GetSizeNewItemAtEnd(objNew.GetComponent<RectTransform>()) : 0;
                if (size > 0) {
                    changed = true;
                }
                if (listItem.transform.childCount > 15) {
                    currentFirst = -listItem.transform.GetChild(4).transform.localPosition.y;
                    currentLast = -listItem.transform.GetChild(11).transform.localPosition.y;
                    //Debug.LogError("listItem.transform.GetChild(4) " + listItem.transform.GetChild(4).transform.localPosition.x + " listItem.transform.GetChild(11) " + listItem.transform.GetChild(11).transform.localPosition.x);
                }
                //else if (listItem.transform.childCount > 7) {
                //    currentFirst = -listItem.transform.GetChild(2).transform.localPosition.y;
                //    currentLast = -listItem.transform.GetChild(5).transform.localPosition.y;
                //}
            } else if (totalCount > maxLength && listItem.transform.childCount == maxLength) {
                //Debug.LogError("currentFirst " + currentFirst + " currentLast " + currentLast + " listItem.transform " + listItem.transform.localPosition.y);
                if (listItem.transform.localPosition.y > currentLast && itemEnd < totalCount) {
                    GameObject objNew = NewItemAtEnd();
                    float size = objNew != null ? GetSizeNewItemAtEnd(objNew.GetComponent<RectTransform>()) : 0;
                    if (size > 0) {
                        changed = true;
                        GameObject objStart = DeleteItemAtStart();
                        float sizeStart = GetSizeDeleteItemAtStart(objStart.GetComponent<RectTransform>());
                        RemoveItem(objStart);
                    }
                } else if (listItem.transform.localPosition.y < currentFirst && itemStart > 0) {
                    GameObject objNew = NewItemAtStart();
                    if (objNew != null)
                        objNew.transform.SetAsFirstSibling();
                    float size = objNew != null ? GetSizeItemAtStart(objNew.GetComponent<RectTransform>()) : 0;
                    if (size > 0) {
                        changed = true;
                        GameObject obj = DeleteItemAtEnd();
                        float sizeLast = GetSizeDeleteItemAtEnd(obj.GetComponent<RectTransform>());
                        RemoveItem(obj);
                    }
                }
            }

            return changed;
        }

        private bool CheckIfItNeedToInstantiateHorizontal(Bounds viewBounds, Bounds contentBounds) {
            //Debug.LogError("CheckIfItNeedToInstantiateHorizontal");
            bool changed = false;
            if (totalCount > 0 && totalCount < maxLength && listItem.transform.childCount < totalCount || totalCount > maxLength && listItem.transform.childCount < maxLength) {
                GameObject objNew = NewItemAtEnd();
                float size = objNew != null ? GetSizeNewItemAtEnd(objNew.GetComponent<RectTransform>()) : 0;
                if (size > 0) {
                    changed = true;
                }
                if (listItem.transform.childCount > 15) {
                    currentFirst = -listItem.transform.GetChild(4).transform.localPosition.x;
                    currentLast = -listItem.transform.GetChild(11).transform.localPosition.x;
                    //Debug.LogError("listItem.transform.GetChild(4) " + listItem.transform.GetChild(4).transform.localPosition.x + " listItem.transform.GetChild(11) " + listItem.transform.GetChild(11).transform.localPosition.x);
                } else if (listItem.transform.childCount > 7) {
                    currentFirst = -listItem.transform.GetChild(2).transform.localPosition.x;
                    currentLast = -listItem.transform.GetChild(5).transform.localPosition.x;
                }
            } else if (totalCount > maxLength && listItem.transform.childCount == maxLength) {
                //Debug.LogError("currentFirst " + currentFirst + " currentLast " + currentLast + " listItem.transform " + listItem.transform.localPosition.x);
                if (listItem.transform.localPosition.x < currentLast && itemEnd < totalCount) {
                    GameObject objNew = NewItemAtEnd();
                    float size = objNew != null ? GetSizeNewItemAtEnd(objNew.GetComponent<RectTransform>()) : 0;
                    if (size > 0) {
                        changed = true;
                        GameObject objStart = DeleteItemAtStart();
                        float sizeStart = GetSizeDeleteItemAtStart(objStart.GetComponent<RectTransform>());
                        RemoveItem(objStart);
                        //Debug.LogError("ItemStart " + itemStart + " ItemEnd " + itemEnd);
                    }
                } else if (listItem.transform.localPosition.x > currentFirst && itemStart > 0) {
                    GameObject objNew = NewItemAtStart();
                    if (objNew != null)
                        objNew.transform.SetAsFirstSibling();
                    float size = objNew != null ? GetSizeItemAtStart(objNew.GetComponent<RectTransform>()) : 0;
                    if (size > 0) {
                        changed = true;
                        GameObject obj = DeleteItemAtEnd();
                        float sizeLast = GetSizeDeleteItemAtEnd(obj.GetComponent<RectTransform>());
                        RemoveItem(obj);
                        //Debug.LogError("ItemStart " + itemStart + " ItemEnd " + itemEnd);
                    }
                }
            }

            return changed;
        }

        protected LoopScrollRect() {
            flexibleWidth = -1;
        }

        public void ClearCells() {
            if (Application.isPlaying) {
                itemStart = 0;
                itemEnd = 0;
                totalCount = 0;
                for (int i = listItem.childCount - 1; i >= 0; i--) {
                    RemoveItem(listItem.GetChild(i).gameObject);
                    //Debug.LogError("Remove Item " + content.GetChild(i).name);
                }
            }
        }
        public void RefillCells(int startIdx = 0) {
            if (Application.isPlaying) {
                StopMovement();
                itemStart = reverseDirection ? totalCount - startIdx : startIdx;
                //Debug.LogError("reverseDirection " + reverseDirection + " itemTypeStart " + itemTypeStart);
                itemEnd = itemStart;
                int listItemCount = listItem.childCount;
                Canvas.ForceUpdateCanvases();
                if (listItemCount > 0) {
                    for (int i = 0; i < listItemCount; i++) {
                        //Debug.LogError("content.childCount " + content.childCount + " itemTypeEnd " + itemTypeEnd);
                        UpdateItemInfo(listItem.GetChild(i).gameObject, itemEnd);
                        itemEnd++;
                        //Debug.LogError("UpdateItemInfo " + content.GetChild(i).name);
                    }
                    //}

                    Canvas.ForceUpdateCanvases();
                    Vector2 pos = listItem.anchoredPosition;
                    if (moveTheoHuong == -1)
                        pos.y = 0;
                    else if (moveTheoHuong == 1)
                        pos.x = 0;
                    listItem.anchoredPosition = pos;
                    UpdateBounds();
                }
            }
        }

        IEnumerator WaitToRemoveItem() {
            yield return new WaitForSeconds(.1f);
        }

        protected GameObject NewItemAtStart() {
            itemStart--;
            GameObject objStart = InstantiateNextItem(itemStart);
            return objStart;
        }

        protected float GetSizeItemAtStart(RectTransform newItem) {

            float size = 0;
            size = Mathf.Max(GetSize(newItem), size);
            if (!reverseDirection) {
                Vector2 offset = GetVector(size);
                listItem.anchoredPosition += offset;
                m_PrevPosition += offset;
                m_ContentStartPosition += offset;
            }
            return size;
        }

        protected GameObject DeleteItemAtStart() {
            itemStart++;
            GameObject objStart = listItem.GetChild(0).gameObject;
            return objStart;
        }

        protected float GetSizeDeleteItemAtStart(RectTransform objStart) {

            float size = 0;

            size = Mathf.Max(GetSize(objStart), size);

            if (!reverseDirection) {
                Vector2 offset = GetVector(size);
                listItem.anchoredPosition -= offset;
                m_PrevPosition -= offset;
                m_ContentStartPosition -= offset;
            }
            return size;
        }

        protected GameObject NewItemAtEnd() {
            GameObject newItem = InstantiateNextItem(itemEnd);
            itemEnd++;
            return newItem;
        }

        protected float GetSizeNewItemAtEnd(RectTransform newItem) {

            float size = 0;


            size = Mathf.Max(GetSize(newItem), size);

            if (reverseDirection) {
                Vector2 offset = GetVector(size);
                listItem.anchoredPosition -= offset;
                m_PrevPosition -= offset;
                m_ContentStartPosition -= offset;
            }
            return size;
        }

        protected GameObject DeleteItemAtEnd() {
            itemEnd--;
            GameObject oldItem = listItem.GetChild(listItem.childCount - 1).gameObject;
            return oldItem;
        }

        protected float GetSizeDeleteItemAtEnd(RectTransform oldItem) {

            float size = 0;

            size = Mathf.Max(GetSize(oldItem), size);

            if (reverseDirection) {
                Vector2 offset = GetVector(size);
                listItem.anchoredPosition += offset;
                m_PrevPosition += offset;
                m_ContentStartPosition += offset;
            }
            return size;
        }

        private GameObject InstantiateNextItem(int itemIdx) {
            GameObject nextItem = GetItemToAdd();
            if (nextItem != null) {
                nextItem.transform.SetParent(listItem, false);
                nextItem.SetActive(true);
                UpdateItemInfo(nextItem, itemIdx);
            }
            return nextItem;
        }

        //TO HERE====================================================================================================================

        public virtual void OnScroll(PointerEventData data) {
            if (!IsActive())
                return;

            EnsureLayoutHasRebuilt();
            UpdateBounds();

            Vector2 delta = data.scrollDelta;
            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            if (scrollType == ScrollType.Vertical) {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    delta.y = delta.x;
                delta.x = 0;
            }
            if (scrollType == ScrollType.Horizontal) {
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                    delta.x = delta.y;
                delta.y = 0;
            }

            Vector2 position = m_ListItem.anchoredPosition;
            position += delta * m_ScrollSensitivity;
            if (m_MovementType == MovementType.Clamped)
                position += CalculateOffset(position - m_ListItem.anchoredPosition);

            SetContentAnchoredPosition(position);
            UpdateBounds();
        }

        public virtual void OnDrag(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            UpdateBounds();

            var pointerDelta = localCursor - m_PointerStartLocalCursor;
            Vector2 position = m_ContentStartPosition + pointerDelta;

            // Offset to get content into place in the view.
            Vector2 offset = CalculateOffset(position - m_ListItem.anchoredPosition);
            position += offset;
            if (m_MovementType == MovementType.Elastic) {
                if (offset.x != 0)
                    position.x = position.x - RubberDelta(offset.x, m_ViewBounds.size.x);
                if (offset.y != 0)
                    position.y = position.y - RubberDelta(offset.y, m_ViewBounds.size.y);
            }

            SetContentAnchoredPosition(position);
        }

        protected virtual void SetContentAnchoredPosition(Vector2 position) {
            if (scrollType == ScrollType.Vertical)
                position.x = m_ListItem.anchoredPosition.x;
            if (scrollType == ScrollType.Horizontal)
                position.y = m_ListItem.anchoredPosition.y;

            if (position != m_ListItem.anchoredPosition) {
                m_ListItem.anchoredPosition = position;
                UpdateBounds();
            }
        }

        protected virtual void LateUpdate() {
            if (!m_ListItem)
                return;

            EnsureLayoutHasRebuilt();
            //UpdateScrollbarVisibility();
            UpdateBounds();
            float deltaTime = Time.unscaledDeltaTime;
            Vector2 offset = CalculateOffset(Vector2.zero);
            if (!m_Dragging && (offset != Vector2.zero || m_Velocity != Vector2.zero)) {
                Vector2 position = m_ListItem.anchoredPosition;
                for (int axis = 0; axis < 2; axis++) {
                    // Apply spring physics if movement is elastic and content has an offset from the view.
                    if (m_MovementType == MovementType.Elastic && offset[axis] != 0) {
                        float speed = m_Velocity[axis];
                        position[axis] = Mathf.SmoothDamp(m_ListItem.anchoredPosition[axis], m_ListItem.anchoredPosition[axis] + offset[axis], ref speed, m_Elasticity, Mathf.Infinity, deltaTime);
                        m_Velocity[axis] = speed;
                    }
                    // Else move content according to velocity with deceleration applied.
                    else if (m_Inertia) {
                        m_Velocity[axis] *= Mathf.Pow(m_DecelerationRate, deltaTime);
                        if (Mathf.Abs(m_Velocity[axis]) < 1)
                            m_Velocity[axis] = 0;
                        position[axis] += m_Velocity[axis] * deltaTime;
                    }
                    // If we have neither elaticity or friction, there shouldn't be any velocity.
                    else {
                        m_Velocity[axis] = 0;
                    }
                }

                if (m_Velocity != Vector2.zero) {
                    if (m_MovementType == MovementType.Clamped) {
                        offset = CalculateOffset(position - m_ListItem.anchoredPosition);
                        position += offset;
                    }

                    SetContentAnchoredPosition(position);
                }
            }

            if (m_Dragging && m_Inertia) {
                Vector3 newVelocity = (m_ListItem.anchoredPosition - m_PrevPosition) / deltaTime;
                m_Velocity = Vector3.Lerp(m_Velocity, newVelocity, deltaTime * 10);
            }

            if (m_ViewBounds != m_PrevViewBounds || m_ContentBounds != m_PrevContentBounds || m_ListItem.anchoredPosition != m_PrevPosition) {
                //UpdateScrollbars(offset);
                m_OnValueChanged.Invoke(normalizedPosition);
                UpdatePrevData();
            }
        }

        private void UpdatePrevData() {
            if (m_ListItem == null)
                m_PrevPosition = Vector2.zero;
            else
                m_PrevPosition = m_ListItem.anchoredPosition;
            m_PrevViewBounds = m_ViewBounds;
            m_PrevContentBounds = m_ContentBounds;
        }


        public Vector2 normalizedPosition {
            get {
                return new Vector2(horizontalNormalizedPosition, verticalNormalizedPosition);
            }
            set {
                SetNormalizedPosition(value.x, 0);
                SetNormalizedPosition(value.y, 1);
            }
        }

        public float horizontalNormalizedPosition {
            get {
                UpdateBounds();
                if (m_ContentBounds.size.x <= m_ViewBounds.size.x)
                    return (m_ViewBounds.min.x > m_ContentBounds.min.x) ? 1 : 0;
                return (m_ViewBounds.min.x - m_ContentBounds.min.x) / (m_ContentBounds.size.x - m_ViewBounds.size.x);
            }
            set {
                SetNormalizedPosition(value, 0);
            }
        }

        public float verticalNormalizedPosition {
            get {
                UpdateBounds();
                if (m_ContentBounds.size.y <= m_ViewBounds.size.y)
                    return (m_ViewBounds.min.y > m_ContentBounds.min.y) ? 1 : 0;
                ;
                return (m_ViewBounds.min.y - m_ContentBounds.min.y) / (m_ContentBounds.size.y - m_ViewBounds.size.y);
            }
            set {
                SetNormalizedPosition(value, 1);
            }
        }

        private void SetHorizontalNormalizedPosition(float value) { SetNormalizedPosition(value, 0); }
        private void SetVerticalNormalizedPosition(float value) { SetNormalizedPosition(value, 1); }

        private void SetNormalizedPosition(float value, int axis) {
            EnsureLayoutHasRebuilt();
            UpdateBounds();
            // How much the content is larger than the view.
            float hiddenLength = m_ContentBounds.size[axis] - m_ViewBounds.size[axis];
            // Where the position of the lower left corner of the content bounds should be, in the space of the view.
            float contentBoundsMinPosition = m_ViewBounds.min[axis] - value * hiddenLength;
            // The new content localPosition, in the space of the view.
            float newLocalPosition = m_ListItem.localPosition[axis] + contentBoundsMinPosition - m_ContentBounds.min[axis];

            Vector3 localPosition = m_ListItem.localPosition;
            if (Mathf.Abs(localPosition[axis] - newLocalPosition) > 0.01f) {
                localPosition[axis] = newLocalPosition;
                m_ListItem.localPosition = localPosition;
                m_Velocity[axis] = 0;
                UpdateBounds();
            }
        }

        private static float RubberDelta(float overStretching, float viewSize) {
            return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
        }

        protected override void OnRectTransformDimensionsChange() {
            SetDirty();
        }

        private bool hScrollingNeeded {
            get {
                if (Application.isPlaying)
                    return m_ContentBounds.size.x > m_ViewBounds.size.x + 0.01f;
                return true;
            }
        }
        private bool vScrollingNeeded {
            get {
                if (Application.isPlaying)
                    return m_ContentBounds.size.y > m_ViewBounds.size.y + 0.01f;
                return true;
            }
        }

        public virtual float flexibleWidth { get; private set; }

        //void UpdateScrollbarVisibility()
        //{
        //    if (m_VerticalScrollbar && m_VerticalScrollbarVisibility != ScrollbarVisibility.Permanent && m_VerticalScrollbar.gameObject.activeSelf != vScrollingNeeded)
        //        m_VerticalScrollbar.gameObject.SetActive(vScrollingNeeded);

        //    if (m_HorizontalScrollbar && m_HorizontalScrollbarVisibility != ScrollbarVisibility.Permanent && m_HorizontalScrollbar.gameObject.activeSelf != hScrollingNeeded)
        //        m_HorizontalScrollbar.gameObject.SetActive(hScrollingNeeded);
        //}

        //void UpdateScrollbarLayout()
        //{
        //    if (m_VSliderExpand && m_HorizontalScrollbar)
        //    {
        //        m_Tracker.Add(this, m_HorizontalScrollbarRect,
        //                      DrivenTransformProperties.AnchorMinX |
        //                      DrivenTransformProperties.AnchorMaxX |
        //                      DrivenTransformProperties.SizeDeltaX |
        //                      DrivenTransformProperties.AnchoredPositionX);
        //        m_HorizontalScrollbarRect.anchorMin = new Vector2(0, m_HorizontalScrollbarRect.anchorMin.y);
        //        m_HorizontalScrollbarRect.anchorMax = new Vector2(1, m_HorizontalScrollbarRect.anchorMax.y);
        //        m_HorizontalScrollbarRect.anchoredPosition = new Vector2(0, m_HorizontalScrollbarRect.anchoredPosition.y);
        //        if (vScrollingNeeded)
        //            m_HorizontalScrollbarRect.sizeDelta = new Vector2(-(m_VSliderWidth + m_VerticalScrollbarSpacing), m_HorizontalScrollbarRect.sizeDelta.y);
        //        else
        //            m_HorizontalScrollbarRect.sizeDelta = new Vector2(0, m_HorizontalScrollbarRect.sizeDelta.y);
        //    }

        //    if (m_HSliderExpand && m_VerticalScrollbar)
        //    {
        //        m_Tracker.Add(this, m_VerticalScrollbarRect,
        //                      DrivenTransformProperties.AnchorMinY |
        //                      DrivenTransformProperties.AnchorMaxY |
        //                      DrivenTransformProperties.SizeDeltaY |
        //                      DrivenTransformProperties.AnchoredPositionY);
        //        m_VerticalScrollbarRect.anchorMin = new Vector2(m_VerticalScrollbarRect.anchorMin.x, 0);
        //        m_VerticalScrollbarRect.anchorMax = new Vector2(m_VerticalScrollbarRect.anchorMax.x, 1);
        //        m_VerticalScrollbarRect.anchoredPosition = new Vector2(m_VerticalScrollbarRect.anchoredPosition.x, 0);
        //        if (hScrollingNeeded)
        //            m_VerticalScrollbarRect.sizeDelta = new Vector2(m_VerticalScrollbarRect.sizeDelta.x, -(m_HSliderHeight + m_HorizontalScrollbarSpacing));
        //        else
        //            m_VerticalScrollbarRect.sizeDelta = new Vector2(m_VerticalScrollbarRect.sizeDelta.x, 0);
        //    }
        //}

        private void UpdateBounds(bool updateItems = true) {
            m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            m_ContentBounds = GetBounds();

            if (m_ListItem == null)
                return;

            // Don't do this in Rebuild
            if (Application.isPlaying && updateItems && UpdateItems(m_ViewBounds, m_ContentBounds)) {
                Canvas.ForceUpdateCanvases();
                m_ContentBounds = GetBounds();
            }

            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 contentSize = m_ContentBounds.size;
            Vector3 contentPos = m_ContentBounds.center;
            Vector3 excess = m_ViewBounds.size - contentSize;
            if (excess.x > 0) {
                contentPos.x -= excess.x * (m_ListItem.pivot.x - 0.5f);
                contentSize.x = m_ViewBounds.size.x;
            }
            if (excess.y > 0) {
                contentPos.y -= excess.y * (m_ListItem.pivot.y - 0.5f);
                contentSize.y = m_ViewBounds.size.y;
            }

            m_ContentBounds.size = contentSize;
            m_ContentBounds.center = contentPos;
        }

        private readonly Vector3[] m_Corners = new Vector3[4];
        private Bounds GetBounds() {
            if (m_ListItem == null)
                return new Bounds();

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = viewRect.worldToLocalMatrix;
            m_ListItem.GetWorldCorners(m_Corners);
            for (int j = 0; j < 4; j++) {
                Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        private Vector2 CalculateOffset(Vector2 delta) {
            Vector2 offset = Vector2.zero;
            if (m_MovementType == MovementType.Unrestricted)
                return offset;

            Vector2 min = m_ContentBounds.min;
            Vector2 max = m_ContentBounds.max;

            if (scrollType == ScrollType.Horizontal) {
                min.x += delta.x;
                max.x += delta.x;
                if (min.x > m_ViewBounds.min.x)
                    offset.x = m_ViewBounds.min.x - min.x;
                else if (max.x < m_ViewBounds.max.x)
                    offset.x = m_ViewBounds.max.x - max.x;
            }

            if (scrollType == ScrollType.Vertical) {
                min.y += delta.y;
                max.y += delta.y;
                if (max.y < m_ViewBounds.max.y)
                    offset.y = m_ViewBounds.max.y - max.y;
                else if (min.y > m_ViewBounds.min.y)
                    offset.y = m_ViewBounds.min.y - min.y;
            }

            return offset;
        }

        protected void SetDirty() {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        protected void SetDirtyCaching() {
            if (!IsActive())
                return;

            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        public abstract GameObject GetItemToAdd();

        public abstract void RemoveItem(GameObject go);

        public abstract void UpdateItemInfo(GameObject item, int index);

#if UNITY_EDITOR
        protected override void OnValidate() {
            SetDirtyCaching();
        }
#endif
        #region Rebuild
        public virtual void Rebuild(CanvasUpdate executing) {
            //    if (executing == CanvasUpdate.Prelayout)
            //    {
            //        //UpdateCachedData();
            //    }

            //    if (executing == CanvasUpdate.PostLayout)
            //    {
            //        UpdateBounds(false);
            //        UpdateScrollbars(Vector2.zero);
            //        UpdatePrevData();

            //        m_HasRebuiltLayout = true;
            //    }
        }
        #endregion

        public virtual void LayoutComplete() { }

        public virtual void GraphicUpdateComplete() { }
        #region UpdateScrollBar
        /*
        
        void UpdateScrollBar()
        {
            Transform transform = this.transform;
            m_HorizontalScrollbarRect = m_HorizontalScrollbar == null ? null : m_HorizontalScrollbar.transform as RectTransform;
            m_VerticalScrollbarRect = m_VerticalScrollbar == null ? null : m_VerticalScrollbar.transform as RectTransform;

             These are true if either the elements are children, or they don't exist at all.
            bool viewIsChild = (viewRect.parent == transform);
            bool hScrollbarIsChild = (!m_HorizontalScrollbarRect || m_HorizontalScrollbarRect.parent == transform);
            bool vScrollbarIsChild = (!m_VerticalScrollbarRect || m_VerticalScrollbarRect.parent == transform);
            bool allAreChildren = (viewIsChild && hScrollbarIsChild && vScrollbarIsChild);

            m_HSliderExpand = allAreChildren && m_HorizontalScrollbarRect && horizontalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport;
            m_VSliderExpand = allAreChildren && m_VerticalScrollbarRect && verticalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport;
            m_HSliderHeight = (m_HorizontalScrollbarRect == null ? 0 : m_HorizontalScrollbarRect.rect.height);
            m_VSliderWidth = (m_VerticalScrollbarRect == null ? 0 : m_VerticalScrollbarRect.rect.width);
        }
        */

        #region UpdateScrollbars
        //private void UpdateScrollbars(Vector2 offset)
        //{
        //    if (m_HorizontalScrollbar)
        //    {
        //        if (m_ContentBounds.size.x > 0)
        //            m_HorizontalScrollbar.size = Mathf.Clamp01((m_ViewBounds.size.x - Mathf.Abs(offset.x)) / m_ContentBounds.size.x);
        //        else
        //            m_HorizontalScrollbar.size = 1;

        //        m_HorizontalScrollbar.value = horizontalNormalizedPosition;
        //    }

        //    if (m_VerticalScrollbar)
        //    {
        //        if (m_ContentBounds.size.y > 0)
        //            m_VerticalScrollbar.size = Mathf.Clamp01((m_ViewBounds.size.y - Mathf.Abs(offset.y)) / m_ContentBounds.size.y);
        //        else
        //            m_VerticalScrollbar.size = 1;

        //        m_VerticalScrollbar.value = verticalNormalizedPosition;
        //    }
        //}
        #endregion
        #endregion
        protected override void OnEnable() {
            base.OnEnable();

            //if (m_HorizontalScrollbar)
            //    m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
            //if (m_VerticalScrollbar)
            //    m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);

            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

        protected override void OnDisable() {
            CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);

            //if (m_HorizontalScrollbar)
            //    m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
            //if (m_VerticalScrollbar)
            //    m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);

            m_HasRebuiltLayout = false;
            m_Tracker.Clear();
            m_Velocity = Vector2.zero;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        public override bool IsActive() {
            return base.IsActive() && m_ListItem != null;
        }

        private void EnsureLayoutHasRebuilt() {
            if (!m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
                Canvas.ForceUpdateCanvases();
        }

        public virtual void StopMovement() {
            m_Velocity = Vector2.zero;
        }

        public virtual void OnBeginDrag(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            UpdateBounds();

            m_PointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
            m_ContentStartPosition = m_ListItem.anchoredPosition;
            m_Dragging = true;
        }

        public virtual void OnEndDrag(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_Dragging = false;
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_Velocity = Vector2.zero;
        }
    }
}