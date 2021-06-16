using Assets.Code.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    class InputCatcher : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerClickHandler
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

        private bool IsDigging = false;


        private void Awake()
        {
            Instance = this;

            onMove = new OnMove();
            onStop = new OnStop();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsDigging)
                return;

            startTouchPosition = eventData.position;

            transform = GetComponent<RectTransform>();
            maxRadiusPx = maxRadiusWidthPercent * transform.rect.width / 2f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsDigging)
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
            if (IsDigging)
                return;

            onStop.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnDigStart()
        {
            IsDigging = transform;
        }

    }
}
