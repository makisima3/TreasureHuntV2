using Assets.Code.CollectableObject;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Entities
{
    [RequireComponent(typeof(Warlord))]
    public class Player : MonoBehaviour, ICollector, ICanDig
    {
        public static Player Instance { get; private set; }

        [InspectorName("Pallets")]
        [SerializeField] private Transform palletPlace;
        [SerializeField] private float palletOffset = 0.5f;

        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed;

        [SerializeField] private new Renderer renderer;
        [SerializeField] private bool isMove;

        [SerializeField] private int digTapCount = 10;

        [SerializeField] private Warlord warlord;

        public bool isDigging = false;
        public Animator animator;
        private Vector3 moveVector;
        private float gravity = -0.1f;
        private List<Pallet> myPallets;

        public Warlord Warlord => warlord;

        public Color MyColor
        {
            get => renderer.material.color;

            set
            {
                renderer.material.color = value;
            }
        }

        public float DigProgress { get; private set; }

        public Transform Transform => transform;

        private void Awake()
        {
            Instance = this;

            warlord = GetComponent<Warlord>();
            warlord.Init(MyColor);
            InputCatcher.Instance.onMove.AddListener(Move);
            InputCatcher.Instance.onStop.AddListener(Stop);
        }

        private void Start()
        {
            myPallets = new List<Pallet>();
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
                warlord.warriors.ForEach(w => w.animator.SetTrigger("run"));
            }
            isMove = true;

            moveVector = new Vector3(direction.x, 0f, direction.y) * speed * 0.1f * speedMultiplier;
            moveVector.y = gravity;

        }

        public void Stop()
        {
            animator.SetTrigger("idle");
            warlord.warriors.ForEach(w => w.animator.SetTrigger("idle"));
            isMove = false;
            moveVector = Vector3.zero;
        }

        public void CollectPallet(Pallet pallet)
        {
            pallet.transform.position = palletPlace.position + (Vector3.up * palletOffset * myPallets.Count);
            pallet.IsTaked = true;
            pallet.MyColor = MyColor;
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
            Destroy(mapPart.gameObject);

            SmokeManager.Instance.RemoveSmoke();
        }

        public void CollectWarrior(Warrior warrior)
        {
            warlord.Add(warrior);
            warrior.animator.SetTrigger("run");
        }

        public void Dig()
        {
            isDigging = true;

            if (DigProgress <= 0)
            {
                animator.SetTrigger("dig");
                warlord.warriors.ForEach(w => w.animator.SetTrigger("dig"));

                moveVector = Vector3.zero;
            }

            DigProgress += 1f / digTapCount;

            if (DigProgress >= 1)
            {
                GameManager.Instance.Victory();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Pallet pallet))
            {
                if (pallet.MyColor == MyColor && !pallet.IsTaked)
                    CollectPallet(pallet);
            }
            else if (other.TryGetComponent(out MapPart mapPart))
            {
                if (mapPart.MyColor == MyColor)
                    CollectMap(mapPart);
            }
            else if (other.TryGetComponent(out Warrior warrior))
            {
                CollectWarrior(warrior);
            }
        }
    }
}
