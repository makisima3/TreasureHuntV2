using Assets.Code.CollectableObject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Entities
{
    class Enemy : MonoBehaviour, ICollector, ICanDig
    {
        [InspectorName("Pallets")]
        [SerializeField] private Transform palletPlace;
        [SerializeField] private float palletOffset = 0.5f;

        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed;
        [SerializeField] private GameObject colorHolder;

        [SerializeField] private float digTime = 10f;

        public Animator animator;
        private List<Pallet> myPallets;
        private List<MapPart> myMapParts;

        private Color myColor;

        public Color MyColor
        {
            get => myColor;

            set
            {
                colorHolder.GetComponent<SkinnedMeshRenderer>().material.color = value;
                myColor = value;
            }
        }

        public float DigProgress { get; private set; }

        public Transform Transform => transform;

        private void Awake()
        {
            myColor = colorHolder.GetComponent<SkinnedMeshRenderer>().material.color;
        }

        private void Start()
        {
            myPallets = new List<Pallet>();
            myMapParts = new List<MapPart>();
        }

        public void CollectPallet(Pallet pallet)
        {
            pallet.transform.position = palletPlace.position + (Vector3.up * palletOffset * myPallets.Count);
            pallet.IsTaked = true;
            pallet.MyColor = myColor;
            pallet.transform.SetParent(transform);

            myPallets.Add(pallet);
        }

        public void PlacePallet(Transform place, Transform parent)
        {
            Pallet pallet = myPallets.Last();

            myPallets.Remove(pallet);
            pallet.Collider.isTrigger = false;
            pallet.transform.position = place.position;
            pallet.transform.rotation = Quaternion.identity;
            pallet.transform.SetParent(parent);
        }

        public void CollectMap(MapPart mapPart)
        {
            throw new System.NotImplementedException();
        }

        public void CollectWarrior(Warrior warrior)
        {
            throw new System.NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Pallet pallet))
            {
                if (pallet.MyColor == myColor && !pallet.IsTaked)
                    CollectPallet(pallet);
            }
            else if (other.TryGetComponent(out MapPart mapPart))
            {
                CollectMap(mapPart);
            }
            else if (other.TryGetComponent(out Warrior warrior))
            {
                CollectWarrior(warrior);
            }
        }

        public void Dig()
        {
            animator.SetTrigger("dig");

            StartCoroutine(DigDelay());
        }

        private IEnumerator DigDelay()
        {
            var delta = 1f / digTime;

            while (DigProgress < 1)
            {
                DigProgress += delta;

                yield return new WaitForSeconds(delta);
            }
        }
    }
}
