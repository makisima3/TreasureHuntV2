using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    [RequireComponent(typeof(Image))]
    public class DigProgressTracker : MonoBehaviour
    {

        [SerializeField] private ICanDig target;


        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public ICanDig Target
        {
            get => target;

            set
            {
                target = value;

                _image.color = target.MyColor;
            }
        }

        private void Update()
        {
            _image.fillAmount = target.DigProgress;
        }
    }
}