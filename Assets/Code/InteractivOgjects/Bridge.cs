using Assets.Code.CollectableObject;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.InteractivOgjects
{
    class Bridge : MonoBehaviour
    {
        [SerializeField] private Transform startPlace;
        [SerializeField] private BridgeTriggerPrefab bridgeTriggerPrefab;
        [SerializeField] private Transform palletPrefab;
        [SerializeField] int stepsCount = 10;
        [SerializeField] Transform border;

        private List<Transform> triggersPositions;
        private int index = 0;

        private void Awake()
        {
            triggersPositions = new List<Transform>();
        }

        private void Start()
        {
            Vector3 currentPosition = startPlace.position;

            for (int i = 0; i < stepsCount; i++)
            {
                var trigger = Instantiate(bridgeTriggerPrefab.gameObject, transform).GetComponent<BridgeTriggerPrefab>();
                trigger.transform.position = currentPosition;
                trigger.Bridge = this;

                triggersPositions.Add(trigger.transform);

                currentPosition += new Vector3(0f, palletPrefab.localScale.y, palletPrefab.localScale.z);
            }

            border.position = triggersPositions[0].position;
        }

        public void MoveBorder()
        {
            border.position += (Vector3.forward * palletPrefab.localScale.z) + (Vector3.up * palletPrefab.localScale.y);
            index++;
            if (index >= stepsCount)
                border.gameObject.SetActive(false);
        }
    }
}
