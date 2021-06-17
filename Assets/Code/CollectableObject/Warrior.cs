using System;
using UnityEngine;

namespace Assets.Code.CollectableObject
{
    public class Warrior : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        [SerializeField] private new Collider collider;
        public Animator animator;

        public Color MyColor
        {
            get => renderer.material.color;

            set => renderer.material.color = value;
        }
        private void Awake()
        {
            
        }

        public Warlord Warlord { get; private set; }

        public bool IsFree => Warlord == null;

        public void Assign(Warlord warlord)
        {
            Warlord = warlord;
            collider.enabled = false;
            MyColor = warlord.MyColor;
        }

        public void Free()
        {
            Warlord = null;
            collider.enabled = true;
        }

        public void Kill()
        {
            Warlord.Remove(this);

            Destroy(this.gameObject);
        }
    }
}
