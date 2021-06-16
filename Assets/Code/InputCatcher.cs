using Assets.Code.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    class InputCatcher : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public static InputCatcher Instance;

        [HideInInspector]
        public OnMove onMove;

        [HideInInspector]
        public OnStop onStop;

        [SerializeField]
        private float deadZone = 10f;

        [SerializeField, Range(0f, 1f)]
        private float maxRadiusWidthPercent = 0.5f;


        private new RectTransform transform;
        private Vector2 startTouchPosition;
        private float maxRadiusPx;

        public bool IsDigging { get; set; }

        private void Awake()
        {
            Instance = this;

            transform = GetComponent<RectTransform>();
            maxRadiusPx = maxRadiusWidthPercent * transform.rect.width / 2f;

            onMove = new OnMove();
            onStop = new OnStop();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsDigging)
                return;

            startTouchPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!eventData.dragging || IsDigging)
                return;
            //if (eventData.delta.magnitude > deadZone)
            //onMove.Invoke(eventData.delta.normalized);

            var delta = eventData.position - startTouchPosition;

            if (delta.magnitude > deadZone)
            {
                onMove.Invoke(delta.normalized, Mathf.Clamp(delta.magnitude, deadZone, maxRadiusPx) / maxRadiusPx);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!eventData.dragging || IsDigging)
                return;

            onStop.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsDigging)
                Player.Instance.Dig();
        }
    }
}
