using Assets.Code.CollectableObject;
using UnityEngine;

namespace Assets.Code.InteractivOgjects
{
    class Bridge : MonoBehaviour
    {
        [SerializeField] private Transform startPlace;
        [SerializeField] private BridgeTriggerPrefab bridgeTriggerPrefab;
        [SerializeField] private Transform palletPrefab;
        [SerializeField] int stepsCount = 10;
        private void Start()
        {
            Vector3 currentPosition = startPlace.position;

            for (int i = 0; i < stepsCount; i++)
            {
                var trigger = Instantiate(bridgeTriggerPrefab.gameObject, transform);

                trigger.transform.position = currentPosition;

                currentPosition += new Vector3(0f, palletPrefab.localScale.y, palletPrefab.localScale.z);
            }
        }
    }
}
