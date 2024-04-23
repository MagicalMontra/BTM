using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class HeartIcon : MonoBehaviour
    {
        [SerializeField]
        private Image heartImage;

        [SerializeField]
        private GameObject loseHeartFX;
        
        [SerializeField]
        private GameObject gainHeartFX;

        public void Enable(bool status)
        {
            heartImage.gameObject.SetActive(status);
        }

        public void OnHeartGained()
        {
            Enable(true);
            loseHeartFX.SetActive(false);
            gainHeartFX.SetActive(true);
        }
        
        public void OnHeartLost()
        {
            gainHeartFX.SetActive(false);
            loseHeartFX.SetActive(true);
            Enable(false);
        }

        public void Reinitialize()
        {
            gainHeartFX.SetActive(false);
            loseHeartFX.SetActive(false);
        }

        public class Pool : MonoMemoryPool<Transform, HeartIcon>
        {
            protected override void Reinitialize(Transform parent, HeartIcon item)
            {
                item.transform.SetParent(parent);
            }
        }
    }
}