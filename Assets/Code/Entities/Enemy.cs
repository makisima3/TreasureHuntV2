using Assets.Code.CollectableObject;
using Assets.Code.InteractivOgjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.Entities
{
    [RequireComponent(typeof(Warlord))]
    public class Enemy : MonoBehaviour, ICollector, ICanDig
    {
        [InspectorName("Pallets")]
        [SerializeField] private Transform palletPlace;
        [SerializeField] private float palletOffset = 0.5f;

        [SerializeField] private float speed;
        [SerializeField] private new Renderer renderer;

        [SerializeField] private float digTime = 10f;

        [SerializeField] private Player player;
        [SerializeField] private NavMeshAgent agent;

        public Animator animator;
        public bool IsFight = false;
        private List<Pallet> myPallets;
        private List<MapPart> myMapParts;

        private Warlord warlord;

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

        public bool isDigging = false;

        private List<Pallet> FindedPallets;
        private List<MapPart> FindedMapParts;
        private Transform treasure;
        private Transform currentTarget;

        private void Awake()
        {
            warlord = GetComponent<Warlord>();
            warlord.Init(MyColor);
        }

        private void Start()
        {
            animator.SetTrigger("run");
            myPallets = new List<Pallet>();
            myMapParts = new List<MapPart>();

            FindedPallets = FindObjectsOfType<Pallet>().Where(p => p.MyColor == MyColor).ToList();
            FindedMapParts = FindObjectsOfType<MapPart>().Where(p => p.MyColor == MyColor).ToList();
            treasure = FindObjectOfType<Treasure>().gameObject.transform;
        }

        private void Update()
        {


            float distance = Vector3.Distance(transform.position, player.transform.position);

            if(distance < player.Warlord.AttackRadius + warlord.AttackRadius)
            {
                if(!player.isDigging && !isDigging)
                {
                    warlord.Fight(player.Warlord);
                }
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                SearchTarget();
            }
        }

        private void SearchTarget()
        {

            if (FindedPallets.Count > 0)
            {
                Pallet target = FindedPallets.FirstOrDefault();

                if (Vector3.Distance(transform.position, target.transform.position) > 1)
                {
                    GoToCollect(target.gameObject);
                    currentTarget = target.transform;
                }
                FindedPallets.Remove(target);
            }
            else if (FindedMapParts.Count > 0)
            {
                MapPart target = FindedMapParts.FirstOrDefault();

                GoToCollect(target.gameObject);
                currentTarget = target.transform;
                FindedMapParts.Remove(target);
            }
            else
            {
                GoToCollect(treasure.gameObject);
                currentTarget = treasure.transform;
            }

        }

        private void GoToCollect(GameObject target)
        {
            agent.SetDestination(target.transform.position);
        }

        public void CollectPallet(Pallet pallet)
        {
            pallet.transform.position = palletPlace.position + (Vector3.up * palletOffset * myPallets.Count);
            pallet.transform.rotation = transform.rotation;
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
            FindedMapParts.Remove(mapPart);
            Destroy(mapPart.gameObject);
        }

        public void CollectWarrior(Warrior warrior)
        {
            warrior.animator.SetTrigger("run");
            warlord.Add(warrior);
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

        public void Dig()
        {
            isDigging = true;

            animator.SetTrigger("dig");
            warlord.warriors.ForEach(w => w.animator.SetTrigger("dig"));

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

            GameManager.Instance.Lose();
        }
    }
}
