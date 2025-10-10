using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    public string nextSceneName; // Name of the scene to load

    void Awake()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned in the Inspector.");
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}