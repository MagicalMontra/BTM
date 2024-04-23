using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotPlay.PecanUI.SceneLoader
{
    public class PecanSceneLoader : ISceneLoader
    {
        private float fakeLoadTime;
        private float fakeLoadTimeThreshold;
        private LoadingProgressor progressor;
        
        public PecanSceneLoader(float fakeLoadTime, float fakeLoadTimeThreshold, LoadingProgressor progressor)
        {
            this.fakeLoadTime = fakeLoadTime;
            this.fakeLoadTimeThreshold = fakeLoadTimeThreshold;
            this.progressor = progressor;
        }
        
        public async UniTask LoadUniTask(SceneReference scene)
        {
            var loadOperation = SceneManager.LoadSceneAsync(scene.BuildIndex);
            loadOperation.allowSceneActivation = false;
            var fakeTime = 0f;
            var elapsedTime = 0f;
            while (!loadOperation.isDone || fakeTime >= fakeLoadTime)
            {
                var progress = loadOperation.progress;
                    
                if (elapsedTime <= fakeLoadTimeThreshold && loadOperation.progress >= 0.9f)
                {
                    fakeTime += Time.deltaTime;
                    progress = Mathf.Clamp(fakeTime / fakeLoadTime, 0f, 1f);
                }
                    
                if (loadOperation.progress < 0.9f)
                    elapsedTime += Time.deltaTime;
                    
                progressor.SetProgress(progress);
                await UniTask.Yield();

                if (fakeTime >= fakeLoadTime)
                    break;
                    
                if (loadOperation.isDone)
                    break;
            }
            loadOperation.allowSceneActivation = true;
            await loadOperation;
        }
    }
}