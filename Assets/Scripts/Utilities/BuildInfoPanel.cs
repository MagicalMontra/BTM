using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.RedAndBlue.UI
{
    [Serializable]
    public struct BuildInfoData
    {
        public string buildNumber;
        public string commitHash;
    }

    public class BuildInfoPanel : MonoBehaviour
    {
        private const string filePath = "BuildInfo/BuildInfo";

        private const string copiedMessage = "Copied to clipboard";

        private const float animateDuration = 1.0f;

        [SerializeField]
        private TextMeshProUGUI messageLabel;

        [SerializeField]
        private Button copyButton;

        private BuildInfoData buildInfo;

        private string buildInfoStr;

        private Coroutine animateCopyRoutine;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
#if !DEBUG_BUILD
            gameObject.SetActive(false);
#endif
            ReadBuildInfo();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"version: {Application.version}");
            builder.AppendLine($"build_number: {buildInfo.buildNumber}");
            builder.Append($"commit: {buildInfo.commitHash}");

            buildInfoStr = builder.ToString();
            messageLabel.text = buildInfoStr;

#if PECAN_NAVIGATOR
            copyButton.interactable = false;
#endif
            copyButton.onClick.AddListener(CopyToClipBoard);
        }

        private void ReadBuildInfo()
        {
            TextAsset buildInfoAsset = Resources.Load<TextAsset>(filePath);

            if (buildInfoAsset != null)
            {
                buildInfo = JsonUtility.FromJson<BuildInfoData>(buildInfoAsset.text);
            }
        }

        private void CopyToClipBoard()
        {
            if (animateCopyRoutine != null)
            {
                StopCoroutine(animateCopyRoutine);
            }

            animateCopyRoutine = StartCoroutine(AnimateCopy());
            GUIUtility.systemCopyBuffer = buildInfoStr;
        }

        private IEnumerator AnimateCopy()
        {
            messageLabel.text = copiedMessage;
            yield return new WaitForSecondsRealtime(animateDuration);

            messageLabel.text = buildInfoStr;
        }
    }
}
