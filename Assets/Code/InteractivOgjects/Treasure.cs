using Assets.Code.Entities;
using Assets.Code.UI;
using UnityEngine;

namespace Assets.Code.InteractivOgjects
{
    class Treasure :MonoBehaviour
    {
        [SerializeField] private RectTransform trackerContainer;
        [SerializeField] private GameObject trackerPrefab;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out ICanDig canDig))
            {
                var tracker = Instantiate(trackerPrefab, trackerContainer);

                tracker.GetComponent<UITracker>().Target = canDig.Transform;
                tracker.GetComponent<DigProgressTracker>().Target = canDig;

                canDig.Dig();
             
                if (canDig is Player)
                    InputCatcher.Instance.IsDigging = true;
            }

        }
    }
}
