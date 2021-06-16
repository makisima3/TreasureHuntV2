using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.InteractivOgjects
{
    class Treasure :MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Player player))
            {
                player.DigStart();
            }
            else if(other.TryGetComponent(out Enemy enemy))
            {

            }
        }
    }
}
