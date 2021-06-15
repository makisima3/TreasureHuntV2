using UnityEngine;

namespace Assets.Code.InteractivOgjects
{
    class BridgeTriggerPrefab : MonoBehaviour
    {
        private bool isEmpty = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICollector>(out var collector))
            {
                if (isEmpty)
                {
                    collector.PlacePallet(transform, transform);
                    isEmpty = false;
                }
            }
        }
    }
}
