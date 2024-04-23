#if UNITY_EDITOR
using MoreMountains.Feedbacks;
using UnityEngine;

public class DebugPlayFeedback : MonoBehaviour
{
    [SerializeField]
    private MMF_Player feedbackPlayer;

    [SerializeField]
    private KeyCode inputKey;

    private void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            if (feedbackPlayer != null && !feedbackPlayer.IsPlaying)
            {
                feedbackPlayer.PlayFeedbacks();
            }
        }
    }
}
#endif