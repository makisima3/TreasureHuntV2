using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Victory()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Lose()
        {

        }
    }
}