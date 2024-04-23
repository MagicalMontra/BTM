using DG.Tweening;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class AdBanner : MonoBehaviour
    {
        [SerializeField]
        private float duration = 4f;
        
        [SerializeField]
        private float moveAmount = 0.5f;

        private Vector3 originalPos;
        private Sequence sequence;

        public void Initialize(Vector3 position)
        {
            transform.position = position;
            originalPos = transform.position;
            var upPos = originalPos.y + moveAmount;
            var downPos = originalPos.y - moveAmount;
            transform.position = new Vector3(0, upPos, 0);
            sequence?.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(downPos, duration));
            sequence.Append(transform.DOMoveY(upPos, duration));
            sequence.SetLoops(-1);
            sequence.Play();
        }

        public void SetActive(bool isActive)
        {
            var upPos = originalPos.y + moveAmount;
            var downPos = originalPos.y - moveAmount;
            transform.position = new Vector3(0, upPos, 0);
            sequence?.Kill();
            gameObject.SetActive(isActive);
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(downPos, duration));
            sequence.Append(transform.DOMoveY(upPos, duration));
            sequence.SetLoops(-1);
            sequence.Play();
        }

        public void Dispose()
        {
            sequence?.Kill();
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<AdBanner, AdBanner>
        {
            
        }
    }
}