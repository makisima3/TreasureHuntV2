using UnityEngine;

namespace Assets.Code.CollectableObject
{
    public class Pallet : MonoBehaviour 
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

        private Collider _collider;

        public Collider Collider => _collider;

        private void Awake()
        {
            myColor = GetComponent<MeshRenderer>().material.color;

            _collider = GetComponent<Collider>();
        }
    }
}
