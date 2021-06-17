using Assets.Code.CollectableObject;
using Assets.Code.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    public class Warlord : MonoBehaviour
    {
        [Header("Collect settings")]
        [SerializeField] private float radiusDelta = 1f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float spawnRadius = 1f;
        [SerializeField] private float spawnRadiusToAttackRadiusOffset = 0.5f;
        [SerializeField] private Renderer colorHolder;

        public List<Warrior> warriors;
  

        public Color MyColor
        {
            get => colorHolder.material.color;

            set
            {
                colorHolder.material.color = value;
            }
        }

        public int AmountOfWarriors => warriors.Count;

        public float AttackRadius => spawnRadius + spawnRadiusToAttackRadiusOffset;

        private void Awake()
        {
            warriors = new List<Warrior>();
        }

        public void Init(Color color)
        {
            MyColor = color;
        }

        public void Fight(Warlord otherWarlord)
        {
            if (AmountOfWarriors > otherWarlord.AmountOfWarriors)
            {
                Fight(this, otherWarlord);
            }
            else
            {
                Fight(otherWarlord, this);
            }
        }

        private void Fight(Warlord winner, Warlord looser)
        {
            if (looser.AmountOfWarriors > 0)
            {
                var deadWarriors = looser.warriors.Where(w => Vector3.Distance(w.transform.position, winner.transform.position) <= AttackRadius).ToList();
                foreach (var deadWarrior in deadWarriors)
                {
                    deadWarrior.Kill();
                }
            }
            else if (winner.AmountOfWarriors > 0)
            {
                if (Vector3.Distance(looser.transform.position, winner.transform.position) <= AttackRadius)
                {
                    if (looser is Player)
                    {
                        GameManager.Instance.Lose();
                    }
                        looser.Kill();
                }
            }
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public void Remove(Warrior warrior)
        {
            warriors.Remove(warrior);
            warrior.Free();
        }

        public void Add(Warrior warrior)
        {
            warriors.Add(warrior);
            warrior.Assign(this);

            warrior.transform.SetParent(transform);

            var attempts = 100;

            while (true)
            {
                var rndAngle = Random.Range(0f, 2f * Mathf.PI);
                var candidatePosition = new Vector3()
                {
                    x = Mathf.Cos(rndAngle) * spawnRadius,
                    y = warrior.transform.position.y,
                    z = Mathf.Sin(rndAngle) * spawnRadius
                };

                if (CheckValidDistance(candidatePosition))
                {
                    warrior.transform.localPosition = candidatePosition;
                    break;
                }

                attempts -= 1;
                if (attempts <= 0)
                {
                    spawnRadius += radiusDelta;
                    attempts = 10;
                }
            }
        }

        private bool CheckValidDistance(Vector3 candidatePosition)
        {
            var warriorsPositions = warriors.Select(w => w.transform.localPosition).ToList();
            foreach (var position in warriorsPositions)
            {
                if (Vector3.Distance(candidatePosition, position) <= minDistance)
                {
                    return false;
                }
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.4f, AttackRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.4f, spawnRadius);
        }
    }
}