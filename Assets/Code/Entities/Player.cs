using Assets.Code.CollectableObject;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Entities
{
    class Player : MonoBehaviour, ICollector
    {
        [InspectorName("Pallets")]
        [SerializeField] private Transform palletPlace;
        [SerializeField] private float palletOffset = 0.5f;

        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed;
        [SerializeField] private GameObject colorHolder;

        [SerializeField] private bool isMove;

        public Animator animator;
        private Vector3 moveVector;
        private float gravity = -0.1f;
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


        private void Awake()
        {
            myColor = colorHolder.GetComponent<SkinnedMeshRenderer>().material.color;

            InputCatcher.Instance.onMove.AddListener(Move);
            InputCatcher.Instance.onStop.AddListener(Stop);
        }

        private void Start()
        {
            myPallets = new List<Pallet>();
            myMapParts = new List<MapPart>();
        }

        private void Update()
        {
            if (isMove)
            {
                controller.Move(moveVector);
            }
            else
            {
                controller.Move(Vector3.up * gravity);
            }
        }

        public void Move(Vector2 direction, float speedMultiplier)
        {
            if (!isMove)
            {
                animator.SetTrigger("run");
            }
            isMove = true;

            moveVector = new Vector3(direction.x, 0f, direction.y) * speed * 0.1f * speedMultiplier;
            moveVector.y = gravity;

        }

        public void Stop()
        {

            animator.SetTrigger("idle");
            isMove = false;
            moveVector = Vector3.zero;
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
    }
}
