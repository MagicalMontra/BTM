#nullable enable
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class Shadow : MonoBehaviour
    {
        [Inject]
        private ShadowConfig config;
        
        [SerializeField]
        private SpriteRenderer spriteRenderer = default!;

        private Transform? reference;

        private void Reinitialize(Transform reference, Vector3 initPosition)
        {
            this.reference = reference;
            transform.position = initPosition;
        }

        private void Update()
        {
            UpdatePosition();
            UpdateScaling();
            UpdateAlpha();
        }

        private void UpdatePosition()
        {
            if (reference == null)
            {
                return;
            }

            transform.position = new Vector3(reference.position.x, transform.position.y, 0);
        }

        private void UpdateScaling()
        {
            if (config == null)
            {
                return;
            }

            float normalizeDistance = GetNormalizeDistance();
            float scalingValue = ((config.MaxScaling - config.MinScaling) * normalizeDistance) + config.MinScaling;

            transform.localScale = new Vector3(scalingValue, scalingValue, 0f);
        }

        private void UpdateAlpha()
        {
            if (config == null)
            {
                return;
            }

            float normalizeDistance = GetNormalizeDistance();
            float alphaValue = ((config.MaxAlpha - config.MinAlpha) * normalizeDistance) + config.MinAlpha;

            Color targetColor = Color.black;
            targetColor.a = alphaValue;
            spriteRenderer.color = targetColor;
        }

        private float GetNormalizeDistance()
        {
            if (reference == null || config == null)
            {
                return 1f;
            }

            float distance = reference.transform.position.y - transform.position.y;
            float interestDistance = config.MaxInterestDistance - config.MinInterestDistance;
            float normalize = distance / interestDistance;

            normalize = Mathf.Clamp(normalize, 0, 1);

            return 1f - normalize;
        }

        public class Pool : MonoMemoryPool<Transform, Vector3, Shadow>
        {
            protected override void Reinitialize(Transform reference, Vector3 position, Shadow item)
            {
                item.Reinitialize(reference, position);
            }
        }
    }
}