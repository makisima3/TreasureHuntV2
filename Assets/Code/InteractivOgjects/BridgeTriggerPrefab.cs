using UnityEngine;

namespace Assets.Code.InteractivOgjects
{
    class BridgeTriggerPrefab : MonoBehaviour
    {
        [SerializeField] private bool isEmpty = true;

        public Bridge Bridge { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICollector>(out var collector))
            {
                if (isEmpty)
                {
                    if (collector.PlacePallet(transform, transform))
                    {
                        Bridge.MoveBorder();
                        isEmpty = false;
                    }
                }
            }
        }
    }
}
