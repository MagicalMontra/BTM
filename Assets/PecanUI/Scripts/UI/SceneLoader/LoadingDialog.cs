using UnityEngine;

namespace HotPlay.PecanUI.SceneLoader
{
    public class LoadingDialog : BaseDialog
    {
        public bool isClosed { get; private set; }
        public bool isReadied { get; private set; }

        public LoadingProgressor Progressor => progressor;
        
        private LoadingProgressor progressor;
        
        [SerializeField]
        private Transform gameTitleHolder;

        [SerializeField]
        private Transform progressorHolder;

            private void Start()
        {
            var gameTitlePrefab = PecanServices.Instance.Configs.GameTitlePrefab;

            if (gameTitlePrefab != null && gameTitleHolder != null)
            {
                Instantiate(gameTitlePrefab, gameTitleHolder);
            }

            var progressorPrefab = PecanServices.Instance.Configs.SceneLoadAnimation;

            if (progressorPrefab != null && progressorHolder != null)
            {
                progressor = Instantiate(progressorPrefab, progressorHolder);
            }
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            progressor.SetProgress(0f);
            isReadied = false;
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            isReadied = true;
        }

        protected override void OnHide()
        {
            base.OnHide();
            isClosed = false;
        }
        
        protected override void OnHidden()
        {
            base.OnHide();
            isClosed = true;
        }
    }
}