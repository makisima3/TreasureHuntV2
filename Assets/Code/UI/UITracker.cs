using UnityEngine;

namespace Assets.Code.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UITracker : MonoBehaviour
    {

        [SerializeField] private Transform target;

        [SerializeField] private Vector2 positionOffset;

        [SerializeField] private bool alwaysUpdate;

        private Camera _camera;

        private new RectTransform transform;
        public Transform Target
        {
            get => target;

            set => target = value;
        }

        private void Awake()
        {
            _camera = Camera.main;

            transform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if(alwaysUpdate)
            {
                UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            var screenPosition = _camera.WorldToScreenPoint(target.position);

            transform.position = new Vector2(screenPosition.x,screenPosition.y) + positionOffset;
        }
    }
}