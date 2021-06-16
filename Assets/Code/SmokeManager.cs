using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    class SmokeManager : MonoBehaviour
    {
        public static SmokeManager Instance;

        [SerializeField] private List<GameObject> smokes;

        private void Awake()
        {
            Instance = this;
        }

        public void RemoveSmoke()
        {
            var smoke = smokes.FirstOrDefault();
            smoke.GetComponent<Collider>().isTrigger = true;
            smokes.Remove(smoke);

            StartCoroutine(SmokeDissolve(smoke));
        }

        private IEnumerator SmokeDissolve(GameObject smoke)
        {
            var mat = smoke.GetComponent<MeshRenderer>().material;

            var color = mat.color;

            for (float i = 1; i >= 0; i -= 0.1f)
            {
                color.a = i;
                mat.color = color;

                yield return new WaitForSeconds(0.1f);
            }            
        }
    }
}
