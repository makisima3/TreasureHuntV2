using System.Collections;
using UnityEngine;

namespace Assets.Code
{
    public class DeathEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private MeshRenderer quad;

        public void Init(Color color)
        {
            particleSystem.startColor = color;
            sprite.color = color;
            quad.material.color = color;
        }
    }
}