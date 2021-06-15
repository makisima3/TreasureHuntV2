using UnityEngine;

namespace Assets.Code.CollectableObject
{
    public class Warrior : MonoBehaviour
    {
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
