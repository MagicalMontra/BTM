using System;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    public class ParallaxElement : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = -1;

        [SerializeField]
        private MeshRenderer renderer;
        
        private bool isMoving;
        private Vector2 offset;

        private const string mainTextName = "_MainTex";
        private readonly int mainTexIndex = Shader.PropertyToID(mainTextName);
        
        public void Move()
        {
            isMoving = true;
        }
        
        public void Stop()
        {
            isMoving = false;
        }

        private void Update()
        {
            if (!isMoving)
                return;

            offset.x += Time.deltaTime * moveSpeed * 0.1f;
            renderer.material.SetTextureOffset(mainTexIndex, offset);
        }
    }
}
