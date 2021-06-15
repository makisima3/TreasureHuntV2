using UnityEngine;

namespace Assets.Code.CollectableObject
{
    public class MapPart : MonoBehaviour
    {
        [HideInInspector] public bool IsTaked = false;

        private Color myColor;

        public Color MyColor
        {
            get => myColor;

            set
            {
                GetComponent<MeshRenderer>().material.color = value;
                myColor = value;
            }
        }
        private void Awake()
        {
            myColor = GetComponent<MeshRenderer>().material.color;
        }
    }
}
